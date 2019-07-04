using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// A vibration file.
    /// </summary>
    public class Vibration {

        /// <summary>
        /// Sample rate.
        /// </summary>
        public ushort SampleRate;

        /// <summary>
        /// If the file loops.
        /// </summary>
        public bool Loops;

        /// <summary>
        /// Loop start sample.
        /// </summary>
        public uint LoopStart;

        /// <summary>
        /// Loop end sample.
        /// </summary>
        public uint LoopEnd;

        /// <summary>
        /// Has loop interval.
        /// </summary>
        public bool HasLoopInterval;

        /// <summary>
        /// Loop interval.
        /// </summary>
        public uint LoopInterval;

        /// <summary>
        /// Signed PCM8.
        /// </summary>
        public sbyte[] pcm8;

        /// <summary>
        /// Load a file.
        /// </summary>
        /// <param name="b">The file.</param>
        public void Load(byte[] b) {

            //New reader.
            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);
            br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            //Size.
            uint size = br.ReadUInt32();

            //Get format.
            ushort format = br.ReadUInt16();
            if (format != 3) {
                throw new Exception("Only PCM BNVIB is supported!");
            }

            //Sample rate.
            SampleRate = br.ReadUInt16();

            //Loop.
            if (size >= 0xC) {
                Loops = true;
                LoopStart = br.ReadUInt32();
                LoopEnd = br.ReadUInt32();
            }

            //Loop interval.
            if (size >= 0x10) {
                LoopInterval = br.ReadUInt32();
            }

            //Data size.
            uint dataSize = br.ReadUInt32();
            pcm8 = br.ReadSBytes((int)dataSize);

        }

    }

}
