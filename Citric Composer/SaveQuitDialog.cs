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

        public SaveQuitDialog(IsabelleSoundEditor parent2)
        {
            InitializeComponent();
            parent = parent2;
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
            Application.Exit();
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            //Save application
            parent.save();

            //Exit application
            Application.Exit();

        }
    }
}
