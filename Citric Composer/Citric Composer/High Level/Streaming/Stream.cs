using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Stream.
    /// </summary>
    public class Stream : ISoundFile {

        /// <summary>
        /// Actual stream.
        /// </summary>
        public b_stm Stm;

        /// <summary>
        /// Get the Riff-Wave.
        /// </summary>
        public RiffWave Riff { get => RiffWaveFactory.CreateRiffWave(Stm); }

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + Stm.fileHeader.magic[0] + "STM").ToLower();
        }

        /// <summary>
        /// Read.
        /// </summary>
        public void Read(BinaryDataReader br) {
            Stm = new b_stm();

            //Get byte order.
            br.Position = 4;
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            if (br.ReadUInt16() == ByteOrder.LittleEndian) {

                //Little endian.
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            }

            br.Position += 0xC;
            UInt32 length = br.ReadUInt32();
            br.Position -= 0x10;
            Stm.Load(br.ReadBytes((int)length));
        }

        public void Write(WriteMode writeMode, BinaryDataWriter bw) {
            switch (writeMode) {
                case WriteMode.Cafe:
                    bw.Write(Stm.ToBytes(ByteOrder.BigEndian, true));
                    break;
                case WriteMode.CTR:
                    bw.Write(Stm.ToBytes(ByteOrder.LittleEndian, false));
                    break;
                case WriteMode.NX:
                    bw.Write(Stm.ToBytes(ByteOrder.LittleEndian, true));
                    break;
            }
        }

        public void Write(BinaryDataWriter bw) {
            bw.Write(Stm.ToBytes(Stm.fileHeader.byteOrder));
        }

    }

}
