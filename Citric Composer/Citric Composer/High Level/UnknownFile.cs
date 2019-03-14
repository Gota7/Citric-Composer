using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Unknown file type.
    /// </summary>
    public class UnknownFile : ISoundFile {

        public UnknownFile(byte[] file) {
            File = file;
        }

        /// <summary>
        /// File data.
        /// </summary>
        public byte[] File;

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return "bin";
        }

        /// <summary>
        /// DO NOT USE IF UNKNOWN FILE. SPECIFY LENGTH INSTEAD.
        /// </summary>
        /// <param name="br"></param>
        public void Read(BinaryDataReader br) {
            //br.Position += 0xC;
            //UInt32 length = br.ReadUInt32();
            //br.Position -= 0x10;
            //File = br.ReadBytes((int)length);
        }

        public void Read(BinaryDataReader br, int length) {
            //File = br.ReadBytes(length);
        }

        public void Write(WriteMode writeMode, BinaryDataWriter bw) {
            bw.Write(File);
        }

        public void Write(BinaryDataWriter bw) {
            bw.Write(File);
        }
    }

}
