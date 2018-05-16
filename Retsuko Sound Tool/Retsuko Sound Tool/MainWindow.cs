using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retsuko_Sound_Tool
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<fileReplacement> replacements;
        public struct fileReplacement {

            public int offset; //Offset.
            public int size; //Size.
            public string path; //Path.
            public string name; //Name.

        }

        private void changeFileButton_Click(object sender, EventArgs e)
        {
            fileSelector.ShowDialog();
            if (fileSelector.FileName != "")
            {
                fileToExtractBox.Text = fileSelector.FileName;
                fileSelector.FileName = "";

            }
        }

        private void changeDirectoryButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != "")
            {

                directoryExtractPath.Text = folderBrowserDialog1.SelectedPath;
                folderBrowserDialog1.SelectedPath = "";

            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            extractor.extractFiles(File.ReadAllBytes(fileToExtractBox.Text), directoryExtractPath.Text);
        }

        private void mapFileBox_TextChanged(object sender, EventArgs e)
        {
            if (mapFileBox.Text != "") { fileBox.Enabled = true; } else { fileBox.Enabled = false; }
        }

        private void changeSourceButton_Click(object sender, EventArgs e)
        {
            fileSelector.ShowDialog();
            if (fileSelector.FileName != "")
            {
                sourceFileBox.Text = fileSelector.FileName;
                fileSelector.FileName = "";

            }
        }

        private void changeFileToInjectButton_Click(object sender, EventArgs e)
        {
            fileSelector.ShowDialog();
            if (fileSelector.FileName != "")
            {
                fileToInjectMap.Text = fileSelector.FileName;
                fileSelector.FileName = "";

            }
        }

        private void changeMapButton_Click(object sender, EventArgs e)
        {
            mapSelector.ShowDialog();
            if (mapSelector.FileName != "")
            {
                mapFileBox.Text = mapSelector.FileName;

                fileBox.Items.Clear();

                string[] map = File.ReadAllLines(mapSelector.FileName);
                replacements = new List<fileReplacement>();
                for (int i = 1; i < map.Length; i++)
                {

                    fileReplacement r = new fileReplacement();
                    string[] split = map[i].Split(';');
                    r.offset = int.Parse(split[0]);
                    r.size = int.Parse(split[1].Substring(1));
                    r.path = split[2].Substring(1);
                    r.name = split[3].Substring(1);
                    replacements.Add(r);

                }

                for (int i = 0; i < replacements.Count(); i++)
                {
                    fileBox.Items.Add(i + " - " + replacements[i].path + "; " + replacements[i].name);
                }

                mapSelector.FileName = "";

            }

        }

        private void injectButton_Click(object sender, EventArgs e)
        {
            injector.injectOneFile(replacements[fileBox.SelectedIndex], File.ReadAllBytes(fileToInjectMap.Text).ToList(), File.ReadAllBytes(sourceFileBox.Text), mapFileBox.Text, sourceFileBox.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool cancel = injector.injectAllFiles(replacements.ToArray(), Path.GetDirectoryName(mapFileBox.Text), File.ReadAllBytes(sourceFileBox.Text), sourceFileBox.Text);
        }
    }
}
