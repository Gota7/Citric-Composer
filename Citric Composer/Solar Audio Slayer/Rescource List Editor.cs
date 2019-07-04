using Microsoft.VisualBasic;
using SolarFileLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolarAudioSlayer {
    public partial class RescourceListEditor : EditorBase {

        /// <summary>
        /// New rescource editor.
        /// </summary>
        /// <param name="mainWindow">Main window.</param>
        public RescourceListEditor(MainWindow mainWindow) : base(typeof(AudioRescourceList), "Rescource List", "arslist", "Rescource List Editor", mainWindow) {
            InitializeComponent();
            Text = "Rescource List Editor";
            //ICON.
        }

        //Update nodes.
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

            //Add rescource list if needed.
            if (tree.Nodes.Count < 2) {
                tree.Nodes.Add("rescources", "Rescources", 7, 7);
            }

            //If file open.
            if (FileOpen) {

                //Root menu.
                tree.Nodes["rescources"].ContextMenuStrip = rootMenu;

                //Add each rescource.
                int rCount = 0;
                foreach (var r in (File as AudioRescourceList).FileNames) {

                    //Add node.
                    tree.Nodes["rescources"].Nodes.Add("rescource" + rCount, "[" + rCount + "] " + r, 4, 4);

                    //Context menu.
                    tree.Nodes["rescources"].Nodes["rescource" + rCount].ContextMenuStrip = CreateMenuStrip(nodeMenu, new int[] { 0, 1, 2, 3, 4, 8 }, new EventHandler[] {
                        new EventHandler(addAboveToolStripMenuItem1_Click),
                        new EventHandler(addBelowToolStripMenuItem1_Click),
                        new EventHandler(moveUpToolStripMenuItem1_Click),
                        new EventHandler(moveDownToolStripMenuItem1_Click),
                        new EventHandler(blankToolStripMenuItem_Click),
                        new EventHandler(replaceFileToolStripMenuItem_Click),
                        new EventHandler(exportToolStripMenuItem1_Click),
                        new EventHandler(nullifyToolStripMenuItem1_Click),
                        new EventHandler(deleteToolStripMenuItem1_Click)
                    });

                    tree.Nodes["rescources"].Nodes["rescource" + rCount].ContextMenuStrip.Items[4].Text = "Rename";

                    //Add count.
                    rCount++;

                }

            } else {

                //Root menu.
                tree.Nodes["rescources"].ContextMenuStrip = null;

            }

            //End update.
            EndUpdateNodes();

        }

        //Info stuff.
        #region InfoStuff

        /// <summary>
        /// Do info stuff.
        /// </summary>
        public override void DoInfoStuff() {

            //Base.
            base.DoInfoStuff();

            //File open.
            if (FileOpen && File != null) {

                //File info.
                if (tree.SelectedNode == tree.Nodes["fileInfo"]) {

                    //Lst info.
                    barslistFileInfo.BringToFront();
                    barslistFileInfo.Show();

                    //Info.
                    vMajBoxLst.Value = File.Version.Major;
                    vMinBoxLst.Value = File.Version.Minor;
                    lstInternalAssetName.Text = (File as AudioRescourceList).RescourceName;

                    status.Text = "No Valid Info Selected!";

                }

                //Parent is not null.
                else if (tree.SelectedNode.Parent != null) {

                    //No info.
                    noInfoPanel.Show();
                    noInfoPanel.BringToFront();

                    //Rescource.
                    if (tree.SelectedNode.Parent == tree.Nodes["rescources"]) {

                        //Show status.
                        status.Text = "Rescource: " + tree.SelectedNode.Index;

                    } else {

                        //Status.
                        status.Text = "No Valid Info Selected!";

                    }

                } else {

                    //No info.
                    noInfoPanel.Show();
                    noInfoPanel.BringToFront();
                    status.Text = "No Valid Info Selected!";

                }

            }

        }

        /// <summary>
        /// Asset name changed.
        /// </summary>
        public override void BoxLstAssetNameChanged() {
            (File as AudioRescourceList).RescourceName = lstInternalAssetName.Text;
        }

        /// <summary>
        /// Major version changed.
        /// </summary>
        public override void BoxLstMajChanged() {
            File.Version.Major = (byte)vMajBoxLst.Value;
        }

        /// <summary>
        /// Minor version changed.
        /// </summary>
        public override void BoxLstMinChanged() {
            File.Version.Minor = (byte)vMinBoxLst.Value;
        }

        #endregion

        //Node menus.
        #region NodeMenus

        /// <summary>
        /// Root add menu.
        /// </summary>
        public override void RootAdd() {

            //Add file name.
            (File as AudioRescourceList).FileNames.Add("NewRescource.bars");
            UpdateNodes();

        }

        /// <summary>
        /// Add above.
        /// </summary>
        public override void NodeAddAbove() {
            if (FileOpen) {
                (File as AudioRescourceList).FileNames.Insert(tree.SelectedNode.Index, "NewRescource.bars");
                UpdateNodes();
                tree.SelectedNode = tree.Nodes["rescources"].Nodes[tree.SelectedNode.Index + 1];
                DoInfoStuff();
            }
        }
        
        /// <summary>
        /// Add below.
        /// </summary>
        public override void NodeAddBelow() {
            if (FileOpen) {
                (File as AudioRescourceList).FileNames.Insert(tree.SelectedNode.Index + 1, "NewRescource.bars");
                UpdateNodes();
                DoInfoStuff();
            }
        }

        /// <summary>
        /// Move up.
        /// </summary>
        public override void NodeMoveUp() {
            if (FileOpen) {
                if (Swap((File as AudioRescourceList).FileNames, tree.SelectedNode.Index, tree.SelectedNode.Index - 1)) {
                    UpdateNodes();
                    tree.SelectedNode = tree.Nodes["rescources"].Nodes[tree.SelectedNode.Index - 1];
                    DoInfoStuff();
                }
            }
        }

        /// <summary>
        /// Move down.
        /// </summary>
        public override void NodeMoveDown() {
            if (FileOpen) {
                if (Swap((File as AudioRescourceList).FileNames, tree.SelectedNode.Index, tree.SelectedNode.Index + 1)) {
                    UpdateNodes();
                    tree.SelectedNode = tree.Nodes["rescources"].Nodes[tree.SelectedNode.Index + 1];
                    DoInfoStuff();
                }
            }
        }

        /// <summary>
        /// Rename.
        /// </summary>
        public override void NodeBlank() {
            if (FileOpen) {
                string name = Interaction.InputBox("New Name:", "Rename Asset", (File as AudioRescourceList).FileNames[tree.SelectedNode.Index], -1, -1);
                if (name != "") {
                    (File as AudioRescourceList).FileNames[tree.SelectedNode.Index] = name;
                    UpdateNodes();
                    DoInfoStuff();
                }
            }
        }

        /// <summary>
        /// Delete.
        /// </summary>
        public override void NodeDelete() {
            if (FileOpen) {
                (File as AudioRescourceList).FileNames.RemoveAt(tree.SelectedNode.Index);
                int ind = tree.SelectedNode.Index;
                UpdateNodes();
                try {
                    tree.SelectedNode = tree.Nodes["rescources"].Nodes[ind - 1];
                } catch {
                    tree.SelectedNode = tree.Nodes["rescources"];
                }
                DoInfoStuff();
            }
        }

        #endregion

    }
}
