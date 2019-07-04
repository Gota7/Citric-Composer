using System;
using System.Windows.Forms;

namespace SolarAudioSlayer {
    public partial class SaveQuitDialog : Form
    {

        Citric_Composer.IsabelleSoundEditor parent;
        EditorBase parentTwo;

        public SaveQuitDialog(Citric_Composer.IsabelleSoundEditor parent2)
        {
            InitializeComponent();
            parent = parent2;
        }

        public SaveQuitDialog(EditorBase parent2)
        {
            InitializeComponent();
            parentTwo = parent2;
        }

        private void SaveQuitDialog_Load(object sender, EventArgs e)
        {

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            try { parent.Close(); } catch { }
            try { parentTwo.Close(); } catch { }
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            //Save application
            //try { parent.save(); } catch { }
            //try { parentTwo.save(); } catch { }
            try { parentTwo.saveToolStripMenuItem_Click(sender, e); } catch { }

            //Exit application
            try { parent.Close(); } catch { }
            try { parentTwo.Close(); } catch { }

        }
    }
}
