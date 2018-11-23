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
        public b_sar file;


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

                try
                {

                    //Make a new file, and load it.
                    file = new b_sar();
                    file.Load(File.ReadAllBytes(openB_sarBox.FileName));
                    fileOpen = true;
                    fileName = openB_sarBox.FileName;
                    
                }
                catch {

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
                    maxSeqNumBox.Value = file.info.projectInfo.maxSeq;
                    maxSeqTrackNumBox.Value = file.info.projectInfo.maxSeqTracks;
                    maxStreamNumBox.Value = file.info.projectInfo.maxStreamSounds;
                    maxStreamNumTracksBox.Value = file.info.projectInfo.maxStreamTracks;
                    maxStreamNumChannelsBox.Value = file.info.projectInfo.maxStreamChannels;
                    maxWaveNumBox.Value = file.info.projectInfo.maxWaveSounds;
                    maxWaveNumTracksBox.Value = file.info.projectInfo.maxWaveTracks;
                    streamBufferTimesBox.Value = file.info.projectInfo.streamBufferTimes;
                    optionsPIBox.Value = file.info.projectInfo.options;

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

            //Update the file.
            file.Update(file.fileHeader.byteOrder);

            //Get nodes that are currently expanded.
            List<string> expandedNodes = collectExpandedNodes(tree.Nodes);

            //First remove all nodes.
            tree.BeginUpdate();
            for (int i = 0; i < tree.Nodes.Count; i++)
            {

                RemoveChildNodes(tree.Nodes[i]);

            }

            //Load sounds.
            int ssCount = 0;
            foreach (b_sar.InfoBlock.soundInfo s in file.info.sounds) {

                //Null.
                if (s == null)
                {
                    tree.Nodes["sequences"].Nodes.Add("[" + ssCount + "] " + "%PLACEHOLDER%");
                }

                //Not null.
                else
                {

                    //Not stream info.
                    if (s.streamInfo == null)
                    {

                        //Not wave info, so sequence info.
                        if (s.waveInfo == null)
                        {

                            //Null.
                            if (s.sequenceInfo == null)
                            {

                                tree.Nodes["sequences"].Nodes.Add("[" + ssCount + "] " + "%PLACEHOLDER%");

                            }

                            //Not null.
                            else
                            {

                                //Get node name.
                                string nodeName = "NO_NAME";
                                if (s.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)s.flags.flagValues[0]].data); }
                                tree.Nodes["sequences"].Nodes.Add("[" + ssCount + "] " + nodeName, "[" + ssCount + "] " + nodeName, 3, 3);

                            }

                        }

                        //Wave info.
                        else
                        {

                            //Get node name.
                            string nodeName = "NO_NAME";
                            if (s.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)s.flags.flagValues[0]].data); }
                            tree.Nodes["waveSoundSets"].Nodes.Add("[" + ssCount + "] " + nodeName, "[" + ssCount + "] " + nodeName, 2, 2);

                        }

                    }

                    //Stream info.
                    else
                    {

                        //Get node name.
                        string nodeName = "NO_NAME";
                        if (s.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)s.flags.flagValues[0]].data); }
                        tree.Nodes["streams"].Nodes.Add("[" + ssCount + "] " + nodeName, "[" + ssCount + "] " + nodeName, 1, 1);

                    }

                }

                ssCount += 1;

            }

            //Load banks.
            int bCount = 0;
            foreach (b_sar.InfoBlock.bankInfo b in file.info.banks) {

                //Null.
                if (b == null)
                {

                    tree.Nodes["banks"].Nodes.Add("[" + bCount + "] " + "%PLACEHOLDER%");

                }
                else
                {

                    //Get node name.
                    string nodeName = "NO_NAME";
                    if (b.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)b.flags.flagValues[0]].data); }
                    tree.Nodes["banks"].Nodes.Add("[" + bCount + "] " + nodeName, "[" + bCount + "] " + nodeName, 5, 5);

                }

                bCount += 1;

            }

            //Load players.
            int pCount = 0;
            foreach (b_sar.InfoBlock.playerInfo p in file.info.players)
            {

                //Null.
                if (p == null)
                {

                    tree.Nodes["players"].Nodes.Add("[" + pCount + "] " + "%PLACEHOLDER%");

                }
                else
                {

                    //Get node name.
                    string nodeName = "NO_NAME";
                    if (p.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)p.flags.flagValues[0]].data); }
                    tree.Nodes["players"].Nodes.Add("[" + pCount + "] " + nodeName, "[" + pCount + "] " + nodeName, 8, 8);

                }

                pCount += 1;

            }

            //Load sound groups.
            int sCount = 0;
            foreach (b_sar.InfoBlock.soundGroupInfo s in file.info.soundGroups)
            {

                //Null.
                if (s == null)
                {

                    tree.Nodes["soundGroups"].Nodes.Add("[" + sCount + "] " + "%PLACEHOLDER%");

                }
                else
                {

                    //Get node name.
                    string nodeName = "NO_NAME";
                    if (s.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)s.flags.flagValues[0]].data); }
                    tree.Nodes["soundGroups"].Nodes.Add("[" + sCount + "] " + nodeName, "[" + sCount + "] " + nodeName, 4, 4);

                    //Add the other entries to the node.
                    for (int i = (int)s.firstId.index; i <= s.lastId.index; i++) {

                        switch (s.firstId.type) {

                            case SoundTypes.Null:
                                tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("NULL");
                                break;

                            case SoundTypes.Sound:
                                if (file.info.sounds[i] == null)
                                {
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("%PLACEHOLDER%");
                                }
                                else {

                                    string nodeName2 = "NO_NAME";
                                    if (file.info.sounds[i].flags.isFlagEnabled[0]) { nodeName2 = new string(file.strg.stringEntries[(int)file.info.sounds[i].flags.flagValues[0]].data); }
                                    if (file.info.sounds[i].sequenceInfo != null) {
                                        tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 3, 3);
                                    } else if (file.info.sounds[i].waveInfo != null) {
                                        tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 2, 2);
                                    } else if (file.info.sounds[i].streamInfo != null) {
                                        tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 1, 1);
                                    } else {
                                        tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("WTF HOW AM I HERE??? PLS REPORT IMMEDIATELY");
                                    }

                                }
                                break;

                            case SoundTypes.SoundGroup:
                                if (file.info.soundGroups[i] == null)
                                {
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("%PLACEHOLDER%");
                                }
                                else
                                {

                                    string nodeName2 = "NO_NAME";
                                    if (file.info.soundGroups[i].flags.isFlagEnabled[0]) {
                                        nodeName2 = new string(file.strg.stringEntries[(int)file.info.soundGroups[i].flags.flagValues[0]].data);
                                    }
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 4, 4);

                                }
                                break;

                            case SoundTypes.Bank:
                                if (file.info.banks[i] == null)
                                {
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("%PLACEHOLDER%");
                                }
                                else
                                {

                                    string nodeName2 = "NO_NAME";
                                    if (file.info.banks[i].flags.isFlagEnabled[0])
                                    {
                                        nodeName2 = new string(file.strg.stringEntries[(int)file.info.banks[i].flags.flagValues[0]].data);
                                    }
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 5, 5);

                                }
                                break;

                            case SoundTypes.Player:
                                if (file.info.players[i] == null)
                                {
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("%PLACEHOLDER%");
                                }
                                else
                                {

                                    string nodeName2 = "NO_NAME";
                                    if (file.info.players[i].flags.isFlagEnabled[0])
                                    {
                                        nodeName2 = new string(file.strg.stringEntries[(int)file.info.players[i].flags.flagValues[0]].data);
                                    }
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 8, 8);

                                }
                                break;

                            case SoundTypes.WaveArchive:
                                if (file.info.wars[i] == null)
                                {
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("%PLACEHOLDER%");
                                }
                                else
                                {

                                    string nodeName2 = "NO_NAME";
                                    if (file.info.wars[i].flags.isFlagEnabled[0])
                                    {
                                        nodeName2 = new string(file.strg.stringEntries[(int)file.info.wars[i].flags.flagValues[0]].data);
                                    }
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 4, 4);

                                }
                                break;

                            case SoundTypes.Group:
                                if (file.info.groups[i] == null)
                                {
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add("%PLACEHOLDER%");
                                }
                                else
                                {

                                    string nodeName2 = "NO_NAME";
                                    if (file.info.groups[i].flags.isFlagEnabled[0])
                                    {
                                        nodeName2 = new string(file.strg.stringEntries[(int)file.info.groups[i].flags.flagValues[0]].data);
                                    }
                                    tree.Nodes["soundGroups"].Nodes[sCount].Nodes.Add(nodeName2, nodeName2, 7, 7);

                                }
                                break;

                        }

                    }

                }

                sCount += 1;

            }

            //Load groups.
            int gCount = 0;
            foreach (b_sar.InfoBlock.groupInfo g in file.info.groups)
            {

                //Null.
                if (g == null)
                {

                    tree.Nodes["groups"].Nodes.Add("[" + gCount + "] " + "%PLACEHOLDER%");

                }
                else
                {

                    //Get node name.
                    string nodeName = "NO_NAME";
                    if (g.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)g.flags.flagValues[0]].data); }
                    tree.Nodes["groups"].Nodes.Add("[" + gCount + "] " + nodeName, "[" + gCount + "] " + nodeName, 7, 7);

                }

                gCount += 1;

            }

            //Load wave archives.
            int wCount = 0;
            foreach (b_sar.InfoBlock.waveArchiveInfo w in file.info.wars)
            {

                //Null.
                if (w == null)
                {

                    tree.Nodes["waveArchives"].Nodes.Add("[" + wCount + "] " + "%PLACEHOLDER%");

                }
                else
                {

                    //Get node name.
                    string nodeName = "NO_NAME";
                    if (w.flags.isFlagEnabled[0]) { nodeName = new string(file.strg.stringEntries[(int)w.flags.flagValues[0]].data); }
                    tree.Nodes["waveArchives"].Nodes.Add("[" + wCount + "] " + nodeName, "[" + wCount + "] " + nodeName, 4, 4);

                }

                wCount += 1;

            }

            //Load files.
            int fCount = 0;
            tree.Nodes["files"].Nodes.Add("internalFiles", "Internal", 11, 11);
            tree.Nodes["files"].Nodes.Add("externalFiles", "External", 11, 11);
            foreach (b_sar.InfoBlock.fileInfo f in file.info.files)
            {

                int icon = 0;

                if (f == null)
                {
                    tree.Nodes["files"].Nodes.Add("[" + fCount + "] " + "%PLACEHOLDER%");
                }
                else {

                    if (f.internalFileInfo == null)
                    {
                        if (f.externalFileName == null)
                        {
                            tree.Nodes["files"].Nodes.Add("[" + fCount + "] " + "%PLACEHOLDER%");
                        }
                        else {

                            if (f.externalFileName.EndsWith("seq"))
                            {
                                icon = 3;
                            }
                            else if (f.externalFileName.EndsWith("wsd"))
                            {
                                icon = 2;
                            }
                            else if (f.externalFileName.EndsWith("stm"))
                            {
                                icon = 1;
                            }
                            else if (f.externalFileName.EndsWith("bnk"))
                            {
                                icon = 5;
                            }
                            else if (f.externalFileName.EndsWith("war"))
                            {
                                icon = 4;
                            }
                            else if (f.externalFileName.EndsWith("grp"))
                            {
                                icon = 7;
                            }

                            tree.Nodes["files"].Nodes["externalFiles"].Nodes.Add("[" + fCount + "] " + f.externalFileName, "[" + fCount + "] " + f.externalFileName, icon, icon);

                        }
                    }
                    else {

                        if (f.externalFileName == null)
                        {
                            tree.Nodes["files"].Nodes["internalFiles"].Nodes.Add("[" + fCount + "] " + file.FindFileName(fCount, ref icon), "[" + fCount + "] " + file.FindFileName(fCount, ref icon), icon, icon);
                        }
                        else {

                            tree.Nodes["files"].Nodes.Add("[" + fCount + "] " + "%PLACEHOLDER%");
                        }

                    }

                }

                fCount += 1;

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
        /// Extract to a folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            folderSelector.SelectedPath = "";
            folderSelector.ShowDialog();

            if (folderSelector.SelectedPath != null) {
                file.Extract(folderSelector.SelectedPath, file.fileHeader.byteOrder);
            }

        }

    }
}