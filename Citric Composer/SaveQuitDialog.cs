using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer
{
    public partial class SaveQuitDialog : Form
    {

        IsabelleSoundEditor parent;
        Brewster_WAR_Brewer parentTwo;

        public SaveQuitDialog(IsabelleSoundEditor parent2)
        {
            InitializeComponent();
            parent = parent2;
        }

        public SaveQuitDialog(Brewster_WAR_Brewer parent2)
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
            try { parent.save(); } catch { }
            try { parentTwo.save(); } catch { }

            //Exit application
            try { parent.Close(); } catch { }
            try { parentTwo.Close(); } catch { }

        }
    }
}
