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
                        players[i][j].samplingRate = (int)w.info.samplingRate;

                    }

                }

            }

        }



        //Player Stuff.
        #region playerStuff

        //Play.
        private void playButton_Click(object sender, EventArgs e)
        {

            if (fileOpen && file.file.files.Count() > 0)
            {

                //Stop players.
                for (int i = 0; i < players.Count(); i++)
                {

                    for (int j = 0; j < players[i].Count(); j++)
                    {
                        if (i != tree.SelectedNode.Index || (tree.SelectedNode.Index != lastIndex || !paused))
                        {
                            try { players[i][j].player.Stop(); } catch { }
                            players[i][j].player = new WaveOutEvent();
                            players[i][j].playerFile = new RawSourceWaveStream(new MemoryStream(players[i][j].file), new NAudio.Wave.WaveFormat(players[i][j].samplingRate, 1));
                            players[i][j].player.Init(players[i][j].playerFile);
                        }

                    }

                }

            }
            if (fileOpen) {

                lastIndex = tree.SelectedNode.Index;
                foreach (channelPlayer p in players[tree.SelectedNode.Index]) {

                    p.player.Play();
                    paused = false;

                }

            }

        }

        //Pause.
        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (fileOpen && file.file.files.Count() > 0)
            {

                //Pause players.
                for (int i = 0; i < players.Count(); i++)
                {

                    for (int j = 0; j < players[i].Count(); j++)
                    {

                        try { players[i][j].player.Pause(); } catch { }

                    }

                }
                paused = true;
            }
        }

        //Stop.
        private void stopButton_Click(object sender, EventArgs e)
        {
            stopMusic();
        }

        //Stop all music.
        public void stopMusic() {

            if (fileOpen && file.file.files.Count() > 0)
            {

                //Stop players.
                for (int i = 0; i < players.Count(); i++)
                {

                    for (int j = 0; j < players[i].Count(); j++)
                    {

                        try { players[i][j].player.Stop(); } catch { }
                        players[i][j].player = new WaveOutEvent();
                        players[i][j].playerFile = new RawSourceWaveStream(new MemoryStream(players[i][j].file), new NAudio.Wave.WaveFormat(players[i][j].samplingRate, 1));
                        players[i][j].player.Init(players[i][j].playerFile);

                    }

                }

            }

        }

        #endregion



        //Do Info Stuff.
        public void doInfoStuff() {

            if (fileOpen) {

                if (tree.SelectedNode != null) {

                    if (tree.SelectedNode.Parent != null)
                    {

                        noInfoPanel.Hide();
                        playerPanel.Show();
                        bytesLabel.Text = "Wave " + tree.SelectedNode.Index + ", " + (decimal)file.file.files[tree.SelectedNode.Index].file.Length/1000 + " KB.";

                    }
                    else {

                        noInfoPanel.Show();
                        playerPanel.Hide();
                        bytesLabel.Text = "No Bytes Selected!";

                    }

                } else
                {

                    noInfoPanel.Show();
                    playerPanel.Hide();
                    bytesLabel.Text = "No Bytes Selected!";

                }

            }
            else
            {

                noInfoPanel.Show();
                playerPanel.Hide();
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

        //New File.
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            stopMusic();
            file = new b_war();
            file.file = new b_war.fileBlock();
            file.file.files = new List<b_war.fileBlock.fileEntry>();
            file.info = new b_war.infoBlock();
            file.info.entries = new b_war.sizedReferenceTable();
            fileOpen = true;
            filePath = "";
            endian = Syroot.BinaryData.ByteOrder.BigEndian;
            this.Text = "Brewster's Archive Brewer - New Archive.bfwar";

            updateNodes();
            loadChannelFiles();

        }

        //Open File.
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            warOpen.ShowDialog();
            if (warOpen.FileName != "")
            {

                stopMusic();
                file = new b_war();
                file.load(File.ReadAllBytes(warOpen.FileName));
                fileOpen = true;
                filePath = warOpen.FileName;
                this.Text = "Brewster's Archive Brewer - " + Path.GetFileName(filePath);
                warOpen.FileName = "";
                endian = file.endian;
                loadChannelFiles();
                updateNodes();

            }

        }

        //Close
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            int res = 1;
            if (fileOpen) {

                SaveCloseDialog c = new SaveCloseDialog();
                res = c.getValue();

            }
            if (res == 0) { save(); }
            if (res == 0 || res == 1) {

                fileOpen = false;
                file = new b_war();
                this.Text = "Brewster's Archive Brewer";
                updateNodes();
                noInfoPanel.Show();
                playerPanel.Hide();
                tree.SelectedNode = tree.Nodes[0];

            }

        }

        //Quit
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            if (fileOpen)
            {
                SaveQuitDialog q = new SaveQuitDialog(this);
                q.ShowDialog();
            }
            else {
                this.Close();
            }
        }

        #endregion



        //Edit nodes.
        #region editNodes

        //Import
        private void importFromFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            if (fileOpen)
            {
                folderOpen.ShowDialog();
                if (folderOpen.SelectedPath != "")
                {

                    file.compress(folderOpen.SelectedPath);
                    folderOpen.SelectedPath = "";
                    updateNodes();
                    loadChannelFiles();

                }
            }
        }

        //Export
        private void exportToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                folderOpen.ShowDialog();
                if (folderOpen.SelectedPath != "")
                {

                    file.extract(folderOpen.SelectedPath, endian);
                    folderOpen.SelectedPath = "";

                }
            }
        }

        //Import Waves.
        private void importWavesFromFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            if (fileOpen)
            {
                folderOpen.ShowDialog();
                if (folderOpen.SelectedPath != "")
                {

                    file.compressWaves(folderOpen.SelectedPath);
                    folderOpen.SelectedPath = "";
                    updateNodes();
                    loadChannelFiles();

                }
            }
        }

        //Export waves.
        private void exportWavesToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (fileOpen)
            {
                folderOpen.ShowDialog();
                if (folderOpen.SelectedPath != "")
                {

                    file.extractWaves(folderOpen.SelectedPath);
                    folderOpen.SelectedPath = "";

                }
            }

        }

        #endregion



        //Save stuff.
        #region saveStuff

        //Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        //Save
        public void save() {

            if (fileOpen)
            {

                file.update(Syroot.BinaryData.ByteOrder.BigEndian);
                if (filePath != "")
                {
                    File.WriteAllBytes(filePath, file.toBytes(endian));
                }
                else
                {
                    saveAs();
                }

            }

        }

        //Save As.
        public void saveAs() {

            if (fileOpen) {

                warSave.ShowDialog();
                if (warSave.FileName != "") {

                    if (warSave.FilterIndex == 1) { endian = Syroot.BinaryData.ByteOrder.BigEndian; } else { endian = Syroot.BinaryData.ByteOrder.LittleEndian; }
                    filePath = warSave.FileName;
                    this.Text = "Brewster's Archive Brewer - " + Path.GetFileName(filePath);
                    save();
                    warSave.FileName = "";

                }

            }

        }

        //Save As.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        #endregion



        //Root node.
        #region rootNode

        //Add.
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

            stopMusic();
            soundOpen.ShowDialog();
            if (soundOpen.FileName != "") {

                b_wav b = new b_wav();
                switch (soundOpen.FilterIndex)
                {

                    case 1:
                    case 2:
                        b.load(File.ReadAllBytes(soundOpen.FileName));
                        break;
                    case 3:
                        RIFF r = new RIFF();
                        r.load(File.ReadAllBytes(soundOpen.FileName));
                        b = r.toGameWav();
                        b.update(endianNess.big);
                        break;

                }
                b_war.fileBlock.fileEntry e5 = new b_war.fileBlock.fileEntry();
                e5.file = b.toBytes(endianNess.big);
                file.file.files.Add(e5);
                soundOpen.FileName = "";
                updateNodes();
                loadChannelFiles();

            }

        }

        //Expand.
        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree.SelectedNode.Expand();
        }

        //Collapse.
        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree.SelectedNode.Collapse();
        }

        #endregion



        //Node menu.
        #region nodeMenu

        //Add above.
        private void addAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            soundOpen.ShowDialog();
            if (soundOpen.FileName != "")
            {

                b_wav b = new b_wav();
                switch (soundOpen.FilterIndex)
                {

                    case 1:
                    case 2:
                        b.load(File.ReadAllBytes(soundOpen.FileName));
                        break;
                    case 3:
                        RIFF r = new RIFF();
                        r.load(File.ReadAllBytes(soundOpen.FileName));
                        b = r.toGameWav();
                        b.update(endianNess.big);
                        break;

                }
                b_war.fileBlock.fileEntry e5 = new b_war.fileBlock.fileEntry();
                e5.file = b.toBytes(endianNess.big);
                file.file.files.Insert(tree.SelectedNode.Index, e5);
                soundOpen.FileName = "";
                updateNodes();
                loadChannelFiles();

            }
        }

        //Add below.
        private void addBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            soundOpen.ShowDialog();
            if (soundOpen.FileName != "")
            {

                b_wav b = new b_wav();
                switch (soundOpen.FilterIndex)
                {

                    case 1:
                    case 2:
                        b.load(File.ReadAllBytes(soundOpen.FileName));
                        break;
                    case 3:
                        RIFF r = new RIFF();
                        r.load(File.ReadAllBytes(soundOpen.FileName));
                        b = r.toGameWav();
                        b.update(endianNess.big);
                        break;

                }
                b_war.fileBlock.fileEntry e5 = new b_war.fileBlock.fileEntry();
                e5.file = b.toBytes(endianNess.big);
                file.file.files.Insert(tree.SelectedNode.Index+1, e5);
                soundOpen.FileName = "";
                updateNodes();
                loadChannelFiles();

            }
        }

        //Move up.
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode.Index != 0) {

                b_war.fileBlock.fileEntry temp1f = file.file.files[tree.SelectedNode.Index];
                b_war.fileBlock.fileEntry temp2f = file.file.files[tree.SelectedNode.Index-1];
                channelPlayer[] temp1p = players[tree.SelectedNode.Index];
                channelPlayer[] temp2p = players[tree.SelectedNode.Index-1];

                file.file.files[tree.SelectedNode.Index - 1] = temp1f;
                file.file.files[tree.SelectedNode.Index] = temp2f;
                players[tree.SelectedNode.Index - 1] = temp1p;
                players[tree.SelectedNode.Index] = temp2p;

            }
        }

        //Move down.
        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode.Index != tree.Nodes[0].Nodes.Count - 1)
            {

                b_war.fileBlock.fileEntry temp1f = file.file.files[tree.SelectedNode.Index];
                b_war.fileBlock.fileEntry temp2f = file.file.files[tree.SelectedNode.Index + 1];
                channelPlayer[] temp1p = players[tree.SelectedNode.Index];
                channelPlayer[] temp2p = players[tree.SelectedNode.Index + 1];

                file.file.files[tree.SelectedNode.Index + 1] = temp1f;
                file.file.files[tree.SelectedNode.Index] = temp2f;
                players[tree.SelectedNode.Index + 1] = temp1p;
                players[tree.SelectedNode.Index] = temp2p;

            }
        }

        //Export.
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            soundSave.ShowDialog();
            if (soundSave.FileName != "") {

                b_wav b = new b_wav();
                b.load(file.file.files[tree.SelectedNode.Index].file);

                switch (soundSave.FilterIndex) {

                    case 1:
                        File.WriteAllBytes(soundSave.FileName, b.toBytes(endianNess.big));
                        break;

                    case 2:
                        File.WriteAllBytes(soundSave.FileName, b.toBytes(endianNess.little));
                        break;

                    case 3:
                        RIFF r = new RIFF();
                        r = b.toRiff();
                        r.fixOffsets();
                        File.WriteAllBytes(soundSave.FileName, r.toBytes());
                        break;

                }

                soundSave.FileName = "";

            }

        }

        //Import.
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

            stopMusic();
            soundOpen.ShowDialog();
            if (soundOpen.FileName != "") {

                b_wav b = new b_wav();
                switch (soundOpen.FilterIndex) {

                    case 1:
                    case 2:
                        b.load(File.ReadAllBytes(soundOpen.FileName));
                        break;
                    case 3:
                        RIFF r = new RIFF();
                        r.load(File.ReadAllBytes(soundOpen.FileName));
                        b = r.toGameWav();
                        b.update(endianNess.big);
                        break;

                }
                b_war.fileBlock.fileEntry e5 = file.file.files[tree.SelectedNode.Index];
                e5.file = b.toBytes(endianNess.big);
                file.file.files[tree.SelectedNode.Index] = e5;
                soundOpen.FileName = "";
                loadChannelFiles();

            }

        }

        //Delete.
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopMusic();
            file.file.files.RemoveAt(tree.SelectedNode.Index);
            updateNodes();
            loadChannelFiles();
        }

        #endregion



        //Volume
        private void volumeBar_Scroll(object sender, EventArgs e)
        {

            if (fileOpen) {

                for (int i = 0; i < players.Count(); i++) {

                    for (int j = 0; j < players[i].Count(); j++) {

                        try { players[i][j].player.Volume = (float)((decimal)volumeBar.Value/100); } catch { }

                    }

                }

            }

        }



        //Isabelle
        private void lauchIsabelle(object sender, TreeNodeMouseClickEventArgs e) {

            IsabelleSoundEditor s = new IsabelleSoundEditor(this, tree.SelectedNode.Index, tree.SelectedNode.Text + ".bfwav");
            s.Show();

        }


        //Stop the players.
        private void formClosing(object sender, EventArgs e) {

            for (int i = 0; i < players.Count(); i++) {

                for (int j = 0; j < players[i].Count(); j++) {

                    try { players[i][j].player.Stop(); } catch { }
                    try { players[i][j].player.Dispose(); } catch { }

                }

            }

        }
        
    }
}
