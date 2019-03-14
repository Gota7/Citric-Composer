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
using CitraFileLoader;

namespace Citric_Composer
{
    public partial class MainWindow : Form
    {

        /// <summary>
        /// File open.
        /// </summary>
        public bool fileOpen = false;


        /// <summary>
        /// File name.
        /// </summary>
        public string fileName = "";


        /// <summary>
        /// The main file.
        /// </summary>
        public SoundArchive file;


        public MainWindow()
        {
            InitializeComponent();
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
                file = SoundArchiveReader.ReadSoundArchive(File.ReadAllBytes(openB_sarBox.FileName));
                fileOpen = true;
                fileName = Path.GetFileName(openB_sarBox.FileName);
                Text = "Citric Composer - " + fileName;

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

        private void isabelleSoundEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Isabelle window.
            IsabelleSoundEditor a = new IsabelleSoundEditor();
            a.Show();
        }


        private void brewstersArchiveBrewerWARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Brewster window.
            Brewster_WAR_Brewer a = new Brewster_WAR_Brewer();
            a.Show();
        }

        private void goldisGrouperGRPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Goldi window.
            Goldi_GRP_Grouper a = new Goldi_GRP_Grouper();
            a.Show();
        }

        private void rolfsRescourceResearcherBARSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Rolf window.
            Rolf_Rescource_Researcher r = new Rolf_Rescource_Researcher();
            r.Show();
        }

        #endregion


        //Info panel stuff.
        #region infoPanelStuff

        /// <summary>
        /// Do the mario!
        /// </summary>
        public void doInfoPanelStuff() {

            //Hide all things.
            hideAllThings();

            //If not null.
            if (tree.SelectedNode != null && fileOpen) {

                //Project info.
                if (tree.Nodes["projectInfo"] == tree.SelectedNode) {

                    //Show project info panel.
                    projectInfoPanel.BringToFront();
                    projectInfoPanel.Show();

                    //Update boxes.
                    maxSeqNumBox.Value = file.MaxSequences;
                    maxSeqTrackNumBox.Value = file.MaxSequenceTracks;
                    maxStreamNumBox.Value = file.MaxStreamSounds;
                    maxStreamNumTracksBox.Value = file.MaxStreamTracks;
                    maxStreamNumChannelsBox.Value = file.MaxStreamChannels;
                    maxWaveNumBox.Value = file.MaxWaveSounds;
                    maxWaveNumTracksBox.Value = file.MaxWaveTracks;
                    streamBufferTimesBox.Value = file.StreamBufferTimes;
                    optionsPIBox.Value = file.Options;

                }

                //Not null parents.
                if (tree.SelectedNode.Parent != null) {

                    //Sequences.
                    if (tree.Nodes["sequences"] == tree.SelectedNode.Parent) {

                        fileIdPanel.BringToFront();
                        fileIdPanel.Show();

                    }

                }

            }

        }

        /// <summary>
        /// Hide all panels when needed.
        /// </summary>
        public void hideAllThings() {

            //Hide panels.
            projectInfoPanel.Hide();
            fileIdPanel.Hide();

            //Show no info.
            noInfoPanel.BringToFront();
            noInfoPanel.Show();

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

            doInfoPanelStuff();

        }

        void treeArrowKey(object sender, KeyEventArgs e)
        {

            doInfoPanelStuff();

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
            foreach (var s in file.Streams) {

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
            foreach (var w in file.WaveSoundDatas) {

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
            foreach (var s in file.Sequences) {

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
            foreach (var b in file.Banks) {

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
            foreach (var p in file.Players) {

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
            foreach (var g in file.Groups) {

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
            foreach (var w in file.WaveArchives) {

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
            foreach (var s in file.SoundSets) {

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
                                if (i >= file.Streams.Count) {
                                    key = "waveSoundSets";
                                }
                                if (i >= file.Streams.Count + file.WaveSoundDatas.Count) {
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
                            tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, tree.Nodes[key].Nodes[i - file.Streams.Count].Text, tree.Nodes[key].ImageIndex, tree.Nodes[key].SelectedImageIndex);

                        } else if (key.Equals("sequences")) {

                            //Add sequence file.
                            tree.Nodes["soundGroups"].Nodes["soundGroup" + sCount].Nodes.Add("entry" + i, tree.Nodes[key].Nodes[i - file.Streams.Count - file.WaveSoundDatas.Count].Text, tree.Nodes[key].ImageIndex, tree.Nodes[key].SelectedImageIndex);

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
            foreach (var f in file.Files)
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
                                icon = 1;
                                break;

                            case "grp":
                                icon = 7;
                                break;

                        }
                    }

                    //Get content type.
                    string type = "(" + f.FileType.ToString() + ")";
                    if (f.FileType == EFileType.Internal && f.File == null) {
                        type = "(Internal But Null)";
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


        /// <summary>
        /// Extract to a folder. MAKE THIS WORK IN THE FUTURE!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            folderSelector.SelectedPath = "";
            folderSelector.ShowDialog();

            if (folderSelector.SelectedPath != null) {
                //file.Extract(folderSelector.SelectedPath, file.fileHeader.byteOrder);
            }

        }

    }
}