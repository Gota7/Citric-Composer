using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using CitraFileLoader;
using IsabelleLib;
using NAudio.Wave;
using System.Threading;
using Syroot.BinaryData;
using System.Reflection;
using System.Diagnostics;
using Citric_Composer.Properties;

namespace Citric_Composer
{

    /// <summary>
    /// Isabelle Sound Editor. W00t.
    /// </summary>
    public partial class IsabelleSoundEditor : Form
    {   

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public IsabelleSoundEditor()
        {
            InitializeComponent();
            loopThread = new Thread(loop);
            loopThread.IsBackground = true;
            loopThread.Start();
        }

        /// <summary>
        /// Make from a WAR editor.
        /// </summary>
        /// <param name="war2"></param>
        /// <param name="index"></param>
        /// <param name="text"></param>
        public IsabelleSoundEditor(Brewster_WAR_Brewer war2, int index, string text)
        {

            //Init.
            InitializeComponent();
            Thread loopThread = new Thread(loop);
            loopThread.IsBackground = true;
            loopThread.Start();
            war = war2;
            launchMode = 1;
            fileIndex = index;

            //Transform Isabelle.
            this.Text = "Isabelle Sound Editor (WAR Mode) - " + text;
            fileNamePath = "INTERNAL";
            openToolStripMenuItem.Text = "Import From File";
            closeToolStripMenuItem.Visible = false;
            exportBinaryToolStripMenuItem.Image = saveAsToolStripMenuItem.Image;
            exportBinaryToolStripMenuItem.ShortcutKeys = saveAsToolStripMenuItem.ShortcutKeys;
            exportBinaryToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Visible = false;
            b_wav b = (war2.File as SoundWaveArchive)[index].Wav;

            //Make new FISP.
            file = new FISP(b);
            fileOpen = true;
            updateNodes();
            loadChannelFiles();
            doInfoStuff();
            playLikeGameBox.Checked = false;
            playPauseButton_Click(null, null);

        }

        /// <summary>
        /// Make from a SAR editor.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="index"></param>
        /// <param name="text"></param>
        /// <param name="wave"></param>
        public IsabelleSoundEditor(MainWindow mainWindow, int index, string text, bool wave) {
            //Init.
            InitializeComponent();
            Thread loopThread = new Thread(loop);
            loopThread.IsBackground = true;
            loopThread.Start();

            launchMode = 1;
            fileIndex = index;
            this.mainWindow = mainWindow;
            outModeWave = wave;

            //Transform Isabelle.
            this.Text = "Isabelle Sound Editor (SAR Mode) - " + text;
            fileNamePath = "INTERNAL";
            openToolStripMenuItem.Text = "Import From File";
            closeToolStripMenuItem.Visible = false;
            exportBinaryToolStripMenuItem.Image = saveAsToolStripMenuItem.Image;
            exportBinaryToolStripMenuItem.ShortcutKeys = saveAsToolStripMenuItem.ShortcutKeys;
            exportBinaryToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Visible = false;

            //Make new FISP.
            if (wave) {
                file = new FISP(((Wave)mainWindow.File.Files[index].File).Wav);
            } else {
                file = new FISP(((CitraFileLoader.Stream)mainWindow.File.Files[index].File).Stm);
            }
            fileOpen = true;
            updateNodes();
            loadChannelFiles();
            doInfoStuff();
            playLikeGameBox.Checked = false;
            playPauseButton_Click(null, null);

        }

        /// <summary>
        /// Make from a file.
        /// </summary>
        /// <param name="fileToOpen">File to open.</param>
        public IsabelleSoundEditor(string fileToOpen) {

            InitializeComponent();
            loopThread = new Thread(loop);
            loopThread.IsBackground = true;
            loopThread.Start();

            file = new FISP();
            fileOpen = true;
            if (launchMode == 0) { this.Text = "Isabelle Sound Editor - New Project.fisp"; }

            switch (fileToOpen.Substring(fileToOpen.Length - 4)) {

                //Wave.
                case ".wav":
                    RiffWave w = new RiffWave();
                    w.Load(File.ReadAllBytes(fileToOpen));
                    file = new FISP(w);
                    break;

                //Game wave.
                case "fwav":
                case "cwav":
                    b_wav b = new b_wav();
                    b.Load(File.ReadAllBytes(fileToOpen));
                    file = new FISP(b);
                    break;

                //Binary wave.
                case "bwav":
                    BinaryWave a = new BinaryWave();
                    a.Load(File.ReadAllBytes(fileToOpen));
                    file = new FISP(a);
                    break;

                //Game stream.
                case "fstm":
                case "cstm":
                    b_stm s = new b_stm();
                    s.Load(File.ReadAllBytes(fileToOpen));
                    file = new FISP(s);
                    break;

                //Project.
                case "fisp":
                case "cisp":
                    //Open project.
                    if (fileToOpen.EndsWith(".cisp")) {
                        CISP c = new CISP();
                        c.load(File.ReadAllBytes(fileToOpen));
                    } else {
                        file = new FISP(File.ReadAllBytes(fileToOpen));
                        fileNamePath = fileToOpen;
                        Text = "Isabelle Sound Editor - " + Path.GetFileName(fileToOpen);
                    }
                    break;

            }

            loadChannelFiles();
            updateNodes();

            //Play song.
            playPauseButton_Click(null, null);

        }

        /// <summary>
        /// For the main loop of the taskbar.
        /// </summary>
        Thread loopThread;

        /// <summary>
        /// If the file is open or not.
        /// </summary>
        public bool fileOpen = false;

        /// <summary>
        /// Current project file.
        /// </summary>
        public FISP file;

        /// <summary>
        /// Where to save the file.
        /// </summary>
        public string fileNamePath = "";

        /// <summary>
        /// Players.
        /// </summary>
        public channelPlayer[] players;

        /// <summary>
        /// If playing a song.
        /// </summary>
        public bool playing;

        /// <summary>
        /// Timer to use for some things.
        /// </summary>
        int timer = 0;

        /// <summary>
        /// If user is scrolling the tracker.
        /// </summary>
        public bool scrolling = false;

        /// <summary>
        /// User scrolling left.
        /// </summary>
        public bool scrollingLeft = false;

        /// <summary>
        /// User scrolling right.
        /// </summary>
        public bool scrollingRight = false;

        /// <summary>
        /// The current channel playing.
        /// </summary>
        public int channelPlaying = 0;

        /// <summary>
        /// Path of Isabelle.
        /// </summary>
        public string isabellePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// Launch mode. 0 = Normal, 1 = B_WAR, 2 = B_SAR
        /// </summary>
        public int launchMode = 0;

        /// <summary>
        /// If launch mode is in WAR.
        /// </summary>
        public Brewster_WAR_Brewer war;

        /// <summary>
        /// Main window.
        /// </summary>
        public MainWindow mainWindow;

        /// <summary>
        /// Out mode wave;
        /// </summary>
        private bool outModeWave;

        /// <summary>
        /// Index of the file opened if not in normal mode.
        /// </summary>
        public int fileIndex;

        /// <summary>
        /// Updating time.
        /// </summary>
        public bool updatingTime;

        //Channel player.
        public struct channelPlayer {
            public byte[] file; //File.
            public WaveOutEvent player; //Player.
            public CSCore.WaveFormat player2;
            public IWaveProvider playerFile; //Audio File.
            public CSCore.IWaveSource source; //Source.
            public CSCore.SoundOut.WasapiOut soundOut; //Sound out.
        }


        //New file.
        #region newFile

        /// <summary>
        /// Make a new file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            projectPanel.Hide();
            channelPanel.Hide();
            trackPanel.Hide();
            noInfoPanel.Show();

            try
            {

                for (int i = 0; i < players.Length; i++)
                {
                    players[i].player.Stop();
                    try { players[i].soundOut.Stop(); } catch { }
                    try { players[i].soundOut.Dispose(); } catch { }
                    players[i].soundOut = null;

                    players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                    players[i].player.Init(players[i].playerFile);

                }


                try {
                    playPauseButton.Image = Resources.Play;
                } catch { }
                playing = false;

            }

            catch { }

            if (launchMode == 0)
            {
                //General stuff.
                fileNamePath = "";
                fileOpen = true;
                this.Text = "Isabelle Sound Editor - New Project.cisp";
            }

            //Make new FISP.
            file = new FISP();
            file.stream = new FISP.StreamInfo();
            file.stream.encoding = 2;
            file.tracks = new List<FISP.TrackInfo>();
            file.regions = new List<FISP.RegionInfo>();
            file.data = new FISP.DataInfo();
            file.data.data = new List<short[]>();

            updateNodes();

        }

