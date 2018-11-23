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
using IsabelleLib;
using NAudio.Wave;
using CitraFileLoaderz;

namespace Citric_Composer
{
    public partial class Goldi_GRP_Grouper : Form
    {
        public Goldi_GRP_Grouper()
        {
            InitializeComponent();
        }


        //Variables.
        public CitraFileLoaderz.b_grpO file; //File.
        public string filePath = ""; //File path to save.
        public bool fileOpen = false; //If file open.
        public channelPlayer[][] players; //Players.
        public Syroot.BinaryData.ByteOrder endian; //Endianess.
        int lastIndex = -1;
        bool paused = false;

        //Channel player.
        public struct channelPlayer
        {
            public byte[] file; //File.
            public WaveOutEvent player; //Player.
            public IWaveProvider playerFile; //Audio File.
            public int samplingRate; //Sampling Rate.
        }

        //Open about.
        private void aboutBrewsterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoldiAbout a = new GoldiAbout();
            a.Show();
        }


        //Do Info Stuff.
        public void doInfoStuff() {



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

        //New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file = new b_grpO();
            
        }

        //Close
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Quit
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        #endregion



        //Edit menu.
        #region editMenu

        //Import.
        private void importFromFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Export.
        private void exportToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        #endregion



        //Save stuff.
        #region saveStuff

        //Save.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Save As.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Save
        public void save() {



        }

        //Save As
        public void saveAs() {



        }

        #endregion


    }
}
