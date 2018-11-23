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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Sound Sequences", 3, 3);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Sound Streams", 1, 1);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Wave Sound Sets", 2, 2);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Instrument Banks", 5, 5);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Players", 8, 8);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Sound Groups", 4, 4);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Groups", 7, 7);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Wave Archives", 6, 6);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Internal", 11, 11);
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("External", 11, 11);
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Files", 11, 11, new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Project Information", 10, 10);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fileIdPanel = new System.Windows.Forms.Panel();
            this.fileIdBox = new System.Windows.Forms.ComboBox();
            this.fileIdLabel = new System.Windows.Forms.Label();
            this.bankPanel = new System.Windows.Forms.Panel();
            this.flagBankButton = new System.Windows.Forms.Button();
            this.flagsBankLabel = new System.Windows.Forms.Label();
            this.projectInfoPanel = new System.Windows.Forms.Panel();
            this.optionsPIBox = new System.Windows.Forms.NumericUpDown();
            this.optionsPILabel = new System.Windows.Forms.Label();
            this.streamBufferTimesBox = new System.Windows.Forms.NumericUpDown();
            this.streamBufferTimesLabel = new System.Windows.Forms.Label();
            this.maxWaveNumTracksBox = new System.Windows.Forms.NumericUpDown();
            this.maxWaveNumTracksLabel = new System.Windows.Forms.Label();
            this.maxWaveNumBox = new System.Windows.Forms.NumericUpDown();
            this.maxWaveNumLabel = new System.Windows.Forms.Label();
            this.maxStreamNumChannelsBox = new System.Windows.Forms.NumericUpDown();
            this.maxStreamNumChannelsLabel = new System.Windows.Forms.Label();
            this.maxStreamNumTracksBox = new System.Windows.Forms.NumericUpDown();
            this.maxStreamNumTracksLabel = new System.Windows.Forms.Label();
            this.maxStreamNumBox = new System.Windows.Forms.NumericUpDown();
            this.maxStreamNumLabel = new System.Windows.Forms.Label();
            this.maxSeqTrackNumBox = new System.Windows.Forms.NumericUpDown();
            this.maxSeqTrackNumLabel = new System.Windows.Forms.Label();
            this.maxSeqNumBox = new System.Windows.Forms.NumericUpDown();
            this.maxSeqNumLabel = new System.Windows.Forms.Label();
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
            this.goldisGrouperGRPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolfsRescourceResearcherBARSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutCitricComposerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openB_sarBox = new System.Windows.Forms.OpenFileDialog();
            this.folderSelector = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.fileIdPanel.SuspendLayout();
            this.bankPanel.SuspendLayout();
            this.projectInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsPIBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.streamBufferTimesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumTracksBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumChannelsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumTracksBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqTrackNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqNumBox)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.fileIdPanel);
            this.splitContainer1.Panel1.Controls.Add(this.bankPanel);
            this.splitContainer1.Panel1.Controls.Add(this.projectInfoPanel);
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
            // fileIdPanel
            // 
            this.fileIdPanel.Controls.Add(this.fileIdBox);
            this.fileIdPanel.Controls.Add(this.fileIdLabel);
            this.fileIdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileIdPanel.Location = new System.Drawing.Point(0, 0);
            this.fileIdPanel.Name = "fileIdPanel";
            this.fileIdPanel.Size = new System.Drawing.Size(235, 54);
            this.fileIdPanel.TabIndex = 21;
            this.fileIdPanel.Visible = false;
            // 
            // fileIdBox
            // 
            this.fileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileIdBox.FormattingEnabled = true;
            this.fileIdBox.Location = new System.Drawing.Point(7, 25);
            this.fileIdBox.Name = "fileIdBox";
            this.fileIdBox.Size = new System.Drawing.Size(221, 21);
            this.fileIdBox.TabIndex = 1;
            // 
            // fileIdLabel
            // 
            this.fileIdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileIdLabel.Location = new System.Drawing.Point(7, 5);
            this.fileIdLabel.Name = "fileIdLabel";
            this.fileIdLabel.Size = new System.Drawing.Size(223, 17);
            this.fileIdLabel.TabIndex = 0;
            this.fileIdLabel.Text = "File Id:";
            this.fileIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bankPanel
            // 
            this.bankPanel.Controls.Add(this.flagBankButton);
            this.bankPanel.Controls.Add(this.flagsBankLabel);
            this.bankPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankPanel.Location = new System.Drawing.Point(0, 0);
            this.bankPanel.Name = "bankPanel";
            this.bankPanel.Size = new System.Drawing.Size(235, 473);
            this.bankPanel.TabIndex = 19;
            this.bankPanel.Visible = false;
            // 
            // flagBankButton
            // 
            this.flagBankButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flagBankButton.Location = new System.Drawing.Point(7, 77);
            this.flagBankButton.Name = "flagBankButton";
            this.flagBankButton.Size = new System.Drawing.Size(222, 23);
            this.flagBankButton.TabIndex = 2;
            this.flagBankButton.Text = "Flag Editor";
            this.flagBankButton.UseVisualStyleBackColor = true;
            // 
            // flagsBankLabel
            // 
            this.flagsBankLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flagsBankLabel.Location = new System.Drawing.Point(7, 57);
            this.flagsBankLabel.Name = "flagsBankLabel";
            this.flagsBankLabel.Size = new System.Drawing.Size(223, 17);
            this.flagsBankLabel.TabIndex = 1;
            this.flagsBankLabel.Text = "Flags:";
            this.flagsBankLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // projectInfoPanel
            // 
            this.projectInfoPanel.Controls.Add(this.optionsPIBox);
            this.projectInfoPanel.Controls.Add(this.optionsPILabel);
            this.projectInfoPanel.Controls.Add(this.streamBufferTimesBox);
            this.projectInfoPanel.Controls.Add(this.streamBufferTimesLabel);
            this.projectInfoPanel.Controls.Add(this.maxWaveNumTracksBox);
            this.projectInfoPanel.Controls.Add(this.maxWaveNumTracksLabel);
            this.projectInfoPanel.Controls.Add(this.maxWaveNumBox);
            this.projectInfoPanel.Controls.Add(this.maxWaveNumLabel);
            this.projectInfoPanel.Controls.Add(this.maxStreamNumChannelsBox);
            this.projectInfoPanel.Controls.Add(this.maxStreamNumChannelsLabel);
            this.projectInfoPanel.Controls.Add(this.maxStreamNumTracksBox);
            this.projectInfoPanel.Controls.Add(this.maxStreamNumTracksLabel);
            this.projectInfoPanel.Controls.Add(this.maxStreamNumBox);
            this.projectInfoPanel.Controls.Add(this.maxStreamNumLabel);
            this.projectInfoPanel.Controls.Add(this.maxSeqTrackNumBox);
            this.projectInfoPanel.Controls.Add(this.maxSeqTrackNumLabel);
            this.projectInfoPanel.Controls.Add(this.maxSeqNumBox);
            this.projectInfoPanel.Controls.Add(this.maxSeqNumLabel);
            this.projectInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.projectInfoPanel.Name = "projectInfoPanel";
            this.projectInfoPanel.Size = new System.Drawing.Size(235, 473);
            this.projectInfoPanel.TabIndex = 20;
            this.projectInfoPanel.Visible = false;
            // 
            // optionsPIBox
            // 
            this.optionsPIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPIBox.Location = new System.Drawing.Point(7, 409);
            this.optionsPIBox.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.optionsPIBox.Name = "optionsPIBox";
            this.optionsPIBox.Size = new System.Drawing.Size(223, 20);
            this.optionsPIBox.TabIndex = 17;
            // 
            // optionsPILabel
            // 
            this.optionsPILabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPILabel.Location = new System.Drawing.Point(11, 384);
            this.optionsPILabel.Name = "optionsPILabel";
            this.optionsPILabel.Size = new System.Drawing.Size(211, 22);
            this.optionsPILabel.TabIndex = 16;
            this.optionsPILabel.Text = "Options:";
            this.optionsPILabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // streamBufferTimesBox
            // 
            this.streamBufferTimesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamBufferTimesBox.Location = new System.Drawing.Point(7, 361);
            this.streamBufferTimesBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.streamBufferTimesBox.Name = "streamBufferTimesBox";
            this.streamBufferTimesBox.Size = new System.Drawing.Size(223, 20);
            this.streamBufferTimesBox.TabIndex = 15;
            // 
            // streamBufferTimesLabel
            // 
            this.streamBufferTimesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamBufferTimesLabel.Location = new System.Drawing.Point(11, 336);
            this.streamBufferTimesLabel.Name = "streamBufferTimesLabel";
            this.streamBufferTimesLabel.Size = new System.Drawing.Size(211, 22);
            this.streamBufferTimesLabel.TabIndex = 14;
            this.streamBufferTimesLabel.Text = "Stream Buffer Times:";
            this.streamBufferTimesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxWaveNumTracksBox
            // 
            this.maxWaveNumTracksBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxWaveNumTracksBox.Location = new System.Drawing.Point(7, 313);
            this.maxWaveNumTracksBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxWaveNumTracksBox.Name = "maxWaveNumTracksBox";
            this.maxWaveNumTracksBox.Size = new System.Drawing.Size(223, 20);
            this.maxWaveNumTracksBox.TabIndex = 13;
            // 
            // maxWaveNumTracksLabel
            // 
            this.maxWaveNumTracksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxWaveNumTracksLabel.Location = new System.Drawing.Point(11, 288);
            this.maxWaveNumTracksLabel.Name = "maxWaveNumTracksLabel";
            this.maxWaveNumTracksLabel.Size = new System.Drawing.Size(211, 22);
            this.maxWaveNumTracksLabel.TabIndex = 12;
            this.maxWaveNumTracksLabel.Text = "Max Number Of Wave Tracks:";
            this.maxWaveNumTracksLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxWaveNumBox
            // 
            this.maxWaveNumBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxWaveNumBox.Location = new System.Drawing.Point(7, 265);
            this.maxWaveNumBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxWaveNumBox.Name = "maxWaveNumBox";
            this.maxWaveNumBox.Size = new System.Drawing.Size(223, 20);
            this.maxWaveNumBox.TabIndex = 11;
            // 
            // maxWaveNumLabel
            // 
            this.maxWaveNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxWaveNumLabel.Location = new System.Drawing.Point(11, 240);
            this.maxWaveNumLabel.Name = "maxWaveNumLabel";
            this.maxWaveNumLabel.Size = new System.Drawing.Size(211, 22);
            this.maxWaveNumLabel.TabIndex = 10;
            this.maxWaveNumLabel.Text = "Max Number Of Waves:";
            this.maxWaveNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxStreamNumChannelsBox
            // 
            this.maxStreamNumChannelsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumChannelsBox.Location = new System.Drawing.Point(7, 217);
            this.maxStreamNumChannelsBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxStreamNumChannelsBox.Name = "maxStreamNumChannelsBox";
            this.maxStreamNumChannelsBox.Size = new System.Drawing.Size(223, 20);
            this.maxStreamNumChannelsBox.TabIndex = 9;
            // 
            // maxStreamNumChannelsLabel
            // 
            this.maxStreamNumChannelsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumChannelsLabel.Location = new System.Drawing.Point(11, 192);
            this.maxStreamNumChannelsLabel.Name = "maxStreamNumChannelsLabel";
            this.maxStreamNumChannelsLabel.Size = new System.Drawing.Size(211, 22);
            this.maxStreamNumChannelsLabel.TabIndex = 8;
            this.maxStreamNumChannelsLabel.Text = "Max Number Of Stream Channels:";
            this.maxStreamNumChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxStreamNumTracksBox
            // 
            this.maxStreamNumTracksBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumTracksBox.Location = new System.Drawing.Point(7, 169);
            this.maxStreamNumTracksBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxStreamNumTracksBox.Name = "maxStreamNumTracksBox";
            this.maxStreamNumTracksBox.Size = new System.Drawing.Size(223, 20);
            this.maxStreamNumTracksBox.TabIndex = 7;
            // 
            // maxStreamNumTracksLabel
            // 
            this.maxStreamNumTracksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumTracksLabel.Location = new System.Drawing.Point(11, 144);
            this.maxStreamNumTracksLabel.Name = "maxStreamNumTracksLabel";
            this.maxStreamNumTracksLabel.Size = new System.Drawing.Size(211, 22);
            this.maxStreamNumTracksLabel.TabIndex = 6;
            this.maxStreamNumTracksLabel.Text = "Max Number Of Stream Tracks:";
            this.maxStreamNumTracksLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxStreamNumBox
            // 
            this.maxStreamNumBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumBox.Location = new System.Drawing.Point(7, 121);
            this.maxStreamNumBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxStreamNumBox.Name = "maxStreamNumBox";
            this.maxStreamNumBox.Size = new System.Drawing.Size(223, 20);
            this.maxStreamNumBox.TabIndex = 5;
            // 
            // maxStreamNumLabel
            // 
            this.maxStreamNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumLabel.Location = new System.Drawing.Point(11, 96);
            this.maxStreamNumLabel.Name = "maxStreamNumLabel";
            this.maxStreamNumLabel.Size = new System.Drawing.Size(211, 22);
            this.maxStreamNumLabel.TabIndex = 4;
            this.maxStreamNumLabel.Text = "Max Number Of Streams:";
            this.maxStreamNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxSeqTrackNumBox
            // 
            this.maxSeqTrackNumBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxSeqTrackNumBox.Location = new System.Drawing.Point(7, 73);
            this.maxSeqTrackNumBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxSeqTrackNumBox.Name = "maxSeqTrackNumBox";
            this.maxSeqTrackNumBox.Size = new System.Drawing.Size(223, 20);
            this.maxSeqTrackNumBox.TabIndex = 3;
            // 
            // maxSeqTrackNumLabel
            // 
            this.maxSeqTrackNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxSeqTrackNumLabel.Location = new System.Drawing.Point(11, 48);
            this.maxSeqTrackNumLabel.Name = "maxSeqTrackNumLabel";
            this.maxSeqTrackNumLabel.Size = new System.Drawing.Size(211, 22);
            this.maxSeqTrackNumLabel.TabIndex = 2;
            this.maxSeqTrackNumLabel.Text = "Max Number Of Sequence Tracks:";
            this.maxSeqTrackNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxSeqNumBox
            // 
            this.maxSeqNumBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxSeqNumBox.Location = new System.Drawing.Point(7, 25);
            this.maxSeqNumBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.maxSeqNumBox.Name = "maxSeqNumBox";
            this.maxSeqNumBox.Size = new System.Drawing.Size(223, 20);
            this.maxSeqNumBox.TabIndex = 1;
            // 
            // maxSeqNumLabel
            // 
            this.maxSeqNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxSeqNumLabel.Location = new System.Drawing.Point(11, 0);
            this.maxSeqNumLabel.Name = "maxSeqNumLabel";
            this.maxSeqNumLabel.Size = new System.Drawing.Size(211, 22);
            this.maxSeqNumLabel.TabIndex = 0;
            this.maxSeqNumLabel.Text = "Max Number Of Sequences:";
            this.maxSeqNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            treeNode1.Name = "sequences";
            treeNode1.SelectedImageIndex = 3;
            treeNode1.Text = "Sound Sequences";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "streams";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Text = "Sound Streams";
            treeNode3.ImageIndex = 2;
            treeNode3.Name = "waveSoundSets";
            treeNode3.SelectedImageIndex = 2;
            treeNode3.Text = "Wave Sound Sets";
            treeNode4.ImageIndex = 5;
            treeNode4.Name = "banks";
            treeNode4.SelectedImageIndex = 5;
            treeNode4.Text = "Instrument Banks";
            treeNode5.ImageIndex = 8;
            treeNode5.Name = "players";
            treeNode5.SelectedImageIndex = 8;
            treeNode5.Text = "Players";
            treeNode6.ImageIndex = 4;
            treeNode6.Name = "soundGroups";
            treeNode6.SelectedImageIndex = 4;
            treeNode6.Text = "Sound Groups";
            treeNode7.ImageIndex = 7;
            treeNode7.Name = "groups";
            treeNode7.SelectedImageIndex = 7;
            treeNode7.Text = "Groups";
            treeNode8.ImageIndex = 6;
            treeNode8.Name = "waveArchives";
            treeNode8.SelectedImageIndex = 6;
            treeNode8.Text = "Wave Archives";
            treeNode9.ImageIndex = 11;
            treeNode9.Name = "internalFiles";
            treeNode9.SelectedImageIndex = 11;
            treeNode9.Text = "Internal";
            treeNode10.ImageIndex = 11;
            treeNode10.Name = "externalFiles";
            treeNode10.SelectedImageIndex = 11;
            treeNode10.Text = "External";
            treeNode11.ImageIndex = 11;
            treeNode11.Name = "files";
            treeNode11.SelectedImageIndex = 11;
            treeNode11.Text = "Files";
            treeNode12.ImageIndex = 10;
            treeNode12.Name = "projectInfo";
            treeNode12.SelectedImageIndex = 10;
            treeNode12.Text = "Project Information";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode11,
            treeNode12});
            this.tree.SelectedImageIndex = 0;
            this.tree.ShowLines = false;
            this.tree.Size = new System.Drawing.Size(609, 473);
            this.tree.TabIndex = 0;
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseClick);
            this.tree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeArrowKey);
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
            this.aboutToolStripMenuItem});
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
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
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
            this.exportToFolderToolStripMenuItem.Click += new System.EventHandler(this.exportToFolderToolStripMenuItem_Click);
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
            this.brewstersArchiveBrewerWARToolStripMenuItem,
            this.goldisGrouperGRPToolStripMenuItem,
            this.rolfsRescourceResearcherBARSToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // isabelleSoundEditorToolStripMenuItem
            // 
            this.isabelleSoundEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("isabelleSoundEditorToolStripMenuItem.Image")));
            this.isabelleSoundEditorToolStripMenuItem.Name = "isabelleSoundEditorToolStripMenuItem";
            this.isabelleSoundEditorToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.isabelleSoundEditorToolStripMenuItem.Text = "Isabelle Sound Editor (WAV, STM)";
            this.isabelleSoundEditorToolStripMenuItem.Click += new System.EventHandler(this.isabelleSoundEditorToolStripMenuItem_Click);
            // 
            // brewstersArchiveBrewerWARToolStripMenuItem
            // 
            this.brewstersArchiveBrewerWARToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("brewstersArchiveBrewerWARToolStripMenuItem.Image")));
            this.brewstersArchiveBrewerWARToolStripMenuItem.Name = "brewstersArchiveBrewerWARToolStripMenuItem";
            this.brewstersArchiveBrewerWARToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.brewstersArchiveBrewerWARToolStripMenuItem.Text = "Brewster\'s Archive Brewer (WAR)";
            this.brewstersArchiveBrewerWARToolStripMenuItem.Click += new System.EventHandler(this.brewstersArchiveBrewerWARToolStripMenuItem_Click);
            // 
            // goldisGrouperGRPToolStripMenuItem
            // 
            this.goldisGrouperGRPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("goldisGrouperGRPToolStripMenuItem.Image")));
            this.goldisGrouperGRPToolStripMenuItem.Name = "goldisGrouperGRPToolStripMenuItem";
            this.goldisGrouperGRPToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.goldisGrouperGRPToolStripMenuItem.Text = "Goldi\'s Grouper (GRP)";
            this.goldisGrouperGRPToolStripMenuItem.Click += new System.EventHandler(this.goldisGrouperGRPToolStripMenuItem_Click);
            // 
            // rolfsRescourceResearcherBARSToolStripMenuItem
            // 
            this.rolfsRescourceResearcherBARSToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("rolfsRescourceResearcherBARSToolStripMenuItem.Image")));
            this.rolfsRescourceResearcherBARSToolStripMenuItem.Name = "rolfsRescourceResearcherBARSToolStripMenuItem";
            this.rolfsRescourceResearcherBARSToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.rolfsRescourceResearcherBARSToolStripMenuItem.Text = "Rolf\'s Rescource Researcher (BARS)";
            this.rolfsRescourceResearcherBARSToolStripMenuItem.Click += new System.EventHandler(this.rolfsRescourceResearcherBARSToolStripMenuItem_Click);
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
            this.getHelpToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
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
            // openB_sarBox
            // 
            this.openB_sarBox.Filter = "Cafe/Citric Sound Archive|*.bfsar;*.bcsar";
            this.openB_sarBox.RestoreDirectory = true;
            // 
            // folderSelector
            // 
            this.folderSelector.RootFolder = System.Environment.SpecialFolder.MyComputer;
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
            this.Text = "Citric Composer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.fileIdPanel.ResumeLayout(false);
            this.bankPanel.ResumeLayout(false);
            this.projectInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionsPIBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.streamBufferTimesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumTracksBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumChannelsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumTracksBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqTrackNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqNumBox)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem goldisGrouperGRPToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openB_sarBox;
        private System.Windows.Forms.Panel bankPanel;
        private System.Windows.Forms.Panel projectInfoPanel;
        private System.Windows.Forms.NumericUpDown optionsPIBox;
        private System.Windows.Forms.Label optionsPILabel;
        private System.Windows.Forms.NumericUpDown streamBufferTimesBox;
        private System.Windows.Forms.Label streamBufferTimesLabel;
        private System.Windows.Forms.NumericUpDown maxWaveNumTracksBox;
        private System.Windows.Forms.Label maxWaveNumTracksLabel;
        private System.Windows.Forms.NumericUpDown maxWaveNumBox;
        private System.Windows.Forms.Label maxWaveNumLabel;
        private System.Windows.Forms.NumericUpDown maxStreamNumChannelsBox;
        private System.Windows.Forms.Label maxStreamNumChannelsLabel;
        private System.Windows.Forms.NumericUpDown maxStreamNumTracksBox;
        private System.Windows.Forms.Label maxStreamNumTracksLabel;
        private System.Windows.Forms.NumericUpDown maxStreamNumBox;
        private System.Windows.Forms.Label maxStreamNumLabel;
        private System.Windows.Forms.NumericUpDown maxSeqTrackNumBox;
        private System.Windows.Forms.Label maxSeqTrackNumLabel;
        private System.Windows.Forms.NumericUpDown maxSeqNumBox;
        private System.Windows.Forms.Label maxSeqNumLabel;
        private System.Windows.Forms.Panel fileIdPanel;
        private System.Windows.Forms.Label fileIdLabel;
        private System.Windows.Forms.ComboBox fileIdBox;
        private System.Windows.Forms.Label flagsBankLabel;
        private System.Windows.Forms.Button flagBankButton;
        private System.Windows.Forms.ToolStripMenuItem rolfsRescourceResearcherBARSToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderSelector;
    }
}