        #endregion


        //Info stuff.
        #region infoStuff

        /// <summary>
        /// Do info stuff.
        /// </summary>
        public void doInfoStuff() {

            if (tree.SelectedNode.Parent != null && fileOpen)
            {

                //Channel.
                if (tree.SelectedNode.Parent.Index == 1)
                {

                    trackPanel.Hide();
                    noInfoPanel.Hide();
                    projectPanel.Hide();
                    channelPanel.Show();
                    regionPanel.Hide();

                }

                //Track.
                else if (tree.SelectedNode.Parent.Index == 2)
                {

                    trackPanel.Show();
                    projectPanel.Hide();
                    noInfoPanel.Hide();
                    channelPanel.Hide();
                    regionPanel.Hide();

                    volumeBox.Value = file.tracks[tree.SelectedNode.Index].volume;
                    panBox.Value = file.tracks[tree.SelectedNode.Index].pan;
                    surroundModeBox.Value = file.tracks[tree.SelectedNode.Index].surroundMode;
                    spanBox.Value = file.tracks[tree.SelectedNode.Index].span;

                    string channelText = "";
                    for (int i = 0; i < file.tracks[tree.SelectedNode.Index].channels.Count; i++)
                    {
                        channelText += (file.tracks[tree.SelectedNode.Index].channels[i] + 1);
                        if (i + 1 != file.tracks[tree.SelectedNode.Index].channels.Count) { channelText += ";"; }
                    }
                    channelTextBox.Text = channelText;


                    if (file.tracks[tree.SelectedNode.Index].channels.Count > 0)
                    {
                        soundDeluxeTrack1.Enabled = true;
                        soundDeluxeTrack2.Enabled = true;
                    }
                    else
                    {
                        soundDeluxeTrack1.Enabled = false;
                        soundDeluxeTrack2.Enabled = false;
                    }

                }
                else if (tree.SelectedNode.Parent.Index == 3) {

                    noInfoPanel.Hide();
                    trackPanel.Hide();
                    channelPanel.Hide();
                    projectPanel.Hide();
                    regionPanel.Show();
                    regionLoopStartBox.Value = file.regions[tree.SelectedNode.Index].start;
                    regionLoopEndBox.Value = file.regions[tree.SelectedNode.Index].end;

                }
                else
                {
                    noInfoPanel.Show();
                    trackPanel.Hide();
                    channelPanel.Hide();
                    projectPanel.Hide();
                    regionPanel.Hide();
                }

            }
            else if (fileOpen) {

                //Sound Project Info.
                if (tree.SelectedNode.Index == 0)
                {
                    trackPanel.Hide();
                    noInfoPanel.Hide();
                    projectPanel.Show();
                    channelPanel.Hide();
                    regionPanel.Hide();

                    if (file.data.data.Count > 0)
                    {

                        projectPanel.Enabled = true;

                        samplingRateBox.Value = file.stream.sampleRate;
                        loopBox.Checked = file.stream.isLoop;
                        loopStartBox.Value = file.stream.loopStart;
                        loopEndBox.Value = file.stream.loopEnd;
                        encodingBox.SelectedIndex = file.stream.encoding;
                        vMajorBox.Value = file.stream.vMajor;
                        vMinorBox.Value = file.stream.vMinor;
                        vRevisionBox.Value = file.stream.vRevision;
                        originalLoopStartBox.Value = file.stream.originalLoopStart;
                        originalLoopEndBox.Value = file.stream.originalLoopEnd;
                        secretBox.Value = file.stream.secretInfo;

                    }
                    else {
                        projectPanel.Enabled = false;
                    }

                }
                else {

                    noInfoPanel.Show();
                    projectPanel.Hide();
                    trackPanel.Hide();
                    regionPanel.Hide();
                    channelPanel.Hide();

                }

            }

        }


        /// <summary>
        /// Update project info.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateProjectInfoButton_Click(object sender, EventArgs e)
        {
            file.stream.isLoop = loopBox.Checked;
            file.stream.loopStart = (UInt32)loopStartBox.Value;
            file.stream.loopEnd = (UInt32)loopEndBox.Value;
            file.stream.originalLoopStart = (uint)originalLoopStartBox.Value;
            file.stream.originalLoopEnd = (uint)originalLoopEndBox.Value;
            file.stream.secretInfo = (uint)secretBox.Value;
            file.stream.vMajor = (byte)vMajorBox.Value;
            file.stream.vMinor = (byte)vMinorBox.Value;
            file.stream.vRevision = (byte)vRevisionBox.Value;
            file.stream.encoding = (byte)encodingBox.SelectedIndex;

            if (file.stream.loopEnd > file.data.data[0].Length) {
                file.stream.loopEnd = (UInt32)file.data.data[0].Length;
            }

        }

        #endregion


        //Track stuff.
        #region trackStuff

        /// <summary>
        /// Volume track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void volumeTrack(object sender, EventArgs e)
        {
            FISP.TrackInfo t = file.tracks[tree.SelectedNode.Index];
            t.volume = (byte)volumeBox.Value;
            file.tracks[tree.SelectedNode.Index] = t;
        }


        /// <summary>
        /// Pan track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void panTrack(object sender, EventArgs e)
        {
            FISP.TrackInfo t = file.tracks[tree.SelectedNode.Index];
            t.pan = (byte)panBox.Value;
            file.tracks[tree.SelectedNode.Index] = t;
        }


        /// <summary>
        /// Span track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spanBox_ValueChanged(object sender, EventArgs e)
        {
            file.tracks[tree.SelectedNode.Index].span = (byte)spanBox.Value;
        }


        /// <summary>
        /// Surround mode track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void surroundModeBox_ValueChanged(object sender, EventArgs e)
        {
            file.tracks[tree.SelectedNode.Index].surroundMode = (byte)surroundModeBox.Value;
        }


        /// <summary>
        /// Region start changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regionLoopStartBox_ValueChanged(object sender, EventArgs e)
        {
            file.regions[tree.SelectedNode.Index].start = (uint)regionLoopStartBox.Value;
        }


        /// <summary>
        /// Region end changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regionLoopEndBox_ValueChanged(object sender, EventArgs e)
        {
            file.regions[tree.SelectedNode.Index].end = (uint)regionLoopEndBox.Value;
        }


        /// <summary>
        /// Set region start point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regionSetStartButton_Click(object sender, EventArgs e)
        {
            file.regions[tree.SelectedNode.Index].start = (UInt32)(players[0].source.Position / 2);
            doInfoStuff();

            //spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);

            if (file.regions[tree.SelectedNode.Index].start >= file.data.data[0].Length) { file.regions[tree.SelectedNode.Index].start = (UInt32)file.data.data[0].Length; }
        }


        /// <summary>
        /// Set region end point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regionSetEndButton_Click(object sender, EventArgs e)
        {
            file.regions[tree.SelectedNode.Index].end = (UInt32)(players[0].source.Position / 2);
            doInfoStuff();

            //spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);

            if (file.regions[tree.SelectedNode.Index].end >= file.data.data[0].Length) { file.regions[tree.SelectedNode.Index].end = (UInt32)file.data.data[0].Length; }
        }


        /// <summary>
        /// Channel track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void channelTrack(object sender, EventArgs e)
        {
            try
            {
                FISP.TrackInfo t = file.tracks[tree.SelectedNode.Index];
                string channelString = channelTextBox.Text;
                t.channels = new List<byte>();
                if (channelString.Length > 0)
                {
                    t.numChannels = (UInt32)channelString.Length / 2 + 1;
                    for (int i = 0; i < t.numChannels; i += 1)
                    {
                        t.channels.Add((byte)(byte.Parse(new string(channelString[i * 2], 1)) - 1));
                    }
                }
                file.tracks[tree.SelectedNode.Index] = t;
            }
            catch
            {
                MessageBox.Show("Enter like this: 1;2");
            }
        }

        #endregion


        //Update nodes.
        #region updateNodes

