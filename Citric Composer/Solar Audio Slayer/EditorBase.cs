using Citric_Composer;
using SolarFileLoader;
using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolarAudioSlayer {

    /// <summary>
    /// An editor base for files.
    /// </summary>
    public abstract class EditorBase : Form {

        /// <summary>
        /// File type to use.
        /// </summary>
        public AudioFile File;

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
        /// Other editor.
        /// </summary>
        public EditorBase OtherEditor;

        /// <summary>
        /// Writing info.
        /// </summary>
        public bool WritingInfo;

        /// <summary>
        /// External file in the B_SAR.
        /// </summary>
        public AudioFile ExtFile;


        /// <summary>
        /// Constructor.
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
        /// Constructor.
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
            File = (AudioFile)Activator.CreateInstance(FileType);
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
        /// Constructor.
        /// </summary>
        /// <param name="fileType">Type of file.</param>
        /// <param name="extensionDescription">Description of the extension.</param>
        /// <param name="extension">File extension type.</param>
        /// <param name="editorName">Editor name.</param>
        /// <param name="fileToOpen">File to open.</param>
        /// <param name="mainWindow">Main window.</param>
        public EditorBase(Type fileType, string extensionDescription, string extension, string editorName, AudioFile fileToOpen, MainWindow mainWindow) {

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
            File = ExtFile;
            FilePath = "";
            string name = "ExtFile";
            if (name == null) {
                name = "{ Null File Name }";
            }
            Text = EditorName + " - " + name + "." + ExtFile;
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
        public ImageList treeIcons;
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
        public Panel barslistFileInfo;
        private Label label6;
        private TableLayoutPanel tableLayoutPanel1;
        public NumericUpDown vMinBoxLst;
        public NumericUpDown vMajBoxLst;
        private Label label5;
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
        public Panel grpFileInfoPanel;
        public Button grpWarForceButton;
        private TableLayoutPanel tableLayoutPanel6;
        public NumericUpDown grpWarRevBox;
        public NumericUpDown grpWarMinBox;
        public NumericUpDown grpWarMajBox;
        private Label label10;
        public Button grpBnkForceButton;
        private TableLayoutPanel tableLayoutPanel5;
        public NumericUpDown grpBnkRevBox;
        public NumericUpDown grpBnkMinBox;
        public NumericUpDown grpBnkMajBox;
        private Label label9;
        public Button grpSeqForceButton;
        private TableLayoutPanel tableLayoutPanel3;
        public NumericUpDown grpSeqRevBox;
        public NumericUpDown grpSeqMinBox;
        public NumericUpDown grpSeqMajBox;
        private Label label7;
        private TableLayoutPanel tableLayoutPanel4;
        public NumericUpDown grpRevBox;
        public NumericUpDown grpMinBox;
        public NumericUpDown grpMajBox;
        private Label label8;
        public Button grpWsdForceButton;
        private TableLayoutPanel tableLayoutPanel7;
        public NumericUpDown grpWsdRevBox;
        public NumericUpDown grpWsdMinBox;
        public NumericUpDown grpWsdMajBox;
        private Label label11;
        public Button grpStpForceButton;
        private TableLayoutPanel tableLayoutPanel8;
        public NumericUpDown grpStpRevBox;
        public NumericUpDown grpStpMinBox;
        public NumericUpDown grpStpMajBox;
        private Label label12;
        public Panel grpFilePanel;
        private Label label13;
        public ComboBox grpFileIdComboBox;
        public NumericUpDown grpFileIdBox;
        private Label label14;
        public ComboBox grpEmbedModeBox;
        public Panel grpDependencyPanel;
        private Label label15;
        public ComboBox grpDepEntryTypeBox;
        public ComboBox grpDepEntryNumComboBox;
        private Label label16;
        public NumericUpDown grpDepEntryNumBox;
        private Label label17;
        public ComboBox grpDepLoadFlagsBox;
        public ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem citricComposerToolStripMenuItem;
        private ToolStripMenuItem isabelleSoundEditorToolStripMenuItem;
        private ToolStripMenuItem resourceEditorToolStripMenuItem;
        private ToolStripMenuItem attenuationArchiveEditorATTARCToolStripMenuItem;
        private ToolStripMenuItem audioGroupSettingsEditorBAGSTToolStripMenuItem;
        private ToolStripMenuItem rescourceListEditorBARSLISTToolStripMenuItem;
        private ToolStripMenuItem attenuationEditorBAATNToolStripMenuItem;
        private ToolStripMenuItem customCurveEditorBACTCToolStripMenuItem;
        private ToolStripMenuItem unitDistanceCurveEditorBAUDCToolStripMenuItem;
        private ToolStripMenuItem rollOffCurveEditorBAROCToolStripMenuItem;
        private ToolStripMenuItem loopAssetListGeneratorBLALToolStripMenuItem;
        public TextBox lstInternalAssetName;

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
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.citricComposerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isabelleSoundEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescourceListEditorBARSLISTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resourceEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioGroupSettingsEditorBAGSTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attenuationArchiveEditorATTARCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attenuationEditorBAATNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollOffCurveEditorBAROCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customCurveEditorBACTCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unitDistanceCurveEditorBAUDCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loopAssetListGeneratorBLALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.barslistFileInfo = new System.Windows.Forms.Panel();
            this.lstInternalAssetName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.vMinBoxLst = new System.Windows.Forms.NumericUpDown();
            this.vMajBoxLst = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.grpDependencyPanel = new System.Windows.Forms.Panel();
            this.grpDepLoadFlagsBox = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.grpDepEntryNumBox = new System.Windows.Forms.NumericUpDown();
            this.grpDepEntryNumComboBox = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.grpDepEntryTypeBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.grpFilePanel = new System.Windows.Forms.Panel();
            this.grpEmbedModeBox = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.grpFileIdBox = new System.Windows.Forms.NumericUpDown();
            this.grpFileIdComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.grpFileInfoPanel = new System.Windows.Forms.Panel();
            this.grpStpForceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.grpStpRevBox = new System.Windows.Forms.NumericUpDown();
            this.grpStpMinBox = new System.Windows.Forms.NumericUpDown();
            this.grpStpMajBox = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.grpWsdForceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.grpWsdRevBox = new System.Windows.Forms.NumericUpDown();
            this.grpWsdMinBox = new System.Windows.Forms.NumericUpDown();
            this.grpWsdMajBox = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.grpWarForceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.grpWarRevBox = new System.Windows.Forms.NumericUpDown();
            this.grpWarMinBox = new System.Windows.Forms.NumericUpDown();
            this.grpWarMajBox = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.grpBnkForceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.grpBnkRevBox = new System.Windows.Forms.NumericUpDown();
            this.grpBnkMinBox = new System.Windows.Forms.NumericUpDown();
            this.grpBnkMajBox = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.grpSeqForceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grpSeqRevBox = new System.Windows.Forms.NumericUpDown();
            this.grpSeqMinBox = new System.Windows.Forms.NumericUpDown();
            this.grpSeqMajBox = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.grpRevBox = new System.Windows.Forms.NumericUpDown();
            this.grpMinBox = new System.Windows.Forms.NumericUpDown();
            this.grpMajBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
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
            this.barslistFileInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vMinBoxLst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMajBoxLst)).BeginInit();
            this.grpDependencyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpDepEntryNumBox)).BeginInit();
            this.grpFilePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpFileIdBox)).BeginInit();
            this.grpFileInfoPanel.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpStpRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpStpMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpStpMajBox)).BeginInit();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpWsdRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWsdMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWsdMajBox)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpWarRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWarMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWarMajBox)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBnkRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBnkMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBnkMajBox)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpSeqRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSeqMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSeqMajBox)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpRevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpMajBox)).BeginInit();
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
            this.toolsWarToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(917, 24);
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
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            this.blankFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.blankFileToolStripMenuItem.Text = "Blank File";
            this.blankFileToolStripMenuItem.Click += new System.EventHandler(this.blankFileToolStripMenuItem_Click);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importFileToolStripMenuItem.Image")));
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importFileToolStripMenuItem.Text = "Import File";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // exportFileToolStripMenuItem
            // 
            this.exportFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportFileToolStripMenuItem.Image")));
            this.exportFileToolStripMenuItem.Name = "exportFileToolStripMenuItem";
            this.exportFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportFileToolStripMenuItem.Text = "Export File";
            this.exportFileToolStripMenuItem.Click += new System.EventHandler(this.exportFileToolStripMenuItem_Click);
            // 
            // nullifyFileToolStripMenuItem
            // 
            this.nullifyFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nullifyFileToolStripMenuItem.Image")));
            this.nullifyFileToolStripMenuItem.Name = "nullifyFileToolStripMenuItem";
            this.nullifyFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.citricComposerToolStripMenuItem,
            this.isabelleSoundEditorToolStripMenuItem,
            this.rescourceListEditorBARSLISTToolStripMenuItem,
            this.resourceEditorToolStripMenuItem,
            this.audioGroupSettingsEditorBAGSTToolStripMenuItem,
            this.attenuationArchiveEditorATTARCToolStripMenuItem,
            this.attenuationEditorBAATNToolStripMenuItem,
            this.rollOffCurveEditorBAROCToolStripMenuItem,
            this.customCurveEditorBACTCToolStripMenuItem,
            this.unitDistanceCurveEditorBAUDCToolStripMenuItem,
            this.loopAssetListGeneratorBLALToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.Visible = false;
            // 
            // citricComposerToolStripMenuItem
            // 
            this.citricComposerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("citricComposerToolStripMenuItem.Image")));
            this.citricComposerToolStripMenuItem.Name = "citricComposerToolStripMenuItem";
            this.citricComposerToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.citricComposerToolStripMenuItem.Text = "Citric Composer";
            this.citricComposerToolStripMenuItem.Click += new System.EventHandler(this.citricComposerToolStripMenuItem_Click);
            // 
            // isabelleSoundEditorToolStripMenuItem
            // 
            this.isabelleSoundEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("isabelleSoundEditorToolStripMenuItem.Image")));
            this.isabelleSoundEditorToolStripMenuItem.Name = "isabelleSoundEditorToolStripMenuItem";
            this.isabelleSoundEditorToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.isabelleSoundEditorToolStripMenuItem.Text = "Isabelle Sound Editor (WAV, STM)";
            this.isabelleSoundEditorToolStripMenuItem.Click += new System.EventHandler(this.isabelleSoundEditorToolStripMenuItem_Click);
            // 
            // rescourceListEditorBARSLISTToolStripMenuItem
            // 
            this.rescourceListEditorBARSLISTToolStripMenuItem.Name = "rescourceListEditorBARSLISTToolStripMenuItem";
            this.rescourceListEditorBARSLISTToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.rescourceListEditorBARSLISTToolStripMenuItem.Text = "Rescource List Editor (BARSLIST)";
            this.rescourceListEditorBARSLISTToolStripMenuItem.Click += new System.EventHandler(this.rescourceListEditorBARSLISTToolStripMenuItem_Click);
            // 
            // resourceEditorToolStripMenuItem
            // 
            this.resourceEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resourceEditorToolStripMenuItem.Image")));
            this.resourceEditorToolStripMenuItem.Name = "resourceEditorToolStripMenuItem";
            this.resourceEditorToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.resourceEditorToolStripMenuItem.Text = "Resource Editor (BARS)";
            this.resourceEditorToolStripMenuItem.Click += new System.EventHandler(this.resourceEditorToolStripMenuItem_Click);
            // 
            // audioGroupSettingsEditorBAGSTToolStripMenuItem
            // 
            this.audioGroupSettingsEditorBAGSTToolStripMenuItem.Name = "audioGroupSettingsEditorBAGSTToolStripMenuItem";
            this.audioGroupSettingsEditorBAGSTToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.audioGroupSettingsEditorBAGSTToolStripMenuItem.Text = "Group Settings Editor (BAGST)";
            // 
            // attenuationArchiveEditorATTARCToolStripMenuItem
            // 
            this.attenuationArchiveEditorATTARCToolStripMenuItem.Name = "attenuationArchiveEditorATTARCToolStripMenuItem";
            this.attenuationArchiveEditorATTARCToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.attenuationArchiveEditorATTARCToolStripMenuItem.Text = "Attenuation Archive Editor (BATTARC)";
            // 
            // attenuationEditorBAATNToolStripMenuItem
            // 
            this.attenuationEditorBAATNToolStripMenuItem.Name = "attenuationEditorBAATNToolStripMenuItem";
            this.attenuationEditorBAATNToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.attenuationEditorBAATNToolStripMenuItem.Text = "Attenuation Editor (BAATN)";
            // 
            // rollOffCurveEditorBAROCToolStripMenuItem
            // 
            this.rollOffCurveEditorBAROCToolStripMenuItem.Name = "rollOffCurveEditorBAROCToolStripMenuItem";
            this.rollOffCurveEditorBAROCToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.rollOffCurveEditorBAROCToolStripMenuItem.Text = "Roll Off Curve Editor (BAROC)";
            // 
            // customCurveEditorBACTCToolStripMenuItem
            // 
            this.customCurveEditorBACTCToolStripMenuItem.Name = "customCurveEditorBACTCToolStripMenuItem";
            this.customCurveEditorBACTCToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.customCurveEditorBACTCToolStripMenuItem.Text = "Custom Curve Editor (BACTC)";
            // 
            // unitDistanceCurveEditorBAUDCToolStripMenuItem
            // 
            this.unitDistanceCurveEditorBAUDCToolStripMenuItem.Name = "unitDistanceCurveEditorBAUDCToolStripMenuItem";
            this.unitDistanceCurveEditorBAUDCToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.unitDistanceCurveEditorBAUDCToolStripMenuItem.Text = "Unit Distance Curve Editor (BAUDC)";
            // 
            // loopAssetListGeneratorBLALToolStripMenuItem
            // 
            this.loopAssetListGeneratorBLALToolStripMenuItem.Name = "loopAssetListGeneratorBLALToolStripMenuItem";
            this.loopAssetListGeneratorBLALToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.loopAssetListGeneratorBLALToolStripMenuItem.Text = "Loop Asset List Generator (BLAL)";
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
            this.splitContainer1.Panel1.Controls.Add(this.barslistFileInfo);
            this.splitContainer1.Panel1.Controls.Add(this.grpDependencyPanel);
            this.splitContainer1.Panel1.Controls.Add(this.grpFilePanel);
            this.splitContainer1.Panel1.Controls.Add(this.grpFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.genericFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.nullDataPanel);
            this.splitContainer1.Panel1.Controls.Add(this.soundPlayerDeluxePanel);
            this.splitContainer1.Panel1.Controls.Add(this.nullFilePanel);
            this.splitContainer1.Panel1.Controls.Add(this.noInfoPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tree);
            this.splitContainer1.Size = new System.Drawing.Size(917, 474);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 1;
            // 
            // barslistFileInfo
            // 
            this.barslistFileInfo.Controls.Add(this.lstInternalAssetName);
            this.barslistFileInfo.Controls.Add(this.label6);
            this.barslistFileInfo.Controls.Add(this.tableLayoutPanel1);
            this.barslistFileInfo.Controls.Add(this.label5);
            this.barslistFileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barslistFileInfo.Location = new System.Drawing.Point(0, 0);
            this.barslistFileInfo.Name = "barslistFileInfo";
            this.barslistFileInfo.Size = new System.Drawing.Size(303, 472);
            this.barslistFileInfo.TabIndex = 6;
            // 
            // lstInternalAssetName
            // 
            this.lstInternalAssetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInternalAssetName.Location = new System.Drawing.Point(4, 78);
            this.lstInternalAssetName.Name = "lstInternalAssetName";
            this.lstInternalAssetName.Size = new System.Drawing.Size(295, 20);
            this.lstInternalAssetName.TabIndex = 5;
            this.lstInternalAssetName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(3, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(299, 23);
            this.label6.TabIndex = 2;
            this.label6.Text = "Internal Asset Name:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label6, "File version to save as.");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.vMinBoxLst, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.vMajBoxLst, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-1, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // vMinBoxLst
            // 
            this.vMinBoxLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMinBoxLst.Location = new System.Drawing.Point(154, 3);
            this.vMinBoxLst.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMinBoxLst.Name = "vMinBoxLst";
            this.vMinBoxLst.Size = new System.Drawing.Size(146, 20);
            this.vMinBoxLst.TabIndex = 1;
            this.toolTip.SetToolTip(this.vMinBoxLst, "Minor version of the file.");
            this.vMinBoxLst.ValueChanged += new System.EventHandler(this.vMinBoxWar_ValueChanged);
            // 
            // vMajBoxLst
            // 
            this.vMajBoxLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMajBoxLst.Location = new System.Drawing.Point(3, 3);
            this.vMajBoxLst.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMajBoxLst.Name = "vMajBoxLst";
            this.vMajBoxLst.Size = new System.Drawing.Size(145, 20);
            this.vMajBoxLst.TabIndex = 0;
            this.toolTip.SetToolTip(this.vMajBoxLst, "Major version of the file.");
            this.vMajBoxLst.ValueChanged += new System.EventHandler(this.vMajBoxWar_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(299, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label5, "File version to save as.");
            // 
            // grpDependencyPanel
            // 
            this.grpDependencyPanel.Controls.Add(this.grpDepLoadFlagsBox);
            this.grpDependencyPanel.Controls.Add(this.label17);
            this.grpDependencyPanel.Controls.Add(this.grpDepEntryNumBox);
            this.grpDependencyPanel.Controls.Add(this.grpDepEntryNumComboBox);
            this.grpDependencyPanel.Controls.Add(this.label16);
            this.grpDependencyPanel.Controls.Add(this.grpDepEntryTypeBox);
            this.grpDependencyPanel.Controls.Add(this.label15);
            this.grpDependencyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDependencyPanel.Location = new System.Drawing.Point(0, 0);
            this.grpDependencyPanel.Name = "grpDependencyPanel";
            this.grpDependencyPanel.Size = new System.Drawing.Size(303, 472);
            this.grpDependencyPanel.TabIndex = 19;
            // 
            // grpDepLoadFlagsBox
            // 
            this.grpDepLoadFlagsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDepLoadFlagsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grpDepLoadFlagsBox.FormattingEnabled = true;
            this.grpDepLoadFlagsBox.Location = new System.Drawing.Point(6, 150);
            this.grpDepLoadFlagsBox.Name = "grpDepLoadFlagsBox";
            this.grpDepLoadFlagsBox.Size = new System.Drawing.Size(293, 21);
            this.grpDepLoadFlagsBox.TabIndex = 6;
            this.grpDepLoadFlagsBox.SelectedIndexChanged += new System.EventHandler(this.grpDepLoadFlagsBox_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.Location = new System.Drawing.Point(2, 124);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(297, 23);
            this.label17.TabIndex = 5;
            this.label17.Text = "Load Flags:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpDepEntryNumBox
            // 
            this.grpDepEntryNumBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDepEntryNumBox.Location = new System.Drawing.Point(6, 101);
            this.grpDepEntryNumBox.Maximum = new decimal(new int[] {
            16777215,
            0,
            0,
            0});
            this.grpDepEntryNumBox.Name = "grpDepEntryNumBox";
            this.grpDepEntryNumBox.Size = new System.Drawing.Size(293, 20);
            this.grpDepEntryNumBox.TabIndex = 4;
            this.grpDepEntryNumBox.ValueChanged += new System.EventHandler(this.grpDepEntryNumBox_ValueChanged);
            // 
            // grpDepEntryNumComboBox
            // 
            this.grpDepEntryNumComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDepEntryNumComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grpDepEntryNumComboBox.FormattingEnabled = true;
            this.grpDepEntryNumComboBox.Location = new System.Drawing.Point(6, 74);
            this.grpDepEntryNumComboBox.Name = "grpDepEntryNumComboBox";
            this.grpDepEntryNumComboBox.Size = new System.Drawing.Size(293, 21);
            this.grpDepEntryNumComboBox.TabIndex = 3;
            this.grpDepEntryNumComboBox.SelectedIndexChanged += new System.EventHandler(this.grpDepEntryNumComboBox_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.Location = new System.Drawing.Point(3, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(297, 23);
            this.label16.TabIndex = 2;
            this.label16.Text = "Entry Number:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpDepEntryTypeBox
            // 
            this.grpDepEntryTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDepEntryTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grpDepEntryTypeBox.FormattingEnabled = true;
            this.grpDepEntryTypeBox.Items.AddRange(new object[] {
            "Sound",
            "Sound Sequence",
            "Bank",
            "Wave Archive"});
            this.grpDepEntryTypeBox.Location = new System.Drawing.Point(6, 26);
            this.grpDepEntryTypeBox.Name = "grpDepEntryTypeBox";
            this.grpDepEntryTypeBox.Size = new System.Drawing.Size(293, 21);
            this.grpDepEntryTypeBox.TabIndex = 1;
            this.grpDepEntryTypeBox.SelectedIndexChanged += new System.EventHandler(this.grpDepEntryTypeBox_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(3, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(297, 23);
            this.label15.TabIndex = 0;
            this.label15.Text = "Entry Type:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpFilePanel
            // 
            this.grpFilePanel.Controls.Add(this.grpEmbedModeBox);
            this.grpFilePanel.Controls.Add(this.label14);
            this.grpFilePanel.Controls.Add(this.grpFileIdBox);
            this.grpFilePanel.Controls.Add(this.grpFileIdComboBox);
            this.grpFilePanel.Controls.Add(this.label13);
            this.grpFilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFilePanel.Location = new System.Drawing.Point(0, 0);
            this.grpFilePanel.Name = "grpFilePanel";
            this.grpFilePanel.Size = new System.Drawing.Size(303, 472);
            this.grpFilePanel.TabIndex = 18;
            // 
            // grpEmbedModeBox
            // 
            this.grpEmbedModeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEmbedModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grpEmbedModeBox.FormattingEnabled = true;
            this.grpEmbedModeBox.Items.AddRange(new object[] {
            "Linked",
            "Embedded"});
            this.grpEmbedModeBox.Location = new System.Drawing.Point(6, 95);
            this.grpEmbedModeBox.Name = "grpEmbedModeBox";
            this.grpEmbedModeBox.Size = new System.Drawing.Size(293, 21);
            this.grpEmbedModeBox.TabIndex = 4;
            this.grpEmbedModeBox.SelectedIndexChanged += new System.EventHandler(this.grpEmbedModeBox_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.Location = new System.Drawing.Point(3, 75);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(296, 21);
            this.label14.TabIndex = 3;
            this.label14.Text = "Embed Mode:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpFileIdBox
            // 
            this.grpFileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFileIdBox.Location = new System.Drawing.Point(6, 52);
            this.grpFileIdBox.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.grpFileIdBox.Name = "grpFileIdBox";
            this.grpFileIdBox.Size = new System.Drawing.Size(293, 20);
            this.grpFileIdBox.TabIndex = 2;
            this.grpFileIdBox.ValueChanged += new System.EventHandler(this.grpFileIdBox_ValueChanged);
            // 
            // grpFileIdComboBox
            // 
            this.grpFileIdComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFileIdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grpFileIdComboBox.FormattingEnabled = true;
            this.grpFileIdComboBox.Location = new System.Drawing.Point(6, 26);
            this.grpFileIdComboBox.Name = "grpFileIdComboBox";
            this.grpFileIdComboBox.Size = new System.Drawing.Size(293, 21);
            this.grpFileIdComboBox.TabIndex = 1;
            this.grpFileIdComboBox.SelectedIndexChanged += new System.EventHandler(this.grpFileIdComboBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Location = new System.Drawing.Point(3, 5);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(296, 21);
            this.label13.TabIndex = 0;
            this.label13.Text = "File Id:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpFileInfoPanel
            // 
            this.grpFileInfoPanel.Controls.Add(this.grpStpForceButton);
            this.grpFileInfoPanel.Controls.Add(this.tableLayoutPanel8);
            this.grpFileInfoPanel.Controls.Add(this.label12);
            this.grpFileInfoPanel.Controls.Add(this.grpWsdForceButton);
            this.grpFileInfoPanel.Controls.Add(this.tableLayoutPanel7);
            this.grpFileInfoPanel.Controls.Add(this.label11);
            this.grpFileInfoPanel.Controls.Add(this.grpWarForceButton);
            this.grpFileInfoPanel.Controls.Add(this.tableLayoutPanel6);
            this.grpFileInfoPanel.Controls.Add(this.label10);
            this.grpFileInfoPanel.Controls.Add(this.grpBnkForceButton);
            this.grpFileInfoPanel.Controls.Add(this.tableLayoutPanel5);
            this.grpFileInfoPanel.Controls.Add(this.label9);
            this.grpFileInfoPanel.Controls.Add(this.grpSeqForceButton);
            this.grpFileInfoPanel.Controls.Add(this.tableLayoutPanel3);
            this.grpFileInfoPanel.Controls.Add(this.label7);
            this.grpFileInfoPanel.Controls.Add(this.tableLayoutPanel4);
            this.grpFileInfoPanel.Controls.Add(this.label8);
            this.grpFileInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFileInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.grpFileInfoPanel.Name = "grpFileInfoPanel";
            this.grpFileInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.grpFileInfoPanel.TabIndex = 7;
            // 
            // grpStpForceButton
            // 
            this.grpStpForceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStpForceButton.Location = new System.Drawing.Point(3, 437);
            this.grpStpForceButton.Name = "grpStpForceButton";
            this.grpStpForceButton.Size = new System.Drawing.Size(296, 23);
            this.grpStpForceButton.TabIndex = 16;
            this.grpStpForceButton.Text = "Force Internal Prefetch Version";
            this.grpStpForceButton.UseVisualStyleBackColor = true;
            this.grpStpForceButton.Click += new System.EventHandler(this.grpStpForceButton_Click);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.Controls.Add(this.grpStpRevBox, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.grpStpMinBox, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.grpStpMajBox, 0, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 408);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel8.TabIndex = 15;
            // 
            // grpStpRevBox
            // 
            this.grpStpRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStpRevBox.Location = new System.Drawing.Point(205, 3);
            this.grpStpRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpStpRevBox.Name = "grpStpRevBox";
            this.grpStpRevBox.Size = new System.Drawing.Size(95, 20);
            this.grpStpRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.grpStpRevBox, "Revision version of the file.");
            this.grpStpRevBox.ValueChanged += new System.EventHandler(this.grpStpRevBox_ValueChanged);
            // 
            // grpStpMinBox
            // 
            this.grpStpMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStpMinBox.Location = new System.Drawing.Point(104, 3);
            this.grpStpMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpStpMinBox.Name = "grpStpMinBox";
            this.grpStpMinBox.Size = new System.Drawing.Size(95, 20);
            this.grpStpMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.grpStpMinBox, "Minor version of the file.");
            this.grpStpMinBox.ValueChanged += new System.EventHandler(this.grpStpMinBox_ValueChanged);
            // 
            // grpStpMajBox
            // 
            this.grpStpMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStpMajBox.Location = new System.Drawing.Point(3, 3);
            this.grpStpMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpStpMajBox.Name = "grpStpMajBox";
            this.grpStpMajBox.Size = new System.Drawing.Size(95, 20);
            this.grpStpMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.grpStpMajBox, "Major version of the file.");
            this.grpStpMajBox.ValueChanged += new System.EventHandler(this.grpStpMajBox_ValueChanged);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Location = new System.Drawing.Point(1, 382);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(299, 23);
            this.label12.TabIndex = 14;
            this.label12.Text = "Internal Prefetch Version:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label12, "File version to save as.");
            // 
            // grpWsdForceButton
            // 
            this.grpWsdForceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpWsdForceButton.Location = new System.Drawing.Point(3, 359);
            this.grpWsdForceButton.Name = "grpWsdForceButton";
            this.grpWsdForceButton.Size = new System.Drawing.Size(296, 23);
            this.grpWsdForceButton.TabIndex = 13;
            this.grpWsdForceButton.Text = "Force Internal Wave Sound Data Version";
            this.grpWsdForceButton.UseVisualStyleBackColor = true;
            this.grpWsdForceButton.Click += new System.EventHandler(this.grpWsdForceButton_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel7.Controls.Add(this.grpWsdRevBox, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.grpWsdMinBox, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.grpWsdMajBox, 0, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 327);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel7.TabIndex = 12;
            // 
            // grpWsdRevBox
            // 
            this.grpWsdRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWsdRevBox.Location = new System.Drawing.Point(205, 3);
            this.grpWsdRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpWsdRevBox.Name = "grpWsdRevBox";
            this.grpWsdRevBox.Size = new System.Drawing.Size(95, 20);
            this.grpWsdRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.grpWsdRevBox, "Revision version of the file.");
            this.grpWsdRevBox.ValueChanged += new System.EventHandler(this.grpWsdRevBox_ValueChanged);
            // 
            // grpWsdMinBox
            // 
            this.grpWsdMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWsdMinBox.Location = new System.Drawing.Point(104, 3);
            this.grpWsdMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpWsdMinBox.Name = "grpWsdMinBox";
            this.grpWsdMinBox.Size = new System.Drawing.Size(95, 20);
            this.grpWsdMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.grpWsdMinBox, "Minor version of the file.");
            this.grpWsdMinBox.ValueChanged += new System.EventHandler(this.grpWsdMinBox_ValueChanged);
            // 
            // grpWsdMajBox
            // 
            this.grpWsdMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWsdMajBox.Location = new System.Drawing.Point(3, 3);
            this.grpWsdMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpWsdMajBox.Name = "grpWsdMajBox";
            this.grpWsdMajBox.Size = new System.Drawing.Size(95, 20);
            this.grpWsdMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.grpWsdMajBox, "Major version of the file.");
            this.grpWsdMajBox.ValueChanged += new System.EventHandler(this.grpWsdMajBox_ValueChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(1, 301);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(299, 23);
            this.label11.TabIndex = 11;
            this.label11.Text = "Internal Wave Sound Data Version:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label11, "File version to save as.");
            // 
            // grpWarForceButton
            // 
            this.grpWarForceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpWarForceButton.Location = new System.Drawing.Point(3, 275);
            this.grpWarForceButton.Name = "grpWarForceButton";
            this.grpWarForceButton.Size = new System.Drawing.Size(296, 23);
            this.grpWarForceButton.TabIndex = 10;
            this.grpWarForceButton.Text = "Force Internal Wave Archive Version";
            this.grpWarForceButton.UseVisualStyleBackColor = true;
            this.grpWarForceButton.Click += new System.EventHandler(this.grpWarForceButton_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.Controls.Add(this.grpWarRevBox, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.grpWarMinBox, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.grpWarMajBox, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 246);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel6.TabIndex = 9;
            // 
            // grpWarRevBox
            // 
            this.grpWarRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWarRevBox.Location = new System.Drawing.Point(205, 3);
            this.grpWarRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpWarRevBox.Name = "grpWarRevBox";
            this.grpWarRevBox.Size = new System.Drawing.Size(95, 20);
            this.grpWarRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.grpWarRevBox, "Revision version of the file.");
            this.grpWarRevBox.ValueChanged += new System.EventHandler(this.grpWarRevBox_ValueChanged);
            // 
            // grpWarMinBox
            // 
            this.grpWarMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWarMinBox.Location = new System.Drawing.Point(104, 3);
            this.grpWarMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpWarMinBox.Name = "grpWarMinBox";
            this.grpWarMinBox.Size = new System.Drawing.Size(95, 20);
            this.grpWarMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.grpWarMinBox, "Minor version of the file.");
            this.grpWarMinBox.ValueChanged += new System.EventHandler(this.grpWarMinBox_ValueChanged);
            // 
            // grpWarMajBox
            // 
            this.grpWarMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWarMajBox.Location = new System.Drawing.Point(3, 3);
            this.grpWarMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpWarMajBox.Name = "grpWarMajBox";
            this.grpWarMajBox.Size = new System.Drawing.Size(95, 20);
            this.grpWarMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.grpWarMajBox, "Major version of the file.");
            this.grpWarMajBox.ValueChanged += new System.EventHandler(this.grpWarMajBox_ValueChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(0, 220);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(299, 23);
            this.label10.TabIndex = 8;
            this.label10.Text = "Internal Wave Archive Version:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label10, "File version to save as.");
            // 
            // grpBnkForceButton
            // 
            this.grpBnkForceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBnkForceButton.Location = new System.Drawing.Point(3, 194);
            this.grpBnkForceButton.Name = "grpBnkForceButton";
            this.grpBnkForceButton.Size = new System.Drawing.Size(296, 23);
            this.grpBnkForceButton.TabIndex = 7;
            this.grpBnkForceButton.Text = "Force Internal Bank Version";
            this.grpBnkForceButton.UseVisualStyleBackColor = true;
            this.grpBnkForceButton.Click += new System.EventHandler(this.grpBnkForceButton_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.grpBnkRevBox, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.grpBnkMinBox, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.grpBnkMajBox, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // grpBnkRevBox
            // 
            this.grpBnkRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBnkRevBox.Location = new System.Drawing.Point(205, 3);
            this.grpBnkRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpBnkRevBox.Name = "grpBnkRevBox";
            this.grpBnkRevBox.Size = new System.Drawing.Size(95, 20);
            this.grpBnkRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.grpBnkRevBox, "Revision version of the file.");
            this.grpBnkRevBox.ValueChanged += new System.EventHandler(this.grpBnkRevBox_ValueChanged);
            // 
            // grpBnkMinBox
            // 
            this.grpBnkMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBnkMinBox.Location = new System.Drawing.Point(104, 3);
            this.grpBnkMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpBnkMinBox.Name = "grpBnkMinBox";
            this.grpBnkMinBox.Size = new System.Drawing.Size(95, 20);
            this.grpBnkMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.grpBnkMinBox, "Minor version of the file.");
            this.grpBnkMinBox.ValueChanged += new System.EventHandler(this.grpBnkMinBox_ValueChanged);
            // 
            // grpBnkMajBox
            // 
            this.grpBnkMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBnkMajBox.Location = new System.Drawing.Point(3, 3);
            this.grpBnkMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpBnkMajBox.Name = "grpBnkMajBox";
            this.grpBnkMajBox.Size = new System.Drawing.Size(95, 20);
            this.grpBnkMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.grpBnkMajBox, "Major version of the file.");
            this.grpBnkMajBox.ValueChanged += new System.EventHandler(this.grpBnkMajBox_ValueChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(0, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(299, 23);
            this.label9.TabIndex = 5;
            this.label9.Text = "Internal Bank Version:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label9, "File version to save as.");
            // 
            // grpSeqForceButton
            // 
            this.grpSeqForceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSeqForceButton.Location = new System.Drawing.Point(3, 110);
            this.grpSeqForceButton.Name = "grpSeqForceButton";
            this.grpSeqForceButton.Size = new System.Drawing.Size(296, 23);
            this.grpSeqForceButton.TabIndex = 4;
            this.grpSeqForceButton.Text = "Force Internal Sequence Version";
            this.grpSeqForceButton.UseVisualStyleBackColor = true;
            this.grpSeqForceButton.Click += new System.EventHandler(this.grpSeqForceButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.grpSeqRevBox, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.grpSeqMinBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.grpSeqMajBox, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 78);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // grpSeqRevBox
            // 
            this.grpSeqRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSeqRevBox.Location = new System.Drawing.Point(205, 3);
            this.grpSeqRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpSeqRevBox.Name = "grpSeqRevBox";
            this.grpSeqRevBox.Size = new System.Drawing.Size(95, 20);
            this.grpSeqRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.grpSeqRevBox, "Revision version of the file.");
            this.grpSeqRevBox.ValueChanged += new System.EventHandler(this.grpSeqRevBox_ValueChanged);
            // 
            // grpSeqMinBox
            // 
            this.grpSeqMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSeqMinBox.Location = new System.Drawing.Point(104, 3);
            this.grpSeqMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpSeqMinBox.Name = "grpSeqMinBox";
            this.grpSeqMinBox.Size = new System.Drawing.Size(95, 20);
            this.grpSeqMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.grpSeqMinBox, "Minor version of the file.");
            this.grpSeqMinBox.ValueChanged += new System.EventHandler(this.grpSeqMinBox_ValueChanged);
            // 
            // grpSeqMajBox
            // 
            this.grpSeqMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSeqMajBox.Location = new System.Drawing.Point(3, 3);
            this.grpSeqMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpSeqMajBox.Name = "grpSeqMajBox";
            this.grpSeqMajBox.Size = new System.Drawing.Size(95, 20);
            this.grpSeqMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.grpSeqMajBox, "Major version of the file.");
            this.grpSeqMajBox.ValueChanged += new System.EventHandler(this.grpSeqMajBox_ValueChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(3, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(299, 23);
            this.label7.TabIndex = 2;
            this.label7.Text = "Internal Sequence Version:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label7, "File version to save as.");
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.grpRevBox, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.grpMinBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.grpMajBox, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(-1, 26);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // grpRevBox
            // 
            this.grpRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRevBox.Location = new System.Drawing.Point(205, 3);
            this.grpRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpRevBox.Name = "grpRevBox";
            this.grpRevBox.Size = new System.Drawing.Size(95, 20);
            this.grpRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.grpRevBox, "Revision version of the file.");
            this.grpRevBox.ValueChanged += new System.EventHandler(this.grpRevBox_ValueChanged);
            // 
            // grpMinBox
            // 
            this.grpMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMinBox.Location = new System.Drawing.Point(104, 3);
            this.grpMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpMinBox.Name = "grpMinBox";
            this.grpMinBox.Size = new System.Drawing.Size(95, 20);
            this.grpMinBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.grpMinBox, "Minor version of the file.");
            this.grpMinBox.ValueChanged += new System.EventHandler(this.grpMinBox_ValueChanged);
            // 
            // grpMajBox
            // 
            this.grpMajBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMajBox.Location = new System.Drawing.Point(3, 3);
            this.grpMajBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpMajBox.Name = "grpMajBox";
            this.grpMajBox.Size = new System.Drawing.Size(95, 20);
            this.grpMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.grpMajBox, "Major version of the file.");
            this.grpMajBox.ValueChanged += new System.EventHandler(this.grpMajBox_ValueChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(299, 23);
            this.label8.TabIndex = 0;
            this.label8.Text = "Version:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.label8, "File version to save as.");
            // 
            // genericFileInfoPanel
            // 
            this.genericFileInfoPanel.Controls.Add(this.versionGrid);
            this.genericFileInfoPanel.Controls.Add(this.label3);
            this.genericFileInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genericFileInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.genericFileInfoPanel.Name = "genericFileInfoPanel";
            this.genericFileInfoPanel.Size = new System.Drawing.Size(303, 472);
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
            this.versionGrid.Size = new System.Drawing.Size(303, 26);
            this.versionGrid.TabIndex = 1;
            // 
            // vRevBox
            // 
            this.vRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vRevBox.Location = new System.Drawing.Point(205, 3);
            this.vRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vRevBox.Name = "vRevBox";
            this.vRevBox.Size = new System.Drawing.Size(95, 20);
            this.vRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.vRevBox, "Revision version of the file.");
            // 
            // vMinBox
            // 
            this.vMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMinBox.Location = new System.Drawing.Point(104, 3);
            this.vMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMinBox.Name = "vMinBox";
            this.vMinBox.Size = new System.Drawing.Size(95, 20);
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
            this.vMajBox.Size = new System.Drawing.Size(95, 20);
            this.vMajBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.vMajBox, "Major version of the file.");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(299, 23);
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
            this.nullDataPanel.Size = new System.Drawing.Size(303, 472);
            this.nullDataPanel.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(303, 472);
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
            this.soundPlayerDeluxePanel.Size = new System.Drawing.Size(303, 472);
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
            this.soundDeluxeTrack2.Size = new System.Drawing.Size(303, 30);
            this.soundDeluxeTrack2.TabIndex = 9;
            // 
            // pauseSoundTrack
            // 
            this.pauseSoundTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pauseSoundTrack.Location = new System.Drawing.Point(3, 3);
            this.pauseSoundTrack.Name = "pauseSoundTrack";
            this.pauseSoundTrack.Size = new System.Drawing.Size(145, 24);
            this.pauseSoundTrack.TabIndex = 0;
            this.pauseSoundTrack.Text = "Pause";
            this.pauseSoundTrack.UseVisualStyleBackColor = true;
            this.pauseSoundTrack.Click += new System.EventHandler(this.pauseSoundTrack_Click);
            // 
            // stopSoundTrack
            // 
            this.stopSoundTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopSoundTrack.Location = new System.Drawing.Point(154, 3);
            this.stopSoundTrack.Name = "stopSoundTrack";
            this.stopSoundTrack.Size = new System.Drawing.Size(146, 24);
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
            this.soundDeluxeTrack1.Size = new System.Drawing.Size(303, 59);
            this.soundDeluxeTrack1.TabIndex = 11;
            // 
            // playSoundTrack
            // 
            this.playSoundTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playSoundTrack.Location = new System.Drawing.Point(3, 32);
            this.playSoundTrack.Name = "playSoundTrack";
            this.playSoundTrack.Size = new System.Drawing.Size(297, 24);
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
            this.soundPlayerDeluxeLabel.Size = new System.Drawing.Size(297, 29);
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
            this.nullFilePanel.Size = new System.Drawing.Size(303, 472);
            this.nullFilePanel.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 472);
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
            this.noInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.noInfoPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 472);
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
            this.tree.Size = new System.Drawing.Size(606, 472);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 498);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(917, 22);
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
            this.ClientSize = new System.Drawing.Size(917, 520);
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
            this.barslistFileInfo.ResumeLayout(false);
            this.barslistFileInfo.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vMinBoxLst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vMajBoxLst)).EndInit();
            this.grpDependencyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpDepEntryNumBox)).EndInit();
            this.grpFilePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpFileIdBox)).EndInit();
            this.grpFileInfoPanel.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpStpRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpStpMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpStpMajBox)).EndInit();
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpWsdRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWsdMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWsdMajBox)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpWarRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWarMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWarMajBox)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBnkRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBnkMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBnkMajBox)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpSeqRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSeqMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSeqMajBox)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpRevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpMajBox)).EndInit();
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
            if (extension.ToLower().Equals("slnk")) {
                openFileDialog.Filter = "Sound (Effect) Link|*.bslnk;*.belnk"; 
            } else {
                openFileDialog.Filter = description + "|*.b" + extension.ToLower();
            }
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
            if (extension.ToLower().Equals("slnk")) {
                saveFileDialog.Filter = "Sound (Effect) Link|*.bslnk;*.belnk";
            } else {
                saveFileDialog.Filter = description + "|*.b" + extension.ToLower();
            }
            saveFileDialog.ShowDialog();            

            //Set write mode.
            if (saveFileDialog.FileName != "") {

                //Fix extension.
                if (Path.GetExtension(saveFileDialog.FileName) == "") {
                    saveFileDialog.FileName += ".b" + extension.ToLower();
                }

            }

            //Return the file name.
            return saveFileDialog.FileName;

        }

        /// <summary>
        /// On closing hook.
        /// </summary>
        private void form_Close(object sender, FormClosingEventArgs e) {
            OnClosing();
        }

        /// <summary>
        /// On closing.
        /// </summary>
        public virtual void OnClosing() {}

        public ContextMenuStrip CreateMenuStrip(ContextMenuStrip orig, int[] indices, EventHandler[] eventHandlers) {

            ContextMenuStrip c = new ContextMenuStrip();

            foreach (int ind in indices) {

                var i = orig.Items[ind];
                c.Items.Add(i.Text, i.Image, eventHandlers[ind]);

            }

            return c;

        }

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
            File = (AudioFile)Activator.CreateInstance(FileType);

            //Reset values.
            FilePath = "";
            FileOpen = true;
            ExtFile = null;
            Text = EditorName + " - New " + ExtensionDescription + ".b" + Extension;

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
                File = (AudioFile)Activator.CreateInstance(FileType);
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
                ExtFile = File;

                //Update the main window.
                if (MainWindow != null) {
                    MainWindow.UpdateNodes();
                    MainWindow.DoInfoStuff();
                }
                if (OtherEditor != null) {
                    OtherEditor.UpdateNodes();
                    OtherEditor.DoInfoStuff();
                }

            }

            //External file is null.
            else {

                //Write the file.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                //Write the file.
                File.Write(bw);

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
            File = (AudioFile)Activator.CreateInstance(FileType);
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
                SaveQuitDialog q = new SaveQuitDialog(MainWindow);
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
            File = (AudioFile)Activator.CreateInstance(FileType);

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
                File = (AudioFile)Activator.CreateInstance(FileType);

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

                //Write the file.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                //Write the file.
                File.Write(bw);

                //Save the file.
                System.IO.File.WriteAllBytes(path, o.ToArray());

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

        public void addAboveToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeAddAbove();
        }

        public void addBelowToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeAddBelow();
        }

        public void moveUpToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeMoveUp();
        }

        public void moveDownToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeMoveDown();
        }

        public void blankToolStripMenuItem_Click(object sender, EventArgs e) {
            NodeBlank();
        }

        public void replaceFileToolStripMenuItem_Click(object sender, EventArgs e) {
            NodeReplace();
        }

        public void exportToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeExport();
        }

        public void nullifyToolStripMenuItem1_Click(object sender, EventArgs e) {
            NodeNullify();
        }

        public void deleteToolStripMenuItem1_Click(object sender, EventArgs e) {
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
            BoxLstMajChanged();
        }

        private void vMinBoxWar_ValueChanged(object sender, EventArgs e) {
            BoxLstMinChanged();
        }

        private void vRevBoxWar_ValueChanged(object sender, EventArgs e) {
            BoxLstRevChanged();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            BoxLstAssetNameChanged();
        }

        public virtual void BoxLstMajChanged() {}
        public virtual void BoxLstMinChanged() {}
        public virtual void BoxLstRevChanged() {}
        public virtual void BoxLstAssetNameChanged() {}

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


        //Group versions.
        #region grpVersions

        private void grpSeqForceButton_Click(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupForceSequenceVersion();
            }
        }

        private void grpBnkForceButton_Click(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupForceBankVersion();
            }
        }

        private void grpWarForceButton_Click(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupForceWaveArchiveVersion();
            }
        }

        private void grpWsdForceButton_Click(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupForceWaveSoundDataVersion();
            }
        }

        private void grpStpForceButton_Click(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupForcePrefetchVersion();
            }
        }

        private void grpMajBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpMinBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpRevBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpSeqMajBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpSeqMinBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpSeqRevBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpBnkMajBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpBnkMinBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpBnkRevBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpWarMajBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpWarMinBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpWarRevBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpWsdMajBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpWsdMinBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpWsdRevBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpStpMajBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpStpMinBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        private void grpStpRevBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupVersionChanged();
            }
        }

        public virtual void GroupForceSequenceVersion() {}
        public virtual void GroupForceBankVersion() {}
        public virtual void GroupForceWaveArchiveVersion() {}
        public virtual void GroupForceWaveSoundDataVersion() {}
        public virtual void GroupForcePrefetchVersion() {}
        public virtual void GroupVersionChanged() {}

        #endregion


        //Group file data.
        #region grpFile

        private void grpFileIdComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupFileIdComboChanged();
            }
        }

        private void grpFileIdBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupFileIdNumBoxChanged();
            }
        }

        private void grpEmbedModeBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupFileIdEmbedModeChanged();
            }
        }

        public virtual void GroupFileIdComboChanged() {}
        public virtual void GroupFileIdNumBoxChanged() {}
        public virtual void GroupFileIdEmbedModeChanged() {}

        #endregion


        //Group dependency.
        #region grpDependency

        private void grpDepEntryTypeBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupDependencyTypeChanged();
            }
        }

        private void grpDepEntryNumComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupDependencyEntryComboChanged();
            }
        }

        private void grpDepEntryNumBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupDependencyEntryNumBoxChanged();
            }
        }

        private void grpDepLoadFlagsBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                GroupDependencyFlagsChanged();
            }
        }

        public virtual void GroupDependencyTypeChanged() {}
        public virtual void GroupDependencyEntryComboChanged() {}
        public virtual void GroupDependencyEntryNumBoxChanged() {}
        public virtual void GroupDependencyFlagsChanged() {}

        #endregion


        //Solar tools.
        #region solarTools

        private void citricComposerToolStripMenuItem_Click(object sender, EventArgs e) {

            //Open Isabelle Sound Editor.
            Process.Start("Citric Composer.exe");

        }

        private void isabelleSoundEditorToolStripMenuItem_Click(object sender, EventArgs e) {

            //Open Isabelle Sound Editor.
            IsabelleSoundEditor s = new IsabelleSoundEditor();
            s.Show();

        }

        private void rescourceListEditorBARSLISTToolStripMenuItem_Click(object sender, EventArgs e) {

            //Rescource List Editor.
            RescourceListEditor r = new RescourceListEditor(MainWindow);
            r.Show();

        }

        private void resourceEditorToolStripMenuItem_Click(object sender, EventArgs e) {

            //Rescource Editor.
            RescourceEditor r = new RescourceEditor(MainWindow);
            r.Show();

        }

        #endregion

    }

}
