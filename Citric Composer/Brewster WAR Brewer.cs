using System;
using System.IO;
using CitraFileLoader;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace Citric_Composer
{
    public partial class Brewster_WAR_Brewer : Form
    {
        public Brewster_WAR_Brewer()
        {
            InitializeComponent();
        }


        //Variables.
        public b_war file; //File.
        public string filePath = ""; //File path to save.
        public bool fileOpen = false; //If file open.
        public channelPlayer[][] players; //Players.

        //Channel player.
        public struct channelPlayer
        {
            public byte[] file; //File.
            public WaveOutEvent player; //Player.
            public IWaveProvider playerFile; //Audio File.
        }

        //Open about.
        private void aboutBrewsterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrewsterAbout a = new BrewsterAbout();
            a.Show();
        }


        //Load Channel Files
        public void loadChannelFiles() {

            if (fileOpen) {

                players = new channelPlayer[file.file.files.Count][];
                for (int i = 0; i < file.file.files.Count(); i++) {

                    b_wav w = new b_wav();
                    w.load(file.file.files[i].file);
                    w = w.toRiff().toGameWavPCM();
                    players[i] = new channelPlayer[w.data.pcm16.Count()];
                    for (int j = 0; j < w.data.pcm16.Count(); j++) {

                        players[i][j] = new channelPlayer();
                        MemoryStream o = new MemoryStream();
                        BinaryWriter bw = new BinaryWriter(o);
                        foreach (UInt16 sample in w.data.pcm16[j])
                        {
                            bw.Write(sample);
                        }
                        players[i][j].file = o.ToArray();
                        players[i][j].player = new WaveOutEvent();
                        players[i][j].playerFile = new RawSourceWaveStream(new MemoryStream(o.ToArray()), new NAudio.Wave.WaveFormat((int)w.info.samplingRate, 1));
                        players[i][j].player.Init(players[i][j].playerFile);

                    }

                }

            }

        }


        //Do Info Stuff.
        public void doInfoStuff() {

            if (fileOpen) {

                if (tree.SelectedNode != null) {

                    if (tree.SelectedNode.Parent != null)
                    {

                        bytesLabel.Text = "" + (decimal)file.file.files[tree.SelectedNode.Index].file.Length/1000 + " KB.";

                    }
                    else {

                        bytesLabel.Text = "No Bytes Selected!";

                    }

                } else
                {

                    bytesLabel.Text = "No Bytes Selected!";

                }

            }
            else
            {

                bytesLabel.Text = "No Bytes Selected!";

            }

        }


        //Update nodes.
        #region updateNodes
        public void updateNodes()
        {

            //Start stuff.
            tree.BeginUpdate();

            tree.SelectedNode = tree.Nodes[0];
            tree.Nodes[0].ContextMenuStrip = null;

            List<string> expandedNodes = collectExpandedNodes(tree.Nodes);

            foreach (TreeNode e in tree.Nodes[0].Nodes)
            {
                tree.Nodes[0].Nodes.RemoveAt(0);
            }


            //Only if file is open.
            if (fileOpen)
            {

                int count = 0;
                foreach (CitraFileLoader.b_war.fileBlock.fileEntry f in file.file.files) {

                    tree.Nodes[0].Nodes.Add("Wave " + count, "Wave " + count, 1, 1);
                    tree.Nodes[0].Nodes[count].ContextMenuStrip = nodeMenu;
                    count += 1;

                }
                tree.Nodes[0].ContextMenuStrip = rootMenu;

            }

            tree.EndUpdate();

        }
        #endregion



        //Node shit.
        #region nodeShit

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

            doInfoStuff();

        }

        void tree_NodeKey(object sender, KeyEventArgs e)
        {

            doInfoStuff();

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

        #endregion



        //File nodes.
        #region fileNodes

        //New File.
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            file = new b_war();
            fileOpen = true;
            this.Text = "Brewster's Archive Brewer - NewArchive.bfwar";

        }

        //Open File.
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            warOpen.ShowDialog();
            if (warOpen.FileName != "")
            {

                file = new b_war();
                file.load(File.ReadAllBytes(warOpen.FileName));
                fileOpen = true;
                filePath = warOpen.FileName;
                this.Text = "Brewster's Archive Brewer - " + Path.GetFileName(filePath);
                warOpen.FileName = "";
                updateNodes();

            }

        }


        #endregion



        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
