using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitraFileLoader;

namespace Citric_Composer
{
    public partial class MainWindow : Form
    {

        /// <summary>
        /// File open.
        /// </summary>
        public bool FileOpen = false;

        /// <summary>
        /// File name.
        /// </summary>
        public string FilePath = "";

        /// <summary>
        /// The main file.
        /// </summary>
        public SoundArchive File;

        /// <summary>
        /// When info is when.
        /// </summary>
        public bool InfoEditing;

        /// <summary>
        /// Citric path.
        /// </summary>
        public string CitricPath = Application.StartupPath;

        public MainWindow() {
            InitializeComponent();
            noInfoPanel.Show();
            noInfoPanel.BringToFront();
        }

        public MainWindow(string fileToOpen) {
            InitializeComponent();

            //Make a new file, and load it.
            File = SoundArchiveReader.ReadSoundArchive(fileToOpen);
            FileOpen = true;
            FilePath = fileToOpen;
            Text = "Citric Composer - " + Path.GetFileName(fileToOpen);
            noInfoPanel.Show();
            noInfoPanel.BringToFront();

            //Fix things.
            UpdateNodes();

        }


        /// <summary>
        /// Open a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Show dialog.
            openB_sarBox.ShowDialog();

            if (openB_sarBox.FileName != "") {

                //Make a new file, and load it.
                File = SoundArchiveReader.ReadSoundArchive(openB_sarBox.FileName);
                FileOpen = true;
                FilePath = openB_sarBox.FileName;
                Text = "Citric Composer - " + Path.GetFileName(openB_sarBox.FileName);

                //Fix things.
                openB_sarBox.FileName = "";
                UpdateNodes();

            }

        }


        //Tools and some other random stuff.
        #region ToolsAndStuff

