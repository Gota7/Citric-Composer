using CitraFileLoader;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
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
    public partial class Brewster_WAR_Brewer : EditorBase {

        WaveOut waveOut = new WaveOut();
        bool paused;
        int prevIndex;
        byte forceWavMaj = 1;
        byte forceWavMin = 0;
        byte forceWavRev = 0;

        public Brewster_WAR_Brewer(MainWindow mainWindow) : base(typeof(SoundWaveArchive), "Wave Archive", "war", "Brewster's War Brewer", mainWindow) {
            InitializeComponent();
            Text = "Brewster's War Brewer";
            Icon = Properties.Resources.Brewster;
            toolsWarToolStripMenuItem.Visible = true;
        }

        public Brewster_WAR_Brewer(string fileToOpen, MainWindow mainWindow) : base(typeof(SoundWaveArchive), "Wave Archive", "war", "Brewster's War Brewer", fileToOpen, mainWindow) {
            InitializeComponent();
            Text = "Brewster's War Brewer - " + Path.GetFileName(fileToOpen);
            Icon = Properties.Resources.Brewster;
            toolsWarToolStripMenuItem.Visible = true;
        }

        public Brewster_WAR_Brewer(SoundFile<ISoundFile> fileToOpen, MainWindow mainWindow, EditorBase otherEditor = null) : base(typeof(SoundWaveArchive), "Wave Archive", "war", "Brewster's War Brewer", fileToOpen, mainWindow) {
            InitializeComponent();
            string name = ExtFile.FileName;
            if (name == null) {
                name = "{ Null File Name }";
            }
            Text = EditorName + " - " + name + "." + ExtFile.FileExtension;
            Icon = Properties.Resources.Brewster;
            OtherEditor = otherEditor;
            toolsWarToolStripMenuItem.Visible = true;
        }

        //Info and updates.
        #region InfoAndUpdates

        /// <summary>
        /// Do info stuff.
        /// </summary>
        public override void DoInfoStuff() {

            //Call base.
            base.DoInfoStuff();

            //Safety check.
            if (!FileOpen || File == null) {
                return;
            }

            //Parent is not null, so a wave is selected.
            if (tree.SelectedNode.Parent != null) {

                //File is null.
                if ((File as SoundWaveArchive)[tree.SelectedNode.Index] == null) {

                    //Null info.
                    nullDataPanel.BringToFront();
                    nullDataPanel.Show();

                    //Update status.
                    status.Text = "No Valid Info Selected!";

                }

                //File is valid.
                else {

                    //Sound player deluxe.
                    soundPlayerDeluxePanel.BringToFront();
                    soundPlayerDeluxePanel.Show();

                    //Update status.
                    status.Text = "Wave: " + tree.SelectedNode.Index + ", Size: " + (File as SoundWaveArchive)[tree.SelectedNode.Index].Wav.fileHeader.size + " bytes.";

                }

            }

            //Waves list is selected.
            else if (tree.SelectedNode.Index == 1) {

                //Show no info screen.
                noInfoPanel.BringToFront();
                noInfoPanel.Show();

                //Show the status.
                status.Text = "Wave Count: " + (File as SoundWaveArchive).Count;

            }

            //File information.
            else if (tree.SelectedNode.Index == 0) {

                //Proper info panel.
                warFileInfoPanel.BringToFront();
                warFileInfoPanel.Show();

                //Update boxes.
                vMajBoxWar.Value = (File as SoundWaveArchive).Version.Major;
                vMinBoxWar.Value = (File as SoundWaveArchive).Version.Minor;
                vRevBoxWar.Value = (File as SoundWaveArchive).Version.Revision;
                vWavMajBox.Value = forceWavMaj;
                vWavMinBox.Value = forceWavMin;
                vWavRevBox.Value = forceWavRev;

                //Status.
                status.Text = "File Information.";

            }

        }

        /// <summary>
        /// Update nodes.
        /// </summary>
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

            //Add waves if node doesn't exist.
            if (tree.Nodes.Count < 2) {
                tree.Nodes.Add("waves", "Waves", 6, 6);
            }

            //File is open and not null.
            if (FileOpen && File != null) {

                //Get the version.
                for (int i = 0; i < (File as SoundWaveArchive).Count; i++) {

                    if ((File as SoundWaveArchive)[i] != null) {
                        forceWavMaj = (File as SoundWaveArchive)[i].Wav.fileHeader.vMajor;
                        forceWavMin = (File as SoundWaveArchive)[i].Wav.fileHeader.vMinor;
                        forceWavRev = (File as SoundWaveArchive)[i].Wav.fileHeader.vRevision;
                    }

                }

                //Context menu.
                tree.Nodes["waves"].ContextMenuStrip = rootMenu;

                //Add each wave.
                var h = File as SoundWaveArchive;
                for (int i = 0; i < (File as SoundWaveArchive).Count; i++) {

                    //File is null.
                    if ((File as SoundWaveArchive)[i] == null) {

                        //Add null wave.
                        tree.Nodes["waves"].Nodes.Add("wave" + i, "{ Null Wave " + i + " }", 0, 0);

                    }

                    //Valid info.
                    else {

                        //Add each wave.
                        tree.Nodes["waves"].Nodes.Add("wave" + i, "Wave " + i, 2, 2);

                    }

                    //Add context menu.
                    tree.Nodes["waves"].Nodes["wave" + i].ContextMenuStrip = nodeMenu;

                }

            } else {

                //Remove context menu.
                tree.Nodes["waves"].ContextMenuStrip = null;

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

            //If parent exists, then it is a wave.
            if (tree.SelectedNode.Parent != null) {

                //Wave is not null.
                if ((File as SoundWaveArchive)[tree.SelectedNode.Index] != null) {

                    //Open the wave in Isabelle.
                    IsabelleSoundEditor e = new IsabelleSoundEditor(this, tree.SelectedNode.Index, "Wave " + tree.SelectedNode.Index);
                    e.Show();

                } else {

                    //Insult user.
                    MessageBox.Show("You can't open a null wave file!", "Notice:");

                }

            }

        }

        #endregion


        //Player.
        #region Player

        /// <summary>
        /// Play sound player deluxe.
        /// </summary>
        public override void Play() {

            if (!paused || prevIndex != tree.SelectedNode.Index) {
                waveOut.Stop();
                var n = new WaveFileReader(new MemoryStream((File as SoundWaveArchive)[tree.SelectedNode.Index].Riff.ToBytes()));
                waveOut.Initialize(n);
                prevIndex = tree.SelectedNode.Index;
            }
            waveOut.Play();
            paused = false;

        }

        /// <summary>
        /// Pause sound player deluxe.
        /// </summary>
        public override void Pause() {

            paused = true;
            waveOut.Pause();

        }

        /// <summary>
        /// Stop sound player deluxe.
        /// </summary>
        public override void Stop() {

            paused = false;
            waveOut.Stop();

        }

        #endregion


        //File info.
        #region FileInfo

        /// <summary>
        /// Force the internal waves to have the same version.
        /// </summary>
        public override void ForceWaveVersionButtonClick() {

            for (int i = 0; i < (File as SoundWaveArchive).Count; i++) {

                if ((File as SoundWaveArchive)[i] != null) {
                    (File as SoundWaveArchive)[i].Wav.fileHeader.vMajor = forceWavMaj;
                    (File as SoundWaveArchive)[i].Wav.fileHeader.vMinor = forceWavMin;
                    (File as SoundWaveArchive)[i].Wav.fileHeader.vRevision = forceWavRev;
                }

            }

        }

        /// <summary>
        /// WAR major.
        /// </summary>
        public override void BoxWarMajChanged() {
            (File as SoundWaveArchive).Version.Major = (byte)vMajBoxWar.Value;
        }

        /// <summary>
        /// WAR minor.
        /// </summary>
        public override void BoxWarMinChanged() {
            (File as SoundWaveArchive).Version.Minor = (byte)vMinBoxWar.Value;
        }

        /// <summary>
        /// WAR revision.
        /// </summary>
        public override void BoxWarRevChanged() {
            (File as SoundWaveArchive).Version.Revision = (byte)vRevBoxWar.Value;
        }

        /// <summary>
        /// WAV major.
        /// </summary>
        public override void BoxWavMajChanged() {
            forceWavMaj = (byte)vWavMajBox.Value;
        }

        /// <summary>
        /// WAV minor.
        /// </summary>
        public override void BoxWavMinChanged() {
            forceWavMin = (byte)vWavMinBox.Value;
        }

        /// <summary>
        /// WAV revision.
        /// </summary>
        public override void BoxWavRevChanged() {
            forceWavRev = (byte)vWavRevBox.Value;
        }

        #endregion


        //Node menus.
        #region NodeMenus

        /// <summary>
        /// Add a wave.
        /// </summary>
        public override void RootAdd() {
            Wave w = GetWave();
            if (w != null) {
                (File as SoundWaveArchive).Add(w);
                UpdateNodes();
            }
        }

        /// <summary>
        /// Add the node above.
        /// </summary>
        public override void NodeAddAbove() {
            Wave w = GetWave();
            if (w != null) {
                (File as SoundWaveArchive).Insert(tree.SelectedNode.Index, w);
                UpdateNodes();
                tree.SelectedNode = tree.Nodes["waves"].Nodes[tree.SelectedNode.Index + 1];
                DoInfoStuff();
            }
        }

        /// <summary>
        /// Add the node below.
        /// </summary>
        public override void NodeAddBelow() {
            Wave w = GetWave();
            if (w != null) {
                (File as SoundWaveArchive).Insert(tree.SelectedNode.Index + 1, w);
                UpdateNodes();
                DoInfoStuff();
            }
        }

        /// <summary>
        /// Move node up.
        /// </summary>
        public override void NodeMoveUp() {
            if (Swap(File as SoundWaveArchive, tree.SelectedNode.Index - 1, tree.SelectedNode.Index)) {
                UpdateNodes();
                tree.SelectedNode = tree.Nodes["waves"].Nodes[tree.SelectedNode.Index - 1];
                DoInfoStuff();
            }
        }

        /// <summary>
        /// Move node down.
        /// </summary>
        public override void NodeMoveDown() {
            if (Swap(File as SoundWaveArchive, tree.SelectedNode.Index + 1, tree.SelectedNode.Index)) {
                UpdateNodes();
                tree.SelectedNode = tree.Nodes["waves"].Nodes[tree.SelectedNode.Index + 1];
                DoInfoStuff();
            }
        }

        /// <summary>
        /// Blank a node.
        /// </summary>
        public override void NodeBlank() {
            NodeReplace();
        }

        /// <summary>
        /// Replace node.
        /// </summary>
        public override void NodeReplace() {
            Wave w = GetWave();
            if (w != null) {
                (File as SoundWaveArchive)[tree.SelectedNode.Index] = w;
                UpdateNodes();
                DoInfoStuff();
            }
        }

        /// <summary>
        /// Export node.
        /// </summary>
        public override void NodeExport() {
            Wave w = (File as SoundWaveArchive)[tree.SelectedNode.Index];
            if (w != null) {

                //Get export path.
                SaveFileDialog s = new SaveFileDialog();
                s.RestoreDirectory = true;
                s.FileName = tree.SelectedNode.Text;
                s.Filter = "Wave|*.wav|Wave (3ds or Wii U)|*.bfwav;*.bcwav|Wave (Switch)|*.bfwav|Stream (3ds or Wii U)|*.bfstm;*.bcstm|Stream (Switch)|*.bfstm";
                s.ShowDialog();
                if (Path.GetExtension(s.FileName) == "") {
                    s.FileName += "wav";
                }
                if (s.FileName != "") {

                    //Get file data.
                    WriteMode m = WriteMode.Cafe;
                    if (Path.GetExtension(s.FileName).ToLower()[2] == 'c') {
                        m = WriteMode.CTR;
                    }
                    byte[] b = null;
                    switch (s.FilterIndex) {

                        //Wave.
                        case 1:
                            b = RiffWaveFactory.CreateRiffWave(w.Wav).ToBytes();
                            break;

                        //SDK Wave.
                        case 2:
                            if (m == WriteMode.Cafe) {
                                b = w.Wav.ToBytes(ByteOrder.BigEndian, true);
                            } else {
                                b = w.Wav.ToBytes(ByteOrder.LittleEndian);
                            }
                            break;

                        //SDK Wave Switch.
                        case 3:
                            b = w.Wav.ToBytes(ByteOrder.LittleEndian, true);
                            break;

                        //SDK Stream.
                        case 4:
                            if (m == WriteMode.Cafe) {
                                b = StreamFactory.CreateStream(w.Wav, w.Wav.fileHeader.vMajor, w.Wav.fileHeader.vMinor, w.Wav.fileHeader.vRevision).ToBytes(ByteOrder.BigEndian, true);
                            } else {
                                b = StreamFactory.CreateStream(w.Wav, w.Wav.fileHeader.vMajor, w.Wav.fileHeader.vMinor, w.Wav.fileHeader.vRevision).ToBytes(ByteOrder.LittleEndian);
                            }
                            break;

                        //SDK Stream Switch.
                        case 5:
                            b = StreamFactory.CreateStream(w.Wav, w.Wav.fileHeader.vMajor, w.Wav.fileHeader.vMinor, w.Wav.fileHeader.vRevision).ToBytes(ByteOrder.LittleEndian, true);
                            break;

                    }

                    //Write if possible.
                    if (b != null) {
                        System.IO.File.WriteAllBytes(s.FileName, b);
                    }

                }

            } else {
                MessageBox.Show("You can't export a null file!", "Notice:");
            }
        }

        /// <summary>
        /// Nullify the node.
        /// </summary>
        public override void NodeNullify() {
            (File as SoundWaveArchive)[tree.SelectedNode.Index] = null;
            UpdateNodes();
            DoInfoStuff();
        }

        /// <summary>
        /// Delete the node.
        /// </summary>
        public override void NodeDelete() {
            (File as SoundWaveArchive).RemoveAt(tree.SelectedNode.Index);
            UpdateNodes();
            try {
                tree.SelectedNode = tree.Nodes["waves"].Nodes[tree.SelectedNode.Index - 1];
            } catch {
                tree.SelectedNode = tree.Nodes["waves"];
            }          
            DoInfoStuff();
        }

        #endregion


        //War tools.
        #region WarTools

        /// <summary>
        /// Extract waves.
        /// </summary>
        public override void WarExtractWave() {

            //File open check.
            if (!FileTest(null, null, false, true)) {
                return;
            }

            //Safety.
            string path = GetFolderPath();
            if (path == "") {
                return;
            }

            //Write each wave.
            int count = 0;
            foreach (var w in (File as SoundWaveArchive)) {

                if (w == null) {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + " (NULL).wav", new byte[0]);
                } else {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + ".wav", RiffWaveFactory.CreateRiffWave(w.Wav).ToBytes());
                }

                count++;
            }

        }

        /// <summary>
        /// Extract 3ds.
        /// </summary>
        public override void WarExtractWave3ds() {

            //File open check.
            if (!FileTest(null, null, false, true)) {
                return;
            }

            //Safety.
            string path = GetFolderPath();
            if (path == "") {
                return;
            }

            //Write each wave.
            int count = 0;
            foreach (var w in (File as SoundWaveArchive)) {

                if (w == null) {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + " (NULL).bcwav", new byte[0]);
                } else {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + ".bcwav", w.Wav.ToBytes(ByteOrder.LittleEndian));
                }

                count++;
            }

        }

        /// <summary>
        /// Extract WiiU.
        /// </summary>
        public override void WarExtractWaveWiiU() {

            //File open check.
            if (!FileTest(null, null, false, true)) {
                return;
            }

            //Safety.
            string path = GetFolderPath();
            if (path == "") {
                return;
            }

            //Write each wave.
            int count = 0;
            foreach (var w in (File as SoundWaveArchive)) {

                if (w == null) {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + " (NULL).bfwav", new byte[0]);
                } else {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + ".bfwav", w.Wav.ToBytes(ByteOrder.BigEndian, true));
                }

                count++;
            }

        }

        /// <summary>
        /// Extract Switch.
        /// </summary>
        public override void WarExtractWaveSwitch() {

            //File open check.
            if (!FileTest(null, null, false, true)) {
                return;
            }

            //Safety.
            string path = GetFolderPath();
            if (path == "") {
                return;
            }

            //Write each wave.
            int count = 0;
            foreach (var w in (File as SoundWaveArchive)) {

                if (w == null) {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + " (NULL).bfwav", new byte[0]);
                } else {
                    System.IO.File.WriteAllBytes(path + "/" + count.ToString("D4") + ".bfwav", w.Wav.ToBytes(ByteOrder.LittleEndian, true));
                }

                count++;
            }

        }

        /// <summary>
        /// Import files.
        /// </summary>
        public override void WarBatchImport() {

            //File open check.
            if (!FileTest(null, null, false, true)) {
                return;
            }

            //Safety.
            string path = GetFolderPath();
            if (path == "") {
                return;
            }

            //Sort files.
            List<string> files = Directory.EnumerateFiles(path).ToList();
            for (int i = files.Count - 1; i >= 0; i--) {
                if (!files[i].Contains("wav")) {
                    files.RemoveAt(i);
                }
            }

            //Read each file.
            foreach (string d in files) {

                if (Path.GetExtension(d).ToLower().Contains(".wav")) {

                    byte[] b = System.IO.File.ReadAllBytes(d);
                    if (b.Length > 0) {
                        RiffWave r = new RiffWave();
                        r.Load(b);
                        (File as SoundWaveArchive).Add(new Wave() { Wav = WaveFactory.CreateWave(r, true, forceWavMaj, forceWavMin, forceWavRev) });
                    } else {
                        (File as SoundWaveArchive).Add(null);
                    }

                } else {

                    byte[] b = System.IO.File.ReadAllBytes(d);
                    if (b.Length > 0) {
                        Wave a = new Wave();
                        a.Wav = new b_wav();
                        a.Wav.Load(b);
                        (File as SoundWaveArchive).Add(a);
                    } else {
                        (File as SoundWaveArchive).Add(null);
                    }

                }

            }

            //Update.
            UpdateNodes();
            DoInfoStuff();

        }

        /// <summary>
        /// Get a folder path.
        /// </summary>
        /// <returns></returns>
        public string GetFolderPath() {

            FolderBrowserDialog f = new FolderBrowserDialog();
            f.ShowDialog();
            return f.SelectedPath;

        }

        #endregion


        //Other functions.
        #region OtherFunctions

        /// <summary>
        /// Get a path to a wave.
        /// </summary>
        /// <returns>Wave path.</returns>
        public string GetWavePath() {

            OpenFileDialog o = new OpenFileDialog();
            o.RestoreDirectory = true;
            o.FileName = "";
            o.Filter = "Supported File|*.bfwav;*.bcwav;*.bfstm;*.bcstm;*.wav";
            o.ShowDialog();
            return o.FileName;

        }

        /// <summary>
        /// Get the wave data from a file path.
        /// </summary>
        /// <returns>Wave data.</returns>
        public Wave GetWave() {

            string wavePath = GetWavePath();
            if (wavePath == "") {
                return null;
            }

            //Extension.
            string ext = Path.GetExtension(wavePath).ToLower();

            //RIFF.
            if (ext.StartsWith(".w")) {
                RiffWave r = new RiffWave();
                r.Load(System.IO.File.ReadAllBytes(wavePath));
                return new Wave() { Wav = WaveFactory.CreateWave(r, true, forceWavMaj, forceWavMin, forceWavRev) };
            }

            //Wave.
            else if (ext.EndsWith("wav")) {
                b_wav b = new b_wav();
                b.Load(System.IO.File.ReadAllBytes(wavePath));
                return new Wave() { Wav = b };
            }

            //Stream.
            else if (ext.EndsWith("stm")) {
                b_stm s = new b_stm();
                s.Load(System.IO.File.ReadAllBytes(wavePath));
                return new Wave() { Wav = WaveFactory.CreateWave(s, forceWavMaj, forceWavMin, forceWavRev) };
            }

            //Null.
            return null;

        }

        /// <summary>
        /// On closing.
        /// </summary>
        public override void OnClosing() {
            waveOut.Stop();
            try { waveOut.Dispose(); } catch { }
        }

        #endregion

    }

}
