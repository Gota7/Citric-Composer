using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {

    /// <summary>
    /// An audio file.
    /// </summary>
    public abstract class AudioFile {

        /// <summary>
        /// Magic.
        /// </summary>
        public string Magic;

        /// <summary>
        /// Version of the audio file.
        /// </summary>
        public Version Version;

        /// <summary>
        /// Byte order of the file.
        /// </summary>
        public ByteOrder ByteOrder;

        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public abstract void Read(BinaryDataReader br);

        /// <summary>
        /// Read a file from a stream.
        /// </summary>
        /// <param name="src">The stream.</param>
        public void Read(MemoryStream src) => Read(new BinaryDataReader(src));

        /// <summary>
        /// Read a file from a byte array.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Read(byte[] file) => Read(new MemoryStream(file));

        /// <summary>
        /// Read a file from its path.
        /// </summary>
        /// <param name="filePath">Path to read the file from.</param>
        public void Read(string filePath) => Read(File.ReadAllBytes(filePath));

        /// <summary>
        /// Read the file magic.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="length">Length of the magic.</param>
        public void ReadMagic(BinaryDataReader br, int length = 4) {
            Magic = new string(br.ReadChars(length));
        }

        /// <summary>
        /// Read the byte order, and correct the reader endian.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void ReadByteOrder(BinaryDataReader br) {

            //Default.
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            ByteOrder = (ByteOrder)br.ReadUInt16();

            //Switch to little endian if needed.
            if (ByteOrder == ByteOrder.LittleEndian) {
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            }

        }

        /// <summary>
        /// Read the version.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void ReadVersion(BinaryDataReader br) {
            Version = new Version(br.ReadUInt16());
        }

        /// <summary>
        /// Write a file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public abstract void Write(BinaryDataWriter bw);

        /// <summary>
        /// Write a file to a stream.
        /// </summary>
        /// <param name="o">The output stream.</param>
        public void Write(MemoryStream o) => Write(new BinaryDataWriter(o));

        /// <summary>
        /// Write a file into a byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        public byte[] ToBytes() { MemoryStream o = new MemoryStream(); Write(o); return o.ToArray(); }

        /// <summary>
        /// Write a file to a path.
        /// </summary>
        /// <param name="filePath">Path to write to.</param>
        public void Write(string filePath) => File.WriteAllBytes(filePath, ToBytes());

        /// <summary>
        /// Write the file magic, and change the writer to proper byte order.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void WriteMagic(BinaryDataWriter bw) {

            //Write magic.
            bw.Write(Magic.ToCharArray());

            //Proper byte order.
            switch (ByteOrder) {

                //Big.
                case ByteOrder.BigEndian:
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                    break;

                //Little.
                case ByteOrder.LittleEndian:
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                    break;

            }

        }

        /// <summary>
        /// Write the byte order.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void WriteByteOrder(BinaryDataWriter bw) {
            bw.Write((UInt16)0xFEFF);
        }

        /// <summary>
        /// Write the version.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void WriteVersion(BinaryDataWriter bw) {
            bw.Write(Version.ToShort());
        }

    }

}
