using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// A wave file.
    /// </summary>
    public class Wave : ISoundFile {

        /// <summary>
        /// Actual wave.
        /// </summary>
        public b_wav Wav;

        /// <summary>
        /// Get the Riff-Wave.
        /// </summary>
        public RiffWave Riff { get => RiffWaveFactory.CreateRiffWave(Wav); }

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + Wav.fileHeader.magic[0] + "WAV").ToLower();
        }

        public void Read(BinaryDataReader br) {
            Wav = new b_wav();

            //Get byte order.
            br.Position = 4;
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            if (br.ReadUInt16() == ByteOrder.LittleEndian) {

                //Little endian.
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            }

            br.Position = 0xC;
            UInt32 length = br.ReadUInt32();
            br.Position -= 0x10;
            Wav.Load(br.ReadBytes((int)length));
        }

        public void Write(WriteMode writeMode, BinaryDataWriter bw) {
            switch (writeMode) {
                case WriteMode.Cafe:
                    bw.Write(Wav.ToBytes(ByteOrder.BigEndian, true));
                    break;
                case WriteMode.CTR:
                    bw.Write(Wav.ToBytes(ByteOrder.LittleEndian, false));
                    break;
                case WriteMode.NX:
                    bw.Write(Wav.ToBytes(ByteOrder.LittleEndian, true));
                    break;
            }
        }

        public void Write(BinaryDataWriter bw) {
            bw.Write(Wav.ToBytes(Wav.fileHeader.byteOrder));
        }
    }

}
