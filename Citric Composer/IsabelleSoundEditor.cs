using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitraFileLoader;
using IsabelleLib;
using NAudio.Wave;
using System.Threading;
using Syroot.BinaryData;
using Softpae.Media;

namespace Citric_Composer
{
    public partial class IsabelleSoundEditor : Form
    {
        public IsabelleSoundEditor()
        {
            InitializeComponent();
            Thread loopThread = new Thread(loop);
            loopThread.IsBackground = true;
            loopThread.Start();
        }

        public bool fileOpen = false; //If a file is open.
        public CISP file; //The file opened. I'm using CISP for simplicity's sake.
        public string fileNamePath = ""; //File name.
        public channelPlayer[] players; //Players.
        public bool playing; //If playing.
        int timer = 0; //Timer.

        //Channel player.
        public struct channelPlayer {
            public byte[] file; //File.
            public WaveOutEvent player; //Player.
            public IWaveProvider playerFile; //Audio File.
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

                    players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                    players[i].player.Init(players[i].playerFile);

                }



                playPauseButton.Image = new Bitmap("Data/Image/play4.png");
                playing = false;

            }

            catch { }

            //General stuff.
            fileNamePath = "";
            fileOpen = true;
            this.Text = "Isabelle Sound Editor - New Project.cisp";

            //Make new CISP.
            file = new CISP();
            file.stream = new CISP.streamInfo();
            file.stream.loop = 0;
            file.stream.loopStart = 0;
            file.stream.loopEnd = 0;
            file.stream.sampleRate = 0xFFFFFFFF;
            file.stream.sampleSize = 0;
            byte[] seek = { 2 };
            file.seekBlock = seek;
            file.tracks = new List<CISP.trackInfo>();
            file.channelData = new List<UInt16[]>();

