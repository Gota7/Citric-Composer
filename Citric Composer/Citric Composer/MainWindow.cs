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
    /// Main window.
    /// </summary>
    public class MainWindow : EditorBase {

        /// <summary>
        /// The file.
        /// </summary>
        public new SoundArchive File;

        public MainWindow() : base(typeof(SoundArchive), "Sound Archive", "sar", "Citric Composer", null) {
            Text = "Citric Composer";
            Icon = Properties.Resources.Citric_Composer;
            toolsTabMainWindow.Visible = true;
            MainWindow = this;
            EntryPlayer.Initialize();
        }

        public MainWindow(string fileToOpen) : base(typeof(SoundArchive), "Sound Archive", "sar", "Citric Composer", fileToOpen, null) {
            SoundArchive.FilePath = fileToOpen;
            Text = "Citric Composer - " + Path.GetFileName(fileToOpen);
            Icon = Properties.Resources.Citric_Composer;
            toolsTabMainWindow.Visible = true;
            EntryPlayer.Initialize();
        }

        //New.
        public override void newToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open and can save test.
            if (!FileTest(sender, e, true)) {
                return;
            }

            //Create instance of file.
            File = new SoundArchive();

            //Reset values.
            FilePath = "";
            FileOpen = true;
            ExtFile = null;
            Text = EditorName + " - New " + ExtensionDescription + ".bf" + Extension;
            
            //Edit properties.
            File.Streams = new List<StreamEntry>();
            File.WaveSoundDatas = new List<WaveSoundDataEntry>();
            File.Sequences = new List<SequenceEntry>();
            File.SoundSets = new List<SoundSetEntry>();
            File.Banks = new List<BankEntry>();
            File.WaveArchives = new List<WaveArchiveEntry>();
            File.Groups = new List<GroupEntry>();
            File.Players = new List<PlayerEntry>();
            File.Files = new List<SoundFile<ISoundFile>>();
            File.CreateStrings = true;
            File.Version = new FileWriter.Version(2, 3, 0);

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        //Open.
        public override void openToolStripMenuItem_Click(object sender, EventArgs e) {

            //File open and save test.
            if (!FileTest(sender, e, true)) {
                return;
            }

            //Open the file.
            string path = GetFileOpenerPath(ExtensionDescription, Extension);

            //File is not null.
            if (path != "") {

                //Set value.
                File = (SoundArchive)Activator.CreateInstance(FileType);
                ExtFile = null;
                FilePath = path;
                Text = EditorName + " - " + Path.GetFileName(path);
                FileOpen = true;

                //Read data.
                File = SoundArchiveReader.ReadSoundArchive(path);
                EntryPlayer.BasePath = Path.GetDirectoryName(FilePath);

                //Update.
                UpdateNodes();
                DoInfoStuff();

            }

        }

        //Save.
        public override void saveToolStripMenuItem_Click(object sender, EventArgs e) {

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

            //Get write mode.
            if (FilePath.ToLower().EndsWith("csar")) {
                if (File.WriteMode == WriteMode.Cafe || File.WriteMode == WriteMode.C_BE) {
                    File.WriteMode = WriteMode.C_BE;
                    MessageBox.Show("There is no such thing as a big endian BCSAR... Double check your byte order.");
                    return;
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

            //Write the file.
            SoundArchiveWriter.WriteSoundArchive(File, Path.GetDirectoryName(FilePath), FilePath);

        }

        /// <summary>
        /// Do info stuff.
        /// </summary>
        public override void DoInfoStuff() {

            //Writing info.
            WritingInfo = true;

            //Call base.
            base.DoInfoStuff();

            //Index.
            int ind = tree.SelectedNode.Index;

            //Safety check.
            if (!FileOpen || File == null) {
                return;
            }

            //A node.
            if (tree.SelectedNode.Parent == null) {

                //Project info.
                if (tree.SelectedNode.Index == 0) {

                    //Show panel.
                    sarProjectInfoPanel.BringToFront();
                    sarProjectInfoPanel.Show();

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
                    sarIncludeStringBlock.Checked = File.CreateStrings;

                    //Status.
                    status.Text = "File Info Selected.";

                }

                //Version info.
                else if (tree.SelectedNode == tree.Nodes["versions"]) {

                    //Show version info.
                    versionPanel.BringToFront();
                    versionPanel.Show();

                    //Status.
                    status.Text = "Version Info Selected.";

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

                //No info.
                else {
                    noInfoPanel.BringToFront();
                    noInfoPanel.Show();
                }

            }

            //A sound entry.
            else {

                //Sound entry.
                if (tree.SelectedNode.Parent.Parent == null) {

                    //Streams.
                    if (tree.Nodes["streams"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Streams[ind] != null) {

                            //Show panel.
                            stmPanel.Show();
                            stmPanel.BringToFront();
                            PopulateFileList(stmFileIdBox, File.Streams[ind].File);

                            //3d info enabled.
                            stmSound3dEnable.Checked = stmSound3dButton.Enabled = File.Streams[ind].Sound3dInfo != null;

                            //Status.
                            status.Text = "Sound " + ind + " Selected.";
                            if (File.Streams[ind].File != null) {
                                if (File.Streams[ind].File.File != null) {
                                    status.Text += " (" + BytesToString(SoundArchiveWriter.WriteFile(File.Streams[ind].File.File).Length) + ")";
                                }
                            }

                            //Track info.
                            stmUpdateTrackInfo.Enabled = stmWriteTrackInfo.Checked = File.Streams[ind].Tracks != null;

                            //Track flags.
                            stmTrack0.Checked = File.Streams[ind].TrackFlagEnabled(0);
                            stmTrack1.Checked = File.Streams[ind].TrackFlagEnabled(1);
                            stmTrack2.Checked = File.Streams[ind].TrackFlagEnabled(2);
                            stmTrack3.Checked = File.Streams[ind].TrackFlagEnabled(3);
                            stmTrack4.Checked = File.Streams[ind].TrackFlagEnabled(4);
                            stmTrack5.Checked = File.Streams[ind].TrackFlagEnabled(5);
                            stmTrack6.Checked = File.Streams[ind].TrackFlagEnabled(6);
                            stmTrack7.Checked = File.Streams[ind].TrackFlagEnabled(7);
                            stmTrack8.Checked = File.Streams[ind].TrackFlagEnabled(8);
                            stmTrack9.Checked = File.Streams[ind].TrackFlagEnabled(9);
                            stmTrack10.Checked = File.Streams[ind].TrackFlagEnabled(10);
                            stmTrack11.Checked = File.Streams[ind].TrackFlagEnabled(11);
                            stmTrack12.Checked = File.Streams[ind].TrackFlagEnabled(12);
                            stmTrack13.Checked = File.Streams[ind].TrackFlagEnabled(13);
                            stmTrack14.Checked = File.Streams[ind].TrackFlagEnabled(14);
                            stmTrack15.Checked = File.Streams[ind].TrackFlagEnabled(15);

                            //Other info.
                            stmStreamType.SelectedIndex = (int)File.Streams[ind].StreamFileType;
                            stmAllocateChannelsNum.Value = File.Streams[ind].AllocateChannelCount;
                            stmPitch.Value = (decimal)File.Streams[ind].Pitch;
                            stmSendMain.Value = File.Streams[ind].SendValue[0];
                            stmSendA.Value = File.Streams[ind].SendValue[1];
                            stmSendB.Value = File.Streams[ind].SendValue[2];
                            stmSendC.Value = File.Streams[ind].SendValue[3];

                            //Extended data.
                            stmIncludeExtension.Checked = stmLoopStartFrame.Enabled = stmLoopEndFrame.Enabled = stmCopyExtensionFromFile.Enabled = File.Streams[ind].SoundExtensionIncluded;
                            if (File.Streams[ind].SoundExtensionIncluded) {
                                stmLoopStartFrame.Value = File.Streams[ind].LoopStartFrame;
                                stmLoopEndFrame.Value = File.Streams[ind].LoopEndFrame;
                            } else {
                                stmLoopStartFrame.Value = 0;
                                stmLoopEndFrame.Value = 0;
                            }

                            //Prefetch file.
                            stmGeneratePrefetch.Checked = stmPrefetchFileIdBox.Enabled = stmUpdatePrefetchInfo.Enabled = stmCreateUniquePrefetchFile.Enabled = File.Streams[ind].GeneratePrefetchFile;
                            if (File.Streams[ind].GeneratePrefetchFile) {
                                stmCreateUniquePrefetchFile.Enabled = !File.FileUnique(File.Files.IndexOf(File.Streams[ind].PrefetchFile));
                                PopulateFileList(stmPrefetchFileIdBox, File.Streams[ind].PrefetchFile);
                            } else {
                                PopulateFileList(stmPrefetchFileIdBox, null);
                            }

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Sound " + ind + " Selected.";
                        }

                    }

                    //Wave sound data.
                    else if (tree.Nodes["waveSoundDatas"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.WaveSoundDatas[ind] != null) {

                            //Show panel.
                            wsdPanel.Show();
                            wsdPanel.BringToFront();
                            PopulateFileList(sarWsdFileIdBox, File.WaveSoundDatas[ind].File);

                            //3d info enabled.
                            if (File.WaveSoundDatas[ind] != null) {
                                wsdSound3dEnable.Checked = wsdSound3dButton.Enabled = File.WaveSoundDatas[ind].Sound3dInfo != null;
                            }

                            //Set data.
                            wsdWaveIndex.Value = File.WaveSoundDatas[ind].WaveIndex;
                            wsdTracksToAllocate.Value = File.WaveSoundDatas[ind].AllocateTrackCount;
                            wsdChannelPriority.Value = File.WaveSoundDatas[ind].ChannelPriority;
                            wsdFixPriority.Checked = File.WaveSoundDatas[ind].IsReleasePriority;

                            //Status.
                            status.Text = "Sound " + (ind + File.Streams.Count) + " Selected.";
                            if (File.WaveSoundDatas[ind].File != null) {
                                if (File.WaveSoundDatas[ind].File.File != null) {
                                    status.Text += " (" + BytesToString(SoundArchiveWriter.WriteFile(File.WaveSoundDatas[ind].File.File).Length) + ")";
                                }
                            }

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Sound " + (ind + File.Streams.Count) + " Selected.";
                        }

                    }

                    //Sequences.
                    else if (tree.Nodes["sequences"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Sequences[ind] != null) {

                            //Show panel.
                            seqPanel.Show();
                            seqPanel.BringToFront();
                            PopulateFileList(sarSeqFileIdBox, File.Sequences[ind].File);

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

                            //Status.
                            status.Text = "Sound " + (ind + File.Streams.Count + File.WaveSoundDatas.Count) + " Selected.";
                            if (File.Sequences[ind].File != null) {
                                if (File.Sequences[ind].File.File != null) {
                                    status.Text += " (" + BytesToString(SoundArchiveWriter.WriteFile(File.Sequences[ind].File.File).Length) + ")";
                                }
                            }

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Sound " + (ind + File.Streams.Count + File.WaveSoundDatas.Count) + " Selected.";
                        }

                    }

                    //Sound groups.
                    else if (tree.Nodes["soundGroups"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.SoundSets[ind] != null) {

                            //Show panel.
                            soundGrpPanel.BringToFront();
                            soundGrpPanel.Show();

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

                            //Status.
                            status.Text = "Sound Group " + ind + " Selected.";

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Sound Group " + ind + " Selected.";
                        }

                    }

                    //Banks.
                    else if (tree.Nodes["banks"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Banks[ind] != null) {

                            //Show panel.
                            bankPanel.Show();
                            bankPanel.BringToFront();
                            PopulateFileList(sarBnkFileIdBox, File.Banks[ind].File);

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

                            //Status.
                            status.Text = "Bank " + ind + " Selected.";
                            if (File.Banks[ind].File != null) {
                                if (File.Banks[ind].File.File != null) {
                                    status.Text += " (" + BytesToString(SoundArchiveWriter.WriteFile(File.Banks[ind].File.File).Length) + ")";
                                }
                            }

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Bank " + ind + " Selected.";
                        }

                    }

                    //Wave archives.
                    else if (tree.Nodes["waveArchives"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.WaveArchives[ind] != null) {

                            //Show panel.
                            warInfoPanel.Show();
                            warInfoPanel.BringToFront();
                            PopulateFileList(sarWarFileIdBox, File.WaveArchives[ind].File);

                            //Checkboxes.
                            warLoadIndividuallyBox.Checked = File.WaveArchives[ind].LoadIndividually;
                            warIncludeWaveCountBox.Checked = File.WaveArchives[ind].IncludeWaveCount;

                            //Status.
                            status.Text = "Wave Archive " + ind + " Selected.";
                            if (File.WaveArchives[ind].File != null) {
                                if (File.WaveArchives[ind].File.File != null) {
                                    status.Text += " (" + BytesToString(SoundArchiveWriter.WriteFile(File.WaveArchives[ind].File.File).Length) + ")";
                                }
                            }

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Wave Archive " + ind + " Selected.";
                        }

                    }

                    //Groups.
                    else if (tree.Nodes["groups"] == tree.SelectedNode.Parent) {

                        //Valid.
                        if (File.Groups[ind] != null) {

                            //Show panel.
                            grpPanel.Show();
                            grpPanel.BringToFront();
                            PopulateFileList(sarGrpFileIdBox, File.Groups[ind].File);

                            //Status.
                            status.Text = "Group " + ind + " Selected.";
                            if (File.Groups[ind].File != null) {
                                if (File.Groups[ind].File.File != null) {
                                    status.Text += " (" + BytesToString(SoundArchiveWriter.WriteFile(File.Groups[ind].File.File).Length) + ")";
                                }
                            }

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Group " + ind + " Selected.";
                        }

                    }

                    //Player.
                    else if (tree.SelectedNode.Parent == tree.Nodes["players"]) {

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

                            //Status.
                            status.Text = "Player " + ind + " Selected.";

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } Player " + ind + " Selected.";
                        }

                    }

                    //Files.
                    else if (tree.SelectedNode.Parent == tree.Nodes["files"]) {

                        //Valid.
                        if (File.Files[ind] != null) {

                            //Show panel.
                            fileInfoPanel.Show();
                            fileInfoPanel.BringToFront();
                            soundPlayerDeluxePanel.SendToBack();
                            soundPlayerDeluxePanel.Hide();

                            //Combobox.
                            fileTypeBox.SelectedIndex = (int)File.Files[ind].FileType;

                            //Set info.
                            filesIncludeGroups.Checked = filesGroupGrid.Enabled = File.Files[ind].Groups != null;

                            //Show groups.
                            filesGroupGrid.Rows.Clear();
                            if (File.Files[ind].Groups != null) {                            
                                ((DataGridViewComboBoxColumn)filesGroupGrid.Columns[0]).Items.Clear();
                                for (int i = 0; i < File.Groups.Count; i++) {

                                    //Name of wave archive.
                                    string name = "{ Null Group or Unknown Name }";
                                    try { name = File.Groups[i].Name; } catch { }
                                    if (name == null) { name = "{ Null Group or Unknown Name }"; }
                                    if (name.Equals("{ Null Group or Unknown Name }")) { name = "{ Null Group or Unknown Name }"; }

                                    //Add to list.
                                    ((DataGridViewComboBoxColumn)filesGroupGrid.Columns[0]).Items.Add(i + " - " + name);

                                }

                                //Add selected groups.
                                foreach (var e in File.Files[ind].Groups) {
                                    filesGroupGrid.Rows.Add(new DataGridViewRow());
                                    int valNum = e;
                                    var h = ((DataGridViewComboBoxCell)filesGroupGrid.Rows[filesGroupGrid.Rows.Count - 2].Cells[0]);
                                    h.Value = h.Items[valNum];
                                }

                            }

                            //Status.
                            status.Text = "File " + ind + " Selected." + (File.Files[ind].File == null ? "" : (" (" + BytesToString(SoundArchiveWriter.WriteFile(File.Files[ind].File).Length) + ")"));

                        } else {
                            nullDataPanel.Show();
                            nullDataPanel.BringToFront();
                            status.Text = "{ Null } File " + ind + " Selected.";
                        }

                    }

                    //Other.
                    else {

                        //No info.
                        noInfoPanel.BringToFront();
                        noInfoPanel.Show();
                        status.Text = "No Valid Info Selected!";

                    }

                }

                //Special cases.
                else {

                    if (tree.Nodes["streams"] == tree.SelectedNode.Parent.Parent) {

                        //Show track info.
                        trackPanel.BringToFront();
                        trackPan.Show();

                        //Show track info.
                        StreamTrackInfo t = File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index];
                        trackVolume.Value = t.Volume;
                        trackPan.Value = t.Pan;
                        trackSpan.Value = t.Span;
                        trackSurround.Checked = t.SurroundMode;
                        trackLPFFrequency.Value = t.LpfFrequency;
                        trackBiquadType.SelectedIndex = t.BiquadType;
                        trackBiquadValue.Value = t.BiquadValue;
                        trackSendMain.Value = t.SendValue[0];
                        trackSendA.Value = t.SendValue[1];
                        trackSendB.Value = t.SendValue[2];
                        trackSendC.Value = t.SendValue[3];

                        //Grid.
                        trackChannelGrid.Rows.Clear();
                        foreach (var e in t.Channels) {
                            trackChannelGrid.Rows.Add(new DataGridViewRow());
                            var h = ((DataGridViewTextBoxCell)trackChannelGrid.Rows[trackChannelGrid.Rows.Count - 2].Cells[0]);
                            h.Value = e;
                        }

                        //Status.
                        status.Text = "Track " + tree.SelectedNode.Index + " Of Stream " + tree.SelectedNode.Parent.Index + " Selected.";

                    }

                    //No info.
                    else {
                        noInfoPanel.BringToFront();
                        noInfoPanel.Show();
                        status.Text = "No Valid Info Selected!";
                    }

                }

            }

            //End writing info.
            WritingInfo = false;

        }

        /// <summary>
        /// Node double clicked.
        /// </summary>
        public override void NodeMouseDoubleClick() {

            //Safety check.
            if (!FileOpen || File == null) {
                return;
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
                else if (tree.SelectedNode.Parent == tree.Nodes["waveSoundDatas"]) {

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
                        b = new Static_Sequencer(File.Sequences[tree.SelectedNode.Index].File, this);

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
                                b = new Static_Sequencer(File.Files[tree.SelectedNode.Index], this);
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

        /// <summary>
        /// Populate with the file list, and select the proper index.
        /// </summary>
        /// <param name="c">Combobox.</param>
        /// <param name="file">File to select.</param>
        public void PopulateFileList(ComboBox c, SoundFile<ISoundFile> file) {

            //Add files.
            c.Items.Clear();
            c.Items.Add("{ Null File }");
            for (int i = 0; i < File.Files.Count; i++) {
                string name = "{ Null File or Unknown Name }";
                try {
                    name = File.Files[i].FileName + "." + File.Files[i].FileExtension;
                } catch { }
                c.Items.Add(i + " - " + name);
            }
            if (file == null) {
                c.SelectedIndex = 0;
            } else {
                c.SelectedIndex = File.Files.IndexOf(file.Reference) + 1;
            }

        }

        /// <summary>
        /// Bytes to string.
        /// </summary>
        /// <param name="byteCount">Number of bytes.</param>
        /// <returns></returns>
        public static string BytesToString(long byteCount) {
            string[] suf = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        /// <summary>
        /// Update nodes.
        /// </summary>
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

            //Add waves if node doesn't exist.
            if (tree.Nodes.Count < 11) {
                tree.Nodes.Add("versions", "Version Editing", 12, 12);
                tree.Nodes.Add("streams", "Sound Streams", 1, 1);
                tree.Nodes.Add("waveSoundDatas", "Sound Wave Entries", 2, 2);
                tree.Nodes.Add("sequences", "Sound Sequences", 3, 3);
                tree.Nodes.Add("soundGroups", "Sound Groups", 4, 4);
                tree.Nodes.Add("banks", "Instrument Banks", 5, 5);
                tree.Nodes.Add("waveArchives", "Wave Archives", 6, 6);
                tree.Nodes.Add("groups", "Groups", 7, 7);
                tree.Nodes.Add("players", "Players", 8, 8);
                tree.Nodes.Add("files", "Files", 11, 11);
            }

            //File is open and not null.
            if (FileOpen && File != null) {

                //Set context menus.
                tree.Nodes["streams"].ContextMenuStrip = rootMenu;
                tree.Nodes["waveSoundDatas"].ContextMenuStrip = rootMenu;
                tree.Nodes["sequences"].ContextMenuStrip = rootMenu;
                tree.Nodes["soundGroups"].ContextMenuStrip = rootMenu;
                tree.Nodes["banks"].ContextMenuStrip = rootMenu;
                tree.Nodes["waveArchives"].ContextMenuStrip = rootMenu;
                tree.Nodes["groups"].ContextMenuStrip = rootMenu;
                tree.Nodes["players"].ContextMenuStrip = rootMenu;
                tree.Nodes["files"].ContextMenuStrip = CreateMenuStrip(rootMenu, new int[] { 1, 2 }, new EventHandler[] { null, new EventHandler(expandToolStripMenuItem_Click), new EventHandler(collapseToolStripMenuItem_Click) });
                
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

                        //Add tracks.
                        if (s.Tracks != null) {
                            int trackCount = 0;
                            foreach (var t in s.Tracks) {

                                //Add track.
                                tree.Nodes["streams"].Nodes["stream" + stmCount].Nodes.Add("track" + trackCount, "Track " + trackCount, 9, 9);

                                //Right click menu.
                                tree.Nodes["streams"].Nodes["stream" + stmCount].Nodes["track" + trackCount].ContextMenuStrip = CreateMenuStrip(sarEntryMenu, new int[] { 0, 1, 3, 4, 9 }, new EventHandler[] { SarAddAbove_Click, SarAddBelow_Click, null, SarMoveUp_Click, SarMoveDown_Click, null, null, null, null, SarDelete_Click });

                                //Increment count.
                                trackCount++;

                            }
                        }

                        //Right click menu.
                        tree.Nodes["streams"].Nodes["stream" + stmCount].ContextMenuStrip = CreateMenuStrip(sarEntryMenu, new int[] { 0, 1, 2, 3, 4, 7, 8, 9 }, new EventHandler[] { SarAddAbove_Click, SarAddBelow_Click, SarAddInside_Click, SarMoveUp_Click, SarMoveDown_Click, null, null, SarRename_Click, SarNullify_Click, SarDelete_Click });
                        tree.Nodes["streams"].Nodes["stream" + stmCount].ContextMenuStrip.Items[2].Visible = true;

                    }

                    //Increment count.
                    stmCount++;

                }

                //Load wave sound infos.
                int wsdCount = stmCount;
                foreach (var w in File.WaveSoundDatas) {

                    //Null.
                    if (w == null) {
                        tree.Nodes["waveSoundDatas"].Nodes.Add("waveSoundData" + wsdCount, "[" + wsdCount + "] " + "{ Null Wave Sound Set }", 2, 2);
                    }

                    //Valid.
                    else {
                        string name = w.Name;
                        if (name == null) {
                            name = "{ Null Name }";
                        }
                        tree.Nodes["waveSoundDatas"].Nodes.Add("waveSoundData" + wsdCount, "[" + wsdCount + "] " + name, 2, 2);

                        //Right click menu.
                        tree.Nodes["waveSoundDatas"].Nodes["waveSoundData" + wsdCount].ContextMenuStrip = sarEntryMenu;

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

                        //Right click menu.
                        tree.Nodes["sequences"].Nodes["sequence" + seqCount].ContextMenuStrip = sarEntryMenu;

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

                        //Right click menu.
                        tree.Nodes["banks"].Nodes["bank" + bCount].ContextMenuStrip = sarEntryMenu;

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

                        //Right click menu.
                        tree.Nodes["players"].Nodes["player" + pCount].ContextMenuStrip = CreateMenuStrip(sarEntryMenu, new int[] { 0, 1, 3, 4, 7, 8, 9 }, new EventHandler[] { SarAddAbove_Click, SarAddBelow_Click, null, SarMoveUp_Click, SarMoveDown_Click, null, null, SarRename_Click, SarNullify_Click, SarDelete_Click });

                    }

                    //Increment count.
                    pCount++;

                }

                //Load groups.
                int gCount = 0;
                foreach (var g in File.Groups) {

                    //Null.
                    if (g == null) {
                        tree.Nodes["groups"].Nodes.Add("group" + gCount, "[" + gCount + "] " + "{ Null Group }", 7, 7);
                    }

                    //Valid.
                    else {
                        string name = g.Name;
                        if (name == null) {
                            name = "{ Null Name }";
                        }
                        tree.Nodes["groups"].Nodes.Add("group" + gCount, "[" + gCount + "] " + name, 7, 7);

                        //Right click menu.
                        tree.Nodes["groups"].Nodes["group" + gCount].ContextMenuStrip = sarEntryMenu;

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

                        //Right click menu.
                        tree.Nodes["waveArchives"].Nodes["waveArchive" + wCount].ContextMenuStrip = sarEntryMenu;

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

                        //Right click menu.
                        tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].ContextMenuStrip = CreateMenuStrip(sarEntryMenu, new int[] { 0, 1, 3, 4, 7, 8, 9 }, new EventHandler[] { SarAddAbove_Click, SarAddBelow_Click, null, SarMoveUp_Click, SarMoveDown_Click, null, null, SarRename_Click, SarNullify_Click, SarDelete_Click });

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
                                        key = "waveSoundDatas";
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
                            if (key.Equals("null") || s.StartIndex == 0xFFFFFF) {

                                //Add null.
                                tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, "{ Null Type }", 0, 0);

                            } else if (key.Equals("waveSoundDatas")) {

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

                //Get file names.
                File.RefreshFileNames();

                //Load files.
                int fCount = 0;
                foreach (var f in File.Files) {

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

                        //Add the right click menu.
                        if (f.FileType == EFileType.External) {
                            tree.Nodes["files"].Nodes["file" + fCount].ContextMenuStrip = CreateMenuStrip(filesMenu, new int[] { 2 }, new EventHandler[] { null, null, new EventHandler(ChangeExternalPathToolStripMenuItem_Click) });
                        } else {
                            tree.Nodes["files"].Nodes["file" + fCount].ContextMenuStrip = CreateMenuStrip(filesMenu, new int[] { 0, 1 }, new EventHandler[] { new EventHandler(ReplaceToolStripMenuItem_Click), new EventHandler(ExportToolStripMenuItem_Click) });
                        }

                    }

                    //Increment count.
                    fCount++;

                }

            } else {

                //Remove context menus.
                foreach (TreeNode n in tree.Nodes) {
                    n.ContextMenuStrip = null;
                }

            }

            //End update.
            EndUpdateNodes();

        }

        /// <summary>
        /// Project info updated.
        /// </summary>
        public override void SarProjectInfoUpdated() {

            //Update info.
            File.MaxSequences = (ushort)maxSeqNumBox.Value;
            File.MaxSequenceTracks = (ushort)maxSeqTrackNumBox.Value;
            File.MaxStreamSounds = (ushort)maxStreamNumBox.Value;
            File.MaxStreamTracks = (ushort)maxStreamNumTracksBox.Value;
            File.MaxStreamChannels = (ushort)maxStreamNumChannelsBox.Value;
            File.MaxWaveSounds = (ushort)maxWaveNumBox.Value;
            File.MaxWaveTracks = (ushort)maxWaveNumTracksBox.Value;
            File.StreamBufferTimes = (byte)streamBufferTimesBox.Value;
            File.Options = (uint)optionsPIBox.Value;
            File.CreateStrings = sarIncludeStringBlock.Checked;

        }

        /// <summary>
        /// Add item to the root.
        /// </summary>
        public override void RootAdd() {

            //Get node.
            string node = tree.SelectedNode.Name;

            //Streams.
            if (node.Equals("streams")) {
                CreateStream(File.Streams.Count);
            }

            //Wave Sound Datas.
            else if (node.Equals("waveSoundDatas")) {
                CreateWaveSoundData(File.WaveSoundDatas.Count);
            }

            //Sequences.
            else if (node.Equals("sequences")) {
                CreateSequence(File.Sequences.Count);
            }

            //Sound Sets.
            else if (node.Equals("soundGroups")) {
                CreateSoundSet(File.SoundSets.Count);
            }

            //Banks.
            else if (node.Equals("banks")) {
                CreateBank(File.Banks.Count);
            }

            //Wave Archives.
            else if (node.Equals("waveArchives")) {
                CreateWaveArchive(File.WaveArchives.Count);
            }

            //Groups.
            else if (node.Equals("groups")) {
                CreateGroup(File.Groups.Count);
            }

            //Players.
            else if (node.Equals("players")) {
                CreatePlayer(File.Players.Count);
            }

            //Update nodes.
            UpdateNodes();
            tree.SelectedNode.Expand();

        }

        //Sound editors.
        #region SoundEditors

        public override void StmSound3dButton_Click(object sender, EventArgs e) {
            File.Streams[tree.SelectedNode.Index].Sound3dInfo = Sound3dEditor.GetInfo(File.Streams[tree.SelectedNode.Index].Sound3dInfo, File.Streams[tree.SelectedNode.Index].Name);
        }

        public override void WsdSound3dButton_Click(object sender, EventArgs e) {
            File.WaveSoundDatas[tree.SelectedNode.Index].Sound3dInfo = Sound3dEditor.GetInfo(File.WaveSoundDatas[tree.SelectedNode.Index].Sound3dInfo, File.WaveSoundDatas[tree.SelectedNode.Index].Name);
        }

        public override void SeqEditSound3dInfoButton_Click(object sender, EventArgs e) {
            File.Sequences[tree.SelectedNode.Index].Sound3dInfo = Sound3dEditor.GetInfo(File.Sequences[tree.SelectedNode.Index].Sound3dInfo, File.Sequences[tree.SelectedNode.Index].Name);
        }

        public override void SeqEditSoundInfoButton_Click(object sender, EventArgs e) {
            File.Sequences[tree.SelectedNode.Index].LoadSoundInfo(SoundEditor.GetInfo(File.Sequences[tree.SelectedNode.Index], File, File.Sequences[tree.SelectedNode.Index].Name));
        }

        public override void WsdEditSoundInfoButton_Click(object sender, EventArgs e) {
            File.WaveSoundDatas[tree.SelectedNode.Index].LoadSoundInfo(SoundEditor.GetInfo(File.WaveSoundDatas[tree.SelectedNode.Index], File, File.WaveSoundDatas[tree.SelectedNode.Index].Name));
        }

        public override void StmSoundInfoButton_Click(object sender, EventArgs e) {
            File.Streams[tree.SelectedNode.Index].LoadSoundInfo(SoundEditor.GetInfo(File.Streams[tree.SelectedNode.Index], File, File.Streams[tree.SelectedNode.Index].Name));
        }

        #endregion

        //File info.
        #region FileInfo

        public override void FileTypeBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Files[tree.SelectedNode.Index].FileType = (EFileType)fileTypeBox.SelectedIndex;
                UpdateNodes();
            }
        }

        public override void FilesIncludeGroups_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (filesIncludeGroups.Checked) {
                    File.Files[tree.SelectedNode.Index].Groups = new List<int>();
                    for (int i = 1; i < filesGroupGrid.Rows.Count; i++) {
                        int ind = ((DataGridViewComboBoxCell)filesGroupGrid.Rows[i - 1].Cells[0]).Items.IndexOf(((DataGridViewComboBoxCell)filesGroupGrid.Rows[i - 1].Cells[0]).Value);
                        File.Files[tree.SelectedNode.Index].Groups.Add(ind);

                    }
                } else {
                    File.Files[tree.SelectedNode.Index].Groups = null;
                }
            }
        }

        public override void FilesGroupGridCellChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {

                File.Files[tree.SelectedNode.Index].Groups = new List<int>();
                for (int i = 1; i < filesGroupGrid.Rows.Count; i++) {
                    int ind = ((DataGridViewComboBoxCell)filesGroupGrid.Rows[i - 1].Cells[0]).Items.IndexOf(((DataGridViewComboBoxCell)filesGroupGrid.Rows[i - 1].Cells[0]).Value);
                    File.Files[tree.SelectedNode.Index].Groups.Add(ind);

                }

            }
        }

        #endregion

        //Player info.
        #region PlayerInfo

        public override void PlayerEnableSoundLimitBox_CheckedChanged(object sender, EventArgs e) {
            playerHeapSizeBox.Enabled = playerEnableSoundLimitBox.Checked;
            if (!WritingInfo && FileOpen) {
                File.Players[tree.SelectedNode.Index].IncludeHeapSize = playerEnableSoundLimitBox.Checked;              
            }
        }

        public override void PlayerHeapSizeBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Players[tree.SelectedNode.Index].PlayerHeapSize = (int)playerHeapSizeBox.Value;
            }
        }

        public override void PlayerSoundLimitBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Players[tree.SelectedNode.Index].SoundLimit = (int)playerSoundLimitBox.Value;
            }
        }

        #endregion

        //Group info.
        #region GroupInfo

        public override void SarGrpFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (sarGrpFileIdBox.SelectedIndex == 0) {
                    File.Groups[tree.SelectedNode.Index].File = null;
                } else {
                    File.Groups[tree.SelectedNode.Index].File = new SoundFile<ISoundFile>() { Reference = File.Files[sarGrpFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        #endregion

        //Wave archive info.
        #region WaveArchiveInfo

        public override void SarWarFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (sarWarFileIdBox.SelectedIndex == 0) {
                    File.WaveArchives[tree.SelectedNode.Index].File = null;
                } else {
                    File.WaveArchives[tree.SelectedNode.Index].File = new SoundFile<ISoundFile>() { Reference = File.Files[sarWarFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        public override void WarIncludeWaveCountBox_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.WaveArchives[tree.SelectedNode.Index].IncludeWaveCount = warIncludeWaveCountBox.Checked;
            }
        }

        public override void WarLoadIndividuallyBox_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.WaveArchives[tree.SelectedNode.Index].LoadIndividually = warLoadIndividuallyBox.Checked;
            }
        }

        #endregion

        //Bank info.
        #region BankInfo

        public override void SarBnkFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (sarBnkFileIdBox.SelectedIndex == 0) {
                    File.Banks[tree.SelectedNode.Index].File = null;
                } else {
                    File.Banks[tree.SelectedNode.Index].File = new SoundFile<ISoundFile>() { Reference = File.Files[sarBnkFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        public override void BnkWarsChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {

                File.Banks[tree.SelectedNode.Index].WaveArchives = new List<WaveArchiveEntry>();
                for (int i = 1; i < bankWarDataGrid.Rows.Count; i++) {
                    int ind = ((DataGridViewComboBoxCell)bankWarDataGrid.Rows[i - 1].Cells[0]).Items.IndexOf(((DataGridViewComboBoxCell)bankWarDataGrid.Rows[i - 1].Cells[0]).Value);
                    if (ind == 0) {
                        File.Banks[tree.SelectedNode.Index].WaveArchives.Add(null);
                    } else {
                        File.Banks[tree.SelectedNode.Index].WaveArchives.Add(File.WaveArchives[ind - 1]);
                    }
                }

            }
        }

        #endregion

        //Sound group info.
        #region SoundGroupInfo

        public override void SoundGrpStartIndex_ValueChanged(object sender, EventArgs e) {
            if (soundGrpStartIndex.Value > soundGrpEndIndex.Value) {
                soundGrpStartIndex.Value = soundGrpEndIndex.Value;
            }
            if (!WritingInfo && FileOpen) {
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

        public override void SoundGrpEndIndex_ValueChanged(object sender, EventArgs e) {
            if (soundGrpEndIndex.Value < soundGrpStartIndex.Value) {
                soundGrpEndIndex.Value = soundGrpStartIndex.Value;
            }
            if (!WritingInfo && FileOpen) {
                File.SoundSets[tree.SelectedNode.Index].EndIndex = (int)soundGrpEndIndex.Value;
                UpdateNodes();
            }
        }

        public override void SoundGroupFilesChanged(object sender, EventArgs e) {
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

        public override void SoundGroupWarsChanged(object sender, EventArgs e) {
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

        public override void SarSeqFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (sarSeqFileIdBox.SelectedIndex == 0) {
                    File.Sequences[tree.SelectedNode.Index].File = null;
                } else {
                    File.Sequences[tree.SelectedNode.Index].File = new SoundFile<ISoundFile>() { Reference = File.Files[sarSeqFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        public override void SeqBank0Box_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].Banks[0] = seqBank0Box.SelectedIndex == 0 ? null : File.Banks[seqBank0Box.SelectedIndex - 1];
            }
        }

        public override void SeqBank1Box_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].Banks[1] = seqBank1Box.SelectedIndex == 0 ? null : File.Banks[seqBank1Box.SelectedIndex - 1];
            }
        }

        public override void SeqBank2Box_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].Banks[2] = seqBank2Box.SelectedIndex == 0 ? null : File.Banks[seqBank2Box.SelectedIndex - 1];
            }
        }

        public override void SeqBank3Box_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].Banks[3] = seqBank3Box.SelectedIndex == 0 ? null : File.Banks[seqBank3Box.SelectedIndex - 1];
            }
        }

        public override void SeqC0_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(0, seqC0.Checked);
            }
        }

        public override void SeqC1_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(1, seqC1.Checked);
            }
        }

        public override void SeqC2_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(2, seqC2.Checked);
            }
        }

        public override void SeqC3_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(3, seqC3.Checked);
            }
        }

        public override void SeqC4_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(4, seqC4.Checked);
            }
        }

        public override void SeqC5_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(5, seqC5.Checked);
            }
        }

        public override void SeqC6_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(6, seqC6.Checked);
            }
        }

        public override void SeqC7_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(7, seqC7.Checked);
            }
        }

        public override void SeqC8_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(8, seqC8.Checked);
            }
        }

        public override void SeqC9_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(9, seqC9.Checked);
            }
        }

        public override void SeqC10_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(10, seqC10.Checked);
            }
        }

        public override void SeqC11_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(11, seqC11.Checked);
            }
        }

        public override void SeqC12_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(12, seqC12.Checked);
            }
        }

        public override void SeqC13_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(13, seqC13.Checked);
            }
        }

        public override void SeqC14_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(14, seqC14.Checked);
            }
        }

        public override void SeqC15_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].SetChannelFlag(15, seqC15.Checked);
            }
        }

        public override void SeqChannelPriorityBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].ChannelPriority = (byte)seqChannelPriorityBox.Value;
            }
        }

        public override void SeqIsReleasePriorityBox_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].IsReleasePriority = seqIsReleasePriorityBox.Checked;
            }
        }

        public override void SeqOffsetFromLabelButton_CheckedChanged(object sender, EventArgs e) {
            seqOffsetFromLabelBox.Enabled = seqOffsetFromLabelButton.Checked;
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].StartOffset = (uint)(File.Sequences[tree.SelectedNode.Index].File.File as SoundSequence).Labels[seqOffsetFromLabelBox.SelectedIndex].Offset;
            }
        }

        public override void SeqOffsetManualButton_CheckedChanged(object sender, EventArgs e) {
            seqOffsetManualBox.Enabled = seqOffsetManualButton.Checked;
            if (!WritingInfo && FileOpen) {
                File.Sequences[tree.SelectedNode.Index].StartOffset = (uint)seqOffsetManualBox.Value;
            }
        }

        public override void SeqOffsetFromLabelBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen && seqOffsetFromLabelBox.Enabled) {
                File.Sequences[tree.SelectedNode.Index].StartOffset = (uint)(File.Sequences[tree.SelectedNode.Index].File.File as SoundSequence).Labels[seqOffsetFromLabelBox.SelectedIndex].Offset;
            }
        }

        public override void SeqOffsetManualBox_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen && seqOffsetManualBox.Enabled) {
                File.Sequences[tree.SelectedNode.Index].StartOffset = (uint)seqOffsetManualBox.Value;
            }
        }

        public override void SeqSound3dInfoExists_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                seqEditSound3dInfoButton.Enabled = seqSound3dInfoExists.Checked;
                if (seqSound3dInfoExists.Checked) {
                    File.Sequences[tree.SelectedNode.Index].Sound3dInfo = new Sound3dInfo();
                } else {
                    File.Sequences[tree.SelectedNode.Index].Sound3dInfo = null;
                }
            }
        }

        #endregion

        //Wave sound data.
        #region WaveSoundDataInfo

        public override void SarWsdFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (sarWsdFileIdBox.SelectedIndex == 0) {
                    File.WaveSoundDatas[tree.SelectedNode.Index].File = null;
                } else {
                    File.WaveSoundDatas[tree.SelectedNode.Index].File = new SoundFile<ISoundFile>() { Reference = File.Files[sarWsdFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        public override void WsdChannelPriority_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.WaveSoundDatas[tree.SelectedNode.Index].ChannelPriority = (byte)wsdChannelPriority.Value;
            }
        }

        public override void WsdCopyCount_Click(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
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

        public override void WsdFixPriority_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.WaveSoundDatas[tree.SelectedNode.Index].IsReleasePriority = wsdFixPriority.Checked;
            }
        }

        public override void WsdSound3dEnable_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                wsdSound3dButton.Enabled = wsdSound3dEnable.Checked;
                if (wsdSound3dEnable.Checked) {
                    File.WaveSoundDatas[tree.SelectedNode.Index].Sound3dInfo = new Sound3dInfo();
                } else {
                    File.WaveSoundDatas[tree.SelectedNode.Index].Sound3dInfo = null;
                }
            }
        }

        public override void WsdTracksToAllocate_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.WaveSoundDatas[tree.SelectedNode.Index].AllocateTrackCount = (byte)wsdTracksToAllocate.Value;
            }
        }

        public override void WsdWaveIndex_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.WaveSoundDatas[tree.SelectedNode.Index].WaveIndex = (byte)wsdWaveIndex.Value;
            }
        }

        #endregion

        //Stream info.
        #region StreamInfo

        public override void StmFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (stmFileIdBox.SelectedIndex == 0) {
                    File.Streams[tree.SelectedNode.Index].File = null;
                } else {
                    File.Streams[tree.SelectedNode.Index].File = new SoundFile<ISoundFile>() { Reference = File.Files[stmFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        public override void StmAllocateChannelsNum_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].AllocateChannelCount = (ushort)stmAllocateChannelsNum.Value;
            }
        }

        public override void StmCopyChannelCountFromFile_Click(object sender, EventArgs e) {
            try {
                CitraFileLoader.Stream s = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(Path.GetDirectoryName(FilePath) + "/" + File.Streams[tree.SelectedNode.Index].File.ExternalFileName));
                File.Streams[tree.SelectedNode.Index].AllocateChannelCount = (ushort)s.Stm.info.channels.Count();
                stmAllocateChannelsNum.Value = File.Streams[tree.SelectedNode.Index].AllocateChannelCount;
            } catch { MessageBox.Show("Unable to open stream!"); }
        }

        public override void StmIncludeExtension_CheckedChanged(object sender, EventArgs e) {
            stmLoopStartFrame.Enabled = stmLoopEndFrame.Enabled = stmCopyExtensionFromFile.Enabled = stmIncludeExtension.Checked;
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SoundExtensionIncluded = stmIncludeExtension.Checked;
                File.Streams[tree.SelectedNode.Index].LoopStartFrame = (uint)stmLoopStartFrame.Value;
                File.Streams[tree.SelectedNode.Index].LoopEndFrame = (uint)stmLoopEndFrame.Value;
            }
        }

        public override void StmLoopStartFrame_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].LoopStartFrame = (uint)stmLoopStartFrame.Value;
            }
        }

        public override void StmLoopEndFrame_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].LoopEndFrame = (uint)stmLoopEndFrame.Value;
            }
        }

        public override void StmCopyExtensionFromFile_Click(object sender, EventArgs e) {
            try {
                CitraFileLoader.Stream s = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(Path.GetDirectoryName(FilePath) + "/" + File.Streams[tree.SelectedNode.Index].File.ExternalFileName));
                File.Streams[tree.SelectedNode.Index].LoopStartFrame = s.Stm.info.streamSoundInfo.originalLoopStart;
                File.Streams[tree.SelectedNode.Index].LoopEndFrame = s.Stm.info.streamSoundInfo.originalLoopEnd;
            } catch { MessageBox.Show("Unable to open stream!"); }
        }

        public override void StmGeneratePrefetch_CheckedChanged(object sender, EventArgs e) {
            stmPrefetchFileIdBox.Enabled = stmCreateUniquePrefetchFile.Enabled = stmUpdatePrefetchInfo.Enabled = stmGeneratePrefetch.Checked;
            if (stmGeneratePrefetch.Checked) {
                stmCreateUniquePrefetchFile.Enabled = !File.FileUnique(File.Files.IndexOf(File.Streams[tree.SelectedNode.Index].PrefetchFile));
            }
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].GeneratePrefetchFile = stmGeneratePrefetch.Checked;
            }
        }

        public override void StmPrefetchFileIdBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (stmPrefetchFileIdBox.SelectedIndex == 0) {
                    File.Streams[tree.SelectedNode.Index].PrefetchFile = null;
                } else {
                    File.Streams[tree.SelectedNode.Index].PrefetchFile = new SoundFile<ISoundFile>() { Reference = File.Files[stmPrefetchFileIdBox.SelectedIndex - 1] };
                }
            }
        }

        public override void StmCreateUniquePrefetchFile_Click(object sender, EventArgs e) {
            if (File.Streams[tree.SelectedNode.Index].PrefetchFile != null) {
                File.Streams[tree.SelectedNode.Index].PrefetchFile = File.AddNewFile(SoundArchive.NewFileEntryType.Prefetch, tree.SelectedNode.Index - 1, File.Streams[tree.SelectedNode.Index].PrefetchFile.File);
            }
        }

        public override void StmUpdatePrefetchInfo_Click(object sender, EventArgs e) {

            //Get stream.
            try {

                //Get stream.
                CitraFileLoader.Stream s = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(Path.GetDirectoryName(FilePath) + "/" + File.Streams[tree.SelectedNode.Index].File.ExternalFileName));

                //Get version.
                byte maj = 5;
                byte min = 0;
                byte rev = 0;
                foreach (var f in File.Files) {
                    if (f != null) {
                        if (f.File != null) {
                            var z = f.File as PrefetchFile;
                            if (z != null) {
                                maj = z.Version.Major;
                                min = z.Version.Minor;
                                rev = z.Version.Revision;
                                break;
                            }
                        }
                    }
                }

                //Make prefetch.
                File.Streams[tree.SelectedNode.Index].PrefetchFile.File = new PrefetchFile() {
                    Stream = s,
                    Version = new FileWriter.Version(maj, min, rev)
                };

            } catch { MessageBox.Show("Unable to open stream!"); }

        }

        public override void StmTrack0_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(0, stmTrack0.Checked);
            }
        }

        public override void StmTrack1_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(1, stmTrack1.Checked);
            }
        }

        public override void StmTrack2_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(2, stmTrack2.Checked);
            }
        }

        public override void StmTrack3_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(3, stmTrack3.Checked);
            }
        }

        public override void StmTrack4_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(4, stmTrack4.Checked);
            }
        }

        public override void StmTrack5_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(5, stmTrack5.Checked);
            }
        }

        public override void StmTrack6_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(6, stmTrack6.Checked);
            }
        }

        public override void StmTrack7_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(7, stmTrack7.Checked);
            }
        }

        public override void StmTrack8_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(8, stmTrack8.Checked);
            }
        }

        public override void StmTrack9_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(9, stmTrack9.Checked);
            }
        }

        public override void StmTrack10_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(10, stmTrack10.Checked);
            }
        }

        public override void StmTrack11_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(11, stmTrack11.Checked);
            }
        }

        public override void StmTrack12_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(12, stmTrack12.Checked);
            }
        }

        public override void StmTrack13_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(13, stmTrack13.Checked);
            }
        }

        public override void StmTrack14_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(14, stmTrack14.Checked);
            }
        }

        public override void StmTrack15_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SetTrackFlag(15, stmTrack15.Checked);
            }
        }

        public override void StmWriteTrackInfo_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                if (stmWriteTrackInfo.Checked) {
                    File.Streams[tree.SelectedNode.Index].Tracks = new List<StreamTrackInfo>();
                } else {
                    File.Streams[tree.SelectedNode.Index].Tracks = null;
                }
            }
        }

        public override void StmUpdateTrackInfo_Click(object sender, EventArgs e) {
            try {
                CitraFileLoader.Stream s = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(Path.GetDirectoryName(FilePath) + "/" + File.Streams[tree.SelectedNode.Index].File.ExternalFileName));
                if (s.Stm.info.tracks == null) {
                    File.Streams[tree.SelectedNode.Index].Tracks = null;
                } else {
                    File.Streams[tree.SelectedNode.Index].Tracks = new List<StreamTrackInfo>();
                    foreach (var t in s.Stm.info.tracks) {
                        File.Streams[tree.SelectedNode.Index].Tracks.Add(new StreamTrackInfo() {
                            BiquadType = 0,
                            BiquadValue = 0,
                            LpfFrequency = 64,
                            Pan = (sbyte)t.pan,
                            Channels = t.globalChannelIndexTable.entries,
                            SendValue = new byte[] { 127, 0, 0, 0 },
                            Span = (sbyte)t.span,
                            SurroundMode = t.surroundMode > 0,
                            Volume = t.volume
                        });
                    }
                }
                UpdateNodes();
                DoInfoStuff();
            } catch { MessageBox.Show("Unable to open stream!"); }
        }

        public override void StmPitch_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].Pitch = (float)stmPitch.Value;
            }
        }

        public override void StmStreamType_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].StreamFileType = (StreamEntry.EStreamFileType)stmStreamType.SelectedIndex;
            }
        }

        public override void StmSound3dEnable_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                stmSound3dButton.Enabled = stmSound3dEnable.Checked;
                if (stmSound3dEnable.Checked) {
                    File.Streams[tree.SelectedNode.Index].Sound3dInfo = new Sound3dInfo();
                } else {
                    File.Streams[tree.SelectedNode.Index].Sound3dInfo = null;
                }
            }
        }

        public override void StmSendMain_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SendValue[0] = (byte)stmSendMain.Value;
            }
        }

        public override void StmSendA_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SendValue[1] = (byte)stmSendA.Value;
            }
        }

        public override void StmSendB_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SendValue[2] = (byte)stmSendB.Value;
            }
        }

        public override void StmSendC_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Index].SendValue[3] = (byte)stmSendC.Value;
            }
        }

        #endregion

        //Track info.
        #region TrackInfo

        public override void TrackBiquadType_SelectedIndexChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].BiquadType = (sbyte)trackBiquadType.SelectedIndex;
            }
        }

        public override void TrackBiquadValue_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].BiquadValue = (sbyte)trackBiquadValue.Value;
            }
        }

        public override void TrackChannelsChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {

                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].Channels = new List<byte>();
                for (int i = 1; i < trackChannelGrid.Rows.Count; i++) {
                    byte ind = byte.Parse(((DataGridViewTextBoxCell)trackChannelGrid.Rows[i - 1].Cells[0]).Value.ToString());
                    File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].Channels.Add(ind);
                }

            }
        }

        public override void TrackLPFFrequency_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].LpfFrequency = (sbyte)trackLPFFrequency.Value;
            }
        }

        public override void TrackPan_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].Pan = (sbyte)trackPan.Value;
            }
        }

        public override void TrackSendMain_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].SendValue[0] = (byte)trackSendMain.Value;
            }
        }

        public override void TrackSendA_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].SendValue[1] = (byte)trackSendA.Value;
            }
        }

        public override void TrackSendB_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].SendValue[2] = (byte)trackSendB.Value;
            }
        }

        public override void TrackSendC_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].SendValue[3] = (byte)trackSendC.Value;
            }
        }

        public override void TrackSpan_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].Span = (sbyte)trackSpan.Value;
            }
        }

        public override void TrackSurround_CheckedChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].SurroundMode = trackSurround.Checked;
            }
        }

        public override void TrackVolume_ValueChanged(object sender, EventArgs e) {
            if (!WritingInfo && FileOpen) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks[tree.SelectedNode.Index].Volume = (byte)trackVolume.Value;
            }
        }

        #endregion

        //Version info.
        #region VersionInfo

        public override void ByteOrderBox_SelectedIndexChanged(object sender, EventArgs e) {
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

        public override void VersionMax_ValueChanged(object sender, EventArgs e) {
            File.Version.Major = (byte)versionMax.Value;
        }

        public override void VersionMin_ValueChanged(object sender, EventArgs e) {
            File.Version.Minor = (byte)versionMin.Value;
        }

        public override void VersionRev_ValueChanged(object sender, EventArgs e) {
            File.Version.Revision = (byte)versionRev.Value;
        }

        public override void SeqVersionUpdate_Click(object sender, EventArgs e) {
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

        public override void BankVersionUpdate_Click(object sender, EventArgs e) {
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

        public override void WarVersionUpdate_Click(object sender, EventArgs e) {
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

        public override void WsdVersionUpdate_Click(object sender, EventArgs e) {
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

        public override void GrpVersionUpdate_Click(object sender, EventArgs e) {
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

        public override void StmVersionUpdate_Click(object sender, EventArgs e) {
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

        public override void StpVersionUpdate_Click(object sender, EventArgs e) {
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

        //File right click menu.
        #region FileRightClickMenu

        public override void ExportToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog s = new SaveFileDialog();
            s.RestoreDirectory = true;
            string be = File.Files[tree.SelectedNode.Index].FileExtension.Substring(2, 3);
            s.Filter = "Sound File|*.bf" + be + ";*.bc" + be + "|Switch Sound File|*.bf" + be;
            if (File.WriteMode == WriteMode.NX) {
                s.FilterIndex = 2;
            }
            s.FileName = File.Files[tree.SelectedNode.Index].FileName + "." + File.Files[tree.SelectedNode.Index].FileExtension;
            s.ShowDialog();
            if (s.FileName != "") {
                WriteMode w = WriteMode.CTR;
                if (s.FileName.ToLower().Contains(".bf")) {
                    if (s.FilterIndex == 1) {
                        w = WriteMode.Cafe;
                    } else {
                        w = WriteMode.CTR;
                    }
                }
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);
                var f = SoundArchiveReader.ReadFile(SoundArchiveWriter.WriteFile(File.Files[tree.SelectedNode.Index].File));
                f.Write(w, bw);
                System.IO.File.WriteAllBytes(s.FileName, o.ToArray());
                try { bw.Dispose(); } catch { }
                try { o.Dispose(); } catch { }
            }
        }

        public override void ReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog s = new OpenFileDialog();
            s.RestoreDirectory = true;
            string be = File.Files[tree.SelectedNode.Index].FileExtension.Substring(2, 3);
            s.Filter = "Sound File|*.bf" + be + ";*.bc" + be;
            s.ShowDialog();
            if (s.FileName != "") {
                File.Files[tree.SelectedNode.Index].File = SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(s.FileName));
            }
            DoInfoStuff();
        }

        public override void ChangeExternalPathToolStripMenuItem_Click(object sender, EventArgs e) {
            string h = Microsoft.VisualBasic.Interaction.InputBox("New External Path", "Enter Path:", File.Files[tree.SelectedNode.Index].ExternalFileName);
            if (h != "") {
                File.Files[tree.SelectedNode.Index].ExternalFileName = h;
                UpdateNodes();
            }
        }

        #endregion

        //Node right click menu.
        #region NodeRightClickMenu

        public override void SarAddAbove_Click(object sender, EventArgs e) {

            //Track.
            if (tree.SelectedNode.Parent.Parent != null) {
                if (File.Streams[tree.SelectedNode.Parent.Index].Tracks != null) {
                    File.Streams[tree.SelectedNode.Parent.Index].Tracks.Insert(tree.SelectedNode.Index, new StreamTrackInfo());
                    UpdateNodes();
                } else {
                    MessageBox.Show("Stream is not set to write track info!");
                }
                return;
            }

            //Get node.
            string node = tree.SelectedNode.Parent.Name;

            //Streams.
            if (node.Equals("streams")) {
                CreateStream(tree.SelectedNode.Index);
            }

            //Wave Sound Datas.
            else if (node.Equals("waveSoundDatas")) {
                CreateWaveSoundData(tree.SelectedNode.Index);
            }

            //Sequences.
            else if (node.Equals("sequences")) {
                CreateSequence(tree.SelectedNode.Index);
            }

            //Sound Sets.
            else if (node.Equals("soundGroups")) {
                CreateSoundSet(tree.SelectedNode.Index);
            }

            //Banks.
            else if (node.Equals("banks")) {
                CreateBank(tree.SelectedNode.Index);
            }

            //Wave Archives.
            else if (node.Equals("waveArchives")) {
                CreateWaveArchive(tree.SelectedNode.Index);
            }

            //Groups.
            else if (node.Equals("groups")) {
                CreateGroup(tree.SelectedNode.Index);
            }

            //Players.
            else if (node.Equals("players")) {
                CreatePlayer(tree.SelectedNode.Index);
            }

            //Update nodes.
            UpdateNodes();

        }

        public override void SarAddBelow_Click(object sender, EventArgs e) {

            //Track.
            if (tree.SelectedNode.Parent.Parent != null) {
                if (File.Streams[tree.SelectedNode.Parent.Index].Tracks != null) {
                    File.Streams[tree.SelectedNode.Parent.Index].Tracks.Insert(tree.SelectedNode.Index + 1, new StreamTrackInfo());
                    UpdateNodes();
                } else {
                    MessageBox.Show("Stream is not set to write track info!");
                }
                return;
            }

            //Get node.
            string node = tree.SelectedNode.Parent.Name;

            //Streams.
            if (node.Equals("streams")) {
                CreateStream(tree.SelectedNode.Index + 1);
            }

            //Wave Sound Datas.
            else if (node.Equals("waveSoundDatas")) {
                CreateWaveSoundData(tree.SelectedNode.Index + 1);
            }

            //Sequences.
            else if (node.Equals("sequences")) {
                CreateSequence(tree.SelectedNode.Index + 1);
            }

            //Sound Sets.
            else if (node.Equals("soundGroups")) {
                CreateSoundSet(tree.SelectedNode.Index + 1);
            }

            //Banks.
            else if (node.Equals("banks")) {
                CreateBank(tree.SelectedNode.Index + 1);
            }

            //Wave Archives.
            else if (node.Equals("waveArchives")) {
                CreateWaveArchive(tree.SelectedNode.Index + 1);
            }

            //Groups.
            else if (node.Equals("groups")) {
                CreateGroup(tree.SelectedNode.Index + 1);
            }

            //Players.
            else if (node.Equals("players")) {
                CreatePlayer(tree.SelectedNode.Index + 1);
            }

            //Update nodes.
            UpdateNodes();

        }

        public override void SarAddInside_Click(object sender, EventArgs e) {

            //Add new track info.
            if (File.Streams[tree.SelectedNode.Index].Tracks != null) {
                File.Streams[tree.SelectedNode.Index].Tracks.Add(new StreamTrackInfo());
                UpdateNodes();
            } else {
                MessageBox.Show("Stream is not set to write track info!");
            }

        }

        public override void SarMoveUp_Click(object sender, EventArgs e) {

            //Track.
            if (tree.SelectedNode.Parent.Parent != null) {
                if (base.Swap(File.Streams[tree.SelectedNode.Parent.Index].Tracks, tree.SelectedNode.Index, tree.SelectedNode.Index - 1)) {
                    UpdateNodes();
                    DoInfoStuff();
                    tree.SelectedNode = tree.Nodes[tree.SelectedNode.Parent.Parent.Index].Nodes[tree.SelectedNode.Parent.Index].Nodes[tree.SelectedNode.Index - 1];
                }
                return;
            }

            string node = tree.SelectedNode.Parent.Name;
            bool ret = false;
            if (node.Equals("stream")) {
                ret = Swap(File.Streams, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("sequences")) {
                ret = Swap(File.Sequences, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("waveSoundDatas")) {
                ret = Swap(File.WaveSoundDatas, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("soundGroups")) {
                ret = Swap(File.SoundSets, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("banks")) {
                ret = Swap(File.Banks, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("waveArchives")) {
                ret = Swap(File.WaveArchives, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("players")) {
                ret = Swap(File.Players, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            } else if (node.Equals("groups")) {
                ret = Swap(File.Groups, tree.SelectedNode.Index, tree.SelectedNode.Index - 1);
            }
            if (ret) {
                tree.SelectedNode = tree.Nodes[tree.SelectedNode.Parent.Index].Nodes[tree.SelectedNode.Index - 1];
                UpdateNodes();
                DoInfoStuff();
            }
        }

        public override void SarMoveDown_Click(object sender, EventArgs e) {

            //Track.
            if (tree.SelectedNode.Parent.Parent != null) {
                if (base.Swap(File.Streams[tree.SelectedNode.Parent.Index].Tracks, tree.SelectedNode.Index, tree.SelectedNode.Index + 1)) {
                    UpdateNodes();
                    DoInfoStuff();
                    tree.SelectedNode = tree.Nodes[tree.SelectedNode.Parent.Parent.Index].Nodes[tree.SelectedNode.Parent.Index].Nodes[tree.SelectedNode.Index + 1];
                }
                return;
            }

            string node = tree.SelectedNode.Parent.Name;
            bool ret = false;
            if (node.Equals("stream")) {
                ret = Swap(File.Streams, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("sequences")) {
                ret = Swap(File.Sequences, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("waveSoundDatas")) {
                ret = Swap(File.WaveSoundDatas, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("soundGroups")) {
                ret = Swap(File.SoundSets, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("banks")) {
                ret = Swap(File.Banks, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("waveArchives")) {
                ret = Swap(File.WaveArchives, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("players")) {
                ret = Swap(File.Players, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            } else if (node.Equals("groups")) {
                ret = Swap(File.Groups, tree.SelectedNode.Index, tree.SelectedNode.Index + 1);
            }
            if (ret) {
                tree.SelectedNode = tree.Nodes[tree.SelectedNode.Parent.Index].Nodes[tree.SelectedNode.Index + 1];
                UpdateNodes();
                DoInfoStuff();
            }
        }

        public override void SarExport_Click(object sender, EventArgs e) {

            //Sound file.
            SoundFile<ISoundFile> f = null;
            string node = tree.SelectedNode.Parent.Name;
            if (node.Equals("waveSoundDatas")) {
                f = File.WaveSoundDatas[tree.SelectedNode.Index].File;
            } else if (node.Equals("sequences")) {
                f = File.Sequences[tree.SelectedNode.Index].File;
            } else if (node.Equals("banks")) {
                f = File.Banks[tree.SelectedNode.Index].File;
            } else if (node.Equals("waveArchives")) {
                f = File.WaveArchives[tree.SelectedNode.Index].File;
            } else if (node.Equals("groups")) {
                f = File.Groups[tree.SelectedNode.Index].File;
            }

            if (f == null) {
                MessageBox.Show("You can't export an entry that has no file attached!");
                return;
            }

            SaveFileDialog s = new SaveFileDialog();
            s.RestoreDirectory = true;
            string be = f.FileExtension.Substring(2, 3);
            s.Filter = "Sound File|*.bf" + be + ";*.bc" + be + "|Switch Sound File|*.bf" + be;
            if (File.WriteMode == WriteMode.NX) {
                s.FilterIndex = 2;
            }
            s.FileName = f.FileName + "." + f.FileExtension;
            s.ShowDialog();
            if (s.FileName != "") {
                WriteMode w = WriteMode.CTR;
                if (s.FileName.ToLower().Contains(".bf")) {
                    if (s.FilterIndex == 1) {
                        w = WriteMode.Cafe;
                    } else {
                        w = WriteMode.CTR;
                    }
                }
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);
                var f2 = SoundArchiveReader.ReadFile(SoundArchiveWriter.WriteFile(f.File));
                f2.Write(w, bw);
                System.IO.File.WriteAllBytes(s.FileName, o.ToArray());
                try { bw.Dispose(); } catch { }
                try { o.Dispose(); } catch { }
            }
        }

        public override void SarReplace_Click(object sender, EventArgs e) {

            //Sound file.
            SoundFile<ISoundFile> f = null;
            string node = tree.SelectedNode.Parent.Name;
            if (node.Equals("waveSoundDatas")) {
                f = File.WaveSoundDatas[tree.SelectedNode.Index].File;
            } else if (node.Equals("sequences")) {
                f = File.Sequences[tree.SelectedNode.Index].File;
            } else if (node.Equals("banks")) {
                f = File.Banks[tree.SelectedNode.Index].File;
            } else if (node.Equals("waveArchives")) {
                f = File.WaveArchives[tree.SelectedNode.Index].File;
            } else if (node.Equals("groups")) {
                f = File.Groups[tree.SelectedNode.Index].File;
            }

            if (f == null) {
                MessageBox.Show("You can't import to an entry that has no file attached!");
                return;
            }

            OpenFileDialog s = new OpenFileDialog();
            s.RestoreDirectory = true;
            string be = f.FileExtension.Substring(2, 3);
            s.Filter = "Sound File|*.bf" + be + ";*.bc" + be;
            s.ShowDialog();
            if (s.FileName != "") {
                f.File = SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(s.FileName));
            }
            DoInfoStuff();

        }

        public override void SarRename_Click(object sender, EventArgs e) {
            string h = Microsoft.VisualBasic.Interaction.InputBox("New Name", "Enter Name:", tree.SelectedNode.Text.Split(']')[1].Substring(1));
            if (h != "") {
                string node = tree.SelectedNode.Parent.Name;
                if (node.Equals("stream")) {
                    File.Streams[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("sequences")) {
                    File.Sequences[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("waveSoundDatas")) {
                    File.WaveSoundDatas[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("soundGroups")) {
                    File.SoundSets[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("banks")) {
                    File.Banks[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("waveArchives")) {
                    File.WaveArchives[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("players")) {
                    File.Players[tree.SelectedNode.Index].Name = h;
                } else if (node.Equals("groups")) {
                    File.Groups[tree.SelectedNode.Index].Name = h;
                }
                UpdateNodes();
            }
        }

        public override void SarNullify_Click(object sender, EventArgs e) {
            //Does this even... Exist?
        }

        public override void SarDelete_Click(object sender, EventArgs e) {

            //Track.
            if (tree.SelectedNode.Parent.Parent != null) {
                File.Streams[tree.SelectedNode.Parent.Index].Tracks.RemoveAt(tree.SelectedNode.Index);
                UpdateNodes();
                return;
            }

            //Get node.
            string node = tree.SelectedNode.Parent.Name;

            //Streams.
            if (node.Equals("streams")) {
                RemoveStream(tree.SelectedNode.Index);
            }

            //Wave Sound Datas.
            else if (node.Equals("waveSoundDatas")) {
                RemoveWaveSoundData(tree.SelectedNode.Index);
            }

            //Sequences.
            else if (node.Equals("sequences")) {
                RemoveSequence(tree.SelectedNode.Index);
            }

            //Sound Sets.
            else if (node.Equals("soundGroups")) {
                RemoveSoundGroup(tree.SelectedNode.Index);
            }

            //Banks.
            else if (node.Equals("banks")) {
                RemoveBank(tree.SelectedNode.Index);
            }

            //Wave Archives.
            else if (node.Equals("waveArchives")) {
                RemoveWaveArchive(tree.SelectedNode.Index);
            }

            //Groups.
            else if (node.Equals("groups")) {
                RemoveGroup(tree.SelectedNode.Index);
            }

            //Players.
            else if (node.Equals("players")) {
                RemovePlayer(tree.SelectedNode.Index);
            }

            //Update nodes.
            UpdateNodes();
            DoInfoStuff();

        }

        #endregion

        //Swap.
        #region SwapFix

        public new bool Swap<T>(IList<T> objects, int a, int b) {

            //Make sure it is possible.
            if (a < 0 || a >= objects.Count || b < 0 || b >= objects.Count) {
                return false;
            }

            //Swap objects.
            T temp = objects[a];
            objects[a] = objects[b];
            objects[b] = temp;

            //Wave archives.
            if (objects.Equals(File.WaveArchives)) {

                //Go through each file.
                for (int i = 0; i < File.Files.Count; i++) {

                    //File is not null.
                    if (File.Files[i].File != null) {

                        //Bank.
                        if (File.Files[i].File as SoundBank != null) {
                            var f = File.Files[i].File as SoundBank;
                            for (int j = 0; j < f.Waves.Count; j++) {
                                if (f.Waves[j].WarIndex == a) {
                                    f.Waves[j].WarIndex = b;
                                } else if (f.Waves[j].WarIndex == b) {
                                    f.Waves[j].WarIndex = a;
                                }
                            }
                        }

                        //WSD.
                        if (File.Files[i].File as WaveSoundData != null) {
                            var f = File.Files[i].File as WaveSoundData;
                            for (int j = 0; j < f.Waves.Count; j++) {
                                if (f.Waves[j].WarIndex == a) {
                                    f.Waves[j].WarIndex = b;
                                } else if (f.Waves[j].WarIndex == b) {
                                    f.Waves[j].WarIndex = a;
                                }
                            }
                        }

                        //Group.
                        if (File.Files[i].File as Group != null) {
                            var f = File.Files[i].File as Group;
                            for (int j = 0; j < f.ExtraInfo.Count; j++) {
                                if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.WaveArchive) {
                                    if (f.ExtraInfo[j].ItemIndex == a) {
                                        f.ExtraInfo[j].ItemIndex = b;
                                    } else if (f.ExtraInfo[j].ItemIndex == b) {
                                        f.ExtraInfo[j].ItemIndex = a;
                                    }
                                }
                            }
                        }

                    }

                }

            }

            //Banks.
            else if (objects.Equals(File.Banks)) {

                //Go through each file.
                for (int i = 0; i < File.Files.Count; i++) {

                    //File is not null.
                    if (File.Files[i].File != null) {

                        //Group.
                        if (File.Files[i].File as Group != null) {
                            var f = File.Files[i].File as Group;
                            for (int j = 0; j < f.ExtraInfo.Count; j++) {
                                if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.Bank) {
                                    if (f.ExtraInfo[j].ItemIndex == a) {
                                        f.ExtraInfo[j].ItemIndex = b;
                                    } else if (f.ExtraInfo[j].ItemIndex == b) {
                                        f.ExtraInfo[j].ItemIndex = a;
                                    }
                                }
                            }
                        }

                    }

                }

            }

            //Sequences.
            else if (objects.Equals(File.Sequences)) {

                //Go through each file.
                for (int i = 0; i < File.Files.Count; i++) {

                    //File is not null.
                    if (File.Files[i].File != null) {

                        //Group.
                        if (File.Files[i].File as Group != null) {
                            var f = File.Files[i].File as Group;
                            for (int j = 0; j < f.ExtraInfo.Count; j++) {
                                if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.Sound) {
                                    if (f.ExtraInfo[j].ItemIndex == a) {
                                        f.ExtraInfo[j].ItemIndex = b;
                                    } else if (f.ExtraInfo[j].ItemIndex == b) {
                                        f.ExtraInfo[j].ItemIndex = a;
                                    }
                                }
                            }
                        }

                    }

                }

            }

            //Sequence set.
            else if (objects.Equals(File.SoundSets)) {

                //Go through each file.
                for (int i = 0; i < File.Files.Count; i++) {

                    //File is not null.
                    if (File.Files[i].File != null) {

                        //Group.
                        if (File.Files[i].File as Group != null) {
                            var f = File.Files[i].File as Group;
                            for (int j = 0; j < f.ExtraInfo.Count; j++) {
                                if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.SequenceSetOrWaveData) {
                                    if (f.ExtraInfo[j].ItemIndex == a) {
                                        f.ExtraInfo[j].ItemIndex = b;
                                    } else if (f.ExtraInfo[j].ItemIndex == b) {
                                        f.ExtraInfo[j].ItemIndex = a;
                                    }
                                }
                            }
                        }

                    }

                }

            }

            return true;

        }

        #endregion

        //Creators.
        #region Creators

        /// <summary>
        /// Create a new stream at the index.
        /// </summary>
        /// <param name="index">Index to create the stream at.</param>
        public void CreateStream(int index) {

            //Player must exist first.
            if (File.Players.Count == 0) {
                MessageBox.Show("There must be at least one player to create a sound entry!");
                return;
            }

            //Get file.
            SoundFile<ISoundFile> f = FileWizard.GetInfo(File, SoundArchive.NewFileEntryType.Stream, index - 1, FilePath);
            if (f.FileId == -1) {
                return;
            } else {

                //Null.
                if (f.FileId == -2) {
                    f = null;
                }

                //New entry.
                StreamEntry e = new StreamEntry() { Name = "STM_" + index, File = f, Player = File.Players[0], Sound3dInfo = new Sound3dInfo() };
                File.Streams.Insert(index, e);

                //Manage sound sets.
                for (int i = 0; i < File.SoundSets.Count; i++) {
                    if (File.SoundSets[i].StartIndex >= index && File.SoundSets[i].StartIndex != 0xFFFFFF) {
                        File.SoundSets[i].StartIndex++;
                    }
                    if (File.SoundSets[i].EndIndex >= index && File.SoundSets[i].EndIndex != 0xFFFFFF) {
                        File.SoundSets[i].EndIndex++;
                        if (File.SoundSets[i].EndIndex < 0) {
                            File.SoundSets[i].EndIndex = 0;
                        }
                    }
                }

            }

        }

        /// <summary>
        /// Create a new wave sound data at the index.
        /// </summary>
        /// <param name="index">Index to create the wave sound data at.</param>
        public void CreateWaveSoundData(int index) {

            //Player must exist first.
            if (File.Players.Count == 0) {
                MessageBox.Show("There must be at least one player to create a sound entry!");
                return;
            }

            //Get file.
            SoundFile<ISoundFile> f = FileWizard.GetInfo(File, SoundArchive.NewFileEntryType.WaveSoundData, index - 1, FilePath);
            if (f.FileId == -1) {
                return;
            } else {

                //Null.
                if (f.FileId == -2) {
                    f = null;
                }

                //New entry.
                WaveSoundDataEntry e = new WaveSoundDataEntry() { Name = "WSD_" + index, File = f, Player = File.Players[0], ChannelPriority = 64, Sound3dInfo = new Sound3dInfo() };
                File.WaveSoundDatas.Insert(index, e);

                //Manage sound sets.
                for (int i = 0; i < File.SoundSets.Count; i++) {
                    if (File.SoundSets[i].StartIndex >= index && File.SoundSets[i].StartIndex != 0xFFFFFF) {
                        File.SoundSets[i].StartIndex++;
                    }
                    if (File.SoundSets[i].EndIndex >= index && File.SoundSets[i].EndIndex != 0xFFFFFF) {
                        File.SoundSets[i].EndIndex++;
                        if (File.SoundSets[i].EndIndex < 0) {
                            File.SoundSets[i].EndIndex = 0;
                        }
                    }
                }

            }

        }

        /// <summary>
        /// Create a new sequence at the index.
        /// </summary>
        /// <param name="index">Index to create the sequence at.</param>
        public void CreateSequence(int index) {

            //Player must exist first.
            if (File.Players.Count == 0) {
                MessageBox.Show("There must be at least one player to create a sound entry!");
                return;
            }

            //Get file.
            SoundFile<ISoundFile> f = FileWizard.GetInfo(File, SoundArchive.NewFileEntryType.Sequence, index - 1, FilePath);
            if (f.FileId == -1) {
                return;
            } else {

                //Null.
                if (f.FileId == -2) {
                    f = null;
                }

                //New entry.
                SequenceEntry e = new SequenceEntry() { Name = "SEQ_" + index, File = f, Banks = new BankEntry[4], Player = File.Players[0], Sound3dInfo = new Sound3dInfo() };
                File.Sequences.Insert(index, e);

                //Manage sound sets.
                for (int i = 0; i < File.SoundSets.Count; i++) {
                    if (File.SoundSets[i].StartIndex >= index && File.SoundSets[i].StartIndex != 0xFFFFFF) {
                        File.SoundSets[i].StartIndex++;
                    }
                    if (File.SoundSets[i].EndIndex >= index && File.SoundSets[i].EndIndex != 0xFFFFFF) {
                        File.SoundSets[i].EndIndex++;
                        if (File.SoundSets[i].EndIndex < 0) {
                            File.SoundSets[i].EndIndex = 0;
                        }
                    }
                }

            }

        }

        /// <summary>
        /// Create a new sound set at the index.
        /// </summary>
        /// <param name="index">Index to create the sound set at.</param>
        public void CreateSoundSet(int index) {

            //New entry.
            SoundSetEntry e = new SoundSetEntry() { Name = "SOUND_SET_" + index, StartIndex = 0xFFFFFF, EndIndex = 0xFFFFFF, WaveArchives = null, Files = new List<SoundFile<ISoundFile>>(), SoundType = SoundType.Sound };
            File.SoundSets.Insert(index, e);

        }

        /// <summary>
        /// Create a new bank at the index.
        /// </summary>
        /// <param name="index">Index to create the bank at.</param>
        public void CreateBank(int index) {

            //Get file.
            SoundFile<ISoundFile> f = FileWizard.GetInfo(File, SoundArchive.NewFileEntryType.Bank, index - 1, FilePath);
            if (f.FileId == -1) {
                return;
            } else {

                //Null.
                if (f.FileId == -2) {
                    f = null;
                }

                //New entry.
                BankEntry e = new BankEntry() { Name = "BANK_" + index, File = f, WaveArchives = new List<WaveArchiveEntry>() };
                File.Banks.Insert(index, e);

            }

        }

        /// <summary>
        /// Create a new wave archive at the index.
        /// </summary>
        /// <param name="index">Index to create the wave archive at.</param>
        public void CreateWaveArchive(int index) {

            //Get file.
            SoundFile<ISoundFile> f = FileWizard.GetInfo(File, SoundArchive.NewFileEntryType.WaveArchive, index - 1, FilePath);
            if (f.FileId == -1) {
                return;
            } else {

                //Null.
                if (f.FileId == -2) {
                    f = null;
                }

                //New entry.
                WaveArchiveEntry e = new WaveArchiveEntry() { Name = "WARC_" + index, IncludeWaveCount = false, LoadIndividually = false, File = f };
                File.WaveArchives.Insert(index, e);

            }

        }

        /// <summary>
        /// Create a new group at the index.
        /// </summary>
        /// <param name="index">Index to create the group at.</param>
        public void CreateGroup(int index) {

            //Get file.
            SoundFile<ISoundFile> f = FileWizard.GetInfo(File, SoundArchive.NewFileEntryType.Group, index - 1, FilePath);
            if (f.FileId == -1) {
                return;
            } else {

                //Null.
                if (f.FileId == -2) {
                    f = null;
                }

                //New entry.
                GroupEntry e = new GroupEntry() { Name = "GROUP_" + index, File = f };
                File.Groups.Insert(index, e);

            }

        }

        /// <summary>
        /// Create a new player at the index.
        /// </summary>
        /// <param name="index">Index to create the player at.</param>
        public void CreatePlayer(int index) {

            //New entry.
            PlayerEntry e = new PlayerEntry() { Name = "PLAYER_" + index, PlayerHeapSize = 0, SoundLimit = 0 };
            File.Players.Insert(index, e);

        }

        #endregion

        //Destroyers.
        #region Destroyers

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveStream(int index) {

            //Remove from any groups.
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i].File != null) {
                    var f = File.Files[i].File as Group;
                    if (f != null) {
                        for (int j = 0; j < f.ExtraInfo.Count; j++) {
                            if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.Sound && f.ExtraInfo[j].ItemIndex == index) {
                                f.ExtraInfo.RemoveAt(j);
                            }
                        }
                        if (File.Streams[index].File != null) {
                            for (int j = 0; j < f.SoundFiles.Count; j++) {
                                if (f.SoundFiles[j] != null) {
                                    if (f.SoundFiles[j].FileId == File.Streams[index].File.FileId) {
                                        f.SoundFiles.RemoveAt(j);
                                    }
                                }
                            }
                        }
                        if (File.Streams[index].PrefetchFile != null) {
                            for (int j = 0; j < f.SoundFiles.Count; j++) {
                                if (f.SoundFiles[j] != null) {
                                    if (f.SoundFiles[j].FileId == File.Streams[index].PrefetchFile.FileId) {
                                        f.SoundFiles.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Adjust sound groups.
            if (File.Streams.Count - 1 + File.Sequences.Count + File.WaveSoundDatas.Count <= 0) {
                File.SoundSets.Clear();
            }
            for (int i = 0; i < File.SoundSets.Count; i++) {
                if (File.SoundSets[i].StartIndex >= index && File.SoundSets[i].StartIndex != 0xFFFFFF) {
                    File.SoundSets[i].StartIndex--;
                    if (File.SoundSets[i].StartIndex < 0) {
                        File.SoundSets[i].StartIndex = 0;
                    }
                }
                if (File.SoundSets[i].EndIndex >= index && File.SoundSets[i].EndIndex != 0xFFFFFF) {
                    File.SoundSets[i].EndIndex--;
                    if (File.SoundSets[i].EndIndex < 0) {
                        File.SoundSets[i].EndIndex = 0;
                    }
                }
            }

            //Remove files.
            if (File.Streams[index].File != null) {
                if (File.FileUnique(File.Streams[index].File.FileId)) {
                    File.Files.RemoveAt(File.Streams[index].File.FileId);
                }
            }
            if (File.Streams[index].PrefetchFile != null) {
                if (File.FileUnique(File.Streams[index].PrefetchFile.FileId)) {
                    File.Files.RemoveAt(File.Streams[index].PrefetchFile.FileId);
                }
            }

            //Remove stream.
            File.Streams.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveWaveSoundData(int index) {

            //Remove from any groups.
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i].File != null) {
                    var f = File.Files[i].File as Group;
                    if (f != null) {
                        for (int j = 0; j < f.ExtraInfo.Count; j++) {
                            if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.Sound && f.ExtraInfo[j].ItemIndex == index + File.Streams.Count) {
                                f.ExtraInfo.RemoveAt(j);
                            }
                        }
                        if (File.WaveSoundDatas[index].File != null) {
                            for (int j = 0; j < f.SoundFiles.Count; j++) {
                                if (f.SoundFiles[j] != null) {
                                    if (f.SoundFiles[j].FileId == File.WaveSoundDatas[index].File.FileId) {
                                        f.SoundFiles.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Adjust sound groups.
            if (File.Streams.Count - 1 + File.Sequences.Count + File.WaveSoundDatas.Count <= 0) {
                File.SoundSets.Clear();
            }
            for (int i = 0; i < File.SoundSets.Count; i++) {
                if (File.SoundSets[i].StartIndex >= index && File.SoundSets[i].StartIndex != 0xFFFFFF) {
                    File.SoundSets[i].StartIndex--;
                    if (File.SoundSets[i].StartIndex < 0) {
                        File.SoundSets[i].StartIndex = 0;
                    }
                }
                if (File.SoundSets[i].EndIndex >= index && File.SoundSets[i].EndIndex != 0xFFFFFF) {
                    File.SoundSets[i].EndIndex--;
                    if (File.SoundSets[i].EndIndex < 0) {
                        File.SoundSets[i].EndIndex = 0;
                    }
                }
            }

            //Remove files.
            if (File.WaveSoundDatas[index].File != null) {
                if (File.FileUnique(File.WaveSoundDatas[index].File.FileId)) {
                    File.Files.RemoveAt(File.WaveSoundDatas[index].File.FileId);
                }
            }

            //Remove entry.
            File.WaveSoundDatas.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveSequence(int index) {

            //Remove from any groups.
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i].File != null) {
                    var f = File.Files[i].File as Group;
                    if (f != null) {
                        for (int j = 0; j < f.ExtraInfo.Count; j++) {
                            if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.Sound && f.ExtraInfo[j].ItemIndex == index + File.Streams.Count + File.WaveSoundDatas.Count) {
                                f.ExtraInfo.RemoveAt(j);
                            }
                        }
                        if (File.Sequences[index].File != null) {
                            for (int j = 0; j < f.SoundFiles.Count; j++) {
                                if (f.SoundFiles[j] != null) {
                                    if (f.SoundFiles[j].FileId == File.Sequences[index].File.FileId) {
                                        f.SoundFiles.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Adjust sound groups.
            if (File.Streams.Count - 1 + File.Sequences.Count + File.WaveSoundDatas.Count <= 0) {
                File.SoundSets.Clear();
            }
            for (int i = 0; i < File.SoundSets.Count; i++) {
                if (File.SoundSets[i].StartIndex >= index && File.SoundSets[i].StartIndex != 0xFFFFFF) {
                    File.SoundSets[i].StartIndex--;
                    if (File.SoundSets[i].StartIndex < 0) {
                        File.SoundSets[i].StartIndex = 0;
                    }
                }
                if (File.SoundSets[i].EndIndex >= index && File.SoundSets[i].EndIndex != 0xFFFFFF) {
                    File.SoundSets[i].EndIndex--;
                    if (File.SoundSets[i].EndIndex < 0) {
                        File.SoundSets[i].EndIndex = 0;
                    }
                }
            }

            //Remove files.
            if (File.Sequences[index].File != null) {
                if (File.FileUnique(File.Sequences[index].File.FileId)) {
                    File.Files.RemoveAt(File.Sequences[index].File.FileId);
                }
            }

            //Remove entry.
            File.Sequences.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveSoundGroup(int index) {

            //Remove from any groups.
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i].File != null) {
                    var f = File.Files[i].File as Group;
                    if (f != null) {
                        for (int j = 0; j < f.ExtraInfo.Count; j++) {
                            if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.SequenceSetOrWaveData && f.ExtraInfo[j].ItemIndex == index) {
                                f.ExtraInfo.RemoveAt(j);
                            }
                        }
                    }
                }
            }

            //Remove entry.
            File.SoundSets.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveBank(int index) {

            //Make sure it is not used in any sequence entries.
            foreach (var e in File.Sequences) {
                if (e.Banks.Contains(File.Banks[index])) {
                    MessageBox.Show("You can't delete this bank, as it is used by sequence " + e.Name + ".");
                    return;
                }
            }

            //Remove from any groups.
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i].File != null) {
                    var f = File.Files[i].File as Group;
                    if (f != null) {
                        for (int j = 0; j < f.ExtraInfo.Count; j++) {
                            if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.Bank && f.ExtraInfo[j].ItemIndex == index) {
                                f.ExtraInfo.RemoveAt(j);
                            }
                        }
                        if (File.Banks[index].File != null) {
                            for (int j = 0; j < f.SoundFiles.Count; j++) {
                                if (f.SoundFiles[j] != null) {
                                    if (f.SoundFiles[j].FileId == File.Banks[index].File.FileId) {
                                        f.SoundFiles.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Remove file.
            if (File.Banks[index].File != null) {
                if (File.FileUnique(File.Banks[index].File.FileId)) {
                    File.Files.RemoveAt(File.Banks[index].File.FileId);
                }
            }

            //Remove entry.
            File.Banks.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveWaveArchive(int index) {

            //Make sure it is not used in any bank entries.
            foreach (var e in File.Banks) {
                if (e.WaveArchives.Contains(File.WaveArchives[index])) {
                    MessageBox.Show("You can't delete this wave archive, as it is used by wave archive " + e.Name + ".");
                    return;
                }
            }

            //Make sure it is not used in any bank files.
            foreach (var file in File.Files) {
                if (file.File != null) {
                    var f = file.File as SoundBank;
                    if (f != null) {
                        foreach (var w in f.Waves) {
                            if (w.WarIndex == index) {
                                MessageBox.Show("You can't delete this wave archive, as it is used inside bank file " + file.FileName + "." + file.FileExtension + ".");
                                return;
                            }
                        }
                    }
                }
            }

            //Make sure it is not used in any WSD files.
            foreach (var file in File.Files) {
                if (file.File != null) {
                    var f = file.File as WaveSoundData;
                    if (f != null) {
                        foreach (var w in f.Waves) {
                            if (w.WarIndex == index) {
                                MessageBox.Show("You can't delete this wave archive, as it is used inside wave sound data file " + file.FileName + "." + file.FileExtension + ".");
                                return;
                            }
                        }
                    }
                }
            }

            //Remove from any groups.
            for (int i = 0; i < File.Files.Count; i++) {
                if (File.Files[i].File != null) {
                    var f = File.Files[i].File as Group;
                    if (f != null) {
                        for (int j = 0; j < f.ExtraInfo.Count; j++) {
                            if (f.ExtraInfo[j].ItemType == InfoExEntry.EItemType.WaveArchive && f.ExtraInfo[j].ItemIndex == index) {
                                f.ExtraInfo.RemoveAt(j);
                            }
                        }
                        if (File.WaveArchives[index].File != null) {
                            for (int j = 0; j < f.SoundFiles.Count; j++) {
                                if (f.SoundFiles[j] != null) {
                                    if (f.SoundFiles[j].FileId == File.WaveArchives[index].File.FileId) {
                                        f.SoundFiles.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Remove file.
            if (File.WaveArchives[index].File != null) {
                if (File.FileUnique(File.WaveArchives[index].File.FileId)) {
                    File.Files.RemoveAt(File.WaveArchives[index].File.FileId);
                }
            }

            //Remove entry.
            File.WaveArchives.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemoveGroup(int index) {

            //Remove file.
            if (File.Groups[index].File != null) {
                if (File.FileUnique(File.Groups[index].File.FileId)) {
                    File.Files.RemoveAt(File.Groups[index].File.FileId);
                }
            }

            //Remove.
            File.Groups.RemoveAt(index);

        }

        /// <summary>
        /// Remove the entry at the index.
        /// </summary>
        /// <param name="index">Index to remove entry at.</param>
        public void RemovePlayer(int index) {

            //Make sure no sounds use the player.
            foreach (var e in File.Sequences) {
                if (e.Player == File.Players[index]) {
                    MessageBox.Show("You can't delete this player, as it is used by sequence " + e.Name + ".");
                }
            }
            foreach (var e in File.Streams) {
                if (e.Player == File.Players[index]) {
                    MessageBox.Show("You can't delete this player, as it is used by stream " + e.Name + ".");
                }
            }
            foreach (var e in File.WaveSoundDatas) {
                if (e.Player == File.Players[index]) {
                    MessageBox.Show("You can't delete this player, as it is used by wave sound data entry " + e.Name + ".");
                }
            }

            //Remove.
            File.Players.RemoveAt(index);

        }

        #endregion

        //Players.
        #region Players

        public override void SarSeqPlay_Click(object sender, EventArgs e) {
            bool unique = !EntryPlayer.CurrHash.Equals("SEQ_" + tree.SelectedNode.Index);
            if (unique) {
                EntryPlayer.PlaySequence(File, tree.SelectedNode.Index);
            } else if (EntryPlayer.Paused) {
                EntryPlayer.Resume();
            } else {
                EntryPlayer.PlaySequence(File, tree.SelectedNode.Index);
            }
        }

        public override void SarWsdPlay_Click(object sender, EventArgs e) {
            bool unique = !EntryPlayer.CurrHash.Equals("WSD_" + tree.SelectedNode.Index);
            if (unique) {
                EntryPlayer.PlayWsd(File, tree.SelectedNode.Index);
            } else if (EntryPlayer.Paused) {
                EntryPlayer.Resume();
            } else {
                EntryPlayer.PlayWsd(File, tree.SelectedNode.Index);
            }
        }

        public override void StmPlay_Click(object sender, EventArgs e) {
            bool unique = !EntryPlayer.CurrHash.Equals("STM_" + tree.SelectedNode.Index);
            if (unique) {
                EntryPlayer.PlayStream(File, tree.SelectedNode.Index);
            } else if (EntryPlayer.Paused) {
                EntryPlayer.Resume();
            } else {
                EntryPlayer.PlayStream(File, tree.SelectedNode.Index);
            }
        }

        public override void SarSeqPause_Click(object sender, EventArgs e) {
            if (EntryPlayer.Playing) {
                EntryPlayer.Pause();
            }
        }

        public override void SarWsdPause_Click(object sender, EventArgs e) {
            if (EntryPlayer.Playing) {
                EntryPlayer.Pause();
            }
        }

        public override void StmPause_Click(object sender, EventArgs e) {
            if (EntryPlayer.Playing) {
                EntryPlayer.Pause();
            }
        }

        public override void SarSeqStop_Click(object sender, EventArgs e) {
            EntryPlayer.Stop();
        }

        public override void SarWsdStop_Click(object sender, EventArgs e) {
            EntryPlayer.Stop();
        }

        public override void StmStop_Click(object sender, EventArgs e) {
            EntryPlayer.Stop();
        }

        public override void OnClosing() {

            //Kill the player.
            EntryPlayer.Kill = true;

        }

        #endregion

    }

}