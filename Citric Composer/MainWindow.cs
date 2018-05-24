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
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// About.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutCitricComposerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //About windows.
            AboutWindow a = new AboutWindow();
            a.Show();
        }

        private void isabelleSoundEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Isabelle window.
            IsabelleSoundEditor a = new IsabelleSoundEditor();
            a.Show();
        }


        private void brewstersArchiveBrewerWARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Brewster window.
            Brewster_WAR_Brewer a = new Brewster_WAR_Brewer();
            a.Show();
        }
    }
}
