using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CitraFileLoader {

    /// <summary>
    /// SDK exporter.
    /// </summary>
    public partial class SdkExporter {

        /// <summary>
        /// Write project file.
        /// </summary>
        /// <param name="folderPath">Directory to write files.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode">How to export it.</param>
        public static void WriteFiles(string folderPath, SoundArchive a, WriteMode mode) {

            //Write wave archives.
            /*foreach (var war in a.WaveArchives) {

                //File is not null.
                if (war.File != null) {
                    var folderName = Path.GetFileNameWithoutExtension(war.File.FileName);
                    Directory.CreateDirectory(folderPath + "/Files/Wave Archives/" + folderName);
                    int count = 0;
                    foreach (var w in war.File.File as SoundWaveArchive) {
                        File.WriteAllBytes(folderPath + "/Files/Wave Archives/" + folderName + "/" + count++ + ".wav", w.Riff.ToBytes());
                    }
                }

            }*/

            //Write banks.
            foreach (var bnk in a.Banks) {

                //File is not null.
                if (bnk.File != null) {
                    var fileName = Path.GetFileNameWithoutExtension(bnk.File.FileName);
                    Directory.CreateDirectory(folderPath + "/Files/Banks");
                    WriteBnk(folderPath, a, bnk, mode);
                }

            }

            //Write sequences.
            /*foreach (var seq in a.Sequences) {

                //File is not null.
                if (seq.File != null) {
                    var fileName = Path.GetFileNameWithoutExtension(seq.File.FileName);
                    Directory.CreateDirectory(folderPath + "/Files/Sequences");
                    File.WriteAllLines(folderPath + "/Files/Sequences/" + fileName + "." + (mode == WriteMode.CTR ? "c" : "f") + "seq", (seq.File.File as SoundSequence).SequenceData.ToSeqFile(fileName, mode == WriteMode.CTR ? "C" : "F"));
                }

            }*/

        }

    }

}