        /// <summary>
        /// About.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutCitricComposerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //About windows.
            AboutWindow a = new AboutWindow();
            a.Show();
        }

        private void solarAudioSlayerToolStripMenuItem_Click(object sender, EventArgs e) {

            //Start audio slayer.
            Process.Start("Solar Audio Slayer.exe");

        }

        private void isabelleSoundEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Isabelle window.
            IsabelleSoundEditor a = new IsabelleSoundEditor();
            a.Show();
        }


        private void brewstersArchiveBrewerWARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Brewster window.
            Brewster_WAR_Brewer a = new Brewster_WAR_Brewer(this);
            a.Show();
        }

        private void goldisGrouperGRPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Goldi window.
            Goldi_GRP_Grouper a = new Goldi_GRP_Grouper(this);
            a.Show();
        }

        private void wolfangsWriterWSDToolStripMenuItem_Click(object sender, EventArgs e) {

            //Wolfgang window.
            Wolfgang_WSD_Writer w = new Wolfgang_WSD_Writer(this);
            w.Show();

        }

        private void sSSSequencerSEQToolStripMenuItem_Click(object sender, EventArgs e) {

            //Sequencer.
            SSS_Sequencer s = new SSS_Sequencer(this);
            s.Show();

        }

        #endregion


        //Info panel stuff.
        #region InfoPanelStuff

        /// <summary>
        /// Do the mario!
        /// </summary>
        public void DoInfoStuff() {

            //Editing.
            InfoEditing = true;

            //Index.
            int ind = tree.SelectedNode.Index;

            //Protect.
            if (tree.SelectedNode == null) {
                tree.SelectedNode = tree.Nodes[0];
            }

            //If not null.
            if (tree.SelectedNode != null && FileOpen) {

                //Project info.
                if (tree.Nodes["projectInfo"] == tree.SelectedNode) {

                    //Show project info panel.
                    projectInfoPanel.BringToFront();
                    projectInfoPanel.Show();

                    //Update boxes.
                    maxSeqNumBox.Value = File.MaxSequences;
                    maxSeqTrackNumBox.Value = File.MaxSequenceTracks;
                    maxStreamNumBox.Value = File.MaxStreamSounds;
                    maxStreamNumTracksBox.Value = File.MaxStreamTracks;
                    maxStreamNumChannelsBox.Value = File.MaxStreamChannels;
                    maxWaveNumBox.Value = File.MaxWaveSounds;
                    maxWaveNumTracksBox.Value = File.MaxWaveTracks;
                    streamBufferTimesBox.Value = File.StreamBufferTimes;
                    optionsPIBox.Value = File.Options;

                }

                //Not null parents.
                if (tree.SelectedNode.Parent != null) {

                    //Parent of parent is not null.
                    if (tree.SelectedNode.Parent.Parent != null) {

                    }

                    //Sequences.
                    if (tree.Nodes["sequences"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Sequences[ind] != null) {

                            //Show panel.
                            ShowPanelWithFileInfo(seqPanel, File.Sequences[ind].File);

                            //3d info enabled.
                            if (File.Sequences[ind] != null) {
                                seqSound3dInfoExists.Checked = seqEditSound3dInfoButton.Enabled = File.Sequences[ind].Sound3dInfo != null;
                            }

                            //Add bank items.
                            seqBank0Box.Items.Clear();
                            seqBank1Box.Items.Clear();
                            seqBank2Box.Items.Clear();
                            seqBank3Box.Items.Clear();
                            seqBank0Box.Items.Add("{ Null Bank }");
                            seqBank1Box.Items.Add("{ Null Bank }");
                            seqBank2Box.Items.Add("{ Null Bank }");
                            seqBank3Box.Items.Add("{ Null Bank }");
                            int bnkCount = 0;
                            foreach (BankEntry b in File.Banks) {

                                //Add the bank.
                                string bnkName = "{ Null Bank }";
                                if (b != null) {

                                    //Set name.
                                    bnkName = "{ Unknown Bank Name }";

                                    //Get name.
                                    if (b.Name != null) {
                                        bnkName = b.Name;
                                    }

                                }

                                //Add banks.
                                seqBank0Box.Items.Add("[" + bnkCount + "] " + bnkName);
                                seqBank1Box.Items.Add("[" + bnkCount + "] " + bnkName);
                                seqBank2Box.Items.Add("[" + bnkCount + "] " + bnkName);
                                seqBank3Box.Items.Add("[" + bnkCount + "] " + bnkName);

                                //Increment count.
                                bnkCount++;

                            }

                            //Get banks.
                            for (int i = 0; i < 4; i++) {

                                //Bank index.
                                int num = 0;
                                if (File.Sequences[ind].Banks[i] != null) {
                                    num = File.Banks.IndexOf(File.Sequences[ind].Banks[i]) + 1;
                                }

                                //Set.
                                switch (i) {
                                    case 0:
                                        seqBank0Box.SelectedIndex = num;
                                        break;
                                    case 1:
                                        seqBank1Box.SelectedIndex = num;
                                        break;
                                    case 2:
                                        seqBank2Box.SelectedIndex = num;
                                        break;
                                    case 3:
                                        seqBank3Box.SelectedIndex = num;
                                        break;
                                }

                            }

                            //Get start offset.
                            seqOffsetFromLabelBox.Items.Clear();
                            bool labelMatch = false;

                            //File is real.
                            if (File.Sequences[ind].File != null) {
                                if (File.Sequences[ind].File.File != null) {

                                    //Fill box.
                                    int cnt = 0;
                                    foreach (var e in (File.Sequences[ind].File.File as SoundSequence).Labels.Where(x => x != null)) {
                                        seqOffsetFromLabelBox.Items.Add(e.Label);

                                        //If offset matches.
                                        if (File.Sequences[ind].StartOffset == e.Offset && !labelMatch) {
                                            labelMatch = true;
                                            seqOffsetFromLabelBox.SelectedIndex = cnt;
                                        }

                                        //Add count.
                                        cnt++;

                                    }

                                }
                            }

                            //Set value.
                            if (labelMatch) {
                                seqOffsetManualBox.Enabled = false;
                                seqOffsetManualBox.Value = 0;
                                seqOffsetFromLabelBox.Enabled = seqOffsetFromLabelButton.Checked = true;
                            } else {
                                seqOffsetManualButton.Checked = seqOffsetManualBox.Enabled = true;
                                seqOffsetFromLabelBox.Enabled = false;
                                seqOffsetManualBox.Value = File.Sequences[ind].StartOffset;
                            }

                            //Channels.
                            seqC0.Checked = File.Sequences[ind].ChannelFlagEnabled(0);
                            seqC1.Checked = File.Sequences[ind].ChannelFlagEnabled(1);
                            seqC2.Checked = File.Sequences[ind].ChannelFlagEnabled(2);
                            seqC3.Checked = File.Sequences[ind].ChannelFlagEnabled(3);
                            seqC4.Checked = File.Sequences[ind].ChannelFlagEnabled(4);
                            seqC5.Checked = File.Sequences[ind].ChannelFlagEnabled(5);
                            seqC6.Checked = File.Sequences[ind].ChannelFlagEnabled(6);
                            seqC7.Checked = File.Sequences[ind].ChannelFlagEnabled(7);
                            seqC8.Checked = File.Sequences[ind].ChannelFlagEnabled(8);
                            seqC9.Checked = File.Sequences[ind].ChannelFlagEnabled(9);
                            seqC10.Checked = File.Sequences[ind].ChannelFlagEnabled(10);
                            seqC11.Checked = File.Sequences[ind].ChannelFlagEnabled(11);
                            seqC12.Checked = File.Sequences[ind].ChannelFlagEnabled(12);
                            seqC13.Checked = File.Sequences[ind].ChannelFlagEnabled(13);
                            seqC14.Checked = File.Sequences[ind].ChannelFlagEnabled(14);
                            seqC15.Checked = File.Sequences[ind].ChannelFlagEnabled(15);

                            //Channel priority.
                            seqChannelPriorityBox.Value = File.Sequences[ind].ChannelPriority;

                            //Is priority release.
                            seqIsReleasePriorityBox.Checked = File.Sequences[ind].IsReleasePriority;

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Banks.
                    else if (tree.Nodes["banks"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Banks[ind] != null) {

                            //Show panel.
                            ShowPanelWithFileInfo(bankPanel, File.Banks[ind].File);

                            //Show wave archives.
                            bankWarDataGrid.Rows.Clear();
                            ((DataGridViewComboBoxColumn)bankWarDataGrid.Columns[0]).Items.Clear();
                            ((DataGridViewComboBoxColumn)bankWarDataGrid.Columns[0]).Items.Add("{ Null Wave Archive }");
                            for (int i = 0; i < File.WaveArchives.Count; i++) {

                                //Name of wave archive.
                                string name = "{ Null Archive or Unknown Name }";
                                try { name = File.WaveArchives[i].Name; } catch { }
                                if (name == null) { name = "{ Null Archive or Unknown Name }"; }
                                if (name.Equals("{ Null Archive or Unknown Name }")) { name = "{ Null Archive or Unknown Name }"; }

                                //Add to list.
                                ((DataGridViewComboBoxColumn)bankWarDataGrid.Columns[0]).Items.Add(i + " - " + name);

                            }

                            //Add selected wave archives.
                            foreach (var e in File.Banks[ind].WaveArchives) {
                                bankWarDataGrid.Rows.Add(new DataGridViewRow());
                                int valNum = 0;
                                if (e != null) {
                                    valNum = File.WaveArchives.IndexOf(e) + 1;
                                }
                                var h = ((DataGridViewComboBoxCell)bankWarDataGrid.Rows[bankWarDataGrid.Rows.Count - 2].Cells[0]);
                                h.Value = h.Items[valNum];
                            }

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Wave archives.
                    else if (tree.Nodes["waveArchives"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.WaveArchives[ind] != null) {

                            //Show panel.
                            ShowPanelWithFileInfo(warInfoPanel, File.WaveArchives[ind].File);

                            //Checkboxes.
                            warLoadIndividuallyBox.Checked = File.WaveArchives[ind].LoadIndividually;
                            warIncludeWaveCountBox.Checked = File.WaveArchives[ind].IncludeWaveCount;

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Groups.
                    else if (tree.Nodes["groups"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Groups[ind] != null) {

                            //Show panel.
                            ShowPanelWithFileInfo(grpPanel, File.Groups[ind].File);

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Players.
                    else if (tree.Nodes["players"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Players[ind] != null) {

                            //Show panel.
                            playerInfoPanel.Show();
                            playerInfoPanel.BringToFront();

                            //Checkboxes.
                            playerSoundLimitBox.Value = File.Players[ind].SoundLimit;
                            playerEnableSoundLimitBox.Checked = File.Players[ind].IncludeHeapSize;
                            if (playerEnableSoundLimitBox.Checked) {
                                playerHeapSizeBox.Value = File.Players[ind].PlayerHeapSize;
                                playerHeapSizeBox.Enabled = true;
                            } else {
                                playerHeapSizeBox.Value = 0;
                                playerHeapSizeBox.Enabled = false;
                            }

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Files.
                    else if (tree.Nodes["files"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Files[ind] != null) {

                            //Show panel.
                            fileInfoPanel.Show();
                            fileInfoPanel.BringToFront();

                            //Type.
                            fileTypeBox.SelectedIndex = (int)File.Files[ind].FileType;

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Other.
                    else {

                        //Show no info panel.
                        noInfoPanel.Show();
                        noInfoPanel.BringToFront();

                    }

                }

                //Other.
                else if (tree.Nodes["projectInfo"] != tree.SelectedNode) {

                    //Show no info panel.
                    noInfoPanel.Show();
                    noInfoPanel.BringToFront();

                }

            }

            //Other.
            else {

                //Show no info panel.
                noInfoPanel.Show();
                noInfoPanel.BringToFront();

            }

            //End editing.
            InfoEditing = false;

        }

        /// <summary>
        /// Show panel with file info.
        /// </summary>
        /// <param name="p">The panel.</param>
        /// <param name="file">File to use.</param>
        public void ShowPanelWithFileInfo(Panel p, SoundFile<ISoundFile> file) {

            //Do stuff.
            p.Show();
            fileIdPanel.Show();
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(fileIdPanel);
            mainPanel.Controls[0].Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            p.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            mainPanel.Controls.Add(p);
            var h = mainPanel.Controls[1].Location;
            h.Y = mainPanel.Controls[0].Location.Y + mainPanel.Controls[0].Height;
            mainPanel.Controls[1].Location = h;
            mainPanel.Show();
            mainPanel.BringToFront();

            //Add files.
            fileIdBox.Items.Clear();
            fileIdBox.Items.Add("{ Null File }");
            for (int i = 0; i < File.Files.Count; i++) {
                string name = "{ Null File or Unknown Name }";
                try {
                    name = File.Files[i].FileName + "." + File.Files[i].FileExtension;
                } catch { }
                fileIdBox.Items.Add(i + " - " + name);
            }
            if (file == null) {
                fileIdBox.SelectedIndex = 0;
            } else {
                fileIdBox.SelectedIndex = File.Files.IndexOf(file.Reference) + 1;
            }

        }

        /// <summary>
        /// File id changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileIdBox_SelectedIndexChanged(object sender, EventArgs e) {

            //Make sure possible.
            if (FileOpen && tree.SelectedNode != null && !InfoEditing) {

                //Parent is not null.
                if (tree.SelectedNode.Parent != null) {

                    //Parent of parent is null.
                    if (tree.SelectedNode.Parent.Parent == null) {

                        //New file.
                        SoundFile<ISoundFile> refFile = null;
                        int ind = fileIdBox.SelectedIndex;
                        if (ind != 0) {
                            refFile = new SoundFile<ISoundFile>() { Reference = File.Files[ind - 1] };
                        }

                        //Set ref file.
                        switch (tree.SelectedNode.Parent.Name) {

                            case "streams":
                                File.Streams[tree.SelectedNode.Index].File = refFile;
                                break;

                            case "waveSoundSets":
                                File.WaveSoundDatas[tree.SelectedNode.Index].File = refFile;
                                break;

                            case "sequences":
                                File.Sequences[tree.SelectedNode.Index].File = refFile;
                                break;

                            case "banks":
                                File.Banks[tree.SelectedNode.Index].File = refFile;
                                break;

                            case "waveArchives":
                                File.WaveArchives[tree.SelectedNode.Index].File = refFile;
                                break;

                            case "groups":
                                File.Groups[tree.SelectedNode.Index].File = refFile;
                                break;

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Bank grid cell changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BankWarGridCellChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen) {

                File.Banks[tree.SelectedNode.Index].WaveArchives = new List<WaveArchiveEntry>();
                for (int i = 1; i < bankWarDataGrid.Rows.Count; i++) {
                    int ind = ((DataGridViewComboBoxCell)bankWarDataGrid.Rows[i - 1].Cells[0]).Items.IndexOf(((DataGridViewComboBoxCell)bankWarDataGrid.Rows[i - 1].Cells[0]).Value);
                    if (ind == 0) {
                        File.Banks[tree.SelectedNode.Index].WaveArchives.Add(null);
                    } else {
                        File.Banks[tree.SelectedNode.Index].WaveArchives.Add(File.WaveArchives[ind-1]);
                    }
                }

            }

        }

        /// <summary>
        /// War flag changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void warLoadIndividuallyBox_CheckedChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen) {
                File.WaveArchives[tree.SelectedNode.Index].LoadIndividually = warLoadIndividuallyBox.Checked;
            }

        }

        /// <summary>
        /// War flag changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void warIncludeWaveCountBox_CheckedChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen) {
                File.WaveArchives[tree.SelectedNode.Index].IncludeWaveCount = warIncludeWaveCountBox.Checked;
            }

        }

        /// <summary>
        /// Player sound limit changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerSoundLimitBox_ValueChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen) {
                File.Players[tree.SelectedNode.Index].SoundLimit = (int)playerSoundLimitBox.Value;
            }

        }

        /// <summary>
        /// Player enable sound heap.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerEnableSoundLimitBox_CheckedChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen) {
                File.Players[tree.SelectedNode.Index].IncludeHeapSize = playerHeapSizeBox.Enabled = playerEnableSoundLimitBox.Checked;
                if (!playerHeapSizeBox.Enabled) {
                    playerHeapSizeBox.Value = 0;
                } else {
                    playerHeapSizeBox.Value = File.Players[tree.SelectedNode.Index].PlayerHeapSize;
                }
            }

        }

        /// <summary>
        /// Player heap size box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerHeapSizeBox_ValueChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen && playerHeapSizeBox.Enabled) {
                File.Players[tree.SelectedNode.Index].PlayerHeapSize = (int)playerHeapSizeBox.Value;
            }

        }

        /// <summary>
        /// Change file type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileTypeBox_SelectedIndexChanged(object sender, EventArgs e) {

            if (!InfoEditing && FileOpen) {
                File.Files[tree.SelectedNode.Index].FileType = (EFileType)fileTypeBox.SelectedIndex;
                UpdateNodes();
            }

        }

        #endregion


        //Node shit.
        #region NodeShit

        //Expand node and parents.
        void expandNodePath(TreeNode node)
        {
            if (node == null)
                return;
            if (node.Level != 0) //check if it is not root
            {
                node.Expand();
                expandNodePath(node.Parent);
            }
            else
            {
                node.Expand(); // this is root 
            }

        }

        //Make right click actually select, and show infoViewer.
        void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                tree.SelectedNode = tree.GetNodeAt(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Left)
            {
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

            //Open the item if possible.
            if (tree.SelectedNode.Parent != null) {

                //Editor to open.
                EditorBase b = null;

                //Parent is the wave sound archives.
                if (tree.SelectedNode.Parent == tree.Nodes["waveArchives"]) {

                    //Open the wave archive file.
                    if (File.WaveArchives[tree.SelectedNode.Index].File != null) {

                        //Open the wave archive.
                        b = new Brewster_WAR_Brewer(File.WaveArchives[tree.SelectedNode.Index].File, this);

                    } else {

                        //No file able to open.
                        MessageBox.Show("You can't open a file for an entry that does not have a file attached!", "Notice:");

                    }

                }

                //Parent is the streams.
                else if (tree.SelectedNode.Parent == tree.Nodes["streams"]) {

                    //Open the file.
                    if (File.Streams[tree.SelectedNode.Index].File != null) {

                        //Internal.
                        if (File.Streams[tree.SelectedNode.Index].File.FileType != EFileType.External) {

                            //Wut.
                            MessageBox.Show("Do to the nature of Isabelle, you have to open internal streams through the files folder.", "Notice:");

                        } else {

                            //Open isabelle.
                            IsabelleSoundEditor i = new IsabelleSoundEditor(Path.GetDirectoryName(FilePath) + "/" + File.Streams[tree.SelectedNode.Index].File.ExternalFileName);
                            i.Show();

                        }

                    } else {

                        //No file able to open.
                        MessageBox.Show("You can't open a file for an entry that does not have a file attached!", "Notice:");

                    }

                }

                //Parent is the groups.
                else if (tree.SelectedNode.Parent == tree.Nodes["groups"]) {

                    //Open the file.
                    if (File.Groups[tree.SelectedNode.Index].File != null) {

                        //Open it.
                        b = new Goldi_GRP_Grouper(File.Groups[tree.SelectedNode.Index].File, this);

                    } else {

                        //No file able to open.
                        MessageBox.Show("You can't open a file for an entry that does not have a file attached!", "Notice:");

                    }

                }

                //Parent is the wave sound data.
                else if (tree.SelectedNode.Parent == tree.Nodes["waveSoundSets"]) {

                    //Open the file.
                    if (File.WaveSoundDatas[tree.SelectedNode.Index].File != null) {

                        //Open it.
                        b = new Wolfgang_WSD_Writer(File.WaveSoundDatas[tree.SelectedNode.Index].File, this);

                    } else {

                        //No file able to open.
                        MessageBox.Show("You can't open a file for an entry that does not have a file attached!", "Notice:");

                    }

                }

                //Parent is the sequence.
                else if (tree.SelectedNode.Parent == tree.Nodes["sequences"]) {

                    //Open the file.
                    if (File.Sequences[tree.SelectedNode.Index].File != null) {

                        //Open it.
                        b = new SSS_Sequencer(File.Sequences[tree.SelectedNode.Index].File, this);

                    } else {

                        //No file able to open.
                        MessageBox.Show("You can't open a file for an entry that does not have a file attached!", "Notice:");

                    }

                }

                //Last resort is the file.
                else if (tree.SelectedNode.Parent == tree.Nodes["files"]) {

                    //Make sure file is not null.
                    if (File.Files[tree.SelectedNode.Index] != null) {

                        //Get the extension.
                        switch (File.Files[tree.SelectedNode.Index].FileExtension.Substring(File.Files[tree.SelectedNode.Index].FileExtension.Length - 3, 3).ToLower()) {

                            //Wave archive.
                            case "war":
                                b = new Brewster_WAR_Brewer(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Sequence.
                            case "seq":
                                b = new SSS_Sequencer(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Group.
                            case "grp":
                                b = new Goldi_GRP_Grouper(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Wave sound data.
                            case "wsd":
                                b = new Wolfgang_WSD_Writer(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Steam.
                            case "stm":

                                //Internal.
                                if (File.Files[tree.SelectedNode.Index].FileType != EFileType.External) {

                                    //Valid.
                                    if (File.Files[tree.SelectedNode.Index].File != null) {

                                        //Open STM.
                                        string name = File.Files[tree.SelectedNode.Index].FileName;
                                        if (name == null) {
                                            name = "{ Null File Name }";
                                        }
                                        IsabelleSoundEditor i = new IsabelleSoundEditor(this, tree.SelectedNode.Index, name + File.Files[tree.SelectedNode.Index].FileExtension, false);
                                        i.Show();

                                    }

                                } else {

                                    //Open isabelle.
                                    IsabelleSoundEditor i = new IsabelleSoundEditor(Path.GetDirectoryName(FilePath) + "/" + File.Files[tree.SelectedNode.Index].ExternalFileName);
                                    i.Show();
                                    break;

                                }
                                break;

                        }

                    } else {

                        //No file able to open.
                        MessageBox.Show("You can't open a null file!", "Notice:");

                    }

                }

                //Run editor.
                if (b != null) {
                    b.Show();
                }

            }

        }

        void treeArrowKey(object sender, KeyEventArgs e)
        {

            DoInfoStuff();

        }

        //Get expanded nodes.
        List<string> collectExpandedNodes(TreeNodeCollection Nodes)
        {
            List<string> _lst = new List<string>();
            foreach (TreeNode checknode in Nodes)
            {
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
        TreeNode FindNodeByName(TreeNodeCollection NodesCollection, string Name)
        {
            TreeNode returnNode = null; // Default value to return
            foreach (TreeNode checkNode in NodesCollection)
            {
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

        private void RemoveChildNodes(TreeNode aNode)
        {
            if (aNode.Nodes.Count > 0)
            {
                for (int i = aNode.Nodes.Count - 1; i >= 0; i--)
                {
                    aNode.Nodes[i].Remove();
                }
            }

        }

        #endregion


        /// <summary>
        /// Update nodes.
        /// </summary>
        public void UpdateNodes() {

            //Update file.
            //NOTHING?

            //Get nodes that are currently expanded.
            List<string> expandedNodes = collectExpandedNodes(tree.Nodes);

            //First remove all nodes.
            tree.BeginUpdate();
            for (int i = 0; i < tree.Nodes.Count; i++)
            {

                RemoveChildNodes(tree.Nodes[i]);

            }

            //Load streams.
            int stmCount = 0;
            foreach (var s in File.Streams) {

                //Null.
                if (s == null) {
                    tree.Nodes["streams"].Nodes.Add("stream" + stmCount, "[" + stmCount + "] " + "{ Null Stream }", 1, 1);
                }

                //Valid.
                else {
                    string name = s.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["streams"].Nodes.Add("stream" + stmCount, "[" + stmCount + "] " + name, 1, 1);
                }

                //Increment count.
                stmCount++;

            }

            //Load wave sound infos.
            int wsdCount = stmCount;
            foreach (var w in File.WaveSoundDatas) {

                //Null.
                if (w == null) {
                    tree.Nodes["waveSoundSets"].Nodes.Add("waveSoundSet" + wsdCount, "[" + wsdCount + "] " + "{ Null Wave Sound Set }", 2, 2);
                }

                //Valid.
                else {
                    string name = w.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["waveSoundSets"].Nodes.Add("waveSoundSet" + wsdCount, "[" + wsdCount + "] " + name, 2, 2);
                }

                //Increment count.
                wsdCount++;

            }

            //Load sequences.
            int seqCount = wsdCount;
            foreach (var s in File.Sequences) {

                //Null.
                if (s == null) {
                    tree.Nodes["sequences"].Nodes.Add("sequence" + seqCount, "[" + seqCount + "] " + "{ Null Sequence }", 3, 3);
                }

                //Valid.
                else {
                    string name = s.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["sequences"].Nodes.Add("sequence" + seqCount, "[" + seqCount + "] " + name, 3, 3);
                }

                //Increment count.
                seqCount++;

            }

            //Load banks.
            int bCount = 0;
            foreach (var b in File.Banks) {

                //Null.
                if (b == null) {
                    tree.Nodes["banks"].Nodes.Add("bank" + bCount, "[" + bCount + "] " + "{ Null Bank }", 5, 5);
                }

                //Valid.
                else {
                    string name = b.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["banks"].Nodes.Add("bank" + bCount, "[" + bCount + "] " + name, 5, 5);
                }

                //Increment count.
                bCount++;

            }

            //Load players.
            int pCount = 0;
            foreach (var p in File.Players) {

                //Null.
                if (p == null) {
                    tree.Nodes["players"].Nodes.Add("player" + pCount, "[" + pCount + "] " + "{ Null Player }", 8, 8);
                }

                //Valid.
                else {
                    string name = p.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["players"].Nodes.Add("player" + pCount, "[" + pCount + "] " + name, 8, 8);
                }

                //Increment count.
                pCount++;

            }

            //Load groups.
            int gCount = 0;
            foreach (var g in File.Groups) {

                //Null.
                if (g == null) {
                    tree.Nodes["players"].Nodes.Add("group" + gCount, "[" + gCount + "] " + "{ Null Group }", 7, 7);
                }

                //Valid.
                else {
                    string name = g.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["groups"].Nodes.Add("group" + gCount, "[" + gCount + "] " + name, 7, 7);
                }

                //Increment count.
                gCount++;

            }

            //Load wave archives.
            int wCount = 0;
            foreach (var w in File.WaveArchives) {

                //Null.
                if (w == null) {
                    tree.Nodes["waveArchives"].Nodes.Add("waveArchives" + wCount, "[" + wCount + "] " + "{ Null Wave Archive }", 6, 6);
                }

                //Valid.
                else {
                    string name = w.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["waveArchives"].Nodes.Add("waveArchive" + wCount, "[" + wCount + "] " + name, 6, 6);
                }

                //Increment count.
                wCount++;

            }

            //Load sound groups.
            int sCount = 0;
            foreach (var s in File.SoundSets) {

                //Null.
                if (s == null) {
                    tree.Nodes["soundGroups"].Nodes.Add("soundGroup" + sCount, "[" + sCount + "] " + "{ Null Sound Group }", 4, 4);
                }

                //Valid.
                else {

                    //Add sound group.
                    string name = s.Name;
                    if (name == null) {
                        name = "{ Null Name }";
                    }
                    tree.Nodes["soundGroups"].Nodes.Add("soundGroup" + sCount, "[" + sCount + "] " + name, 4, 4);

                    //Add each sub entry.
                    for (int i = s.StartIndex; i <= s.EndIndex; i++) {

                        //Get the key name.
                        string key = "null";
                        switch (s.SoundType) {

                            case SoundType.Bank:
                                key = "banks";
                                break;

                            case SoundType.Group:
                                key = "groups";
                                break;

                            case SoundType.Null:
                                key = "null";
                                break;

                            case SoundType.Player:
                                key = "players";
                                break;

                            case SoundType.Sound:
                                key = "stream";
                                if (i >= File.Streams.Count) {
                                    key = "waveSoundSets";
                                }
                                if (i >= File.Streams.Count + File.WaveSoundDatas.Count) {
                                    key = "sequences";
                                }
                                break;

                            case SoundType.SoundGroup:
                                key = "soundGroups";
                                break;

                            case SoundType.WaveArchive:
                                key = "waveArchives";
                                break;

                        }

                        //Null key.
                        if (key.Equals("null")) {

                            //Add null.
                            tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, "{ Null Type }", 0, 0);

                        } else if (key.Equals("waveSoundSets")) {

                            //Add wave sound set.
                            tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, tree.Nodes[key].Nodes[i - File.Streams.Count].Text, tree.Nodes[key].ImageIndex, tree.Nodes[key].SelectedImageIndex);

                        } else if (key.Equals("sequences")) {

                            //Add sequence file.
                            tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, tree.Nodes[key].Nodes[i - File.Streams.Count - File.WaveSoundDatas.Count].Text, tree.Nodes[key].ImageIndex, tree.Nodes[key].SelectedImageIndex);

                        } else {

                            //Regular key.
                            tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, tree.Nodes[key].Nodes[i].Text, tree.Nodes[key].ImageIndex, tree.Nodes[key].SelectedImageIndex);

                        }

                    }

                }

                //Increment count.
                sCount++;

            }

            //Load files.
            int fCount = 0;
            foreach (var f in File.Files)
            {

                //Null.
                if (f == null) {
                    tree.Nodes["files"].Nodes.Add("file" + fCount, "[" + fCount + "] " + "{ Null File }", 0, 0);
                }

                //Valid.
                else {

                    string name = f.FileName;
                    if (name == null) {
                        name = "{ Null File Name }";
                    }
                    name += "." + f.FileExtension;

                    //Get the icon.
                    int icon = 0;
                    if (f.FileExtension.Length > 3) {
                        switch (f.FileExtension.Substring(2, 3)) {

                            case "seq":
                                icon = 3;
                                break;

                            case "stm":
                                icon = 1;
                                break;

                            case "wsd":
                                icon = 2;
                                break;

                            case "bnk":
                                icon = 5;
                                break;

                            case "war":
                                icon = 6;
                                break;

                            case "stp":
                                icon = 9;
                                break;

                            case "grp":
                                icon = 7;
                                break;

                        }
                    }

                    //Get content type.
                    string type = "(" + f.FileType.ToString() + ")";
                    switch (f.FileType) {

                        case EFileType.InGroupOnly:
                            type = "(In Group Only)";
                            break;

                        case EFileType.NullReference:
                            type = "(Null Reference)";
                            break;

                        case EFileType.Aras:
                            type = "(Inside Aras File)";
                            break;

                    }

                    //Name is external if external.
                    if (f.FileType == EFileType.External) {
                        name = f.ExternalFileName;
                    }

                    tree.Nodes["files"].Nodes.Add("file" + fCount, "[" + fCount + "] " + name + " " + type, icon, icon);
                }

                //Increment count.
                fCount++;

            }

            //Restore the nodes if they exist.
            if (expandedNodes.Count > 0)
            {
                TreeNode IamExpandedNode;
                for (int i = 0; i < expandedNodes.Count; i++)
                {
                    IamExpandedNode = FindNodeByName(tree.Nodes, expandedNodes[i]);
                    expandNodePath(IamExpandedNode);
                }

            }

            tree.SelectedNode = tree.Nodes[0];
            tree.EndUpdate();

        }

        //Tools and menus.
        #region ToolsAndMenus

        /// <summary>
        /// Extract to a folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            folderSelector.SelectedPath = "";
            folderSelector.ShowDialog();

            if (folderSelector.SelectedPath != null) {
                SoundArchiveWriter.WriteSoundArchiveToFolder(File, Path.GetDirectoryName(FilePath), folderSelector.SelectedPath);
            }

        }

        /// <summary>
        /// Import from a folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFromFolderToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            folderSelector.SelectedPath = "";
            folderSelector.ShowDialog();

            if (folderSelector.SelectedPath != null) {
                SoundArchiveReader.ImportSoundArchiveFromFolder(File, Path.GetDirectoryName(FilePath), folderSelector.SelectedPath);
            }

        }

        /// <summary>
        /// Export the symbol map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportSymbolMapToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            SaveFileDialog o = new SaveFileDialog();
            o.Filter = "Symbol Map|*.txt";
            o.RestoreDirectory = true;
            o.ShowDialog();

            if (o.FileName != "") {
                SoundArchiveWriter.ExportSymbols(File, o.FileName);
            }

        }

        /// <summary>
        /// Import a symbol map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importSymbolMapToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Symbol Map|*.txt";
            o.RestoreDirectory = true;
            o.ShowDialog();

            if (o.FileName != "") {
                SoundArchiveReader.ImportSymbols(File, o.FileName);
            }

            //Update.
            UpdateNodes();

        }

        /// <summary>
        /// Save the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //No where to save.
            if (FilePath == "") {

                //Save as.
                saveAsToolStripMenuItem_Click(sender, e);

                //Return.
                return;

            }

            //Save the file.
            //SoundArchiveWriter.WriteSoundArchive(File, Path.GetDirectoryName(FilePath), FilePath);
            MessageBox.Show("Saving has been disabled due to instability to protect your files. Please click the help button to join Gota's Sound Tools server to get a dev build that may support saving for your game.");

        }

        /// <summary>
        /// Save the file as something.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open test.
            if (!FileTest(sender, e, false, true)) {
                return;
            }

            //Get the save file name.
            string path = GetFileSaverPath("Sound Archive", "sar");

            //If the path is valid.
            if (path != "") {

                //Set values.
                FilePath = path;
                Text = "Citric Composer - " + Path.GetFileName(path);

                //Save the file.
                saveToolStripMenuItem_Click(sender, e);

            }

        }

        /// <summary>
        /// Get the path for saving a file.
        /// </summary>
        /// <param name="description">File description.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Path of the file to save.</returns>
        public string GetFileSaverPath(string description, string extension) {

            //Set filer.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "";
            if (extension.ToLower().Equals("sar")) {
                saveFileDialog.Filter = "Sound Archive|*.bfsar;*.bcsar";
            } else {
                saveFileDialog.Filter = description + "|*.bf" + extension.ToLower();
            }
            saveFileDialog.ShowDialog();

            //Set write mode.
            if (saveFileDialog.FileName != "") {

                //Fix extension.
                if (Path.GetExtension(saveFileDialog.FileName) == "") {
                    saveFileDialog.FileName += ".bf" + extension.ToLower();
                }

            }

            //Return the file name.
            return saveFileDialog.FileName;

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

    }
}