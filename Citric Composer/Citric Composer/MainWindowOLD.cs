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
    public partial class MainWindowOLD : Form
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
        public bool WritingInfo;

        /// <summary>
        /// Citric path.
        /// </summary>
        public string CitricPath = Application.StartupPath;

        public MainWindowOLD() {
            InitializeComponent();
            noInfoPanel.Show();
            noInfoPanel.BringToFront();
            exportToSDKProjectToolStripMenuItem.Image = Properties.Resources.NxTool;
        }

        public MainWindowOLD(string fileToOpen) {
            InitializeComponent();

            //Make a new file, and load it.
            File = SoundArchiveReader.ReadSoundArchive(fileToOpen);
            FileOpen = true;
            FilePath = fileToOpen;
            Text = "Citric Composer - " + Path.GetFileName(fileToOpen);
            noInfoPanel.Show();
            noInfoPanel.BringToFront();
            exportToSDKProjectToolStripMenuItem.Image = Properties.Resources.NxTool;

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
                switch (File.WriteMode) {
                    case WriteMode.Cafe:
                        exportToSDKProjectToolStripMenuItem.Image = Properties.Resources.CafeTool;
                        break;
                    case WriteMode.CTR:
                        exportToSDKProjectToolStripMenuItem.Image = Properties.Resources.CtrTool;
                        break;
                    case WriteMode.NX:
                        exportToSDKProjectToolStripMenuItem.Image = Properties.Resources.NxTool;
                        break;
                }

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
            //IsabelleSoundEditor a = new IsabelleSoundEditor();
            //a.Show();
        }


        private void brewstersArchiveBrewerWARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Brewster window.
            //Brewster_WAR_Brewer a = new Brewster_WAR_Brewer(this);
            //a.Show();
        }

        private void goldisGrouperGRPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Goldi window.
            //Goldi_GRP_Grouper a = new Goldi_GRP_Grouper(this);
            //a.Show();
        }

        private void wolfangsWriterWSDToolStripMenuItem_Click(object sender, EventArgs e) {

            //Wolfgang window.
            //Wolfgang_WSD_Writer w = new Wolfgang_WSD_Writer(this);
            //w.Show();

        }

        private void sSSSequencerSEQToolStripMenuItem_Click(object sender, EventArgs e) {

            //Sequencer.
            //SSS_Sequencer s = new SSS_Sequencer(this);
            //s.Show();

        }

        #endregion


        //Info panel stuff.
        #region InfoPanelStuff

        /// <summary>
        /// Do the mario!
        /// </summary>
        public void DoInfoStuff() {

            //Editing.
            WritingInfo = true;

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

                //Versions.
                else if (tree.Nodes["versions"] == tree.SelectedNode) {

                    //Show version info.
                    versionPanel.BringToFront();
                    versionPanel.Show();

                    //Update boxes.
                    if (File.WriteMode == WriteMode.CTR || File.WriteMode == WriteMode.NX) {
                        byteOrderBox.SelectedIndex = 0;
                    } else {
                        byteOrderBox.SelectedIndex = 1;
                    }

                    //Version.
                    versionMax.Value = File.Version.Major;
                    versionMin.Value = File.Version.Minor;
                    versionRev.Value = File.Version.Revision;

                    //Get versions.
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as SoundSequence;
                                if (z != null) {
                                    seqVersionMax.Value = z.Version.Major;
                                    seqVersionMin.Value = z.Version.Minor;
                                    seqVersionRev.Value = z.Version.Revision;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as SoundBank;
                                if (z != null) {
                                    bankVersionMax.Value = z.Version.Major;
                                    bankVersionMin.Value = z.Version.Minor;
                                    bankVersionRev.Value = z.Version.Revision;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as SoundWaveArchive;
                                if (z != null) {
                                    warVersionMax.Value = z.Version.Major;
                                    warVersionMin.Value = z.Version.Minor;
                                    warVersionRev.Value = z.Version.Revision;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as WaveSoundData;
                                if (z != null) {
                                    wsdVersionMax.Value = z.Version.Major;
                                    wsdVersionMin.Value = z.Version.Minor;
                                    wsdVersionRev.Value = z.Version.Revision;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as Group;
                                if (z != null) {
                                    grpVersionMax.Value = z.Version.Major;
                                    grpVersionMin.Value = z.Version.Minor;
                                    grpVersionRev.Value = z.Version.Revision;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as CitraFileLoader.Stream;
                                if (z != null) {
                                    stmVersionMax.Value = z.Stm.fileHeader.vMajor;
                                    stmVersionMin.Value = z.Stm.fileHeader.vMinor;
                                    stmVersionRev.Value = z.Stm.fileHeader.vRevision;
                                    break;
                                }
                            } else if (f.ExternalFileName != null) {
                                var z = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(Path.GetDirectoryName(FilePath) + "/" + f.ExternalFileName));
                                stmVersionMax.Value = z.Stm.fileHeader.vMajor;
                                stmVersionMin.Value = z.Stm.fileHeader.vMinor;
                                stmVersionRev.Value = z.Stm.fileHeader.vRevision;
                                break;
                            }
                        }
                    }
                    foreach (var f in File.Files) {
                        if (f != null) {
                            if (f.File != null) {
                                var z = f.File as PrefetchFile;
                                if (z != null) {
                                    stpVersionMax.Value = z.Version.Major;
                                    stpVersionMin.Value = z.Version.Minor;
                                    stpVersionRev.Value = z.Version.Revision;
                                    break;
                                }
                            }
                        }
                    }

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
                            //soundPlayerDeluxePanel.BringToFront();
                            //soundPlayerDeluxePanel.Show();

                            //3d info enabled.
                            seqSound3dInfoExists.Checked = seqEditSound3dInfoButton.Enabled = File.Sequences[ind].Sound3dInfo != null;

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

                    //Wave sound data.
                    else if (tree.Nodes["waveSoundSets"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.WaveSoundDatas[ind] != null) {

                            //Show panel.
                            ShowPanelWithFileInfo(wsdPanel, File.WaveSoundDatas[ind].File);
                            //soundPlayerDeluxePanel.BringToFront();
                            //soundPlayerDeluxePanel.Show();

                            //3d info enabled.
                            if (File.WaveSoundDatas[ind] != null) {
                                wsdSound3dEnable.Checked = wsdSound3dButton.Enabled = File.WaveSoundDatas[ind].Sound3dInfo != null;
                            }

                            //Set data.
                            wsdWaveIndex.Value = File.WaveSoundDatas[ind].WaveIndex;
                            wsdTracksToAllocate.Value = File.WaveSoundDatas[ind].AllocateTrackCount;
                            wsdChannelPriority.Value = File.WaveSoundDatas[ind].ChannelPriority;
                            wsdFixPriority.Checked = File.WaveSoundDatas[ind].IsReleasePriority;

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Streams.
                    else if (tree.Nodes["streams"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Streams[ind] != null) {

                            //Show panel.
                            ShowPanelWithFileInfo(stmPanel, File.Streams[ind].File);
                            //soundPlayerDeluxePanel.BringToFront();
                            //soundPlayerDeluxePanel.Show();

                        } else {
                            nullinfoPanel.Show();
                            nullinfoPanel.BringToFront();
                        }

                    }

                    //Sound groups.
                    else if (tree.Nodes["soundGroups"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.SoundSets[ind] != null) {

                            //Show panel.
                            soundGrpPanel.BringToFront();
                            soundGrpPanel.Show();
                            soundPlayerDeluxePanel.SendToBack();
                            soundPlayerDeluxePanel.Hide();

                            //If the entry is a WSD.
                            bool wsd = File.SoundSets[ind].StartIndex >= File.Streams.Count && File.SoundSets[ind].StartIndex < File.Streams.Count + File.WaveSoundDatas.Count;

                            //Switch type.
                            if (File.SoundSets[ind].SoundType == SoundType.Null || File.SoundSets[ind].StartIndex == 0xFFFFFF) {
                                soundGrpSoundType.SelectedIndex = 0;
                            } else if (!wsd) {
                                soundGrpSoundType.SelectedIndex = 1;
                            } else {
                                soundGrpSoundType.SelectedIndex = 2;
                            }

                            //Start index.
                            soundGrpStartIndex.Value = File.SoundSets[ind].StartIndex;

                            //End index.
                            soundGrpEndIndex.Value = File.SoundSets[ind].EndIndex;

                            //Show files.
                            soundGrpFilesGrid.Show();
                            soundGrpFilesGrid.Rows.Clear();
                            ((DataGridViewComboBoxColumn)soundGrpFilesGrid.Columns[0]).Items.Clear();
                            for (int i = 0; i < File.Files.Count; i++) {

                                //Name of wave archive.
                                string name = "{ Null File or Unknown Name }";
                                try { name = File.Files[i].FileName + "." + File.Files[i].FileExtension; } catch { }
                                if (name == null) { name = "{ Null File or Unknown Name }"; }
                                if (name.Equals("{ Null File or Unknown Name }")) { name = "{ Null Files or Unknown Name }"; }

                                //Add to list.
                                ((DataGridViewComboBoxColumn)soundGrpFilesGrid.Columns[0]).Items.Add(i + " - " + name);

                            }

                            //Add selected files.
                            foreach (var e in File.SoundSets[ind].Files) {
                                soundGrpFilesGrid.Rows.Add(new DataGridViewRow());
                                int valNum = 0;
                                if (e != null) {
                                    valNum = File.Files.IndexOf(e);
                                }
                                var h = ((DataGridViewComboBoxCell)soundGrpFilesGrid.Rows[soundGrpFilesGrid.Rows.Count - 2].Cells[0]);
                                h.Value = h.Items[valNum];
                            }

                            //Wave archives.
                            if (wsd) {

                                //Show wave archives.
                                soundGrpWaveArchives.Show();
                                soundGrpGridTable.RowStyles[0].Height = 50F;
                                soundGrpGridTable.RowStyles[1].Height = 50F;
                                soundGrpWaveArchives.Rows.Clear();
                                ((DataGridViewComboBoxColumn)soundGrpWaveArchives.Columns[0]).Items.Clear();
                                ((DataGridViewComboBoxColumn)soundGrpWaveArchives.Columns[0]).Items.Add("{ Null Wave Archive }");
                                for (int i = 0; i < File.WaveArchives.Count; i++) {

                                    //Name of wave archive.
                                    string name = "{ Null Archive or Unknown Name }";
                                    try { name = File.WaveArchives[i].Name; } catch { }
                                    if (name == null) { name = "{ Null Archive or Unknown Name }"; }
                                    if (name.Equals("{ Null Archive or Unknown Name }")) { name = "{ Null Archive or Unknown Name }"; }

                                    //Add to list.
                                    ((DataGridViewComboBoxColumn)soundGrpWaveArchives.Columns[0]).Items.Add(i + " - " + name);

                                }

                                //Add selected wave archives.
                                foreach (var e in File.SoundSets[ind].WaveArchives) {
                                    soundGrpWaveArchives.Rows.Add(new DataGridViewRow());
                                    int valNum = 0;
                                    if (e != null) {
                                        valNum = File.WaveArchives.IndexOf(e) + 1;
                                    }
                                    var h = ((DataGridViewComboBoxCell)soundGrpWaveArchives.Rows[soundGrpWaveArchives.Rows.Count - 2].Cells[0]);
                                    h.Value = h.Items[valNum];
                                }

                            } else {
                                soundGrpWaveArchives.Hide();
                                soundGrpGridTable.RowStyles[0].Height = 100F;
                                soundGrpGridTable.RowStyles[1].Height = 0F;
                            }

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
                            soundPlayerDeluxePanel.SendToBack();
                            soundPlayerDeluxePanel.Hide();

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
                            soundPlayerDeluxePanel.SendToBack();
                            soundPlayerDeluxePanel.Hide();

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
                            soundPlayerDeluxePanel.SendToBack();
                            soundPlayerDeluxePanel.Hide();

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
                            soundPlayerDeluxePanel.SendToBack();
                            soundPlayerDeluxePanel.Hide();

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

                        //Hide.
                        soundPlayerDeluxePanel.SendToBack();
                        soundPlayerDeluxePanel.Hide();

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
                        soundPlayerDeluxePanel.SendToBack();
                        soundPlayerDeluxePanel.Hide();

                    }

                }

                //Other.
                else if (tree.Nodes["projectInfo"] != tree.SelectedNode && tree.Nodes["versions"] != tree.SelectedNode) {

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
            WritingInfo = false;

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
            if (FileOpen && tree.SelectedNode != null && !WritingInfo) {

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

            if (!WritingInfo && FileOpen) {

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

            if (!WritingInfo && FileOpen) {
                File.WaveArchives[tree.SelectedNode.Index].LoadIndividually = warLoadIndividuallyBox.Checked;
            }

        }

        /// <summary>
        /// War flag changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void warIncludeWaveCountBox_CheckedChanged(object sender, EventArgs e) {

            if (!WritingInfo && FileOpen) {
                File.WaveArchives[tree.SelectedNode.Index].IncludeWaveCount = warIncludeWaveCountBox.Checked;
            }

        }

        /// <summary>
        /// Player sound limit changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerSoundLimitBox_ValueChanged(object sender, EventArgs e) {

            if (!WritingInfo && FileOpen) {
                File.Players[tree.SelectedNode.Index].SoundLimit = (int)playerSoundLimitBox.Value;
            }

        }

        /// <summary>
        /// Player enable sound heap.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerEnableSoundLimitBox_CheckedChanged(object sender, EventArgs e) {

            if (!WritingInfo && FileOpen) {
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

            if (!WritingInfo && FileOpen && playerHeapSizeBox.Enabled) {
                File.Players[tree.SelectedNode.Index].PlayerHeapSize = (int)playerHeapSizeBox.Value;
            }

        }

        /// <summary>
        /// Change file type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileTypeBox_SelectedIndexChanged(object sender, EventArgs e) {

            if (!WritingInfo && FileOpen) {
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
                        //b = new Brewster_WAR_Brewer(File.WaveArchives[tree.SelectedNode.Index].File, this);

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
                        //b = new Goldi_GRP_Grouper(File.Groups[tree.SelectedNode.Index].File, this);

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
                        //b = new Wolfgang_WSD_Writer(File.WaveSoundDatas[tree.SelectedNode.Index].File, this);

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
                        //b = new SSS_Sequencer(File.Sequences[tree.SelectedNode.Index].File, this);

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
                                //b = new Brewster_WAR_Brewer(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Sequence.
                            case "seq":
                                //b = new SSS_Sequencer(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Group.
                            case "grp":
                                //b = new Goldi_GRP_Grouper(File.Files[tree.SelectedNode.Index], this);
                                break;

                            //Wave sound data.
                            case "wsd":
                                //b = new Wolfgang_WSD_Writer(File.Files[tree.SelectedNode.Index], this);
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
                                        //IsabelleSoundEditor i = new IsabelleSoundEditor(this, tree.SelectedNode.Index, name + File.Files[tree.SelectedNode.Index].FileExtension, false);
                                        //i.Show();

                                    }

                                } else {

                                    //Open isabelle.
                                    //IsabelleSoundEditor i = new IsabelleSoundEditor(Path.GetDirectoryName(FilePath) + "/" + File.Files[tree.SelectedNode.Index].ExternalFileName);
                                    //i.Show();
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

            //Get write mode.
            if (FilePath.ToLower().EndsWith("csar")) {
                if (File.WriteMode == WriteMode.Cafe || File.WriteMode == WriteMode.C_BE) {
                    File.WriteMode = WriteMode.C_BE;
                } else {
                    File.WriteMode = WriteMode.CTR;
                }
            } else {
                if (File.WriteMode == WriteMode.Cafe || File.WriteMode == WriteMode.C_BE) {
                    File.WriteMode = WriteMode.Cafe;
                } else {
                    File.WriteMode = WriteMode.NX;
                }
            }

            //Save the file.
            SoundArchiveWriter.WriteSoundArchive(File, Path.GetDirectoryName(FilePath), FilePath);
            //MessageBox.Show("Saving has been disabled due to instability to protect your files. Please click the help button to join Gota's Sound Tools server to get a dev build that may support saving for your game.");

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

        private void ExportToSDKProjectToolStripMenuItem_Click(object sender, EventArgs e) {

            //File is open.
            if (FileOpen) {

                SaveFileDialog s = new SaveFileDialog();
                s.RestoreDirectory = true;
                switch (File.WriteMode) {
                    case WriteMode.CTR:
                        s.Filter = "CTR Project|*.cspj";
                        break;                      
                    case WriteMode.Cafe:
                        s.Filter = "Cafe Project|*.fspj";
                        break;
                    case WriteMode.NX:
                        s.Filter = "NX Project|*.fspj";
                        break;
                }
                s.ShowDialog();
                if (s.FileName != "") {
                    File.WriteSDKProject(s.FileName);
                }

            }

        }
        
        //Version updating.
        #region VersionUpdating

        private void ByteOrderBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (File.WriteMode == WriteMode.Cafe || File.WriteMode == WriteMode.NX) {
                if (byteOrderBox.SelectedIndex == 0) {
                    File.WriteMode = WriteMode.NX;
                } else {
                    File.WriteMode = WriteMode.Cafe;
                }
            } else {
                if (byteOrderBox.SelectedIndex == 0) {
                    File.WriteMode = WriteMode.CTR;
                } else {
                    File.WriteMode = WriteMode.C_BE;
                }
            }
        }

        private void VersionMax_ValueChanged(object sender, EventArgs e) {
            File.Version.Major = (byte)versionMax.Value;
        }

        private void VersionMin_ValueChanged(object sender, EventArgs e) {
            File.Version.Minor = (byte)versionMin.Value;
        }

        private void VersionRev_ValueChanged(object sender, EventArgs e) {
            File.Version.Revision = (byte)versionRev.Value;
        }

        private void SeqVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as SoundSequence;
                        z.Version.Major = (byte)seqVersionMax.Value;
                        z.Version.Minor = (byte)seqVersionMin.Value;
                        z.Version.Revision = (byte)seqVersionRev.Value;
                    }
                }
            }
        }

        private void BankVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as SoundBank;
                        z.Version.Major = (byte)bankVersionMax.Value;
                        z.Version.Minor = (byte)bankVersionMin.Value;
                        z.Version.Revision = (byte)bankVersionRev.Value;
                    }
                }
            }
        }

        private void WarVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as SoundWaveArchive;
                        z.Version.Major = (byte)warVersionMax.Value;
                        z.Version.Minor = (byte)warVersionMin.Value;
                        z.Version.Revision = (byte)warVersionRev.Value;
                    }
                }
            }
        }

        private void WsdVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as WaveSoundData;
                        z.Version.Major = (byte)wsdVersionMax.Value;
                        z.Version.Minor = (byte)wsdVersionMin.Value;
                        z.Version.Revision = (byte)wsdVersionRev.Value;
                    }
                }
            }
        }

        private void GrpVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as Group;
                        z.Version.Major = (byte)grpVersionMax.Value;
                        z.Version.Minor = (byte)grpVersionMin.Value;
                        z.Version.Revision = (byte)grpVersionRev.Value;
                    }
                }
            }
        }

        private void StmVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as CitraFileLoader.Stream;
                        z.Stm.fileHeader.vMajor = (byte)stmVersionMax.Value;
                        z.Stm.fileHeader.vMinor = (byte)stmVersionMin.Value;
                        z.Stm.fileHeader.vRevision = (byte)stmVersionRev.Value;
                    } else if (File.Files[i].ExternalFileName != null) {
                        var z = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(Path.GetDirectoryName(FilePath) + "/" + File.Files[i].ExternalFileName));
                        z.Stm.fileHeader.vMajor = (byte)stmVersionMax.Value;
                        z.Stm.fileHeader.vMinor = (byte)stmVersionMin.Value;
                        z.Stm.fileHeader.vRevision = (byte)stmVersionRev.Value;
                        System.IO.File.WriteAllBytes(Path.GetDirectoryName(FilePath) + "/" + File.Files[i].ExternalFileName, SoundArchiveWriter.WriteFile(z));
                    }
                }
            }
        }

        private void StpVersionUpdate_Click(object sender, EventArgs e) {
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i] != null) {
                    if (File.Files[i].File != null) {
                        var z = File.Files[i].File as PrefetchFile;
                        z.Version.Major = (byte)stpVersionMax.Value;
                        z.Version.Minor = (byte)stpVersionMin.Value;
                        z.Version.Revision = (byte)stpVersionRev.Value;
                    }
                }
            }
        }

        #endregion

        //Project info.
        #region ProjectInfo

        private void MaxSeqNumBox_ValueChanged(object sender, EventArgs e) {
            File.MaxSequences = (ushort)maxSeqNumBox.Value;
        }

        private void MaxSeqTrackNumBox_ValueChanged(object sender, EventArgs e) {
            File.MaxSequenceTracks = (ushort)maxSeqTrackNumBox.Value;
        }

        private void MaxStreamNumBox_ValueChanged(object sender, EventArgs e) {
            File.MaxStreamSounds = (ushort)maxStreamNumBox.Value;
        }

        private void MaxStreamNumTracksBox_ValueChanged(object sender, EventArgs e) {
            File.MaxStreamTracks = (ushort)maxStreamNumTracksBox.Value;
        }

        private void MaxStreamNumChannelsBox_ValueChanged(object sender, EventArgs e) {
            File.MaxStreamChannels = (ushort)maxStreamNumChannelsBox.Value;
        }

        private void MaxWaveNumBox_ValueChanged(object sender, EventArgs e) {
            File.MaxWaveSounds = (ushort)maxWaveNumBox.Value;
        }

        private void MaxWaveNumTracksBox_ValueChanged(object sender, EventArgs e) {
            File.MaxWaveTracks = (ushort)maxWaveNumTracksBox.Value;
        }

        private void StreamBufferTimesBox_ValueChanged(object sender, EventArgs e) {
            File.StreamBufferTimes = (byte)streamBufferTimesBox.Value;
        }

        private void OptionsPIBox_ValueChanged(object sender, EventArgs e) {
            File.Options = (uint)optionsPIBox.Value;
        }

        #endregion

        //Sound group info.
        #region SoundGroupInfo

        private void SoundGrpStartIndex_ValueChanged(object sender, EventArgs e) {
            if (soundGrpStartIndex.Value > soundGrpEndIndex.Value) {
                soundGrpStartIndex.Value = soundGrpEndIndex.Value;
            }
            if (!WritingInfo) {
                int ind = tree.SelectedNode.Index;
                File.SoundSets[tree.SelectedNode.Index].StartIndex = (int)soundGrpStartIndex.Value;
                bool wsd = File.SoundSets[ind].StartIndex >= File.Streams.Count && File.SoundSets[ind].StartIndex < File.Streams.Count + File.WaveSoundDatas.Count;
                if (wsd) {
                    if (File.SoundSets[ind].WaveArchives == null) { File.SoundSets[ind].WaveArchives = new List<WaveArchiveEntry>(); }
                } else {
                    File.SoundSets[ind].WaveArchives = null;
                }
                UpdateNodes();
            }
        }

        private void SoundGrpEndIndex_ValueChanged(object sender, EventArgs e) {
            if (soundGrpEndIndex.Value < soundGrpStartIndex.Value) {
                soundGrpEndIndex.Value = soundGrpStartIndex.Value;
            }
            if (!WritingInfo) {
                File.SoundSets[tree.SelectedNode.Index].EndIndex = (int)soundGrpEndIndex.Value;
                UpdateNodes();
            }
        }

        /// <summary>
        /// File cell changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundGrpFileGridCellChanged(object sender, EventArgs e) {

            if (!WritingInfo && FileOpen) {

                File.SoundSets[tree.SelectedNode.Index].Files = new List<SoundFile<ISoundFile>>();
                for (int i = 1; i < soundGrpFilesGrid.Rows.Count; i++) {
                    int ind = ((DataGridViewComboBoxCell)soundGrpFilesGrid.Rows[i - 1].Cells[0]).Items.IndexOf(((DataGridViewComboBoxCell)soundGrpFilesGrid.Rows[i - 1].Cells[0]).Value);
                    if (ind == 0) {
                        File.SoundSets[tree.SelectedNode.Index].Files.Add(null);
                    } else {
                        File.SoundSets[tree.SelectedNode.Index].Files.Add(File.Files[ind]);
                    }
                }

            }

        }

        /// <summary>
        /// Wave archives cell changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundGrpWarGridCellChanged(object sender, EventArgs e) {

            if (!WritingInfo && FileOpen) {

                File.SoundSets[tree.SelectedNode.Index].WaveArchives = new List<WaveArchiveEntry>();
                for (int i = 1; i < soundGrpWaveArchives.Rows.Count; i++) {
                    int ind = ((DataGridViewComboBoxCell)soundGrpWaveArchives.Rows[i - 1].Cells[0]).Items.IndexOf(((DataGridViewComboBoxCell)soundGrpWaveArchives.Rows[i - 1].Cells[0]).Value);
                    if (ind == 0) {
                        File.SoundSets[tree.SelectedNode.Index].WaveArchives.Add(null);
                    } else {
                        File.SoundSets[tree.SelectedNode.Index].WaveArchives.Add(File.WaveArchives[ind - 1]);
                    }
                }

            }

        }

        #endregion

        //Sequence info.
        #region SequenceInfo

        private void SeqEditSoundInfoButton_Click(object sender, EventArgs e) {

        }

        private void SeqSound3dInfoExists_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                seqEditSound3dInfoButton.Enabled = seqSound3dInfoExists.Checked;
                if (seqSound3dInfoExists.Checked) {
                    File.Sequences[tree.SelectedNode.Index].Sound3dInfo = new Sound3dInfo();
                } else {
                    File.Sequences[tree.SelectedNode.Index].Sound3dInfo = null;
                }
            }
        }

        private void SeqEditSound3dInfoButton_Click(object sender, EventArgs e) {

        }

        private void SeqBank0Box_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void SeqBank1Box_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void SeqBank2Box_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void SeqBank3Box_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void SeqOffsetFromLabelButton_CheckedChanged(object sender, EventArgs e) {

        }

        private void SeqOffsetManualButton_CheckedChanged(object sender, EventArgs e) {

        }

        private void SeqOffsetFromLabelBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void SeqOffsetManualBox_ValueChanged(object sender, EventArgs e) {

        }

        private void SeqChannelPriorityBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].ChannelPriority = (byte)seqChannelPriorityBox.Value;
            }
        }

        private void SeqIsReleasePriorityBox_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].IsReleasePriority = seqIsReleasePriorityBox.Checked;
            }
        }

        private void SeqC0_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(0, seqC0.Checked);
            }
        }

        private void SeqC1_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(1, seqC1.Checked);
            }
        }

        private void SeqC2_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(2, seqC2.Checked);
            }
        }

        private void SeqC3_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(3, seqC3.Checked);
            }
        }

        private void SeqC4_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(4, seqC4.Checked);
            }
        }

        private void SeqC5_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(5, seqC5.Checked);
            }
        }

        private void SeqC6_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(6, seqC6.Checked);
            }
        }

        private void SeqC7_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(7, seqC7.Checked);
            }
        }

        private void SeqC8_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(8, seqC8.Checked);
            }
        }

        private void SeqC9_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(9, seqC9.Checked);
            }
        }

        private void SeqC10_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(10, seqC10.Checked);
            }
        }

        private void SeqC11_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(11, seqC11.Checked);
            }
        }

        private void SeqC12_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(12, seqC12.Checked);
            }
        }

        private void SeqC13_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(13, seqC13.Checked);
            }
        }

        private void SeqC14_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(14, seqC14.Checked);
            }
        }

        private void SeqC15_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(15, seqC15.Checked);
            }
        }

        #endregion

        //Wave sound data info.
        #region WaveSoundDataInfo

        private void WsdEditSoundInfoButton_Click(object sender, EventArgs e) {

        }

        private void WsdSound3dEnable_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                wsdSound3dButton.Enabled = wsdSound3dEnable.Checked;
                if (wsdSound3dEnable.Checked) {
                    File.WaveSoundDatas[tree.SelectedNode.Index].Sound3dInfo = new Sound3dInfo();
                } else {
                    File.WaveSoundDatas[tree.SelectedNode.Index].Sound3dInfo = null;
                }
            }
        }

        private void WsdSound3dButton_Click(object sender, EventArgs e) {

        }

        private void WsdWaveIndex_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.WaveSoundDatas[tree.SelectedNode.Index].WaveIndex = (byte)wsdWaveIndex.Value;
            }
        }

        private void WsdTracksToAllocate_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.WaveSoundDatas[tree.SelectedNode.Index].AllocateTrackCount = (byte)wsdTracksToAllocate.Value;
            }
        }

        private void WsdCopyCount_Click(object sender, EventArgs e) {
            if (!WritingInfo) {
                try {
                    int wsdNum = File.WaveSoundDatas[tree.SelectedNode.Index].WaveIndex;
                    WaveSoundData wsd = File.WaveSoundDatas[tree.SelectedNode.Index].File.File as WaveSoundData;
                    var wave = wsd.Waves[wsd.DataItems[wsdNum].Notes[0].WaveIndex];
                    Wave w = (File.WaveArchives[wave.WarIndex].File.File as SoundWaveArchive)[wave.WaveIndex];
                    int numTracks = 0;
                    if (w.Wav.info.channelInfo != null) {
                        numTracks = w.Wav.info.channelInfo.Count / 2;
                        if (numTracks == 0) { numTracks++; }
                        if (w.Wav.info.channelInfo.Count == 0) { numTracks = 0; }
                    }
                    File.WaveSoundDatas[tree.SelectedNode.Index].AllocateTrackCount = numTracks;
                    wsdTracksToAllocate.Value = numTracks;
                } catch { MessageBox.Show("Unable to read track information from corresponding wave file."); }
            }
        }

        private void WsdChannelPriority_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.WaveSoundDatas[tree.SelectedNode.Index].ChannelPriority = (byte)wsdChannelPriority.Value;
            }
        }

        private void WsdFixPriority_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo) {
                File.WaveSoundDatas[tree.SelectedNode.Index].IsReleasePriority = wsdFixPriority.Checked;
            }
        }

        #endregion

    }

}