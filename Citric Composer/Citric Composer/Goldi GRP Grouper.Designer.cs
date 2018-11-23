namespace Citric_Composer
{
    partial class Goldi_GRP_Grouper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Goldi_GRP_Grouper));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Internal Files", 6, 6);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Sound Archive Files", 7, 7);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Dependency Loader", 8, 8);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutBrewsterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.noInfoPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.playerPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.playButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.tree = new System.Windows.Forms.TreeView();
            this.treeIcons = new System.Windows.Forms.ImageList(this.components);
            this.grpOpen = new System.Windows.Forms.OpenFileDialog();
            this.rootMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byteStrip = new System.Windows.Forms.StatusStrip();
            this.bytesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpSave = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.folderOpen = new System.Windows.Forms.FolderBrowserDialog();
            this.soundOpen = new System.Windows.Forms.OpenFileDialog();
            this.soundSave = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.noInfoPanel.SuspendLayout();
            this.playerPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            this.rootMenu.SuspendLayout();
            this.nodeMenu.SuspendLayout();
            this.byteStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(809, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("quitToolStripMenuItem.Image")));
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFromFolderToolStripMenuItem,
            this.exportToFolderToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // importFromFolderToolStripMenuItem
            // 
            this.importFromFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importFromFolderToolStripMenuItem.Image")));
            this.importFromFolderToolStripMenuItem.Name = "importFromFolderToolStripMenuItem";
            this.importFromFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importFromFolderToolStripMenuItem.Text = "Import From Folder";
            this.importFromFolderToolStripMenuItem.Click += new System.EventHandler(this.importFromFolderToolStripMenuItem_Click);
            // 
            // exportToFolderToolStripMenuItem
            // 
            this.exportToFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToFolderToolStripMenuItem.Image")));
            this.exportToFolderToolStripMenuItem.Name = "exportToFolderToolStripMenuItem";
            this.exportToFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportToFolderToolStripMenuItem.Text = "Export To Folder";
            this.exportToFolderToolStripMenuItem.Click += new System.EventHandler(this.exportToFolderToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutBrewsterToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutBrewsterToolStripMenuItem
            // 
            this.aboutBrewsterToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutBrewsterToolStripMenuItem.Image")));
            this.aboutBrewsterToolStripMenuItem.Name = "aboutBrewsterToolStripMenuItem";
            this.aboutBrewsterToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.aboutBrewsterToolStripMenuItem.Text = "About Goldi\'s Grouper";
            this.aboutBrewsterToolStripMenuItem.Click += new System.EventHandler(this.aboutBrewsterToolStripMenuItem_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.playerPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.volumeBar);
            this.splitContainer1.Panel2.Controls.Add(this.tree);
            this.splitContainer1.Size = new System.Drawing.Size(809, 415);
            this.splitContainer1.SplitterDistance = 269;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // noInfoPanel
            // 
            this.noInfoPanel.Controls.Add(this.label1);
            this.noInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.noInfoPanel.Name = "noInfoPanel";
            this.noInfoPanel.Size = new System.Drawing.Size(267, 413);
            this.noInfoPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 413);
            this.label1.TabIndex = 0;
            this.label1.Text = "No Valid Info Selected!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerPanel
            // 
            this.playerPanel.Controls.Add(this.tableLayoutPanel1);
            this.playerPanel.Controls.Add(this.stopButton);
            this.playerPanel.Controls.Add(this.label2);
            this.playerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerPanel.Location = new System.Drawing.Point(0, 0);
            this.playerPanel.Name = "playerPanel";
            this.playerPanel.Size = new System.Drawing.Size(267, 413);
            this.playerPanel.TabIndex = 1;
            this.playerPanel.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.playButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pauseButton, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(261, 30);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.Location = new System.Drawing.Point(3, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(124, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseButton.Location = new System.Drawing.Point(133, 3);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(125, 23);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(6, 61);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(255, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sound Player Deluxe™";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(409, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Volume:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label3.Visible = false;
            // 
            // volumeBar
            // 
            this.volumeBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.volumeBar.Location = new System.Drawing.Point(405, 32);
            this.volumeBar.Maximum = 100;
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(104, 45);
            this.volumeBar.TabIndex = 1;
            this.volumeBar.TickFrequency = 10;
            this.volumeBar.Value = 50;
            this.volumeBar.Visible = false;
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeIcons;
            this.tree.Indent = 14;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            treeNode1.ImageIndex = 6;
            treeNode1.Name = "internalFiles";
            treeNode1.SelectedImageIndex = 6;
            treeNode1.Text = "Internal Files";
            treeNode2.ImageIndex = 7;
            treeNode2.Name = "soundArchiveFiles";
            treeNode2.SelectedImageIndex = 7;
            treeNode2.Text = "Sound Archive Files";
            treeNode3.ImageIndex = 8;
            treeNode3.Name = "dependencyLoader";
            treeNode3.SelectedImageIndex = 8;
            treeNode3.Text = "Dependency Loader";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.tree.SelectedImageIndex = 0;
            this.tree.ShowLines = false;
            this.tree.Size = new System.Drawing.Size(536, 413);
            this.tree.TabIndex = 0;
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseClick);
            this.tree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tree_NodeKey);
            // 
            // treeIcons
            // 
            this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
            this.treeIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.treeIcons.Images.SetKeyName(0, "blank.png");
            this.treeIcons.Images.SetKeyName(1, "sseq.png");
            this.treeIcons.Images.SetKeyName(2, "seqArc.png");
            this.treeIcons.Images.SetKeyName(3, "bank.png");
            this.treeIcons.Images.SetKeyName(4, "waveArchive.png");
            this.treeIcons.Images.SetKeyName(5, "wave.png");
            this.treeIcons.Images.SetKeyName(6, "FILES.png");
            this.treeIcons.Images.SetKeyName(7, "recordArc.png");
            this.treeIcons.Images.SetKeyName(8, "group.png");
            // 
            // grpOpen
            // 
            this.grpOpen.Filter = "Group|*bfwar;*bcwar";
            this.grpOpen.RestoreDirectory = true;
            // 
            // rootMenu
            // 
            this.rootMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.collapseToolStripMenuItem});
            this.rootMenu.Name = "rootMenu";
            this.rootMenu.Size = new System.Drawing.Size(120, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addToolStripMenuItem.Image")));
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("expandToolStripMenuItem.Image")));
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.expandToolStripMenuItem.Text = "Expand";
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("collapseToolStripMenuItem.Image")));
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.collapseToolStripMenuItem.Text = "Collapse";
            // 
            // nodeMenu
            // 
            this.nodeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAboveToolStripMenuItem,
            this.addBelowToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.nodeMenu.Name = "nodeMenu";
            this.nodeMenu.Size = new System.Drawing.Size(139, 158);
            // 
            // addAboveToolStripMenuItem
            // 
            this.addAboveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addAboveToolStripMenuItem.Image")));
            this.addAboveToolStripMenuItem.Name = "addAboveToolStripMenuItem";
            this.addAboveToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addAboveToolStripMenuItem.Text = "Add Above";
            // 
            // addBelowToolStripMenuItem
            // 
            this.addBelowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addBelowToolStripMenuItem.Image")));
            this.addBelowToolStripMenuItem.Name = "addBelowToolStripMenuItem";
            this.addBelowToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addBelowToolStripMenuItem.Text = "Add Below";
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveUpToolStripMenuItem.Image")));
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveDownToolStripMenuItem.Image")));
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("replaceToolStripMenuItem.Image")));
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // byteStrip
            // 
            this.byteStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bytesLabel});
            this.byteStrip.Location = new System.Drawing.Point(0, 439);
            this.byteStrip.Name = "byteStrip";
            this.byteStrip.Size = new System.Drawing.Size(809, 22);
            this.byteStrip.TabIndex = 4;
            this.byteStrip.Text = "statusStrip1";
            // 
            // bytesLabel
            // 
            this.bytesLabel.Name = "bytesLabel";
            this.bytesLabel.Size = new System.Drawing.Size(104, 17);
            this.bytesLabel.Text = "No Bytes Selected!";
            // 
            // grpSave
            // 
            this.grpSave.Filter = "Cafe Group|*.bfgrp|Citra Group|*.bcgrp";
            this.grpSave.RestoreDirectory = true;
            // 
            // folderOpen
            // 
            this.folderOpen.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // soundOpen
            // 
            this.soundOpen.Filter = "Cafe Wave|*.bfwav|Citra Wave|*.bcwav|Sound Wave|*.wav";
            this.soundOpen.RestoreDirectory = true;
            // 
            // soundSave
            // 
            this.soundSave.Filter = "Cafe Wave|*.bfwav|Citra Wave|*.bcwav|Sound Wave|*.wav";
            this.soundSave.RestoreDirectory = true;
            // 
            // Goldi_GRP_Grouper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 461);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.byteStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Goldi_GRP_Grouper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Goldi\'s Grouper";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.noInfoPanel.ResumeLayout(false);
            this.playerPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).EndInit();
            this.rootMenu.ResumeLayout(false);
            this.nodeMenu.ResumeLayout(false);
            this.byteStrip.ResumeLayout(false);
            this.byteStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutBrewsterToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel noInfoPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ImageList treeIcons;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToFolderToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog grpOpen;
        private System.Windows.Forms.ContextMenuStrip rootMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip nodeMenu;
        private System.Windows.Forms.ToolStripMenuItem addAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBelowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.StatusStrip byteStrip;
        private System.Windows.Forms.ToolStripStatusLabel bytesLabel;
        private System.Windows.Forms.Panel playerPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.SaveFileDialog grpSave;
        private System.Windows.Forms.TrackBar volumeBar;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderOpen;
        private System.Windows.Forms.OpenFileDialog soundOpen;
        private System.Windows.Forms.SaveFileDialog soundSave;
    }
}