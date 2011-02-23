﻿/*
The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is Windows Heap Profiler Frontend.

The Initial Developer of the Original Code is Mozilla Corporation.

Original Author: Kevin Gadd (kevin.gadd@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Squared.Task;
using System.Diagnostics;
using System.IO;
using Squared.Task.IO;
using System.Text.RegularExpressions;
using Squared.Util.RegexExtensions;
using System.Globalization;

using Snapshot = HeapProfiler.RunningProcess.Snapshot;
using Squared.Util;

namespace HeapProfiler {
    public partial class DiffViewer : TaskForm {
        const int ProgressInterval = 50;

        public static Regex ModuleRegex = new Regex(
            @"DBGHELP: (?'module'.*?)( - )(?'symboltype'[^\n\r]*)", 
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
        );
        public static Regex BytesDeltaRegex = new Regex(
            @"(?'type'\+|\-)(\s+)(?'delta_bytes'[\da-fA-F]+)(\s+)\((\s*)(?'old_bytes'[\da-fA-F]*)(\s*)-(\s*)(?'new_bytes'[\da-fA-F]*)\)(\s*)(?'new_count'[\da-fA-F]+) allocs\t(BackTrace(?'trace_id'\w*))",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
        );
        public static Regex CountDeltaRegex = new Regex(
            @"(?'type'\+|\-)(\s+)(?'delta'[\da-fA-F]+)(\s+)\((\s*)(?'old_count'[\da-fA-F]*)(\s*)-(\s*)(?'new_count'[\da-fA-F]*)\)\t(BackTrace(?'trace_id'\w*))\tallocations",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
        );
        public static Regex TracebackRegex = new Regex(
            @"\t(?'module'[^!]+)!(?'function'[^+]+)\+(?'offset'[\dA-Fa-f]+)(\s*:\s*(?'offset2'[\dA-Fa-f]+))?(\s*\(\s*(?'path'[^,]+),\s*(?'line'[0-9]*)\))?",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
        );

        public Dictionary<string, ModuleInfo> Modules = new Dictionary<string, ModuleInfo>();
        public HashSet<string> FunctionNames = new HashSet<string>();
        public List<DeltaInfo> Deltas = new List<DeltaInfo>();
        public Dictionary<string, TracebackInfo> Tracebacks = new Dictionary<string, TracebackInfo>();

        public List<DeltaInfo> ListItems = new List<DeltaInfo>();

        protected IFuture PendingLoad = null;
        protected Pair<int> PendingLoadPair = new Pair<int>(-1, -1);
        protected RunningProcess Instance = null;

        protected Pair<int> CurrentPair = new Pair<int>(-1, -1);
        protected string Filename;
        protected string FunctionFilter = null;
        protected StringFormat DeltaListFormat;
        protected bool Updating = false;

        public DiffViewer (TaskScheduler scheduler, RunningProcess instance)
            : base (scheduler) {
            InitializeComponent();

            DeltaListFormat = new StringFormat();
            DeltaListFormat.Trimming = StringTrimming.None;
            DeltaListFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;

            Instance = instance;
            if (Instance != null) {
                Timeline.Items = Instance.Snapshots;
            } else {
                Timeline.Visible = false;
                MainSplit.Height += Timeline.Bottom - MainSplit.Bottom;
            }
        }

        public DiffViewer (TaskScheduler scheduler)
            : this(scheduler, null) {
        }

        protected void SetBusy (bool busy) {
            UseWaitCursor = Updating = busy;
        }

        public IEnumerator<object> LoadRange (Pair<int> range) {
            PendingLoadPair = range;

            LoadingPanel.Text = "Generating diff...";
            LoadingProgress.Value = 0;
            LoadingProgress.Style = ProgressBarStyle.Marquee;

            MainMenuStrip.Enabled = false;
            LoadingPanel.Visible = true;
            MainSplit.Visible = false;
            UseWaitCursor = true;

            var s1 = Instance.Snapshots[range.First].Filename;
            var s2 = Instance.Snapshots[range.Second].Filename;

            Timeline.Indices = range;

            var f = Start(Instance.DiffSnapshots(s1, s2));
            using (f)
                yield return f;

            var filename = f.Result as string;
            f = Start(LoadDiff(filename));
            using (f)
                yield return f;

            PendingLoadPair = Pair.New(-1, -1);
            CurrentPair = range;
        }

        public IEnumerator<object> LoadDiff (string filename) {
            LoadingPanel.Text = "Loading diff...";

            var fLines = Future.RunInThread(() => File.ReadAllLines(filename));
            yield return fLines;

            var lines = fLines.Result;

            LoadingPanel.Text = "Parsing diff...";

            var modules = new Dictionary<string, ModuleInfo>();
            var functionNames = new HashSet<string>();
            var deltas = new List<DeltaInfo>();
            var tracebacks = new Dictionary<string, TracebackInfo>();

            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i];

                try {
                    LoadingProgress.Style = ProgressBarStyle.Continuous;
                    LoadingProgress.Maximum = lines.Length;
                } catch {
                }

                if ((i % ProgressInterval == 0) && (LoadingProgress.Style == ProgressBarStyle.Continuous)) {
                    // Setting the progress higher and then lower bypasses the slow animation baked into
                    //  the windows theme engine's progress bar implementation
                    LoadingProgress.Value = Math.Min(i + 1, LoadingProgress.Maximum);
                    LoadingProgress.Value = i;

                    // Suspend processing until any messages in the windows message queue have been processed
                    yield return new Yield();
                }

            retryFromHere:

                Match m;
                if (ModuleRegex.TryMatch(line, out m)) {
                    var moduleName = String.Intern(m.Groups["module"].Value);

                    var info = new ModuleInfo {
                        ModuleName = moduleName,
                        SymbolType = String.Intern(m.Groups["symboltype"].Value),
                    };

                    if (i < lines.Length - 1) {
                        line = lines[++i];
                        if (!ModuleRegex.IsMatch(line)) {
                            info.SymbolPath = line.Trim();
                        } else {
                            goto retryFromHere;
                        }
                    }

                    modules[moduleName] = info;
                } else if (BytesDeltaRegex.TryMatch(line, out m)) {
                    var traceId = String.Intern(m.Groups["trace_id"].Value);
                    var info = new DeltaInfo {
                        Added = (m.Groups["type"].Value == "+"),
                        BytesDelta = int.Parse(m.Groups["delta_bytes"].Value),
                        NewBytes = int.Parse(m.Groups["new_bytes"].Value),
                        OldBytes = int.Parse(m.Groups["old_bytes"].Value),
                        NewCount = int.Parse(m.Groups["new_count"].Value),
                    };

                    if (i < lines.Length - 1) {
                        line = lines[++i];

                        if (CountDeltaRegex.TryMatch(line, out m)) {
                            info.OldCount = int.Parse(m.Groups["old_count"].Value);
                            info.CountDelta = int.Parse(m.Groups["delta"].Value);
                        }
                    }

                    bool readingLeadingWhitespace = true;

                    var frames = new List<TracebackFrame>();
                    var itemModules = new HashSet<string>();
                    var itemFunctions = new HashSet<string>();

                    while (i++ < lines.Length) {
                        line = lines[i];

                        if (line.Trim().Length == 0) {
                            if (readingLeadingWhitespace)
                                continue;
                            else
                                break;
                        } else if (TracebackRegex.TryMatch(line, out m)) {
                            readingLeadingWhitespace = false;

                            var moduleName = String.Intern(m.Groups["module"].Value);
                            itemModules.Add(moduleName);

                            var functionName = String.Intern(m.Groups["function"].Value);
                            itemFunctions.Add(functionName);
                            functionNames.Add(functionName);

                            if (!modules.ContainsKey(moduleName)) {
                                modules[moduleName] = new ModuleInfo {
                                    ModuleName = moduleName,
                                    SymbolType = "Unknown",
                                    References = 1
                                };
                            } else {
                                modules[moduleName].References += 1;
                            }

                            var frame = new TracebackFrame {
                                Module = moduleName,
                                Function = functionName,
                                Offset = UInt32.Parse(m.Groups["offset"].Value, NumberStyles.HexNumber)
                            };
                            if (m.Groups["offset2"].Success)
                                frame.Offset2 = UInt32.Parse(m.Groups["offset2"].Value, NumberStyles.HexNumber);

                            if (m.Groups["path"].Success)
                                frame.SourceFile = m.Groups["path"].Value;

                            if (m.Groups["line"].Success)
                                frame.SourceLine = int.Parse(m.Groups["line"].Value);

                            frames.Add(frame);
                        } else {
                            i--;
                            break;
                        }
                    }

                    if (tracebacks.ContainsKey(traceId)) {
                        info.Traceback = tracebacks[traceId];
                        Console.WriteLine("Duplicate traceback for id {0}!", traceId);
                    } else {
                        info.Traceback = tracebacks[traceId] = new TracebackInfo {
                            TraceId = traceId,
                            Frames = frames.ToArray(),
                            Modules = itemModules,
                            Functions = itemFunctions
                        };
                    }

                    deltas.Add(info);
                } else {
                    // Console.WriteLine(line);
                }
            }

            foreach (var key in modules.Keys.ToArray()) {
                if (modules[key].References == 0)
                    modules.Remove(key);
            }

            Modules = modules;
            FunctionNames = functionNames;
            Deltas = deltas;
            Tracebacks = tracebacks;

            TracebackFilter.AutoCompleteCustomSource.Clear();
            TracebackFilter.AutoCompleteCustomSource.AddRange(functionNames.ToArray());

            Text = "Diff Viewer - " + filename;
            Filename = filename;

            RefreshModules();
            RefreshDeltas();

            MainMenuStrip.Enabled = true;
            LoadingPanel.Visible = false;
            MainSplit.Visible = true;
            Timeline.Enabled = true;
            UseWaitCursor = false;
        }

        public void RefreshModules () {
            if (Updating)
                return;

            SetBusy(true);

            ModuleList.BeginUpdate();
            ModuleList.Items.Clear();
            foreach (var key in Modules.Keys.OrderBy((s) => s))
                ModuleList.Items.Add(Modules[key]);
            for (int i = 0; i < ModuleList.Items.Count; i++)
                ModuleList.SetItemChecked(i, true);
            ModuleList.EndUpdate();

            SetBusy(false);
        }

        public void RefreshDeltas () {
            if (Updating)
                return;

            SetBusy(true);

            int max = -int.MaxValue;
            int totalBytes = 0, totalAllocs = 0;

            ListItems.Clear();
            foreach (var delta in Deltas) {
                if (FunctionFilter != null) {
                    if (!delta.Traceback.Functions.Contains(FunctionFilter))
                        continue;
                }

                bool filteredOut = (delta.Traceback.Modules.Count > 0);
                foreach (var module in delta.Traceback.Modules) {
                    filteredOut &= Modules[module].Filtered;

                    if (!filteredOut)
                        break;
                }

                if (!filteredOut) {
                    ListItems.Add(delta);
                    totalBytes += delta.BytesDelta * (delta.Added ? 1 : -1);
                    totalAllocs += delta.CountDelta.GetValueOrDefault(0) * (delta.Added ? 1 : -1);
                    max = Math.Max(max, delta.BytesDelta);
                }
            }

            StatusLabel.Text = String.Format("Showing {0} out of {1} item(s)", ListItems.Count, Deltas.Count);
            AllocationTotals.Text = String.Format("Delta bytes: {0} Delta allocations: {1}", totalBytes, totalAllocs);

            DeltaHistogram.Items = DeltaList.Items = ListItems;
            if (ListItems.Count > 0)
                DeltaHistogram.Maximum = max;
            else
                DeltaHistogram.Maximum = 1024;

            DeltaList.Invalidate();
            DeltaHistogram.Invalidate();

            SetBusy(false);
        }

        private void DiffViewer_Shown (object sender, EventArgs e) {
            UseWaitCursor = true;
        }

        private void DiffViewer_FormClosed (object sender, FormClosedEventArgs e) {
            Dispose();
        }

        private void ModuleList_ItemCheck (object sender, ItemCheckEventArgs e) {
            var m = (ModuleInfo)ModuleList.Items[e.Index];
            m.Filtered = (e.NewValue == CheckState.Unchecked);
            RefreshDeltas();
        }

        private void SelectAllModules_Click (object sender, EventArgs e) {
            SetBusy(true);
            ModuleList.BeginUpdate();

            for (int i = 0; i < ModuleList.Items.Count; i++)
                ModuleList.SetItemChecked(i, true);

            ModuleList.EndUpdate();
            SetBusy(false);

            RefreshDeltas();
        }

        private void SelectNoModules_Click (object sender, EventArgs e) {
            SetBusy(true);
            ModuleList.BeginUpdate();

            for (int i = 0; i < ModuleList.Items.Count; i++)
                ModuleList.SetItemChecked(i, false);

            ModuleList.EndUpdate();
            SetBusy(false);

            RefreshDeltas();
        }

        private void InvertModuleSelection_Click (object sender, EventArgs e) {
            SetBusy(true);
            ModuleList.BeginUpdate();

            for (int i = 0; i < ModuleList.Items.Count; i++)
                ModuleList.SetItemChecked(i, !ModuleList.GetItemChecked(i));

            ModuleList.EndUpdate();
            SetBusy(false);

            RefreshDeltas();
        }

        private void SaveDiffMenu_Click (object sender, EventArgs e) {
            using (var dialog = new SaveFileDialog()) {
                dialog.Title = "Save Diff";
                dialog.Filter = "Heap Diffs|*.heapdiff";
                dialog.AddExtension = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = ".heapdiff";

                if (dialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;

                File.Copy(Filename, dialog.FileName, true);
                Filename = dialog.FileName;
                Text = "Diff Viewer - " + Filename;
            }
        }

        private void CloseMenu_Click (object sender, EventArgs e) {
            Close();
        }

        private void TracebackFilter_TextChanged (object sender, EventArgs e) {
            string newFilter = null;
            if (FunctionNames.Contains(TracebackFilter.Text))
                newFilter = String.Intern(TracebackFilter.Text);

            var newColor =
                (TracebackFilter.Text.Length > 0) ?
                    ((newFilter == null) ?
                        Color.LightPink : Color.LightGoldenrodYellow)
                    : SystemColors.Window;

            if (newColor != TracebackFilter.BackColor)
                TracebackFilter.BackColor = newColor;

            if (newFilter != FunctionFilter) {
                DeltaHistogram.FunctionFilter = DeltaList.FunctionFilter = FunctionFilter = newFilter;
                RefreshDeltas();
            }
        }

        private void ViewListMenu_Click (object sender, EventArgs e) {
            DeltaHistogram.Visible = ViewHistogramMenu.Checked = false;
            DeltaList.Visible = ViewListMenu.Checked = true;
        }

        private void ViewHistogramMenu_Click (object sender, EventArgs e) {
            DeltaList.Visible = ViewListMenu.Checked = false;
            DeltaHistogram.Visible = ViewHistogramMenu.Checked = true;
        }

        private void Timeline_RangeChanged (object sender, EventArgs e) {
            var pair = Timeline.Indices;

            if (pair.CompareTo(PendingLoadPair) != 0) {
                if (PendingLoad != null) {
                    PendingLoad.Dispose();
                    PendingLoad = null;
                    PendingLoadPair = Pair.New(-1, -1);
                }
            } else {
                return;
            }

            if (pair.CompareTo(CurrentPair) != 0)
                PendingLoad = Start(LoadRange(pair));
        }
    }
}
