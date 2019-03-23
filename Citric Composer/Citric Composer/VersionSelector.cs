using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer {
    public partial class VersionSelector : Form {

        public VersionSelector() {
            InitializeComponent();
        }

        private void setVer_Click(object sender, EventArgs e) {
            Close();
        }

        public void GetValues(byte maj, byte min, byte rev) {
            this.maj.Value = maj;
            this.min.Value = min;
            this.rev.Value = rev;
            ShowDialog();
        }

    }
}
