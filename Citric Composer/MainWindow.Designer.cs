namespace Citric_Composer
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Sequence Sounds", 3, 3);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Sequence Sound Sets", 4, 4);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Banks", 5, 5);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Stream Sounds", 1, 1);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Wave Sound Sets", 2, 2);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Waveform Archives", 6, 6);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Groups", 7, 7);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Players", 8, 8);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Misc. Information", 10, 10);
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("FILES", 11, 11);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.noInfoPanel = new System.Windows.Forms.Panel();
            this.noInfoLabel = new System.Windows.Forms.Label();
            this.tree = new System.Windows.Forms.TreeView();
            this.treeIcons = new System.Windows.Forms.ImageList(this.components);
            this.status = new System.Windows.Forms.StatusStrip();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.readyLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.noBytesSelectedText = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isabelleSoundEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brewstersArchiveBrewerWARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutCitricComposerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.noInfoPanel.SuspendLayout();
            this.status.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.noInfoPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tree);
            this.splitContainer1.Size = new System.Drawing.Size(850, 475);
            this.splitContainer1.SplitterDistance = 237;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 0;
            // 
            // noInfoPanel
            // 
            this.noInfoPanel.Controls.Add(this.noInfoLabel);
            this.noInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.noInfoPanel.Name = "noInfoPanel";
            this.noInfoPanel.Size = new System.Drawing.Size(235, 473);
            this.noInfoPanel.TabIndex = 0;
            // 
            // noInfoLabel
            // 
            this.noInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noInfoLabel.Location = new System.Drawing.Point(0, 0);
            this.noInfoLabel.Name = "noInfoLabel";
            this.noInfoLabel.Size = new System.Drawing.Size(235, 473);
            this.noInfoLabel.TabIndex = 0;
            this.noInfoLabel.Text = "No Valid Info Selected!";
            this.noInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeIcons;
            this.tree.Indent = 12;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            treeNode1.ImageIndex = 3;
            treeNode1.Name = "sequenceSounds";
            treeNode1.SelectedImageIndex = 3;
            treeNode1.Text = "Sequence Sounds";
            treeNode2.ImageIndex = 4;
            treeNode2.Name = "sequenceSoundSets";
            treeNode2.SelectedImageIndex = 4;
            treeNode2.Text = "Sequence Sound Sets";
            treeNode3.ImageIndex = 5;
            treeNode3.Name = "banks";
            treeNode3.SelectedImageIndex = 5;
            treeNode3.Text = "Banks";
            treeNode4.ImageIndex = 1;
            treeNode4.Name = "streamSounds";
            treeNode4.SelectedImageIndex = 1;
            treeNode4.Text = "Stream Sounds";
            treeNode5.ImageIndex = 2;
            treeNode5.Name = "waveSoundSets";
            treeNode5.SelectedImageIndex = 2;
            treeNode5.Text = "Wave Sound Sets";
            treeNode6.ImageIndex = 6;
            treeNode6.Name = "waveformArchives";
            treeNode6.SelectedImageIndex = 6;
            treeNode6.Text = "Waveform Archives";
            treeNode7.ImageIndex = 7;
            treeNode7.Name = "groups";
            treeNode7.SelectedImageIndex = 7;
            treeNode7.Text = "Groups";
            treeNode8.ImageIndex = 8;
            treeNode8.Name = "players";
            treeNode8.SelectedImageIndex = 8;
            treeNode8.Text = "Players";
            treeNode9.ImageIndex = 10;
            treeNode9.Name = "miscInfo";
            treeNode9.SelectedImageIndex = 10;
            treeNode9.Text = "Misc. Information";
            treeNode10.ImageIndex = 11;
            treeNode10.Name = "files";
            treeNode10.SelectedImageIndex = 11;
            treeNode10.Text = "FILES";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            this.tree.SelectedImageIndex = 0;
            this.tree.ShowLines = false;
            this.tree.Size = new System.Drawing.Size(609, 473);
            this.tree.TabIndex = 0;
            // 
            // treeIcons
            // 
            this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
            this.treeIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.treeIcons.Images.SetKeyName(0, "blank.png");
            this.treeIcons.Images.SetKeyName(1, "strm.png");
            this.treeIcons.Images.SetKeyName(2, "wave.png");
            this.treeIcons.Images.SetKeyName(3, "sseq.png");
            this.treeIcons.Images.SetKeyName(4, "seqArc.png");
            this.treeIcons.Images.SetKeyName(5, "bank.png");
            this.treeIcons.Images.SetKeyName(6, "waveArchive.png");
            this.treeIcons.Images.SetKeyName(7, "group.png");
            this.treeIcons.Images.SetKeyName(8, "player.png");
            this.treeIcons.Images.SetKeyName(9, "lookup.png");
            this.treeIcons.Images.SetKeyName(10, "record.png");
            this.treeIcons.Images.SetKeyName(11, "FILES.png");
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress,
            this.readyLabel,
            this.noBytesSelectedText});
            this.status.Location = new System.Drawing.Point(0, 499);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(850, 22);
            this.status.TabIndex = 2;
            this.status.Text = "statusStrip1";
            // 
            // progress
            // 
            this.progress.MarqueeAnimationSpeed = 1;
            this.progress.Maximum = 5;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(100, 16);
            this.progress.Step = 1;
            this.progress.Value = 5;
            // 
            // readyLabel
            // 
            this.readyLabel.Name = "readyLabel";
            this.readyLabel.Size = new System.Drawing.Size(42, 17);
            this.readyLabel.Text = "Ready!";
            // 
            // noBytesSelectedText
            // 
            this.noBytesSelectedText.Name = "noBytesSelectedText";
            this.noBytesSelectedText.Size = new System.Drawing.Size(103, 17);
            this.noBytesSelectedText.Text = "No bytes selected!";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(850, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeFileToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // closeFileToolStripMenuItem
            // 
            this.closeFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeFileToolStripMenuItem.Image")));
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.closeFileToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.closeFileToolStripMenuItem.Text = "Close File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("quitToolStripMenuItem.Image")));
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToFolderToolStripMenuItem,
            this.importFromFolderToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // exportToFolderToolStripMenuItem
            // 
            this.exportToFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToFolderToolStripMenuItem.Image")));
            this.exportToFolderToolStripMenuItem.Name = "exportToFolderToolStripMenuItem";
            this.exportToFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToFolderToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.exportToFolderToolStripMenuItem.Text = "Export To Folder";
            // 
            // importFromFolderToolStripMenuItem
            // 
            this.importFromFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importFromFolderToolStripMenuItem.Image")));
            this.importFromFolderToolStripMenuItem.Name = "importFromFolderToolStripMenuItem";
            this.importFromFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importFromFolderToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.importFromFolderToolStripMenuItem.Text = "Import From Folder";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isabelleSoundEditorToolStripMenuItem,
            this.brewstersArchiveBrewerWARToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // isabelleSoundEditorToolStripMenuItem
            // 
            this.isabelleSoundEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("isabelleSoundEditorToolStripMenuItem.Image")));
            this.isabelleSoundEditorToolStripMenuItem.Name = "isabelleSoundEditorToolStripMenuItem";
            this.isabelleSoundEditorToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.isabelleSoundEditorToolStripMenuItem.Text = "Isabelle Sound Editor (WAV, STM)";
            this.isabelleSoundEditorToolStripMenuItem.Click += new System.EventHandler(this.isabelleSoundEditorToolStripMenuItem_Click);
            // 
            // brewstersArchiveBrewerWARToolStripMenuItem
            // 
            this.brewstersArchiveBrewerWARToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("brewstersArchiveBrewerWARToolStripMenuItem.Image")));
            this.brewstersArchiveBrewerWARToolStripMenuItem.Name = "brewstersArchiveBrewerWARToolStripMenuItem";
            this.brewstersArchiveBrewerWARToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.brewstersArchiveBrewerWARToolStripMenuItem.Text = "Brewster\'s Archive Brewer (WAR)";
            this.brewstersArchiveBrewerWARToolStripMenuItem.Click += new System.EventHandler(this.brewstersArchiveBrewerWARToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // getHelpToolStripMenuItem
            // 
            this.getHelpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("getHelpToolStripMenuItem.Image")));
            this.getHelpToolStripMenuItem.Name = "getHelpToolStripMenuItem";
            this.getHelpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.getHelpToolStripMenuItem.Text = "View Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutCitricComposerToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutCitricComposerToolStripMenuItem
            // 
            this.aboutCitricComposerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutCitricComposerToolStripMenuItem.Image")));
            this.aboutCitricComposerToolStripMenuItem.Name = "aboutCitricComposerToolStripMenuItem";
            this.aboutCitricComposerToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.aboutCitricComposerToolStripMenuItem.Text = "About Citric Composer";
            this.aboutCitricComposerToolStripMenuItem.Click += new System.EventHandler(this.aboutCitricComposerToolStripMenuItem_Click);
            // 
            // nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem
            // 
            this.nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem.Name = "nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem";
            this.nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem.Size = new System.Drawing.Size(258, 20);
            this.nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem.Text = "NOTE: ONLY TOOLS WORK ATM, BUT ENJOY!";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 521);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Citric Composer - TOOL DEMO";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.noInfoPanel.ResumeLayout(false);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel noInfoPanel;
        private System.Windows.Forms.Label noInfoLabel;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutCitricComposerToolStripMenuItem;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ToolStripProgressBar progress;
        private System.Windows.Forms.ToolStripStatusLabel readyLabel;
        private System.Windows.Forms.ToolStripStatusLabel noBytesSelectedText;
        private System.Windows.Forms.ImageList treeIcons;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isabelleSoundEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brewstersArchiveBrewerWARToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nOTEONLYTOOLSWORKATMBUTENJOYToolStripMenuItem;
    }
}

