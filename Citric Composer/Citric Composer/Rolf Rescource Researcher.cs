using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitraFileLoader;

namespace Citric_Composer
{
    public partial class Rolf_Rescource_Researcher : Form
    {
        public Rolf_Rescource_Researcher()
        {
            InitializeComponent();
        }

        public bool fileOpen = false;
        public string filePath = "";

        public bars file;


        /// <summary>
        /// Update nodes.
        /// </summary>
        public void UpdateNodes()
        {

            //Update the file.
            //file.Update(file.fileHeader.byteOrder);

            //Get nodes that are currently expanded.
            List<string> expandedNodes = collectExpandedNodes(tree.Nodes);

            //First remove all nodes.
            tree.BeginUpdate();
            for (int i = 0; i < tree.Nodes.Count; i++)
            {

                RemoveChildNodes(tree.Nodes[i]);

            }

            foreach (bars.FileReference f in file.fileReferences) {


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
        /// Do info stuff.
        /// </summary>
        public void doInfoPanelStuff() {



        }

        /// <summary>
        /// About click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutRolf_Click(object sender, EventArgs e)
        {
            RolfAbout r = new RolfAbout();
            r.Show();
        }

    }

}
