using CitraFileLoader;
using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer {

    /// <summary>
    /// An editor base for files.
    /// </summary>
    public abstract class EditorBase : Form {

        /// <summary>
        /// File type to use.
        /// </summary>
        public ISoundFile File;

        /// <summary>
        /// Write mode.
        /// </summary>
        public WriteMode WriteMode;

        /// <summary>
        /// File path.
        /// </summary>
        public string FilePath;

        /// <summary>
        /// Description of the extension.
        /// </summary>
        public string ExtensionDescription;

        /// <summary>
        /// Extension of format.
        /// </summary>
        public string Extension;

        /// <summary>
        /// Editor name.
        /// </summary>
        public string EditorName;

        /// <summary>
        /// Current file open.
        /// </summary>
        public bool FileOpen;

        /// <summary>
        /// File type.
        /// </summary>
        public Type FileType;

        /// <summary>
        /// Main window.
        /// </summary>
        public MainWindow MainWindow;

        /// <summary>
        /// External file in the 
        /// </summary>
        public SoundFile<CitraFileLoader.ISoundFile> ExtFile;


        /// <summary>
        /// Constructur.
        /// </summary>
        /// <param name="fileType">Type of file.</param>
        /// <param name="extensionDescription">Description of the extension.</param>
        /// <param name="extension">File extension type.</param>
        /// <param name="editorName">Editor name.</param>
        /// <param name="mainWindow">Main window.</param>
        public EditorBase(Type fileType, string extensionDescription, string extension, string editorName, MainWindow mainWindow) {

            //Initialize component.
            InitializeComponent();

            //Set main window.
            MainWindow = mainWindow;

            //Set file type.
            FileType = fileType;

            //Extension description.
            ExtensionDescription = extensionDescription;

            //Extension.
            Extension = extension;

            //Editor name.
            EditorName = editorName;
            Text = EditorName;

            //Remove the nullify menu.
            editToolStripMenuItem.DropDownItems.Remove(nullifyFileToolStripMenuItem);

            //Update nodes.
            UpdateNodes();

            //Do info stuff.
            DoInfoStuff();

        }

        /// <summary>
        /// Constructur.
        /// </summary>
        /// <param name="fileType">Type of file.</param>
        /// <param name="extensionDescription">Description of the extension.</param>
        /// <param name="extension">File extension type.</param>
        /// <param name="editorName">Editor name.</param>
        /// <param name="fileToOpen">File to open.</param>
        /// <param name="mainWindow">Main window.</param>
        public EditorBase(Type fileType, string extensionDescription, string extension, string editorName, string fileToOpen, MainWindow mainWindow) {

            //Initialize component.
            InitializeComponent();

            //Set main window.
            MainWindow = mainWindow;

            //Set file type.
            FileType = fileType;

            //Extension description.
            ExtensionDescription = extensionDescription;

            //Extension.
            Extension = extension;

            //Editor name.
            EditorName = editorName;

            //Open file.
            File = (ISoundFile)Activator.CreateInstance(FileType);
            ExtFile = null;
            FilePath = fileToOpen;
            Text = EditorName + " - " + Path.GetFileName(fileToOpen);
            FileOpen = true;

            //Read data.
            MemoryStream src = new MemoryStream(System.IO.File.ReadAllBytes(fileToOpen));
            BinaryDataReader br = new BinaryDataReader(src);
            File.Read(br);

            //Remove the nullify menu.
            editToolStripMenuItem.DropDownItems.Remove(nullifyFileToolStripMenuItem);

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        /// <summary>
        /// Constructur.
        /// </summary>
        /// <param name="fileType">Type of file.</param>
        /// <param name="extensionDescription">Description of the extension.</param>
        /// <param name="extension">File extension type.</param>
        /// <param name="editorName">Editor name.</param>
        /// <param name="fileToOpen">File to open.</param>
        /// <param name="mainWindow">Main window.</param>
        public EditorBase(Type fileType, string extensionDescription, string extension, string editorName, SoundFile<ISoundFile> fileToOpen, MainWindow mainWindow) {

            //Initialize component.
            InitializeComponent();

            //Set main window.
            MainWindow = mainWindow;

            //Set file type.
            FileType = fileType;

            //Extension description.
            ExtensionDescription = extensionDescription;

            //Extension.
            Extension = extension;

            //Editor name.
            EditorName = editorName;

            //Open file.
            ExtFile = fileToOpen;
            File = ExtFile.File;
            FilePath = "";
            string name = ExtFile.FileName;
            if (name == null) {
                name = "{ Null File Name }";
            }
            Text = EditorName + " - " + name + "." + ExtFile.FileExtension;
            FileOpen = true;

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        public MenuStrip menuStrip;
        public ToolStripMenuItem newToolStripMenuItem;
        public ToolStripMenuItem openToolStripMenuItem;
        public ToolStripMenuItem saveToolStripMenuItem;
        public ToolStripMenuItem saveAsToolStripMenuItem;
        public ToolStripMenuItem closeToolStripMenuItem;
        public ToolStripMenuItem quitToolStripMenuItem;
        public ToolStripMenuItem editToolStripMenuItem;
        public ToolStripMenuItem blankFileToolStripMenuItem;
        public ToolStripMenuItem importFileToolStripMenuItem;
        public ToolStripMenuItem exportFileToolStripMenuItem;
        public SplitContainer splitContainer1;
        public TreeView tree;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private StatusStrip statusStrip;
        public ToolStripStatusLabel status;
        private ImageList treeIcons;
        private System.ComponentModel.IContainer components;
        public ContextMenuStrip rootMenu;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem expandToolStripMenuItem;
        private ToolStripMenuItem collapseToolStripMenuItem;
        public Panel noInfoPanel;
        private Label label1;
        private Panel nullFilePanel;
        private ToolStripMenuItem nullifyFileToolStripMenuItem;
        private Label label2;
        private Panel genericFileInfoPanel;
        private Label label3;
        private TableLayoutPanel versionGrid;
        private ToolTip toolTip;
        public NumericUpDown vRevBox;
        public NumericUpDown vMinBox;
        public NumericUpDown vMajBox;
        public ToolStripMenuItem fileMenu;
        public Panel soundPlayerDeluxePanel;
        private TableLayoutPanel soundDeluxeTrack1;
        private Button playSoundTrack;
        private Label soundPlayerDeluxeLabel;
        private TableLayoutPanel soundDeluxeTrack2;
        private Button pauseSoundTrack;
        private Button stopSoundTrack;
        public Panel nullDataPanel;
        private Label label4;
        public Panel warFileInfoPanel;
        private Label label6;
        private TableLayoutPanel tableLayoutPanel1;
        public NumericUpDown vRevBoxWar;
        public NumericUpDown vMinBoxWar;
        public NumericUpDown vMajBoxWar;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel2;
        public NumericUpDown vWavRevBox;
        public NumericUpDown vWavMinBox;
        public NumericUpDown vWavMajBox;
        public Button forceWaveVersionButton;
        public ContextMenuStrip nodeMenu;
        private ToolStripMenuItem addAboveToolStripMenuItem1;
        private ToolStripMenuItem addBelowToolStripMenuItem1;
        private ToolStripMenuItem moveUpToolStripMenuItem1;
        private ToolStripMenuItem moveDownToolStripMenuItem1;
        private ToolStripMenuItem blankToolStripMenuItem;
        private ToolStripMenuItem replaceFileToolStripMenuItem;
        private ToolStripMenuItem nullifyToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem exportToolStripMenuItem1;
        public ToolStripMenuItem toolsWarToolStripMenuItem;
        private ToolStripMenuItem batchExtractWavesToolStripMenuItem;
        private ToolStripMenuItem batchExtract3dsWavesToolStripMenuItem;
        private ToolStripMenuItem batchExtractWiiUWavesToolStripMenuItem;
        private ToolStripMenuItem batchExtractSwitchWavesToolStripMenuItem;
        private ToolStripMenuItem batchImportToolStripMenuItem;

        public void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorBase));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("File Information", 10, 10);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nullifyFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsWarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchExtractWavesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchExtract3dsWavesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchExtractWiiUWavesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchExtractSwitchWavesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.warFileInfoPanel = new System.Windows.Forms.Panel();
            this.forceWaveVersionButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.vWavRevBox = new System.Windows.Forms.NumericUpDown();
            this.vWavMinBox = new System.Windows.Forms.NumericUpDown();
            this.vWavMajBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.vRevBoxWar = new System.Windows.Forms.NumericUpDown();
            this.vMinBoxWar = new System.Windows.Forms.NumericUpDown();
            this.vMajBoxWar = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.genericFileInfoPanel = new System.Windows.Forms.Panel();
            this.versionGrid = new System.Windows.Forms.TableLayoutPanel();
            this.vRevBox = new System.Windows.Forms.NumericUpDown();
            this.vMinBox = new System.Windows.Forms.NumericUpDown();
            this.vMajBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nullDataPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.soundPlayerDeluxePanel = new System.Windows.Forms.Panel();
            this.soundDeluxeTrack2 = new System.Windows.Forms.TableLayoutPanel();
            this.pauseSoundTrack = new System.Windows.Forms.Button();
            this.stopSoundTrack = new System.Windows.Forms.Button();
            this.soundDeluxeTrack1 = new System.Windows.Forms.TableLayoutPanel();
            this.playSoundTrack = new System.Windows.Forms.Button();
            this.soundPlayerDeluxeLabel = new System.Windows.Forms.Label();
            this.nullFilePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.noInfoPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tree = new System.Windows.Forms.TreeView();
            this.treeIcons = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.rootMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.nodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAboveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addBelowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.blankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nullifyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.warFileInfoPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vWavRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vWavMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vWavMajBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vRevBoxWar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMinBoxWar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMajBoxWar)).BeginInit();
            this.genericFileInfoPanel.SuspendLayout();
            this.versionGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMajBox)).BeginInit();
            this.nullDataPanel.SuspendLayout();
            this.soundPlayerDeluxePanel.SuspendLayout();
            this.soundDeluxeTrack2.SuspendLayout();
            this.soundDeluxeTrack1.SuspendLayout();
            this.nullFilePanel.SuspendLayout();
            this.noInfoPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.rootMenu.SuspendLayout();
            this.nodeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editToolStripMenuItem,
            this.toolsWarToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(744, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("quitToolStripMenuItem.Image")));
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blankFileToolStripMenuItem,
            this.importFileToolStripMenuItem,
            this.exportFileToolStripMenuItem,
            this.nullifyFileToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // blankFileToolStripMenuItem
            // 
            this.blankFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("blankFileToolStripMenuItem.Image")));
            this.blankFileToolStripMenuItem.Name = "blankFileToolStripMenuItem";
            this.blankFileToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.blankFileToolStripMenuItem.Text = "Blank File";
            this.blankFileToolStripMenuItem.Click += new System.EventHandler(this.blankFileToolStripMenuItem_Click);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importFileToolStripMenuItem.Image")));
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.importFileToolStripMenuItem.Text = "Import File";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // exportFileToolStripMenuItem
            // 
            this.exportFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportFileToolStripMenuItem.Image")));
            this.exportFileToolStripMenuItem.Name = "exportFileToolStripMenuItem";
            this.exportFileToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.exportFileToolStripMenuItem.Text = "Export File";
            this.exportFileToolStripMenuItem.Click += new System.EventHandler(this.exportFileToolStripMenuItem_Click);
            // 
            // nullifyFileToolStripMenuItem
            // 
            this.nullifyFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nullifyFileToolStripMenuItem.Image")));
            this.nullifyFileToolStripMenuItem.Name = "nullifyFileToolStripMenuItem";
            this.nullifyFileToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.nullifyFileToolStripMenuItem.Text = "Nullify File";
            this.nullifyFileToolStripMenuItem.Click += new System.EventHandler(this.nullifyFileToolStripMenuItem_Click);
            // 
            // toolsWarToolStripMenuItem
            // 
            this.toolsWarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchExtractWavesToolStripMenuItem,
            this.batchExtract3dsWavesToolStripMenuItem,
            this.batchExtractWiiUWavesToolStripMenuItem,
            this.batchExtractSwitchWavesToolStripMenuItem,
            this.batchImportToolStripMenuItem});
            this.toolsWarToolStripMenuItem.Name = "toolsWarToolStripMenuItem";
            this.toolsWarToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsWarToolStripMenuItem.Text = "Tools";
            this.toolsWarToolStripMenuItem.Visible = false;
            // 
            // batchExtractWavesToolStripMenuItem
            // 
            this.batchExtractWavesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("batchExtractWavesToolStripMenuItem.Image")));
            this.batchExtractWavesToolStripMenuItem.Name = "batchExtractWavesToolStripMenuItem";
            this.batchExtractWavesToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.batchExtractWavesToolStripMenuItem.Text = "Batch Extract Waves";
            this.batchExtractWavesToolStripMenuItem.Click += new System.EventHandler(this.batchExtractWavesToolStripMenuItem_Click);
            // 
            // batchExtract3dsWavesToolStripMenuItem
            // 
            this.batchExtract3dsWavesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("batchExtract3dsWavesToolStripMenuItem.Image")));
            this.batchExtract3dsWavesToolStripMenuItem.Name = "batchExtract3dsWavesToolStripMenuItem";
            this.batchExtract3dsWavesToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.batchExtract3dsWavesToolStripMenuItem.Text = "Batch Extract 3ds Waves";
            this.batchExtract3dsWavesToolStripMenuItem.Click += new System.EventHandler(this.batchExtract3dsWavesToolStripMenuItem_Click);
            // 
            // batchExtractWiiUWavesToolStripMenuItem
            // 
            this.batchExtractWiiUWavesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("batchExtractWiiUWavesToolStripMenuItem.Image")));
            this.batchExtractWiiUWavesToolStripMenuItem.Name = "batchExtractWiiUWavesToolStripMenuItem";
            this.batchExtractWiiUWavesToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.batchExtractWiiUWavesToolStripMenuItem.Text = "Batch Extract Wii U Waves";
            this.batchExtractWiiUWavesToolStripMenuItem.Click += new System.EventHandler(this.batchExtractWiiUWavesToolStripMenuItem_Click);
            // 
            // batchExtractSwitchWavesToolStripMenuItem
            // 
            this.batchExtractSwitchWavesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("batchExtractSwitchWavesToolStripMenuItem.Image")));
            this.batchExtractSwitchWavesToolStripMenuItem.Name = "batchExtractSwitchWavesToolStripMenuItem";
            this.batchExtractSwitchWavesToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.batchExtractSwitchWavesToolStripMenuItem.Text = "Batch Extract Switch Waves";
            this.batchExtractSwitchWavesToolStripMenuItem.Click += new System.EventHandler(this.batchExtractSwitchWavesToolStripMenuItem_Click);
            // 
            // batchImportToolStripMenuItem
            // 
            this.batchImportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("batchImportToolStripMenuItem.Image")));
            this.batchImportToolStripMenuItem.Name = "batchImportToolStripMenuItem";
            this.batchImportToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.batchImportToolStripMenuItem.Text = "Batch Import";
            this.batchImportToolStripMenuItem.Click += new System.EventHandler(this.batchImportToolStripMenuItem_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.warFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.genericFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.nullDataPanel);
            this.splitContainer1.Panel1.Controls.Add(this.soundPlayerDeluxePanel);
            this.splitContainer1.Panel1.Controls.Add(this.nullFilePanel);
            this.splitContainer1.Panel1.Controls.Add(this.noInfoPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tree);
            this.splitContainer1.Size = new System.Drawing.Size(744, 356);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 1;
            // 
            // warFileInfoPanel
            // 
            this.warFileInfoPanel.Controls.Add(this.forceWaveVersionButton);
            this.warFileInfoPanel.Controls.Add(this.tableLayoutPanel2);
            this.warFileInfoPanel.Controls.Add(this.label6);
            this.warFileInfoPanel.Controls.Add(this.tableLayoutPanel1);
            this.warFileInfoPanel.Controls.Add(this.label5);
            this.warFileInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warFileInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.warFileInfoPanel.Name = "warFileInfoPanel";
            this.warFileInfoPanel.Size = new System.Drawing.Size(246, 354);
            this.warFileInfoPanel.TabIndex = 6;
            // 
            // forceWaveVersionButton
            // 
            this.forceWaveVersionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forceWaveVersionButton.Location = new System.Drawing.Point(3, 110);
            this.forceWaveVersionButton.Name = "forceWaveVersionButton";
            this.forceWaveVersionButton.Size = new System.Drawing.Size(240, 23);
            this.forceWaveVersionButton.TabIndex = 4;
            this.forceWaveVersionButton.Text = "Force Internal Wave Version";
            this.forceWaveVersionButton.UseVisualStyleBackColor = true;
            this.forceWaveVersionButton.Click += new System.EventHandler(this.forceWaveVersionButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.vWavRevBox, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.vWavMinBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.vWavMajBox, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 78);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(247, 26);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // vWavRevBox
            // 
            this.vWavRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vWavRevBox.Location = new System.Drawing.Point(167, 3);
            this.vWavRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vWavRevBox.Name = "vWavRevBox";
            this.vWavRevBox.Size = new System.Drawing.Size(77, 20);
            this.vWavRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.vWavRevBox, "Revision version of the file.");
            this.vWavRevBox.ValueChanged += new System.EventHandler(this.vWavRevBox_ValueChanged);
            // 
            // vWavMinBox
            // 
            this.vWavMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vWavMinBox.Location = new System.Drawing.Point(85, 3);
            this.vWavMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vWavMinBox.Name = "vWavMinBox";
            this.vWavMinBox.Size = new System.Drawing.Size(76, 20);
            this.vWavMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.vWavMinBox, "Minor version of the file.");
            this.vWavMinBox.ValueChanged += new System.EventHandler(this.vWavMinBox_ValueChanged);
            // 
            // vWavMajBox
            // 
            this.vWavMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vWavMajBox.Location = new System.Drawing.Point(3, 3);
            this.vWavMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vWavMajBox.Name = "vWavMajBox";
            this.vWavMajBox.Size = new System.Drawing.Size(76, 20);
            this.vWavMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.vWavMajBox, "Major version of the file.");
            this.vWavMajBox.ValueChanged += new System.EventHandler(this.vWavMajBox_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(3, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(243, 23);
            this.label6.TabIndex = 2;
            this.label6.Text = "Internal Wave Version:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label6, "File version to save as.");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.vRevBoxWar, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.vMinBoxWar, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.vMajBoxWar, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-1, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(247, 26);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // vRevBoxWar
            // 
            this.vRevBoxWar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vRevBoxWar.Location = new System.Drawing.Point(167, 3);
            this.vRevBoxWar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vRevBoxWar.Name = "vRevBoxWar";
            this.vRevBoxWar.Size = new System.Drawing.Size(77, 20);
            this.vRevBoxWar.TabIndex = 2;
            this.toolTip.SetToolTip(this.vRevBoxWar, "Revision version of the file.");
            this.vRevBoxWar.ValueChanged += new System.EventHandler(this.vRevBoxWar_ValueChanged);
            // 
            // vMinBoxWar
            // 
            this.vMinBoxWar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMinBoxWar.Location = new System.Drawing.Point(85, 3);
            this.vMinBoxWar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMinBoxWar.Name = "vMinBoxWar";
            this.vMinBoxWar.Size = new System.Drawing.Size(76, 20);
            this.vMinBoxWar.TabIndex = 1;
            this.toolTip.SetToolTip(this.vMinBoxWar, "Minor version of the file.");
            this.vMinBoxWar.ValueChanged += new System.EventHandler(this.vMinBoxWar_ValueChanged);
            // 
            // vMajBoxWar
            // 
            this.vMajBoxWar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMajBoxWar.Location = new System.Drawing.Point(3, 3);
            this.vMajBoxWar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMajBoxWar.Name = "vMajBoxWar";
            this.vMajBoxWar.Size = new System.Drawing.Size(76, 20);
            this.vMajBoxWar.TabIndex = 0;
            this.toolTip.SetToolTip(this.vMajBoxWar, "Major version of the file.");
            this.vMajBoxWar.ValueChanged += new System.EventHandler(this.vMajBoxWar_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(243, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label5, "File version to save as.");
            // 
            // genericFileInfoPanel
            // 
            this.genericFileInfoPanel.Controls.Add(this.versionGrid);
            this.genericFileInfoPanel.Controls.Add(this.label3);
            this.genericFileInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genericFileInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.genericFileInfoPanel.Name = "genericFileInfoPanel";
            this.genericFileInfoPanel.Size = new System.Drawing.Size(246, 354);
            this.genericFileInfoPanel.TabIndex = 3;
            // 
            // versionGrid
            // 
            this.versionGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.versionGrid.ColumnCount = 3;
            this.versionGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.versionGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.versionGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.versionGrid.Controls.Add(this.vRevBox, 2, 0);
            this.versionGrid.Controls.Add(this.vMinBox, 1, 0);
            this.versionGrid.Controls.Add(this.vMajBox, 0, 0);
            this.versionGrid.Location = new System.Drawing.Point(-1, 26);
            this.versionGrid.Name = "versionGrid";
            this.versionGrid.RowCount = 1;
            this.versionGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.versionGrid.Size = new System.Drawing.Size(247, 26);
            this.versionGrid.TabIndex = 1;
            // 
            // vRevBox
            // 
            this.vRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vRevBox.Location = new System.Drawing.Point(167, 3);
            this.vRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vRevBox.Name = "vRevBox";
            this.vRevBox.Size = new System.Drawing.Size(77, 20);
            this.vRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.vRevBox, "Revision version of the file.");
            // 
            // vMinBox
            // 
            this.vMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMinBox.Location = new System.Drawing.Point(85, 3);
            this.vMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMinBox.Name = "vMinBox";
            this.vMinBox.Size = new System.Drawing.Size(76, 20);
            this.vMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.vMinBox, "Minor version of the file.");
            // 
            // vMajBox
            // 
            this.vMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMajBox.Location = new System.Drawing.Point(3, 3);
            this.vMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMajBox.Name = "vMajBox";
            this.vMajBox.Size = new System.Drawing.Size(76, 20);
            this.vMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.vMajBox, "Major version of the file.");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Version:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label3, "File version to save as.");
            // 
            // nullDataPanel
            // 
            this.nullDataPanel.Controls.Add(this.label4);
            this.nullDataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nullDataPanel.Location = new System.Drawing.Point(0, 0);
            this.nullDataPanel.Name = "nullDataPanel";
            this.nullDataPanel.Size = new System.Drawing.Size(246, 354);
            this.nullDataPanel.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(246, 354);
            this.label4.TabIndex = 0;
            this.label4.Text = "This data is currently null. Blank this data from the right click menu in order t" +
    "o create the data.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soundPlayerDeluxePanel
            // 
            this.soundPlayerDeluxePanel.Controls.Add(this.soundDeluxeTrack2);
            this.soundPlayerDeluxePanel.Controls.Add(this.soundDeluxeTrack1);
            this.soundPlayerDeluxePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundPlayerDeluxePanel.Location = new System.Drawing.Point(0, 0);
            this.soundPlayerDeluxePanel.Name = "soundPlayerDeluxePanel";
            this.soundPlayerDeluxePanel.Size = new System.Drawing.Size(246, 354);
            this.soundPlayerDeluxePanel.TabIndex = 4;
            this.soundPlayerDeluxePanel.Visible = false;
            // 
            // soundDeluxeTrack2
            // 
            this.soundDeluxeTrack2.ColumnCount = 2;
            this.soundDeluxeTrack2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundDeluxeTrack2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundDeluxeTrack2.Controls.Add(this.pauseSoundTrack, 0, 0);
            this.soundDeluxeTrack2.Controls.Add(this.stopSoundTrack, 1, 0);
            this.soundDeluxeTrack2.Dock = System.Windows.Forms.DockStyle.Top;
            this.soundDeluxeTrack2.Location = new System.Drawing.Point(0, 59);
            this.soundDeluxeTrack2.Name = "soundDeluxeTrack2";
            this.soundDeluxeTrack2.RowCount = 1;
            this.soundDeluxeTrack2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundDeluxeTrack2.Size = new System.Drawing.Size(246, 30);
            this.soundDeluxeTrack2.TabIndex = 9;
            // 
            // pauseSoundTrack
            // 
            this.pauseSoundTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pauseSoundTrack.Location = new System.Drawing.Point(3, 3);
            this.pauseSoundTrack.Name = "pauseSoundTrack";
            this.pauseSoundTrack.Size = new System.Drawing.Size(117, 24);
            this.pauseSoundTrack.TabIndex = 0;
            this.pauseSoundTrack.Text = "Pause";
            this.pauseSoundTrack.UseVisualStyleBackColor = true;
            this.pauseSoundTrack.Click += new System.EventHandler(this.pauseSoundTrack_Click);
            // 
            // stopSoundTrack
            // 
            this.stopSoundTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopSoundTrack.Location = new System.Drawing.Point(126, 3);
            this.stopSoundTrack.Name = "stopSoundTrack";
            this.stopSoundTrack.Size = new System.Drawing.Size(117, 24);
            this.stopSoundTrack.TabIndex = 1;
            this.stopSoundTrack.Text = "Stop";
            this.stopSoundTrack.UseVisualStyleBackColor = true;
            this.stopSoundTrack.Click += new System.EventHandler(this.stopSoundTrack_Click);
            // 
            // soundDeluxeTrack1
            // 
            this.soundDeluxeTrack1.ColumnCount = 1;
            this.soundDeluxeTrack1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundDeluxeTrack1.Controls.Add(this.playSoundTrack, 0, 1);
            this.soundDeluxeTrack1.Controls.Add(this.soundPlayerDeluxeLabel, 0, 0);
            this.soundDeluxeTrack1.Dock = System.Windows.Forms.DockStyle.Top;
            this.soundDeluxeTrack1.Location = new System.Drawing.Point(0, 0);
            this.soundDeluxeTrack1.Name = "soundDeluxeTrack1";
            this.soundDeluxeTrack1.RowCount = 2;
            this.soundDeluxeTrack1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundDeluxeTrack1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundDeluxeTrack1.Size = new System.Drawing.Size(246, 59);
            this.soundDeluxeTrack1.TabIndex = 11;
            // 
            // playSoundTrack
            // 
            this.playSoundTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playSoundTrack.Location = new System.Drawing.Point(3, 32);
            this.playSoundTrack.Name = "playSoundTrack";
            this.playSoundTrack.Size = new System.Drawing.Size(240, 24);
            this.playSoundTrack.TabIndex = 10;
            this.playSoundTrack.Text = "Play";
            this.playSoundTrack.UseVisualStyleBackColor = true;
            this.playSoundTrack.Click += new System.EventHandler(this.playSoundTrack_Click);
            // 
            // soundPlayerDeluxeLabel
            // 
            this.soundPlayerDeluxeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundPlayerDeluxeLabel.Location = new System.Drawing.Point(3, 0);
            this.soundPlayerDeluxeLabel.Name = "soundPlayerDeluxeLabel";
            this.soundPlayerDeluxeLabel.Size = new System.Drawing.Size(240, 29);
            this.soundPlayerDeluxeLabel.TabIndex = 11;
            this.soundPlayerDeluxeLabel.Text = "Sound Player Deluxe™";
            this.soundPlayerDeluxeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nullFilePanel
            // 
            this.nullFilePanel.Controls.Add(this.label2);
            this.nullFilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nullFilePanel.Location = new System.Drawing.Point(0, 0);
            this.nullFilePanel.Name = "nullFilePanel";
            this.nullFilePanel.Size = new System.Drawing.Size(246, 354);
            this.nullFilePanel.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 354);
            this.label2.TabIndex = 0;
            this.label2.Text = "This file is currently null. Blank this file from the edit menu in order to creat" +
    "e a file.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // noInfoPanel
            // 
            this.noInfoPanel.Controls.Add(this.label1);
            this.noInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.noInfoPanel.Name = "noInfoPanel";
            this.noInfoPanel.Size = new System.Drawing.Size(246, 354);
            this.noInfoPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 354);
            this.label1.TabIndex = 0;
            this.label1.Text = "No Valid Info Selected!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeIcons;
            this.tree.Indent = 12;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            treeNode2.ImageIndex = 10;
            treeNode2.Name = "fileInfo";
            treeNode2.SelectedImageIndex = 10;
            treeNode2.Text = "File Information";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.tree.SelectedImageIndex = 0;
            this.tree.ShowLines = false;
            this.tree.Size = new System.Drawing.Size(490, 354);
            this.tree.TabIndex = 0;
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseClick);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            this.tree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tree_NodeKey);
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
            // openFileDialog
            // 
            this.openFileDialog.RestoreDirectory = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip.Location = new System.Drawing.Point(0, 380);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(744, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(125, 17);
            this.status.Text = "No Valid Info Selected!";
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
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("expandToolStripMenuItem.Image")));
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.expandToolStripMenuItem.Text = "Expand";
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("collapseToolStripMenuItem.Image")));
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.collapseToolStripMenuItem.Text = "Collapse";
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // nodeMenu
            // 
            this.nodeMenu.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nodeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAboveToolStripMenuItem1,
            this.addBelowToolStripMenuItem1,
            this.moveUpToolStripMenuItem1,
            this.moveDownToolStripMenuItem1,
            this.blankToolStripMenuItem,
            this.replaceFileToolStripMenuItem,
            this.exportToolStripMenuItem1,
            this.nullifyToolStripMenuItem1,
            this.deleteToolStripMenuItem1});
            this.nodeMenu.Name = "contextMenuStrip1";
            this.nodeMenu.Size = new System.Drawing.Size(139, 202);
            // 
            // addAboveToolStripMenuItem1
            // 
            this.addAboveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("addAboveToolStripMenuItem1.Image")));
            this.addAboveToolStripMenuItem1.Name = "addAboveToolStripMenuItem1";
            this.addAboveToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.addAboveToolStripMenuItem1.Text = "Add Above";
            this.addAboveToolStripMenuItem1.Click += new System.EventHandler(this.addAboveToolStripMenuItem1_Click);
            // 
            // addBelowToolStripMenuItem1
            // 
            this.addBelowToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("addBelowToolStripMenuItem1.Image")));
            this.addBelowToolStripMenuItem1.Name = "addBelowToolStripMenuItem1";
            this.addBelowToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.addBelowToolStripMenuItem1.Text = "Add Below";
            this.addBelowToolStripMenuItem1.Click += new System.EventHandler(this.addBelowToolStripMenuItem1_Click);
            // 
            // moveUpToolStripMenuItem1
            // 
            this.moveUpToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("moveUpToolStripMenuItem1.Image")));
            this.moveUpToolStripMenuItem1.Name = "moveUpToolStripMenuItem1";
            this.moveUpToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem1.Text = "Move Up";
            this.moveUpToolStripMenuItem1.Click += new System.EventHandler(this.moveUpToolStripMenuItem1_Click);
            // 
            // moveDownToolStripMenuItem1
            // 
            this.moveDownToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("moveDownToolStripMenuItem1.Image")));
            this.moveDownToolStripMenuItem1.Name = "moveDownToolStripMenuItem1";
            this.moveDownToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem1.Text = "Move Down";
            this.moveDownToolStripMenuItem1.Click += new System.EventHandler(this.moveDownToolStripMenuItem1_Click);
            // 
            // blankToolStripMenuItem
            // 
            this.blankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("blankToolStripMenuItem.Image")));
            this.blankToolStripMenuItem.Name = "blankToolStripMenuItem";
            this.blankToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.blankToolStripMenuItem.Text = "Blank";
            this.blankToolStripMenuItem.Click += new System.EventHandler(this.blankToolStripMenuItem_Click);
            // 
            // replaceFileToolStripMenuItem
            // 
            this.replaceFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("replaceFileToolStripMenuItem.Image")));
            this.replaceFileToolStripMenuItem.Name = "replaceFileToolStripMenuItem";
            this.replaceFileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.replaceFileToolStripMenuItem.Text = "Replace";
            this.replaceFileToolStripMenuItem.Click += new System.EventHandler(this.replaceFileToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem1
            // 
            this.exportToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem1.Image")));
            this.exportToolStripMenuItem1.Name = "exportToolStripMenuItem1";
            this.exportToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.exportToolStripMenuItem1.Text = "Export";
            this.exportToolStripMenuItem1.Click += new System.EventHandler(this.exportToolStripMenuItem1_Click);
            // 
            // nullifyToolStripMenuItem1
            // 
            this.nullifyToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("nullifyToolStripMenuItem1.Image")));
            this.nullifyToolStripMenuItem1.Name = "nullifyToolStripMenuItem1";
            this.nullifyToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.nullifyToolStripMenuItem1.Text = "Nullify";
            this.nullifyToolStripMenuItem1.Click += new System.EventHandler(this.nullifyToolStripMenuItem1_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem1.Image")));
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // EditorBase
            // 
            this.ClientSize = new System.Drawing.Size(744, 402);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "EditorBase";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_Close);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.warFileInfoPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vWavRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vWavMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vWavMajBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vRevBoxWar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMinBoxWar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMajBoxWar)).EndInit();
            this.genericFileInfoPanel.ResumeLayout(false);
            this.versionGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMajBox)).EndInit();
            this.nullDataPanel.ResumeLayout(false);
            this.soundPlayerDeluxePanel.ResumeLayout(false);
            this.soundDeluxeTrack2.ResumeLayout(false);
            this.soundDeluxeTrack1.ResumeLayout(false);
            this.nullFilePanel.ResumeLayout(false);
            this.noInfoPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.rootMenu.ResumeLayout(false);
            this.nodeMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        /// <summary>
        /// Get the path for opening a file.
        /// </summary>
        /// <param name="description">File description.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Path of the file to open.</returns>
        public string GetFileOpenerPath(string description, string extension) {

            //Set filer.
            openFileDialog.FileName = "";
            openFileDialog.Filter = description + "|" + "*.bf" + extension.ToLower() + ";*.bc" + extension.ToLower();
            openFileDialog.ShowDialog();

            //Return the file name.
            return openFileDialog.FileName;

        }

        /// <summary>
        /// Get the path for saving a file.
        /// </summary>
        /// <param name="description">File description.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Path of the file to save.</returns>
        public string GetFileSaverPath(string description, string extension) {

            //Set filer.
            saveFileDialog.FileName = "";
            saveFileDialog.Filter = description + " (3ds or Wii U)|" + "*.bf" + extension.ToLower() + ";*.bc" + extension.ToLower() + "|" + description + " (Switch)|*.bf" + extension.ToLower();
            saveFileDialog.ShowDialog();

            //Set write mode.
            if (saveFileDialog.FileName != "") {

                //3ds or WiiU.
                if (saveFileDialog.FilterIndex == 0) {

                    //3ds.
                    if (Path.GetExtension(saveFileDialog.FileName).StartsWith("bc")) {
                        WriteMode = WriteMode.CTR;
                    }

                    //WiiU.
                    else if (Path.GetExtension(saveFileDialog.FileName).StartsWith("bf")) {
                        WriteMode = WriteMode.Cafe;
                    }

                }

                //Switch.
                else {

                    //Switch.
                    if (Path.GetExtension(saveFileDialog.FileName).StartsWith("bf")) {
                        WriteMode = WriteMode.NX;
                    }

                }

            }

            //Return the file name.
            return saveFileDialog.FileName;

        }


        private void form_Close(object sender, FormClosingEventArgs e) {
            OnClosing();
        }

        /// <summary>
        /// On closing.
        /// </summary>
        public virtual void OnClosing() {}

        //Updating.
        #region Updating

        /// <summary>
        /// Do the info stuff on node selected.
        /// </summary>
        public virtual void DoInfoStuff() {

            //Fix selected node issue.
            if (tree.SelectedNode == null) {
                tree.SelectedNode = tree.Nodes[0];
            }

            //File open.
            if (FileOpen) {

                //File is null.
                if (File == null) {

                    //Show null info panel.
                    nullFilePanel.BringToFront();
                    nullFilePanel.Show();

                    //Update status.
                    status.Text = "No Valid Info Selected!";

                }

                //File is valid.
                else {

                    //If the file info node is selected.
                    if (tree.SelectedNode.Index == 0 && tree.SelectedNode.Parent == null) {

                        //Display the file info.
                        genericFileInfoPanel.BringToFront();
                        genericFileInfoPanel.Show();

                        //Update status.
                        status.Text = "No Valid Info Selected!";

                    }

                }

            }

            //No file open.
            else {

                //Show no info panel.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

                //Update status.
                status.Text = "No Valid Info Selected!";

            }

        }

        Stack<int> nodeIndices;
        List<string> expandedNodes;

        /// <summary>
        /// Begin the updating of nodes.
        /// </summary>
        public void BeginUpdateNodes() {

            //Begin update.
            tree.BeginUpdate();

            //Get list of expanded nodes.
            expandedNodes = collectExpandedNodes(tree.Nodes);

            //Safety.
            if (tree.SelectedNode == null) {
                tree.SelectedNode = tree.Nodes[0];
            }

            //Get the selected index.
            nodeIndices = new Stack<int>();
            nodeIndices.Push(tree.SelectedNode.Index);
            while (tree.SelectedNode.Parent != null) {
                tree.SelectedNode = tree.SelectedNode.Parent;
                nodeIndices.Push(tree.SelectedNode.Index);
            }

            //Clear each node.
            for (int i = 1; i < tree.Nodes.Count; i++) {
                tree.Nodes[i].Nodes.Clear();
            }

        }

        /// <summary>
        /// Update the nodes in the tree. THIS MUST CONTAIN THE BEGIN AND END UPDATE NODES!
        /// </summary>
        public abstract void UpdateNodes();

        /// <summary>
        /// Complete the updating of nodes.
        /// </summary>
        public void EndUpdateNodes() {

            //Restore the expanded nodes if they exist.
            if (expandedNodes.Count > 0) {
                TreeNode IamExpandedNode;
                for (int i = 0; i < expandedNodes.Count; i++) {
                    IamExpandedNode = FindNodeByName(tree.Nodes, expandedNodes[i]);
                    expandNodePath(IamExpandedNode);
                }

            }

            //Set the selected node.
            tree.SelectedNode = tree.Nodes[nodeIndices.Pop()];
            while (nodeIndices.Count > 0) {
                try {
                    tree.SelectedNode = tree.SelectedNode.Nodes[nodeIndices.Pop()];
                } catch {
                    nodeIndices.Clear();
                }
            }
            tree.SelectedNode.EnsureVisible();

            //End update.
            tree.EndUpdate();

        }

        #endregion


        //File menu.
        #region fileMenu

        //New.
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open and can save test.
            if (!FileTest(sender, e, true)) {
                return;
            }

            //Create instance of file.
            File = (ISoundFile)Activator.CreateInstance(FileType);

            //Reset values.
            FilePath = "";
            FileOpen = true;
            ExtFile = null;
            Text = EditorName + " - New " + ExtensionDescription + ".bf" + Extension;

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        //Open.
        private void openToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open and save test.
            if (!FileTest(sender, e, true)) {
                return;
            }

            //Open the file.
            string path = GetFileOpenerPath(ExtensionDescription, Extension);

            //File is not null.
            if (path != "") {

                //Set value.
                File = (ISoundFile)Activator.CreateInstance(FileType);
                ExtFile = null;
                FilePath = path;
                Text = EditorName + " - " + Path.GetFileName(path);
                FileOpen = true;

                //Read data.
                MemoryStream src = new MemoryStream(System.IO.File.ReadAllBytes(path));
                BinaryDataReader br = new BinaryDataReader(src);
                File.Read(br);

                //Update.
                UpdateNodes();
                DoInfoStuff();

            }

        }

        //Save.
        public void saveToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //No where to save.
            if (ExtFile == null && FilePath == "") {

                //Save as.
                saveAsToolStripMenuItem_Click(sender, e);

                //Return.
                return;

            }

            //External file is not null.
            if (ExtFile != null) {

                //Set the file.
                ExtFile.File = File;

                //Update the main window.
                MainWindow.UpdateNodes();
                MainWindow.doInfoPanelStuff();

            }

            //External file is null.
            else {

                //Write the file.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                //Write the file.
                File.Write(WriteMode, bw);

                //Save the file.
                System.IO.File.WriteAllBytes(FilePath, o.ToArray());

            }

        }

        //Save as.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //Get the save file name.
            string path = GetFileSaverPath(ExtensionDescription, Extension);

            //If the path is valid.
            if (path != "") {

                //Set values.
                FilePath = path;
                ExtFile = null;
                Text = EditorName + " - " + Path.GetFileName(path);

                //Save the file.
                saveToolStripMenuItem_Click(sender, e);

            }

        }

        //Close.
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open and can save test.
            if (!FileTest(sender, e, true, true)) {
                return;
            }

            //Close the file.
            File = (ISoundFile)Activator.CreateInstance(FileType);
            ExtFile = null;
            FilePath = "";
            FileOpen = false;
            Text = EditorName;

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        //Quit.
        private void quitToolStripMenuItem_Click(object sender, EventArgs e) {

            //Quit if wanted.
            if (FileOpen) {
                SaveQuitDialog q = new SaveQuitDialog(this);
                q.ShowDialog();
            } else {
                Close();
            }

        }

        #endregion


        //Edit menu.
        #region editMenu

        //Blank the file.
        private void blankFileToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open save test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //Create instance of file.
            File = (ISoundFile)Activator.CreateInstance(FileType);

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        //Import data from another file.
        private void importFileToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //Open the file.
            string path = GetFileOpenerPath(ExtensionDescription, Extension);

            //File is not null.
            if (path != "") {

                //Set value.
                File = (ISoundFile)Activator.CreateInstance(FileType);

                //Read data.
                MemoryStream src = new MemoryStream(System.IO.File.ReadAllBytes(path));
                BinaryDataReader br = new BinaryDataReader(src);
                File.Read(br);

                //Update.
                UpdateNodes();
                DoInfoStuff();

            }

        }

        //Export data to somewhere.
        private void exportFileToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //Get the save file name.
            string path = GetFileSaverPath(ExtensionDescription, Extension);

            //If the path is valid.
            if (path != "") {

                //Save the file.
                saveToolStripMenuItem_Click(sender, e);

            }

        }

        //Set file data to null.
        private void nullifyFileToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //External file cannot be null.
            if (ExtFile == null) {

                //Call out the user.
                MessageBox.Show("You can't nullify data that is not in a parent file!", "Notice:");
                return;

            }

            //Nullify file.
            File = null;

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        /// <summary>
        /// Returns true if the user wants to continue.
        /// </summary>
        public bool FileTest(object sender, EventArgs e, bool save, bool forceOpen = false) {

            //File is open.
            if (FileOpen) {

                //Ask user if they want to save.
                if (save) {

                    SaveCloseDialog c = new SaveCloseDialog();
                    switch (c.getValue()) {

                        //Save.
                        case 0:
                            saveToolStripMenuItem_Click(sender, e);
                            return true;

                        //No button.
                        case 1:
                            return true;

                        //Cancel.
                        default:
                            return false;

                    }

                }

                //Passed test.
                return true;

            } else {

                if (forceOpen) {
                    MessageBox.Show("There must be a file open to do this!", "Notice:");
                    return false;
                } else {
                    return true;
                }

            }

        }

        #endregion


        //Node shit.
        #region nodeShit

        //Expand node and parents.
        void expandNodePath(TreeNode node) {
            if (node == null)
                return;
            if (node.Level != 0) //check if it is not root
            {
                node.Expand();
                expandNodePath(node.Parent);
            } else {
                node.Expand(); // this is root 
            }



        }

        //Make right click actually select, and show infoViewer.
        void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                // Select the clicked node
                tree.SelectedNode = tree.GetNodeAt(e.X, e.Y);
            } else if (e.Button == MouseButtons.Left) {
                // Select the clicked node
                tree.SelectedNode = tree.GetNodeAt(e.X, e.Y);
            }

            DoInfoStuff();

        }

        void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {

            //Select.
            if (e.Button == MouseButtons.Right) {
                // Select the clicked node
                tree.SelectedNode = tree.GetNodeAt(e.X, e.Y);
            } else if (e.Button == MouseButtons.Left) {
                // Select the clicked node
                tree.SelectedNode = tree.GetNodeAt(e.X, e.Y);
            }

            //Do info stuff.
            DoInfoStuff();

            //Do double click action.
            NodeMouseDoubleClick();

        }

        public virtual void NodeMouseDoubleClick() {

        }

        void tree_NodeKey(object sender, KeyEventArgs e) {

            DoInfoStuff();

        }

        //Get expanded nodes.
        List<string> collectExpandedNodes(TreeNodeCollection Nodes) {
            List<string> _lst = new List<string>();
            foreach (TreeNode checknode in Nodes) {
                if (checknode.IsExpanded)
                    _lst.Add(checknode.Name);
                if (checknode.Nodes.Count > 0)
                    _lst.AddRange(collectExpandedNodes(checknode.Nodes));
            }
            return _lst;
        }


        /// <summary>
        /// Find nodes by name.
        /// </summary>
        /// <param name="NodesCollection"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        TreeNode FindNodeByName(TreeNodeCollection NodesCollection, string Name) {
            TreeNode returnNode = null; // Default value to return
            foreach (TreeNode checkNode in NodesCollection) {
                if (checkNode.Name == Name)  //checks if this node name is correct
                    returnNode = checkNode;
                else if (checkNode.Nodes.Count > 0) //node has child
                {
                    returnNode = FindNodeByName(checkNode.Nodes, Name);
                }

                if (returnNode != null) //check if founded do not continue and break
                {
                    return returnNode;
                }

            }
            //not found
            return returnNode;
        }

        #endregion


        //Sound player deluxe.
        #region soundPlayerDeluxe

        //Play.
        private void playSoundTrack_Click(object sender, EventArgs e) {
            Play();
        }

        //Pause.
        private void pauseSoundTrack_Click(object sender, EventArgs e) {
            Pause();
        }

        //Stop.
        private void stopSoundTrack_Click(object sender, EventArgs e) {
            Stop();
        }

        public virtual void Play() {}
        public virtual void Pause() {}
        public virtual void Stop() {}

        #endregion


        //Other buttons.
        #region otherButtons

        private void forceWaveVersionButton_Click(object sender, EventArgs e) {
            ForceWaveVersionButtonClick();
        }

        public virtual void ForceWaveVersionButtonClick() {}

        #endregion


        //Root menu.
        #region rootMenu

        private void addToolStripMenuItem_Click(object sender, EventArgs e) {
            RootAdd();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e) {
            tree.SelectedNode.Expand();
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e) {
            tree.SelectedNode.Collapse();
        }

        public virtual void RootAdd() {}

        #endregion


        //Node menu.
        #region nodeMenu

        private void addAboveToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeAddAbove();
        }

        private void addBelowToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeAddBelow();
        }

        private void moveUpToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeMoveUp();
        }

        private void moveDownToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeMoveDown();
        }

        private void blankToolStripMenuItem_Click(object sender, EventArgs e) {
            NodeBlank();
        }

        private void replaceFileToolStripMenuItem_Click(object sender, EventArgs e) {
            NodeReplace();
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeExport();
        }

        private void nullifyToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeNullify();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeDelete();
        }

        public virtual void NodeAddAbove() {}
        public virtual void NodeAddBelow() {}
        public virtual void NodeMoveUp() {}
        public virtual void NodeMoveDown() {}
        public virtual void NodeBlank() {}
        public virtual void NodeReplace() {}
        public virtual void NodeExport() {}
        public virtual void NodeNullify() {}
        public virtual void NodeDelete() {}

        /// <summary>
        /// Swap the a and b objects.
        /// </summary>
        /// <param name="objects">Objects list.</param>
        /// <param name="a">Object a to swap.</param>
        /// <param name="b">Object b to swap.</param>
        public bool Swap<T>(IList<T> objects, int a, int b) {

            //Make sure it is possible.
            if (a < 0 || a >= objects.Count || b < 0 || b >= objects.Count) {
                return false;
            }

            //Swap objects.
            T temp = objects[a];
            objects[a] = objects[b];
            objects[b] = temp;
            return true;

        }

        #endregion


        //War boxes.
        #region warBoxes

        private void vMajBoxWar_ValueChanged(object sender, EventArgs e) {
            BoxWarMajChanged();
        }

        private void vMinBoxWar_ValueChanged(object sender, EventArgs e) {
            BoxWarMinChanged();
        }

        private void vRevBoxWar_ValueChanged(object sender, EventArgs e) {
            BoxWarRevChanged();
        }

        private void vWavMajBox_ValueChanged(object sender, EventArgs e) {
            BoxWavMajChanged();
        }

        private void vWavMinBox_ValueChanged(object sender, EventArgs e) {
            BoxWavMinChanged();
        }

        private void vWavRevBox_ValueChanged(object sender, EventArgs e) {
            BoxWavRevChanged();
        }

        public virtual void BoxWarMajChanged() {}
        public virtual void BoxWarMinChanged() {}
        public virtual void BoxWarRevChanged() {}
        public virtual void BoxWavMajChanged() {}
        public virtual void BoxWavMinChanged() {}
        public virtual void BoxWavRevChanged() {}

        #endregion


        //War tools.
        #region warTools

        private void batchExtractWavesToolStripMenuItem_Click(object sender, EventArgs e) {
            WarExtractWave();
        }

        private void batchExtract3dsWavesToolStripMenuItem_Click(object sender, EventArgs e) {
            WarExtractWave3ds();
        }

        private void batchExtractWiiUWavesToolStripMenuItem_Click(object sender, EventArgs e) {
            WarExtractWaveWiiU();
        }

        private void batchExtractSwitchWavesToolStripMenuItem_Click(object sender, EventArgs e) {
            WarExtractWaveSwitch();
        }

        private void batchImportToolStripMenuItem_Click(object sender, EventArgs e) {
            WarBatchImport();
        }

        public virtual void WarExtractWave() {}
        public virtual void WarExtractWave3ds() {}
        public virtual void WarExtractWaveWiiU() {}
        public virtual void WarExtractWaveSwitch() {}
        public virtual void WarBatchImport() {}

        #endregion

    }

}
