using CitraFileLoader;
using ScintillaNET;
using SequenceDataLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer {

    /// <summary>
    /// Sequence editor.
    /// </summary>
    public class SSS_Sequencer : EditorBase {

        private const int BACK_COLOR = 0x2F2F2F;
        private const int FORE_COLOR = 0xB7B7B7;
        SequenceCommand prevCom = null;

        public SSS_Sequencer(MainWindow mainWindow) : base(typeof(SoundSequence), "Sequence", "seq", "SSS Sequencer", mainWindow) {
            Text = "SSS Sequencer";
            Icon = Properties.Resources.Wolfgang;
            sequenceEditorPanel.BringToFront();
            sequenceEditorPanel.Show();
            sequenceEditor.Insert += scintilla_Insert;
            sequenceEditor.Delete += scintilla_Delete;
            Load += new System.EventHandler(this.SequenceEditor_Load);
        }

        public SSS_Sequencer(string fileToOpen, MainWindow mainWindow) : base(typeof(SoundSequence), "Sequence", "seq", "SSS Sequencer", fileToOpen, mainWindow) {
            Text = "SSS Sequencer - " + Path.GetFileName(fileToOpen);
            Icon = Properties.Resources.Wolfgang;
            sequenceEditorPanel.BringToFront();
            sequenceEditorPanel.Show();
            sequenceEditor.Insert += scintilla_Insert;
            sequenceEditor.Delete += scintilla_Delete;
            Load += new System.EventHandler(this.SequenceEditor_Load);
        }

        public SSS_Sequencer(SoundFile<ISoundFile> fileToOpen, MainWindow mainWindow, EditorBase otherEditor = null) : base(typeof(SoundSequence), "Sequence", "seq", "SSS Sequencer", fileToOpen, mainWindow) {
            string name = ExtFile.FileName;
            if (name == null) {
                name = "{ Null File Name }";
            }
            Text = EditorName + " - " + name + "." + ExtFile.FileExtension;
            Icon = Properties.Resources.Wolfgang;
            sequenceEditorPanel.BringToFront();
            sequenceEditorPanel.Show();
            OtherEditor = otherEditor;
            sequenceEditor.Insert += scintilla_Insert;
            sequenceEditor.Delete += scintilla_Delete;
            Load += new System.EventHandler(this.SequenceEditor_Load);
            string ext = "F";
            if (fileToOpen.File != null) {
                ext = fileToOpen.File.GetExtension().Substring(1, 1);
            }
        }

        /// <summary>
        /// Update nodes. Good for refreshing sequence.
        /// </summary>
        public override void UpdateNodes() {

            //If file open.
            if (FileOpen) {

                //Refresh sequence.
                string name = "{ Null File Name }";
                string ext = "fseq";
                if (ExtFile != null) {
                    name = ExtFile.FileName;
                    ext = ExtFile.File.GetExtension().Substring(1, 1);
                }
                if (FilePath != "") {
                    name = Path.GetFileName(FilePath);
                    ext = Path.GetExtension(name).Substring(2, 1);
                }
                LoadSequenceText(Path.GetFileNameWithoutExtension(name), ext);

            }
            
            //No file open.
            else {
                sequenceEditor.ReadOnly = false;
                sequenceEditor.Text = "{ No File Open }";
                sequenceEditor.ReadOnly = true;
            }

        }

        /// <summary>
        /// Load the editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SequenceEditor_Load(object sender, EventArgs e) {

            //Init style.
            sequenceEditor.Dock = DockStyle.Fill;
            sequenceEditor.WrapMode = WrapMode.None;
            sequenceEditor.IndentationGuides = IndentView.LookBoth;
            sequenceEditor.StyleResetDefault();
            sequenceEditor.Styles[Style.Default].Font = "Consolas";
            sequenceEditor.Styles[Style.Default].Size = 11;
            sequenceEditor.Styles[Style.Default].BackColor = IntToColor(0x212121);
            sequenceEditor.Styles[Style.Default].ForeColor = IntToColor(0xE7E7E7);
            sequenceEditor.CaretForeColor = IntToColor(0xFFFFFF);
            sequenceEditor.StyleClearAll();
            sequenceEditor.ScrollWidth = 1;
            sequenceEditor.ScrollWidthTracking = true;
            sequenceEditor.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            sequenceEditor.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            sequenceEditor.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            sequenceEditor.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);
            sequenceEditor.Lexer = Lexer.Container;
            sequenceEditor.StyleNeeded += new EventHandler<StyleNeededEventArgs>(this.SEQ_StyleNeeded);
            sequenceEditor.UpdateUI += new EventHandler<UpdateUIEventArgs>(this.SEQ_Changed);
            sequenceEditor.TextChanged += new EventHandler(this.SEQ_ChangedText);
            StyleSeq(0, sequenceEditor.Text.Length);
            UpdateLineNumbers(0, sequenceEditor.Text.Length);
        }

        /// <summary>
        /// Sequence changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SEQ_Changed(object sender, EventArgs e) {
            /*
            var startPos = sequenceEditor.CurrentPosition;
            var endPos = startPos + 5000;

            if (startPos >= 500) { startPos -= 500; } else { startPos = 0; }
            if ((sequenceEditor.Text.Length - endPos) >= 500) { endPos += 500; } else { endPos = sequenceEditor.Text.Length; }
          
            StyleSeq(startPos, endPos);*/
        }

        /// <summary>
        /// Sequence changed text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SEQ_ChangedText(object sender, EventArgs e) {
            //Remove comment area.
            string s = sequenceEditor.Lines[sequenceEditor.CurrentLine].Text;
            if (s.Contains(";")) {
                s = s.Split(';')[0];
            }

            //Remove spaces.
            s = s.Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
            var com = prevCom;
            try {
                com = SequenceData.CommandFromString(s, 0, new List<int>(), new Dictionary<string, int>(), new Dictionary<int, string>());
            } catch { }
            if (com != prevCom) {
                if (prevCom != null) {
                    if (com == null) {
                        UpdateLineNumbers(0, sequenceEditor.Lines.Count);
                        prevCom = com;
                    } else {
                        if (com.Identifier != prevCom.Identifier) {
                            UpdateLineNumbers(0, sequenceEditor.Lines.Count);
                            prevCom = com;
                        }
                    }
                } else {
                    if (com != null) {
                        UpdateLineNumbers(0, sequenceEditor.Lines.Count);
                        prevCom = com;
                    }
                }
            }
        }

        /// <summary>
        /// SEQ needs style.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SEQ_StyleNeeded(object sender, StyleNeededEventArgs e) {
            var startPos = sequenceEditor.GetEndStyled();
            var endPos = e.Position;
            
            if (startPos >= 500) { startPos -= 500; } else { startPos = 0; }
            if ((sequenceEditor.Text.Length - endPos) >= 500) { endPos += 500; } else { endPos = sequenceEditor.Text.Length; }
            
            StyleSeq(startPos, endPos);
        }

        /// <summary>
        /// Style the SEQ.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        public void StyleSeq(int startPos, int endPos) {

            //Syntax highlighting.
            sequenceEditor.Styles[(int)CommandStyleType.Regular].ForeColor = IntToColor(0xE7E7E7);
            sequenceEditor.Styles[(int)CommandStyleType.Comment].ForeColor = IntToColor(0xAEAEAE);
            sequenceEditor.Styles[(int)CommandStyleType.Label].ForeColor = IntToColor(0xE7BB00);
            sequenceEditor.Styles[(int)CommandStyleType.Prefix].ForeColor = IntToColor(0x4AF0B6);
            sequenceEditor.Styles[(int)CommandStyleType.Value0].ForeColor = Color.Red;
            sequenceEditor.Styles[(int)CommandStyleType.Value1].ForeColor = Color.Orange;
            sequenceEditor.Styles[(int)CommandStyleType.Value2].ForeColor = Color.Yellow;
            sequenceEditor.Styles[(int)CommandStyleType.Value3].ForeColor = Color.LimeGreen;
            sequenceEditor.Styles[(int)CommandStyleType.Value4].ForeColor = Color.LightBlue;
            sequenceEditor.Styles[(int)CommandStyleType.Value5].ForeColor = Color.PaleVioletRed;

            //Sum.
            int pos = startPos;
            if (endPos > sequenceEditor.Text.Length) {
                endPos = sequenceEditor.Text.Length;
            }
            CommandStyleType style = CommandStyleType.Regular;
            string[] lines = sequenceEditor.Text.Substring(startPos, endPos - startPos).Split('\n');
            foreach (string s in lines) {

                //Do each char.
                style = CommandStyleType.Regular;
                bool initialSpaceCut = false;
                string withoutInitialSpace = s.Replace("\t", " ");
                int numWhiteSpace = 0;
                for (int j = 0; j < s.Length; j++) {

                    //Convert tabs to spaces.
                    string l = s.Replace("\t", " ");

                    //Label.
                    if (l.Contains(":") && j == 0) {
                        sequenceEditor.StartStyling(pos);
                        sequenceEditor.SetStyling(l.IndexOf(':') + 1, (int)CommandStyleType.Label);
                        j += l.IndexOf(':') + 1;
                        if (j >= l.Length) {
                            break;
                        }
                    }

                    //Jump to cut off intro spaces.
                    bool kill = false;
                    while ((l[j] == ' ') && !initialSpaceCut) {
                        j++;
                        if (j >= l.Length) {
                            kill = true;
                            break;
                        } else {
                            withoutInitialSpace = l.Substring(j, l.Length - j);
                            numWhiteSpace = j;
                        }
                    }
                    initialSpaceCut = true;
                    if (kill) {
                        break;
                    }

                    //Get char and index.
                    char c = l[j];
                    int ind = j + pos;

                    //Comment.
                    if (c == ';') {
                        sequenceEditor.StartStyling(ind);
                        sequenceEditor.SetStyling(l.Length - j, (int)CommandStyleType.Comment);
                        break;
                    }

                    //Prefix.
                    if (c == '_') {

                        //Check prefix.
                        string p = l.Substring(j, l.Length - j).Split(' ')[0];
                        bool afterSpace = false;
                        if (withoutInitialSpace.Contains(" ")) {
                            if (j > withoutInitialSpace.IndexOf(" ") + numWhiteSpace) { afterSpace = true; }
                        }
                        if (!afterSpace && (p.Contains("_if ") || p.Contains("_v ") || p.Contains("_r ") || p.Contains("_t ") || p.Contains("_tr ") || p.Contains("_tv ") || p.EndsWith("_if") || p.EndsWith("_v") || p.EndsWith("_t") || p.EndsWith("_tv") || p.EndsWith("_tr") || p.EndsWith("_r"))) {
                            style = CommandStyleType.Prefix;
                        }

                    }

                    //Space.
                    if (c == ' ') {
                        if (j > 0) {
                            if (l[j - 1] != ' ') {
                                if (style < CommandStyleType.Prefix) {
                                    style = CommandStyleType.Prefix;
                                }
                                style++;
                            }
                        }
                    }

                    //Do style.
                    sequenceEditor.StartStyling(ind);
                    sequenceEditor.SetStyling(1, (int)style);

                }

                //Add position, plus one for \r.
                pos += s.Length + 1;

            }

        }

        /// <summary>
        /// Command style.
        /// </summary>
        public enum CommandStyleType {
            Null, Regular, Comment, Label, Prefix, Value0, Value1, Value2, Value3, Value4, Value5
        }

        /// <summary>
        /// Int to color.
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static Color IntToColor(int rgb) {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        /// <summary>
        /// Load sequence text.
        /// </summary>
        public void LoadSequenceText(string name = "Sequence", string type = "F") {

            //Why not.
            sequenceEditor.Margins[0].Type = MarginType.RightText;
            sequenceEditor.Margins[0].Width = 35;

            //File is not null.
            if (File != null) {

                //Not read only.
                sequenceEditor.ReadOnly = false;

                //Set the text.
                sequenceEditor.Text = String.Join("\n", (File as SoundSequence).SequenceData.ToSeqFile(name, type));

            }

            //File is null.
            else {
                sequenceEditor.ReadOnly = false;
                sequenceEditor.Text = "{ NULL FILE INFO }";
                sequenceEditor.ReadOnly = true;
            }

            //Update line numbers.
            UpdateLineNumbers(0, sequenceEditor.Lines.Count);

        }

        private void UpdateLineNumbers(int startingAtLine, int endingAtLine) {

            //Get previous number.
            int pastNum = 0;
            if (startingAtLine != 0) {
                pastNum = int.Parse(sequenceEditor.Lines[startingAtLine - 1].MarginText);
            }

            //Add each command length.
            int sum = pastNum;
            if (endingAtLine > sequenceEditor.Lines.Count) {
                endingAtLine = sequenceEditor.Lines.Count;
            }
            for (int i = startingAtLine; i < endingAtLine; i++) {

                //Set style.
                sequenceEditor.Lines[i].MarginStyle = Style.LineNumber;

                //Remove comment area.
                string s = sequenceEditor.Lines[i].Text;
                if (s.Contains(";")) {
                    s = s.Split(';')[0];
                }

                //Remove spaces.
                s = s.Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");

                //Get num.
                sequenceEditor.Lines[i].MarginText = "" + sum;

                //Number to add.
                try {
                    var com = SequenceData.CommandFromString(s, 0, new List<int>(), new Dictionary<string, int>(), new Dictionary<int, string>());
                    if (com != null) {
                        sum += com.ByteSize;
                    }
                } catch { }

            }

        }



        private void scintilla_Insert(object sender, ModificationEventArgs e) {
            // Only update line numbers if the number of lines changed
            if (e.LinesAdded != 0)
                UpdateLineNumbers(0, sequenceEditor.Lines.Count);
        }

        private void scintilla_Delete(object sender, ModificationEventArgs e) {
            // Only update line numbers if the number of lines changed
            if (e.LinesAdded != 0)
                UpdateLineNumbers(0, sequenceEditor.Lines.Count);
        }

    }

}
