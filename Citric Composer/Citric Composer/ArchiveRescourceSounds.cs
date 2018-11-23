using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    /// <summary>
    /// BARS fi1e.
    /// </summary>
    public class bars
    {

        /// <summary>
        /// 1 - File header.
        /// </summary>
        public Header fileHeader;

        /// <summary>
        /// 2 - Number of tracks.
        /// </summary>
        public UInt32 numTracks;

        /// <summary>
        /// 3 - Track info [numTracks].
        /// </summary>
        public List<UInt32> trackInfo;

        /// <summary>
        /// 4 - File references [numTracks].
        /// </summary>
        public FileReference[] fileReferences;

        /// <summary>
        /// 5 - AMTA chunks [numTracks].
        /// </summary>
        public List<AMTA> amtas;

        /// <summary>
        /// 6 - WAVs.
        /// </summary>
        public List<b_wav> wavs;


        /// <summary>
        /// Header.
        /// </summary>
        public class Header {

            /// <summary>
            /// 1 - BARS.
            /// </summary>
            public string magic;

            /// <summary>
            /// 2 - File size.
            /// </summary>
            public UInt32 fileSize;

            /// <summary>
            /// 3 - Byte order.
            /// </summary>
            public UInt16 byteOrder;

            /// <summary>
            /// 4 - Version.
            /// </summary>
            public UInt16 version;


            /// <summary>
            /// New header.
            /// </summary>
            public Header(UInt32 fileSize, UInt16 version)
            {

                magic = "BARS";
                this.fileSize = fileSize + 12;
                this.byteOrder = ByteOrder.BigEndian;
                this.version = version;

            }

            /// <summary>
            /// New header.
            /// </summary>
            /// <param name="br">The reader.</param>
            public Header(ref BinaryDataReader br) {

                br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                magic = new string(br.ReadChars(4));
                br.ReadUInt32();
                byteOrder = br.ReadUInt16();
                version = br.ReadUInt16();

                br.Position -= 8;
                byteOrder = br.ReadUInt16();
                if (byteOrder == ByteOrder.LittleEndian) {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }
                br.ReadUInt32();
                byteOrder = ByteOrder.BigEndian;

            }

            public void Write(ref BinaryDataWriter bw, UInt16 byteOrder) {

                if (byteOrder == ByteOrder.BigEndian) {
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else {
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }

                bw.Write(magic.ToCharArray());
                bw.Write(fileSize);
                bw.Write(byteOrder);
                bw.Write(version);

            }

        }


        /// <summary>
        /// File reference.
        /// </summary>
        public class FileReference {

            /// <summary>
            /// 1 - AMTA offset.
            /// </summary>
            public UInt32 AMTAOffset;
            
            /// <summary>
            /// 2 - WAV offset.
            /// </summary>
            public UInt32 WAVOffset;

        }


        /// <summary>
        /// AMTA chunk.
        /// </summary>
        public class AMTA {

            /// <summary>
            /// 1 - Magic.
            /// </summary>
            public string magic;

            /// <summary>
            /// 2 - Byte order.
            /// </summary>
            public UInt16 byteOrder;

            /// <summary>
            /// 3 - Version.
            /// </summary>
            public UInt16 version;

            /// <summary>
            /// 4 - Chunk size.
            /// </summary>
            public UInt32 chunkSize;

            /// <summary>
            /// 5 - Data offset.
            /// </summary>
            public UInt32 dataOffset;

            /// <summary>
            /// 6 - Mark offset.
            /// </summary>
            public UInt32 markOffset;

            /// <summary>
            /// 7 - Ext offet.
            /// </summary>
            public UInt32 extOffset;

            /// <summary>
            /// 8 - Strg offset.
            /// </summary>
            public UInt32 strgOffset;

            /// <summary>
            /// 9 - Data.
            /// </summary>
            public Data data;

            /// <summary>
            /// 10 - Mark.
            /// </summary>
            public Mark mark;

            /// <summary>
            /// 11 - Ext.
            /// </summary>
            public Ext ext;

            /// <summary>
            /// 12 - Strg.
            /// </summary>
            public Strg strg;


            /// <summary>
            /// Data chunk.
            /// </summary>
            public class Data {

                /// <summary>
                /// 1 - Magic.
                /// </summary>
                public string magic;

                /// <summary>
                /// 2 - Size of block after this.
                /// </summary>
                public UInt32 restOfSize;

                /// <summary>
                /// 3 - Unknown.
                /// </summary>
                public byte[] unknown;

            }


            /// <summary>
            /// Mark chunk.
            /// </summary>
            public class Mark
            {

                /// <summary>
                /// 1 - Magic.
                /// </summary>
                public string magic;

                /// <summary>
                /// 2 - Size of block after this.
                /// </summary>
                public UInt32 restOfSize;

                /// <summary>
                /// 3 - Unknown.
                /// </summary>
                public byte[] unknown;


            }


            /// <summary>
            /// Ext chunk.
            /// </summary>
            public class Ext
            {

                /// <summary>
                /// 1 - Magic.
                /// </summary>
                public string magic;

                /// <summary>
                /// 2 - Size of block after this.
                /// </summary>
                public UInt32 restOfSize;

                /// <summary>
                /// 3 - Unknown.
                /// </summary>
                public byte[] unknown;

            }


            /// <summary>
            /// Strg chunk.
            /// </summary>
            public class Strg
            {

                /// <summary>
                /// 1 - Magic.
                /// </summary>
                public string magic;

                /// <summary>
                /// 2 - Size of block after this.
                /// </summary>
                public UInt32 restOfSize;

                /// <summary>
                /// 3 - Null terminated track name.
                /// </summary>
                public string trackName;

            }

        }


        /// <summary>
        /// Load bars from bytes.
        /// </summary>
        /// <param name="src"></param>
        public void Load(byte[] b) {

            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);

            fileHeader = new Header(ref br);
            numTracks = br.ReadUInt32();
            trackInfo = new List<UInt32>();
            for (int i = 0; i < numTracks; i++) {
                trackInfo.Add(br.ReadUInt32());
            }
            fileReferences = new FileReference[numTracks];
            for (int i = 0; i < numTracks; i++)
            {
                fileReferences[i] = new FileReference() { AMTAOffset = br.ReadUInt32(), WAVOffset = br.ReadUInt32() };
            }

            amtas = new List<AMTA>();
            wavs = new List<b_wav>();

            for (int i = 0; i < numTracks; i++)
            {

                long basePos = br.Position;

                Syroot.BinaryData.ByteOrder b2 = br.ByteOrder;
                br.Position = fileReferences[i].AMTAOffset;
                AMTA a = new AMTA();
                a.magic = new string(br.ReadChars(4));
                a.byteOrder = br.ReadUInt16();

                if (a.byteOrder == ByteOrder.BigEndian)
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }

                a.byteOrder = ByteOrder.BigEndian;
                a.version = br.ReadUInt16();
                a.chunkSize = br.ReadUInt32();
                a.dataOffset = br.ReadUInt32();
                a.markOffset = br.ReadUInt32();
                a.extOffset = br.ReadUInt32();
                a.strgOffset = br.ReadUInt32();

                a.data = new AMTA.Data();
                a.mark = new AMTA.Mark();
                a.ext = new AMTA.Ext();
                a.strg = new AMTA.Strg();

                br.Position = basePos + a.dataOffset;
                a.data.magic = new string(br.ReadChars(4));
                a.data.restOfSize = br.ReadUInt32();
                a.data.unknown = br.ReadBytes((int)a.data.restOfSize);

                br.Position = basePos + a.markOffset;
                a.mark.magic = new string(br.ReadChars(4));
                a.mark.restOfSize = br.ReadUInt32();
                a.mark.unknown = br.ReadBytes((int)a.mark.restOfSize);

                br.Position = basePos + a.extOffset;
                a.ext.magic = new string(br.ReadChars(4));
                a.ext.restOfSize = br.ReadUInt32();
                a.ext.unknown = br.ReadBytes((int)a.ext.restOfSize);

                br.Position = basePos + a.strgOffset;
                a.strg.magic = new string(br.ReadChars(4));
                a.strg.restOfSize = br.ReadUInt32();
                a.strg.trackName = new string(br.ReadChars((int)a.strg.restOfSize - 1));

                br.ByteOrder = b2;
            }

            for (int i = 0; i < numTracks; i++)
            {
                br.Position = fileReferences[i].WAVOffset;
                br.ReadUInt32s(3);
                b_wav w = new b_wav();
                int length = (int)br.ReadUInt32();
                br.Position -= 12;
                w.Load(br.ReadBytes(length));
                wavs.Add(w);
            }

        }

    }


}
