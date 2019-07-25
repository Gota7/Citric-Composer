using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace SolarFileLoader {

    /// <summary>
    /// Audio rescource list (.barslist).
    /// </summary>
    public class AudioRescourceList : AudioFile {

        /// <summary>
        /// Name of this rescource.
        /// </summary>
        public string RescourceName;

        /// <summary>
        /// List of file names.
        /// </summary>
        public List<string> FileNames;

        /// <summary>
        /// New audio rescource list.
        /// </summary>
        public AudioRescourceList() {
            Magic = "ARSL";
            Version = new Version(0, 1);
            RescourceName = "NewRescourceList";
            FileNames = new List<string>();
        }

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void Read(BinaryDataReader br) {
            
            //Read magic.
            ReadMagic(br);

            //Read byte order.
            ReadByteOrder(br);

            //Read version.
            ReadVersion(br);

            //Rescource name offset.
            int resNameOffset = br.ReadInt32();

            //Number of filenames.
            int numFileNames = (int)br.ReadUInt32();

            //Offsets.
            int[] fileNameOffsets = br.ReadInt32s(numFileNames);

            //New file names.
            FileNames = new List<string>();

            //The table is a new structure.
            ReadHelper r = new ReadHelper(br);

            //Go to the rescource name offset.
            r.JumpToOffset(br, resNameOffset);

            //Read the rescource name.
            RescourceName = r.ReadNullTerminated(br);

            //Read each file name.
            foreach (int i in fileNameOffsets) {

                //Go to the offset.
                r.JumpToOffset(br, i);

                //Read the name.
                FileNames.Add(r.ReadNullTerminated(br));

            }

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void Write(BinaryDataWriter bw) {

            //Write header.
            WriteMagic(bw);
            WriteByteOrder(bw);
            WriteVersion(bw);

            //Rescource name offset will be 0.
            bw.Write((uint)0);

            //Write number of filenames.
            bw.Write((uint)FileNames.Count);

            //New write helper.
            WriteHelper w = new WriteHelper(bw);

            //Init offset table.
            w.InitOffsetTable(bw, "FileNamesTable");

            //Allocate memory to offsets.
            w.AllocateStructures(bw, FileNames.Count, 4);

            //New structure here.
            w.SS(bw);

            //Write the rescource name.
            w.WriteNullTerminated(bw, RescourceName);

            //Write each string.
            for (int i = 0; i < FileNames.Count; i++) {
                w.WriteOffsetTableEntry(bw, i, "FileNamesTable");
                w.WriteNullTerminated(bw, FileNames[i]);
            }

            //Close table.
            w.CloseOffsetTable("FileNamesTable");

            //End structure.
            w.ES();

        }

    }

}
