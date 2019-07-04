using SolarFileLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolarAudioSlayer {
    public partial class MainWindow : EditorBase {
        public MainWindow() : base(typeof(Linker), "Linker", "slnk", "Solar Audio Slayer", null) {
            InitializeComponent();
            MainWindow = this;
            toolsToolStripMenuItem.Visible = true;
        }

        public override void UpdateNodes() {
            
        }

    }
}