        /// <summary>
        /// Update the nodes.
        /// </summary>
        public void updateNodes() {

            //Start stuff.
            tree.BeginUpdate();

            tree.SelectedNode = tree.Nodes[0];
            trackPanel.Hide();

            List<string> expandedNodes = collectExpandedNodes(tree.Nodes);

            foreach (TreeNode e in tree.Nodes[1].Nodes)
            {
                tree.Nodes[1].Nodes.RemoveAt(0);
            }
            foreach (TreeNode e in tree.Nodes[2].Nodes)
            {
                tree.Nodes[2].Nodes.RemoveAt(0);
            }
            foreach (TreeNode e in tree.Nodes[3].Nodes)
            {
                tree.Nodes[3].Nodes.RemoveAt(0);
            }

            //Only if file is open.
            if (fileOpen)
            {

                //Add tracks, channels, and regions.
                for (int i = 0; i < file.tracks.Count; i++)
                {

                    tree.Nodes[2].Nodes.Add("Track " + (i + 1), "Track " + (i + 1), 4, 4);

                }
                for (int i = 0; i < file.data.data.Count; i++)
                {

                    tree.Nodes[1].Nodes.Add("Channel " + (i + 1), "Channel " + (i + 1), 2, 2);

                }
                for (int i = 0; i < file.regions.Count(); i++) {

                    tree.Nodes[3].Nodes.Add("Region " + (i + 1), "Region " + (i + 1), 6, 6);

                }

                //Root menus.
                tree.Nodes[1].ContextMenuStrip = rootMenu;
                tree.Nodes[2].ContextMenuStrip = rootMenu;
                tree.Nodes[3].ContextMenuStrip = rootMenu;

                //Node menus.
                foreach (TreeNode n in tree.Nodes[1].Nodes) {
                    n.ContextMenuStrip = nodeMenu;
                }
                foreach (TreeNode n in tree.Nodes[2].Nodes) {
                    n.ContextMenuStrip = nodeMenu;
                }
                foreach (TreeNode n in tree.Nodes[3].Nodes) {
                    n.ContextMenuStrip = nodeMenu;
                }


            }
            else {
                tree.Nodes[1].ContextMenuStrip = null;
                tree.Nodes[2].ContextMenuStrip = null;
                tree.Nodes[3].ContextMenuStrip = null;
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

        /// <summary>
        /// Expand node path.
        /// </summary>
        /// <param name="node"></param>
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


        /// <summary>
        /// Right click a tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Node kep press.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tree_NodeKey(object sender, KeyEventArgs e)
        {

            doInfoStuff();

        }

        /// <summary>
        /// Collect expanded nodes.
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
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
            
            //Not found.
            return returnNode;
        }

        #endregion


        //Root nodes. ADDING NEW CHANNELS INCOMPLETE!!!
        #region rootNodes

        /// <summary>
        /// Add something on click. INCOMPLETE!!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, EventArgs e)
        {

            //Add track.
            if (tree.SelectedNode.Index == 2)
            {
                //Make new track.
                FISP.TrackInfo t = new FISP.TrackInfo();
                t.numChannels = 0;
                t.channels = new List<byte>();
                t.surroundMode = 0;
                t.span = 0;
                t.magic = "TRAC".ToCharArray();
                t.pan = 64;
                t.volume = 127;
                file.tracks.Add(t);

                //Update nodes.
                updateNodes();
            }

            //Add channels.
            else if (tree.SelectedNode.Index == 1)
            {

                MessageBox.Show("To add audio to a project, make a new one from importing existing audio.");
                /* Yeah... idk how this would play out.
                anyFileSelectorSound.ShowDialog();
                if (anyFileSelectorSound.FileName != "") {

                    RiffWave r = new RiffWave();
                    switch (anyFileSelectorSound.FileName.Substring(anyFileSelectorSound.FileName.Length - 4).ToLower()) {

                        //Game wave.
                        case "fwav":
                        case "cwav":
                            b_wav w = new b_wav();
                            w.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            r = RiffWaveFactory.CreateRiffWave(w);
                            break;

                        //Game stream.
                        case "fstm":
                        case "cstm":
                            b_stm s = new b_stm();
                            s.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            r = RiffWaveFactory.CreateRiffWave(s);
                            break;

                        //Wave.
                        case ".wav":
                            r.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            break;

                    }

                    if (file.data.data.Count == 0) {
                        //file.stream.loopEnd = r.data.channels[0].
                    }

                }
                */

            }

            //Add region.
            else if (tree.SelectedNode.Index == 3) {

                file.regions.Add(new FISP.RegionInfo() {
                    start = 0,
                    end = file.data.data.Count > 0 ? (uint)file.data.data[0].Length : 0
                });
                updateNodes();

            }

        }

        private void expand_Click(object sender, EventArgs e)
        {
            tree.SelectedNode.Expand();
        }

        private void collapse_Click(object sender, EventArgs e)
        {
            tree.SelectedNode.Collapse();
        }

        #endregion


        //Child nodes.
        #region childNodes

        /// <summary>
        /// Move item up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (tree.SelectedNode.Index != 0)
            {

                //Move channel up.
                if (tree.SelectedNode.Parent.Index == 1)
                {

                    if (tree.SelectedNode.Index >= 1)
                    {

                        Int16[] temp = file.data.data[tree.SelectedNode.Index];
                        Int16[] temp2 = file.data.data[tree.SelectedNode.Index - 1];
                        file.data.data[tree.SelectedNode.Index] = temp2; ;
                        file.data.data[tree.SelectedNode.Index - 1] = temp;
                        loadChannelFiles();

                    }

                }

                //Move track up.
                else if (tree.SelectedNode.Parent.Index == 2)
                {

                    if (tree.SelectedNode.Index >= 1)
                    {

                        FISP.TrackInfo temp = file.tracks[tree.SelectedNode.Index];
                        FISP.TrackInfo temp2 = file.tracks[tree.SelectedNode.Index - 1];
                        file.tracks[tree.SelectedNode.Index] = temp2; ;
                        file.tracks[tree.SelectedNode.Index - 1] = temp;

                    }

                }

                //Move region up.
                else if (tree.SelectedNode.Parent.Index == 3) {

                    if (tree.SelectedNode.Index >= 1)
                    {

                        FISP.RegionInfo temp = file.regions[tree.SelectedNode.Index];
                        FISP.RegionInfo temp2 = file.regions[tree.SelectedNode.Index - 1];
                        file.regions[tree.SelectedNode.Index] = temp2; ;
                        file.regions[tree.SelectedNode.Index - 1] = temp;

                    }

                }

                updateNodes();

            }

        }


        /// <summary>
        /// Move item down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveDownItem_Click(object sender, EventArgs e)
        {

            if (tree.SelectedNode.Index != 0)
            {

                //Move channel down.
                if (tree.SelectedNode.Parent.Index == 1)
                {

                    if (tree.SelectedNode.Index < file.data.data.Count)
                    {

                        Int16[] temp = file.data.data[tree.SelectedNode.Index];
                        Int16[] temp2 = file.data.data[tree.SelectedNode.Index + 1];
                        file.data.data[tree.SelectedNode.Index] = temp2; ;
                        file.data.data[tree.SelectedNode.Index + 1] = temp;
                        loadChannelFiles();

                    }

                }

                //Move track up.
                else if (tree.SelectedNode.Parent.Index == 2)
                {

                    if (tree.SelectedNode.Index < file.tracks.Count)
                    {

                        FISP.TrackInfo temp = file.tracks[tree.SelectedNode.Index];
                        FISP.TrackInfo temp2 = file.tracks[tree.SelectedNode.Index + 1];
                        file.tracks[tree.SelectedNode.Index] = temp2; ;
                        file.tracks[tree.SelectedNode.Index + 1] = temp;

                    }

                }

                //Move region up.
                else if (tree.SelectedNode.Parent.Index == 3)
                {

                    if (tree.SelectedNode.Index < file.regions.Count)
                    {

                        FISP.RegionInfo temp = file.regions[tree.SelectedNode.Index];
                        FISP.RegionInfo temp2 = file.regions[tree.SelectedNode.Index + 1];
                        file.regions[tree.SelectedNode.Index] = temp2; ;
                        file.regions[tree.SelectedNode.Index + 1] = temp;

                    }

                }

                updateNodes();

            }

        }


