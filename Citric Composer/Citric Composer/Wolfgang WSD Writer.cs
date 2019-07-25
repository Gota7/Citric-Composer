using CitraFileLoader;
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
    public partial class Wolfgang_WSD_Writer : EditorBase {

        public Wolfgang_WSD_Writer(MainWindow mainWindow) : base(typeof(WaveSoundData), "Wave Sound Data", "wsd", "Wolfgang's Data Writer", mainWindow) {
            InitializeComponent();
            Text = "Wolfgang's Data Writer";
            Icon = Properties.Resources.Wolfgang;
        }

        public Wolfgang_WSD_Writer(string fileToOpen, MainWindow mainWindow) : base(typeof(WaveSoundData), "Wave Sound Data", "wsd", "Wolfgang's Data Writer", fileToOpen, mainWindow) {
            InitializeComponent();
            Text = "Wolfgang's Data Writer - " + Path.GetFileName(fileToOpen);
            Icon = Properties.Resources.Wolfgang;
        }

        public Wolfgang_WSD_Writer(SoundFile<ISoundFile> fileToOpen, MainWindow mainWindow) : base(typeof(WaveSoundData), "Wave Sound Data", "wsd", "Wolfgang's Data Writer", fileToOpen, mainWindow) {
            InitializeComponent();
            string name = ExtFile.FileName;
            if (name == null) {
                name = "{ Null File Name }";
            }
            Text = EditorName + " - " + name + "." + ExtFile.FileExtension;
            Icon = Properties.Resources.Wolfgang;
        }

        /// <summary>
        /// Update nodes.
        /// </summary>
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

            //Load needed wave files for WSD.
            if (MainWindow != null) {

            }

            //Add entries if doesn't exist.
            if (tree.Nodes.Count < 2) {
                tree.Nodes.Add("entries", "Entries", 7, 7);
            }

            //Add references if doesn't exist.
            if (tree.Nodes.Count < 3) {
                tree.Nodes.Add("references", "References", 6, 6);
            }

            //File open and not null.
            if (FileOpen && File != null) {

                //Root context menu.
                tree.Nodes["entries"].ContextMenuStrip = rootMenu;
                tree.Nodes["references"].ContextMenuStrip = rootMenu;

                //Add references.
                WaveSoundData d = File as WaveSoundData;
                for (int i = 0; i < d.Waves.Count(); i++) {

                    //Null entry.
                    if (d == null) {

                        //Add null entry.
                        tree.Nodes["references"].Nodes.Add("reference" + i, "[" + i + "] { Null Reference }", 0, 0);

                    }
                    
                    //Valid entry.
                    else {

                        //Get wave archive name.
                        string warName = "{ Null Wave Archive Name (" + d.Waves[i].WarIndex + ") }";
                        if (MainWindow != null) {

                            //If there is a B_SAR open.
                            if (MainWindow.File != null) {

                                try {
                                    if ((MainWindow.File as SoundArchive).WaveArchives[d.Waves[i].WarIndex].Name != null) {
                                        warName = (MainWindow.File as SoundArchive).WaveArchives[d.Waves[i].WarIndex].Name;
                                    }
                                } catch { }

                            }

                        }

                        //Add entry.
                        tree.Nodes["references"].Nodes.Add("reference" + i, "[" + i + "] " + warName + " - Wave " + d.Waves[i].WaveIndex, 2, 2);

                    }

                }

                //Add entries.
                for (int i = 0; i < d.DataItems.Count(); i++) {

                    //Null.
                    if (d.DataItems[i] == null) {

                        //Null entry.
                        tree.Nodes["entries"].Nodes.Add("entries" + i, "Null Entry (" + i + ")");

                    }

                    //Valid.
                    else {

                        //Valid.
                        tree.Nodes["entries"].Nodes.Add("entries" + i, "Entry " + i);

                        //Add tracks.
                        for (int j = 0; j < d.DataItems[i].NoteEvents.Count; j++) {

                            //Null.
                            if (d.DataItems[i].NoteEvents[j] == null) {

                            }

                            //Valid.
                            else {

                                //Add note events.
                                

                            }

                        }

                        //Add notes.
                        for (int j = 0; j < d.DataItems[i].Notes.Count; j++) {

                            //Null.
                            if (d.DataItems[i].Notes[j] == null) {

                            }

                            //Valid.
                            else {

                            }

                        }

                    }

                }

            } else {

                //Remove context menus.
                tree.Nodes["entries"].ContextMenuStrip = null;
                tree.Nodes["references"].ContextMenuStrip = null;

            }

            //End update.
            EndUpdateNodes();

        }

        //Function to load all needed dependencies.

        //Function to load a dependency if it's needed, or force load it.

    }
}
