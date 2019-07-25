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
    /// Sound editor.
    /// </summary>
    public partial class SoundEditor : Form {

        private SoundInfo bak;
        bool ok = false;

        public SoundEditor() {
            InitializeComponent();
        }

        public static SoundInfo GetInfo(SoundInfo bak, SoundArchive a, string name) {

            //Sound editor.
            SoundEditor e = new SoundEditor();

            //Player grid.
            e.player.Items.Clear();
            foreach (var p in a.Players) {
                e.player.Items.Add(p.Name != null ? p.Name : "{ Unknown Player Name }");
            }

            //Other stuff.
            e.Text = name;
            e.frontBypass.Checked = bak.IsFrontBypass;
            e.panCurve.SelectedIndex = (int)bak.PanCurve;
            e.panMode.SelectedIndex = (int)bak.PanMode;
            e.player.SelectedIndex = a.Players.IndexOf(bak.Player);
            e.playerActorId.SelectedIndex = bak.PlayerActorId;
            e.playerPriority.Value = bak.PlayerPriority;
            e.remoteFilter.Value = bak.RemoteFilter;
            e.volume.Value = bak.Volume;
            e.p1Enable.Checked = e.p1.Enabled = bak.UserParamsEnabled[0];
            e.p2Enable.Checked = e.p2.Enabled = bak.UserParamsEnabled[1];
            e.p3Enable.Checked = e.p3.Enabled = bak.UserParamsEnabled[2];
            e.p4Enable.Checked = e.p4.Enabled = bak.UserParamsEnabled[3];
            e.p1.Value = bak.UserParamsEnabled[0] ? bak.UserParameter[0] : 0;
            e.p2.Value = bak.UserParamsEnabled[1] ? bak.UserParameter[1] : 0;
            e.p3.Value = bak.UserParamsEnabled[2] ? bak.UserParameter[2] : 0;
            e.p4.Value = bak.UserParamsEnabled[3] ? bak.UserParameter[3] : 0;
            e.bak = bak;
            e.ShowDialog();
            if (e.ok) {
                return new SoundInfo()
                {
                    File = bak.File,
                    IsFrontBypass = e.frontBypass.Checked,
                    Name = bak.Name,
                    PanCurve = (SoundInfo.EPanCurve)e.panCurve.SelectedIndex,
                    PanMode = (SoundInfo.EPanMode)e.panMode.SelectedIndex,
                    Player = a.Players[e.player.SelectedIndex],
                    PlayerActorId = (sbyte)e.playerActorId.SelectedIndex,
                    PlayerPriority = (sbyte)e.playerPriority.Value,
                    RemoteFilter = (sbyte)e.remoteFilter.Value,
                    Sound3dInfo = bak.Sound3dInfo,
                    Volume = (byte)e.volume.Value,
                    UserParamsEnabled = new bool[] { e.p1Enable.Checked, e.p2Enable.Checked, e.p3Enable.Checked, e.p4Enable.Checked },
                    UserParameter = new uint[]
                    {
                        (uint)(e.p1Enable.Checked ? e.p1.Value : 0),
                        (uint)(e.p2Enable.Checked ? e.p2.Value : 0),
                        (uint)(e.p3Enable.Checked ? e.p3.Value : 0),
                        (uint)(e.p4Enable.Checked ? e.p4.Value : 0)
                    }
                };
            } else {
                return e.bak;
            }
        }

        private void OkButton_Click(object sender, EventArgs e) {
            ok = true;
            Close();
        }

        private void P1Enable_CheckedChanged(object sender, EventArgs e) {
            p1.Enabled = p1Enable.Checked;
        }

        private void P2Enable_CheckedChanged(object sender, EventArgs e) {
            p2.Enabled = p2Enable.Checked;
        }

        private void P3Enable_CheckedChanged(object sender, EventArgs e) {
            p3.Enabled = p3Enable.Checked;
        }

        private void P4Enable_CheckedChanged(object sender, EventArgs e) {
            p4.Enabled = p4Enable.Checked;
        }

    }
}