        /// <summary>
        /// Export WAV.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportWAVToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            saveWaveBox.ShowDialog();
            RiffWave r = new RiffWave();
            bool error = false;
            if (saveWaveBox.FileName != "" && file.data.data.Count > 0)
            {

                switch (tree.SelectedNode.Parent.Index)
                {

                    //Channel.
                    case 1:
                        short[][] c = new short[1][];
                        c[0] = file.data.data[tree.SelectedNode.Index];
                        if (file.stream.isLoop) {
                            r = RiffWaveFactory.CreateRiffWave(file.stream.sampleRate, 2, c, file.stream.loopStart, file.stream.loopEnd);
                        }
                        else {
                            r = RiffWaveFactory.CreateRiffWave(file.stream.sampleRate, 2, c);
                        }
                        break;

                    //Track.
                    case 2:
                        List<short[]> t = new List<short[]>();
                        try
                        {
                            foreach (byte trac in file.tracks[tree.SelectedNode.Index].channels)
                            {
                                t.Add(file.data.data[trac]);
                            }
                        }
                        catch {
                            MessageBox.Show("You can't export a track with channels that don't exist!");
                            error = true;
                        }
                        if (file.stream.isLoop)
                        {
                            r = RiffWaveFactory.CreateRiffWave(file.stream.sampleRate, 2, t.ToArray(), file.stream.loopStart, file.stream.loopEnd);
                        }
                        else
                        {
                            r = RiffWaveFactory.CreateRiffWave(file.stream.sampleRate, 2, t.ToArray());
                        }
                        break;

                    //Region.
                    case 3:
                        List<short[]> n = new List<short[]>();
                        try
                        {
                            foreach (short[] s in file.data.data)
                            {
                                List<short> l = new List<short>();
                                for (uint i = file.regions[tree.SelectedNode.Index].start; i <= file.regions[tree.SelectedNode.Index].end; i++)
                                {
                                    l.Add(s[i]);
                                }
                                n.Add(l.ToArray());
                            }
                        } catch {
                            MessageBox.Show("Region contains samples that don't exist.");
                            error = true;
                        }
                        r = RiffWaveFactory.CreateRiffWave(file.stream.sampleRate, 2, n.ToArray(), 0, (uint)n[0].Length);
                        break;

                }

                if (!error) {
                    File.WriteAllBytes(saveWaveBox.FileName, r.ToBytes());
                }

            }
            saveWaveBox.FileName = "";

        }


        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Stop players.
            for (int i = 0; i < players.Length; i++)
            {
                try { players[i].soundOut.Dispose(); } catch { }
                players[i].soundOut = null;

                players[i].player.Stop();

                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

            }

            try {
                playPauseButton.Image = Resources.Play;
            } catch { }
            playing = false;

            //Channels.
            if (tree.SelectedNode.Parent.Index == 1)
            {

                file.data.data.RemoveAt(tree.SelectedNode.Index);
                loadChannelFiles();

            }

            //Tracks.
            else if (tree.SelectedNode.Parent.Index == 2)
            {

                file.tracks.RemoveAt(tree.SelectedNode.Index);

            }

            //Regions.
            else if (tree.SelectedNode.Parent.Index == 3) {

                file.regions.RemoveAt(tree.SelectedNode.Index);

            }

            updateNodes();

        }

        #endregion


        //Channel player.
        #region channelPlayer

        /// <summary>
        /// Play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playButtonChannel_Click(object sender, EventArgs e)
        {

            if (!playing)
            {
                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {

                        if (players[i].soundOut == null)
                        {

                            players[i].soundOut = new CSCore.SoundOut.WasapiOut();
                            players[i].soundOut.Initialize(players[i].source);

                        }

                        else if (players[i].soundOut.PlaybackState == CSCore.SoundOut.PlaybackState.Playing)
                        {
                            try { players[i].soundOut.Stop(); } catch { }
                            try { players[i].soundOut.Dispose(); } catch { }

                            //players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                            players[i].soundOut = new CSCore.SoundOut.WasapiOut();
                            players[i].soundOut.Initialize(players[i].source);
                        }
                    }

                    foreach (channelPlayer p in players)
                    {
                        if (p.soundOut != null)
                        {
                            p.soundOut.Volume = (float)volume.Value / (float)100;
                        }
                    }

                    channelPlaying = tree.SelectedNode.Index;
                    players[tree.SelectedNode.Index].soundOut.Play();

                    playing = true;

                }
                catch
                {
                    MessageBox.Show("You can't play channels that don't exist! :p");
                }

            }
            else
            {
                stopButtonChannel_Click(sender, e);
                playButtonChannel_Click(sender, e);
            }

        }


        /// <summary>
        /// Pause button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseButtonChannel_Click(object sender, EventArgs e)
        {
            foreach (channelPlayer p in players)
            {
                if (p.soundOut != null)
                {
                    p.soundOut.Pause();
                }
            }

            playing = false;
        }


        /// <summary>
        /// Stop button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopButtonChannel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Length; i++)
            {
                try { players[i].soundOut.Stop(); } catch { }
                try { players[i].soundOut.Dispose(); } catch { }
                players[i].soundOut = null;
                players[i].source.Position = 0;

            }

            playing = false;
        }


        #endregion


        //Saving
        #region saving

        /// <summary>
        /// Save the file click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen) save();
        }


        /// <summary>
        /// Save as file click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen) saveAs();
        }

        /// <summary>
        /// Save the file. WAR INCOMPLETE.
        /// </summary>
        public void save() {

            if (fileNamePath == "")
            {
                saveAs();
            }

            else {

                //Normal mode.
                if (launchMode == 0) { File.WriteAllBytes(fileNamePath, file.ToBytes()); }

                //Other mode.
                else if (launchMode == 1) {

                    //Save and refresh.
                    (war.File as SoundWaveArchive)[fileIndex].Wav = WaveFactory.CreateWave(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision);
                    war.UpdateNodes();
                    war.DoInfoStuff();

                } else {

                    //Save and refresh.
                    if (outModeWave) {
                        (mainWindow.File.Files[fileIndex].File as Wave).Wav = WaveFactory.CreateWave(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision);
                    } else {
                        (mainWindow.File.Files[fileIndex].File as CitraFileLoader.Stream).Stm = StreamFactory.CreateStream(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision);
                    }
                    mainWindow.UpdateNodes();
                    mainWindow.DoInfoStuff();

                }

            }
        }

        /// <summary>
        /// Save as.
        /// </summary>
        public void saveAs() {

            saveFISP.ShowDialog();

            if (saveFISP.FileName != "") {

                if (launchMode == 0)
                {

                    fileNamePath = saveFISP.FileName;
                    saveFISP.FileName = "";
                    this.Text = "Isabelle Sound Editor - " + Path.GetFileName(fileNamePath);
                    save();

                } else {

                    File.WriteAllBytes(saveFISP.FileName, file.ToBytes());
                    saveFISP.FileName = "";

                }

            }
        }

        #endregion


        //Opening.
        #region opening

        /// <summary>
        /// Open a project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openProject.ShowDialog();
            if (openProject.FileName != "") {

                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                noInfoPanel.Show();

                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].player.Stop();
                        try { players[i].soundOut.Stop(); } catch { }
                        try { players[i].soundOut.Dispose(); } catch { }
                        players[i].soundOut = null;

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);

                    }

                    try {
                        playPauseButton.Image = Resources.Play;
                    } catch { }
                    playing = false;

                }

                catch { }

                if (launchMode == 0)
                {
                    fileNamePath = openProject.FileName;
                    this.Text = "Isabelle Sound Editor - " + Path.GetFileName(fileNamePath);
                    openProject.FileName = "";
                    fileOpen = true;
                }

                //Open project.
                if (fileNamePath.EndsWith(".cisp"))
                {
                    CISP c = new CISP();
                    c.load(File.ReadAllBytes(fileNamePath));
                }
                else {
                    file = new FISP(File.ReadAllBytes(fileNamePath));
                }

                updateNodes();
                loadChannelFiles();

                tree.SelectedNode = tree.Nodes[0];
                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                regionPanel.Hide();
                noInfoPanel.Show();

            }
        }

        #endregion


        //Track Player.
        #region trackPlayer

        /// <summary>
        /// Play a track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playSoundTrack_Click(object sender, EventArgs e)
        {

            if (!playing)
            {
                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {

                        if (players[i].soundOut == null)
                        {

                            players[i].soundOut = new CSCore.SoundOut.WasapiOut();
                            players[i].soundOut.Initialize(players[i].source);

                        }

                        else if (players[i].soundOut.PlaybackState == CSCore.SoundOut.PlaybackState.Playing)
                        {
                            try { players[i].soundOut.Stop(); } catch { }
                            try { players[i].soundOut.Dispose(); } catch { }

                            //players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                            players[i].soundOut = new CSCore.SoundOut.WasapiOut();
                            players[i].soundOut.Initialize(players[i].source);
                        }
                    }

                    foreach (channelPlayer p in players)
                    {
                        if (p.soundOut != null)
                        {
                            p.soundOut.Volume = (float)volume.Value / (float)100;
                        }
                    }

                    for (int i = 0; i < file.tracks[tree.SelectedNode.Index].channels.Count; i++)
                    {
                        channelPlaying = file.tracks[tree.SelectedNode.Index].channels[0];
                        players[file.tracks[tree.SelectedNode.Index].channels[i]].soundOut.Play();
                    }

                    playing = true;

                }
                catch
                {
                    MessageBox.Show("You can't play channels that don't exist! :p");
                }

            }
            else {
                stopSoundTrack_Click(sender, e);
                playSoundTrack_Click(sender, e);
            }

        }


        /// <summary>
        /// Pause a track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseSoundTrack_Click(object sender, EventArgs e)
        {
            foreach (channelPlayer p in players)
            {
                if (p.soundOut != null)
                {
                    p.soundOut.Pause();
                }
            }

            playing = false;
        }


        /// <summary>
        /// Stop a track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopSoundTrack_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Length; i++)
            {
                try { players[i].soundOut.Stop(); } catch { }
                try { players[i].soundOut.Dispose(); } catch { }
                players[i].soundOut = null;
                players[i].source.Position = 0;

            }

            playing = false;
        }

        #endregion


        //Load channel files.
        #region loadChannelFiles

        /// <summary>
        /// Load channel files.
        /// </summary>
        public void loadChannelFiles() {

            if (players != null)
            {
                for (int i = 0; i < players.Count(); i++)
                {
                    try { players[i].source.Dispose(); } catch { }
                    try { players[i].soundOut.Dispose(); } catch { }
                }
            }

            players = new channelPlayer[file.data.data.Count];
            for (int i = 0; i < file.data.data.Count; i++) {

                //Write the RIFF.
                MemoryStream channelData = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(channelData);
                bw.Write(file.data.data[i]);

                players[i].file = channelData.ToArray();
                players[i].player = new WaveOutEvent();
                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(channelData.ToArray()), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

                //CSCore.WaveFormat wf = new CSCore.WaveFormat((int)file.stream.sampleRate, 16, 1, AudioEncoding.Pcm);

                try { players[i].source.Dispose(); } catch { }
                players[i].source = null;

                RiffWave r = new RiffWave();
                r.fmt = new RiffWave.FmtChunk(1, file.stream.sampleRate, 2);

                //Write data from players[i].file
                Int16[][] samples = new short[1][];
                samples[0] = file.data.data[i].ToArray();
                r.data = new RiffWave.DataChunk(samples, r.fmt);    
                r.Update();
                players[i].source = new CSCore.MediaFoundation.MediaFoundationDecoder(new MemoryStream(r.ToBytes()));

            }

        }

        #endregion


        //Volume Stuff.
        #region volumeStuff 

        private void volume_Scroll(object sender, EventArgs e)
        {
            try {
                foreach (channelPlayer p in players)
                {
                    if (p.player != null)
                    {
                        p.player.Volume = (float)volume.Value / (float)100;
                    }
                }
            } catch { }
        }

        #endregion


        //Main Player.
        #region mainPlayer

        /// <summary>
        /// Play pause the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playPauseButton_Click(object sender, EventArgs e)
        {

            channelPlaying = 0;

            if (playing)
            {

                foreach (channelPlayer p in players)
                {
                    if (p.soundOut != null)
                    {
                        p.soundOut.Pause();
                    }
                }

                try {
                    playPauseButton.Image = Resources.Play;
                } catch { }
                playing = false;

            }
            else
            {

                for (int i = 0; i < players.Length; i++)
                {

                    if (players[i].soundOut == null) {

                        players[i].soundOut = new CSCore.SoundOut.WasapiOut();
                        players[i].soundOut.Initialize(players[i].source);

                    }

                    else if (players[i].soundOut.PlaybackState == CSCore.SoundOut.PlaybackState.Playing)
                    {
                        try { players[i].soundOut.Stop(); } catch { }
                        try { players[i].soundOut.Dispose(); } catch { }

                        //players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].soundOut = new CSCore.SoundOut.WasapiOut();
                        players[i].soundOut.Initialize(players[i].source);
                    }
                }

                foreach (channelPlayer p in players)
                {
                    if (p.soundOut != null)
                    {
                        p.soundOut.Volume = (float)volume.Value / (float)100;
                    }
                }

                for (int i = 0; i < file.data.data.Count; i++)
                {
                    players[i].soundOut.Play();
                }

                try {
                    playPauseButton.Image = Resources.Pause;
                } catch { }
                playing = true;

            }
        }


        /// <summary>
        /// Stop the audio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Length; i++)
            {
                try { players[i].soundOut.Stop(); } catch { }
                try { players[i].soundOut.Dispose(); } catch { }
                players[i].soundOut = null;
                players[i].source.Position = 0;

            }

            try {
                playPauseButton.Image = Resources.Play;
            } catch { }
            playing = false;

        }

        #endregion


        //Loop
        #region loop

        /// <summary>
        /// Loop the thing.
        /// </summary>
        public void loop() {

            while (true) {

                if (!scrolling) try { timeBar.Value = (int)(((decimal)players[channelPlaying].source.Position / ((decimal)file.data.data[channelPlaying].Length * 2)) * 1440); if (playLikeGameBox.Checked) { if (players[channelPlaying].source.Position >= file.stream.loopEnd*2 && players[channelPlaying].soundOut.PlaybackState == CSCore.SoundOut.PlaybackState.Playing) { for (int i = 0; i < players.Count(); i++) { players[i].source.Position = (long)file.stream.loopStart*2; } } } } catch { }
                bool regnMode = false;

                try
                {
                    if (!scrollingLeft) spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);
                    if (!scrollingRight) spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.data.data[channelPlaying].Length * (decimal)pnlLoop.Size.Width);

                    //Region stuff.
                    if (tree.SelectedNode != null && fileOpen) {

                        if (tree.SelectedNode.Parent != null) {

                            if (tree.SelectedNode.Parent.Index == 3)
                            {

                                regnStart.SplitPosition = (int)((decimal)file.regions[tree.SelectedNode.Index].start / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);
                                regnEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.regions[tree.SelectedNode.Index].end / (decimal)file.data.data[channelPlaying].Length * (decimal)pnlLoop.Size.Width);
                                if (!scrolling && regionPreviewBox.Checked) try { timeBar.Value = (int)(((decimal)players[channelPlaying].source.Position / ((decimal)file.data.data[channelPlaying].Length * 2)) * 1440); if ((players[channelPlaying].source.Position > file.regions[tree.SelectedNode.Index].end * 2 || players[channelPlaying].source.Position < file.regions[tree.SelectedNode.Index].start * 2) && players[channelPlaying].soundOut.PlaybackState == CSCore.SoundOut.PlaybackState.Playing) { for (int i = 0; i < players.Count(); i++) { players[i].source.Position = (long)file.regions[tree.SelectedNode.Index].start * 2; } } } catch { }

                                regnMode = true;

                            }

                        }

                    }

                }
                catch { }

                try {
                    if (file.data.data.Count() == 0) {

                        playPauseButton.Enabled = false;
                        stopButton.Enabled = false;
                        timeBar.Enabled = false;
                        spltStart.Enabled = false;
                        spltEnd.Enabled = false;
                        setLoopStartButton.Enabled = false;
                        setLoopEndButton.Enabled = false;

                    } else {

                        if (regnMode) {

                            spltStart.Enabled = false;
                            spltEnd.Enabled = false;
                            setLoopStartButton.Enabled = false;
                            setLoopEndButton.Enabled = false;
                            regnStart.Visible = true;
                            regnEnd.Visible = true;
                            spltStart.Visible = false;
                            spltEnd.Visible = false;
                            
                        }
                        else {

                            spltStart.Enabled = true;
                            spltEnd.Enabled = true;
                            setLoopStartButton.Enabled = true;
                            setLoopEndButton.Enabled = true;
                            regnStart.Visible = false;
                            regnEnd.Visible = false;
                            spltStart.Width = 3;
                            spltEnd.Width = 3;
                            spltStart.Visible = true;
                            spltEnd.Visible = true;

                        }

                        playPauseButton.Enabled = true;
                        stopButton.Enabled = true;
                        timeBar.Enabled = true;   

                    }
                } catch { }

            }

        }

        #endregion


        //Loop buttons.
        #region loopButtons

        /// <summary>
        /// Set the starting loop position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setLoopStartButton_Click(object sender, EventArgs e)
        {
            file.stream.isLoop = true;
            file.stream.loopStart = (UInt32)(players[0].source.Position/2);
            doInfoStuff();

            spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);

            if (file.stream.loopStart >= file.data.data[0].Length) { file.stream.loopStart = (UInt32)file.data.data[0].Length; }

        }


        /// <summary>
        /// Set the ending loop position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setLoopEndButton_Click(object sender, EventArgs e)
        {
            file.stream.isLoop = true;
            file.stream.loopEnd = (UInt32)(players[0].source.Position / 2);
            doInfoStuff();

            spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);

            if (file.stream.loopEnd >= file.data.data[0].Length) { file.stream.loopEnd = (UInt32)file.data.data[0].Length; }

        }

        #endregion


        //Tabs.
        #region Tabs

        /// <summary>
        /// Convert file to RIFF.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportWavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen)
            {
                if (file.data.data.Count > 0)
                {
                    saveWaveBox.ShowDialog();
                    if (saveWaveBox.FileName != "")
                    {

                        File.WriteAllBytes(saveWaveBox.FileName, RiffWaveFactory.CreateRiffWave(file).ToBytes());
                        saveWaveBox.FileName = "";

                    }
                }
                else
                {

                    MessageBox.Show("You need at least one channel to save first!");

                }
            }
            else
            {

                MessageBox.Show("You... don't even have a file open...");

            }
        }


        /// <summary>
        /// Import file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int j = 0;
            if (fileOpen)
            {

                SaveCloseDialog c = new SaveCloseDialog();
                j = c.getValue();
                if (j == 0) { save(); }

            }
            if (j == 0 || j == 1)
            {

                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].player.Stop();

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);

                    }



                    try {
                        playPauseButton.Image = Resources.Play;
                    } catch { }
                    playing = false;

                }

                catch { }

                if (launchMode == 0)
                {
                    //General stuff.
                    fileOpen = true;
                    this.Text = "Isabelle Sound Editor";
                }

                fileOpen = false;

                updateNodes();

                tree.SelectedNode = tree.Nodes[0];
                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                regionPanel.Hide();
                noInfoPanel.Show();

                anyFileSelectorSound.ShowDialog();
                if (anyFileSelectorSound.FileName != "")
                {

                    file = new FISP();
                    fileOpen = true;
                    if (launchMode == 0) { this.Text = "Isabelle Sound Editor - New Project.fisp"; }

                    switch (anyFileSelectorSound.FileName.Substring(anyFileSelectorSound.FileName.Length - 4))
                    {

                        //Wave.
                        case ".wav":
                            RiffWave w = new RiffWave();
                            w.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            file = new FISP(w);
                            break;

                        //Game wave.
                        case "fwav":
                        case "cwav":
                            b_wav b = new b_wav();
                            b.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            file = new FISP(b);
                            break;

                        //Game stream.
                        case "fstm":
                        case "cstm":
                            b_stm s = new b_stm();
                            s.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            file = new FISP(s);
                            break;

                        //Binary wave.
                        case "bwav":
                            BinaryWave r = new BinaryWave();
                            r.Load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            file = new FISP(r);
                            break;

                    }

                    loadChannelFiles();
                    updateNodes();

                }
                else
                {
                    if (launchMode == 0) { this.Text = "Isabelle Sound Editor"; }
                }

                anyFileSelectorSound.FileName = "";

            }
        }


        /// <summary>
        /// Fix resizing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formResized(object sender, EventArgs e)
        {
            if (playing)
            {

                try { timeBar.Value = (int)(((decimal)players[0].player.GetPosition() / ((decimal)file.data.data[0].Length * 2)) * 1440); } catch { }

            }
            else
            {

                try { if (players[0].player.PlaybackState == PlaybackState.Paused) { timeBar.Value = (int)(((decimal)players[0].player.GetPosition() / ((decimal)file.data.data[0].Length * 2)) * 1440); } } catch { }

            }

            try
            {
                spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);
                spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.data.data[0].Length * (decimal)pnlLoop.Size.Width);
            }
            catch { }
        }


        /// <summary>
        /// Export binary button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (fileOpen)
            {

                if (file.data.data.Count() > 0)
                {

                    saveGameFile.ShowDialog();
                    if (saveGameFile.FileName != "")
                    {

                        bool forceSwitch = false;
                        if (saveGameFile.FilterIndex > 5) {
                            forceSwitch = true;
                        }

                        //See what file to save.
                        switch (saveGameFile.FileName.Substring(saveGameFile.FileName.Length - 4).ToLower())
                        {

                            //FWAV.
                            case "fwav":
                                File.WriteAllBytes(saveGameFile.FileName, WaveFactory.CreateWave(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision).ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                break;

                            //CWAV.
                            case "cwav":
                                File.WriteAllBytes(saveGameFile.FileName, WaveFactory.CreateWave(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision).ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                break;

                            //FSTM.
                            case "fstm":
                                File.WriteAllBytes(saveGameFile.FileName, StreamFactory.CreateStream(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision).ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                break;

                            //CSTM.
                            case "cstm":
                                File.WriteAllBytes(saveGameFile.FileName, StreamFactory.CreateStream(file, file.stream.vMajor, file.stream.vMinor, file.stream.vRevision).ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                break;

                            //BWAV.
                            case "bwav":
                                File.WriteAllBytes(saveGameFile.FileName, BinaryWave.FromFISP(file).ToBytes());
                                break;

                        }

                        saveGameFile.FileName = "";

                    }

                }
                else {
                    MessageBox.Show("You need at least one channel in order to export a binary!");
                }

            }
            else {
                MessageBox.Show("You need to have a file open first!");
            }

        }


        /// <summary>
        /// Close the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int result = 2;
            if (fileOpen)
            {

                SaveCloseDialog s = new SaveCloseDialog();
                result = s.getValue();

                if (result == 0)
                {
                    save();
                }

            }

            if (fileOpen && (result == 1 || result == 0))
            {

                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].player.Stop();
                        try { players[i].soundOut.Stop(); } catch { }
                        try { players[i].soundOut.Dispose(); } catch { }
                        players[i].soundOut = null;

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new NAudio.Wave.WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);

                    }

                    try {
                        playPauseButton.Image = Resources.Play;
                    } catch { }
                    playing = false;

                }

                catch { }


                //General stuff.
                fileNamePath = "";
                fileOpen = true;
                this.Text = "Isabelle Sound Editor";

                //Make new FISP.
                file = new FISP();
                file.stream = new FISP.StreamInfo();
                file.tracks = new List<FISP.TrackInfo>();
                file.regions = new List<FISP.RegionInfo>();
                file.data = new FISP.DataInfo();
                file.data.data = new List<short[]>();

                fileOpen = false;

                updateNodes();

                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                regionPanel.Hide();
                noInfoPanel.Show();

                tree.SelectedNode = tree.Nodes[0];
                spltStart.SplitPosition = 0;
                spltEnd.SplitPosition = 0;

            }

        }


        /// <summary>
        /// Quit the editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (fileOpen)
            {

                SaveCloseDialog s = new SaveCloseDialog();
                int result = s.getValue();

                if (result == 0)
                {
                    save();
                }
                else if (result == 1)
                {
                    this.Close();
                }

            }
            else {

                this.Close();

            }

        }


        /// <summary>
        /// Game file to game file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameFileToGameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            gameFileSelectorBox.ShowDialog();

            if (gameFileSelectorBox.FileName != "") {

                //See what type of game file.
                switch (gameFileSelectorBox.FileName.Substring(gameFileSelectorBox.FileName.Length - 4)) {

                    //Game Wave.
                    case "fwav":
                    case "cwav":

                        b_wav b = new b_wav();
                        b.Load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        saveGameFile.ShowDialog();

                        if (saveGameFile.FileName != "") {

                            bool forceSwitch = false;
                            if (saveGameFile.FilterIndex > 5) {
                                forceSwitch = true;
                            }

                            VersionSelector v = new VersionSelector();
                            v.GetValues(b.fileHeader.vMajor, b.fileHeader.vMinor, b.fileHeader.vRevision);
                            byte maj = (byte)v.maj.Value;
                            byte min = (byte)v.min.Value;
                            byte rev = (byte)v.rev.Value;
                            b.fileHeader.vMajor = maj;
                            b.fileHeader.vMinor = min;
                            b.fileHeader.vRevision = rev;

                            //Save game file.
                            switch (saveGameFile.FileName.Substring(saveGameFile.FileName.Length - 4).ToLower()) {

                                //FWAV.
                                case "fwav":
                                    File.WriteAllBytes(saveGameFile.FileName, b.ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                    break;

                                //CWAV.
                                case "cwav":
                                    File.WriteAllBytes(saveGameFile.FileName, b.ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                    break;

                                //FSTM.
                                case "fstm":
                                    File.WriteAllBytes(saveGameFile.FileName, StreamFactory.CreateStream(b, b.fileHeader.vMajor, b.fileHeader.vMinor, b.fileHeader.vRevision).ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                    break;

                                //CSTM.
                                case "cstm":
                                    File.WriteAllBytes(saveGameFile.FileName, StreamFactory.CreateStream(b, b.fileHeader.vMajor, b.fileHeader.vMinor, b.fileHeader.vRevision).ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                    break;

                                //BWAV.
                                case "bwav":
                                    File.WriteAllBytes(saveGameFile.FileName, new BinaryWave(b).ToBytes());
                                    break;

                            }

                        }

                        saveGameFile.FileName = "";

                        break;

                    //Game stream.
                    case "fstm":
                    case "cstm":

                        b_stm s = new b_stm();
                        s.Load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        saveGameFile.ShowDialog();

                        if (saveGameFile.FileName != "")
                        {

                            bool forceSwitch = false;
                            if (saveGameFile.FilterIndex > 5) {
                                forceSwitch = true;
                            }

                            //Save game file.
                            switch (saveGameFile.FileName.Substring(saveGameFile.FileName.Length - 4).ToLower())
                            {

                                //FWAV.
                                case "fwav":
                                    File.WriteAllBytes(saveGameFile.FileName, WaveFactory.CreateWave(s, s.fileHeader.vMajor, s.fileHeader.vMinor, s.fileHeader.vRevision).ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                    break;

                                //CWAV.
                                case "cwav":
                                    File.WriteAllBytes(saveGameFile.FileName, WaveFactory.CreateWave(s, s.fileHeader.vMajor, s.fileHeader.vMinor, s.fileHeader.vRevision).ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                    break;

                                //FSTM.
                                case "fstm":
                                    File.WriteAllBytes(saveGameFile.FileName, s.ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                    break;

                                //CSTM.
                                case "cstm":
                                    File.WriteAllBytes(saveGameFile.FileName, s.ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                    break;

                                //BWAV.
                                case "bwav":
                                    File.WriteAllBytes(saveGameFile.FileName, new BinaryWave(s).ToBytes());
                                    break;

                            }

                        }

                        saveGameFile.FileName = "";

                        break;

                    //Binary wave.
                    case "bwav":

                        BinaryWave w = new BinaryWave();
                        w.Load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        saveGameFile.ShowDialog();

                        if (saveGameFile.FileName != "") {

                            bool forceSwitch = false;
                            if (saveGameFile.FilterIndex > 5) {
                                forceSwitch = true;
                            }

                            //Save game file.
                            switch (saveGameFile.FileName.Substring(saveGameFile.FileName.Length - 4).ToLower()) {

                                //FWAV.
                                case "fwav":
                                    File.WriteAllBytes(saveGameFile.FileName, WaveFactory.CreateWave(w, 1, 0, 0).ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                    break;

                                //CWAV.
                                case "cwav":
                                    File.WriteAllBytes(saveGameFile.FileName, WaveFactory.CreateWave(w, 1, 0, 0).ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                    break;

                                //FSTM.
                                case "fstm":
                                    File.WriteAllBytes(saveGameFile.FileName, StreamFactory.CreateStream(w, 4, 0, 0).ToBytes(forceSwitch ? CitraFileLoader.ByteOrder.LittleEndian : CitraFileLoader.ByteOrder.BigEndian, forceSwitch));
                                    break;

                                //CSTM.
                                case "cstm":
                                    File.WriteAllBytes(saveGameFile.FileName, StreamFactory.CreateStream(w, 4, 0, 0).ToBytes(CitraFileLoader.ByteOrder.LittleEndian));
                                    break;

                                //BWAV.
                                case "bwav":
                                    File.WriteAllBytes(saveGameFile.FileName, w.ToBytes());
                                    break;

                            }

                        }

                        saveGameFile.FileName = "";

                        break;

                }

            }

            gameFileSelectorBox.FileName = "";

        }


        /// <summary>
        /// HELP!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://discord.gg/6VDPGne");
        }


        /// <summary>
        /// When the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void formClosing(object sender, System.EventArgs e) {

            if (players != null)
            {
                for (int i = 0; i < players.Count(); i++)
                {
                    try { players[i].player.Stop(); } catch { }
                    try { players[i].soundOut.Stop(); } catch { }
                    try { players[i].soundOut.Dispose(); } catch { }
                    try { players[i].soundOut = null; } catch { }
                    try { players[i].player.Dispose(); } catch { }
                    try { players[i].source.Dispose(); } catch { }
                }
            }

        }


        /// <summary>
        /// Convert game file to wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleGameFileToWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            gameFileSelectorBox.ShowDialog();
            if (gameFileSelectorBox.FileName != "")
            {

                //Game wave.
                if (gameFileSelectorBox.FileName.ToLower().EndsWith("cwav") || gameFileSelectorBox.FileName.ToLower().EndsWith("fwav")) {

                    //Have user select output.
                    saveWaveBox.ShowDialog();
                    if (saveWaveBox.FileName != "") {

                        //Do actual conversions.
                        b_wav b = new b_wav();
                        b.Load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        VersionSelector v = new VersionSelector();
                        v.GetValues(b.fileHeader.vMajor, b.fileHeader.vMinor, b.fileHeader.vRevision);
                        byte maj = (byte)v.maj.Value;
                        byte min = (byte)v.min.Value;
                        byte rev = (byte)v.rev.Value;
                        b.fileHeader.vMajor = maj;
                        b.fileHeader.vMinor = min;
                        b.fileHeader.vRevision = rev;

                        File.WriteAllBytes(saveWaveBox.FileName, RiffWaveFactory.CreateRiffWave(b).ToBytes());

                    }

                }

                //Game stream.
                else if (gameFileSelectorBox.FileName.ToLower().EndsWith("cstm") || gameFileSelectorBox.FileName.ToLower().EndsWith("fstm")) {

                    //Have user select output.
                    saveWaveBox.ShowDialog();
                    if (saveWaveBox.FileName != "") {

                        //Do actual conversions.
                        b_stm b = new b_stm();
                        b.Load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        VersionSelector v = new VersionSelector();
                        v.GetValues(b.fileHeader.vMajor, b.fileHeader.vMinor, b.fileHeader.vRevision);
                        byte maj = (byte)v.maj.Value;
                        byte min = (byte)v.min.Value;
                        byte rev = (byte)v.rev.Value;
                        b.fileHeader.vMajor = maj;
                        b.fileHeader.vMinor = min;
                        b.fileHeader.vRevision = rev;

                        File.WriteAllBytes(saveWaveBox.FileName, RiffWaveFactory.CreateRiffWave(b).ToBytes());

                    }

                }

                //Binary wave.
                else {

                    //Have user select output.
                    saveWaveBox.ShowDialog();
                    if (saveWaveBox.FileName != "") {

                        //Do actual conversions.
                        BinaryWave b = new BinaryWave();
                        b.Load(File.ReadAllBytes(gameFileSelectorBox.FileName));
                        File.WriteAllBytes(saveWaveBox.FileName, RiffWaveFactory.CreateRiffWave(b).ToBytes());

                    }

                }

                gameFileSelectorBox.FileName = "";
                saveWaveBox.FileName = "";

            }

        }


        /// <summary>
        /// About menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutIsabelleSoundEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsabelleAbout a = new IsabelleAbout();
            a.Show();
        }

        #endregion


        //Allow scrolling.
        #region scrolling

        /// <summary>
        /// User touches time bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeBar_MouseDown(object sender, MouseEventArgs e)
        {

            scrolling = true;

        }


        /// <summary>
        /// Mouse released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeBar_MouseUp(object sender, MouseEventArgs e)
        {

            scrolling = false;

            if (true) {

                try
                {

                    //timeBar.Value = (int)(((decimal)players[0].source.Position / ((decimal)file.channelData[0].Length * 2)) * 1440);
                    for (int i = 0; i < players.Count(); i++)
                    {
                        players[i].source.Position = (long)(((decimal)file.data.data[0].Length) * 2 * ((decimal)timeBar.Value / (decimal)timeBar.Maximum));
                    }

                }
                catch { }

            }

        }


        /// <summary>
        /// Moving loop start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopStart_MouseDown(object sender, MouseEventArgs e)
        {

            scrollingLeft = true;

        }


        /// <summary>
        /// Stop moving loop start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopStart_MouseUp(object sender, MouseEventArgs e)
        {

            scrollingLeft = false;

            if (true)
            {

                try
                {
                    //if (!scrollingLeft) spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);
                    //if (!scrollingRight) spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);

                    file.stream.loopStart = (UInt32)(spltStart.SplitPosition * (decimal)file.data.data[0].Length / (decimal)pnlLoop.Size.Width);
                    doInfoStuff();

                }
                catch { }

            }

        }


        /// <summary>
        /// Moving loop end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopEnd_MouseDown(object sender, MouseEventArgs e)
        {

            scrollingRight = true;

        }


        /// <summary>
        /// Stop moving loop end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopEnd_MouseUp(object sender, MouseEventArgs e)
        {

            scrollingRight = false;

            if (true)
            {

                try
                {

                    try
                    {
                        //if (!scrollingLeft) spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);
                        //if (!scrollingRight) spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);

                        file.stream.loopEnd = (UInt32)((decimal)file.data.data[0].Length * (pnlLoop.Size.Width - spltEnd.SplitPosition) / (decimal)pnlLoop.Size.Width);
                        doInfoStuff();

                    }
                    catch { }

                }
                catch { }

            }

        }


        /// <summary>
        /// Ignore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion


        //Fix loop static.
        #region fixLoopStatic

        /// <summary>
        /// Eliminate loop static.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopStartRoundButton_Click(object sender, EventArgs e)
        {

            int numSamplesPerBlock = 0x3800;

            if (file.stream.encoding == EncodingTypes.PCM8) {
                numSamplesPerBlock = 0x2000;
            }
            else if (file.stream.encoding == EncodingTypes.PCM16) {
                numSamplesPerBlock = 0x1000;
            }

            int rounded = (int)file.stream.loopStart / numSamplesPerBlock;
            uint orig = file.stream.loopStart;
            int test1 = rounded * numSamplesPerBlock;
            int test2 = (rounded + 1) * numSamplesPerBlock;
            uint close = (uint)test1;
            if (Math.Abs(orig - test1) > Math.Abs(orig - test2)) {
                close = (uint)test2;
            } else {
                close = (uint)test1;
            }
            int numChange = (int)file.stream.loopStart - (int)close;
            file.stream.loopStart = (UInt32)close;
            if (numChange >= 0) {
                file.stream.loopEnd -= (uint)Math.Abs(numChange);
            } else {
                file.stream.loopEnd += (uint)Math.Abs(numChange);
            }
            if (file.stream.loopEnd > file.data.data[0].Count()) {
                file.stream.loopEnd = (uint)file.data.data[0].Count();
            }
            loopStartBox.Value = close;
            loopEndBox.Value = file.stream.loopEnd;
            updateNodes();

        }


        /// <summary>
        /// Get closer int.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Closer(int a, int b)
        {
            int calcA = a - 14336;
            int calcB = b - 14336;
            if (Math.Abs(calcA) == Math.Abs(calcB))
            {
                return 0;
            }
            else if ((a >= 14336 || b >= 14336) && a < b)
            {
                return a;
            }
            else if ((a >= 14336 || b >= 14336) && b < a)
            {
                return b;
            }
            else if (calcB > calcA && Math.Abs(a) != Math.Abs(b))
            {
                return b;
            }
            else if ((calcA < 0 || calcB < 0) && (calcA > calcB && Math.Abs(a) != Math.Abs(b)))
            {
                return a;
            }
            else return Closer(a, b);
        }


        #endregion

        /// <summary>
        /// Stream to prefetch.
        /// </summary>
        private void streamToPrefetchToolStripMenuItem_Click(object sender, EventArgs e) {

            OpenFileDialog o = new OpenFileDialog();
            o.RestoreDirectory = true;
            o.Filter = "Stream|*.bfstm;*.bcstm";
            o.ShowDialog();

            if (o.FileName != "") {

                //Load stream.
                b_stm s = new b_stm();
                s.Load(File.ReadAllBytes(o.FileName));

                //Show version thing.
                VersionSelector v = new VersionSelector();
                v.GetValues(s.fileHeader.vMajor, s.fileHeader.vMinor, s.fileHeader.vRevision);

                //Output prefetch.
                SaveFileDialog d = new SaveFileDialog();
                d.RestoreDirectory = true;
                d.Filter = "3ds or Wii U Prefetch|*.bfstp;*.bcstp|Switch Prefetch|*.bfstp";
                d.ShowDialog();

                if (d.FileName != "") {

                    PrefetchFile p = new PrefetchFile() { Stream = new CitraFileLoader.Stream() { Stm = s }, Version = new FileWriter.Version((byte)v.maj.Value, (byte)v.min.Value, (byte)v.rev.Value) };
                    MemoryStream o2 = new MemoryStream();
                    BinaryDataWriter bw = new BinaryDataWriter(o2);

                    if (Path.GetExtension(d.FileName) == "") {
                        d.FileName += ".bfstp";
                    }

                    if (d.FileName.ToLower().EndsWith(".bfstp")) {
                        if (d.FilterIndex == 1) {
                            p.Write(WriteMode.Cafe, bw);
                        } else {
                            p.Write(WriteMode.NX, bw);
                        }
                    } else {
                        p.Write(WriteMode.CTR, bw);
                    }

                    File.WriteAllBytes(d.FileName, o2.ToArray());

                }

            }

        }

        //Time conversion
        #region TimeConversion

        public TimeSpan Samples2TimeSpan(uint samples) {
            if (fileOpen) {
                return new TimeSpan(0, 0, 0, 0, (int)(samples / (decimal)file.stream.sampleRate * 1000));
            } else { return new TimeSpan(); }
        }

        public uint TimeSpan2Samples(TimeSpan t) {
            if (fileOpen) {
                return (uint)(t.TotalMilliseconds / 1000d * file.stream.sampleRate);
            } else { return 0; }
        }

        private void LoopStartBox_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                TimeSpan t = Samples2TimeSpan((uint)loopStartBox.Value);
                loopStartMinutes.Value = (int)t.TotalMinutes;
                loopStartSeconds.Value = t.Seconds;
                loopStartMilliseconds.Value = t.Milliseconds;
                updatingTime = false;
            }
        }

        private void LoopStartMinutes_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                loopStartBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)loopStartMinutes.Value, (int)loopStartSeconds.Value, (int)loopStartMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void LoopStartSeconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                loopStartBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)loopStartMinutes.Value, (int)loopStartSeconds.Value, (int)loopStartMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void LoopStartMilliseconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                loopStartBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)loopStartMinutes.Value, (int)loopStartSeconds.Value, (int)loopStartMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void LoopEndBox_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                TimeSpan t = Samples2TimeSpan((uint)loopEndBox.Value);
                loopEndMinutes.Value = (int)t.TotalMinutes;
                loopEndSeconds.Value = t.Seconds;
                loopEndMilliseconds.Value = t.Milliseconds;
                updatingTime = false;
            }
        }

        private void LoopEndMinutes_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                loopEndBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)loopEndMinutes.Value, (int)loopEndSeconds.Value, (int)loopEndMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void LoopEndSeconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                loopEndBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)loopEndMinutes.Value, (int)loopEndSeconds.Value, (int)loopEndMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void LoopEndMilliseconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                loopEndBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)loopEndMinutes.Value, (int)loopEndSeconds.Value, (int)loopEndMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void OriginalLoopStartBox_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                TimeSpan t = Samples2TimeSpan((uint)originalLoopStartBox.Value);
                originalLoopStartMinutes.Value = (int)t.TotalMinutes;
                originalLoopStartSeconds.Value = t.Seconds;
                originalLoopStartMilliseconds.Value = t.Milliseconds;
                updatingTime = false;
            }
        }

        private void OriginalLoopStartMinutes_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                originalLoopStartBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)originalLoopStartMinutes.Value, (int)originalLoopStartSeconds.Value, (int)originalLoopStartMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void OriginalLoopStartSeconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                originalLoopStartBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)originalLoopStartMinutes.Value, (int)originalLoopStartSeconds.Value, (int)originalLoopStartMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void OriginalLoopStartMilliseconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                originalLoopStartBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)originalLoopStartMinutes.Value, (int)originalLoopStartSeconds.Value, (int)originalLoopStartMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void OriginalLoopEndBox_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                TimeSpan t = Samples2TimeSpan((uint)originalLoopEndBox.Value);
                originalLoopEndMinutes.Value = (int)t.TotalMinutes;
                originalLoopEndSeconds.Value = t.Seconds;
                originalLoopEndMilliseconds.Value = t.Milliseconds;
                updatingTime = false;
            }
        }

        private void OriginalLoopEndMinutes_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                originalLoopEndBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)originalLoopEndMinutes.Value, (int)originalLoopEndSeconds.Value, (int)originalLoopEndMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void OriginalLoopEndSeconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                originalLoopEndBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)originalLoopEndMinutes.Value, (int)originalLoopEndSeconds.Value, (int)originalLoopEndMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void OriginalLoopEndMilliseconds_ValueChanged(object sender, EventArgs e) {
            if (!updatingTime) {
                updatingTime = true;
                originalLoopEndBox.Value = TimeSpan2Samples(new TimeSpan(0, 0, (int)originalLoopEndMinutes.Value, (int)originalLoopEndSeconds.Value, (int)originalLoopEndMilliseconds.Value));
                updatingTime = false;
            }
        }

        private void SecretBox_ValueChanged(object sender, EventArgs e) {

        }

        #endregion

    }

}
