using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melanchall.DryWetMidi.Smf;

namespace SequenceDataLib {

    class Program {

        static void Main(string[] args) {

            /*
            MemoryStream src = new MemoryStream(File.ReadAllBytes("tmp.bin"));
            BinaryDataReader br = new BinaryDataReader(src);
            br.ByteOrder = ByteOrder.BigEndian;
            SequenceCommand s = SequenceCommand.Read(br);
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            bw.ByteOrder = ByteOrder.BigEndian;
            s.Write(bw);
            File.WriteAllBytes("tmp2.bin", o.ToArray());*/

            SequenceData d = new SequenceData();
            d.Read(File.ReadAllBytes("tmp.bin"), ByteOrder.LittleEndian);
            File.WriteAllBytes("tmp2.bin", d.ToBytes(ByteOrder.LittleEndian));
            File.WriteAllLines("out.fseq", d.ToSeqFile("Lancer"));
            //SequenceData d2 = new SequenceData();
            //d2.FromSeqFile(File.ReadAllLines("out.fseq"));

            MidiFile m = MidiGenerator.Sequence2Midi(d);
            m.Write("out.mid", true);

            MidiFile m2 = MidiFile.Read("Lancer.mid",
                             new ReadingSettings
                             {
                                 NoHeaderChunkPolicy = NoHeaderChunkPolicy.Abort,
                             });

            MidiFile m3 = MidiFile.Read("out.mid",
                             new ReadingSettings
                             {
                                 NoHeaderChunkPolicy = NoHeaderChunkPolicy.Abort,
                             });

        }

    }

}
