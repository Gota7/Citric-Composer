using CitraFileLoader;
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

                //TEMP.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

            }

            //File info.
            else if (tree.SelectedNode == tree.Nodes["fileInfo"]) {

                //TEMP.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

                //Group file info panel.
                //grpFileInfoPanel.BringToFront();
                //grpFileInfoPanel.Show();

            }

            //Dependencies folder.
            else if (tree.SelectedNode == tree.Nodes["dependencies"]) {

                //TEMP.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

            }

            //Files.
            else {

                //TEMP.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

            }

        }

        /// <summary>
        /// Update nodes.
        /// </summary>
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

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

                //Force versions here?

                //Context menu.
                //tree.Nodes["dependencies"].ContextMenuStrip = rootMenu;
                //tree.Nodes["files"].ContextMenuStrip = rootMenu;

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
                            if (MainWindow.file != null && ExtFile != null) {

                                //Safety.
                                try {

                                    //Item type.
                                    switch (g.ExtraInfo[i].ItemType) {

                                        //Sequence.
                                        case InfoExEntry.EItemType.Sequence:
                                            depName = MainWindow.file.Sequences[g.ExtraInfo[i].ItemIndex].Name;
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
                    //tree.Nodes["dependencies"].Nodes["dependency" + i].ContextMenuStrip = nodeMenu;

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
                                    icon = 1;
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

    }
}