            updateNodes();

        }
        #endregion



        //Info stuff.
        #region infoStuff
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

                }

                //Track.
                else if (tree.SelectedNode.Parent.Index == 2)
                {

                    trackPanel.Show();
                    projectPanel.Hide();
                    noInfoPanel.Hide();
                    channelPanel.Hide();

                    volumeBox.Value = file.tracks[tree.SelectedNode.Index].volume;
                    panBox.Value = file.tracks[tree.SelectedNode.Index].pan;
                    if (file.tracks[tree.SelectedNode.Index].flags == 0)
                    {
                        flagBox.Checked = false;
                    }
                    else
                    {
                        flagBox.Checked = true;
                    }

                    string channelText = "";
                    for (int i = 0; i < file.tracks[tree.SelectedNode.Index].channels.Count; i++)
                    {
                        channelText += (file.tracks[tree.SelectedNode.Index].channels[i]+1);
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
                else
                {
                    noInfoPanel.Show();
                    trackPanel.Hide();
                    channelPanel.Hide();
                    projectPanel.Hide();
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

                    if (file.channelData.Count > 0)
                    {

                        projectPanel.Enabled = true;

                        samplingRateBox.Value = file.stream.sampleRate;
                        if (file.stream.loop == 1)
                        {
                            loopBox.Checked = true;
                        }
                        else { loopBox.Checked = false; }
                        loopStartBox.Value = file.stream.loopStart;
                        loopEndBox.Value = file.stream.loopEnd;
                    }
                    else {
                        projectPanel.Enabled = false;
                    }

                }
                else {

                    noInfoPanel.Show();
                    projectPanel.Hide();
                    trackPanel.Hide();

                }

            }

        }

        #region trackStuff
        public void volumeTrack(object sender, EventArgs e) {
            CISP.trackInfo t = file.tracks[tree.SelectedNode.Index];
            t.volume = (byte)volumeBox.Value;
            file.tracks[tree.SelectedNode.Index] = t;
        }

        public void panTrack(object sender, EventArgs e)
        {
            CISP.trackInfo t = file.tracks[tree.SelectedNode.Index];
            t.pan = (byte)panBox.Value;
            file.tracks[tree.SelectedNode.Index] = t;
        }

        public void flagTrack(object sender, EventArgs e)
        {
            CISP.trackInfo t = file.tracks[tree.SelectedNode.Index];
            if (flagBox.Checked) { t.flags = 1; } else { t.flags = 0; }
            file.tracks[tree.SelectedNode.Index] = t;
        }

        public void channelTrack(object sender, EventArgs e)
        {
            try
            {
                CISP.trackInfo t = file.tracks[tree.SelectedNode.Index];
                string channelString = channelTextBox.Text;
                t.channels = new List<byte>();
                if (channelString.Length > 0)
                {
                    t.channelCount = (UInt32)channelString.Length / 2 + 1;
                    for (int i = 0; i < t.channelCount; i += 1) {
                        t.channels.Add((byte)(byte.Parse(new string(channelString[i*2], 1))-1));
                    }
                }
                file.tracks[tree.SelectedNode.Index] = t;
            }
            catch {
                MessageBox.Show("Enter like this: 1;2");
            }
        }
        #endregion

        private void updateProjectInfoButton_Click(object sender, EventArgs e)
        {
            if (loopBox.Checked) { file.stream.loop = 1; } else { file.stream.loop = 0; }
            file.stream.loopStart = (UInt32)loopStartBox.Value;
            file.stream.loopEnd = (UInt32)loopEndBox.Value;

            if (file.stream.loopEnd > file.channelData[0].Length) {
                file.stream.loopEnd = (UInt32)file.channelData[0].Length;
            }

        }

        #endregion



        //Update nodes.
        #region updateNodes
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
            

            //Only if file is open.
            if (fileOpen)
            {

                //Add tracks and channels.
                for (int i = 0; i < file.tracks.Count; i++)
                {

                    tree.Nodes[2].Nodes.Add("Track " + (i + 1), "Track " + (i + 1), 4, 4);

                }
                for (int i = 0; i < file.channelData.Count; i++)
                {

                    tree.Nodes[1].Nodes.Add("Channel " + (i + 1), "Channel " + (i + 1), 2, 2);

                }

                //Root menus.
                tree.Nodes[1].ContextMenuStrip = rootMenu;
                tree.Nodes[2].ContextMenuStrip = rootMenu;

                //Node menus.
                foreach (TreeNode n in tree.Nodes[1].Nodes) {
                    n.ContextMenuStrip = nodeMenu;
                }
                foreach (TreeNode n in tree.Nodes[2].Nodes)
                {
                    n.ContextMenuStrip = nodeMenu;
                }

                //Tables.
                if (file.channelData.Count > 0) { tableLayoutPanel1.Enabled = true; } else { tableLayoutPanel1.Enabled = false; }
                if (/*file.stream.loop == 1 && */file.channelData.Count > 0) { tableLayoutPanel2.Enabled = true; } else { tableLayoutPanel2.Enabled = false; }


            }
            else {
                tree.Nodes[1].ContextMenuStrip = null;
                tree.Nodes[2].ContextMenuStrip = null;
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



        //Root nodes. ADDING NEW CHANNELS INCOMPLETE!!!
        #region rootNodes

        private void add_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode.Index == 2)
            {
                //Make new track.
                CISP.trackInfo t = new CISP.trackInfo();
                t.channelCount = 0;
                t.channels = new List<byte>();
                t.flags = 0;
                t.magic = "TRAC".ToCharArray();
                t.pan = 40;
                t.volume = 127;
                file.tracks.Add(t);

                //Update nodes.
                updateNodes();
            }
            else if (tree.SelectedNode.Index == 1)
            {

                //Show file selector.
                anyFileSelectorSound.ShowDialog();

                if (anyFileSelectorSound.FileName != "")
                {

                    if (file.channelData.Count == 0)
                    {
                        switch (anyFileSelectorSound.FilterIndex)
                        {

                            //Wave.
                            case 1:
                                RIFF r = new RIFF();
                                r.load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                                int channelLength = r.data.data.Length / r.fmt.numChannels;
                                if (r.fmt.bitsPerSample != 16 || r.fmt.chunkFormat != 1)
                                {
                                    MessageBox.Show("Not PCM16 Wave!");
                                }
                                else
                                {

                                    file.stream.sampleRate = r.fmt.sampleRate;

                                    //Add the channels.
                                    List<UInt16>[] soundData = new List<UInt16>[r.fmt.numChannels];
                                    for (int i = 0; i < soundData.Count(); i++)
                                    {
                                        soundData[i] = new List<UInt16>();

                                        MemoryStream src = new MemoryStream(r.data.data);
                                        BinaryDataReader br = new BinaryDataReader(src);

                                        br.Position = i * 2;

                                        while (br.Position < r.data.chunkSize)
                                        {

                                            soundData[i].Add(br.ReadUInt16());
                                            try { for (int j = 1; j < r.fmt.numChannels; j++) { br.ReadUInt16(); } } catch { }

                                        }
                                    }

                                    //Now convert the corrected data per channel to the samples.
                                    file.channelData = new List<UInt16[]>();
                                    foreach (List<UInt16> x in soundData)
                                    {
                                        file.channelData.Add(x.ToArray());
                                    }

                                    file.stream.loopEnd = (UInt32)file.channelData[0].Length;

                                }

                                break;

                            //Game Wave.
                            case 2:
                                b_wav b = new b_wav();
                                b.load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                                b = b.toRiff().toGameWavPCM();
                                b.update(endianNess.big);
                                file.stream.sampleRate = b.info.samplingRate;
                                file.stream.loopEnd = b.info.loopEnd;
                                foreach (UInt16[] u in b.data.pcm16)
                                {
                                    file.channelData.Add(u);
                                }
                                break;

                            //Game Stream.
                            case 3:
                                b_stm s = new b_stm();
                                s.load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                                b_wav v = new b_wav();
                                v = s.toRiff().toGameWavPCM();
                                v.update(endianNess.big);
                                file.stream.sampleRate = v.info.samplingRate;
                                file.stream.loopEnd = v.info.loopEnd;
                                foreach (UInt16[] u in v.data.pcm16)
                                {
                                    file.channelData.Add(u);
                                }
                                break;
                        }

                        loadChannelFiles();

                    }
                    else {
                        MessageBox.Show("You can only use channels from one file at a time! Use Audacity to have a wave file with all the channels you need. To do this, in Import/Export Settings of Audacity, enable custom mix.");
                    }

                    anyFileSelectorSound.FileName = "";

                    //Update nodes.
                    updateNodes();

                }



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



        //Child nodes. INCOMPLETE!!!
        #region childNodes

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (tree.SelectedNode.Index != 0)
            {

                if (tree.SelectedNode.Parent.Index == 1)
                {

                    if (file.channelData.Count > 1) {

                        UInt16[] temp = file.channelData[tree.SelectedNode.Index];
                        UInt16[] temp2 = file.channelData[tree.SelectedNode.Index-1];
                        file.channelData[tree.SelectedNode.Index] = temp2; ;
                        file.channelData[tree.SelectedNode.Index - 1] = temp;
                        loadChannelFiles();

                    }

                }
                else
                {

                    if (file.tracks.Count > 1)
                    {

                        CISP.trackInfo temp = file.tracks[tree.SelectedNode.Index];
                        CISP.trackInfo temp2 = file.tracks[tree.SelectedNode.Index - 1];
                        file.tracks[tree.SelectedNode.Index] = temp2; ;
                        file.tracks[tree.SelectedNode.Index - 1] = temp;

                    }

                }
                updateNodes();

            }
        }

        private void moveDownItem_Click(object sender, EventArgs e)
        {

        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode.Parent.Index == 1)
            {

                ////CODE ME YOU LAZY ASSHOLE!!!!!

            }
            else {
                MessageBox.Show("You can't replace a track!");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < players.Length; i++)
            {
                players[i].player.Stop();

                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

            }

            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;

            if (tree.SelectedNode.Parent.Index == 1)
            {

                file.channelData.RemoveAt(tree.SelectedNode.Index);
                loadChannelFiles();

            }
            else
            {

                file.tracks.RemoveAt(tree.SelectedNode.Index);

            }
            updateNodes();
        }

        #endregion


        //Buttons that do small things.
        #region otherButtons

        /// <summary>
        /// Convert wav to game wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleWaveToGameWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            b_wav b = new b_wav();
            waveSelectorBox.ShowDialog();
            if (waveSelectorBox.FileName != "")
            {
                RIFF r = new RIFF();
                r.load(File.ReadAllBytes(waveSelectorBox.FileName));
                waveSelectorBox.FileName = "";
                b = r.toGameWav();


                saveGameWaveBox.ShowDialog();
                if (saveGameWaveBox.FileName != "")
                {

                    if (saveGameWaveBox.FilterIndex == 1)
                    {

                        File.WriteAllBytes(saveGameWaveBox.FileName, b.toBytes(endianNess.big));

                    }
                    else
                    {

                        File.WriteAllBytes(saveGameWaveBox.FileName, b.toBytes(endianNess.little));

                    }

                    saveGameWaveBox.FileName = "";

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
            if (gameFileSelectorBox.FileName != "") {

                if (gameFileSelectorBox.FilterIndex == 1)
                {

                    //Have user select output.
                    saveWaveBox.ShowDialog();
                    if (saveWaveBox.FileName != "") {

                        //Do actual conversions.
                        b_wav b = new b_wav();
                        b.load(File.ReadAllBytes(gameFileSelectorBox.FileName));
                        File.WriteAllBytes(saveWaveBox.FileName, b.toRiff().toBytes());

                    }

                }
                else
                {

                    //Have user select output.
                    saveWaveBox.ShowDialog();
                    if (saveWaveBox.FileName != "")
                    {

                        //Do actual conversions.
                        b_stm b = new b_stm();
                        b.load(File.ReadAllBytes(gameFileSelectorBox.FileName));
                        File.WriteAllBytes(saveWaveBox.FileName, b.toRiff().toBytes());

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



        //Channel player.
        #region channelPlayer
        private void playButtonChannel_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].player.PlaybackState == PlaybackState.Playing)
                {
                    players[i].player.Stop();

                    players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                    players[i].player.Init(players[i].playerFile);
                }

            }

            foreach (channelPlayer p in players)
            {
                if (p.player != null)
                {
                    p.player.Volume = (float)((float)volume.Value / (float)100);
                }
            }

            players[tree.SelectedNode.Index].player.Play();

            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;

        }

        private void pauseButtonChannel_Click(object sender, EventArgs e)
        {
            players[tree.SelectedNode.Index].player.Pause();
            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;
        }

        private void stopButtonChannel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].player.Stop();

                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

            }
            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;
        }


        #endregion



        //Saving
        #region saving

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen) save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen) saveAs();
        }

        public void save() {
            if (fileNamePath == "")
            {
                saveAs();
            }
            else {
                File.WriteAllBytes(fileNamePath, file.toBytes());
            }
        }

        public void saveAs() {
            saveCISP.ShowDialog();
            if (saveCISP.FileName != "") {
                fileNamePath = saveCISP.FileName;
                saveCISP.FileName = "";
                this.Text = "Isabelle Sound Editor - " + Path.GetFileName(fileNamePath);
                save();
            }
        }

        #endregion



        //Opening
        #region opening

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openCISP.ShowDialog();
            if (openCISP.FileName != "") {

                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                noInfoPanel.Show();

                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].player.Stop();

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);

                    }



                    playPauseButton.Image = new Bitmap("Data/Image/play4.png");
                    playing = false;

                }

                catch { }

                file = new CISP();
                file.load(File.ReadAllBytes(openCISP.FileName));
                fileNamePath = openCISP.FileName;
                this.Text = "Isabelle Sound Editor - " + Path.GetFileName(fileNamePath);
                openCISP.FileName = "";
                fileOpen = true;
                loadChannelFiles();
                updateNodes();

            }
        }

        #endregion



        //Track Player.
        #region trackPlayer

        private void playSoundTrack_Click(object sender, EventArgs e)
        {

            try
            {

                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].player.PlaybackState == PlaybackState.Playing)
                    {
                        players[i].player.Stop();

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);
                    }
                }

                foreach (channelPlayer p in players)
                {
                    if (p.player != null)
                    {
                        p.player.Volume = (float)volume.Value / (float)100;
                    }
                }

                foreach (byte b in file.tracks[tree.SelectedNode.Index].channels)
                {
                    players[(int)b].player.Play();
                }

            }
            catch {
                MessageBox.Show("You can't play channels that don't exist! :p");
            }

            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;
        }

        private void pauseSoundTrack_Click(object sender, EventArgs e)
        {
            foreach (byte b in file.tracks[tree.SelectedNode.Index].channels)
            {
                players[(int)b].player.Pause();
            }
            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;
        }

        private void stopSoundTrack_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].player.Stop();

                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

            }
            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;
        }

        #endregion



        //Load channel files.
        #region loadChannelFiles
        public void loadChannelFiles() {

            players = new channelPlayer[file.channelData.Count];
            for (int i = 0; i < file.channelData.Count; i++) {

                //Write the RIFF.
                MemoryStream channelData = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(channelData);
                bw.Write(file.channelData[i]);

                players[i].file = channelData.ToArray();
                players[i].player = new WaveOutEvent();
                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(channelData.ToArray()), new WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

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

        //Play pause.
        private void playPauseButton_Click(object sender, EventArgs e)
        {
            if (playing)
            {

                foreach (channelPlayer p in players)
                {
                    if (p.player != null)
                    {
                        p.player.Pause();
                    }
                }

                playPauseButton.Image = new Bitmap("Data/Image/play4.png");
                playing = false;

            }
            else
            {

                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].player.PlaybackState == PlaybackState.Playing)
                    {
                        players[i].player.Stop();

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);
                    }
                }

                foreach (channelPlayer p in players)
                {
                    if (p.player != null)
                    {
                        p.player.Volume = (float)volume.Value / (float)100;
                    }
                }

                for (int i = 0; i < file.channelData.Count; i++)
                {
                    players[i].player.Play();
                }

                playPauseButton.Image = new Bitmap("Data/Image/pause.png");
                playing = true;

            }
        }


        //Stop.
        private void stopButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].player.Stop();

                players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                players[i].player.Init(players[i].playerFile);

            }

            playPauseButton.Image = new Bitmap("Data/Image/play4.png");
            playing = false;

        }

        #endregion



        //Loop
        public void loop() {

            while (true) {

                try { timeBar.Value = (int)(((decimal)players[0].player.GetPosition() / ((decimal)file.channelData[0].Length * 2)) * 1440); } catch { }
                
                try
                {
                    spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);
                    spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);
                }
                catch { }

            }

        }



        //Loop buttons.
        #region loopButtons

        private void setLoopStartButton_Click(object sender, EventArgs e)
        {
            file.stream.loop = 1;
            file.stream.loopStart = (UInt32)(players[0].player.GetPosition()/2);
            doInfoStuff();

            spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);

            if (file.stream.loopStart >= file.channelData[0].Length) { file.stream.loopStart = (UInt32)file.channelData[0].Length; }

        }

        private void setLoopEndButton_Click(object sender, EventArgs e)
        {
            file.stream.loop = 1;
            file.stream.loopEnd = (UInt32)(players[0].player.GetPosition() / 2);
            doInfoStuff();

            spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);

            if (file.stream.loopEnd >= file.channelData[0].Length) { file.stream.loopEnd = (UInt32)file.channelData[0].Length; }

        }








        #endregion



        //To RIFF.
        private void exportWavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (file.channelData.Count > 0)
            {
                saveWaveBox.ShowDialog();
                if (saveWaveBox.FileName != "")
                {

                    File.WriteAllBytes(saveWaveBox.FileName, file.toRIFF().toBytes());
                    saveWaveBox.FileName = "";

                }
            }
            else {

                MessageBox.Show("You need at least one channel to save first!");

            }
        }



        //Import file.
        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int j = 0;
            if (fileOpen)
            {

                SaveCloseDialog c = new SaveCloseDialog();
                j = c.getValue();
                if (j == 0) { save(); }

            }
            if (j == 0 || j == 1) {

                try
                {

                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].player.Stop();

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);

                    }



                    playPauseButton.Image = new Bitmap("Data/Image/play4.png");
                    playing = false;

                }

                catch { }


                //General stuff.
                fileNamePath = "";
                fileOpen = true;
                this.Text = "Isabelle Sound Editor";

                //Make new CISP.
                file = new CISP();
                file.stream = new CISP.streamInfo();
                file.stream.loop = 0;
                file.stream.loopStart = 0;
                file.stream.loopEnd = 0;
                file.stream.sampleRate = 0xFFFFFFFF;
                file.stream.sampleSize = 0;
                byte[] seek = { 2 };
                file.seekBlock = seek;
                file.tracks = new List<CISP.trackInfo>();
                file.channelData = new List<UInt16[]>();

                fileOpen = false;

                updateNodes();

                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                noInfoPanel.Show();
                
                anyFileSelectorSound.ShowDialog();
                if (anyFileSelectorSound.FileName != "")
                {

                    file = new CISP();
                    fileOpen = true;
                    this.Text = "Isabelle Sound Editor - New Project.cisp";

                    switch (anyFileSelectorSound.FilterIndex)
                    {

                        case 1:
                            file.seekSize = 0;
                            file.seekBlock = new byte[0];
                            file.tracks = new List<CISP.trackInfo>();
                            file.channelData = new List<UInt16[]>();
                            RIFF r = new RIFF();
                            r.load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            file.stream.sampleRate = r.fmt.sampleRate;
                            int channelLength = r.data.data.Length / r.fmt.numChannels;
                            if (r.fmt.bitsPerSample != 16 || r.fmt.chunkFormat != 1)
                            {
                                MessageBox.Show("Not PCM16 Wave!");
                            }
                            else
                            {

                                file.stream.sampleRate = r.fmt.sampleRate;

                                //Add the channels.
                                List<UInt16>[] soundData = new List<UInt16>[r.fmt.numChannels];
                                for (int i = 0; i < soundData.Count(); i++)
                                {
                                    soundData[i] = new List<UInt16>();

                                    MemoryStream src = new MemoryStream(r.data.data);
                                    BinaryDataReader br = new BinaryDataReader(src);

                                    br.Position = i * 2;

                                    while (br.Position < r.data.chunkSize)
                                    {

                                        soundData[i].Add(br.ReadUInt16());
                                        try { for (int j2 = 1; j2 < r.fmt.numChannels; j2++) { br.ReadUInt16(); } } catch { }

                                    }
                                }

                                //Now convert the corrected data per channel to the samples.
                                file.channelData = new List<UInt16[]>();
                                foreach (List<UInt16> x in soundData)
                                {
                                    file.channelData.Add(x.ToArray());
                                }

                                file.stream.loopEnd = (UInt32)file.channelData[0].Length;

                            }

                            break;

                        case 2:
                            b_wav b = new b_wav();
                            b.load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            file.seekSize = 0;
                            file.seekBlock = new byte[0];
                            file.stream.loop = b.info.loop;
                            file.stream.loopEnd = b.info.loopEnd;
                            file.stream.loopStart = b.info.loopStart;
                            file.stream.sampleRate = b.info.samplingRate;
                            file.tracks = new List<CISP.trackInfo>();
                            file.channelData = new List<UInt16[]>();

                            //Import due to encoding.
                            switch (b.info.soundEncoding)
                            {

                                case 0:
                                    MessageBox.Show("Unsupported Data type! Must be PCM16 or DSPADPCM!");
                                    break;

                                case 1:
                                    file.stream.sampleRate = b.info.samplingRate;
                                    file.stream.loopEnd = b.info.loopEnd;
                                    file.stream.loop = b.info.loop;
                                    file.stream.loopStart = b.info.loopStart;
                                    for (int i = 0; i < b.data.pcm16.Count; i++)
                                    {
                                        file.channelData.Add(b.data.pcm16[i]);
                                    }
                                    break;

                                case 2:
                                    b_wav v2 = b;
                                    b = b.toRiff().toGameWavPCM();
                                    b.update(endianNess.big);
                                    file.stream.sampleRate = b.info.samplingRate;
                                    file.stream.loopEnd = v2.info.loopEnd;
                                    file.stream.loop = v2.info.loop;
                                    file.stream.loopStart = v2.info.loopStart;
                                    foreach (UInt16[] u in b.data.pcm16)
                                    {
                                        file.channelData.Add(u);
                                    }

                                    break;

                                case 3:
                                    MessageBox.Show("Unsupported Data type! Must be PCM16 or DSPADPCM!");
                                    break;

                            }
                            

                            break;

                        case 3:
                            b_stm s = new b_stm();
                            s.load(File.ReadAllBytes(anyFileSelectorSound.FileName));
                            b_wav v = new b_wav();
                            v = s.toRiff().toGameWavPCM();
                            v.update(endianNess.big);
                            file.stream.sampleRate = v.info.samplingRate;
                            file.stream.loopEnd = s.info.stream.loopEnd;
                            file.stream.loop = s.info.stream.loop;
                            file.stream.loopStart = s.info.stream.loopStart;
                            file.tracks = new List<CISP.trackInfo>();
                            file.channelData = new List<UInt16[]>();
                            foreach (UInt16[] u in v.data.pcm16)
                            {
                                file.channelData.Add(u);
                            }

                            for (int i = 0; i < s.info.track.Count; i++) {
                                CISP.trackInfo t = new CISP.trackInfo();
                                b_stm.infoBlock.trackInfo t2 = s.info.track[i];
                                t.channelCount = t2.byteTable.count;
                                t.channels = t2.byteTable.channelIndexes;
                                t.flags = t2.flags;
                                t.magic = "TRAC".ToCharArray();
                                t.pan = t2.pan;
                                t.volume = t2.volume;
                                file.tracks.Add(t);
                            }

                            break;

                    }

                    loadChannelFiles();
                    updateNodes();

                }
                else {
                    this.Text = "Isabelle Sound Editor";
                }

                anyFileSelectorSound.FileName = "";

            }
        }



        //Fix resizing.
        private void formResized(object sender, EventArgs e) {
            if (playing)
            {

                try { timeBar.Value = (int)(((decimal)players[0].player.GetPosition() / ((decimal)file.channelData[0].Length * 2)) * 1440); } catch { }

            }
            else
            {

                try { if (players[0].player.PlaybackState == PlaybackState.Paused) { timeBar.Value = (int)(((decimal)players[0].player.GetPosition() / ((decimal)file.channelData[0].Length * 2)) * 1440); } } catch { }

            }

            try
            {
                spltStart.SplitPosition = (int)((decimal)file.stream.loopStart / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);
                spltEnd.SplitPosition = pnlLoop.Size.Width - (int)((decimal)file.stream.loopEnd / (decimal)file.channelData[0].Length * (decimal)pnlLoop.Size.Width);
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

            saveGameFile.ShowDialog();
            if (saveGameFile.FileName != "") {

                //See what file to save.
                switch (saveGameFile.FilterIndex) {

                    case 1:
                        file.update();
                        b_wav b = file.toB_wav();
                        File.WriteAllBytes(saveGameFile.FileName, b.toBytes(endianNess.big));
                        break;

                    case 2:
                        file.update();
                        b_wav b2 = file.toB_wav();
                        File.WriteAllBytes(saveGameFile.FileName, b2.toBytes(endianNess.little));
                        break;

                    case 3:
                        file.update();
                        b_stm b3 = file.toB_stm();
                        File.WriteAllBytes(saveGameFile.FileName, b3.toBytes(endianNess.big));
                        break;

                    case 4:
                        file.update();
                        b_stm b4 = file.toB_stm();
                        File.WriteAllBytes(saveGameFile.FileName, b4.toBytes(endianNess.little));
                        break;

                }


                saveGameFile.FileName = "";

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

                        players[i].playerFile = new RawSourceWaveStream(new MemoryStream(players[i].file), new WaveFormat((int)file.stream.sampleRate, 1));
                        players[i].player.Init(players[i].playerFile);

                    }



                    playPauseButton.Image = new Bitmap("Data/Image/play4.png");
                    playing = false;

                }

                catch { }


                //General stuff.
                fileNamePath = "";
                fileOpen = true;
                this.Text = "Isabelle Sound Editor";

                //Make new CISP.
                file = new CISP();
                file.stream = new CISP.streamInfo();
                file.stream.loop = 0;
                file.stream.loopStart = 0;
                file.stream.loopEnd = 0;
                file.stream.sampleRate = 0xFFFFFFFF;
                file.stream.sampleSize = 0;
                byte[] seek = { 2 };
                file.seekBlock = seek;
                file.tracks = new List<CISP.trackInfo>();
                file.channelData = new List<UInt16[]>();

                fileOpen = false;

                updateNodes();

                projectPanel.Hide();
                channelPanel.Hide();
                trackPanel.Hide();
                noInfoPanel.Show();

            }

        }


        //Quit.
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

        }

        //Save stream from wave.
        private void simpleWaveToGameStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            b_stm b = new b_stm();
            waveSelectorBox.ShowDialog();
            if (waveSelectorBox.FileName != "")
            {
                RIFF r = new RIFF();
                r.load(File.ReadAllBytes(waveSelectorBox.FileName));
                waveSelectorBox.FileName = "";
                b = r.toGameWav().toB_stm();
                b.update(endianNess.big, true);


                saveGameWaveBox.ShowDialog();
                if (saveGameWaveBox.FileName != "")
                {

                    if (saveGameWaveBox.FilterIndex == 1)
                    {

                        File.WriteAllBytes(saveGameWaveBox.FileName, b.toBytes(endianNess.big));

                    }
                    else
                    {

                        File.WriteAllBytes(saveGameWaveBox.FileName, b.toBytes(endianNess.little));

                    }
                    
                    saveGameWaveBox.FileName = "";

                }
            }
        }

        //Game file to game file.
        private void gameFileToGameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            gameFileSelectorBox.ShowDialog();

            if (gameFileSelectorBox.FileName != "") {

                switch (gameFileSelectorBox.FilterIndex) {

                    case 1:

                        b_wav b = new b_wav();
                        b.load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        saveGameFile.ShowDialog();

                        if (saveGameFile.FileName != "") {

                            switch (saveGameFile.FilterIndex) {

                                case 1:
                                    File.WriteAllBytes(saveGameFile.FileName, b.toBytes(endianNess.big));
                                    break;

                                case 2:
                                    File.WriteAllBytes(saveGameFile.FileName, b.toBytes(endianNess.little));
                                    break;

                                case 3:
                                    File.WriteAllBytes(saveGameFile.FileName, b.toB_stm().toBytes(endianNess.big));
                                    break;

                                case 4:
                                    File.WriteAllBytes(saveGameFile.FileName, b.toB_stm().toBytes(endianNess.little));
                                    break;

                            }

                        }

                        saveGameFile.FileName = "";

                        break;

                    case 2:

                        b_stm s = new b_stm();
                        s.load(File.ReadAllBytes(gameFileSelectorBox.FileName));

                        saveGameFile.ShowDialog();

                        if (saveGameFile.FileName != "")
                        {

                            switch (saveGameFile.FilterIndex)
                            {

                                case 1:
                                    File.WriteAllBytes(saveGameFile.FileName, s.toB_wav().toBytes(endianNess.big));
                                    break;

                                case 2:
                                    File.WriteAllBytes(saveGameFile.FileName, s.toB_wav().toBytes(endianNess.little));
                                    break;

                                case 3:
                                    File.WriteAllBytes(saveGameFile.FileName, s.toBytes(endianNess.big));
                                    break;

                                case 4:
                                    File.WriteAllBytes(saveGameFile.FileName, s.toBytes(endianNess.little));
                                    break;

                            }

                        }

                        saveGameFile.FileName = "";

                        break;

                }

            }

            gameFileSelectorBox.FileName = "";

        }

        //Help.
        private void getHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://discord.gg/6VDPGne");
        }
    }
}
