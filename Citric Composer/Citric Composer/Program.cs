using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitraFileLoader;
using Syroot.BinaryData;

namespace Citric_Composer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
            //Application.Run(new IsabelleSoundEditor());

            /*
            b_sar b = new b_sar();
            b.Load(File.ReadAllBytes("BigRedSound.bcsar"));
            ProjectExporter.WriteFSPJ("C:\\Users\\samsp\\Desktop\\SAR DECOMP\\EX_4F", "Export", b, ProjectExporter.ExportType.Cafe);
            ProjectExporter.WriteFSST("C:\\Users\\samsp\\Desktop\\SAR DECOMP\\EX_4F", "Export", b, ProjectExporter.ExportType.Cafe);
            

            b_sar b = new b_sar();
            b.Load(File.ReadAllBytes("BgmData.bfsar"));
            int id = 0;
            var w = b.info.files;
            foreach (var h in b.info.files) {
                if (h.file != null) {
                    File.WriteAllBytes("TEMP/" + id.ToString("D4"), h.file);
                }
                id++;
            }

            SoundWaveArchive s = new SoundWaveArchive();
            MemoryStream src = new MemoryStream(File.ReadAllBytes("sample.bfwar"));
            BinaryDataReader br = new BinaryDataReader(src);
            s.Read(br);

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            s.Write(bw);

            File.WriteAllBytes("sampleOUT.bfwar", o.ToArray());

            if (args.Length > 0) {

                PrefetchFile p = new PrefetchFile();
                p.Stream = new CitraFileLoader.Stream();
                MemoryStream src = new MemoryStream(File.ReadAllBytes(args[0]));
                BinaryDataReader br = new BinaryDataReader(src);
                p.Stream.Read(br);
                p.Version = new FileWriter.Version(5, 1, 0);
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);
                p.Write(WriteMode.NX, bw);
                File.WriteAllBytes(Path.GetFileNameWithoutExtension(args[0]) + ".bfstp", o.ToArray());

            } else {

                Console.WriteLine("Usage: bfstm2bfstp.exe something.bfstm");

            }

            MemoryStream src = new MemoryStream(File.ReadAllBytes("sample.bfwsd"));
            BinaryDataReader br = new BinaryDataReader(src);
            WaveSoundData d = new WaveSoundData();
            d.Read(br);

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            d.Write(bw);
            File.WriteAllBytes("sampleOUT.bfwsd", o.ToArray());

            MemoryStream src = new MemoryStream(File.ReadAllBytes("BANK_0.bfbnk"));
            BinaryDataReader br = new BinaryDataReader(src);
            SoundBank b = new SoundBank();
            b.Read(br);

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            b.Write(bw);
            File.WriteAllBytes("BANK_0OUT.bfbnk", o.ToArray());

            */

            //SoundArchive a = SoundArchiveReader.ReadSoundArchive(File.ReadAllBytes("BgmData.bfsar"));

        }
    }
}
