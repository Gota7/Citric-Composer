using SolarFileLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarAudioSlayer {

    /// <summary>
    /// Rescource editor.
    /// </summary>
    public class RescourceEditor : EditorBase {

        /// <summary>
        /// New rescource editor.
        /// </summary>
        /// <param name="mainWindow">Main window.</param>
        public RescourceEditor(MainWindow mainWindow) : base(typeof(AudioRescource), "Rescource", "ars", "Rescource Editor", mainWindow) {
            Text = "Rescource Editor";
            Icon = Properties.Resources.Chocolat;
        }

        /// <summary>
        /// Update nodes.
        /// </summary>
        public override void UpdateNodes() {

            //Begin update.
            BeginUpdateNodes();

            //Add assets if needed.
            if (tree.Nodes.Count < 2) {
                tree.Nodes.Add("assets", "Assets", 6, 6);
            }

            //File open.
            if (FileOpen) {

                int aCount = 0;
                foreach (var a in File as AudioRescource) {

                    //Icon index.
                    int iconInd = 2;
                    if (a.Meta.AudioType == AudioType.Stream) {
                        iconInd = 1;
                    }

                    //Add item.
                    tree.Nodes["assets"].Nodes.Add("asset" + aCount, "[" + aCount + "] " + a.Meta.Name, iconInd, iconInd);

                    //Add markers.
                    tree.Nodes["assets"].Nodes["asset" + aCount].Nodes.Add("markers", "Markers", 9, 9);
                    for (int i = 0; i < a.Meta.Markers.Count; i++) {
                        tree.Nodes["assets"].Nodes["asset" + aCount].Nodes["markers"].Nodes.Add("Marker " + i);
                    }

                    //Add exts.
                    tree.Nodes["assets"].Nodes["asset" + aCount].Nodes.Add("exts", "External", 8, 8);
                    for (int i = 0; i < a.Meta.Exts.Count; i++) {
                        tree.Nodes["assets"].Nodes["asset" + aCount].Nodes["exts"].Nodes.Add("External " + i);
                    }

                    //Increment count.
                    aCount++;

                }           

            }

            //End update.
            EndUpdateNodes();

        }

    }

}
