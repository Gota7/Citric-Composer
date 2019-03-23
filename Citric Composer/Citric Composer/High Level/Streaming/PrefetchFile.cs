using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Prefetch file.
    /// </summary>
    public class PrefetchFile : ISoundFile {

        /// <summary>
        /// Stream to use.
        /// </summary>
        public Stream Stream;

        /// <summary>
        /// Verison.
        /// </summary>
        public FileWriter.Version Version;

        /// <summary>
        /// Write mode.
        /// </summary>
        private WriteMode writeMode;

        /// <summary>
        /// Backup prefetch data.
        /// </summary>
        private byte[] BackupSTP;

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(writeMode) + "STP").ToLower();
        }

        /// <summary>
        /// Just gloss over the data, it will never be used ever.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Version stuff.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out writeMode, out Version);

            //Close file.
            FileReader.CloseFile(br);

            //Get byte order.
            br.Position = 4;
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            if (br.ReadUInt16() == ByteOrder.LittleEndian) {

                //Little endian.
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            }

            br.Position += 6;
            UInt32 length = br.ReadUInt32();
            br.Position -= 0x10;
            BackupSTP = br.ReadBytes((int)length);

        }

        /// <summary>
        /// Write the data.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {

            //In case STM is null.
            if (Stream == null) {
                bw.Write(BackupSTP);
                return;
            }

            //Set write mode.
            this.writeMode = writeMode;

            //Write the prefetch file.
            FileWriter FileWriter = new FileWriter();
            FileWriter.InitFile(bw, writeMode, "STP", 2, Version);

            //Open the info block.
            FileWriter.InitBlock(bw, ReferenceTypes.STRM_Block_Info, "INFO");

            //Write the stream file, to steal data from later.
            MemoryStream src = new MemoryStream();
            BinaryDataReader br = new BinaryDataReader(src);
            BinaryDataWriter bw2 = new BinaryDataWriter(src);
            br.ByteOrder = bw.ByteOrder;
            bw2.ByteOrder = bw.ByteOrder;
            Stream.Write(writeMode, bw2);

            //Get the info block to copy.
            br.Position = 0x18;
            Int32 infoOffset = br.ReadInt32();
            UInt32 infoSize = br.ReadUInt32();
            br.Position = infoOffset + 8;
            bw.Write(br.ReadBytes((int)infoSize - 8));

            //Highjack the data pointer in the info.
            br.Position = infoOffset + 0xC;
            Int32 dataOffset = br.ReadInt32();
            long trueDataOffset = dataOffset + infoOffset + 8 + 52;

            //Close the info block.
            FileWriter.CloseBlock(bw);

            //Open the prefetch data block.
            FileWriter.InitBlock(bw, ReferenceTypes.STRM_Block_PrefetchData, "PDAT");

            //Write the prefetch data table.
            bw.Write((UInt32)1);

            //Prefetch start frame.
            bw.Write((UInt32)0);

            //Write the prefetch size.
            bw.Write((UInt32)(Stream.Stm.info.channels.Count * 0xA000));

            //Write reserved.
            bw.Write((UInt32)0);

            //Write the prefetch reference.
            bw.Write((UInt32)0);
            bw.Write((UInt32)0x34);

            //Write padding.
            bw.Write(new byte[0x20]);

            //Go to offset in stream.
            br.Position = 0x30;
            Int32 someDataOffset = br.ReadInt32();
            br.Position = someDataOffset + Stream.Stm.info.streamSoundInfo.sampleDataOffset.offset + 8;

            //Take backup.
            long back = bw.Position;

            //Set data position.
            bw.Position = trueDataOffset;
            bw.Write((UInt32)back);
            bw.Position = back;

            //Add the b_stm data.
            bw.Write(br.ReadBytes(Stream.Stm.info.channels.Count * 0xA000));

            //Close prefetch data block.
            FileWriter.CloseBlock(bw);

            //Close the prefetch file.
            FileWriter.CloseFile(bw);

        }

        /// <summary>
        /// Write the data.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(writeMode, bw);
        }

    }

}
