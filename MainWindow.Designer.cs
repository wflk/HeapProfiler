﻿namespace HeapProfiler {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container();
            this.ToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.SelectExecutable = new System.Windows.Forms.Button();
            this.GroupExecutable = new System.Windows.Forms.GroupBox();
            this.ExecutableStatus = new System.Windows.Forms.Label();
            this.ExecutablePath = new System.Windows.Forms.TextBox();
            this.LaunchProcess = new System.Windows.Forms.Button();
            this.GroupSnapshots = new System.Windows.Forms.GroupBox();
            this.SaveSelection = new System.Windows.Forms.Button();
            this.DiffSelection = new System.Windows.Forms.Button();
            this.CaptureSnapshot = new System.Windows.Forms.Button();
            this.SnapshotList = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SymbolPathMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupExecutable.SuspendLayout();
            this.GroupSnapshots.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectExecutable
            // 
            this.SelectExecutable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectExecutable.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectExecutable.Location = new System.Drawing.Point(406, 16);
            this.SelectExecutable.Margin = new System.Windows.Forms.Padding(2);
            this.SelectExecutable.Name = "SelectExecutable";
            this.SelectExecutable.Size = new System.Drawing.Size(30, 20);
            this.SelectExecutable.TabIndex = 1;
            this.SelectExecutable.Text = "...";
            this.ToolTips.SetToolTip(this.SelectExecutable, "Select Executable");
            this.SelectExecutable.UseVisualStyleBackColor = true;
            this.SelectExecutable.Click += new System.EventHandler(this.SelectExecutable_Click);
            // 
            // GroupExecutable
            // 
            this.GroupExecutable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupExecutable.Controls.Add(this.ExecutableStatus);
            this.GroupExecutable.Controls.Add(this.SelectExecutable);
            this.GroupExecutable.Controls.Add(this.ExecutablePath);
            this.GroupExecutable.Controls.Add(this.LaunchProcess);
            this.GroupExecutable.Location = new System.Drawing.Point(8, 32);
            this.GroupExecutable.Margin = new System.Windows.Forms.Padding(2);
            this.GroupExecutable.Name = "GroupExecutable";
            this.GroupExecutable.Padding = new System.Windows.Forms.Padding(2);
            this.GroupExecutable.Size = new System.Drawing.Size(519, 58);
            this.GroupExecutable.TabIndex = 0;
            this.GroupExecutable.TabStop = false;
            this.GroupExecutable.Text = "Executable";
            // 
            // ExecutableStatus
            // 
            this.ExecutableStatus.AutoSize = true;
            this.ExecutableStatus.Location = new System.Drawing.Point(4, 37);
            this.ExecutableStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExecutableStatus.Name = "ExecutableStatus";
            this.ExecutableStatus.Size = new System.Drawing.Size(97, 13);
            this.ExecutableStatus.TabIndex = 3;
            this.ExecutableStatus.Text = "Status: Not Started";
            // 
            // ExecutablePath
            // 
            this.ExecutablePath.AllowDrop = true;
            this.ExecutablePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExecutablePath.Location = new System.Drawing.Point(4, 17);
            this.ExecutablePath.Margin = new System.Windows.Forms.Padding(2);
            this.ExecutablePath.Name = "ExecutablePath";
            this.ExecutablePath.Size = new System.Drawing.Size(398, 20);
            this.ExecutablePath.TabIndex = 0;
            this.ExecutablePath.TextChanged += new System.EventHandler(this.ExecutablePath_TextChanged);
            this.ExecutablePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.ExecutablePath_DragDrop);
            this.ExecutablePath.DragOver += new System.Windows.Forms.DragEventHandler(this.ExecutablePath_DragOver);
            // 
            // LaunchProcess
            // 
            this.LaunchProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LaunchProcess.Enabled = false;
            this.LaunchProcess.Location = new System.Drawing.Point(439, 16);
            this.LaunchProcess.Margin = new System.Windows.Forms.Padding(2);
            this.LaunchProcess.Name = "LaunchProcess";
            this.LaunchProcess.Size = new System.Drawing.Size(75, 20);
            this.LaunchProcess.TabIndex = 2;
            this.LaunchProcess.Text = "&Launch";
            this.LaunchProcess.UseVisualStyleBackColor = true;
            this.LaunchProcess.Click += new System.EventHandler(this.LaunchProcess_Click);
            // 
            // GroupSnapshots
            // 
            this.GroupSnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSnapshots.Controls.Add(this.SaveSelection);
            this.GroupSnapshots.Controls.Add(this.DiffSelection);
            this.GroupSnapshots.Controls.Add(this.CaptureSnapshot);
            this.GroupSnapshots.Controls.Add(this.SnapshotList);
            this.GroupSnapshots.Location = new System.Drawing.Point(8, 94);
            this.GroupSnapshots.Margin = new System.Windows.Forms.Padding(2);
            this.GroupSnapshots.Name = "GroupSnapshots";
            this.GroupSnapshots.Padding = new System.Windows.Forms.Padding(2);
            this.GroupSnapshots.Size = new System.Drawing.Size(519, 257);
            this.GroupSnapshots.TabIndex = 1;
            this.GroupSnapshots.TabStop = false;
            this.GroupSnapshots.Text = "Snapshots";
            // 
            // SaveSelection
            // 
            this.SaveSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSelection.Enabled = false;
            this.SaveSelection.Location = new System.Drawing.Point(415, 67);
            this.SaveSelection.Margin = new System.Windows.Forms.Padding(2);
            this.SaveSelection.Name = "SaveSelection";
            this.SaveSelection.Size = new System.Drawing.Size(100, 20);
            this.SaveSelection.TabIndex = 3;
            this.SaveSelection.Text = "&Save Selection";
            this.SaveSelection.UseVisualStyleBackColor = true;
            // 
            // DiffSelection
            // 
            this.DiffSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DiffSelection.Enabled = false;
            this.DiffSelection.Location = new System.Drawing.Point(415, 42);
            this.DiffSelection.Margin = new System.Windows.Forms.Padding(2);
            this.DiffSelection.Name = "DiffSelection";
            this.DiffSelection.Size = new System.Drawing.Size(100, 20);
            this.DiffSelection.TabIndex = 2;
            this.DiffSelection.Text = "&Diff Selection";
            this.DiffSelection.UseVisualStyleBackColor = true;
            this.DiffSelection.Click += new System.EventHandler(this.DiffSelection_Click);
            // 
            // CaptureSnapshot
            // 
            this.CaptureSnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureSnapshot.Enabled = false;
            this.CaptureSnapshot.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptureSnapshot.Location = new System.Drawing.Point(415, 17);
            this.CaptureSnapshot.Margin = new System.Windows.Forms.Padding(2);
            this.CaptureSnapshot.Name = "CaptureSnapshot";
            this.CaptureSnapshot.Size = new System.Drawing.Size(100, 20);
            this.CaptureSnapshot.TabIndex = 1;
            this.CaptureSnapshot.Text = "&Capture Now";
            this.CaptureSnapshot.UseVisualStyleBackColor = true;
            this.CaptureSnapshot.Click += new System.EventHandler(this.CaptureSnapshot_Click);
            // 
            // SnapshotList
            // 
            this.SnapshotList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SnapshotList.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SnapshotList.IntegralHeight = false;
            this.SnapshotList.ItemHeight = 17;
            this.SnapshotList.Location = new System.Drawing.Point(4, 17);
            this.SnapshotList.Margin = new System.Windows.Forms.Padding(2);
            this.SnapshotList.Name = "SnapshotList";
            this.SnapshotList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.SnapshotList.Size = new System.Drawing.Size(407, 236);
            this.SnapshotList.TabIndex = 0;
            this.SnapshotList.SelectedIndexChanged += new System.EventHandler(this.SnapshotList_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(534, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SymbolPathMenu});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // SymbolPathMenu
            // 
            this.SymbolPathMenu.Enabled = false;
            this.SymbolPathMenu.Name = "SymbolPathMenu";
            this.SymbolPathMenu.Size = new System.Drawing.Size(152, 22);
            this.SymbolPathMenu.Text = "&Symbol Path...";
            this.SymbolPathMenu.Click += new System.EventHandler(this.SymbolPathMenu_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 359);
            this.Controls.Add(this.GroupSnapshots);
            this.Controls.Add(this.GroupExecutable);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Heap Profiler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.GroupExecutable.ResumeLayout(false);
            this.GroupExecutable.PerformLayout();
            this.GroupSnapshots.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip ToolTips;
        private System.Windows.Forms.GroupBox GroupExecutable;
        private System.Windows.Forms.Button SelectExecutable;
        private System.Windows.Forms.TextBox ExecutablePath;
        private System.Windows.Forms.Button LaunchProcess;
        private System.Windows.Forms.Label ExecutableStatus;
        private System.Windows.Forms.GroupBox GroupSnapshots;
        private System.Windows.Forms.ListBox SnapshotList;
        private System.Windows.Forms.Button DiffSelection;
        private System.Windows.Forms.Button CaptureSnapshot;
        private System.Windows.Forms.Button SaveSelection;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SymbolPathMenu;
    }
}

