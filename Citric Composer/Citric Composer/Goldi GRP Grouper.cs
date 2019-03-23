using CitraFileLoader;
using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer {
    public partial class Goldi_GRP_Grouper : EditorBase {

        //Versions.
        FileWriter.Version seqVersion = new FileWriter.Version(1, 0, 0);
        FileWriter.Version bnkVersion = new FileWriter.Version(1, 0, 0);
        FileWriter.Version warVersion = new FileWriter.Version(1, 0, 0);
        FileWriter.Version wsdVersion = new FileWriter.Version(1, 0, 0);
        FileWriter.Version stpVersion = new FileWriter.Version(1, 0, 0);

        //Set dependencies.
        bool depsSet;

        public Goldi_GRP_Grouper(MainWindow mainWindow) : base(typeof(Group), "Group", "grp", "Goldi's Grouper", mainWindow) {
            InitializeComponent();
            Text = "Goldi's Grouper";
            Icon = Properties.Resources.Goldi;
        }

        public Goldi_GRP_Grouper(string fileToOpen, MainWindow mainWindow) : base(typeof(Group), "Group", "grp", "Goldi's Grouper", fileToOpen, mainWindow) {
            InitializeComponent();
            Text = "Goldi's Grouper - " + Path.GetFileName(fileToOpen);
            Icon = Properties.Resources.Goldi;

        }

        public Goldi_GRP_Grouper(SoundFile<ISoundFile> fileToOpen, MainWindow mainWindow) : base(typeof(Group), "Group", "grp", "Goldi's Grouper", fileToOpen, mainWindow) {
            InitializeComponent();
            string name = ExtFile.FileName;
            if (name == null) {
                name = "{ Null File Name }";
            }
            Text = EditorName + " - " + name + "." + ExtFile.FileExtension;
            Icon = Properties.Resources.Goldi;

        }

        /// <summary>
        /// Load dependencies if in independent mode since the files are not linked.
        /// </summary>
        public void LoadDependencies() {

            //Check.
            if (MainWindow != null) {
                if (!depsSet && ExtFile == null && MainWindow.file != null) {

                    for (int i = 0; i < (File as Group).SoundFiles.Count; i++) {

                        if ((File as Group).SoundFiles[i].FileId < MainWindow.file.Files.Count()) {
                            if (MainWindow.file.Files[(File as Group).SoundFiles[i].FileId].File == null) {
                                MainWindow.file.Files[(File as Group).SoundFiles[i].FileId].File = (File as Group).SoundFiles[i].File;
                            }
                            if (MainWindow.file.Files[(File as Group).SoundFiles[i].FileId].Reference == null) {
                                (File as Group).SoundFiles[i].Reference = MainWindow.file.Files[(File as Group).SoundFiles[i].FileId];
                                MainWindow.file.Files[(File as Group).SoundFiles[i].FileId].ReferencedBy.Add((File as Group).SoundFiles[i]);
                            }
                        }

                    }
                    depsSet = true;

                }
            }

        }


        //Info and updates.
        #region InfoAndUpdates
        
        /// <summary>
        /// Do info stuff.
        /// </summary>
        public override void DoInfoStuff() {

            //Do pre-info stuff.
            base.DoInfoStuff();

            //Safety check.
            if (!FileOpen || File == null) {
                return;
            }

            //Entry is selected.
            if (tree.SelectedNode.Parent != null) {

                //Dependency.
                if (tree.SelectedNode.Parent == tree.Nodes["dependencies"]) {

                    var d = (File as Group).ExtraInfo[tree.SelectedNode.Index];

                    //Data is null.
                    if (d == null) {

                        //Null.
                        nullDataPanel.BringToFront();
                        nullDataPanel.Show();

                        //Show status.
                        status.Text = "No Valid Info Selected!";

                    }

                    //Data is valid.
                    else {

                        //Panel.
                        grpDependencyPanel.BringToFront();
                        grpDependencyPanel.Show();
                        grpDepLoadFlagsBox.Items.Clear();

                        //Show status.
                        status.Text = "Dependency: " + tree.SelectedNode.Index;

                        //Writing info.
                        WritingInfo = true;

                        //Add first index.
                        grpDepEntryNumComboBox.Items.Clear();
                        grpDepEntryNumComboBox.Items.Add("Unknown Entry");

                        //Set raw entry number.
                        grpDepEntryNumBox.Value = d.ItemIndex;

                        //Set data such as item type, entry combo box, and load flags.
                        switch (d.ItemType) {

                            case InfoExEntry.EItemType.Sequence:

                                //Entry type.
                                grpDepEntryTypeBox.SelectedIndex = 0;

                                //Load flags.
                                grpDepLoadFlagsBox.Items.Add("All");
                                grpDepLoadFlagsBox.Items.Add("Sound And Bank");
                                grpDepLoadFlagsBox.Items.Add("Sound And Wave Archive");
                                grpDepLoadFlagsBox.Items.Add("Bank And Wave Archive");
                                grpDepLoadFlagsBox.Items.Add("Sound Only");
                                grpDepLoadFlagsBox.Items.Add("Bank Only");
                                grpDepLoadFlagsBox.Items.Add("Wave Archive Only");
                                switch (d.LoadFlags) {

                                    case InfoExEntry.ELoadFlags.SeqAndBank:
                                        grpDepLoadFlagsBox.SelectedIndex = 1;
                                        break;

                                    case InfoExEntry.ELoadFlags.SeqAndWarc:
                                        grpDepLoadFlagsBox.SelectedIndex = 2;
                                        break;

                                    case InfoExEntry.ELoadFlags.BankAndWarc:
                                        grpDepLoadFlagsBox.SelectedIndex = 3;
                                        break;

                                    case InfoExEntry.ELoadFlags.Seq:
                                        grpDepLoadFlagsBox.SelectedIndex = 4;
                                        break;

                                    case InfoExEntry.ELoadFlags.Bank:
                                        grpDepLoadFlagsBox.SelectedIndex = 5;
                                        break;

                                    case InfoExEntry.ELoadFlags.Warc:
                                        grpDepLoadFlagsBox.SelectedIndex = 6;
                                        break;

                                    default:
                                        grpDepLoadFlagsBox.SelectedIndex = 0;
                                        break;

                                }

                                //Get entry.
                                if (MainWindow != null) {
                                    if (MainWindow.file != null || ExtFile != null) {

                                        for (int i = 0; i < MainWindow.file.Streams.Count; i++) {
                                            if (MainWindow.file.Streams[i] == null) {
                                                grpDepEntryNumComboBox.Items.Add("[" + i + "] { Null Entry }");
                                                continue;
                                            }
                                            string name = MainWindow.file.Streams[i].Name;
                                            if (name == null) {
                                                name = "{ Null Name }";
                                            }
                                            grpDepEntryNumComboBox.Items.Add("[" + i + "] " + name);
                                        }
                                        for (int i = 0; i < MainWindow.file.WaveSoundDatas.Count; i++) {
                                            if (MainWindow.file.WaveSoundDatas[i] == null) {
                                                grpDepEntryNumComboBox.Items.Add("[" + (i + MainWindow.file.Streams.Count) + "] { Null Entry }");
                                                continue;
                                            }
                                            string name = MainWindow.file.WaveSoundDatas[i].Name;
                                            if (name == null) {
                                                name = "{ Null Name }";
                                            }
                                            grpDepEntryNumComboBox.Items.Add("[" + (i + MainWindow.file.Streams.Count) + "] " + name);
                                        }
                                        for (int i = 0; i < MainWindow.file.Sequences.Count; i++) {
                                            if (MainWindow.file.Sequences[i] == null) {
                                                grpDepEntryNumComboBox.Items.Add("[" + (i + MainWindow.file.WaveSoundDatas.Count + MainWindow.file.Streams.Count) + "] { Null Entry }");
                                                continue;
                                            }
                                            string name = MainWindow.file.Sequences[i].Name;
                                            if (name == null) {
                                                name = "{ Null Name }";
                                            }
                                            grpDepEntryNumComboBox.Items.Add("[" + (i + MainWindow.file.WaveSoundDatas.Count + MainWindow.file.Streams.Count) + "] " + name);
                                        }

                                        //Select value.
                                        try {
                                            grpDepEntryNumComboBox.SelectedIndex = d.ItemIndex + 1;
                                        } catch { grpDepEntryNumComboBox.SelectedIndex = 0; }

                                    }
                                }

                                break;

                            case InfoExEntry.EItemType.SequenceSetOrWaveData:

                                //Entry type.
                                grpDepEntryTypeBox.SelectedIndex = 1;

                                //Get entry.
                                if (MainWindow != null) {
                                    if (MainWindow.file != null || ExtFile != null) {

                                        for (int i = 0; i < MainWindow.file.SoundSets.Count; i++) {
                                            if (MainWindow.file.SoundSets[i] == null) {
                                                grpDepEntryNumComboBox.Items.Add("[" + i + "] { Null Entry }");
                                                continue;
                                            }
                                            string name = MainWindow.file.SoundSets[i].Name;
                                            if (name == null) {
                                                name = "{ Null Name }";
                                            }
                                            grpDepEntryNumComboBox.Items.Add("[" + i + "] " + name);
                                        }

                                        //Select value.
                                        try {
                                            grpDepEntryNumComboBox.SelectedIndex = d.ItemIndex + 1;
                                        } catch { grpDepEntryNumComboBox.SelectedIndex = 0; }

                                    }
                                }

                                //Get flags.
                                grpDepLoadFlagsBox.Items.Add("All");
                                grpDepLoadFlagsBox.Items.Add("Sequence Set And Bank");
                                grpDepLoadFlagsBox.Items.Add("Sequence Set And Wave Archive");
                                grpDepLoadFlagsBox.Items.Add("Bank And Wave Archive");
                                grpDepLoadFlagsBox.Items.Add("Sequence Set Only");
                                grpDepLoadFlagsBox.Items.Add("Bank Only");
                                grpDepLoadFlagsBox.Items.Add("Wave Archive Only");
                                grpDepLoadFlagsBox.Items.Add("Wave Sound Data Set Only");
                                switch (d.LoadFlags) {

                                    case InfoExEntry.ELoadFlags.SeqAndBank:
                                        grpDepLoadFlagsBox.SelectedIndex = 1;
                                        break;

                                    case InfoExEntry.ELoadFlags.SeqAndWarc:
                                        grpDepLoadFlagsBox.SelectedIndex = 2;
                                        break;

                                    case InfoExEntry.ELoadFlags.BankAndWarc:
                                        grpDepLoadFlagsBox.SelectedIndex = 3;
                                        break;

                                    case InfoExEntry.ELoadFlags.Seq:
                                        grpDepLoadFlagsBox.SelectedIndex = 4;
                                        break;

                                    case InfoExEntry.ELoadFlags.Bank:
                                        grpDepLoadFlagsBox.SelectedIndex = 5;
                                        break;

                                    case InfoExEntry.ELoadFlags.Warc:
                                        grpDepLoadFlagsBox.SelectedIndex = 6;
                                        break;

                                    case InfoExEntry.ELoadFlags.Wsd:
                                        grpDepLoadFlagsBox.SelectedIndex = 7;
                                        break;

                                    default:
                                        grpDepLoadFlagsBox.SelectedIndex = 0;
                                        break;

                                }

                                break;

                            case InfoExEntry.EItemType.Bank:

                                //Entry type.
                                grpDepEntryTypeBox.SelectedIndex = 2;

                                //Load flags.
                                grpDepLoadFlagsBox.Items.Add("All");
                                grpDepLoadFlagsBox.Items.Add("Bank Only");
                                grpDepLoadFlagsBox.Items.Add("Wave Archive Only");
                                switch (d.LoadFlags) {

                                    case InfoExEntry.ELoadFlags.Bank:
                                        grpDepLoadFlagsBox.SelectedIndex = 1;
                                        break;

                                    case InfoExEntry.ELoadFlags.Warc:
                                        grpDepLoadFlagsBox.SelectedIndex = 2;
                                        break;

                                    default:
                                        grpDepLoadFlagsBox.SelectedIndex = 0;
                                        break;

                                }

                                //Get entry.
                                if (MainWindow != null) {
                                    if (MainWindow.file != null || ExtFile != null) {

                                        for (int i = 0; i < MainWindow.file.Banks.Count; i++) {
                                            if (MainWindow.file.Banks[i] == null) {
                                                grpDepEntryNumComboBox.Items.Add("[" + i + "] { Null Entry }");
                                                continue;
                                            }
                                            string name = MainWindow.file.Banks[i].Name;
                                            if (name == null) {
                                                name = "{ Null Name }";
                                            }
                                            grpDepEntryNumComboBox.Items.Add("[" + i + "] " + name);
                                        }

                                        //Select value.
                                        try {
                                            grpDepEntryNumComboBox.SelectedIndex = d.ItemIndex + 1;
                                        } catch { grpDepEntryNumComboBox.SelectedIndex = 0; }

                                    }
                                }

                                break;

                            case InfoExEntry.EItemType.WaveArchive:

                                //Entry type.
                                grpDepEntryTypeBox.SelectedIndex = 3;

                                //Load flags.
                                grpDepLoadFlagsBox.Items.Add("All");
                                grpDepLoadFlagsBox.SelectedIndex = 0;

                                //Get entry.
                                if (MainWindow != null) {
                                    if (MainWindow.file != null || ExtFile != null) {

                                        for (int i = 0; i < MainWindow.file.WaveArchives.Count; i++) {
                                            if (MainWindow.file.WaveArchives[i] == null) {
                                                grpDepEntryNumComboBox.Items.Add("[" + i + "] { Null Entry }");
                                                continue;
                                            }
                                            string name = MainWindow.file.WaveArchives[i].Name;
                                            if (name == null) {
                                                name = "{ Null Name }";
                                            }
                                            grpDepEntryNumComboBox.Items.Add("[" + i + "] " + name);
                                        }

                                        //Select value.
                                        try {
                                            grpDepEntryNumComboBox.SelectedIndex = d.ItemIndex + 1;
                                        } catch { grpDepEntryNumComboBox.SelectedIndex = 0; }

                                    }
                                }

                                break;

                        }

                        //Done writing indo.
                        WritingInfo = false;

                    }

                }

                //File.
                else {

                    Group g = File as Group;

                    //File is null.
                    if (g.SoundFiles[tree.SelectedNode.Index] == null) {

                        //Show null.
                        nullDataPanel.BringToFront();
                        nullDataPanel.Show();

                    }

                    //Valid.
                    else {

                        //Show file stuff.
                        grpFilePanel.BringToFront();
                        grpFilePanel.Show();

                        //Writing info.
                        WritingInfo = true;

                        //Embed mode.
                        grpEmbedModeBox.SelectedIndex = g.SoundFiles[tree.SelectedNode.Index].Embed ? 1 : 0;

                        //File id.
                        int fileId = g.SoundFiles[tree.SelectedNode.Index].FileId;
                        grpFileIdBox.Value = fileId;

                        //Get file names.
                        grpFileIdComboBox.Items.Clear();
                        grpFileIdComboBox.Items.Add("Unknown File Entry");

                        //File is open.
                        if (MainWindow != null) {
                            if (MainWindow.file != null || ExtFile != null) {

                                //Add entries.
                                int index = 0;
                                foreach (var f in MainWindow.file.Files) {

                                    if (f == null) {
                                        grpFileIdComboBox.Items.Add("{ NULL }");
                                    } else {
                                        string name = f.FileName;
                                        if (name == null) { name = "{ Null File Name }"; }
                                        grpFileIdComboBox.Items.Add("[" + index + "] " + name + "." + f.FileExtension);
                                    }

                                    index++;

                                }

                                try {
                                    grpFileIdComboBox.SelectedIndex = fileId + 1;
                                } catch { }

                            } else {
                                grpFileIdComboBox.SelectedIndex = 0;
                            }
                        }

                        //No file open.
                        else {
                            grpFileIdComboBox.SelectedIndex = 0;
                        }

                        //Done.
                        WritingInfo = false;

                    }

                    //Show status.
                    status.Text = "File Entry: " + tree.SelectedNode.Index;

                }

            }

            //File info.
            else if (tree.SelectedNode == tree.Nodes["fileInfo"]) {

                //Group file info panel.
                grpFileInfoPanel.BringToFront();
                grpFileInfoPanel.Show();

                //Set values.
                WritingInfo = true;
                grpMajBox.Value = (File as Group).Version.Major;
                grpMinBox.Value = (File as Group).Version.Minor;
                grpRevBox.Value = (File as Group).Version.Revision;

                grpSeqMajBox.Value = seqVersion.Major;
                grpSeqMinBox.Value = seqVersion.Minor;
                grpSeqRevBox.Value = seqVersion.Revision;

                grpBnkMajBox.Value = bnkVersion.Major;
                grpBnkMinBox.Value = bnkVersion.Minor;
                grpBnkRevBox.Value = bnkVersion.Revision;

                grpWarMajBox.Value = warVersion.Major;
                grpWarMinBox.Value = warVersion.Minor;
                grpWarRevBox.Value = warVersion.Revision;

                grpWsdMajBox.Value = wsdVersion.Major;
                grpWsdMinBox.Value = wsdVersion.Minor;
                grpWsdRevBox.Value = wsdVersion.Revision;

                grpStpMajBox.Value = stpVersion.Major;
                grpStpMinBox.Value = stpVersion.Minor;
                grpStpRevBox.Value = stpVersion.Revision;
                WritingInfo = false;

                //Status.
                status.Text = "File Information.";

            }

            //Dependencies folder.
            else if (tree.SelectedNode == tree.Nodes["dependencies"]) {

                status.Text = "Dependency Count: " + (File as Group).ExtraInfo.Count;

            }

            //Files, as in folder.
            else {

                //No info.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

                //Show status.
                status.Text = "File Entry Count: " + (File as Group).SoundFiles.Count;

            }

        }

        /// <summary>
        /// Update nodes.
        /// </summary>
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

            //Check.
            if (FileOpen) {
                LoadDependencies();
            } else {
                depsSet = false;
            }

            //Add dependencies if doesn't exist.
            if (tree.Nodes.Count < 2) {
                tree.Nodes.Add("dependencies", "Dependencies", 7, 7);
            }

            //Add files if doesn't exist.
            if (tree.Nodes.Count < 3) {
                tree.Nodes.Add("files", "Files", 11, 11);
            }

            //File is open and not null.
            if (FileOpen && File != null) {

                //Root context menu.
                tree.Nodes["dependencies"].ContextMenuStrip = rootMenu;
                tree.Nodes["files"].ContextMenuStrip = rootMenu;

                //Add each dependency.
                Group g = (File as Group);
                for (int i = 0; i < g.ExtraInfo.Count; i++) {

                    //Null entry.
                    if (g.ExtraInfo[i] == null) {

                        //Add null entry.
                        tree.Nodes["dependencies"].Nodes.Add("dependency" + i, "[" + i + "] { Null Dependency }", 0, 0);

                    } else {

                        //Get icon.
                        int icon = 0;
                        switch (g.ExtraInfo[i].ItemType) {

                            //Bank.
                            case InfoExEntry.EItemType.Bank:
                                icon = 5;
                                break;

                            //Sequence.
                            case InfoExEntry.EItemType.Sequence:
                                icon = 3;
                                break;

                            //Sequence set or wave data.
                            case InfoExEntry.EItemType.SequenceSetOrWaveData:
                                icon = 4;
                                break;

                            //Wave archive.
                            case InfoExEntry.EItemType.WaveArchive:
                                icon = 6;
                                break;

                        }

                        //Try to get dependency name.
                        string depName = "{ Unknown Dependency Name }";
                        if (MainWindow != null) {

                            //File is valid.
                            if (MainWindow.file != null || ExtFile != null) {

                                //Safety.
                                try {

                                    //Item type.
                                    switch (g.ExtraInfo[i].ItemType) {

                                        //Sequence.
                                        case InfoExEntry.EItemType.Sequence:

                                            int index = g.ExtraInfo[i].ItemIndex;

                                            //Stream.
                                            if (index < MainWindow.file.Streams.Count) {
                                                icon = 1;
                                                depName = MainWindow.file.Streams[index].Name;
                                            }

                                            //Wave sound data.
                                            else if (index >= MainWindow.file.Streams.Count && index < MainWindow.file.Streams.Count + MainWindow.file.WaveSoundDatas.Count) {
                                                icon = 2;
                                                depName = MainWindow.file.WaveSoundDatas[index - MainWindow.file.Streams.Count].Name;
                                            }

                                            //Sequence.
                                            else if (index >= MainWindow.file.Streams.Count + MainWindow.file.WaveSoundDatas.Count) {
                                                icon = 3;
                                                depName = MainWindow.file.Sequences[index - MainWindow.file.WaveSoundDatas.Count - MainWindow.file.Streams.Count].Name;
                                            }

                                            break;

                                        //Bank.
                                        case InfoExEntry.EItemType.Bank:
                                            depName = MainWindow.file.Banks[g.ExtraInfo[i].ItemIndex].Name;
                                            break;

                                        //Wave archive.
                                        case InfoExEntry.EItemType.WaveArchive:
                                            depName = MainWindow.file.WaveArchives[g.ExtraInfo[i].ItemIndex].Name;
                                            break;

                                        //Sequence set or wave data.
                                        case InfoExEntry.EItemType.SequenceSetOrWaveData:
                                            depName = MainWindow.file.SoundSets[g.ExtraInfo[i].ItemIndex].Name;
                                            break;

                                    }

                                } catch { }

                                if (depName == null) { depName = "{ Null Name }"; }

                            }

                        }

                        //Add entry.
                        tree.Nodes["dependencies"].Nodes.Add("dependency" + i, "[" + i + "] " + depName, icon, icon);

                    }

                    //Add context menu.
                    tree.Nodes["dependencies"].Nodes["dependency" + i].ContextMenuStrip = CreateMenuStrip(nodeMenu, new int[] { 0, 1, 2, 3, 4, 7, 8 }, new EventHandler[] { new EventHandler(this.addAboveToolStripMenuItem1_Click), new EventHandler(this.addBelowToolStripMenuItem1_Click), new EventHandler(this.moveUpToolStripMenuItem1_Click), new EventHandler(this.moveDownToolStripMenuItem1_Click), new EventHandler(blankToolStripMenuItem_Click), new EventHandler(replaceFileToolStripMenuItem_Click), new EventHandler(exportToolStripMenuItem1_Click), new EventHandler(nullifyToolStripMenuItem1_Click), new EventHandler(deleteToolStripMenuItem1_Click) });

                }

                //Load files.
                int fCount = 0;
                foreach (var f in g.SoundFiles) {

                    //Null.
                    if (f == null) {
                        tree.Nodes["files"].Nodes.Add("file" + fCount, "[" + fCount + "] " + "{ Null File }", 0, 0);
                    }

                    //Valid.
                    else {

                        //File is valid.
                        if (f.File != null) {

                            //Get version.
                            switch (f.File.GetExtension().Substring(f.File.GetExtension().Length - 3, 3).ToLower()) {

                                case "seq":
                                    seqVersion.Major = (f.File as SoundSequence).Version.Major;
                                    seqVersion.Minor = (f.File as SoundSequence).Version.Minor;
                                    seqVersion.Revision = (f.File as SoundSequence).Version.Revision;
                                    break;

                                case "bnk":
                                    bnkVersion.Major = (f.File as SoundBank).Version.Major;
                                    bnkVersion.Minor = (f.File as SoundBank).Version.Minor;
                                    bnkVersion.Revision = (f.File as SoundBank).Version.Revision;
                                    break;

                                case "war":
                                    warVersion.Major = (f.File as SoundWaveArchive).Version.Major;
                                    warVersion.Minor = (f.File as SoundWaveArchive).Version.Minor;
                                    warVersion.Revision = (f.File as SoundWaveArchive).Version.Revision;
                                    break;

                                case "wsd":
                                    wsdVersion.Major = (f.File as WaveSoundData).Version.Major;
                                    wsdVersion.Minor = (f.File as WaveSoundData).Version.Minor;
                                    wsdVersion.Revision = (f.File as WaveSoundData).Version.Revision;
                                    break;

                                case "stp":
                                    stpVersion.Major = (f.File as PrefetchFile).Version.Major;
                                    stpVersion.Minor = (f.File as PrefetchFile).Version.Minor;
                                    stpVersion.Revision = (f.File as PrefetchFile).Version.Revision;
                                    break;

                            }

                        }

                        string name = f.FileName;
                        if (name == null) {
                            name = "{ Null File Name }";

                            //Try and get name.
                            if (MainWindow != null) {
                                if (MainWindow.file != null || ExtFile != null) {

                                    //Try.
                                    try {

                                        //Set name.
                                        if (MainWindow.file.Files[f.FileId].FileName != null) {
                                            name = MainWindow.file.Files[f.FileId].FileName;
                                        }

                                    } catch { }

                                }
                            }

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
                        string type = "(Embedded)";
                        if (!f.Embed) {
                            type = "(Linked)";
                        }

                        tree.Nodes["files"].Nodes.Add("file" + fCount, "[" + fCount + "] " + name + " " + type, icon, icon);
                    }

                    //Add context menu.
                    tree.Nodes["files"].Nodes["file" + fCount].ContextMenuStrip = CreateMenuStrip(nodeMenu, new int[] { 0, 1, 2, 3, 5, 6, 8 }, new EventHandler[] { new EventHandler(this.addAboveToolStripMenuItem1_Click), new EventHandler(this.addBelowToolStripMenuItem1_Click), new EventHandler(this.moveUpToolStripMenuItem1_Click), new EventHandler(this.moveDownToolStripMenuItem1_Click), new EventHandler(blankToolStripMenuItem_Click), new EventHandler(replaceFileToolStripMenuItem_Click), new EventHandler(exportToolStripMenuItem1_Click), new EventHandler(nullifyToolStripMenuItem1_Click), new EventHandler(deleteToolStripMenuItem1_Click) });

                    //Type is STP context menu.
                    if (MainWindow != null) {
                        if (MainWindow.file != null && f.FileExtension.EndsWith("stp")) {

                            //Add extra entry.
                            tree.Nodes["files"].Nodes["file" + fCount].ContextMenuStrip.Items.Insert(4, new ToolStripMenuItem("Generate From Stream", treeIcons.Images[1], new EventHandler(this.GenerateFromStreamClick)));

                        }
                    }

                    //Increment count.
                    fCount++;

                }

            } else {

                //Remove context menus.
                tree.Nodes["dependencies"].ContextMenuStrip = null;
                tree.Nodes["files"].ContextMenuStrip = null;

            }

            //End update.
            EndUpdateNodes();

        }

        /// <summary>
        /// Node is double clicked.
        /// </summary>
        public override void NodeMouseDoubleClick() {

            //Safety check.
            if (!FileOpen || File == null) {
                return;
            }

            //Make sure node is valid.
            if (tree.SelectedNode.Parent != null) {

                //Editor to open.
                EditorBase b = null;

                //If the parent is a file.
                Group g = File as Group;
                if (tree.SelectedNode.Parent == tree.Nodes["files"]) {

                    //Embedded.
                    if (g.SoundFiles[tree.SelectedNode.Index].Embed || (ExtFile != null && g.SoundFiles[tree.SelectedNode.Index].File != null)) {

                        //Get the extension.
                        switch (g.SoundFiles[tree.SelectedNode.Index].FileExtension.Substring(g.SoundFiles[tree.SelectedNode.Index].FileExtension.Length - 3, 3).ToLower()) {

                            //Wave archive.
                            case "war":
                                b = new Brewster_WAR_Brewer(g.SoundFiles[tree.SelectedNode.Index], null, this);
                                break;

                        }

                    }

                    //Linked.
                    else {
                        MessageBox.Show("A linked file has no file data that can be read editing a group in independent mode!", "Notice:");
                    }

                }

                //Open the editor.
                if (b != null) {
                    b.Show();
                }

            }

        }

        #endregion


        //Forcing versions or updating.
        #region ForcingVersions

        /// <summary>
        /// Version changed.
        /// </summary>
        public override void GroupVersionChanged() {

            //Set version variables.
            (File as Group).Version.Major = (byte)grpMajBox.Value;
            (File as Group).Version.Minor = (byte)grpMinBox.Value;
            (File as Group).Version.Revision = (byte)grpRevBox.Value;

            seqVersion.Major = (byte)grpSeqMajBox.Value;
            seqVersion.Minor = (byte)grpSeqMinBox.Value;
            seqVersion.Revision = (byte)grpSeqRevBox.Value;

            bnkVersion.Major = (byte)grpBnkMajBox.Value;
            bnkVersion.Minor = (byte)grpBnkMinBox.Value;
            bnkVersion.Revision = (byte)grpBnkRevBox.Value;

            warVersion.Major = (byte)grpWarMajBox.Value;
            warVersion.Minor = (byte)grpWarMinBox.Value;
            warVersion.Revision = (byte)grpWarRevBox.Value;

            wsdVersion.Major = (byte)grpWsdMajBox.Value;
            wsdVersion.Minor = (byte)grpWsdMinBox.Value;
            wsdVersion.Revision = (byte)grpWsdRevBox.Value;

            stpVersion.Major = (byte)grpStpMajBox.Value;
            stpVersion.Minor = (byte)grpStpMinBox.Value;
            stpVersion.Revision = (byte)grpStpRevBox.Value;

        }

        /// <summary>
        /// Force version.
        /// </summary>
        public override void GroupForceSequenceVersion() {

            Group g = (File as Group);
            for (int i = 0; i < g.SoundFiles.Count; i++) {

                //Not null.
                if (g.SoundFiles[i] != null) {

                    if (g.SoundFiles[i].File != null) {
                        if (g.SoundFiles[i].File as SoundSequence != null) {
                            (g.SoundFiles[i].File as SoundSequence).Version = seqVersion;
                        }
                    }

                }

            }

        }

        /// <summary>
        /// Force version.
        /// </summary>
        public override void GroupForceBankVersion() {

            Group g = (File as Group);
            for (int i = 0; i < g.SoundFiles.Count; i++) {

                //Not null.
                if (g.SoundFiles[i] != null) {

                    if (g.SoundFiles[i].File != null) {
                        if (g.SoundFiles[i].File as SoundBank != null) {
                            (g.SoundFiles[i].File as SoundBank).Version = bnkVersion;
                        }
                    }

                }

            }

        }

        /// <summary>
        /// Force version.
        /// </summary>
        public override void GroupForceWaveArchiveVersion() {

            Group g = (File as Group);
            for (int i = 0; i < g.SoundFiles.Count; i++) {

                //Not null.
                if (g.SoundFiles[i] != null) {

                    if (g.SoundFiles[i].File != null) {
                        if (g.SoundFiles[i].File as SoundWaveArchive != null) {
                            (g.SoundFiles[i].File as SoundWaveArchive).Version = warVersion;
                        }
                    }

                }

            }

        }

        /// <summary>
        /// Force version.
        /// </summary>
        public override void GroupForceWaveSoundDataVersion() {

            Group g = (File as Group);
            for (int i = 0; i < g.SoundFiles.Count; i++) {

                //Not null.
                if (g.SoundFiles[i] != null) {

                    if (g.SoundFiles[i].File != null) {
                        if (g.SoundFiles[i].File as WaveSoundData != null) {
                            (g.SoundFiles[i].File as WaveSoundData).Version = wsdVersion;
                        }
                    }

                }

            }

        }

        /// <summary>
        /// Force version.
        /// </summary>
        public override void GroupForcePrefetchVersion() {

            Group g = (File as Group);
            for (int i = 0; i < g.SoundFiles.Count; i++) {

                //Not null.
                if (g.SoundFiles[i] != null) {

                    if (g.SoundFiles[i].File != null) {
                        if (g.SoundFiles[i].File as PrefetchFile != null) {
                            (g.SoundFiles[i].File as PrefetchFile).Version = stpVersion;
                        }
                    }

                }

            }

        }

        #endregion


        //File info.
        #region FileInfo

        public override void GroupFileIdEmbedModeChanged() {

            if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {
                (File as Group).SoundFiles[tree.SelectedNode.Index].Embed = grpEmbedModeBox.SelectedIndex > 0;

                //File cannot be null if embedded mode.
                if (grpEmbedModeBox.SelectedIndex > 0 && (File as Group).SoundFiles[tree.SelectedNode.Index].File == null) {

                    //Open any file
                    OpenFileDialog o = new OpenFileDialog();
                    o.RestoreDirectory = true;
                    o.Filter = "Any Sound File|*.*";
                    o.ShowDialog();

                    if (o.FileName != "") {
                        (File as Group).SoundFiles[tree.SelectedNode.Index].File = SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(o.FileName));
                    } else {
                        (File as Group).SoundFiles[tree.SelectedNode.Index].Embed = false;
                    }

                }

                DoInfoStuff();
                UpdateNodes();

            }

        }

        public override void GroupFileIdComboChanged() {

            if (grpFileIdComboBox.SelectedIndex != 0 && (File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                    //Linked mode.
                    if (MainWindow != null) {
                        if ((MainWindow.file != null || ExtFile != null) && (File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                            //Dereference files.
                            (File as Group).SoundFiles[tree.SelectedNode.Index].Reference.ReferencedBy.Remove((File as Group).SoundFiles[tree.SelectedNode.Index]);

                            //Set new reference.
                            (File as Group).SoundFiles[tree.SelectedNode.Index].Reference = MainWindow.file.Files[grpFileIdComboBox.SelectedIndex - 1];
                            (File as Group).SoundFiles[tree.SelectedNode.Index].Reference.ReferencedBy.Add((File as Group).SoundFiles[tree.SelectedNode.Index]);
                            DoInfoStuff();
                            UpdateNodes();

                        } else if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                            //Just set file id.
                            (File as Group).SoundFiles[tree.SelectedNode.Index].FileId = grpFileIdComboBox.SelectedIndex - 1;
                            DoInfoStuff();
                            UpdateNodes();

                        }
                    }
                    //Independent mode.
                    else if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                        //Just set file id.
                        (File as Group).SoundFiles[tree.SelectedNode.Index].FileId = grpFileIdComboBox.SelectedIndex - 1;
                        DoInfoStuff();
                        UpdateNodes();

                    }

                }

            }

        }

        public override void GroupFileIdNumBoxChanged() {

            if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                //Linked mode.
                if (MainWindow != null) {
                    if ((MainWindow.file != null || ExtFile != null) && (File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                        //Dereference files.
                        (File as Group).SoundFiles[tree.SelectedNode.Index].Reference.ReferencedBy.Remove((File as Group).SoundFiles[tree.SelectedNode.Index]);

                        //Set new reference.
                        (File as Group).SoundFiles[tree.SelectedNode.Index].Reference = MainWindow.file.Files[(int)grpFileIdBox.Value];
                        (File as Group).SoundFiles[tree.SelectedNode.Index].Reference.ReferencedBy.Add((File as Group).SoundFiles[tree.SelectedNode.Index]);
                        DoInfoStuff();
                        UpdateNodes();

                    }

                    //Independent mode.
                    else if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                        //Just set file id.
                        (File as Group).SoundFiles[tree.SelectedNode.Index].FileId = (int)grpFileIdBox.Value;
                        DoInfoStuff();
                        UpdateNodes();

                    }
                }

                //Independent mode.
                else if ((File as Group).SoundFiles[tree.SelectedNode.Index] != null) {

                    //Just set file id.
                    (File as Group).SoundFiles[tree.SelectedNode.Index].FileId = (int)grpFileIdBox.Value;
                    DoInfoStuff();
                    UpdateNodes();

                }

            }

        }

        #endregion


        //Dependency info.
        #region DependencyInfo

        /// <summary>
        /// Change dependency type.
        /// </summary>
        public override void GroupDependencyTypeChanged() {

            InfoExEntry e = (File as Group).ExtraInfo[tree.SelectedNode.Index];
            e.ItemType = (InfoExEntry.EItemType)grpDepEntryTypeBox.SelectedIndex;
            e.LoadFlags = InfoExEntry.ELoadFlags.All;
            DoInfoStuff();
            UpdateNodes();

        }

        /// <summary>
        /// Flags changed.
        /// </summary>
        public override void GroupDependencyFlagsChanged() {

            //Switch type.
            InfoExEntry e = (File as Group).ExtraInfo[tree.SelectedNode.Index];
            switch (e.ItemType) {

                case InfoExEntry.EItemType.Sequence:
                    switch (grpDepLoadFlagsBox.SelectedIndex) {

                        case 1:
                            e.LoadFlags = InfoExEntry.ELoadFlags.SeqAndBank;
                            break;

                        case 2:
                            e.LoadFlags = InfoExEntry.ELoadFlags.SeqAndWarc;
                            break;

                        case 3:
                            e.LoadFlags = InfoExEntry.ELoadFlags.BankAndWarc;
                            break;

                        case 4:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Seq;
                            break;

                        case 5:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Bank;
                            break;

                        case 6:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Warc;
                            break;

                        default:
                            e.LoadFlags = InfoExEntry.ELoadFlags.All;
                            break;

                    }
                    break;

                case InfoExEntry.EItemType.SequenceSetOrWaveData:
                    switch (grpDepLoadFlagsBox.SelectedIndex) {

                        case 1:
                            e.LoadFlags = InfoExEntry.ELoadFlags.SeqAndBank;
                            break;

                        case 2:
                            e.LoadFlags = InfoExEntry.ELoadFlags.SeqAndWarc;
                            break;

                        case 3:
                            e.LoadFlags = InfoExEntry.ELoadFlags.BankAndWarc;
                            break;

                        case 4:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Seq;
                            break;

                        case 5:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Bank;
                            break;

                        case 6:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Warc;
                            break;

                        case 7:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Wsd;
                            break;

                        default:
                            e.LoadFlags = InfoExEntry.ELoadFlags.All;
                            break;

                    }
                    break;

                case InfoExEntry.EItemType.Bank:
                    switch (grpDepLoadFlagsBox.SelectedIndex) {

                        case 1:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Bank;
                            break;

                        case 2:
                            e.LoadFlags = InfoExEntry.ELoadFlags.Warc;
                            break;

                        default:
                            e.LoadFlags = InfoExEntry.ELoadFlags.All;
                            break;

                    }
                    break;

                case InfoExEntry.EItemType.WaveArchive:
                    e.LoadFlags = InfoExEntry.ELoadFlags.All;
                    break;

            }

        }

        /// <summary>
        /// Entry combo box changed.
        /// </summary>
        public override void GroupDependencyEntryComboChanged() {

            if (grpDepEntryNumComboBox.SelectedIndex != 0) {

                //Simply change the index.
                (File as Group).ExtraInfo[tree.SelectedNode.Index].ItemIndex = (int)grpDepEntryNumComboBox.SelectedIndex - 1;
                DoInfoStuff();
                UpdateNodes();

            }

        }

        /// <summary>
        /// Entry box changed.
        /// </summary>
        public override void GroupDependencyEntryNumBoxChanged() {

            //Simply change the index.
            (File as Group).ExtraInfo[tree.SelectedNode.Index].ItemIndex = (int)grpDepEntryNumBox.Value;
            DoInfoStuff();
            UpdateNodes();

        }

        #endregion


        //Node menus.
        #region NodeMenus

        /// <summary>
        /// Add an entry.
        /// </summary>
        public override void RootAdd() {

            //Dependency.
            if (tree.SelectedNode == tree.Nodes["dependencies"]) {

                //Add info.
                (File as Group).ExtraInfo.Add(new InfoExEntry() { LoadFlags = InfoExEntry.ELoadFlags.All });
                UpdateNodes();

            }

            //File.
            else if (tree.SelectedNode == tree.Nodes["files"]) {

                //Sound file.
                SoundFile<ISoundFile> s = new SoundFile<ISoundFile>();
                s.FileId = 0;
                s.Embed = false;

                //Reference.
                if (MainWindow != null) {
                    if (MainWindow.file != null || ExtFile != null) {
                        s.Reference = MainWindow.file.Files[0];
                    }
                }

                //Add file.
                (File as Group).SoundFiles.Add(s);
                UpdateNodes();

            }

        }

        /// <summary>
        /// Add above.
        /// </summary>
        public override void NodeAddAbove() {

            //Dependency.
            if (tree.SelectedNode.Parent == tree.Nodes["dependencies"]) {

                //Add info.
                (File as Group).ExtraInfo.Insert(tree.SelectedNode.Index, new InfoExEntry() { LoadFlags = InfoExEntry.ELoadFlags.All });
                UpdateNodes();
                tree.SelectedNode = tree.Nodes["dependencies"].Nodes[tree.SelectedNode.Index + 1];
                DoInfoStuff();

            }

            //File.
            else {

                //Sound file.
                SoundFile<ISoundFile> s = new SoundFile<ISoundFile>();
                s.FileId = 0;
                s.Embed = false;

                //Reference.
                if (MainWindow != null) {
                    if (MainWindow.file != null || ExtFile != null) {
                        s.Reference = MainWindow.file.Files[0];
                    }
                }

                //Add file.
                (File as Group).SoundFiles.Insert(tree.SelectedNode.Index, s);
                UpdateNodes();
                tree.SelectedNode = tree.Nodes["files"].Nodes[tree.SelectedNode.Index + 1];
                DoInfoStuff();

            }

        }

        /// <summary>
        /// Add below.
        /// </summary>
        public override void NodeAddBelow() {

            //Dependency.
            if (tree.SelectedNode.Parent == tree.Nodes["dependencies"]) {

                //Add info.
                (File as Group).ExtraInfo.Insert(tree.SelectedNode.Index + 1, new InfoExEntry() { LoadFlags = InfoExEntry.ELoadFlags.All });
                UpdateNodes();
                DoInfoStuff();

            }

            //File.
            else {

                //Sound file.
                SoundFile<ISoundFile> s = new SoundFile<ISoundFile>();
                s.FileId = 0;
                s.Embed = false;

                //Reference.
                if (MainWindow != null) {
                    if (MainWindow.file != null || ExtFile != null) {
                        s.Reference = MainWindow.file.Files[0];
                    }
                }

                //Add file.
                (File as Group).SoundFiles.Insert(tree.SelectedNode.Index + 1, s);
                UpdateNodes();
                DoInfoStuff();

            }

        }

        /// <summary>
        /// Move up.
        /// </summary>
        public override void NodeMoveUp() {

            //Dependency.
            if (tree.SelectedNode.Parent == tree.Nodes["dependencies"]) {
                if (Swap((File as Group).ExtraInfo, tree.SelectedNode.Index - 1, tree.SelectedNode.Index)) {
                    UpdateNodes();
                    tree.SelectedNode = tree.Nodes["dependencies"].Nodes[tree.SelectedNode.Index - 1];
                    DoInfoStuff();
                }
            }

            //File.
            else {
                if (Swap((File as Group).SoundFiles, tree.SelectedNode.Index - 1, tree.SelectedNode.Index)) {
                    UpdateNodes();
                    tree.SelectedNode = tree.Nodes["files"].Nodes[tree.SelectedNode.Index - 1];
                    DoInfoStuff();
                }
            }

        }

        /// <summary>
        /// Move down.
        /// </summary>
        public override void NodeMoveDown() {

            //Dependency.
            if (tree.SelectedNode.Parent == tree.Nodes["dependencies"]) {
                if (Swap((File as Group).ExtraInfo, tree.SelectedNode.Index + 1, tree.SelectedNode.Index)) {
                    UpdateNodes();
                    tree.SelectedNode = tree.Nodes["dependencies"].Nodes[tree.SelectedNode.Index + 1];
                    DoInfoStuff();
                }
            }

            //File.
            else {
                if (Swap((File as Group).SoundFiles, tree.SelectedNode.Index + 1, tree.SelectedNode.Index)) {
                    UpdateNodes();
                    tree.SelectedNode = tree.Nodes["files"].Nodes[tree.SelectedNode.Index + 1];
                    DoInfoStuff();
                }
            }

        }

        /// <summary>
        /// Delete.
        /// </summary>
        public override void NodeDelete() {

            //Dependency.
            if (tree.SelectedNode.Parent == tree.Nodes["dependencies"]) {
                (File as Group).ExtraInfo.RemoveAt(tree.SelectedNode.Index);
                UpdateNodes();
                try {
                    tree.SelectedNode = tree.Nodes["dependencies"].Nodes[tree.SelectedNode.Index - 1];
                } catch {
                    tree.SelectedNode = tree.Nodes["dependencies"];
                }
                DoInfoStuff();
            }

            //File.
            else {
                (File as Group).SoundFiles.RemoveAt(tree.SelectedNode.Index);
                UpdateNodes();
                try {
                    tree.SelectedNode = tree.Nodes["files"].Nodes[tree.SelectedNode.Index - 1];
                } catch {
                    tree.SelectedNode = tree.Nodes["files"];
                }
                DoInfoStuff();
            }

        }

        /// <summary>
        /// Export.
        /// </summary>
        public override void NodeExport() {

            //File data is not null.
            var f = (File as Group).SoundFiles[tree.SelectedNode.Index].File;
            WriteMode wM = WriteMode;
            if (f != null) {

                string path = GetFileSaverPath("Sound File", f.GetExtension().Substring(f.GetExtension().Length - 3, 3), ref wM);
                if (path != "") {

                    //Save file.
                    MemoryStream o = new MemoryStream();
                    BinaryDataWriter bw = new BinaryDataWriter(o);

                    //Write the file.
                    f.Write(wM, bw);

                    //Save the file.
                    System.IO.File.WriteAllBytes(path, o.ToArray());

                }

            }

            //Null.
            else {
                MessageBox.Show("You can't export a linked file!");
            }

        }

        /// <summary>
        /// Replace.
        /// </summary>
        public override void NodeReplace() {

            //Open any file
            OpenFileDialog o = new OpenFileDialog();
            o.RestoreDirectory = true;
            o.Filter = "Any Sound File|*.*";
            o.ShowDialog();

            if (o.FileName != "") {
                (File as Group).SoundFiles[tree.SelectedNode.Index].File = SoundArchiveReader.ReadFile(System.IO.File.ReadAllBytes(o.FileName));
                DoInfoStuff();
                UpdateNodes();
            }

        }

        /// <summary>
        /// Blank.
        /// </summary>
        public override void NodeBlank() {
            (File as Group).ExtraInfo[tree.SelectedNode.Index] = new InfoExEntry() { LoadFlags = InfoExEntry.ELoadFlags.All };
            UpdateNodes();
            DoInfoStuff();
        }

        /// <summary>
        /// Nullify.
        /// </summary>
        public override void NodeNullify() {
            (File as Group).ExtraInfo[tree.SelectedNode.Index] = null;
            UpdateNodes();
            DoInfoStuff();
        }

        /// <summary>
        /// Generate stream.
        /// </summary>
        private void GenerateFromStreamClick(object sender, EventArgs e) {

            MessageBox.Show("In Progress!");

        }

        #endregion

    }
}
