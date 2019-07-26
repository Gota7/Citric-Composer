using CitraFileLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer {

    /// <summary>
    /// File wizard.
    /// </summary>
    public partial class FileWizard : Form {

        SoundArchive.NewFileEntryType type;
        bool cancel = true;

        public FileWizard() {
            InitializeComponent();
        }

        public static SoundFile<ISoundFile> GetInfo(SoundArchive a, SoundArchive.NewFileEntryType type, int lastEntry, string filePath) {

            //File wizard.
            FileWizard w = new FileWizard();
            w.type = type;

            //Add existing files.
            foreach (var f in a.Files) {
                w.existingFiles.Items.Add(f.FileName + "." + f.FileExtension);
            }

            //Prepare info.
            w.referenceFileExternally.Enabled = false;
            w.referenceFileExternally.Checked = type == SoundArchive.NewFileEntryType.Stream;
            w.blankFile.Enabled = type != SoundArchive.NewFileEntryType.Stream && type != SoundArchive.NewFileEntryType.Prefetch;
            w.useExistingFile.Checked = true;
            w.useExistingFile.Enabled = w.newFile.Checked = a.Files.Count == 0;
            w.useExistingFile.Enabled = !w.useExistingFile.Enabled;
            w.okButton.Enabled = false;

            //Stream specifics.
            if (type == SoundArchive.NewFileEntryType.Stream) {
                w.blankFile.Enabled = false;
            }

            //Show.
            w.ShowDialog();

            //Return data.
            if (w.cancel) {
                return new SoundFile<ISoundFile>() { FileId = -1 };
            } else {

                //Get versions.
                byte maj = 1;
                byte min = 0;
                byte rev = 0;

                //Use existing file.
                if (w.useExistingFile.Checked) {
                    return new SoundFile<ISoundFile>() { Reference = a.Files[w.existingFiles.SelectedIndex] };
                }

                //Use new file.
                else if (w.newFile.Checked) {
                    if (w.referenceFileExternally.Checked) {
                        return new SoundFile<ISoundFile>() { Reference = a.AddNewFile(type, lastEntry, null, false, GetRelativePath(w.newFilePath.Text, Path.GetDirectoryName(filePath))) };
                    } else {
                        return new SoundFile<ISoundFile>() { Reference = a.AddNewFile(type, lastEntry, SoundArchiveReader.ReadFile(File.ReadAllBytes(w.newFilePath.Text))) };
                    }
                }

                //Blank file.
                else if (w.blankFile.Checked) {
                    ISoundFile f = null;
                    switch (type) {
                        case SoundArchive.NewFileEntryType.Bank:
                            foreach (var fi in a.Files) {
                                if (fi != null) {
                                    if (fi.File != null) {
                                        var z = fi.File as SoundBank;
                                        if (z != null) {
                                            maj = z.Version.Major;
                                            min = z.Version.Minor;
                                            rev = z.Version.Revision;
                                            break;
                                        }
                                    }
                                }
                            }
                            f = new SoundBank() { Version = new FileWriter.Version(maj, min, rev) };
                            break;
                        case SoundArchive.NewFileEntryType.Group:
                            foreach (var fi in a.Files) {
                                if (fi != null) {
                                    if (fi.File != null) {
                                        var z = fi.File as Group;
                                        if (z != null) {
                                            maj = z.Version.Major;
                                            min = z.Version.Minor;
                                            rev = z.Version.Revision;
                                            break;
                                        }
                                    }
                                }
                            }
                            f = new Group() { Version = new FileWriter.Version(maj, min, rev) };
                            break;
                        case SoundArchive.NewFileEntryType.Sequence:
                            foreach (var fi in a.Files) {
                                if (fi != null) {
                                    if (fi.File != null) {
                                        var z = fi.File as SoundSequence;
                                        if (z != null) {
                                            maj = z.Version.Major;
                                            min = z.Version.Minor;
                                            rev = z.Version.Revision;
                                            break;
                                        }
                                    }
                                }
                            }
                            f = new SoundSequence() { Version = new FileWriter.Version(maj, min, rev) };
                            break;
                        case SoundArchive.NewFileEntryType.WaveArchive:
                            foreach (var fi in a.Files) {
                                if (fi != null) {
                                    if (fi.File != null) {
                                        var z = fi.File as SoundWaveArchive;
                                        if (z != null) {
                                            maj = z.Version.Major;
                                            min = z.Version.Minor;
                                            rev = z.Version.Revision;
                                            break;
                                        }
                                    }
                                }
                            }
                            f = new SoundWaveArchive() { Version = new FileWriter.Version(maj, min, rev) };
                            break;
                        case SoundArchive.NewFileEntryType.WaveSoundData:
                            foreach (var fi in a.Files) {
                                if (fi != null) {
                                    if (fi.File != null) {
                                        var z = fi.File as WaveSoundData;
                                        if (z != null) {
                                            maj = z.Version.Major;
                                            min = z.Version.Minor;
                                            rev = z.Version.Revision;
                                            break;
                                        }
                                    }
                                }
                            }
                            f = new WaveSoundData() { Version = new FileWriter.Version(maj, min, rev) };
                            break;
                    }
                    return new SoundFile<ISoundFile>() { Reference = a.AddNewFile(type, lastEntry, f) };
                }

                //Null file.
                else {
                    return new SoundFile<ISoundFile>() { FileId = -2 };
                }

            }

        }

        private void UseExistingFile_CheckedChanged(object sender, EventArgs e) {
            if (useExistingFile.Checked) {
                existingFiles.Enabled = true;
                //referenceFileExternally.Enabled = false;
                browse.Enabled = false;
                okButton.Enabled = existingFiles.SelectedItem != null;
            }
        }

        private void NewFile_CheckedChanged(object sender, EventArgs e) {
            if (newFile.Checked) {
                existingFiles.Enabled = false;
                //referenceFileExternally.Enabled = true;
                browse.Enabled = true;
                if (newFilePath.Text == "") {
                    okButton.Enabled = false;
                }
            }
        }

        private void BlankFile_CheckedChanged(object sender, EventArgs e) {
            existingFiles.Enabled = false;
            //referenceFileExternally.Enabled = false;
            browse.Enabled = false;
            okButton.Enabled = true;
        }

        private void NullFile_CheckedChanged(object sender, EventArgs e) {
            if (nullFile.Checked) {
                existingFiles.Enabled = false;
                //referenceFileExternally.Enabled = false;
                browse.Enabled = false;
                okButton.Enabled = true;
            }
        }

        private void Browse_Click(object sender, EventArgs e) {
            OpenFileDialog o = new OpenFileDialog();
            o.RestoreDirectory = true;
            o.Title = "Select New File";
            o.Filter = "All Files|*.*";
            switch (type) {
                case SoundArchive.NewFileEntryType.Bank:
                    o.Filter = "Bank|*.bfbnk;*.bcbnk";
                    break;
                case SoundArchive.NewFileEntryType.Group:
                    o.Filter = "Group|*.bfgrp;*.bcgrp";
                    break;
                case SoundArchive.NewFileEntryType.Prefetch:
                    o.Filter = "Prefetch|*.bfstp;*.bcstp";
                    break;
                case SoundArchive.NewFileEntryType.Sequence:
                    o.Filter = "Sequence|*.bfseq;*.bcseq";
                    break;
                case SoundArchive.NewFileEntryType.Stream:
                    o.Filter = "Stream|*.bfstm;*.bcstm";
                    break;
                case SoundArchive.NewFileEntryType.WaveArchive:
                    o.Filter = "Wave Archive|*.bfwar;*.bcwar";
                    break;
                case SoundArchive.NewFileEntryType.WaveSoundData:
                    o.Filter = "Wave Sound Data|*.bfwsd;*.bcwsd";
                    break;
            }
            o.ShowDialog();
            if (o.FileName != "") {
                newFilePath.Text = o.FileName;
                okButton.Enabled = true;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            cancel = true;
            Close();
        }

        private void OkButton_Click(object sender, EventArgs e) {
            cancel = false;
            Close();
        }

        public static string GetRelativePath(string p_fullDestinationPath, string p_startPath) {
            string[] l_startPathParts = Path.GetFullPath(p_startPath).Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string[] l_destinationPathParts = p_fullDestinationPath.Split(Path.DirectorySeparatorChar);

            int l_sameCounter = 0;
            while ((l_sameCounter < l_startPathParts.Length) && (l_sameCounter < l_destinationPathParts.Length) && l_startPathParts[l_sameCounter].Equals(l_destinationPathParts[l_sameCounter], StringComparison.InvariantCultureIgnoreCase)) {
                l_sameCounter++;
            }

            if (l_sameCounter == 0) {
                return p_fullDestinationPath; // There is no relative link.
            }

            StringBuilder l_builder = new StringBuilder();
            for (int i = l_sameCounter; i < l_startPathParts.Length; i++) {
                l_builder.Append(".." + Path.DirectorySeparatorChar);
            }

            for (int i = l_sameCounter; i < l_destinationPathParts.Length; i++) {
                l_builder.Append(l_destinationPathParts[i] + Path.DirectorySeparatorChar);
            }

            l_builder.Length--;

            return l_builder.ToString().Replace('\\', '/');
        }

        private void ExistingFiles_SelectedIndexChanged(object sender, EventArgs e) {
            okButton.Enabled = true;
        }

    }

}
