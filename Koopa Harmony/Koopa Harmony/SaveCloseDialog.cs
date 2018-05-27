using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoopaHarmony
{
    public partial class SaveCloseDialog : Form
    {
        public SaveCloseDialog()
        {
            InitializeComponent();
        }

        int returnValue = 0;

        private void SaveCloseDialog_Load(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            returnValue = 0;
            this.Close();
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            returnValue = 1;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            returnValue = 2;
            this.Close();
        }


        public int getValue() {

            this.ShowDialog();
            return returnValue;

        }
    }
}
