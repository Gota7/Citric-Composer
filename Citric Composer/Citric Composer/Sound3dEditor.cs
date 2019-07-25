using CitraFileLoader;
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

    /// <summary>
    /// Sound 3d editor.
    /// </summary>
    public partial class Sound3dEditor : Form {

        private Sound3dInfo bak;
        bool ok = false;

        public Sound3dEditor() {
            InitializeComponent();
        }

        public static Sound3dInfo GetInfo(Sound3dInfo bak, string name) {
            Sound3dEditor e = new Sound3dEditor();
            e.Text = name;
            e.bak = bak;
            e.attenuationCurve.SelectedIndex = (int)bak.AttenuationCurve;
            e.attenuationRate.Value = (decimal)bak.AttenuationRate;
            e.dopplerFactor.Value = bak.DopplerFactor;
            e.filter.Checked = bak.Filter;
            e.pan.Checked = bak.Pan;
            e.priority.Checked = bak.Priority;
            e.surroundPan.Checked = bak.Span;
            e.unknownFlag.Checked = bak.UnknownFlag;
            e.volume.Checked = bak.Volume;
            e.ShowDialog();
            if (e.ok) {
                return new Sound3dInfo()
                {
                    AttenuationCurve = (Sound3dInfo.EAttenuationCurve)e.attenuationCurve.SelectedIndex,
                    AttenuationRate = (float)e.attenuationRate.Value,
                    DopplerFactor = (byte)e.dopplerFactor.Value,
                    Filter = e.filter.Checked,
                    Pan = e.pan.Checked,
                    Priority = e.priority.Checked,
                    Span = e.surroundPan.Checked,
                    UnknownFlag = e.unknownFlag.Checked,
                    Volume = e.volume.Checked
                };
            } else {
                return e.bak;
            }
        }

        private void OkButton_Click_1(object sender, EventArgs e) {
            ok = true;
            Close();
        }

    }
}
