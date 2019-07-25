using CitraFileLoader;
using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.Diagnostics;

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
        public SoundFile<CitraFileLoader.ISoundFile> ExtFile;
        public ContextMenuStrip filesMenu;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem changeExternalPathToolStripMenuItem;

        /// <summary>
        /// Main window.
        /// </summary>
        public static MainWindow MainWindow;


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
        /// Constructor.
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

        public ComboBox sarBnkFileIdBox;
        public Label label34;
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
        public RadioButton soundPlayerDeluxePlayNextBox;
        public RadioButton soundPlayerDeluxePlayLoopBox;
        public RadioButton soundPlayerDeluxePlayOnceBox;
        private TableLayoutPanel soundDeluxeTrack3;
        public Panel sequenceEditorPanel;
        public Scintilla sequenceEditor;
        private BindingSource bindingSource1;
        public ToolStripMenuItem toolsTabMainWindow;
        private ToolStripMenuItem solarAudioSlayerToolStripMenuItem;
        private ToolStripMenuItem isabelleSoundEditorWAVSTMToolStripMenuItem;
        private ToolStripMenuItem brewstersWARBrewerToolStripMenuItem;
        private ToolStripMenuItem wolfgangsDataWriterWSDToolStripMenuItem;
        private ToolStripMenuItem bBBsBankerBNKToolStripMenuItem;
        private ToolStripMenuItem sSSsSequencerSEQToolStripMenuItem;
        private ToolStripMenuItem goldisGrouperGRPToolStripMenuItem;
        public Panel sarProjectInfoPanel;
        public NumericUpDown optionsPIBox;
        public Label optionsPILabel;
        public NumericUpDown streamBufferTimesBox;
        public Label streamBufferTimesLabel;
        public NumericUpDown maxWaveNumTracksBox;
        public Label maxWaveNumTracksLabel;
        public NumericUpDown maxWaveNumBox;
        public Label maxWaveNumLabel;
        public NumericUpDown maxStreamNumChannelsBox;
        public Label maxStreamNumChannelsLabel;
        public NumericUpDown maxStreamNumTracksBox;
        public Label maxStreamNumTracksLabel;
        public NumericUpDown maxStreamNumBox;
        public Label maxStreamNumLabel;
        public NumericUpDown maxSeqTrackNumBox;
        public Label maxSeqTrackNumLabel;
        public NumericUpDown maxSeqNumBox;
        public Label maxSeqNumLabel;
        public CheckBox sarIncludeStringBlock;
        public Panel versionPanel;
        public Button grpVersionUpdate;
        public TableLayoutPanel tableLayoutPanel11;
        public NumericUpDown grpVersionRev;
        public NumericUpDown grpVersionMin;
        public NumericUpDown grpVersionMax;
        public Label label23;
        public Button stpVersionUpdate;
        public TableLayoutPanel tableLayoutPanel10;
        public NumericUpDown stpVersionRev;
        public NumericUpDown stpVersionMin;
        public NumericUpDown stpVersionMax;
        public Label label22;
        public Button stmVersionUpdate;
        public TableLayoutPanel tableLayoutPanel9;
        public NumericUpDown stmVersionRev;
        public NumericUpDown stmVersionMax;
        public NumericUpDown stmVersionMin;
        public Label label21;
        public Button wsdVersionUpdate;
        public TableLayoutPanel tableLayoutPanel12;
        public NumericUpDown wsdVersionRev;
        public NumericUpDown wsdVersionMin;
        public NumericUpDown wsdVersionMax;
        public Label label20;
        public Button warVersionUpdate;
        public TableLayoutPanel tableLayoutPanel13;
        public NumericUpDown warVersionRev;
        public NumericUpDown warVersionMin;
        public NumericUpDown warVersionMax;
        public Label label19;
        public Button bankVersionUpdate;
        public TableLayoutPanel tableLayoutPanel14;
        public NumericUpDown bankVersionRev;
        public NumericUpDown bankVersionMin;
        public NumericUpDown bankVersionMax;
        public Label label18;
        public TableLayoutPanel tableLayoutPanel15;
        public NumericUpDown versionRev;
        public NumericUpDown versionMin;
        public NumericUpDown versionMax;
        public Label label24;
        public Button seqVersionUpdate;
        public TableLayoutPanel tableLayoutPanel16;
        public NumericUpDown seqVersionRev;
        public NumericUpDown seqVersionMin;
        public NumericUpDown seqVersionMax;
        public Label label25;
        public ComboBox byteOrderBox;
        public Label label26;
        public Panel playerInfoPanel;
        public CheckBox playerEnableSoundLimitBox;
        public NumericUpDown playerHeapSizeBox;
        public Label label27;
        public Label label28;
        public NumericUpDown playerSoundLimitBox;
        public Label label29;
        public Panel fileInfoPanel;
        public ComboBox fileTypeBox;
        public Label label30;
        public Panel grpPanel;
        public ComboBox sarGrpFileIdBox;
        public Label fileIdLabel;
        public Panel warInfoPanel;
        public Label label33;
        public ComboBox sarWarFileIdBox;
        public TableLayoutPanel tableLayoutPanel17;
        public CheckBox warIncludeWaveCountBox;
        public Label label31;
        public Label label32;
        public CheckBox warLoadIndividuallyBox;
        public Panel bankPanel;
        public DataGridView bankWarDataGrid;
        public DataGridViewComboBoxColumn waveArchives;
        public TableLayoutPanel tableLayoutPanel18;
        public Label label35;
        public Panel soundGrpPanel;
        public TableLayoutPanel soundGrpGridTable;
        public DataGridView soundGrpWaveArchives;
        public DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        public DataGridView soundGrpFilesGrid;
        public DataGridViewComboBoxColumn Files;
        public NumericUpDown soundGrpEndIndex;
        public Label label36;
        public NumericUpDown soundGrpStartIndex;
        public Label label37;
        public ComboBox soundGrpSoundType;
        public Label label38;
        public Panel seqPanel;
        public TableLayoutPanel tableLayoutPanel19;
        public CheckBox seqC12;
        public CheckBox seqC0;
        public CheckBox seqC1;
        public CheckBox seqC2;
        public CheckBox seqC3;
        public CheckBox seqC4;
        public CheckBox seqC5;
        public CheckBox seqC6;
        public CheckBox seqC7;
        public CheckBox seqC8;
        public CheckBox seqC9;
        public CheckBox seqC10;
        public CheckBox seqC11;
        public CheckBox seqC15;
        public CheckBox seqC14;
        public CheckBox seqC13;
        public Label label39;
        public CheckBox seqIsReleasePriorityBox;
        public NumericUpDown seqChannelPriorityBox;
        public Label label41;
        public TableLayoutPanel tableLayoutPanel20;
        public ComboBox seqOffsetFromLabelBox;
        public RadioButton seqOffsetManualButton;
        public RadioButton seqOffsetFromLabelButton;
        public NumericUpDown seqOffsetManualBox;
        public Label label42;
        public CheckBox seqSound3dInfoExists;
        public ComboBox seqBank3Box;
        public ComboBox seqBank2Box;
        public ComboBox seqBank1Box;
        public ComboBox seqBank0Box;
        public Label label43;
        public Button seqEditSound3dInfoButton;
        public Button seqEditSoundInfoButton;
        public ComboBox sarSeqFileIdBox;
        public Label label47;
        public TableLayoutPanel tableLayoutPanel23;
        public Button sarSeqPlay;
        public Label label44;
        public TableLayoutPanel tableLayoutPanel22;
        public Button sarSeqPause;
        public Button sarSeqStop;
        public TableLayoutPanel tableLayoutPanel21;
        public RadioButton sarSeqPlayNext;
        public RadioButton sarSeqPlayLoop;
        public RadioButton sarSeqPlayOnce;
        public Panel wsdPanel;
        public TableLayoutPanel tableLayoutPanel26;
        public RadioButton sarWsdPlayNext;
        public RadioButton sarWsdPlayLoop;
        public RadioButton sarWsdPlayOnce;
        public TableLayoutPanel tableLayoutPanel25;
        public Button sarWsdPause;
        public Button sarWsdStop;
        public TableLayoutPanel tableLayoutPanel24;
        public Button sarWsdPlay;
        public Label label50;
        public ComboBox sarWsdFileIdBox;
        public Label label49;
        public CheckBox wsdFixPriority;
        public NumericUpDown wsdChannelPriority;
        public Label label40;
        public Button wsdCopyCount;
        public NumericUpDown wsdTracksToAllocate;
        public Label label45;
        public NumericUpDown wsdWaveIndex;
        public Label label46;
        public CheckBox wsdSound3dEnable;
        public Button wsdSound3dButton;
        public Button wsdEditSoundInfoButton;
        public Panel stmPanel;
        public TableLayoutPanel tableLayoutPanel29;
        public Label label61;
        public TabControl tabControl1;
        public TabPage tabPage1;
        public Label label48;
        public TableLayoutPanel tableLayoutPanel27;
        public CheckBox stmTrack12;
        public CheckBox stmTrack0;
        public CheckBox stmTrack1;
        public CheckBox stmTrack2;
        public CheckBox stmTrack3;
        public CheckBox stmTrack4;
        public CheckBox stmTrack5;
        public CheckBox stmTrack6;
        public CheckBox stmTrack7;
        public CheckBox stmTrack8;
        public CheckBox stmTrack9;
        public CheckBox stmTrack10;
        public CheckBox stmTrack11;
        public CheckBox stmTrack15;
        public CheckBox stmTrack14;
        public CheckBox stmTrack13;
        public Label label51;
        public Button stmSoundInfoButton;
        public NumericUpDown stmPitch;
        public Button stmSound3dButton;
        public Label label56;
        public CheckBox stmSound3dEnable;
        public NumericUpDown stmAllocateChannelsNum;
        public CheckBox stmWriteTrackInfo;
        public Button stmCopyChannelCountFromFile;
        public Button stmUpdateTrackInfo;
        public Label stmAllocateChannels;
        public ComboBox stmStreamType;
        public Label label57;
        public TabPage tabPage2;
        public CheckBox stmGeneratePrefetch;
        public Label label58;
        public ComboBox stmPrefetchFileIdBox;
        public Button stmCreateUniquePrefetchFile;
        public Button stmUpdatePrefetchInfo;
        public NumericUpDown stmLoopEndFrame;
        public Label label59;
        public NumericUpDown stmLoopStartFrame;
        public Label label60;
        public CheckBox stmIncludeExtension;
        public ComboBox stmFileIdBox;
        public TableLayoutPanel tableLayoutPanel30;
        public Button stmPlay;
        public Label label62;
        public TableLayoutPanel tableLayoutPanel31;
        public Button stmPause;
        public Button stmStop;
        public TableLayoutPanel tableLayoutPanel32;
        public RadioButton stmPlayNext;
        public RadioButton stmPlayLoop;
        public RadioButton stmPlayOnce;
        public TableLayoutPanel tableLayoutPanel28;
        public Label label52;
        public NumericUpDown stmSendC;
        public Label label53;
        public NumericUpDown stmSendB;
        public Label label54;
        public NumericUpDown stmSendA;
        public Label label55;
        public NumericUpDown stmSendMain;
        public Button stmCopyExtensionFromFile;
        public Panel trackPanel;
        public Label label63;
        public NumericUpDown trackVolume;
        public CheckBox trackSurround;
        public NumericUpDown trackSpan;
        public Label label65;
        public NumericUpDown trackPan;
        public Label label64;
        public NumericUpDown trackBiquadValue;
        public Label label68;
        public Label label67;
        public NumericUpDown trackLPFFrequency;
        public Label label66;
        public TableLayoutPanel tableLayoutPanel33;
        public Label label69;
        public NumericUpDown trackSendC;
        public Label label70;
        public NumericUpDown trackSendB;
        public Label label71;
        public NumericUpDown trackSendA;
        public Label label72;
        public NumericUpDown trackSendMain;
        public ComboBox trackBiquadType;
        public DataGridView trackChannelGrid;
        private DataGridViewTextBoxColumn channel;
        public CheckBox filesIncludeGroups;
        public DataGridView filesGroupGrid;
        private DataGridViewComboBoxColumn groups;

        public void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorBase));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("File Information", 10, 10);
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
            this.toolsTabMainWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.solarAudioSlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isabelleSoundEditorWAVSTMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brewstersWARBrewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wolfgangsDataWriterWSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bBBsBankerBNKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sSSsSequencerSEQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goldisGrouperGRPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.versionPanel = new System.Windows.Forms.Panel();
            this.grpVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.grpVersionRev = new System.Windows.Forms.NumericUpDown();
            this.grpVersionMin = new System.Windows.Forms.NumericUpDown();
            this.grpVersionMax = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.stpVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.stpVersionRev = new System.Windows.Forms.NumericUpDown();
            this.stpVersionMin = new System.Windows.Forms.NumericUpDown();
            this.stpVersionMax = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.stmVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.stmVersionRev = new System.Windows.Forms.NumericUpDown();
            this.stmVersionMax = new System.Windows.Forms.NumericUpDown();
            this.stmVersionMin = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.wsdVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.wsdVersionRev = new System.Windows.Forms.NumericUpDown();
            this.wsdVersionMin = new System.Windows.Forms.NumericUpDown();
            this.wsdVersionMax = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.warVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.warVersionRev = new System.Windows.Forms.NumericUpDown();
            this.warVersionMin = new System.Windows.Forms.NumericUpDown();
            this.warVersionMax = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.bankVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.bankVersionRev = new System.Windows.Forms.NumericUpDown();
            this.bankVersionMin = new System.Windows.Forms.NumericUpDown();
            this.bankVersionMax = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.versionRev = new System.Windows.Forms.NumericUpDown();
            this.versionMin = new System.Windows.Forms.NumericUpDown();
            this.versionMax = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.seqVersionUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.seqVersionRev = new System.Windows.Forms.NumericUpDown();
            this.seqVersionMin = new System.Windows.Forms.NumericUpDown();
            this.seqVersionMax = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.byteOrderBox = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.fileInfoPanel = new System.Windows.Forms.Panel();
            this.filesGroupGrid = new System.Windows.Forms.DataGridView();
            this.groups = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.filesIncludeGroups = new System.Windows.Forms.CheckBox();
            this.fileTypeBox = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.trackPanel = new System.Windows.Forms.Panel();
            this.trackChannelGrid = new System.Windows.Forms.DataGridView();
            this.channel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackBiquadType = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel33 = new System.Windows.Forms.TableLayoutPanel();
            this.label69 = new System.Windows.Forms.Label();
            this.trackSendC = new System.Windows.Forms.NumericUpDown();
            this.label70 = new System.Windows.Forms.Label();
            this.trackSendB = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.trackSendA = new System.Windows.Forms.NumericUpDown();
            this.label72 = new System.Windows.Forms.Label();
            this.trackSendMain = new System.Windows.Forms.NumericUpDown();
            this.trackBiquadValue = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.trackLPFFrequency = new System.Windows.Forms.NumericUpDown();
            this.label66 = new System.Windows.Forms.Label();
            this.trackSurround = new System.Windows.Forms.CheckBox();
            this.trackSpan = new System.Windows.Forms.NumericUpDown();
            this.label65 = new System.Windows.Forms.Label();
            this.trackPan = new System.Windows.Forms.NumericUpDown();
            this.label64 = new System.Windows.Forms.Label();
            this.trackVolume = new System.Windows.Forms.NumericUpDown();
            this.label63 = new System.Windows.Forms.Label();
            this.stmPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel29 = new System.Windows.Forms.TableLayoutPanel();
            this.label61 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label48 = new System.Windows.Forms.Label();
            this.tableLayoutPanel27 = new System.Windows.Forms.TableLayoutPanel();
            this.stmTrack12 = new System.Windows.Forms.CheckBox();
            this.stmTrack0 = new System.Windows.Forms.CheckBox();
            this.stmTrack1 = new System.Windows.Forms.CheckBox();
            this.stmTrack2 = new System.Windows.Forms.CheckBox();
            this.stmTrack3 = new System.Windows.Forms.CheckBox();
            this.stmTrack4 = new System.Windows.Forms.CheckBox();
            this.stmTrack5 = new System.Windows.Forms.CheckBox();
            this.stmTrack6 = new System.Windows.Forms.CheckBox();
            this.stmTrack7 = new System.Windows.Forms.CheckBox();
            this.stmTrack8 = new System.Windows.Forms.CheckBox();
            this.stmTrack9 = new System.Windows.Forms.CheckBox();
            this.stmTrack10 = new System.Windows.Forms.CheckBox();
            this.stmTrack11 = new System.Windows.Forms.CheckBox();
            this.stmTrack15 = new System.Windows.Forms.CheckBox();
            this.stmTrack14 = new System.Windows.Forms.CheckBox();
            this.stmTrack13 = new System.Windows.Forms.CheckBox();
            this.label51 = new System.Windows.Forms.Label();
            this.stmSoundInfoButton = new System.Windows.Forms.Button();
            this.stmPitch = new System.Windows.Forms.NumericUpDown();
            this.stmSound3dButton = new System.Windows.Forms.Button();
            this.label56 = new System.Windows.Forms.Label();
            this.stmSound3dEnable = new System.Windows.Forms.CheckBox();
            this.stmAllocateChannelsNum = new System.Windows.Forms.NumericUpDown();
            this.stmWriteTrackInfo = new System.Windows.Forms.CheckBox();
            this.stmCopyChannelCountFromFile = new System.Windows.Forms.Button();
            this.stmUpdateTrackInfo = new System.Windows.Forms.Button();
            this.stmAllocateChannels = new System.Windows.Forms.Label();
            this.stmStreamType = new System.Windows.Forms.ComboBox();
            this.label57 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.stmCopyExtensionFromFile = new System.Windows.Forms.Button();
            this.tableLayoutPanel28 = new System.Windows.Forms.TableLayoutPanel();
            this.label52 = new System.Windows.Forms.Label();
            this.stmSendC = new System.Windows.Forms.NumericUpDown();
            this.label53 = new System.Windows.Forms.Label();
            this.stmSendB = new System.Windows.Forms.NumericUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.stmSendA = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.stmSendMain = new System.Windows.Forms.NumericUpDown();
            this.stmGeneratePrefetch = new System.Windows.Forms.CheckBox();
            this.label58 = new System.Windows.Forms.Label();
            this.stmPrefetchFileIdBox = new System.Windows.Forms.ComboBox();
            this.stmCreateUniquePrefetchFile = new System.Windows.Forms.Button();
            this.stmUpdatePrefetchInfo = new System.Windows.Forms.Button();
            this.stmLoopEndFrame = new System.Windows.Forms.NumericUpDown();
            this.label59 = new System.Windows.Forms.Label();
            this.stmLoopStartFrame = new System.Windows.Forms.NumericUpDown();
            this.label60 = new System.Windows.Forms.Label();
            this.stmIncludeExtension = new System.Windows.Forms.CheckBox();
            this.stmFileIdBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel30 = new System.Windows.Forms.TableLayoutPanel();
            this.stmPlay = new System.Windows.Forms.Button();
            this.label62 = new System.Windows.Forms.Label();
            this.tableLayoutPanel31 = new System.Windows.Forms.TableLayoutPanel();
            this.stmPause = new System.Windows.Forms.Button();
            this.stmStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel32 = new System.Windows.Forms.TableLayoutPanel();
            this.stmPlayNext = new System.Windows.Forms.RadioButton();
            this.stmPlayLoop = new System.Windows.Forms.RadioButton();
            this.stmPlayOnce = new System.Windows.Forms.RadioButton();
            this.wsdPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel26 = new System.Windows.Forms.TableLayoutPanel();
            this.sarWsdPlayNext = new System.Windows.Forms.RadioButton();
            this.sarWsdPlayLoop = new System.Windows.Forms.RadioButton();
            this.sarWsdPlayOnce = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel25 = new System.Windows.Forms.TableLayoutPanel();
            this.sarWsdPause = new System.Windows.Forms.Button();
            this.sarWsdStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel24 = new System.Windows.Forms.TableLayoutPanel();
            this.sarWsdPlay = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.sarWsdFileIdBox = new System.Windows.Forms.ComboBox();
            this.label49 = new System.Windows.Forms.Label();
            this.wsdFixPriority = new System.Windows.Forms.CheckBox();
            this.wsdChannelPriority = new System.Windows.Forms.NumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.wsdCopyCount = new System.Windows.Forms.Button();
            this.wsdTracksToAllocate = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.wsdWaveIndex = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.wsdSound3dEnable = new System.Windows.Forms.CheckBox();
            this.wsdSound3dButton = new System.Windows.Forms.Button();
            this.wsdEditSoundInfoButton = new System.Windows.Forms.Button();
            this.seqPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.sarSeqPlay = new System.Windows.Forms.Button();
            this.label44 = new System.Windows.Forms.Label();
            this.tableLayoutPanel22 = new System.Windows.Forms.TableLayoutPanel();
            this.sarSeqPause = new System.Windows.Forms.Button();
            this.sarSeqStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.sarSeqPlayNext = new System.Windows.Forms.RadioButton();
            this.sarSeqPlayLoop = new System.Windows.Forms.RadioButton();
            this.sarSeqPlayOnce = new System.Windows.Forms.RadioButton();
            this.sarSeqFileIdBox = new System.Windows.Forms.ComboBox();
            this.label47 = new System.Windows.Forms.Label();
            this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
            this.seqC12 = new System.Windows.Forms.CheckBox();
            this.seqC0 = new System.Windows.Forms.CheckBox();
            this.seqC1 = new System.Windows.Forms.CheckBox();
            this.seqC2 = new System.Windows.Forms.CheckBox();
            this.seqC3 = new System.Windows.Forms.CheckBox();
            this.seqC4 = new System.Windows.Forms.CheckBox();
            this.seqC5 = new System.Windows.Forms.CheckBox();
            this.seqC6 = new System.Windows.Forms.CheckBox();
            this.seqC7 = new System.Windows.Forms.CheckBox();
            this.seqC8 = new System.Windows.Forms.CheckBox();
            this.seqC9 = new System.Windows.Forms.CheckBox();
            this.seqC10 = new System.Windows.Forms.CheckBox();
            this.seqC11 = new System.Windows.Forms.CheckBox();
            this.seqC15 = new System.Windows.Forms.CheckBox();
            this.seqC14 = new System.Windows.Forms.CheckBox();
            this.seqC13 = new System.Windows.Forms.CheckBox();
            this.label39 = new System.Windows.Forms.Label();
            this.seqIsReleasePriorityBox = new System.Windows.Forms.CheckBox();
            this.seqChannelPriorityBox = new System.Windows.Forms.NumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.seqOffsetFromLabelBox = new System.Windows.Forms.ComboBox();
            this.seqOffsetManualButton = new System.Windows.Forms.RadioButton();
            this.seqOffsetFromLabelButton = new System.Windows.Forms.RadioButton();
            this.seqOffsetManualBox = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.seqSound3dInfoExists = new System.Windows.Forms.CheckBox();
            this.seqBank3Box = new System.Windows.Forms.ComboBox();
            this.seqBank2Box = new System.Windows.Forms.ComboBox();
            this.seqBank1Box = new System.Windows.Forms.ComboBox();
            this.seqBank0Box = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.seqEditSound3dInfoButton = new System.Windows.Forms.Button();
            this.seqEditSoundInfoButton = new System.Windows.Forms.Button();
            this.soundGrpPanel = new System.Windows.Forms.Panel();
            this.soundGrpGridTable = new System.Windows.Forms.TableLayoutPanel();
            this.soundGrpWaveArchives = new System.Windows.Forms.DataGridView();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.soundGrpFilesGrid = new System.Windows.Forms.DataGridView();
            this.Files = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.soundGrpEndIndex = new System.Windows.Forms.NumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.soundGrpStartIndex = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.soundGrpSoundType = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.bankPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.label34 = new System.Windows.Forms.Label();
            this.sarBnkFileIdBox = new System.Windows.Forms.ComboBox();
            this.bankWarDataGrid = new System.Windows.Forms.DataGridView();
            this.waveArchives = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label35 = new System.Windows.Forms.Label();
            this.warInfoPanel = new System.Windows.Forms.Panel();
            this.label33 = new System.Windows.Forms.Label();
            this.sarWarFileIdBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.warIncludeWaveCountBox = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.warLoadIndividuallyBox = new System.Windows.Forms.CheckBox();
            this.grpPanel = new System.Windows.Forms.Panel();
            this.sarGrpFileIdBox = new System.Windows.Forms.ComboBox();
            this.fileIdLabel = new System.Windows.Forms.Label();
            this.playerInfoPanel = new System.Windows.Forms.Panel();
            this.playerEnableSoundLimitBox = new System.Windows.Forms.CheckBox();
            this.playerHeapSizeBox = new System.Windows.Forms.NumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.playerSoundLimitBox = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.sarProjectInfoPanel = new System.Windows.Forms.Panel();
            this.sarIncludeStringBlock = new System.Windows.Forms.CheckBox();
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
            this.soundPlayerDeluxePanel = new System.Windows.Forms.Panel();
            this.soundDeluxeTrack3 = new System.Windows.Forms.TableLayoutPanel();
            this.soundPlayerDeluxePlayNextBox = new System.Windows.Forms.RadioButton();
            this.soundPlayerDeluxePlayLoopBox = new System.Windows.Forms.RadioButton();
            this.soundPlayerDeluxePlayOnceBox = new System.Windows.Forms.RadioButton();
            this.soundDeluxeTrack2 = new System.Windows.Forms.TableLayoutPanel();
            this.pauseSoundTrack = new System.Windows.Forms.Button();
            this.stopSoundTrack = new System.Windows.Forms.Button();
            this.soundDeluxeTrack1 = new System.Windows.Forms.TableLayoutPanel();
            this.playSoundTrack = new System.Windows.Forms.Button();
            this.soundPlayerDeluxeLabel = new System.Windows.Forms.Label();
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
            this.nullFilePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.noInfoPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.sequenceEditorPanel = new System.Windows.Forms.Panel();
            this.sequenceEditor = new ScintillaNET.Scintilla();
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
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.filesMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeExternalPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.versionPanel.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpVersionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpVersionMax)).BeginInit();
            this.tableLayoutPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpVersionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpVersionMax)).BeginInit();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stmVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmVersionMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmVersionMin)).BeginInit();
            this.tableLayoutPanel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wsdVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdVersionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdVersionMax)).BeginInit();
            this.tableLayoutPanel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warVersionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warVersionMax)).BeginInit();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankVersionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankVersionMax)).BeginInit();
            this.tableLayoutPanel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.versionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.versionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.versionMax)).BeginInit();
            this.tableLayoutPanel16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seqVersionRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seqVersionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seqVersionMax)).BeginInit();
            this.fileInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesGroupGrid)).BeginInit();
            this.trackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackChannelGrid)).BeginInit();
            this.tableLayoutPanel33.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBiquadValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackLPFFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSpan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            this.stmPanel.SuspendLayout();
            this.tableLayoutPanel29.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stmPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmAllocateChannelsNum)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmLoopEndFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmLoopStartFrame)).BeginInit();
            this.tableLayoutPanel30.SuspendLayout();
            this.tableLayoutPanel31.SuspendLayout();
            this.tableLayoutPanel32.SuspendLayout();
            this.wsdPanel.SuspendLayout();
            this.tableLayoutPanel26.SuspendLayout();
            this.tableLayoutPanel25.SuspendLayout();
            this.tableLayoutPanel24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wsdChannelPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdTracksToAllocate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdWaveIndex)).BeginInit();
            this.seqPanel.SuspendLayout();
            this.tableLayoutPanel23.SuspendLayout();
            this.tableLayoutPanel22.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.tableLayoutPanel19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seqChannelPriorityBox)).BeginInit();
            this.tableLayoutPanel20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seqOffsetManualBox)).BeginInit();
            this.soundGrpPanel.SuspendLayout();
            this.soundGrpGridTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpWaveArchives)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpFilesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpEndIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpStartIndex)).BeginInit();
            this.bankPanel.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankWarDataGrid)).BeginInit();
            this.warInfoPanel.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.grpPanel.SuspendLayout();
            this.playerInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playerHeapSizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerSoundLimitBox)).BeginInit();
            this.sarProjectInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsPIBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.streamBufferTimesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumTracksBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumChannelsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumTracksBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqTrackNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqNumBox)).BeginInit();
            this.soundPlayerDeluxePanel.SuspendLayout();
            this.soundDeluxeTrack3.SuspendLayout();
            this.soundDeluxeTrack2.SuspendLayout();
            this.soundDeluxeTrack1.SuspendLayout();
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
            this.nullFilePanel.SuspendLayout();
            this.noInfoPanel.SuspendLayout();
            this.sequenceEditorPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.rootMenu.SuspendLayout();
            this.nodeMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.filesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editToolStripMenuItem,
            this.toolsWarToolStripMenuItem,
            this.toolsTabMainWindow});
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
            // toolsTabMainWindow
            // 
            this.toolsTabMainWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solarAudioSlayerToolStripMenuItem,
            this.isabelleSoundEditorWAVSTMToolStripMenuItem,
            this.brewstersWARBrewerToolStripMenuItem,
            this.wolfgangsDataWriterWSDToolStripMenuItem,
            this.bBBsBankerBNKToolStripMenuItem,
            this.sSSsSequencerSEQToolStripMenuItem,
            this.goldisGrouperGRPToolStripMenuItem});
            this.toolsTabMainWindow.Name = "toolsTabMainWindow";
            this.toolsTabMainWindow.Size = new System.Drawing.Size(47, 20);
            this.toolsTabMainWindow.Text = "Tools";
            this.toolsTabMainWindow.Visible = false;
            // 
            // solarAudioSlayerToolStripMenuItem
            // 
            this.solarAudioSlayerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("solarAudioSlayerToolStripMenuItem.Image")));
            this.solarAudioSlayerToolStripMenuItem.Name = "solarAudioSlayerToolStripMenuItem";
            this.solarAudioSlayerToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.solarAudioSlayerToolStripMenuItem.Text = "Solar Audio Slayer";
            this.solarAudioSlayerToolStripMenuItem.Click += new System.EventHandler(this.SolarAudioSlayerToolStripMenuItem_Click);
            // 
            // isabelleSoundEditorWAVSTMToolStripMenuItem
            // 
            this.isabelleSoundEditorWAVSTMToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("isabelleSoundEditorWAVSTMToolStripMenuItem.Image")));
            this.isabelleSoundEditorWAVSTMToolStripMenuItem.Name = "isabelleSoundEditorWAVSTMToolStripMenuItem";
            this.isabelleSoundEditorWAVSTMToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.isabelleSoundEditorWAVSTMToolStripMenuItem.Text = "Isabelle Sound Editor (WAV, STM)";
            this.isabelleSoundEditorWAVSTMToolStripMenuItem.Click += new System.EventHandler(this.IsabelleSoundEditorWAVSTMToolStripMenuItem_Click);
            // 
            // brewstersWARBrewerToolStripMenuItem
            // 
            this.brewstersWARBrewerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("brewstersWARBrewerToolStripMenuItem.Image")));
            this.brewstersWARBrewerToolStripMenuItem.Name = "brewstersWARBrewerToolStripMenuItem";
            this.brewstersWARBrewerToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.brewstersWARBrewerToolStripMenuItem.Text = "Brewster\'s Archive Brewer (WAR)";
            this.brewstersWARBrewerToolStripMenuItem.Click += new System.EventHandler(this.BrewstersWARBrewerToolStripMenuItem_Click);
            // 
            // wolfgangsDataWriterWSDToolStripMenuItem
            // 
            this.wolfgangsDataWriterWSDToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("wolfgangsDataWriterWSDToolStripMenuItem.Image")));
            this.wolfgangsDataWriterWSDToolStripMenuItem.Name = "wolfgangsDataWriterWSDToolStripMenuItem";
            this.wolfgangsDataWriterWSDToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.wolfgangsDataWriterWSDToolStripMenuItem.Text = "Wolfgang\'s Data Writer (WSD)";
            this.wolfgangsDataWriterWSDToolStripMenuItem.Click += new System.EventHandler(this.WolfgangsDataWriterWSDToolStripMenuItem_Click);
            // 
            // bBBsBankerBNKToolStripMenuItem
            // 
            this.bBBsBankerBNKToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("bBBsBankerBNKToolStripMenuItem.Image")));
            this.bBBsBankerBNKToolStripMenuItem.Name = "bBBsBankerBNKToolStripMenuItem";
            this.bBBsBankerBNKToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.bBBsBankerBNKToolStripMenuItem.Text = "Beau\'s Banker (BNK)";
            this.bBBsBankerBNKToolStripMenuItem.Click += new System.EventHandler(this.BBBsBankerBNKToolStripMenuItem_Click);
            // 
            // sSSsSequencerSEQToolStripMenuItem
            // 
            this.sSSsSequencerSEQToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sSSsSequencerSEQToolStripMenuItem.Image")));
            this.sSSsSequencerSEQToolStripMenuItem.Name = "sSSsSequencerSEQToolStripMenuItem";
            this.sSSsSequencerSEQToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.sSSsSequencerSEQToolStripMenuItem.Text = "Statics\'s Sequencer (SEQ)";
            this.sSSsSequencerSEQToolStripMenuItem.Click += new System.EventHandler(this.SSSsSequencerSEQToolStripMenuItem_Click);
            // 
            // goldisGrouperGRPToolStripMenuItem
            // 
            this.goldisGrouperGRPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("goldisGrouperGRPToolStripMenuItem.Image")));
            this.goldisGrouperGRPToolStripMenuItem.Name = "goldisGrouperGRPToolStripMenuItem";
            this.goldisGrouperGRPToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.goldisGrouperGRPToolStripMenuItem.Text = "Goldi\'s Grouper (GRP)";
            this.goldisGrouperGRPToolStripMenuItem.Click += new System.EventHandler(this.GoldisGrouperGRPToolStripMenuItem_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.versionPanel);
            this.splitContainer1.Panel1.Controls.Add(this.fileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.trackPanel);
            this.splitContainer1.Panel1.Controls.Add(this.stmPanel);
            this.splitContainer1.Panel1.Controls.Add(this.wsdPanel);
            this.splitContainer1.Panel1.Controls.Add(this.seqPanel);
            this.splitContainer1.Panel1.Controls.Add(this.soundGrpPanel);
            this.splitContainer1.Panel1.Controls.Add(this.bankPanel);
            this.splitContainer1.Panel1.Controls.Add(this.warInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.grpPanel);
            this.splitContainer1.Panel1.Controls.Add(this.playerInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.sarProjectInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.soundPlayerDeluxePanel);
            this.splitContainer1.Panel1.Controls.Add(this.grpDependencyPanel);
            this.splitContainer1.Panel1.Controls.Add(this.grpFilePanel);
            this.splitContainer1.Panel1.Controls.Add(this.grpFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.warFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.genericFileInfoPanel);
            this.splitContainer1.Panel1.Controls.Add(this.nullDataPanel);
            this.splitContainer1.Panel1.Controls.Add(this.nullFilePanel);
            this.splitContainer1.Panel1.Controls.Add(this.noInfoPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sequenceEditorPanel);
            this.splitContainer1.Panel2.Controls.Add(this.tree);
            this.splitContainer1.Size = new System.Drawing.Size(917, 474);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 1;
            // 
            // versionPanel
            // 
            this.versionPanel.Controls.Add(this.grpVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel11);
            this.versionPanel.Controls.Add(this.label23);
            this.versionPanel.Controls.Add(this.stpVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel10);
            this.versionPanel.Controls.Add(this.label22);
            this.versionPanel.Controls.Add(this.stmVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel9);
            this.versionPanel.Controls.Add(this.label21);
            this.versionPanel.Controls.Add(this.wsdVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel12);
            this.versionPanel.Controls.Add(this.label20);
            this.versionPanel.Controls.Add(this.warVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel13);
            this.versionPanel.Controls.Add(this.label19);
            this.versionPanel.Controls.Add(this.bankVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel14);
            this.versionPanel.Controls.Add(this.label18);
            this.versionPanel.Controls.Add(this.tableLayoutPanel15);
            this.versionPanel.Controls.Add(this.label24);
            this.versionPanel.Controls.Add(this.seqVersionUpdate);
            this.versionPanel.Controls.Add(this.tableLayoutPanel16);
            this.versionPanel.Controls.Add(this.label25);
            this.versionPanel.Controls.Add(this.byteOrderBox);
            this.versionPanel.Controls.Add(this.label26);
            this.versionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versionPanel.Location = new System.Drawing.Point(0, 0);
            this.versionPanel.Name = "versionPanel";
            this.versionPanel.Size = new System.Drawing.Size(303, 472);
            this.versionPanel.TabIndex = 24;
            // 
            // grpVersionUpdate
            // 
            this.grpVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVersionUpdate.Location = new System.Drawing.Point(6, 430);
            this.grpVersionUpdate.Name = "grpVersionUpdate";
            this.grpVersionUpdate.Size = new System.Drawing.Size(293, 23);
            this.grpVersionUpdate.TabIndex = 25;
            this.grpVersionUpdate.Text = "Update All Versions";
            this.grpVersionUpdate.UseVisualStyleBackColor = true;
            this.grpVersionUpdate.Click += new System.EventHandler(this.GrpVersionUpdate_Click);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel11.ColumnCount = 3;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.Controls.Add(this.grpVersionRev, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.grpVersionMin, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.grpVersionMax, 0, 0);
            this.tableLayoutPanel11.Location = new System.Drawing.Point(6, 401);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel11.TabIndex = 24;
            // 
            // grpVersionRev
            // 
            this.grpVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVersionRev.Location = new System.Drawing.Point(197, 3);
            this.grpVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpVersionRev.Name = "grpVersionRev";
            this.grpVersionRev.Size = new System.Drawing.Size(93, 20);
            this.grpVersionRev.TabIndex = 2;
            // 
            // grpVersionMin
            // 
            this.grpVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpVersionMin.Location = new System.Drawing.Point(100, 3);
            this.grpVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpVersionMin.Name = "grpVersionMin";
            this.grpVersionMin.Size = new System.Drawing.Size(91, 20);
            this.grpVersionMin.TabIndex = 1;
            // 
            // grpVersionMax
            // 
            this.grpVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpVersionMax.Location = new System.Drawing.Point(3, 3);
            this.grpVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grpVersionMax.Name = "grpVersionMax";
            this.grpVersionMax.Size = new System.Drawing.Size(91, 20);
            this.grpVersionMax.TabIndex = 0;
            this.grpVersionMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.Location = new System.Drawing.Point(6, 384);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(291, 20);
            this.label23.TabIndex = 23;
            this.label23.Text = "Groups:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stpVersionUpdate
            // 
            this.stpVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stpVersionUpdate.Location = new System.Drawing.Point(6, 575);
            this.stpVersionUpdate.Name = "stpVersionUpdate";
            this.stpVersionUpdate.Size = new System.Drawing.Size(293, 24);
            this.stpVersionUpdate.TabIndex = 22;
            this.stpVersionUpdate.Text = "Update All Versions";
            this.stpVersionUpdate.UseVisualStyleBackColor = true;
            this.stpVersionUpdate.Click += new System.EventHandler(this.StpVersionUpdate_Click);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.Controls.Add(this.stpVersionRev, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.stpVersionMin, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.stpVersionMax, 0, 0);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(7, 543);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel10.TabIndex = 21;
            // 
            // stpVersionRev
            // 
            this.stpVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stpVersionRev.Location = new System.Drawing.Point(197, 3);
            this.stpVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.stpVersionRev.Name = "stpVersionRev";
            this.stpVersionRev.Size = new System.Drawing.Size(93, 20);
            this.stpVersionRev.TabIndex = 2;
            // 
            // stpVersionMin
            // 
            this.stpVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stpVersionMin.Location = new System.Drawing.Point(100, 3);
            this.stpVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.stpVersionMin.Name = "stpVersionMin";
            this.stpVersionMin.Size = new System.Drawing.Size(91, 20);
            this.stpVersionMin.TabIndex = 1;
            // 
            // stpVersionMax
            // 
            this.stpVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stpVersionMax.Location = new System.Drawing.Point(3, 3);
            this.stpVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.stpVersionMax.Name = "stpVersionMax";
            this.stpVersionMax.Size = new System.Drawing.Size(91, 20);
            this.stpVersionMax.TabIndex = 0;
            this.stpVersionMax.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.Location = new System.Drawing.Point(6, 527);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(291, 13);
            this.label22.TabIndex = 20;
            this.label22.Text = "Prefetches:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmVersionUpdate
            // 
            this.stmVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmVersionUpdate.Location = new System.Drawing.Point(5, 501);
            this.stmVersionUpdate.Name = "stmVersionUpdate";
            this.stmVersionUpdate.Size = new System.Drawing.Size(293, 23);
            this.stmVersionUpdate.TabIndex = 19;
            this.stmVersionUpdate.Text = "Update All Versions";
            this.stmVersionUpdate.UseVisualStyleBackColor = true;
            this.stmVersionUpdate.Click += new System.EventHandler(this.StmVersionUpdate_Click);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.Controls.Add(this.stmVersionRev, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.stmVersionMax, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.stmVersionMin, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(6, 473);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel9.TabIndex = 18;
            // 
            // stmVersionRev
            // 
            this.stmVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmVersionRev.Location = new System.Drawing.Point(197, 3);
            this.stmVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.stmVersionRev.Name = "stmVersionRev";
            this.stmVersionRev.Size = new System.Drawing.Size(93, 20);
            this.stmVersionRev.TabIndex = 2;
            // 
            // stmVersionMax
            // 
            this.stmVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmVersionMax.Location = new System.Drawing.Point(3, 3);
            this.stmVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.stmVersionMax.Name = "stmVersionMax";
            this.stmVersionMax.Size = new System.Drawing.Size(91, 20);
            this.stmVersionMax.TabIndex = 1;
            this.stmVersionMax.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // stmVersionMin
            // 
            this.stmVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmVersionMin.Location = new System.Drawing.Point(100, 3);
            this.stmVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.stmVersionMin.Name = "stmVersionMin";
            this.stmVersionMin.Size = new System.Drawing.Size(91, 20);
            this.stmVersionMin.TabIndex = 0;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.Location = new System.Drawing.Point(4, 458);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(291, 16);
            this.label21.TabIndex = 17;
            this.label21.Text = "Streams:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wsdVersionUpdate
            // 
            this.wsdVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdVersionUpdate.Location = new System.Drawing.Point(5, 361);
            this.wsdVersionUpdate.Name = "wsdVersionUpdate";
            this.wsdVersionUpdate.Size = new System.Drawing.Size(293, 23);
            this.wsdVersionUpdate.TabIndex = 16;
            this.wsdVersionUpdate.Text = "Update All Versions";
            this.wsdVersionUpdate.UseVisualStyleBackColor = true;
            this.wsdVersionUpdate.Click += new System.EventHandler(this.WsdVersionUpdate_Click);
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel12.ColumnCount = 3;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel12.Controls.Add(this.wsdVersionRev, 2, 0);
            this.tableLayoutPanel12.Controls.Add(this.wsdVersionMin, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.wsdVersionMax, 0, 0);
            this.tableLayoutPanel12.Location = new System.Drawing.Point(5, 336);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel12.TabIndex = 15;
            // 
            // wsdVersionRev
            // 
            this.wsdVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdVersionRev.Location = new System.Drawing.Point(197, 3);
            this.wsdVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.wsdVersionRev.Name = "wsdVersionRev";
            this.wsdVersionRev.Size = new System.Drawing.Size(93, 20);
            this.wsdVersionRev.TabIndex = 2;
            // 
            // wsdVersionMin
            // 
            this.wsdVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wsdVersionMin.Location = new System.Drawing.Point(100, 3);
            this.wsdVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.wsdVersionMin.Name = "wsdVersionMin";
            this.wsdVersionMin.Size = new System.Drawing.Size(91, 20);
            this.wsdVersionMin.TabIndex = 1;
            // 
            // wsdVersionMax
            // 
            this.wsdVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wsdVersionMax.Location = new System.Drawing.Point(3, 3);
            this.wsdVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.wsdVersionMax.Name = "wsdVersionMax";
            this.wsdVersionMax.Size = new System.Drawing.Size(91, 20);
            this.wsdVersionMax.TabIndex = 0;
            this.wsdVersionMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.Location = new System.Drawing.Point(6, 323);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(291, 13);
            this.label20.TabIndex = 14;
            this.label20.Text = "Wave Sound Data:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // warVersionUpdate
            // 
            this.warVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.warVersionUpdate.Location = new System.Drawing.Point(5, 297);
            this.warVersionUpdate.Name = "warVersionUpdate";
            this.warVersionUpdate.Size = new System.Drawing.Size(293, 23);
            this.warVersionUpdate.TabIndex = 13;
            this.warVersionUpdate.Text = "Update All Versions";
            this.warVersionUpdate.UseVisualStyleBackColor = true;
            this.warVersionUpdate.Click += new System.EventHandler(this.WarVersionUpdate_Click);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel13.ColumnCount = 3;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel13.Controls.Add(this.warVersionRev, 2, 0);
            this.tableLayoutPanel13.Controls.Add(this.warVersionMin, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.warVersionMax, 0, 0);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 265);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel13.TabIndex = 12;
            // 
            // warVersionRev
            // 
            this.warVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.warVersionRev.Location = new System.Drawing.Point(197, 3);
            this.warVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.warVersionRev.Name = "warVersionRev";
            this.warVersionRev.Size = new System.Drawing.Size(93, 20);
            this.warVersionRev.TabIndex = 2;
            // 
            // warVersionMin
            // 
            this.warVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warVersionMin.Location = new System.Drawing.Point(100, 3);
            this.warVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.warVersionMin.Name = "warVersionMin";
            this.warVersionMin.Size = new System.Drawing.Size(91, 20);
            this.warVersionMin.TabIndex = 1;
            // 
            // warVersionMax
            // 
            this.warVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warVersionMax.Location = new System.Drawing.Point(3, 3);
            this.warVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.warVersionMax.Name = "warVersionMax";
            this.warVersionMax.Size = new System.Drawing.Size(91, 20);
            this.warVersionMax.TabIndex = 0;
            this.warVersionMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.Location = new System.Drawing.Point(4, 243);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(291, 17);
            this.label19.TabIndex = 11;
            this.label19.Text = "Wave Archives:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bankVersionUpdate
            // 
            this.bankVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bankVersionUpdate.Location = new System.Drawing.Point(5, 219);
            this.bankVersionUpdate.Name = "bankVersionUpdate";
            this.bankVersionUpdate.Size = new System.Drawing.Size(293, 23);
            this.bankVersionUpdate.TabIndex = 10;
            this.bankVersionUpdate.Text = "Update All Versions";
            this.bankVersionUpdate.UseVisualStyleBackColor = true;
            this.bankVersionUpdate.Click += new System.EventHandler(this.BankVersionUpdate_Click);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel14.Controls.Add(this.bankVersionRev, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.bankVersionMin, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.bankVersionMax, 0, 0);
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 190);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel14.TabIndex = 9;
            // 
            // bankVersionRev
            // 
            this.bankVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bankVersionRev.Location = new System.Drawing.Point(197, 3);
            this.bankVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.bankVersionRev.Name = "bankVersionRev";
            this.bankVersionRev.Size = new System.Drawing.Size(93, 20);
            this.bankVersionRev.TabIndex = 2;
            // 
            // bankVersionMin
            // 
            this.bankVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankVersionMin.Location = new System.Drawing.Point(100, 3);
            this.bankVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.bankVersionMin.Name = "bankVersionMin";
            this.bankVersionMin.Size = new System.Drawing.Size(91, 20);
            this.bankVersionMin.TabIndex = 1;
            // 
            // bankVersionMax
            // 
            this.bankVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankVersionMax.Location = new System.Drawing.Point(3, 3);
            this.bankVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.bankVersionMax.Name = "bankVersionMax";
            this.bankVersionMax.Size = new System.Drawing.Size(91, 20);
            this.bankVersionMax.TabIndex = 0;
            this.bankVersionMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.Location = new System.Drawing.Point(5, 170);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(291, 17);
            this.label18.TabIndex = 8;
            this.label18.Text = "Banks:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.Controls.Add(this.versionRev, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.versionMin, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.versionMax, 0, 0);
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 66);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel15.TabIndex = 7;
            // 
            // versionRev
            // 
            this.versionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.versionRev.Location = new System.Drawing.Point(197, 3);
            this.versionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.versionRev.Name = "versionRev";
            this.versionRev.Size = new System.Drawing.Size(93, 20);
            this.versionRev.TabIndex = 2;
            this.versionRev.ValueChanged += new System.EventHandler(this.VersionRev_ValueChanged);
            // 
            // versionMin
            // 
            this.versionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versionMin.Location = new System.Drawing.Point(100, 3);
            this.versionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.versionMin.Name = "versionMin";
            this.versionMin.Size = new System.Drawing.Size(91, 20);
            this.versionMin.TabIndex = 1;
            this.versionMin.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.versionMin.ValueChanged += new System.EventHandler(this.VersionMin_ValueChanged);
            // 
            // versionMax
            // 
            this.versionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versionMax.Location = new System.Drawing.Point(3, 3);
            this.versionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.versionMax.Name = "versionMax";
            this.versionMax.Size = new System.Drawing.Size(91, 20);
            this.versionMax.TabIndex = 0;
            this.versionMax.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.versionMax.ValueChanged += new System.EventHandler(this.VersionMax_ValueChanged);
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.Location = new System.Drawing.Point(7, 47);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(291, 17);
            this.label24.TabIndex = 6;
            this.label24.Text = "Version:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // seqVersionUpdate
            // 
            this.seqVersionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqVersionUpdate.Location = new System.Drawing.Point(5, 144);
            this.seqVersionUpdate.Name = "seqVersionUpdate";
            this.seqVersionUpdate.Size = new System.Drawing.Size(293, 23);
            this.seqVersionUpdate.TabIndex = 5;
            this.seqVersionUpdate.Text = "Update All Versions";
            this.seqVersionUpdate.UseVisualStyleBackColor = true;
            this.seqVersionUpdate.Click += new System.EventHandler(this.SeqVersionUpdate_Click);
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel16.ColumnCount = 3;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel16.Controls.Add(this.seqVersionRev, 2, 0);
            this.tableLayoutPanel16.Controls.Add(this.seqVersionMin, 1, 0);
            this.tableLayoutPanel16.Controls.Add(this.seqVersionMax, 0, 0);
            this.tableLayoutPanel16.Location = new System.Drawing.Point(4, 114);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(293, 26);
            this.tableLayoutPanel16.TabIndex = 4;
            // 
            // seqVersionRev
            // 
            this.seqVersionRev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqVersionRev.Location = new System.Drawing.Point(197, 3);
            this.seqVersionRev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.seqVersionRev.Name = "seqVersionRev";
            this.seqVersionRev.Size = new System.Drawing.Size(93, 20);
            this.seqVersionRev.TabIndex = 2;
            // 
            // seqVersionMin
            // 
            this.seqVersionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqVersionMin.Location = new System.Drawing.Point(100, 3);
            this.seqVersionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.seqVersionMin.Name = "seqVersionMin";
            this.seqVersionMin.Size = new System.Drawing.Size(91, 20);
            this.seqVersionMin.TabIndex = 1;
            // 
            // seqVersionMax
            // 
            this.seqVersionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqVersionMax.Location = new System.Drawing.Point(3, 3);
            this.seqVersionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.seqVersionMax.Name = "seqVersionMax";
            this.seqVersionMax.Size = new System.Drawing.Size(91, 20);
            this.seqVersionMax.TabIndex = 0;
            this.seqVersionMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.Location = new System.Drawing.Point(6, 94);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(291, 18);
            this.label25.TabIndex = 2;
            this.label25.Text = "Sequences:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // byteOrderBox
            // 
            this.byteOrderBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.byteOrderBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.byteOrderBox.FormattingEnabled = true;
            this.byteOrderBox.Items.AddRange(new object[] {
            "Little Endian",
            "Big Endian"});
            this.byteOrderBox.Location = new System.Drawing.Point(5, 22);
            this.byteOrderBox.Name = "byteOrderBox";
            this.byteOrderBox.Size = new System.Drawing.Size(292, 21);
            this.byteOrderBox.TabIndex = 1;
            this.byteOrderBox.SelectedIndexChanged += new System.EventHandler(this.ByteOrderBox_SelectedIndexChanged);
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.Location = new System.Drawing.Point(6, 3);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(291, 17);
            this.label26.TabIndex = 0;
            this.label26.Text = "Byte Order:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fileInfoPanel
            // 
            this.fileInfoPanel.Controls.Add(this.filesGroupGrid);
            this.fileInfoPanel.Controls.Add(this.filesIncludeGroups);
            this.fileInfoPanel.Controls.Add(this.fileTypeBox);
            this.fileInfoPanel.Controls.Add(this.label30);
            this.fileInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.fileInfoPanel.Name = "fileInfoPanel";
            this.fileInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.fileInfoPanel.TabIndex = 26;
            // 
            // filesGroupGrid
            // 
            this.filesGroupGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesGroupGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filesGroupGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groups});
            this.filesGroupGrid.Location = new System.Drawing.Point(8, 75);
            this.filesGroupGrid.Name = "filesGroupGrid";
            this.filesGroupGrid.Size = new System.Drawing.Size(287, 390);
            this.filesGroupGrid.TabIndex = 5;
            this.filesGroupGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.FilesGroupGridCellChanged);
            this.filesGroupGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.FilesGroupGridCellChanged);
            // 
            // groups
            // 
            this.groups.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groups.HeaderText = "Groups";
            this.groups.Name = "groups";
            // 
            // filesIncludeGroups
            // 
            this.filesIncludeGroups.AutoSize = true;
            this.filesIncludeGroups.Location = new System.Drawing.Point(8, 53);
            this.filesIncludeGroups.Name = "filesIncludeGroups";
            this.filesIncludeGroups.Size = new System.Drawing.Size(88, 17);
            this.filesIncludeGroups.TabIndex = 4;
            this.filesIncludeGroups.Text = "Write Groups";
            this.filesIncludeGroups.UseVisualStyleBackColor = true;
            this.filesIncludeGroups.CheckedChanged += new System.EventHandler(this.FilesIncludeGroups_CheckedChanged);
            // 
            // fileTypeBox
            // 
            this.fileTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileTypeBox.FormattingEnabled = true;
            this.fileTypeBox.Items.AddRange(new object[] {
            "Undefined",
            "Internal",
            "External",
            "In Group Only",
            "Null Reference",
            "Inside Aras File"});
            this.fileTypeBox.Location = new System.Drawing.Point(8, 23);
            this.fileTypeBox.Name = "fileTypeBox";
            this.fileTypeBox.Size = new System.Drawing.Size(287, 21);
            this.fileTypeBox.TabIndex = 3;
            this.fileTypeBox.SelectedIndexChanged += new System.EventHandler(this.FileTypeBox_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label30.Location = new System.Drawing.Point(3, 3);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(298, 17);
            this.label30.TabIndex = 2;
            this.label30.Text = "File Type:";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackPanel
            // 
            this.trackPanel.Controls.Add(this.trackChannelGrid);
            this.trackPanel.Controls.Add(this.trackBiquadType);
            this.trackPanel.Controls.Add(this.tableLayoutPanel33);
            this.trackPanel.Controls.Add(this.trackBiquadValue);
            this.trackPanel.Controls.Add(this.label68);
            this.trackPanel.Controls.Add(this.label67);
            this.trackPanel.Controls.Add(this.trackLPFFrequency);
            this.trackPanel.Controls.Add(this.label66);
            this.trackPanel.Controls.Add(this.trackSurround);
            this.trackPanel.Controls.Add(this.trackSpan);
            this.trackPanel.Controls.Add(this.label65);
            this.trackPanel.Controls.Add(this.trackPan);
            this.trackPanel.Controls.Add(this.label64);
            this.trackPanel.Controls.Add(this.trackVolume);
            this.trackPanel.Controls.Add(this.label63);
            this.trackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackPanel.Location = new System.Drawing.Point(0, 0);
            this.trackPanel.Name = "trackPanel";
            this.trackPanel.Size = new System.Drawing.Size(303, 472);
            this.trackPanel.TabIndex = 34;
            // 
            // trackChannelGrid
            // 
            this.trackChannelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackChannelGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.trackChannelGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.channel});
            this.trackChannelGrid.Location = new System.Drawing.Point(6, 336);
            this.trackChannelGrid.Name = "trackChannelGrid";
            this.trackChannelGrid.Size = new System.Drawing.Size(291, 129);
            this.trackChannelGrid.TabIndex = 54;
            this.trackChannelGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.TrackChannelsChanged);
            this.trackChannelGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.TrackChannelsChanged);
            // 
            // channel
            // 
            this.channel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.channel.HeaderText = "Channels";
            this.channel.Name = "channel";
            // 
            // trackBiquadType
            // 
            this.trackBiquadType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBiquadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackBiquadType.FormattingEnabled = true;
            this.trackBiquadType.Items.AddRange(new object[] {
            "Unused",
            "LPF",
            "HPF",
            "BPF 512Hz",
            "BPF 1024Hz",
            "BPF 2048Hz",
            "User Definition 0",
            "User Definition 1",
            "User Definition 2",
            "User Definition 3",
            "User Definition 4",
            "User Definition 5",
            "User Definition 6",
            "User Definition 7",
            "User Definition 8",
            "User Definition 9",
            "User Definition 10",
            "User Definition 11",
            "User Definition 12",
            "User Definition 13",
            "User Definition 14",
            "User Definition 15",
            "User Definition 16",
            "User Definition 17",
            "User Definition 18",
            "User Definition 19",
            "User Definition 20",
            "User Definition 21",
            "User Definition 22",
            "User Definition 23",
            "User Definition 24",
            "User Definition 25",
            "User Definition 26",
            "User Definition 27",
            "User Definition 28",
            "User Definition 29",
            "User Definition 30",
            "User Definition 31",
            "User Definition 32",
            "User Definition 33",
            "User Definition 34",
            "User Definition 35",
            "User Definition 36",
            "User Definition 37",
            "User Definition 38",
            "User Definition 39",
            "User Definition 40",
            "User Definition 41",
            "User Definition 42",
            "User Definition 43",
            "User Definition 44",
            "User Definition 45",
            "User Definition 46",
            "User Definition 47",
            "User Definition 48",
            "User Definition 49",
            "User Definition 50",
            "User Definition 51",
            "User Definition 52",
            "User Definition 53",
            "User Definition 54",
            "User Definition 55",
            "User Definition 56",
            "User Definition 57",
            "User Definition 58",
            "User Definition 59",
            "User Definition 60",
            "User Definition 61",
            "User Definition 62",
            "User Definition 63"});
            this.trackBiquadType.Location = new System.Drawing.Point(8, 203);
            this.trackBiquadType.Name = "trackBiquadType";
            this.trackBiquadType.Size = new System.Drawing.Size(287, 21);
            this.trackBiquadType.TabIndex = 52;
            this.trackBiquadType.SelectedIndexChanged += new System.EventHandler(this.TrackBiquadType_SelectedIndexChanged);
            // 
            // tableLayoutPanel33
            // 
            this.tableLayoutPanel33.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel33.ColumnCount = 4;
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel33.Controls.Add(this.label69, 0, 0);
            this.tableLayoutPanel33.Controls.Add(this.trackSendC, 3, 1);
            this.tableLayoutPanel33.Controls.Add(this.label70, 1, 0);
            this.tableLayoutPanel33.Controls.Add(this.trackSendB, 2, 1);
            this.tableLayoutPanel33.Controls.Add(this.label71, 2, 0);
            this.tableLayoutPanel33.Controls.Add(this.trackSendA, 1, 1);
            this.tableLayoutPanel33.Controls.Add(this.label72, 3, 0);
            this.tableLayoutPanel33.Controls.Add(this.trackSendMain, 0, 1);
            this.tableLayoutPanel33.Location = new System.Drawing.Point(7, 278);
            this.tableLayoutPanel33.Name = "tableLayoutPanel33";
            this.tableLayoutPanel33.RowCount = 2;
            this.tableLayoutPanel33.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel33.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel33.Size = new System.Drawing.Size(290, 54);
            this.tableLayoutPanel33.TabIndex = 51;
            // 
            // label69
            // 
            this.label69.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label69.Location = new System.Drawing.Point(3, 0);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(66, 27);
            this.label69.TabIndex = 27;
            this.label69.Text = "Send Main:";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackSendC
            // 
            this.trackSendC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackSendC.Location = new System.Drawing.Point(219, 30);
            this.trackSendC.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackSendC.Name = "trackSendC";
            this.trackSendC.Size = new System.Drawing.Size(68, 20);
            this.trackSendC.TabIndex = 34;
            this.trackSendC.ValueChanged += new System.EventHandler(this.TrackSendC_ValueChanged);
            // 
            // label70
            // 
            this.label70.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label70.Location = new System.Drawing.Point(75, 0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(66, 27);
            this.label70.TabIndex = 29;
            this.label70.Text = "Send A:";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackSendB
            // 
            this.trackSendB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackSendB.Location = new System.Drawing.Point(147, 30);
            this.trackSendB.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackSendB.Name = "trackSendB";
            this.trackSendB.Size = new System.Drawing.Size(66, 20);
            this.trackSendB.TabIndex = 33;
            this.trackSendB.ValueChanged += new System.EventHandler(this.TrackSendB_ValueChanged);
            // 
            // label71
            // 
            this.label71.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label71.Location = new System.Drawing.Point(147, 0);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(66, 27);
            this.label71.TabIndex = 28;
            this.label71.Text = "Send B:";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackSendA
            // 
            this.trackSendA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackSendA.Location = new System.Drawing.Point(75, 30);
            this.trackSendA.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackSendA.Name = "trackSendA";
            this.trackSendA.Size = new System.Drawing.Size(66, 20);
            this.trackSendA.TabIndex = 32;
            this.trackSendA.ValueChanged += new System.EventHandler(this.TrackSendA_ValueChanged);
            // 
            // label72
            // 
            this.label72.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label72.Location = new System.Drawing.Point(219, 0);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(68, 27);
            this.label72.TabIndex = 30;
            this.label72.Text = "Send C:";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackSendMain
            // 
            this.trackSendMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackSendMain.Location = new System.Drawing.Point(3, 30);
            this.trackSendMain.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackSendMain.Name = "trackSendMain";
            this.trackSendMain.Size = new System.Drawing.Size(66, 20);
            this.trackSendMain.TabIndex = 31;
            this.trackSendMain.ValueChanged += new System.EventHandler(this.TrackSendMain_ValueChanged);
            // 
            // trackBiquadValue
            // 
            this.trackBiquadValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBiquadValue.Location = new System.Drawing.Point(9, 246);
            this.trackBiquadValue.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackBiquadValue.Name = "trackBiquadValue";
            this.trackBiquadValue.Size = new System.Drawing.Size(287, 20);
            this.trackBiquadValue.TabIndex = 50;
            this.trackBiquadValue.ValueChanged += new System.EventHandler(this.TrackBiquadValue_ValueChanged);
            // 
            // label68
            // 
            this.label68.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label68.Location = new System.Drawing.Point(6, 227);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(292, 17);
            this.label68.TabIndex = 49;
            this.label68.Text = "Biquad Value:";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label67
            // 
            this.label67.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label67.Location = new System.Drawing.Point(5, 184);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(292, 17);
            this.label67.TabIndex = 47;
            this.label67.Text = "Biquad Type:";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackLPFFrequency
            // 
            this.trackLPFFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackLPFFrequency.Location = new System.Drawing.Point(8, 162);
            this.trackLPFFrequency.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackLPFFrequency.Name = "trackLPFFrequency";
            this.trackLPFFrequency.Size = new System.Drawing.Size(287, 20);
            this.trackLPFFrequency.TabIndex = 46;
            this.trackLPFFrequency.ValueChanged += new System.EventHandler(this.TrackLPFFrequency_ValueChanged);
            // 
            // label66
            // 
            this.label66.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label66.Location = new System.Drawing.Point(5, 143);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(292, 17);
            this.label66.TabIndex = 45;
            this.label66.Text = "LPF Frequency:";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackSurround
            // 
            this.trackSurround.AutoSize = true;
            this.trackSurround.Location = new System.Drawing.Point(8, 129);
            this.trackSurround.Name = "trackSurround";
            this.trackSurround.Size = new System.Drawing.Size(69, 17);
            this.trackSurround.TabIndex = 44;
            this.trackSurround.Text = "Surround";
            this.trackSurround.UseVisualStyleBackColor = true;
            this.trackSurround.CheckedChanged += new System.EventHandler(this.TrackSurround_CheckedChanged);
            // 
            // trackSpan
            // 
            this.trackSpan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackSpan.Location = new System.Drawing.Point(8, 102);
            this.trackSpan.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackSpan.Name = "trackSpan";
            this.trackSpan.Size = new System.Drawing.Size(287, 20);
            this.trackSpan.TabIndex = 43;
            this.trackSpan.ValueChanged += new System.EventHandler(this.TrackSpan_ValueChanged);
            // 
            // label65
            // 
            this.label65.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label65.Location = new System.Drawing.Point(5, 83);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(292, 17);
            this.label65.TabIndex = 42;
            this.label65.Text = "Surround Pan:";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackPan
            // 
            this.trackPan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPan.Location = new System.Drawing.Point(8, 63);
            this.trackPan.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.trackPan.Name = "trackPan";
            this.trackPan.Size = new System.Drawing.Size(287, 20);
            this.trackPan.TabIndex = 41;
            this.trackPan.ValueChanged += new System.EventHandler(this.TrackPan_ValueChanged);
            // 
            // label64
            // 
            this.label64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label64.Location = new System.Drawing.Point(5, 44);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(292, 17);
            this.label64.TabIndex = 40;
            this.label64.Text = "Pan:";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackVolume
            // 
            this.trackVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackVolume.Location = new System.Drawing.Point(8, 22);
            this.trackVolume.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(287, 20);
            this.trackVolume.TabIndex = 39;
            this.trackVolume.ValueChanged += new System.EventHandler(this.TrackVolume_ValueChanged);
            // 
            // label63
            // 
            this.label63.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label63.Location = new System.Drawing.Point(5, 3);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(292, 17);
            this.label63.TabIndex = 38;
            this.label63.Text = "Volume:";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmPanel
            // 
            this.stmPanel.Controls.Add(this.tableLayoutPanel29);
            this.stmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmPanel.Location = new System.Drawing.Point(0, 0);
            this.stmPanel.Name = "stmPanel";
            this.stmPanel.Size = new System.Drawing.Size(303, 472);
            this.stmPanel.TabIndex = 33;
            // 
            // tableLayoutPanel29
            // 
            this.tableLayoutPanel29.ColumnCount = 1;
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel29.Controls.Add(this.label61, 0, 0);
            this.tableLayoutPanel29.Controls.Add(this.tabControl1, 0, 5);
            this.tableLayoutPanel29.Controls.Add(this.stmFileIdBox, 0, 1);
            this.tableLayoutPanel29.Controls.Add(this.tableLayoutPanel30, 0, 2);
            this.tableLayoutPanel29.Controls.Add(this.tableLayoutPanel31, 0, 3);
            this.tableLayoutPanel29.Controls.Add(this.tableLayoutPanel32, 0, 4);
            this.tableLayoutPanel29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel29.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel29.Name = "tableLayoutPanel29";
            this.tableLayoutPanel29.RowCount = 6;
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel29.Size = new System.Drawing.Size(303, 472);
            this.tableLayoutPanel29.TabIndex = 37;
            // 
            // label61
            // 
            this.label61.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label61.Location = new System.Drawing.Point(3, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(297, 20);
            this.label61.TabIndex = 6;
            this.label61.Text = "File Id:";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 178);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(297, 291);
            this.tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label48);
            this.tabPage1.Controls.Add(this.tableLayoutPanel27);
            this.tabPage1.Controls.Add(this.label51);
            this.tabPage1.Controls.Add(this.stmSoundInfoButton);
            this.tabPage1.Controls.Add(this.stmPitch);
            this.tabPage1.Controls.Add(this.stmSound3dButton);
            this.tabPage1.Controls.Add(this.label56);
            this.tabPage1.Controls.Add(this.stmSound3dEnable);
            this.tabPage1.Controls.Add(this.stmAllocateChannelsNum);
            this.tabPage1.Controls.Add(this.stmWriteTrackInfo);
            this.tabPage1.Controls.Add(this.stmCopyChannelCountFromFile);
            this.tabPage1.Controls.Add(this.stmUpdateTrackInfo);
            this.tabPage1.Controls.Add(this.stmAllocateChannels);
            this.tabPage1.Controls.Add(this.stmStreamType);
            this.tabPage1.Controls.Add(this.label57);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(289, 265);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Page 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label48.Location = new System.Drawing.Point(1, 141);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(286, 17);
            this.label48.TabIndex = 37;
            this.label48.Text = "Track Flags:";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel27
            // 
            this.tableLayoutPanel27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel27.ColumnCount = 4;
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.Controls.Add(this.stmTrack12, 0, 3);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack0, 0, 0);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack1, 1, 0);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack2, 2, 0);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack3, 3, 0);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack4, 0, 1);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack5, 1, 1);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack6, 2, 1);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack7, 3, 1);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack8, 0, 2);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack9, 1, 2);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack10, 2, 2);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack11, 3, 2);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack15, 3, 3);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack14, 2, 3);
            this.tableLayoutPanel27.Controls.Add(this.stmTrack13, 1, 3);
            this.tableLayoutPanel27.Location = new System.Drawing.Point(4, 161);
            this.tableLayoutPanel27.Name = "tableLayoutPanel27";
            this.tableLayoutPanel27.RowCount = 4;
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.Size = new System.Drawing.Size(280, 94);
            this.tableLayoutPanel27.TabIndex = 36;
            // 
            // stmTrack12
            // 
            this.stmTrack12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack12.Location = new System.Drawing.Point(3, 72);
            this.stmTrack12.Name = "stmTrack12";
            this.stmTrack12.Size = new System.Drawing.Size(64, 19);
            this.stmTrack12.TabIndex = 35;
            this.stmTrack12.Text = "12";
            this.stmTrack12.UseVisualStyleBackColor = true;
            this.stmTrack12.CheckedChanged += new System.EventHandler(this.StmTrack12_CheckedChanged);
            // 
            // stmTrack0
            // 
            this.stmTrack0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack0.Location = new System.Drawing.Point(3, 3);
            this.stmTrack0.Name = "stmTrack0";
            this.stmTrack0.Size = new System.Drawing.Size(64, 17);
            this.stmTrack0.TabIndex = 20;
            this.stmTrack0.Text = "0";
            this.stmTrack0.UseVisualStyleBackColor = true;
            this.stmTrack0.CheckedChanged += new System.EventHandler(this.StmTrack0_CheckedChanged);
            // 
            // stmTrack1
            // 
            this.stmTrack1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack1.Location = new System.Drawing.Point(73, 3);
            this.stmTrack1.Name = "stmTrack1";
            this.stmTrack1.Size = new System.Drawing.Size(64, 17);
            this.stmTrack1.TabIndex = 21;
            this.stmTrack1.Text = "1";
            this.stmTrack1.UseVisualStyleBackColor = true;
            this.stmTrack1.CheckedChanged += new System.EventHandler(this.StmTrack1_CheckedChanged);
            // 
            // stmTrack2
            // 
            this.stmTrack2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack2.Location = new System.Drawing.Point(143, 3);
            this.stmTrack2.Name = "stmTrack2";
            this.stmTrack2.Size = new System.Drawing.Size(64, 17);
            this.stmTrack2.TabIndex = 22;
            this.stmTrack2.Text = "2";
            this.stmTrack2.UseVisualStyleBackColor = true;
            this.stmTrack2.CheckedChanged += new System.EventHandler(this.StmTrack2_CheckedChanged);
            // 
            // stmTrack3
            // 
            this.stmTrack3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack3.Location = new System.Drawing.Point(213, 3);
            this.stmTrack3.Name = "stmTrack3";
            this.stmTrack3.Size = new System.Drawing.Size(64, 17);
            this.stmTrack3.TabIndex = 23;
            this.stmTrack3.Text = "3";
            this.stmTrack3.UseVisualStyleBackColor = true;
            this.stmTrack3.CheckedChanged += new System.EventHandler(this.StmTrack3_CheckedChanged);
            // 
            // stmTrack4
            // 
            this.stmTrack4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack4.Location = new System.Drawing.Point(3, 26);
            this.stmTrack4.Name = "stmTrack4";
            this.stmTrack4.Size = new System.Drawing.Size(64, 17);
            this.stmTrack4.TabIndex = 24;
            this.stmTrack4.Text = "4";
            this.stmTrack4.UseVisualStyleBackColor = true;
            this.stmTrack4.CheckedChanged += new System.EventHandler(this.StmTrack4_CheckedChanged);
            // 
            // stmTrack5
            // 
            this.stmTrack5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack5.Location = new System.Drawing.Point(73, 26);
            this.stmTrack5.Name = "stmTrack5";
            this.stmTrack5.Size = new System.Drawing.Size(64, 17);
            this.stmTrack5.TabIndex = 25;
            this.stmTrack5.Text = "5";
            this.stmTrack5.UseVisualStyleBackColor = true;
            this.stmTrack5.CheckedChanged += new System.EventHandler(this.StmTrack5_CheckedChanged);
            // 
            // stmTrack6
            // 
            this.stmTrack6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack6.Location = new System.Drawing.Point(143, 26);
            this.stmTrack6.Name = "stmTrack6";
            this.stmTrack6.Size = new System.Drawing.Size(64, 17);
            this.stmTrack6.TabIndex = 26;
            this.stmTrack6.Text = "6";
            this.stmTrack6.UseVisualStyleBackColor = true;
            this.stmTrack6.CheckedChanged += new System.EventHandler(this.StmTrack6_CheckedChanged);
            // 
            // stmTrack7
            // 
            this.stmTrack7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack7.Location = new System.Drawing.Point(213, 26);
            this.stmTrack7.Name = "stmTrack7";
            this.stmTrack7.Size = new System.Drawing.Size(64, 17);
            this.stmTrack7.TabIndex = 27;
            this.stmTrack7.Text = "7";
            this.stmTrack7.UseVisualStyleBackColor = true;
            this.stmTrack7.CheckedChanged += new System.EventHandler(this.StmTrack7_CheckedChanged);
            // 
            // stmTrack8
            // 
            this.stmTrack8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack8.Location = new System.Drawing.Point(3, 49);
            this.stmTrack8.Name = "stmTrack8";
            this.stmTrack8.Size = new System.Drawing.Size(64, 17);
            this.stmTrack8.TabIndex = 28;
            this.stmTrack8.Text = "8";
            this.stmTrack8.UseVisualStyleBackColor = true;
            this.stmTrack8.CheckedChanged += new System.EventHandler(this.StmTrack8_CheckedChanged);
            // 
            // stmTrack9
            // 
            this.stmTrack9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack9.Location = new System.Drawing.Point(73, 49);
            this.stmTrack9.Name = "stmTrack9";
            this.stmTrack9.Size = new System.Drawing.Size(64, 17);
            this.stmTrack9.TabIndex = 29;
            this.stmTrack9.Text = "9";
            this.stmTrack9.UseVisualStyleBackColor = true;
            this.stmTrack9.CheckedChanged += new System.EventHandler(this.StmTrack9_CheckedChanged);
            // 
            // stmTrack10
            // 
            this.stmTrack10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack10.Location = new System.Drawing.Point(143, 49);
            this.stmTrack10.Name = "stmTrack10";
            this.stmTrack10.Size = new System.Drawing.Size(64, 17);
            this.stmTrack10.TabIndex = 30;
            this.stmTrack10.Text = "10";
            this.stmTrack10.UseVisualStyleBackColor = true;
            this.stmTrack10.CheckedChanged += new System.EventHandler(this.StmTrack10_CheckedChanged);
            // 
            // stmTrack11
            // 
            this.stmTrack11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack11.Location = new System.Drawing.Point(213, 49);
            this.stmTrack11.Name = "stmTrack11";
            this.stmTrack11.Size = new System.Drawing.Size(64, 17);
            this.stmTrack11.TabIndex = 31;
            this.stmTrack11.Text = "11";
            this.stmTrack11.UseVisualStyleBackColor = true;
            this.stmTrack11.CheckedChanged += new System.EventHandler(this.StmTrack11_CheckedChanged);
            // 
            // stmTrack15
            // 
            this.stmTrack15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack15.Location = new System.Drawing.Point(213, 72);
            this.stmTrack15.Name = "stmTrack15";
            this.stmTrack15.Size = new System.Drawing.Size(64, 19);
            this.stmTrack15.TabIndex = 32;
            this.stmTrack15.Text = "15";
            this.stmTrack15.UseVisualStyleBackColor = true;
            this.stmTrack15.CheckedChanged += new System.EventHandler(this.StmTrack15_CheckedChanged);
            // 
            // stmTrack14
            // 
            this.stmTrack14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack14.Location = new System.Drawing.Point(143, 72);
            this.stmTrack14.Name = "stmTrack14";
            this.stmTrack14.Size = new System.Drawing.Size(64, 19);
            this.stmTrack14.TabIndex = 33;
            this.stmTrack14.Text = "14";
            this.stmTrack14.UseVisualStyleBackColor = true;
            this.stmTrack14.CheckedChanged += new System.EventHandler(this.StmTrack14_CheckedChanged);
            // 
            // stmTrack13
            // 
            this.stmTrack13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmTrack13.Location = new System.Drawing.Point(73, 72);
            this.stmTrack13.Name = "stmTrack13";
            this.stmTrack13.Size = new System.Drawing.Size(64, 19);
            this.stmTrack13.TabIndex = 34;
            this.stmTrack13.Text = "13";
            this.stmTrack13.UseVisualStyleBackColor = true;
            this.stmTrack13.CheckedChanged += new System.EventHandler(this.StmTrack13_CheckedChanged);
            // 
            // label51
            // 
            this.label51.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label51.Location = new System.Drawing.Point(3, 3);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(298, 17);
            this.label51.TabIndex = 0;
            this.label51.Text = "Sound Info:";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmSoundInfoButton
            // 
            this.stmSoundInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmSoundInfoButton.Location = new System.Drawing.Point(5, 21);
            this.stmSoundInfoButton.Name = "stmSoundInfoButton";
            this.stmSoundInfoButton.Size = new System.Drawing.Size(278, 23);
            this.stmSoundInfoButton.TabIndex = 1;
            this.stmSoundInfoButton.Text = "Edit Sound Info";
            this.stmSoundInfoButton.UseVisualStyleBackColor = true;
            this.stmSoundInfoButton.Click += new System.EventHandler(this.StmSoundInfoButton_Click);
            // 
            // stmPitch
            // 
            this.stmPitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmPitch.DecimalPlaces = 7;
            this.stmPitch.Location = new System.Drawing.Point(7, 393);
            this.stmPitch.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.stmPitch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.stmPitch.Name = "stmPitch";
            this.stmPitch.Size = new System.Drawing.Size(274, 20);
            this.stmPitch.TabIndex = 26;
            this.stmPitch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.stmPitch.ValueChanged += new System.EventHandler(this.StmPitch_ValueChanged);
            // 
            // stmSound3dButton
            // 
            this.stmSound3dButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmSound3dButton.Location = new System.Drawing.Point(5, 68);
            this.stmSound3dButton.Name = "stmSound3dButton";
            this.stmSound3dButton.Size = new System.Drawing.Size(278, 23);
            this.stmSound3dButton.TabIndex = 3;
            this.stmSound3dButton.Text = "Edit Sound 3d Info";
            this.stmSound3dButton.UseVisualStyleBackColor = true;
            this.stmSound3dButton.Click += new System.EventHandler(this.StmSound3dButton_Click);
            // 
            // label56
            // 
            this.label56.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label56.Location = new System.Drawing.Point(0, 374);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(286, 16);
            this.label56.TabIndex = 25;
            this.label56.Text = "Pitch:";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmSound3dEnable
            // 
            this.stmSound3dEnable.AutoSize = true;
            this.stmSound3dEnable.Location = new System.Drawing.Point(6, 51);
            this.stmSound3dEnable.Name = "stmSound3dEnable";
            this.stmSound3dEnable.Size = new System.Drawing.Size(129, 17);
            this.stmSound3dEnable.TabIndex = 12;
            this.stmSound3dEnable.Text = "Enable Sound 3d Info";
            this.stmSound3dEnable.UseVisualStyleBackColor = true;
            this.stmSound3dEnable.CheckedChanged += new System.EventHandler(this.StmSound3dEnable_CheckedChanged);
            // 
            // stmAllocateChannelsNum
            // 
            this.stmAllocateChannelsNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmAllocateChannelsNum.Location = new System.Drawing.Point(7, 324);
            this.stmAllocateChannelsNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.stmAllocateChannelsNum.Name = "stmAllocateChannelsNum";
            this.stmAllocateChannelsNum.Size = new System.Drawing.Size(274, 20);
            this.stmAllocateChannelsNum.TabIndex = 24;
            this.stmAllocateChannelsNum.ValueChanged += new System.EventHandler(this.StmAllocateChannelsNum_ValueChanged);
            // 
            // stmWriteTrackInfo
            // 
            this.stmWriteTrackInfo.AutoSize = true;
            this.stmWriteTrackInfo.Location = new System.Drawing.Point(5, 97);
            this.stmWriteTrackInfo.Name = "stmWriteTrackInfo";
            this.stmWriteTrackInfo.Size = new System.Drawing.Size(103, 17);
            this.stmWriteTrackInfo.TabIndex = 13;
            this.stmWriteTrackInfo.Text = "Write Track Info";
            this.stmWriteTrackInfo.UseVisualStyleBackColor = true;
            this.stmWriteTrackInfo.CheckedChanged += new System.EventHandler(this.StmWriteTrackInfo_CheckedChanged);
            // 
            // stmCopyChannelCountFromFile
            // 
            this.stmCopyChannelCountFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmCopyChannelCountFromFile.Location = new System.Drawing.Point(5, 348);
            this.stmCopyChannelCountFromFile.Name = "stmCopyChannelCountFromFile";
            this.stmCopyChannelCountFromFile.Size = new System.Drawing.Size(278, 23);
            this.stmCopyChannelCountFromFile.TabIndex = 23;
            this.stmCopyChannelCountFromFile.Text = "Copy Channel Count From Stream File";
            this.stmCopyChannelCountFromFile.UseVisualStyleBackColor = true;
            this.stmCopyChannelCountFromFile.Click += new System.EventHandler(this.StmCopyChannelCountFromFile_Click);
            // 
            // stmUpdateTrackInfo
            // 
            this.stmUpdateTrackInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmUpdateTrackInfo.Location = new System.Drawing.Point(5, 115);
            this.stmUpdateTrackInfo.Name = "stmUpdateTrackInfo";
            this.stmUpdateTrackInfo.Size = new System.Drawing.Size(278, 23);
            this.stmUpdateTrackInfo.TabIndex = 14;
            this.stmUpdateTrackInfo.Text = "Update Track Info From File";
            this.stmUpdateTrackInfo.UseVisualStyleBackColor = true;
            this.stmUpdateTrackInfo.Click += new System.EventHandler(this.StmUpdateTrackInfo_Click);
            // 
            // stmAllocateChannels
            // 
            this.stmAllocateChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmAllocateChannels.Location = new System.Drawing.Point(1, 304);
            this.stmAllocateChannels.Name = "stmAllocateChannels";
            this.stmAllocateChannels.Size = new System.Drawing.Size(286, 17);
            this.stmAllocateChannels.TabIndex = 22;
            this.stmAllocateChannels.Text = "Number Of Channels To Allocate:";
            this.stmAllocateChannels.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmStreamType
            // 
            this.stmStreamType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmStreamType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stmStreamType.FormattingEnabled = true;
            this.stmStreamType.Items.AddRange(new object[] {
            "Invalid",
            "Binary",
            "ADTS"});
            this.stmStreamType.Location = new System.Drawing.Point(7, 280);
            this.stmStreamType.Name = "stmStreamType";
            this.stmStreamType.Size = new System.Drawing.Size(276, 21);
            this.stmStreamType.TabIndex = 21;
            this.stmStreamType.SelectedIndexChanged += new System.EventHandler(this.StmStreamType_SelectedIndexChanged);
            // 
            // label57
            // 
            this.label57.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label57.Location = new System.Drawing.Point(2, 260);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(286, 17);
            this.label57.TabIndex = 20;
            this.label57.Text = "Stream Type:";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.stmCopyExtensionFromFile);
            this.tabPage2.Controls.Add(this.tableLayoutPanel28);
            this.tabPage2.Controls.Add(this.stmGeneratePrefetch);
            this.tabPage2.Controls.Add(this.label58);
            this.tabPage2.Controls.Add(this.stmPrefetchFileIdBox);
            this.tabPage2.Controls.Add(this.stmCreateUniquePrefetchFile);
            this.tabPage2.Controls.Add(this.stmUpdatePrefetchInfo);
            this.tabPage2.Controls.Add(this.stmLoopEndFrame);
            this.tabPage2.Controls.Add(this.label59);
            this.tabPage2.Controls.Add(this.stmLoopStartFrame);
            this.tabPage2.Controls.Add(this.label60);
            this.tabPage2.Controls.Add(this.stmIncludeExtension);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(289, 265);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Page 2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // stmCopyExtensionFromFile
            // 
            this.stmCopyExtensionFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmCopyExtensionFromFile.Location = new System.Drawing.Point(1, 113);
            this.stmCopyExtensionFromFile.Name = "stmCopyExtensionFromFile";
            this.stmCopyExtensionFromFile.Size = new System.Drawing.Size(289, 23);
            this.stmCopyExtensionFromFile.TabIndex = 37;
            this.stmCopyExtensionFromFile.Text = "Copy Extension Data From Stream File";
            this.stmCopyExtensionFromFile.UseVisualStyleBackColor = true;
            this.stmCopyExtensionFromFile.Click += new System.EventHandler(this.StmCopyExtensionFromFile_Click);
            // 
            // tableLayoutPanel28
            // 
            this.tableLayoutPanel28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel28.ColumnCount = 4;
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.Controls.Add(this.label52, 0, 0);
            this.tableLayoutPanel28.Controls.Add(this.stmSendC, 3, 1);
            this.tableLayoutPanel28.Controls.Add(this.label53, 1, 0);
            this.tableLayoutPanel28.Controls.Add(this.stmSendB, 2, 1);
            this.tableLayoutPanel28.Controls.Add(this.label54, 2, 0);
            this.tableLayoutPanel28.Controls.Add(this.stmSendA, 1, 1);
            this.tableLayoutPanel28.Controls.Add(this.label55, 3, 0);
            this.tableLayoutPanel28.Controls.Add(this.stmSendMain, 0, 1);
            this.tableLayoutPanel28.Location = new System.Drawing.Point(-1, 264);
            this.tableLayoutPanel28.Name = "tableLayoutPanel28";
            this.tableLayoutPanel28.RowCount = 2;
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel28.Size = new System.Drawing.Size(281, 54);
            this.tableLayoutPanel28.TabIndex = 36;
            // 
            // label52
            // 
            this.label52.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label52.Location = new System.Drawing.Point(3, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(64, 27);
            this.label52.TabIndex = 27;
            this.label52.Text = "Send Main:";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmSendC
            // 
            this.stmSendC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmSendC.Location = new System.Drawing.Point(213, 30);
            this.stmSendC.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.stmSendC.Name = "stmSendC";
            this.stmSendC.Size = new System.Drawing.Size(65, 20);
            this.stmSendC.TabIndex = 34;
            this.stmSendC.ValueChanged += new System.EventHandler(this.StmSendC_ValueChanged);
            // 
            // label53
            // 
            this.label53.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label53.Location = new System.Drawing.Point(73, 0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(64, 27);
            this.label53.TabIndex = 29;
            this.label53.Text = "Send A:";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmSendB
            // 
            this.stmSendB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmSendB.Location = new System.Drawing.Point(143, 30);
            this.stmSendB.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.stmSendB.Name = "stmSendB";
            this.stmSendB.Size = new System.Drawing.Size(64, 20);
            this.stmSendB.TabIndex = 33;
            this.stmSendB.ValueChanged += new System.EventHandler(this.StmSendB_ValueChanged);
            // 
            // label54
            // 
            this.label54.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label54.Location = new System.Drawing.Point(143, 0);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(64, 27);
            this.label54.TabIndex = 28;
            this.label54.Text = "Send B:";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmSendA
            // 
            this.stmSendA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmSendA.Location = new System.Drawing.Point(73, 30);
            this.stmSendA.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.stmSendA.Name = "stmSendA";
            this.stmSendA.Size = new System.Drawing.Size(64, 20);
            this.stmSendA.TabIndex = 32;
            this.stmSendA.ValueChanged += new System.EventHandler(this.StmSendA_ValueChanged);
            // 
            // label55
            // 
            this.label55.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label55.Location = new System.Drawing.Point(213, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(65, 27);
            this.label55.TabIndex = 30;
            this.label55.Text = "Send C:";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmSendMain
            // 
            this.stmSendMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmSendMain.Location = new System.Drawing.Point(3, 30);
            this.stmSendMain.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.stmSendMain.Name = "stmSendMain";
            this.stmSendMain.Size = new System.Drawing.Size(64, 20);
            this.stmSendMain.TabIndex = 31;
            this.stmSendMain.ValueChanged += new System.EventHandler(this.StmSendMain_ValueChanged);
            // 
            // stmGeneratePrefetch
            // 
            this.stmGeneratePrefetch.AutoSize = true;
            this.stmGeneratePrefetch.Location = new System.Drawing.Point(5, 139);
            this.stmGeneratePrefetch.Name = "stmGeneratePrefetch";
            this.stmGeneratePrefetch.Size = new System.Drawing.Size(113, 17);
            this.stmGeneratePrefetch.TabIndex = 31;
            this.stmGeneratePrefetch.Text = "Write Prefetch File";
            this.stmGeneratePrefetch.UseVisualStyleBackColor = true;
            this.stmGeneratePrefetch.CheckedChanged += new System.EventHandler(this.StmGeneratePrefetch_CheckedChanged);
            // 
            // label58
            // 
            this.label58.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label58.Location = new System.Drawing.Point(-1, 156);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(286, 17);
            this.label58.TabIndex = 32;
            this.label58.Text = "Prefetch File Id:";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmPrefetchFileIdBox
            // 
            this.stmPrefetchFileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmPrefetchFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stmPrefetchFileIdBox.FormattingEnabled = true;
            this.stmPrefetchFileIdBox.Location = new System.Drawing.Point(0, 176);
            this.stmPrefetchFileIdBox.Name = "stmPrefetchFileIdBox";
            this.stmPrefetchFileIdBox.Size = new System.Drawing.Size(288, 21);
            this.stmPrefetchFileIdBox.TabIndex = 33;
            this.stmPrefetchFileIdBox.SelectedIndexChanged += new System.EventHandler(this.StmPrefetchFileIdBox_SelectedIndexChanged);
            // 
            // stmCreateUniquePrefetchFile
            // 
            this.stmCreateUniquePrefetchFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmCreateUniquePrefetchFile.Location = new System.Drawing.Point(0, 231);
            this.stmCreateUniquePrefetchFile.Name = "stmCreateUniquePrefetchFile";
            this.stmCreateUniquePrefetchFile.Size = new System.Drawing.Size(290, 23);
            this.stmCreateUniquePrefetchFile.TabIndex = 35;
            this.stmCreateUniquePrefetchFile.Text = "Create Unique File For Prefetch Data";
            this.stmCreateUniquePrefetchFile.UseVisualStyleBackColor = true;
            this.stmCreateUniquePrefetchFile.Click += new System.EventHandler(this.StmCreateUniquePrefetchFile_Click);
            // 
            // stmUpdatePrefetchInfo
            // 
            this.stmUpdatePrefetchInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmUpdatePrefetchInfo.Location = new System.Drawing.Point(0, 205);
            this.stmUpdatePrefetchInfo.Name = "stmUpdatePrefetchInfo";
            this.stmUpdatePrefetchInfo.Size = new System.Drawing.Size(290, 23);
            this.stmUpdatePrefetchInfo.TabIndex = 34;
            this.stmUpdatePrefetchInfo.Text = "Update Prefetch Data From Stream File";
            this.stmUpdatePrefetchInfo.UseVisualStyleBackColor = true;
            this.stmUpdatePrefetchInfo.Click += new System.EventHandler(this.StmUpdatePrefetchInfo_Click);
            // 
            // stmLoopEndFrame
            // 
            this.stmLoopEndFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmLoopEndFrame.Location = new System.Drawing.Point(1, 90);
            this.stmLoopEndFrame.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.stmLoopEndFrame.Name = "stmLoopEndFrame";
            this.stmLoopEndFrame.Size = new System.Drawing.Size(289, 20);
            this.stmLoopEndFrame.TabIndex = 28;
            this.stmLoopEndFrame.ValueChanged += new System.EventHandler(this.StmLoopEndFrame_ValueChanged);
            // 
            // label59
            // 
            this.label59.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label59.Location = new System.Drawing.Point(1, 70);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(286, 17);
            this.label59.TabIndex = 27;
            this.label59.Text = "Original Loop End Frame:";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmLoopStartFrame
            // 
            this.stmLoopStartFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmLoopStartFrame.Location = new System.Drawing.Point(1, 45);
            this.stmLoopStartFrame.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.stmLoopStartFrame.Name = "stmLoopStartFrame";
            this.stmLoopStartFrame.Size = new System.Drawing.Size(288, 20);
            this.stmLoopStartFrame.TabIndex = 26;
            this.stmLoopStartFrame.ValueChanged += new System.EventHandler(this.StmLoopStartFrame_ValueChanged);
            // 
            // label60
            // 
            this.label60.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label60.Location = new System.Drawing.Point(0, 25);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(286, 17);
            this.label60.TabIndex = 25;
            this.label60.Text = "Original Loop Start Frame:";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stmIncludeExtension
            // 
            this.stmIncludeExtension.AutoSize = true;
            this.stmIncludeExtension.Location = new System.Drawing.Point(5, 6);
            this.stmIncludeExtension.Name = "stmIncludeExtension";
            this.stmIncludeExtension.Size = new System.Drawing.Size(135, 17);
            this.stmIncludeExtension.TabIndex = 14;
            this.stmIncludeExtension.Text = "Include Extended Data";
            this.stmIncludeExtension.UseVisualStyleBackColor = true;
            this.stmIncludeExtension.CheckedChanged += new System.EventHandler(this.StmIncludeExtension_CheckedChanged);
            // 
            // stmFileIdBox
            // 
            this.stmFileIdBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stmFileIdBox.FormattingEnabled = true;
            this.stmFileIdBox.Location = new System.Drawing.Point(3, 23);
            this.stmFileIdBox.Name = "stmFileIdBox";
            this.stmFileIdBox.Size = new System.Drawing.Size(297, 21);
            this.stmFileIdBox.TabIndex = 7;
            this.stmFileIdBox.SelectedIndexChanged += new System.EventHandler(this.StmFileIdBox_SelectedIndexChanged);
            // 
            // tableLayoutPanel30
            // 
            this.tableLayoutPanel30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel30.ColumnCount = 1;
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel30.Controls.Add(this.stmPlay, 0, 1);
            this.tableLayoutPanel30.Controls.Add(this.label62, 0, 0);
            this.tableLayoutPanel30.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel30.Name = "tableLayoutPanel30";
            this.tableLayoutPanel30.RowCount = 2;
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel30.Size = new System.Drawing.Size(297, 54);
            this.tableLayoutPanel30.TabIndex = 28;
            // 
            // stmPlay
            // 
            this.stmPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stmPlay.Location = new System.Drawing.Point(3, 30);
            this.stmPlay.Name = "stmPlay";
            this.stmPlay.Size = new System.Drawing.Size(291, 21);
            this.stmPlay.TabIndex = 10;
            this.stmPlay.Text = "Play";
            this.stmPlay.UseVisualStyleBackColor = true;
            this.stmPlay.Click += new System.EventHandler(this.StmPlay_Click);
            // 
            // label62
            // 
            this.label62.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label62.Location = new System.Drawing.Point(3, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(291, 27);
            this.label62.TabIndex = 11;
            this.label62.Text = "Sound Player Deluxe™";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel31
            // 
            this.tableLayoutPanel31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel31.ColumnCount = 2;
            this.tableLayoutPanel31.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel31.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel31.Controls.Add(this.stmPause, 0, 0);
            this.tableLayoutPanel31.Controls.Add(this.stmStop, 1, 0);
            this.tableLayoutPanel31.Location = new System.Drawing.Point(3, 113);
            this.tableLayoutPanel31.Name = "tableLayoutPanel31";
            this.tableLayoutPanel31.RowCount = 1;
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel31.Size = new System.Drawing.Size(297, 29);
            this.tableLayoutPanel31.TabIndex = 29;
            // 
            // stmPause
            // 
            this.stmPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmPause.Location = new System.Drawing.Point(3, 3);
            this.stmPause.Name = "stmPause";
            this.stmPause.Size = new System.Drawing.Size(142, 23);
            this.stmPause.TabIndex = 0;
            this.stmPause.Text = "Pause";
            this.stmPause.UseVisualStyleBackColor = true;
            this.stmPause.Click += new System.EventHandler(this.StmPause_Click);
            // 
            // stmStop
            // 
            this.stmStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmStop.Location = new System.Drawing.Point(151, 3);
            this.stmStop.Name = "stmStop";
            this.stmStop.Size = new System.Drawing.Size(143, 23);
            this.stmStop.TabIndex = 1;
            this.stmStop.Text = "Stop";
            this.stmStop.UseVisualStyleBackColor = true;
            this.stmStop.Click += new System.EventHandler(this.StmStop_Click);
            // 
            // tableLayoutPanel32
            // 
            this.tableLayoutPanel32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel32.ColumnCount = 3;
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel32.Controls.Add(this.stmPlayNext, 2, 0);
            this.tableLayoutPanel32.Controls.Add(this.stmPlayLoop, 1, 0);
            this.tableLayoutPanel32.Controls.Add(this.stmPlayOnce, 0, 0);
            this.tableLayoutPanel32.Location = new System.Drawing.Point(3, 148);
            this.tableLayoutPanel32.Name = "tableLayoutPanel32";
            this.tableLayoutPanel32.RowCount = 1;
            this.tableLayoutPanel32.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel32.Size = new System.Drawing.Size(297, 24);
            this.tableLayoutPanel32.TabIndex = 30;
            // 
            // stmPlayNext
            // 
            this.stmPlayNext.AutoSize = true;
            this.stmPlayNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmPlayNext.Location = new System.Drawing.Point(201, 3);
            this.stmPlayNext.Name = "stmPlayNext";
            this.stmPlayNext.Size = new System.Drawing.Size(93, 18);
            this.stmPlayNext.TabIndex = 2;
            this.stmPlayNext.Text = "Play Next";
            this.stmPlayNext.UseVisualStyleBackColor = true;
            // 
            // stmPlayLoop
            // 
            this.stmPlayLoop.AutoSize = true;
            this.stmPlayLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmPlayLoop.Location = new System.Drawing.Point(102, 3);
            this.stmPlayLoop.Name = "stmPlayLoop";
            this.stmPlayLoop.Size = new System.Drawing.Size(93, 18);
            this.stmPlayLoop.TabIndex = 1;
            this.stmPlayLoop.Text = "Play Loop";
            this.stmPlayLoop.UseVisualStyleBackColor = true;
            // 
            // stmPlayOnce
            // 
            this.stmPlayOnce.AutoSize = true;
            this.stmPlayOnce.Checked = true;
            this.stmPlayOnce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stmPlayOnce.Location = new System.Drawing.Point(3, 3);
            this.stmPlayOnce.Name = "stmPlayOnce";
            this.stmPlayOnce.Size = new System.Drawing.Size(93, 18);
            this.stmPlayOnce.TabIndex = 0;
            this.stmPlayOnce.TabStop = true;
            this.stmPlayOnce.Text = "Play Once";
            this.stmPlayOnce.UseVisualStyleBackColor = true;
            // 
            // wsdPanel
            // 
            this.wsdPanel.Controls.Add(this.tableLayoutPanel26);
            this.wsdPanel.Controls.Add(this.tableLayoutPanel25);
            this.wsdPanel.Controls.Add(this.tableLayoutPanel24);
            this.wsdPanel.Controls.Add(this.sarWsdFileIdBox);
            this.wsdPanel.Controls.Add(this.label49);
            this.wsdPanel.Controls.Add(this.wsdFixPriority);
            this.wsdPanel.Controls.Add(this.wsdChannelPriority);
            this.wsdPanel.Controls.Add(this.label40);
            this.wsdPanel.Controls.Add(this.wsdCopyCount);
            this.wsdPanel.Controls.Add(this.wsdTracksToAllocate);
            this.wsdPanel.Controls.Add(this.label45);
            this.wsdPanel.Controls.Add(this.wsdWaveIndex);
            this.wsdPanel.Controls.Add(this.label46);
            this.wsdPanel.Controls.Add(this.wsdSound3dEnable);
            this.wsdPanel.Controls.Add(this.wsdSound3dButton);
            this.wsdPanel.Controls.Add(this.wsdEditSoundInfoButton);
            this.wsdPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wsdPanel.Location = new System.Drawing.Point(0, 0);
            this.wsdPanel.Name = "wsdPanel";
            this.wsdPanel.Size = new System.Drawing.Size(303, 472);
            this.wsdPanel.TabIndex = 32;
            // 
            // tableLayoutPanel26
            // 
            this.tableLayoutPanel26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel26.ColumnCount = 3;
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel26.Controls.Add(this.sarWsdPlayNext, 2, 0);
            this.tableLayoutPanel26.Controls.Add(this.sarWsdPlayLoop, 1, 0);
            this.tableLayoutPanel26.Controls.Add(this.sarWsdPlayOnce, 0, 0);
            this.tableLayoutPanel26.Location = new System.Drawing.Point(4, 146);
            this.tableLayoutPanel26.Name = "tableLayoutPanel26";
            this.tableLayoutPanel26.RowCount = 1;
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel26.Size = new System.Drawing.Size(293, 28);
            this.tableLayoutPanel26.TabIndex = 26;
            // 
            // sarWsdPlayNext
            // 
            this.sarWsdPlayNext.AutoSize = true;
            this.sarWsdPlayNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarWsdPlayNext.Location = new System.Drawing.Point(197, 3);
            this.sarWsdPlayNext.Name = "sarWsdPlayNext";
            this.sarWsdPlayNext.Size = new System.Drawing.Size(93, 22);
            this.sarWsdPlayNext.TabIndex = 2;
            this.sarWsdPlayNext.Text = "Play Next";
            this.sarWsdPlayNext.UseVisualStyleBackColor = true;
            // 
            // sarWsdPlayLoop
            // 
            this.sarWsdPlayLoop.AutoSize = true;
            this.sarWsdPlayLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarWsdPlayLoop.Location = new System.Drawing.Point(100, 3);
            this.sarWsdPlayLoop.Name = "sarWsdPlayLoop";
            this.sarWsdPlayLoop.Size = new System.Drawing.Size(91, 22);
            this.sarWsdPlayLoop.TabIndex = 1;
            this.sarWsdPlayLoop.Text = "Play Loop";
            this.sarWsdPlayLoop.UseVisualStyleBackColor = true;
            // 
            // sarWsdPlayOnce
            // 
            this.sarWsdPlayOnce.AutoSize = true;
            this.sarWsdPlayOnce.Checked = true;
            this.sarWsdPlayOnce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarWsdPlayOnce.Location = new System.Drawing.Point(3, 3);
            this.sarWsdPlayOnce.Name = "sarWsdPlayOnce";
            this.sarWsdPlayOnce.Size = new System.Drawing.Size(91, 22);
            this.sarWsdPlayOnce.TabIndex = 0;
            this.sarWsdPlayOnce.TabStop = true;
            this.sarWsdPlayOnce.Text = "Play Once";
            this.sarWsdPlayOnce.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel25
            // 
            this.tableLayoutPanel25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel25.ColumnCount = 2;
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel25.Controls.Add(this.sarWsdPause, 0, 0);
            this.tableLayoutPanel25.Controls.Add(this.sarWsdStop, 1, 0);
            this.tableLayoutPanel25.Location = new System.Drawing.Point(3, 112);
            this.tableLayoutPanel25.Name = "tableLayoutPanel25";
            this.tableLayoutPanel25.RowCount = 1;
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel25.Size = new System.Drawing.Size(292, 32);
            this.tableLayoutPanel25.TabIndex = 27;
            // 
            // sarWsdPause
            // 
            this.sarWsdPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarWsdPause.Location = new System.Drawing.Point(3, 3);
            this.sarWsdPause.Name = "sarWsdPause";
            this.sarWsdPause.Size = new System.Drawing.Size(140, 26);
            this.sarWsdPause.TabIndex = 0;
            this.sarWsdPause.Text = "Pause";
            this.sarWsdPause.UseVisualStyleBackColor = true;
            this.sarWsdPause.Click += new System.EventHandler(this.SarWsdPause_Click);
            // 
            // sarWsdStop
            // 
            this.sarWsdStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarWsdStop.Location = new System.Drawing.Point(149, 3);
            this.sarWsdStop.Name = "sarWsdStop";
            this.sarWsdStop.Size = new System.Drawing.Size(140, 26);
            this.sarWsdStop.TabIndex = 1;
            this.sarWsdStop.Text = "Stop";
            this.sarWsdStop.UseVisualStyleBackColor = true;
            this.sarWsdStop.Click += new System.EventHandler(this.SarWsdStop_Click);
            // 
            // tableLayoutPanel24
            // 
            this.tableLayoutPanel24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel24.ColumnCount = 1;
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel24.Controls.Add(this.sarWsdPlay, 0, 1);
            this.tableLayoutPanel24.Controls.Add(this.label50, 0, 0);
            this.tableLayoutPanel24.Location = new System.Drawing.Point(2, 50);
            this.tableLayoutPanel24.Name = "tableLayoutPanel24";
            this.tableLayoutPanel24.RowCount = 2;
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel24.Size = new System.Drawing.Size(293, 59);
            this.tableLayoutPanel24.TabIndex = 28;
            // 
            // sarWsdPlay
            // 
            this.sarWsdPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sarWsdPlay.Location = new System.Drawing.Point(3, 32);
            this.sarWsdPlay.Name = "sarWsdPlay";
            this.sarWsdPlay.Size = new System.Drawing.Size(287, 24);
            this.sarWsdPlay.TabIndex = 10;
            this.sarWsdPlay.Text = "Play";
            this.sarWsdPlay.UseVisualStyleBackColor = true;
            this.sarWsdPlay.Click += new System.EventHandler(this.SarWsdPlay_Click);
            // 
            // label50
            // 
            this.label50.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label50.Location = new System.Drawing.Point(3, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(287, 29);
            this.label50.TabIndex = 11;
            this.label50.Text = "Sound Player Deluxe™";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sarWsdFileIdBox
            // 
            this.sarWsdFileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sarWsdFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sarWsdFileIdBox.FormattingEnabled = true;
            this.sarWsdFileIdBox.Location = new System.Drawing.Point(7, 22);
            this.sarWsdFileIdBox.Name = "sarWsdFileIdBox";
            this.sarWsdFileIdBox.Size = new System.Drawing.Size(289, 21);
            this.sarWsdFileIdBox.TabIndex = 25;
            this.sarWsdFileIdBox.SelectedIndexChanged += new System.EventHandler(this.SarWsdFileIdBox_SelectedIndexChanged);
            // 
            // label49
            // 
            this.label49.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label49.Location = new System.Drawing.Point(8, 4);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(286, 17);
            this.label49.TabIndex = 24;
            this.label49.Text = "File Id:";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wsdFixPriority
            // 
            this.wsdFixPriority.AutoSize = true;
            this.wsdFixPriority.Location = new System.Drawing.Point(6, 416);
            this.wsdFixPriority.Name = "wsdFixPriority";
            this.wsdFixPriority.Size = new System.Drawing.Size(128, 17);
            this.wsdFixPriority.TabIndex = 21;
            this.wsdFixPriority.Text = "Fix Priority At Release";
            this.wsdFixPriority.UseVisualStyleBackColor = true;
            this.wsdFixPriority.CheckedChanged += new System.EventHandler(this.WsdFixPriority_CheckedChanged);
            // 
            // wsdChannelPriority
            // 
            this.wsdChannelPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdChannelPriority.Location = new System.Drawing.Point(5, 390);
            this.wsdChannelPriority.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.wsdChannelPriority.Name = "wsdChannelPriority";
            this.wsdChannelPriority.Size = new System.Drawing.Size(290, 20);
            this.wsdChannelPriority.TabIndex = 19;
            this.wsdChannelPriority.ValueChanged += new System.EventHandler(this.WsdChannelPriority_ValueChanged);
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label40.Location = new System.Drawing.Point(3, 373);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(294, 17);
            this.label40.TabIndex = 18;
            this.label40.Text = "Channel Priority:";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wsdCopyCount
            // 
            this.wsdCopyCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdCopyCount.Location = new System.Drawing.Point(5, 344);
            this.wsdCopyCount.Name = "wsdCopyCount";
            this.wsdCopyCount.Size = new System.Drawing.Size(290, 23);
            this.wsdCopyCount.TabIndex = 17;
            this.wsdCopyCount.Text = "Copy Count From Linked Wave";
            this.wsdCopyCount.UseVisualStyleBackColor = true;
            this.wsdCopyCount.Click += new System.EventHandler(this.WsdCopyCount_Click);
            // 
            // wsdTracksToAllocate
            // 
            this.wsdTracksToAllocate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdTracksToAllocate.Location = new System.Drawing.Point(6, 318);
            this.wsdTracksToAllocate.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.wsdTracksToAllocate.Name = "wsdTracksToAllocate";
            this.wsdTracksToAllocate.Size = new System.Drawing.Size(289, 20);
            this.wsdTracksToAllocate.TabIndex = 16;
            this.wsdTracksToAllocate.ValueChanged += new System.EventHandler(this.WsdTracksToAllocate_ValueChanged);
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label45.Location = new System.Drawing.Point(2, 301);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(294, 17);
            this.label45.TabIndex = 15;
            this.label45.Text = "Number Of Tracks To Allocate:";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wsdWaveIndex
            // 
            this.wsdWaveIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdWaveIndex.Location = new System.Drawing.Point(5, 275);
            this.wsdWaveIndex.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.wsdWaveIndex.Name = "wsdWaveIndex";
            this.wsdWaveIndex.Size = new System.Drawing.Size(289, 20);
            this.wsdWaveIndex.TabIndex = 14;
            this.wsdWaveIndex.ValueChanged += new System.EventHandler(this.WsdWaveIndex_ValueChanged);
            // 
            // label46
            // 
            this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label46.Location = new System.Drawing.Point(1, 258);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(294, 17);
            this.label46.TabIndex = 13;
            this.label46.Text = "Wave Index (In WSD File):";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wsdSound3dEnable
            // 
            this.wsdSound3dEnable.AutoSize = true;
            this.wsdSound3dEnable.Location = new System.Drawing.Point(5, 210);
            this.wsdSound3dEnable.Name = "wsdSound3dEnable";
            this.wsdSound3dEnable.Size = new System.Drawing.Size(129, 17);
            this.wsdSound3dEnable.TabIndex = 12;
            this.wsdSound3dEnable.Text = "Enable Sound 3d Info";
            this.wsdSound3dEnable.UseVisualStyleBackColor = true;
            this.wsdSound3dEnable.CheckedChanged += new System.EventHandler(this.WsdSound3dEnable_CheckedChanged);
            // 
            // wsdSound3dButton
            // 
            this.wsdSound3dButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdSound3dButton.Location = new System.Drawing.Point(4, 227);
            this.wsdSound3dButton.Name = "wsdSound3dButton";
            this.wsdSound3dButton.Size = new System.Drawing.Size(290, 23);
            this.wsdSound3dButton.TabIndex = 3;
            this.wsdSound3dButton.Text = "Edit Sound 3d Info";
            this.wsdSound3dButton.UseVisualStyleBackColor = true;
            this.wsdSound3dButton.Click += new System.EventHandler(this.WsdSound3dButton_Click);
            // 
            // wsdEditSoundInfoButton
            // 
            this.wsdEditSoundInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsdEditSoundInfoButton.Location = new System.Drawing.Point(4, 180);
            this.wsdEditSoundInfoButton.Name = "wsdEditSoundInfoButton";
            this.wsdEditSoundInfoButton.Size = new System.Drawing.Size(290, 23);
            this.wsdEditSoundInfoButton.TabIndex = 1;
            this.wsdEditSoundInfoButton.Text = "Edit Sound Info";
            this.wsdEditSoundInfoButton.UseVisualStyleBackColor = true;
            this.wsdEditSoundInfoButton.Click += new System.EventHandler(this.WsdEditSoundInfoButton_Click);
            // 
            // seqPanel
            // 
            this.seqPanel.Controls.Add(this.tableLayoutPanel23);
            this.seqPanel.Controls.Add(this.tableLayoutPanel22);
            this.seqPanel.Controls.Add(this.tableLayoutPanel21);
            this.seqPanel.Controls.Add(this.sarSeqFileIdBox);
            this.seqPanel.Controls.Add(this.label47);
            this.seqPanel.Controls.Add(this.tableLayoutPanel19);
            this.seqPanel.Controls.Add(this.label39);
            this.seqPanel.Controls.Add(this.seqIsReleasePriorityBox);
            this.seqPanel.Controls.Add(this.seqChannelPriorityBox);
            this.seqPanel.Controls.Add(this.label41);
            this.seqPanel.Controls.Add(this.tableLayoutPanel20);
            this.seqPanel.Controls.Add(this.label42);
            this.seqPanel.Controls.Add(this.seqSound3dInfoExists);
            this.seqPanel.Controls.Add(this.seqBank3Box);
            this.seqPanel.Controls.Add(this.seqBank2Box);
            this.seqPanel.Controls.Add(this.seqBank1Box);
            this.seqPanel.Controls.Add(this.seqBank0Box);
            this.seqPanel.Controls.Add(this.label43);
            this.seqPanel.Controls.Add(this.seqEditSound3dInfoButton);
            this.seqPanel.Controls.Add(this.seqEditSoundInfoButton);
            this.seqPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqPanel.Location = new System.Drawing.Point(0, 0);
            this.seqPanel.Name = "seqPanel";
            this.seqPanel.Size = new System.Drawing.Size(303, 472);
            this.seqPanel.TabIndex = 31;
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel23.ColumnCount = 1;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.Controls.Add(this.sarSeqPlay, 0, 1);
            this.tableLayoutPanel23.Controls.Add(this.label44, 0, 0);
            this.tableLayoutPanel23.Location = new System.Drawing.Point(5, 54);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 2;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(293, 59);
            this.tableLayoutPanel23.TabIndex = 27;
            // 
            // sarSeqPlay
            // 
            this.sarSeqPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sarSeqPlay.Location = new System.Drawing.Point(3, 32);
            this.sarSeqPlay.Name = "sarSeqPlay";
            this.sarSeqPlay.Size = new System.Drawing.Size(287, 24);
            this.sarSeqPlay.TabIndex = 10;
            this.sarSeqPlay.Text = "Play";
            this.sarSeqPlay.UseVisualStyleBackColor = true;
            this.sarSeqPlay.Click += new System.EventHandler(this.SarSeqPlay_Click);
            // 
            // label44
            // 
            this.label44.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label44.Location = new System.Drawing.Point(3, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(287, 29);
            this.label44.TabIndex = 11;
            this.label44.Text = "Sound Player Deluxe™";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel22
            // 
            this.tableLayoutPanel22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel22.ColumnCount = 2;
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.Controls.Add(this.sarSeqPause, 0, 0);
            this.tableLayoutPanel22.Controls.Add(this.sarSeqStop, 1, 0);
            this.tableLayoutPanel22.Location = new System.Drawing.Point(6, 116);
            this.tableLayoutPanel22.Name = "tableLayoutPanel22";
            this.tableLayoutPanel22.RowCount = 1;
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.Size = new System.Drawing.Size(291, 30);
            this.tableLayoutPanel22.TabIndex = 26;
            // 
            // sarSeqPause
            // 
            this.sarSeqPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarSeqPause.Location = new System.Drawing.Point(3, 3);
            this.sarSeqPause.Name = "sarSeqPause";
            this.sarSeqPause.Size = new System.Drawing.Size(139, 24);
            this.sarSeqPause.TabIndex = 0;
            this.sarSeqPause.Text = "Pause";
            this.sarSeqPause.UseVisualStyleBackColor = true;
            this.sarSeqPause.Click += new System.EventHandler(this.SarSeqPause_Click);
            // 
            // sarSeqStop
            // 
            this.sarSeqStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarSeqStop.Location = new System.Drawing.Point(148, 3);
            this.sarSeqStop.Name = "sarSeqStop";
            this.sarSeqStop.Size = new System.Drawing.Size(140, 24);
            this.sarSeqStop.TabIndex = 1;
            this.sarSeqStop.Text = "Stop";
            this.sarSeqStop.UseVisualStyleBackColor = true;
            this.sarSeqStop.Click += new System.EventHandler(this.SarSeqStop_Click);
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel21.ColumnCount = 3;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel21.Controls.Add(this.sarSeqPlayNext, 2, 0);
            this.tableLayoutPanel21.Controls.Add(this.sarSeqPlayLoop, 1, 0);
            this.tableLayoutPanel21.Controls.Add(this.sarSeqPlayOnce, 0, 0);
            this.tableLayoutPanel21.Location = new System.Drawing.Point(7, 149);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(291, 26);
            this.tableLayoutPanel21.TabIndex = 25;
            // 
            // sarSeqPlayNext
            // 
            this.sarSeqPlayNext.AutoSize = true;
            this.sarSeqPlayNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarSeqPlayNext.Location = new System.Drawing.Point(197, 3);
            this.sarSeqPlayNext.Name = "sarSeqPlayNext";
            this.sarSeqPlayNext.Size = new System.Drawing.Size(91, 20);
            this.sarSeqPlayNext.TabIndex = 2;
            this.sarSeqPlayNext.Text = "Play Next";
            this.sarSeqPlayNext.UseVisualStyleBackColor = true;
            // 
            // sarSeqPlayLoop
            // 
            this.sarSeqPlayLoop.AutoSize = true;
            this.sarSeqPlayLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarSeqPlayLoop.Location = new System.Drawing.Point(100, 3);
            this.sarSeqPlayLoop.Name = "sarSeqPlayLoop";
            this.sarSeqPlayLoop.Size = new System.Drawing.Size(91, 20);
            this.sarSeqPlayLoop.TabIndex = 1;
            this.sarSeqPlayLoop.Text = "Play Loop";
            this.sarSeqPlayLoop.UseVisualStyleBackColor = true;
            // 
            // sarSeqPlayOnce
            // 
            this.sarSeqPlayOnce.AutoSize = true;
            this.sarSeqPlayOnce.Checked = true;
            this.sarSeqPlayOnce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarSeqPlayOnce.Location = new System.Drawing.Point(3, 3);
            this.sarSeqPlayOnce.Name = "sarSeqPlayOnce";
            this.sarSeqPlayOnce.Size = new System.Drawing.Size(91, 20);
            this.sarSeqPlayOnce.TabIndex = 0;
            this.sarSeqPlayOnce.TabStop = true;
            this.sarSeqPlayOnce.Text = "Play Once";
            this.sarSeqPlayOnce.UseVisualStyleBackColor = true;
            // 
            // sarSeqFileIdBox
            // 
            this.sarSeqFileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sarSeqFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sarSeqFileIdBox.FormattingEnabled = true;
            this.sarSeqFileIdBox.Location = new System.Drawing.Point(7, 25);
            this.sarSeqFileIdBox.Name = "sarSeqFileIdBox";
            this.sarSeqFileIdBox.Size = new System.Drawing.Size(289, 21);
            this.sarSeqFileIdBox.TabIndex = 24;
            this.sarSeqFileIdBox.SelectedIndexChanged += new System.EventHandler(this.SarSeqFileIdBox_SelectedIndexChanged);
            // 
            // label47
            // 
            this.label47.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label47.Location = new System.Drawing.Point(8, 4);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(286, 17);
            this.label47.TabIndex = 23;
            this.label47.Text = "File Id:";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel19
            // 
            this.tableLayoutPanel19.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel19.ColumnCount = 4;
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.Controls.Add(this.seqC12, 0, 3);
            this.tableLayoutPanel19.Controls.Add(this.seqC0, 0, 0);
            this.tableLayoutPanel19.Controls.Add(this.seqC1, 1, 0);
            this.tableLayoutPanel19.Controls.Add(this.seqC2, 2, 0);
            this.tableLayoutPanel19.Controls.Add(this.seqC3, 3, 0);
            this.tableLayoutPanel19.Controls.Add(this.seqC4, 0, 1);
            this.tableLayoutPanel19.Controls.Add(this.seqC5, 1, 1);
            this.tableLayoutPanel19.Controls.Add(this.seqC6, 2, 1);
            this.tableLayoutPanel19.Controls.Add(this.seqC7, 3, 1);
            this.tableLayoutPanel19.Controls.Add(this.seqC8, 0, 2);
            this.tableLayoutPanel19.Controls.Add(this.seqC9, 1, 2);
            this.tableLayoutPanel19.Controls.Add(this.seqC10, 2, 2);
            this.tableLayoutPanel19.Controls.Add(this.seqC11, 3, 2);
            this.tableLayoutPanel19.Controls.Add(this.seqC15, 3, 3);
            this.tableLayoutPanel19.Controls.Add(this.seqC14, 2, 3);
            this.tableLayoutPanel19.Controls.Add(this.seqC13, 1, 3);
            this.tableLayoutPanel19.Location = new System.Drawing.Point(2, 546);
            this.tableLayoutPanel19.Name = "tableLayoutPanel19";
            this.tableLayoutPanel19.RowCount = 4;
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel19.Size = new System.Drawing.Size(297, 94);
            this.tableLayoutPanel19.TabIndex = 22;
            // 
            // seqC12
            // 
            this.seqC12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC12.Location = new System.Drawing.Point(3, 72);
            this.seqC12.Name = "seqC12";
            this.seqC12.Size = new System.Drawing.Size(68, 19);
            this.seqC12.TabIndex = 35;
            this.seqC12.Text = "12";
            this.seqC12.UseVisualStyleBackColor = true;
            this.seqC12.CheckedChanged += new System.EventHandler(this.SeqC12_CheckedChanged);
            // 
            // seqC0
            // 
            this.seqC0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC0.Location = new System.Drawing.Point(3, 3);
            this.seqC0.Name = "seqC0";
            this.seqC0.Size = new System.Drawing.Size(68, 17);
            this.seqC0.TabIndex = 20;
            this.seqC0.Text = "0";
            this.seqC0.UseVisualStyleBackColor = true;
            this.seqC0.CheckedChanged += new System.EventHandler(this.SeqC0_CheckedChanged);
            // 
            // seqC1
            // 
            this.seqC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC1.Location = new System.Drawing.Point(77, 3);
            this.seqC1.Name = "seqC1";
            this.seqC1.Size = new System.Drawing.Size(68, 17);
            this.seqC1.TabIndex = 21;
            this.seqC1.Text = "1";
            this.seqC1.UseVisualStyleBackColor = true;
            this.seqC1.CheckedChanged += new System.EventHandler(this.SeqC1_CheckedChanged);
            // 
            // seqC2
            // 
            this.seqC2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC2.Location = new System.Drawing.Point(151, 3);
            this.seqC2.Name = "seqC2";
            this.seqC2.Size = new System.Drawing.Size(68, 17);
            this.seqC2.TabIndex = 22;
            this.seqC2.Text = "2";
            this.seqC2.UseVisualStyleBackColor = true;
            this.seqC2.CheckedChanged += new System.EventHandler(this.SeqC2_CheckedChanged);
            // 
            // seqC3
            // 
            this.seqC3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC3.Location = new System.Drawing.Point(225, 3);
            this.seqC3.Name = "seqC3";
            this.seqC3.Size = new System.Drawing.Size(69, 17);
            this.seqC3.TabIndex = 23;
            this.seqC3.Text = "3";
            this.seqC3.UseVisualStyleBackColor = true;
            this.seqC3.CheckedChanged += new System.EventHandler(this.SeqC3_CheckedChanged);
            // 
            // seqC4
            // 
            this.seqC4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC4.Location = new System.Drawing.Point(3, 26);
            this.seqC4.Name = "seqC4";
            this.seqC4.Size = new System.Drawing.Size(68, 17);
            this.seqC4.TabIndex = 24;
            this.seqC4.Text = "4";
            this.seqC4.UseVisualStyleBackColor = true;
            this.seqC4.CheckedChanged += new System.EventHandler(this.SeqC4_CheckedChanged);
            // 
            // seqC5
            // 
            this.seqC5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC5.Location = new System.Drawing.Point(77, 26);
            this.seqC5.Name = "seqC5";
            this.seqC5.Size = new System.Drawing.Size(68, 17);
            this.seqC5.TabIndex = 25;
            this.seqC5.Text = "5";
            this.seqC5.UseVisualStyleBackColor = true;
            this.seqC5.CheckedChanged += new System.EventHandler(this.SeqC5_CheckedChanged);
            // 
            // seqC6
            // 
            this.seqC6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC6.Location = new System.Drawing.Point(151, 26);
            this.seqC6.Name = "seqC6";
            this.seqC6.Size = new System.Drawing.Size(68, 17);
            this.seqC6.TabIndex = 26;
            this.seqC6.Text = "6";
            this.seqC6.UseVisualStyleBackColor = true;
            this.seqC6.CheckedChanged += new System.EventHandler(this.SeqC6_CheckedChanged);
            // 
            // seqC7
            // 
            this.seqC7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC7.Location = new System.Drawing.Point(225, 26);
            this.seqC7.Name = "seqC7";
            this.seqC7.Size = new System.Drawing.Size(69, 17);
            this.seqC7.TabIndex = 27;
            this.seqC7.Text = "7";
            this.seqC7.UseVisualStyleBackColor = true;
            this.seqC7.CheckedChanged += new System.EventHandler(this.SeqC7_CheckedChanged);
            // 
            // seqC8
            // 
            this.seqC8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC8.Location = new System.Drawing.Point(3, 49);
            this.seqC8.Name = "seqC8";
            this.seqC8.Size = new System.Drawing.Size(68, 17);
            this.seqC8.TabIndex = 28;
            this.seqC8.Text = "8";
            this.seqC8.UseVisualStyleBackColor = true;
            this.seqC8.CheckedChanged += new System.EventHandler(this.SeqC8_CheckedChanged);
            // 
            // seqC9
            // 
            this.seqC9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC9.Location = new System.Drawing.Point(77, 49);
            this.seqC9.Name = "seqC9";
            this.seqC9.Size = new System.Drawing.Size(68, 17);
            this.seqC9.TabIndex = 29;
            this.seqC9.Text = "9";
            this.seqC9.UseVisualStyleBackColor = true;
            this.seqC9.CheckedChanged += new System.EventHandler(this.SeqC9_CheckedChanged);
            // 
            // seqC10
            // 
            this.seqC10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC10.Location = new System.Drawing.Point(151, 49);
            this.seqC10.Name = "seqC10";
            this.seqC10.Size = new System.Drawing.Size(68, 17);
            this.seqC10.TabIndex = 30;
            this.seqC10.Text = "10";
            this.seqC10.UseVisualStyleBackColor = true;
            this.seqC10.CheckedChanged += new System.EventHandler(this.SeqC10_CheckedChanged);
            // 
            // seqC11
            // 
            this.seqC11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC11.Location = new System.Drawing.Point(225, 49);
            this.seqC11.Name = "seqC11";
            this.seqC11.Size = new System.Drawing.Size(69, 17);
            this.seqC11.TabIndex = 31;
            this.seqC11.Text = "11";
            this.seqC11.UseVisualStyleBackColor = true;
            this.seqC11.CheckedChanged += new System.EventHandler(this.SeqC11_CheckedChanged);
            // 
            // seqC15
            // 
            this.seqC15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC15.Location = new System.Drawing.Point(225, 72);
            this.seqC15.Name = "seqC15";
            this.seqC15.Size = new System.Drawing.Size(69, 19);
            this.seqC15.TabIndex = 32;
            this.seqC15.Text = "15";
            this.seqC15.UseVisualStyleBackColor = true;
            this.seqC15.CheckedChanged += new System.EventHandler(this.SeqC15_CheckedChanged);
            // 
            // seqC14
            // 
            this.seqC14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC14.Location = new System.Drawing.Point(151, 72);
            this.seqC14.Name = "seqC14";
            this.seqC14.Size = new System.Drawing.Size(68, 19);
            this.seqC14.TabIndex = 33;
            this.seqC14.Text = "14";
            this.seqC14.UseVisualStyleBackColor = true;
            this.seqC14.CheckedChanged += new System.EventHandler(this.SeqC14_CheckedChanged);
            // 
            // seqC13
            // 
            this.seqC13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqC13.Location = new System.Drawing.Point(77, 72);
            this.seqC13.Name = "seqC13";
            this.seqC13.Size = new System.Drawing.Size(68, 19);
            this.seqC13.TabIndex = 34;
            this.seqC13.Text = "13";
            this.seqC13.UseVisualStyleBackColor = true;
            this.seqC13.CheckedChanged += new System.EventHandler(this.SeqC13_CheckedChanged);
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label39.Location = new System.Drawing.Point(5, 523);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(293, 17);
            this.label39.TabIndex = 20;
            this.label39.Text = "Channel Flags:";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // seqIsReleasePriorityBox
            // 
            this.seqIsReleasePriorityBox.AutoSize = true;
            this.seqIsReleasePriorityBox.Location = new System.Drawing.Point(7, 503);
            this.seqIsReleasePriorityBox.Name = "seqIsReleasePriorityBox";
            this.seqIsReleasePriorityBox.Size = new System.Drawing.Size(132, 17);
            this.seqIsReleasePriorityBox.TabIndex = 19;
            this.seqIsReleasePriorityBox.Text = "Fix Priority On Release";
            this.seqIsReleasePriorityBox.UseVisualStyleBackColor = true;
            this.seqIsReleasePriorityBox.CheckedChanged += new System.EventHandler(this.SeqIsReleasePriorityBox_CheckedChanged);
            // 
            // seqChannelPriorityBox
            // 
            this.seqChannelPriorityBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqChannelPriorityBox.Location = new System.Drawing.Point(5, 478);
            this.seqChannelPriorityBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.seqChannelPriorityBox.Name = "seqChannelPriorityBox";
            this.seqChannelPriorityBox.Size = new System.Drawing.Size(293, 20);
            this.seqChannelPriorityBox.TabIndex = 17;
            this.seqChannelPriorityBox.ValueChanged += new System.EventHandler(this.SeqChannelPriorityBox_ValueChanged);
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label41.Location = new System.Drawing.Point(1, 458);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(289, 17);
            this.label41.TabIndex = 16;
            this.label41.Text = "Channel Priority:";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel20.ColumnCount = 2;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.Controls.Add(this.seqOffsetFromLabelBox, 0, 1);
            this.tableLayoutPanel20.Controls.Add(this.seqOffsetManualButton, 1, 0);
            this.tableLayoutPanel20.Controls.Add(this.seqOffsetFromLabelButton, 0, 0);
            this.tableLayoutPanel20.Controls.Add(this.seqOffsetManualBox, 1, 1);
            this.tableLayoutPanel20.Location = new System.Drawing.Point(1, 399);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 2;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(297, 56);
            this.tableLayoutPanel20.TabIndex = 15;
            // 
            // seqOffsetFromLabelBox
            // 
            this.seqOffsetFromLabelBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqOffsetFromLabelBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seqOffsetFromLabelBox.FormattingEnabled = true;
            this.seqOffsetFromLabelBox.Location = new System.Drawing.Point(3, 31);
            this.seqOffsetFromLabelBox.Name = "seqOffsetFromLabelBox";
            this.seqOffsetFromLabelBox.Size = new System.Drawing.Size(142, 21);
            this.seqOffsetFromLabelBox.TabIndex = 6;
            this.seqOffsetFromLabelBox.SelectedIndexChanged += new System.EventHandler(this.SeqOffsetFromLabelBox_SelectedIndexChanged);
            // 
            // seqOffsetManualButton
            // 
            this.seqOffsetManualButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqOffsetManualButton.Location = new System.Drawing.Point(151, 3);
            this.seqOffsetManualButton.Name = "seqOffsetManualButton";
            this.seqOffsetManualButton.Size = new System.Drawing.Size(143, 22);
            this.seqOffsetManualButton.TabIndex = 1;
            this.seqOffsetManualButton.TabStop = true;
            this.seqOffsetManualButton.Text = "Manual";
            this.seqOffsetManualButton.UseVisualStyleBackColor = true;
            this.seqOffsetManualButton.CheckedChanged += new System.EventHandler(this.SeqOffsetManualButton_CheckedChanged);
            // 
            // seqOffsetFromLabelButton
            // 
            this.seqOffsetFromLabelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqOffsetFromLabelButton.Location = new System.Drawing.Point(3, 3);
            this.seqOffsetFromLabelButton.Name = "seqOffsetFromLabelButton";
            this.seqOffsetFromLabelButton.Size = new System.Drawing.Size(142, 22);
            this.seqOffsetFromLabelButton.TabIndex = 0;
            this.seqOffsetFromLabelButton.TabStop = true;
            this.seqOffsetFromLabelButton.Text = "From Label";
            this.seqOffsetFromLabelButton.UseVisualStyleBackColor = true;
            this.seqOffsetFromLabelButton.CheckedChanged += new System.EventHandler(this.SeqOffsetFromLabelButton_CheckedChanged);
            // 
            // seqOffsetManualBox
            // 
            this.seqOffsetManualBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seqOffsetManualBox.Location = new System.Drawing.Point(151, 31);
            this.seqOffsetManualBox.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.seqOffsetManualBox.Name = "seqOffsetManualBox";
            this.seqOffsetManualBox.Size = new System.Drawing.Size(143, 20);
            this.seqOffsetManualBox.TabIndex = 7;
            this.seqOffsetManualBox.ValueChanged += new System.EventHandler(this.SeqOffsetManualBox_ValueChanged);
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label42.Location = new System.Drawing.Point(2, 379);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(289, 17);
            this.label42.TabIndex = 13;
            this.label42.Text = "Start Offset:";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // seqSound3dInfoExists
            // 
            this.seqSound3dInfoExists.AutoSize = true;
            this.seqSound3dInfoExists.Location = new System.Drawing.Point(9, 210);
            this.seqSound3dInfoExists.Name = "seqSound3dInfoExists";
            this.seqSound3dInfoExists.Size = new System.Drawing.Size(129, 17);
            this.seqSound3dInfoExists.TabIndex = 12;
            this.seqSound3dInfoExists.Text = "Enable Sound 3d Info";
            this.seqSound3dInfoExists.UseVisualStyleBackColor = true;
            this.seqSound3dInfoExists.CheckedChanged += new System.EventHandler(this.SeqSound3dInfoExists_CheckedChanged);
            // 
            // seqBank3Box
            // 
            this.seqBank3Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqBank3Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seqBank3Box.FormattingEnabled = true;
            this.seqBank3Box.Location = new System.Drawing.Point(5, 355);
            this.seqBank3Box.Name = "seqBank3Box";
            this.seqBank3Box.Size = new System.Drawing.Size(292, 21);
            this.seqBank3Box.TabIndex = 11;
            this.seqBank3Box.SelectedIndexChanged += new System.EventHandler(this.SeqBank3Box_SelectedIndexChanged);
            // 
            // seqBank2Box
            // 
            this.seqBank2Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqBank2Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seqBank2Box.FormattingEnabled = true;
            this.seqBank2Box.Location = new System.Drawing.Point(5, 328);
            this.seqBank2Box.Name = "seqBank2Box";
            this.seqBank2Box.Size = new System.Drawing.Size(292, 21);
            this.seqBank2Box.TabIndex = 9;
            this.seqBank2Box.SelectedIndexChanged += new System.EventHandler(this.SeqBank2Box_SelectedIndexChanged);
            // 
            // seqBank1Box
            // 
            this.seqBank1Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqBank1Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seqBank1Box.FormattingEnabled = true;
            this.seqBank1Box.Location = new System.Drawing.Point(5, 301);
            this.seqBank1Box.Name = "seqBank1Box";
            this.seqBank1Box.Size = new System.Drawing.Size(292, 21);
            this.seqBank1Box.TabIndex = 7;
            this.seqBank1Box.SelectedIndexChanged += new System.EventHandler(this.SeqBank1Box_SelectedIndexChanged);
            // 
            // seqBank0Box
            // 
            this.seqBank0Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqBank0Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seqBank0Box.FormattingEnabled = true;
            this.seqBank0Box.Location = new System.Drawing.Point(5, 274);
            this.seqBank0Box.Name = "seqBank0Box";
            this.seqBank0Box.Size = new System.Drawing.Size(292, 21);
            this.seqBank0Box.TabIndex = 5;
            this.seqBank0Box.SelectedIndexChanged += new System.EventHandler(this.SeqBank0Box_SelectedIndexChanged);
            // 
            // label43
            // 
            this.label43.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label43.Location = new System.Drawing.Point(4, 254);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(291, 17);
            this.label43.TabIndex = 4;
            this.label43.Text = "Banks:";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // seqEditSound3dInfoButton
            // 
            this.seqEditSound3dInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqEditSound3dInfoButton.Location = new System.Drawing.Point(5, 228);
            this.seqEditSound3dInfoButton.Name = "seqEditSound3dInfoButton";
            this.seqEditSound3dInfoButton.Size = new System.Drawing.Size(292, 23);
            this.seqEditSound3dInfoButton.TabIndex = 3;
            this.seqEditSound3dInfoButton.Text = "Edit Sound 3d Info";
            this.seqEditSound3dInfoButton.UseVisualStyleBackColor = true;
            this.seqEditSound3dInfoButton.Click += new System.EventHandler(this.SeqEditSound3dInfoButton_Click);
            // 
            // seqEditSoundInfoButton
            // 
            this.seqEditSoundInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seqEditSoundInfoButton.Location = new System.Drawing.Point(5, 181);
            this.seqEditSoundInfoButton.Name = "seqEditSoundInfoButton";
            this.seqEditSoundInfoButton.Size = new System.Drawing.Size(292, 23);
            this.seqEditSoundInfoButton.TabIndex = 1;
            this.seqEditSoundInfoButton.Text = "Edit Sound Info";
            this.seqEditSoundInfoButton.UseVisualStyleBackColor = true;
            this.seqEditSoundInfoButton.Click += new System.EventHandler(this.SeqEditSoundInfoButton_Click);
            // 
            // soundGrpPanel
            // 
            this.soundGrpPanel.Controls.Add(this.soundGrpGridTable);
            this.soundGrpPanel.Controls.Add(this.soundGrpEndIndex);
            this.soundGrpPanel.Controls.Add(this.label36);
            this.soundGrpPanel.Controls.Add(this.soundGrpStartIndex);
            this.soundGrpPanel.Controls.Add(this.label37);
            this.soundGrpPanel.Controls.Add(this.soundGrpSoundType);
            this.soundGrpPanel.Controls.Add(this.label38);
            this.soundGrpPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundGrpPanel.Location = new System.Drawing.Point(0, 0);
            this.soundGrpPanel.Name = "soundGrpPanel";
            this.soundGrpPanel.Size = new System.Drawing.Size(303, 472);
            this.soundGrpPanel.TabIndex = 30;
            // 
            // soundGrpGridTable
            // 
            this.soundGrpGridTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundGrpGridTable.ColumnCount = 1;
            this.soundGrpGridTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundGrpGridTable.Controls.Add(this.soundGrpWaveArchives, 0, 1);
            this.soundGrpGridTable.Controls.Add(this.soundGrpFilesGrid, 0, 0);
            this.soundGrpGridTable.Location = new System.Drawing.Point(5, 140);
            this.soundGrpGridTable.Name = "soundGrpGridTable";
            this.soundGrpGridTable.RowCount = 2;
            this.soundGrpGridTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundGrpGridTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.soundGrpGridTable.Size = new System.Drawing.Size(292, 329);
            this.soundGrpGridTable.TabIndex = 6;
            // 
            // soundGrpWaveArchives
            // 
            this.soundGrpWaveArchives.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.soundGrpWaveArchives.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewComboBoxColumn1});
            this.soundGrpWaveArchives.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundGrpWaveArchives.Location = new System.Drawing.Point(3, 167);
            this.soundGrpWaveArchives.Name = "soundGrpWaveArchives";
            this.soundGrpWaveArchives.Size = new System.Drawing.Size(286, 159);
            this.soundGrpWaveArchives.TabIndex = 7;
            this.soundGrpWaveArchives.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SoundGroupWarsChanged);
            this.soundGrpWaveArchives.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.SoundGroupWarsChanged);
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn1.HeaderText = "Wave Archives";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            // 
            // soundGrpFilesGrid
            // 
            this.soundGrpFilesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.soundGrpFilesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Files});
            this.soundGrpFilesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundGrpFilesGrid.Location = new System.Drawing.Point(3, 3);
            this.soundGrpFilesGrid.Name = "soundGrpFilesGrid";
            this.soundGrpFilesGrid.Size = new System.Drawing.Size(286, 158);
            this.soundGrpFilesGrid.TabIndex = 6;
            this.soundGrpFilesGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SoundGroupFilesChanged);
            this.soundGrpFilesGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.SoundGroupFilesChanged);
            // 
            // Files
            // 
            this.Files.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Files.HeaderText = "Files";
            this.Files.Name = "Files";
            // 
            // soundGrpEndIndex
            // 
            this.soundGrpEndIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundGrpEndIndex.Location = new System.Drawing.Point(5, 114);
            this.soundGrpEndIndex.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.soundGrpEndIndex.Name = "soundGrpEndIndex";
            this.soundGrpEndIndex.Size = new System.Drawing.Size(293, 20);
            this.soundGrpEndIndex.TabIndex = 5;
            this.soundGrpEndIndex.ValueChanged += new System.EventHandler(this.SoundGrpEndIndex_ValueChanged);
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.Location = new System.Drawing.Point(5, 91);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(297, 20);
            this.label36.TabIndex = 4;
            this.label36.Text = "End Index:";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soundGrpStartIndex
            // 
            this.soundGrpStartIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundGrpStartIndex.Location = new System.Drawing.Point(5, 69);
            this.soundGrpStartIndex.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.soundGrpStartIndex.Name = "soundGrpStartIndex";
            this.soundGrpStartIndex.Size = new System.Drawing.Size(292, 20);
            this.soundGrpStartIndex.TabIndex = 3;
            this.soundGrpStartIndex.ValueChanged += new System.EventHandler(this.SoundGrpStartIndex_ValueChanged);
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.Location = new System.Drawing.Point(2, 50);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(297, 20);
            this.label37.TabIndex = 2;
            this.label37.Text = "Start Index:";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soundGrpSoundType
            // 
            this.soundGrpSoundType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundGrpSoundType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.soundGrpSoundType.Enabled = false;
            this.soundGrpSoundType.FormattingEnabled = true;
            this.soundGrpSoundType.Items.AddRange(new object[] {
            "Null",
            "Sequence Set",
            "Wave Sound Set"});
            this.soundGrpSoundType.Location = new System.Drawing.Point(5, 26);
            this.soundGrpSoundType.Name = "soundGrpSoundType";
            this.soundGrpSoundType.Size = new System.Drawing.Size(291, 21);
            this.soundGrpSoundType.TabIndex = 1;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.Location = new System.Drawing.Point(3, 3);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(297, 20);
            this.label38.TabIndex = 0;
            this.label38.Text = "Sound Type:";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bankPanel
            // 
            this.bankPanel.Controls.Add(this.tableLayoutPanel18);
            this.bankPanel.Controls.Add(this.label35);
            this.bankPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankPanel.Location = new System.Drawing.Point(0, 0);
            this.bankPanel.Name = "bankPanel";
            this.bankPanel.Size = new System.Drawing.Size(303, 472);
            this.bankPanel.TabIndex = 29;
            this.bankPanel.Visible = false;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 1;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Controls.Add(this.label34, 0, 0);
            this.tableLayoutPanel18.Controls.Add(this.sarBnkFileIdBox, 0, 1);
            this.tableLayoutPanel18.Controls.Add(this.bankWarDataGrid, 0, 2);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 3;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(303, 472);
            this.tableLayoutPanel18.TabIndex = 10;
            // 
            // label34
            // 
            this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label34.Location = new System.Drawing.Point(3, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(297, 20);
            this.label34.TabIndex = 6;
            this.label34.Text = "File Id:";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sarBnkFileIdBox
            // 
            this.sarBnkFileIdBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarBnkFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sarBnkFileIdBox.FormattingEnabled = true;
            this.sarBnkFileIdBox.Location = new System.Drawing.Point(3, 23);
            this.sarBnkFileIdBox.Name = "sarBnkFileIdBox";
            this.sarBnkFileIdBox.Size = new System.Drawing.Size(297, 21);
            this.sarBnkFileIdBox.TabIndex = 7;
            this.sarBnkFileIdBox.SelectedIndexChanged += new System.EventHandler(this.SarBnkFileIdBox_SelectedIndexChanged);
            // 
            // bankWarDataGrid
            // 
            this.bankWarDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bankWarDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.waveArchives});
            this.bankWarDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankWarDataGrid.Location = new System.Drawing.Point(3, 53);
            this.bankWarDataGrid.Name = "bankWarDataGrid";
            this.bankWarDataGrid.Size = new System.Drawing.Size(297, 416);
            this.bankWarDataGrid.TabIndex = 0;
            this.bankWarDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.BnkWarsChanged);
            this.bankWarDataGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.BnkWarsChanged);
            // 
            // waveArchives
            // 
            this.waveArchives.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.waveArchives.HeaderText = "Wave Archives:";
            this.waveArchives.Name = "waveArchives";
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label35.Location = new System.Drawing.Point(8, 46);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(286, 17);
            this.label35.TabIndex = 8;
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // warInfoPanel
            // 
            this.warInfoPanel.Controls.Add(this.label33);
            this.warInfoPanel.Controls.Add(this.sarWarFileIdBox);
            this.warInfoPanel.Controls.Add(this.tableLayoutPanel17);
            this.warInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.warInfoPanel.Name = "warInfoPanel";
            this.warInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.warInfoPanel.TabIndex = 28;
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label33.Location = new System.Drawing.Point(9, 4);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(286, 17);
            this.label33.TabIndex = 5;
            this.label33.Text = "File Id:";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sarWarFileIdBox
            // 
            this.sarWarFileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sarWarFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sarWarFileIdBox.FormattingEnabled = true;
            this.sarWarFileIdBox.Location = new System.Drawing.Point(9, 25);
            this.sarWarFileIdBox.Name = "sarWarFileIdBox";
            this.sarWarFileIdBox.Size = new System.Drawing.Size(286, 21);
            this.sarWarFileIdBox.TabIndex = 4;
            this.sarWarFileIdBox.SelectedIndexChanged += new System.EventHandler(this.SarWarFileIdBox_SelectedIndexChanged);
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel17.ColumnCount = 2;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel17.Controls.Add(this.warIncludeWaveCountBox, 1, 1);
            this.tableLayoutPanel17.Controls.Add(this.label31, 1, 0);
            this.tableLayoutPanel17.Controls.Add(this.label32, 0, 0);
            this.tableLayoutPanel17.Controls.Add(this.warLoadIndividuallyBox, 0, 1);
            this.tableLayoutPanel17.Location = new System.Drawing.Point(10, 53);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 2;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(285, 46);
            this.tableLayoutPanel17.TabIndex = 0;
            // 
            // warIncludeWaveCountBox
            // 
            this.warIncludeWaveCountBox.AutoSize = true;
            this.warIncludeWaveCountBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.warIncludeWaveCountBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warIncludeWaveCountBox.Location = new System.Drawing.Point(145, 26);
            this.warIncludeWaveCountBox.Name = "warIncludeWaveCountBox";
            this.warIncludeWaveCountBox.Size = new System.Drawing.Size(137, 17);
            this.warIncludeWaveCountBox.TabIndex = 3;
            this.warIncludeWaveCountBox.UseVisualStyleBackColor = true;
            this.warIncludeWaveCountBox.CheckedChanged += new System.EventHandler(this.WarIncludeWaveCountBox_CheckedChanged);
            // 
            // label31
            // 
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Location = new System.Drawing.Point(145, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(137, 23);
            this.label31.TabIndex = 1;
            this.label31.Text = "Include Wave Count:";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label32
            // 
            this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label32.Location = new System.Drawing.Point(3, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(136, 23);
            this.label32.TabIndex = 0;
            this.label32.Text = "Load Individually:";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // warLoadIndividuallyBox
            // 
            this.warLoadIndividuallyBox.AutoSize = true;
            this.warLoadIndividuallyBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.warLoadIndividuallyBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warLoadIndividuallyBox.Location = new System.Drawing.Point(3, 26);
            this.warLoadIndividuallyBox.Name = "warLoadIndividuallyBox";
            this.warLoadIndividuallyBox.Size = new System.Drawing.Size(136, 17);
            this.warLoadIndividuallyBox.TabIndex = 2;
            this.warLoadIndividuallyBox.UseVisualStyleBackColor = true;
            this.warLoadIndividuallyBox.CheckedChanged += new System.EventHandler(this.WarLoadIndividuallyBox_CheckedChanged);
            // 
            // grpPanel
            // 
            this.grpPanel.Controls.Add(this.sarGrpFileIdBox);
            this.grpPanel.Controls.Add(this.fileIdLabel);
            this.grpPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPanel.Location = new System.Drawing.Point(0, 0);
            this.grpPanel.Name = "grpPanel";
            this.grpPanel.Size = new System.Drawing.Size(303, 472);
            this.grpPanel.TabIndex = 27;
            // 
            // sarGrpFileIdBox
            // 
            this.sarGrpFileIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sarGrpFileIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sarGrpFileIdBox.FormattingEnabled = true;
            this.sarGrpFileIdBox.Location = new System.Drawing.Point(8, 24);
            this.sarGrpFileIdBox.Name = "sarGrpFileIdBox";
            this.sarGrpFileIdBox.Size = new System.Drawing.Size(286, 21);
            this.sarGrpFileIdBox.TabIndex = 3;
            this.sarGrpFileIdBox.SelectedIndexChanged += new System.EventHandler(this.SarGrpFileIdBox_SelectedIndexChanged);
            // 
            // fileIdLabel
            // 
            this.fileIdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileIdLabel.Location = new System.Drawing.Point(8, 4);
            this.fileIdLabel.Name = "fileIdLabel";
            this.fileIdLabel.Size = new System.Drawing.Size(286, 17);
            this.fileIdLabel.TabIndex = 2;
            this.fileIdLabel.Text = "File Id:";
            this.fileIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerInfoPanel
            // 
            this.playerInfoPanel.Controls.Add(this.playerEnableSoundLimitBox);
            this.playerInfoPanel.Controls.Add(this.playerHeapSizeBox);
            this.playerInfoPanel.Controls.Add(this.label27);
            this.playerInfoPanel.Controls.Add(this.label28);
            this.playerInfoPanel.Controls.Add(this.playerSoundLimitBox);
            this.playerInfoPanel.Controls.Add(this.label29);
            this.playerInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.playerInfoPanel.Name = "playerInfoPanel";
            this.playerInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.playerInfoPanel.TabIndex = 25;
            // 
            // playerEnableSoundLimitBox
            // 
            this.playerEnableSoundLimitBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playerEnableSoundLimitBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.playerEnableSoundLimitBox.Location = new System.Drawing.Point(4, 67);
            this.playerEnableSoundLimitBox.Name = "playerEnableSoundLimitBox";
            this.playerEnableSoundLimitBox.Size = new System.Drawing.Size(293, 24);
            this.playerEnableSoundLimitBox.TabIndex = 7;
            this.playerEnableSoundLimitBox.UseVisualStyleBackColor = true;
            this.playerEnableSoundLimitBox.CheckedChanged += new System.EventHandler(this.PlayerEnableSoundLimitBox_CheckedChanged);
            // 
            // playerHeapSizeBox
            // 
            this.playerHeapSizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playerHeapSizeBox.Location = new System.Drawing.Point(5, 119);
            this.playerHeapSizeBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.playerHeapSizeBox.Name = "playerHeapSizeBox";
            this.playerHeapSizeBox.Size = new System.Drawing.Size(292, 20);
            this.playerHeapSizeBox.TabIndex = 6;
            this.playerHeapSizeBox.ValueChanged += new System.EventHandler(this.PlayerHeapSizeBox_ValueChanged);
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.Location = new System.Drawing.Point(5, 95);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(290, 17);
            this.label27.TabIndex = 5;
            this.label27.Text = "Player Heap Size:";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label28.Location = new System.Drawing.Point(3, 50);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(295, 17);
            this.label28.TabIndex = 3;
            this.label28.Text = "Enable Heap Size:";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerSoundLimitBox
            // 
            this.playerSoundLimitBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playerSoundLimitBox.Location = new System.Drawing.Point(4, 26);
            this.playerSoundLimitBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.playerSoundLimitBox.Name = "playerSoundLimitBox";
            this.playerSoundLimitBox.Size = new System.Drawing.Size(293, 20);
            this.playerSoundLimitBox.TabIndex = 2;
            this.playerSoundLimitBox.ValueChanged += new System.EventHandler(this.PlayerSoundLimitBox_ValueChanged);
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.Location = new System.Drawing.Point(3, 3);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(294, 17);
            this.label29.TabIndex = 1;
            this.label29.Text = "Sound Limit:";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sarProjectInfoPanel
            // 
            this.sarProjectInfoPanel.Controls.Add(this.sarIncludeStringBlock);
            this.sarProjectInfoPanel.Controls.Add(this.optionsPIBox);
            this.sarProjectInfoPanel.Controls.Add(this.optionsPILabel);
            this.sarProjectInfoPanel.Controls.Add(this.streamBufferTimesBox);
            this.sarProjectInfoPanel.Controls.Add(this.streamBufferTimesLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxWaveNumTracksBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxWaveNumTracksLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxWaveNumBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxWaveNumLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxStreamNumChannelsBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxStreamNumChannelsLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxStreamNumTracksBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxStreamNumTracksLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxStreamNumBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxStreamNumLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxSeqTrackNumBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxSeqTrackNumLabel);
            this.sarProjectInfoPanel.Controls.Add(this.maxSeqNumBox);
            this.sarProjectInfoPanel.Controls.Add(this.maxSeqNumLabel);
            this.sarProjectInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sarProjectInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.sarProjectInfoPanel.Name = "sarProjectInfoPanel";
            this.sarProjectInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.sarProjectInfoPanel.TabIndex = 21;
            this.sarProjectInfoPanel.Visible = false;
            // 
            // sarIncludeStringBlock
            // 
            this.sarIncludeStringBlock.AutoSize = true;
            this.sarIncludeStringBlock.Location = new System.Drawing.Point(7, 435);
            this.sarIncludeStringBlock.Name = "sarIncludeStringBlock";
            this.sarIncludeStringBlock.Size = new System.Drawing.Size(121, 17);
            this.sarIncludeStringBlock.TabIndex = 18;
            this.sarIncludeStringBlock.Text = "Include String Block";
            this.sarIncludeStringBlock.UseVisualStyleBackColor = true;
            this.sarIncludeStringBlock.CheckedChanged += new System.EventHandler(this.SarIncludeStringBlock_CheckedChanged);
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
            this.optionsPIBox.Size = new System.Drawing.Size(290, 20);
            this.optionsPIBox.TabIndex = 17;
            this.optionsPIBox.ValueChanged += new System.EventHandler(this.OptionsPIBox_ValueChanged);
            // 
            // optionsPILabel
            // 
            this.optionsPILabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPILabel.Location = new System.Drawing.Point(11, 384);
            this.optionsPILabel.Name = "optionsPILabel";
            this.optionsPILabel.Size = new System.Drawing.Size(286, 22);
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
            this.streamBufferTimesBox.Size = new System.Drawing.Size(290, 20);
            this.streamBufferTimesBox.TabIndex = 15;
            this.streamBufferTimesBox.ValueChanged += new System.EventHandler(this.StreamBufferTimesBox_ValueChanged);
            // 
            // streamBufferTimesLabel
            // 
            this.streamBufferTimesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamBufferTimesLabel.Location = new System.Drawing.Point(11, 336);
            this.streamBufferTimesLabel.Name = "streamBufferTimesLabel";
            this.streamBufferTimesLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxWaveNumTracksBox.Size = new System.Drawing.Size(290, 20);
            this.maxWaveNumTracksBox.TabIndex = 13;
            this.maxWaveNumTracksBox.ValueChanged += new System.EventHandler(this.MaxWaveNumTracksBox_ValueChanged);
            // 
            // maxWaveNumTracksLabel
            // 
            this.maxWaveNumTracksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxWaveNumTracksLabel.Location = new System.Drawing.Point(11, 288);
            this.maxWaveNumTracksLabel.Name = "maxWaveNumTracksLabel";
            this.maxWaveNumTracksLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxWaveNumBox.Size = new System.Drawing.Size(290, 20);
            this.maxWaveNumBox.TabIndex = 11;
            this.maxWaveNumBox.ValueChanged += new System.EventHandler(this.MaxWaveNumBox_ValueChanged);
            // 
            // maxWaveNumLabel
            // 
            this.maxWaveNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxWaveNumLabel.Location = new System.Drawing.Point(11, 240);
            this.maxWaveNumLabel.Name = "maxWaveNumLabel";
            this.maxWaveNumLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxStreamNumChannelsBox.Size = new System.Drawing.Size(290, 20);
            this.maxStreamNumChannelsBox.TabIndex = 9;
            this.maxStreamNumChannelsBox.ValueChanged += new System.EventHandler(this.MaxStreamNumChannelsBox_ValueChanged);
            // 
            // maxStreamNumChannelsLabel
            // 
            this.maxStreamNumChannelsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumChannelsLabel.Location = new System.Drawing.Point(11, 192);
            this.maxStreamNumChannelsLabel.Name = "maxStreamNumChannelsLabel";
            this.maxStreamNumChannelsLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxStreamNumTracksBox.Size = new System.Drawing.Size(290, 20);
            this.maxStreamNumTracksBox.TabIndex = 7;
            this.maxStreamNumTracksBox.ValueChanged += new System.EventHandler(this.MaxStreamNumTracksBox_ValueChanged);
            // 
            // maxStreamNumTracksLabel
            // 
            this.maxStreamNumTracksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumTracksLabel.Location = new System.Drawing.Point(11, 144);
            this.maxStreamNumTracksLabel.Name = "maxStreamNumTracksLabel";
            this.maxStreamNumTracksLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxStreamNumBox.Size = new System.Drawing.Size(290, 20);
            this.maxStreamNumBox.TabIndex = 5;
            this.maxStreamNumBox.ValueChanged += new System.EventHandler(this.MaxStreamNumBox_ValueChanged);
            // 
            // maxStreamNumLabel
            // 
            this.maxStreamNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxStreamNumLabel.Location = new System.Drawing.Point(11, 96);
            this.maxStreamNumLabel.Name = "maxStreamNumLabel";
            this.maxStreamNumLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxSeqTrackNumBox.Size = new System.Drawing.Size(290, 20);
            this.maxSeqTrackNumBox.TabIndex = 3;
            this.maxSeqTrackNumBox.ValueChanged += new System.EventHandler(this.MaxSeqTrackNumBox_ValueChanged);
            // 
            // maxSeqTrackNumLabel
            // 
            this.maxSeqTrackNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxSeqTrackNumLabel.Location = new System.Drawing.Point(11, 48);
            this.maxSeqTrackNumLabel.Name = "maxSeqTrackNumLabel";
            this.maxSeqTrackNumLabel.Size = new System.Drawing.Size(286, 22);
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
            this.maxSeqNumBox.Size = new System.Drawing.Size(290, 20);
            this.maxSeqNumBox.TabIndex = 1;
            this.maxSeqNumBox.ValueChanged += new System.EventHandler(this.MaxSeqNumBox_ValueChanged);
            // 
            // maxSeqNumLabel
            // 
            this.maxSeqNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxSeqNumLabel.Location = new System.Drawing.Point(11, 0);
            this.maxSeqNumLabel.Name = "maxSeqNumLabel";
            this.maxSeqNumLabel.Size = new System.Drawing.Size(286, 22);
            this.maxSeqNumLabel.TabIndex = 0;
            this.maxSeqNumLabel.Text = "Max Number Of Sequences:";
            this.maxSeqNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soundPlayerDeluxePanel
            // 
            this.soundPlayerDeluxePanel.Controls.Add(this.soundDeluxeTrack3);
            this.soundPlayerDeluxePanel.Controls.Add(this.soundDeluxeTrack2);
            this.soundPlayerDeluxePanel.Controls.Add(this.soundDeluxeTrack1);
            this.soundPlayerDeluxePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundPlayerDeluxePanel.Location = new System.Drawing.Point(0, 0);
            this.soundPlayerDeluxePanel.Name = "soundPlayerDeluxePanel";
            this.soundPlayerDeluxePanel.Size = new System.Drawing.Size(303, 472);
            this.soundPlayerDeluxePanel.TabIndex = 4;
            this.soundPlayerDeluxePanel.Visible = false;
            // 
            // soundDeluxeTrack3
            // 
            this.soundDeluxeTrack3.ColumnCount = 3;
            this.soundDeluxeTrack3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.soundDeluxeTrack3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.soundDeluxeTrack3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.soundDeluxeTrack3.Controls.Add(this.soundPlayerDeluxePlayNextBox, 2, 0);
            this.soundDeluxeTrack3.Controls.Add(this.soundPlayerDeluxePlayLoopBox, 1, 0);
            this.soundDeluxeTrack3.Controls.Add(this.soundPlayerDeluxePlayOnceBox, 0, 0);
            this.soundDeluxeTrack3.Dock = System.Windows.Forms.DockStyle.Top;
            this.soundDeluxeTrack3.Location = new System.Drawing.Point(0, 89);
            this.soundDeluxeTrack3.Name = "soundDeluxeTrack3";
            this.soundDeluxeTrack3.RowCount = 1;
            this.soundDeluxeTrack3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.soundDeluxeTrack3.Size = new System.Drawing.Size(303, 30);
            this.soundDeluxeTrack3.TabIndex = 12;
            // 
            // soundPlayerDeluxePlayNextBox
            // 
            this.soundPlayerDeluxePlayNextBox.AutoSize = true;
            this.soundPlayerDeluxePlayNextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundPlayerDeluxePlayNextBox.Location = new System.Drawing.Point(205, 3);
            this.soundPlayerDeluxePlayNextBox.Name = "soundPlayerDeluxePlayNextBox";
            this.soundPlayerDeluxePlayNextBox.Size = new System.Drawing.Size(95, 24);
            this.soundPlayerDeluxePlayNextBox.TabIndex = 2;
            this.soundPlayerDeluxePlayNextBox.Text = "Play Next";
            this.soundPlayerDeluxePlayNextBox.UseVisualStyleBackColor = true;
            // 
            // soundPlayerDeluxePlayLoopBox
            // 
            this.soundPlayerDeluxePlayLoopBox.AutoSize = true;
            this.soundPlayerDeluxePlayLoopBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundPlayerDeluxePlayLoopBox.Location = new System.Drawing.Point(104, 3);
            this.soundPlayerDeluxePlayLoopBox.Name = "soundPlayerDeluxePlayLoopBox";
            this.soundPlayerDeluxePlayLoopBox.Size = new System.Drawing.Size(95, 24);
            this.soundPlayerDeluxePlayLoopBox.TabIndex = 1;
            this.soundPlayerDeluxePlayLoopBox.Text = "Play Loop";
            this.soundPlayerDeluxePlayLoopBox.UseVisualStyleBackColor = true;
            // 
            // soundPlayerDeluxePlayOnceBox
            // 
            this.soundPlayerDeluxePlayOnceBox.AutoSize = true;
            this.soundPlayerDeluxePlayOnceBox.Checked = true;
            this.soundPlayerDeluxePlayOnceBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundPlayerDeluxePlayOnceBox.Location = new System.Drawing.Point(3, 3);
            this.soundPlayerDeluxePlayOnceBox.Name = "soundPlayerDeluxePlayOnceBox";
            this.soundPlayerDeluxePlayOnceBox.Size = new System.Drawing.Size(95, 24);
            this.soundPlayerDeluxePlayOnceBox.TabIndex = 0;
            this.soundPlayerDeluxePlayOnceBox.TabStop = true;
            this.soundPlayerDeluxePlayOnceBox.Text = "Play Once";
            this.soundPlayerDeluxePlayOnceBox.UseVisualStyleBackColor = true;
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
            this.warFileInfoPanel.Size = new System.Drawing.Size(303, 472);
            this.warFileInfoPanel.TabIndex = 6;
            // 
            // forceWaveVersionButton
            // 
            this.forceWaveVersionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forceWaveVersionButton.Location = new System.Drawing.Point(3, 110);
            this.forceWaveVersionButton.Name = "forceWaveVersionButton";
            this.forceWaveVersionButton.Size = new System.Drawing.Size(296, 23);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // vWavRevBox
            // 
            this.vWavRevBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vWavRevBox.Location = new System.Drawing.Point(205, 3);
            this.vWavRevBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vWavRevBox.Name = "vWavRevBox";
            this.vWavRevBox.Size = new System.Drawing.Size(95, 20);
            this.vWavRevBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.vWavRevBox, "Revision version of the file.");
            this.vWavRevBox.ValueChanged += new System.EventHandler(this.vWavRevBox_ValueChanged);
            // 
            // vWavMinBox
            // 
            this.vWavMinBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vWavMinBox.Location = new System.Drawing.Point(104, 3);
            this.vWavMinBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vWavMinBox.Name = "vWavMinBox";
            this.vWavMinBox.Size = new System.Drawing.Size(95, 20);
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
            this.vWavMajBox.Size = new System.Drawing.Size(95, 20);
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
            this.label6.Size = new System.Drawing.Size(299, 23);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(303, 26);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // vRevBoxWar
            // 
            this.vRevBoxWar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vRevBoxWar.Location = new System.Drawing.Point(205, 3);
            this.vRevBoxWar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vRevBoxWar.Name = "vRevBoxWar";
            this.vRevBoxWar.Size = new System.Drawing.Size(95, 20);
            this.vRevBoxWar.TabIndex = 2;
            this.toolTip.SetToolTip(this.vRevBoxWar, "Revision version of the file.");
            this.vRevBoxWar.ValueChanged += new System.EventHandler(this.vRevBoxWar_ValueChanged);
            // 
            // vMinBoxWar
            // 
            this.vMinBoxWar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMinBoxWar.Location = new System.Drawing.Point(104, 3);
            this.vMinBoxWar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.vMinBoxWar.Name = "vMinBoxWar";
            this.vMinBoxWar.Size = new System.Drawing.Size(95, 20);
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
            this.vMajBoxWar.Size = new System.Drawing.Size(95, 20);
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
            this.label5.Size = new System.Drawing.Size(299, 23);
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
            // sequenceEditorPanel
            // 
            this.sequenceEditorPanel.Controls.Add(this.sequenceEditor);
            this.sequenceEditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceEditorPanel.Location = new System.Drawing.Point(0, 0);
            this.sequenceEditorPanel.Name = "sequenceEditorPanel";
            this.sequenceEditorPanel.Size = new System.Drawing.Size(606, 472);
            this.sequenceEditorPanel.TabIndex = 3;
            this.sequenceEditorPanel.Visible = false;
            // 
            // sequenceEditor
            // 
            this.sequenceEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceEditor.Location = new System.Drawing.Point(0, 0);
            this.sequenceEditor.Name = "sequenceEditor";
            this.sequenceEditor.Size = new System.Drawing.Size(606, 472);
            this.sequenceEditor.TabIndex = 0;
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeIcons;
            this.tree.Indent = 12;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            treeNode1.ImageIndex = 10;
            treeNode1.Name = "fileInfo";
            treeNode1.SelectedImageIndex = 10;
            treeNode1.Text = "File Information";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
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
            this.treeIcons.Images.SetKeyName(12, "version.png");
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
            // filesMenu
            // 
            this.filesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.changeExternalPathToolStripMenuItem});
            this.filesMenu.Name = "contextMenuStrip1";
            this.filesMenu.Size = new System.Drawing.Size(187, 70);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("replaceToolStripMenuItem.Image")));
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.ReplaceToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // changeExternalPathToolStripMenuItem
            // 
            this.changeExternalPathToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("changeExternalPathToolStripMenuItem.Image")));
            this.changeExternalPathToolStripMenuItem.Name = "changeExternalPathToolStripMenuItem";
            this.changeExternalPathToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.changeExternalPathToolStripMenuItem.Text = "Change External Path";
            this.changeExternalPathToolStripMenuItem.Click += new System.EventHandler(this.ChangeExternalPathToolStripMenuItem_Click);
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
            this.versionPanel.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpVersionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpVersionMax)).EndInit();
            this.tableLayoutPanel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpVersionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpVersionMax)).EndInit();
            this.tableLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stmVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmVersionMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmVersionMin)).EndInit();
            this.tableLayoutPanel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wsdVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdVersionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdVersionMax)).EndInit();
            this.tableLayoutPanel13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.warVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warVersionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warVersionMax)).EndInit();
            this.tableLayoutPanel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bankVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankVersionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankVersionMax)).EndInit();
            this.tableLayoutPanel15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.versionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.versionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.versionMax)).EndInit();
            this.tableLayoutPanel16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seqVersionRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seqVersionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seqVersionMax)).EndInit();
            this.fileInfoPanel.ResumeLayout(false);
            this.fileInfoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesGroupGrid)).EndInit();
            this.trackPanel.ResumeLayout(false);
            this.trackPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackChannelGrid)).EndInit();
            this.tableLayoutPanel33.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackSendC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSendMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBiquadValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackLPFFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSpan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            this.stmPanel.ResumeLayout(false);
            this.tableLayoutPanel29.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel27.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stmPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmAllocateChannelsNum)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel28.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stmSendC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmSendMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmLoopEndFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stmLoopStartFrame)).EndInit();
            this.tableLayoutPanel30.ResumeLayout(false);
            this.tableLayoutPanel31.ResumeLayout(false);
            this.tableLayoutPanel32.ResumeLayout(false);
            this.tableLayoutPanel32.PerformLayout();
            this.wsdPanel.ResumeLayout(false);
            this.wsdPanel.PerformLayout();
            this.tableLayoutPanel26.ResumeLayout(false);
            this.tableLayoutPanel26.PerformLayout();
            this.tableLayoutPanel25.ResumeLayout(false);
            this.tableLayoutPanel24.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wsdChannelPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdTracksToAllocate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsdWaveIndex)).EndInit();
            this.seqPanel.ResumeLayout(false);
            this.seqPanel.PerformLayout();
            this.tableLayoutPanel23.ResumeLayout(false);
            this.tableLayoutPanel22.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel21.PerformLayout();
            this.tableLayoutPanel19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seqChannelPriorityBox)).EndInit();
            this.tableLayoutPanel20.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seqOffsetManualBox)).EndInit();
            this.soundGrpPanel.ResumeLayout(false);
            this.soundGrpGridTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpWaveArchives)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpFilesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpEndIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundGrpStartIndex)).EndInit();
            this.bankPanel.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bankWarDataGrid)).EndInit();
            this.warInfoPanel.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel17.PerformLayout();
            this.grpPanel.ResumeLayout(false);
            this.playerInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playerHeapSizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerSoundLimitBox)).EndInit();
            this.sarProjectInfoPanel.ResumeLayout(false);
            this.sarProjectInfoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsPIBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.streamBufferTimesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumTracksBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWaveNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumChannelsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumTracksBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxStreamNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqTrackNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSeqNumBox)).EndInit();
            this.soundPlayerDeluxePanel.ResumeLayout(false);
            this.soundDeluxeTrack3.ResumeLayout(false);
            this.soundDeluxeTrack3.PerformLayout();
            this.soundDeluxeTrack2.ResumeLayout(false);
            this.soundDeluxeTrack1.ResumeLayout(false);
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
            this.nullFilePanel.ResumeLayout(false);
            this.noInfoPanel.ResumeLayout(false);
            this.sequenceEditorPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.rootMenu.ResumeLayout(false);
            this.nodeMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.filesMenu.ResumeLayout(false);
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
        public string GetFileSaverPath(string description, string extension, ref WriteMode writeMode) {

            //Set filer.
            saveFileDialog.FileName = "";
            saveFileDialog.Filter = description + " (3ds or Wii U)|" + "*.bf" + extension.ToLower() + ";*.bc" + extension.ToLower() + "|" + description + " (Switch)|*.bf" + extension.ToLower();
            saveFileDialog.ShowDialog();            

            //Set write mode.
            if (saveFileDialog.FileName != "") {

                //Fix extension.
                if (Path.GetExtension(saveFileDialog.FileName) == "") {
                    saveFileDialog.FileName += ".bf" + extension.ToLower();
                }

                //3ds or WiiU.
                if (saveFileDialog.FilterIndex == 1) {

                    //3ds.
                    if (Path.GetExtension(saveFileDialog.FileName).StartsWith(".bc")) {
                        writeMode = WriteMode.CTR;
                    }

                    //WiiU.
                    else if (Path.GetExtension(saveFileDialog.FileName).StartsWith(".bf")) {
                        writeMode = WriteMode.Cafe;
                    }

                }

                //Switch.
                else {

                    //Switch.
                    if (Path.GetExtension(saveFileDialog.FileName).StartsWith(".bf")) {
                        writeMode = WriteMode.NX;
                    }

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
        public virtual void newToolStripMenuItem_Click(object sender, EventArgs e) {

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
        public virtual void openToolStripMenuItem_Click(object sender, EventArgs e) {

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
                try { br.Dispose(); } catch { }
                try { src.Dispose(); } catch { }

                //Update.
                UpdateNodes();
                DoInfoStuff();

            }

        }

        //Save.
        public virtual void saveToolStripMenuItem_Click(object sender, EventArgs e) {

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
                File.Write(WriteMode, bw);

                //Save the file.
                System.IO.File.WriteAllBytes(FilePath, o.ToArray());

            }

        }

        //Save as.
        public void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //Get the save file name.
            string path = GetFileSaverPath(ExtensionDescription, Extension, ref WriteMode);

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
            WriteMode wM = WriteMode;
            string path = GetFileSaverPath(ExtensionDescription, Extension, ref wM);

            //If the path is valid.
            if (path != "") {

                //Write the file.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                //Write the file.
                File.Write(wM, bw);

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

        public void expandToolStripMenuItem_Click(object sender, EventArgs e) {
            tree.SelectedNode.Expand();
        }

        public void collapseToolStripMenuItem_Click(object sender, EventArgs e) {
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


        //Launchers.
        #region Launchers

        private void SolarAudioSlayerToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start audio slayer.
            Process.Start("Solar Audio Slayer.exe");

        }

        private void IsabelleSoundEditorWAVSTMToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start editor.
            IsabelleSoundEditor z = new IsabelleSoundEditor();
            z.Show();

        }

        private void BrewstersWARBrewerToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start editor.
            EditorBase z = new Brewster_WAR_Brewer(MainWindow);
            z.Show();

        }

        private void WolfgangsDataWriterWSDToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start editor.
            EditorBase z = new Wolfgang_WSD_Writer(MainWindow);
            z.Show();

        }

        private void BBBsBankerBNKToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start editor.
            EditorBase z = new Brewster_WAR_Brewer(MainWindow);
            z.Show();

        }

        private void SSSsSequencerSEQToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start editor.
            EditorBase z = new Static_Sequencer(MainWindow);
            z.Show();

        }

        private void GoldisGrouperGRPToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start editor.
            EditorBase z = new Goldi_GRP_Grouper(MainWindow);
            z.Show();

        }

        #endregion

        //SAR Project info.
        #region SARProjectInfo

        private void MaxSeqNumBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void MaxSeqTrackNumBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void MaxStreamNumBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void MaxStreamNumTracksBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void MaxStreamNumChannelsBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void MaxWaveNumBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void MaxWaveNumTracksBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void StreamBufferTimesBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void OptionsPIBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        private void SarIncludeStringBlock_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                SarProjectInfoUpdated();
            }
        }

        public virtual void SarProjectInfoUpdated() { }

        #endregion

        public virtual void StmSound3dButton_Click(object sender, EventArgs e) {}

        public virtual void WsdSound3dButton_Click(object sender, EventArgs e) {}

        public virtual void SeqEditSound3dInfoButton_Click(object sender, EventArgs e) {}

        public virtual void SeqEditSoundInfoButton_Click(object sender, EventArgs e) {}

        public virtual void WsdEditSoundInfoButton_Click(object sender, EventArgs e) {}

        public virtual void StmSoundInfoButton_Click(object sender, EventArgs e) {}

        public virtual void FileTypeBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void PlayerSoundLimitBox_ValueChanged(object sender, EventArgs e) {}

        public virtual void PlayerEnableSoundLimitBox_CheckedChanged(object sender, EventArgs e) {}

        public virtual void PlayerHeapSizeBox_ValueChanged(object sender, EventArgs e) {}

        public virtual void SarGrpFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SarWarFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void WarLoadIndividuallyBox_CheckedChanged(object sender, EventArgs e) {}

        public virtual void WarIncludeWaveCountBox_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SarBnkFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void BnkWarsChanged(object sender, EventArgs e) {}

        public virtual void SoundGrpStartIndex_ValueChanged(object sender, EventArgs e) {}

        public virtual void SoundGrpEndIndex_ValueChanged(object sender, EventArgs e) {}

        public virtual void SoundGroupFilesChanged(object sender, EventArgs e) {}

        public virtual void SoundGroupWarsChanged(object sender, EventArgs e) {}

        public virtual void SarSeqFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SarSeqPlay_Click(object sender, EventArgs e) {}

        public virtual void SarSeqPause_Click(object sender, EventArgs e) {}

        public virtual void SarSeqStop_Click(object sender, EventArgs e) {}

        public virtual void SeqSound3dInfoExists_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqBank0Box_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SeqBank1Box_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SeqBank2Box_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SeqBank3Box_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SeqOffsetFromLabelButton_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqOffsetManualButton_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqOffsetFromLabelBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SeqOffsetManualBox_ValueChanged(object sender, EventArgs e) {}

        public virtual void SeqChannelPriorityBox_ValueChanged(object sender, EventArgs e) {}

        public virtual void SeqIsReleasePriorityBox_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC0_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC1_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC2_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC3_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC4_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC5_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC6_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC7_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC8_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC9_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC10_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC11_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC12_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC13_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC14_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SeqC15_CheckedChanged(object sender, EventArgs e) {}

        public virtual void SarWsdFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void SarWsdPlay_Click(object sender, EventArgs e) {}

        public virtual void SarWsdPause_Click(object sender, EventArgs e) {}

        public virtual void SarWsdStop_Click(object sender, EventArgs e) {}

        public virtual void WsdSound3dEnable_CheckedChanged(object sender, EventArgs e) {}

        public virtual void WsdWaveIndex_ValueChanged(object sender, EventArgs e) {}

        public virtual void WsdTracksToAllocate_ValueChanged(object sender, EventArgs e) {}

        public virtual void WsdCopyCount_Click(object sender, EventArgs e) {}

        public virtual void WsdChannelPriority_ValueChanged(object sender, EventArgs e) {}

        public virtual void WsdFixPriority_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void StmPlay_Click(object sender, EventArgs e) {}

        public virtual void StmPause_Click(object sender, EventArgs e) {}

        public virtual void StmStop_Click(object sender, EventArgs e) {}

        public virtual void StmSound3dEnable_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmWriteTrackInfo_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmUpdateTrackInfo_Click(object sender, EventArgs e) {}

        public virtual void StmTrack0_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack1_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack2_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack3_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack4_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack5_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack6_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack7_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack8_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack9_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack10_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack11_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack12_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack13_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack14_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmTrack15_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmStreamType_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void StmAllocateChannelsNum_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmCopyChannelCountFromFile_Click(object sender, EventArgs e) {}

        public virtual void StmPitch_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmIncludeExtension_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmLoopStartFrame_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmLoopEndFrame_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmCopyExtensionFromFile_Click(object sender, EventArgs e) {}

        public virtual void StmGeneratePrefetch_CheckedChanged(object sender, EventArgs e) {}

        public virtual void StmPrefetchFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void StmUpdatePrefetchInfo_Click(object sender, EventArgs e) {}

        public virtual void StmCreateUniquePrefetchFile_Click(object sender, EventArgs e) {}

        public virtual void StmSendMain_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmSendA_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmSendB_ValueChanged(object sender, EventArgs e) {}

        public virtual void StmSendC_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackVolume_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackPan_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackSpan_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackSurround_CheckedChanged(object sender, EventArgs e) {}

        public virtual void TrackLPFFrequency_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackBiquadType_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void TrackBiquadValue_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackSendMain_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackSendA_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackSendB_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackSendC_ValueChanged(object sender, EventArgs e) {}

        public virtual void TrackChannelsChanged(object sender, EventArgs e) {}

        public virtual void ByteOrderBox_SelectedIndexChanged(object sender, EventArgs e) {}

        public virtual void VersionMax_ValueChanged(object sender, EventArgs e) {}

        public virtual void VersionMin_ValueChanged(object sender, EventArgs e) {}

        public virtual void VersionRev_ValueChanged(object sender, EventArgs e) {}

        public virtual void SeqVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void BankVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void WarVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void WsdVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void GrpVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void StmVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void StpVersionUpdate_Click(object sender, EventArgs e) {}

        public virtual void FilesIncludeGroups_CheckedChanged(object sender, EventArgs e) {}

        public virtual void FilesGroupGridCellChanged(object sender, EventArgs e) {}

        public virtual void ReplaceToolStripMenuItem_Click(object sender, EventArgs e) {}

        public virtual void ExportToolStripMenuItem_Click(object sender, EventArgs e) {}

        public virtual void ChangeExternalPathToolStripMenuItem_Click(object sender, EventArgs e) {}

    }

}