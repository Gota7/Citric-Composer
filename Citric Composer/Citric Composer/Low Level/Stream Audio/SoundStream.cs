using IsabelleLib;
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
    /// Stream file.
    /// </summary>
    public class b_stm
    {

        //Version constants.
        public static readonly UInt32 noRegionInfo = 2;
		public static readonly UInt32 regionInfo = 3;
		public static readonly UInt32 originalLoopInfo = 4;
		public static readonly UInt32 secretInfo = 6;

        /// <summary>
        /// 1 - File header.
        /// </summary>
        public FileHeader fileHeader;

        /// <summary>
        /// 2 - Info block.
        /// </summary>
        public InfoBlock info;

        /// <summary>
        /// 3 - Seek block.
        /// </summary>
        public SoundNStreamSeekBlock seek;

        /// <summary>
        /// 4 - Data block.
        /// </summary>
        public SoundNStreamDataBlock data;

        /// <summary>
        /// 5 - Region block.
        /// </summary>
        public SoundNStreamRegionBlock region;


        /// <summary>
        /// Info block.
        /// </summary>
        public class InfoBlock
        {

            /// <summary>
            /// Null loop constant.
            /// </summary>
            public const UInt32 NULL_LOOP = 0;

            /// <summary>
            /// 1 - INFO.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Block size.
            /// </summary>
            public UInt32 blockSize;

            /// <summary>
            /// 3 - Reference to stream sound info.
            /// </summary>
            public Reference streamSoundInfoRef;

            /// <summary>
            /// 4 - Reference to the track reference table.
            /// </summary>
            public Reference trackInfoTableRef;

            /// <summary>
            /// 5 - Reference to the channel reference table.
            /// </summary>
            public Reference channelInfoTableRef;

            /// <summary>
            /// 6 - Stream sound info.
            /// </summary>
            public StreamSoundInfo streamSoundInfo;

            /// <summary>
            /// 7 - Track reference table.
            /// </summary>
            public ReferenceTable trackInfoRefTable;

            /// <summary>
            /// 8 - Channel info reference table.
            /// </summary>
            public ReferenceTable channelInfoRefTable;

            /// <summary>
            /// 9 - Tracks.
            /// </summary>
            public List<TrackInfo> tracks;

            /// <summary>
            /// 10 - Channels.
            /// </summary>
            public List<ChannelInfo> channels;

        }


        /// <summary>
        /// Stream sound info.
        /// </summary>
        public class StreamSoundInfo
        {

            /// <summary>
            /// 1 - Encoding constant here.
            /// </summary>
            public byte encoding;

            /// <summary>
            /// 2 - If stream loops.
            /// </summary>
            public bool isLoop;

            /// <summary>
            /// 3 - Channel count.
            /// </summary>
            public byte channelCount;

            /// <summary>
            /// 4 - Region count.
            /// </summary>
            public byte regionCount;

            /// <summary>
            /// 5 - Sampling rate.
            /// </summary>
            public UInt32 sampleRate;

            /// <summary>
            /// 6 - Loop start.
            /// </summary>
            public UInt32 loopStart;

            /// <summary>
            /// 7 - Number of samples.
            /// </summary>
            public UInt32 sampleCount;

            /// <summary>
            /// 8 - Number of blocks.
            /// </summary>
            public UInt32 blockCount;

            /// <summary>
            /// 9 - Size of one block in bytes.
            /// </summary>
            public UInt32 oneBlockBytesize;

            /// <summary>
            /// 10 - Size of one block in samples.
            /// </summary>
            public UInt32 oneBlockSamples;

            /// <summary>
            /// 11 - Size of the last block in bytes without padding.
            /// </summary>
            public UInt32 lastBlockBytesize;

            /// <summary>
            /// 12 - Size of the last block in samples.
            /// </summary>
            public UInt32 lastBlockSamples;

            /// <summary>
            /// 13 - Size of the last block in bytes including padding.
            /// </summary>
            public UInt32 lastBlockPaddedBytesize;

            /// <summary>
            /// 14 - The size of 1 channel's worth of seek info.
            /// </summary>
            public UInt32 sizeOfSeekInfo;

            /// <summary>
            /// 15 - How many samples a seek interval represents?
            /// </summary>
            public UInt32 seekInfoIntervalSamples;

            /// <summary>
            /// 16 - Where the sample data begins in the data block.
            /// </summary>
            public Reference sampleDataOffset;

            /// <summary>
            /// 17 - Size of each region info in bytes - this and rest only present if V2.1 at least?
            /// </summary>
            public UInt16 regionInfoBytesize;

            /// <summary>
            /// 18 - Padding.
            /// </summary>
            public UInt16 padding;

            /// <summary>
            /// 19 - Where the region data begins in the region block.
            /// </summary>
            public Reference regionDataOffset;

            /// <summary>
            /// 20 - Original loop start - this and rest only present if V4 at least?
            /// </summary>
            public UInt32 originalLoopStart;

            /// <summary>
            /// 21 - Original loop end.
            /// </summary>
            public UInt32 originalLoopEnd;

            /// <summary>
            /// 22 - Secret info.
            /// </summary>
            public UInt32 secretInfo;

        }


        /// <summary>
        /// Track info.
        /// </summary>
        public class TrackInfo
        {

            /// <summary>
            /// 1 - Volume.
            /// </summary>
            public byte volume;

            /// <summary>
            /// 2 - Pan.
            /// </summary>
            public byte pan;

            /// <summary>
            /// 3 - Span.
            /// </summary>
            public byte span;

            /// <summary>
            /// 4 - Surround mode.
            /// </summary>
            public byte surroundMode;

            /// <summary>
            /// 5 - Reference to the global channel index table.
            /// </summary>
            public Reference globalChannelIndexTableRef;

            /// <summary>
            /// 6 - Global channel index table.
            /// </summary>
            public Table<byte> globalChannelIndexTable;

        }


        /// <summary>
        /// Channel info.
        /// </summary>
        public class ChannelInfo
        {

            /// <summary>
            /// 1 - Reference to dspAdpcm info.
            /// </summary>
            public Reference dspAdpcmInfoRef;

            /// <summary>
            /// 2 - DspAdpcm info.
            /// </summary>
            public DspAdpcmInfo dspAdpcmInfo;

        }


        /// <summary>
        /// Load a file.
        /// </summary>
        /// <param name="b">The file.</param>
        public void Load(byte[] b)
        {

            //Set up the reader.
            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);

            //Read the header.
            fileHeader = new FileHeader(ref br);

            //Read blocks.
            foreach (SizedReference s in fileHeader.blockOffsets)
            {

                if (s.offset != Reference.NULL_PTR)
                {
                    br.Position = s.offset;
                    switch (s.typeId)
                    {

                        //Info block.
                        case ReferenceTypes.STRM_Block_Info:
                            long basePos = br.Position + 8;
                            info = new InfoBlock()
                            {
                                magic = br.ReadChars(4),
                                blockSize = br.ReadUInt32(),
                                streamSoundInfoRef = new Reference(ref br),
                                trackInfoTableRef = new Reference(ref br),
                                channelInfoTableRef = new Reference(ref br),
                                streamSoundInfo = null,
                                trackInfoRefTable = null,
                                channelInfoRefTable = null,
                                tracks = null,
                                channels = null
                            };

                            //Stream sound info.
                            if (info.streamSoundInfoRef.typeId == ReferenceTypes.STRM_Info_StreamSound && info.streamSoundInfoRef.offset != Reference.NULL_PTR)
                            {

                                br.Position = basePos + info.streamSoundInfoRef.offset;
                                info.streamSoundInfo = new StreamSoundInfo()
                                {
                                    encoding = br.ReadByte(),
                                    isLoop = br.ReadBoolean(),
                                    channelCount = br.ReadByte(),
                                    regionCount = br.ReadByte(),
                                    sampleRate = br.ReadUInt32(),
                                    loopStart = br.ReadUInt32(),
                                    sampleCount = br.ReadUInt32(),
                                    blockCount = br.ReadUInt32(),
                                    oneBlockBytesize = br.ReadUInt32(),
                                    oneBlockSamples = br.ReadUInt32(),
                                    lastBlockBytesize = br.ReadUInt32(),
                                    lastBlockSamples = br.ReadUInt32(),
                                    lastBlockPaddedBytesize = br.ReadUInt32(),
                                    sizeOfSeekInfo = br.ReadUInt32(),
                                    seekInfoIntervalSamples = br.ReadUInt32(),
                                    sampleDataOffset = new Reference(ref br),
                                    regionInfoBytesize = 0x100,
                                    padding = 0,
                                    regionDataOffset = new Reference(0, 0x18),
                                    originalLoopStart = 0,
                                    originalLoopEnd = 0,
                                    secretInfo = 0
                                };

                                if (fileHeader.vMajor >= regionInfo)
                                {
                                    info.streamSoundInfo.regionInfoBytesize = br.ReadUInt16();
                                    info.streamSoundInfo.padding = br.ReadUInt16();
                                    info.streamSoundInfo.regionDataOffset = new Reference(ref br);
                                }

                                if (fileHeader.vMajor >= originalLoopInfo)
                                {
                                    info.streamSoundInfo.originalLoopStart = br.ReadUInt32();
                                    info.streamSoundInfo.originalLoopEnd = br.ReadUInt32();
                                }

                                if (fileHeader.vMajor >= secretInfo)
                                {
                                    info.streamSoundInfo.secretInfo = br.ReadUInt32();
                                }

                            }

                            //Track info.
                            if (info.trackInfoTableRef.typeId == ReferenceTypes.Tables + 1 && info.trackInfoTableRef.offset != Reference.NULL_PTR)
                            {

                                br.Position = basePos + info.trackInfoTableRef.offset;
                                long newPos = br.Position;
                                info.trackInfoRefTable = new ReferenceTable(ref br);

                                //Get tracks.
                                info.tracks = new List<TrackInfo>();
                                foreach (Reference r in info.trackInfoRefTable.references)
                                {

                                    TrackInfo t = null;
                                    if (r.typeId == ReferenceTypes.STRM_Info_Track && r.offset != Reference.NULL_PTR)
                                    {

                                        br.Position = newPos + r.offset;
                                        t = new TrackInfo()
                                        {
                                            volume = br.ReadByte(),
                                            pan = br.ReadByte(),
                                            span = br.ReadByte(),
                                            surroundMode = br.ReadByte(),
                                            globalChannelIndexTableRef = new Reference(ref br),
                                            globalChannelIndexTable = null
                                        };

                                        if (t.globalChannelIndexTableRef.offset != Reference.NULL_PTR)
                                        {

                                            br.Position = newPos + r.offset + t.globalChannelIndexTableRef.offset;
                                            t.globalChannelIndexTable = new Table<byte>();
                                            t.globalChannelIndexTable.count = br.ReadUInt32();
                                            t.globalChannelIndexTable.entries = new List<byte>();
                                            for (int i = 0; i < t.globalChannelIndexTable.count; i++)
                                            {
                                                t.globalChannelIndexTable.entries.Add(br.ReadByte());
                                            }

                                        }

                                    }
                                    info.tracks.Add(t);

                                }

                            }

                            //Channel info.
                            if (info.channelInfoTableRef.typeId == ReferenceTypes.Tables + 1 && info.channelInfoTableRef.offset != Reference.NULL_PTR)
                            {

                                br.Position = basePos + info.channelInfoTableRef.offset;
                                long newPos = br.Position;
                                info.channelInfoRefTable = new ReferenceTable(ref br);

                                //Get channels.
                                info.channels = new List<ChannelInfo>();
                                foreach (Reference r in info.channelInfoRefTable.references)
                                {

                                    ChannelInfo c = null;
                                    if (r.offset != Reference.NULL_PTR)
                                    {

                                        br.Position = newPos + r.offset;
                                        c = new ChannelInfo()
                                        {
                                            dspAdpcmInfoRef = new Reference(ref br),
                                            dspAdpcmInfo = null
                                        };

                                        if (c.dspAdpcmInfoRef.offset != Reference.NULL_PTR)
                                        {
                                            br.Position = newPos + r.offset + c.dspAdpcmInfoRef.offset;
                                            c.dspAdpcmInfo = new DspAdpcmInfo();
                                            c.dspAdpcmInfo = new DspAdpcmInfo() { coefs = new short[8][] };
                                            c.dspAdpcmInfo.coefs[0] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[1] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[2] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[3] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[4] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[5] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[6] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.coefs[7] = br.ReadInt16s(2);
                                            c.dspAdpcmInfo.pred_scale = br.ReadUInt16();
                                            c.dspAdpcmInfo.yn1 = br.ReadInt16();
                                            c.dspAdpcmInfo.yn2 = br.ReadInt16();
                                            c.dspAdpcmInfo.loop_pred_scale = br.ReadUInt16();
                                            c.dspAdpcmInfo.loop_yn1 = br.ReadInt16();
                                            c.dspAdpcmInfo.loop_yn2 = br.ReadInt16();
                                        }

                                    }
                                    info.channels.Add(c);

                                }

                            }

                            break;

                        //Seek block.
                        case ReferenceTypes.STRM_Block_Seek:
                            seek = new SoundNStreamSeekBlock(ref br, info.streamSoundInfo, fileHeader);
                            break;

                        //Region block.
                        case ReferenceTypes.STRM_Block_Region:
                            region = new SoundNStreamRegionBlock(ref br, info.streamSoundInfo);
                            break;

                        //Data block.
                        case ReferenceTypes.STRM_Block_Data:
                            data = new SoundNStreamDataBlock(ref br, info);
                            break;

                    }
                }

            }

        }

        /// <summary>
        /// Update the file.
        /// </summary>
        public void Update(UInt16 endian) {

            //Update info.
            info.magic = "INFO".ToCharArray();
            info.streamSoundInfoRef = new Reference(ReferenceTypes.STRM_Info_StreamSound, 0x18);

            //Stream sound info.
            UInt32 infoBaseOffet = 0x18;

            info.streamSoundInfo.channelCount = (byte)info.channels.Count;
            info.streamSoundInfo.regionCount = (byte)(region != null ? region.regions.Length : 0);
            info.streamSoundInfo.regionDataOffset = new Reference(0, 0x18);
            info.streamSoundInfo.regionInfoBytesize = 0x100;
            info.streamSoundInfo.sampleDataOffset = new Reference(ReferenceTypes.General_ByteStream, 0x18);
            info.streamSoundInfo.padding = 0;
            info.streamSoundInfo.sizeOfSeekInfo = 4;

            infoBaseOffet += 56;
            if (fileHeader.vMajor >= regionInfo) infoBaseOffet += 12;
            if (fileHeader.vMajor >= originalLoopInfo) infoBaseOffet += 8;
            if (fileHeader.vMajor >= secretInfo) infoBaseOffet += 4;

            //Reference table for track info ref.
            info.trackInfoTableRef = new Reference(0x0101, (int)infoBaseOffet);    
            
            //Skip ref table coords.
            infoBaseOffet += (uint)(info.tracks != null ? (info.tracks.Count * 8 + 4) : 0);

            info.channelInfoTableRef = new Reference(0x0101, (int)infoBaseOffet);

            List<Reference> trackRefs = new List<Reference>();

            //Track info.
            UInt32 trackBaseOffset = (uint)((info.tracks != null ? (info.tracks.Count * 8 + 4) : 0) + (info.channels.Count * 8 + 4));
            if (info.tracks != null)
            {
                for (int i = 0; i < info.tracks.Count; i++)
                {

                    trackRefs.Add(new Reference(ReferenceTypes.STRM_Info_Track, (int)trackBaseOffset));

                    infoBaseOffet += 12;
                    trackBaseOffset += 12;
                    if (info.tracks[i].globalChannelIndexTable != null)
                    {
                        info.tracks[i].globalChannelIndexTableRef = new Reference(0x100, 0xC);
                        info.tracks[i].globalChannelIndexTable.count = (uint)info.tracks[i].globalChannelIndexTable.entries.Count;
                        infoBaseOffet += 4 + info.tracks[i].globalChannelIndexTable.count;
                        trackBaseOffset += 4 + info.tracks[i].globalChannelIndexTable.count;
                        while (infoBaseOffet % 4 != 0)
                        {
                            infoBaseOffet++;
                            trackBaseOffset++;
                        }
                    }
                    else
                    {
                        info.tracks[i].globalChannelIndexTableRef = new Reference(0, Reference.NULL_PTR);
                    }

                }
                info.trackInfoRefTable = new ReferenceTable(trackRefs);
            }
            else {
                info.trackInfoTableRef = new Reference(0, Reference.NULL_PTR);
                info.trackInfoRefTable = null;
            }

            //Channel info.
            UInt32 channelBaseOffset = (uint)(4 + 8 * info.channels.Count() + trackBaseOffset - ((uint)((info.tracks != null ? (info.tracks.Count * 8 + 4) : 0) + (info.channels.Count * 8 + 4))));
            infoBaseOffet += (uint)(4 + 8 * info.channels.Count());
            List<Reference> channelRefs = new List<Reference>();

            for (int i = 0; i < info.channels.Count; i++) {

                channelRefs.Add(new Reference(ReferenceTypes.STRM_Info_Channel, (int)channelBaseOffset));
                channelBaseOffset += 8;
                infoBaseOffet += 8;

            }

            info.channelInfoRefTable = new ReferenceTable(channelRefs);

            channelBaseOffset = (uint)(8 * info.channels.Count());
            for (int i = 0; i < info.channels.Count; i++) {

                info.channels[i].dspAdpcmInfoRef = new Reference(0, Reference.NULL_PTR);
                if (info.channels[i].dspAdpcmInfo != null) {
                    info.channels[i].dspAdpcmInfoRef = new Reference(0x300, (int)channelBaseOffset);
                    channelBaseOffset += 0x2E;
                    infoBaseOffet += 0x2E;
                }
                channelBaseOffset -= 8;

            }

            //Make offsets, depeding on what blocks exist, and on the sizes of the blocks.
            List<SizedReference> blocks = new List<SizedReference>();

            //Can't forget about magic and size.
            infoBaseOffet += 8;

            while (infoBaseOffet % 0x20 != 0) {
                infoBaseOffet++;
            }
            info.blockSize = infoBaseOffet;

            blocks.Add(new SizedReference(ReferenceTypes.STRM_Block_Info, 0, info.blockSize));
            if (seek != null) blocks.Add(new SizedReference(ReferenceTypes.STRM_Block_Seek, (int)info.blockSize, seek.GetSize()));
            if (region != null) blocks.Add(new SizedReference(ReferenceTypes.STRM_Block_Region, (int)info.blockSize + (seek != null ? (int)seek.GetSize() : 0), region.GetSize()));
            blocks.Add(new SizedReference(ReferenceTypes.STRM_Block_Data, (int)info.blockSize + (seek != null ? (int)seek.GetSize() : 0) + (region != null ? (int)region.GetSize() : 0), data.GetSize(info.streamSoundInfo.encoding, ref info)));
            
            fileHeader = new FileHeader(endian == ByteOrder.LittleEndian ? "CSTM" : "FSTM", endian, fileHeader.vMajor, fileHeader.vMinor, fileHeader.vRevision, (uint)(info.blockSize + (seek != null ? (int)seek.GetSize() : 0) + (region != null ? (int)region.GetSize() : 0) + data.GetSize(info.streamSoundInfo.encoding, ref info)), blocks);

            if (region == null) {
                info.streamSoundInfo.regionDataOffset = new Reference(0, Reference.NULL_PTR);
            }

            //Update stuff.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            if (seek != null) seek.Write(ref bw, fileHeader);
            if (region != null) region.Write(ref bw);
            data.WriteSTM(ref bw, info, info.streamSoundInfo.sampleCount);

        }

        /// <summary>
        /// Convert file to bytes.
        /// </summary>
        /// <param name="endian"></param>
        /// <returns></returns>
        public byte[] ToBytes(UInt16 endian, bool forceFstm = false) {

            //Update file.
            Update(endian);
            if (forceFstm) { fileHeader.magic = "FSTM".ToCharArray(); }

            //Writer.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);

            //Write header.
            fileHeader.Write(ref bw);

            //Info block.
            bw.Write(info.magic);
            bw.Write(info.blockSize);
            info.streamSoundInfoRef.Write(ref bw);
            info.trackInfoTableRef.Write(ref bw);
            info.channelInfoTableRef.Write(ref bw);

            //Stream sound info.
            bw.Write(info.streamSoundInfo.encoding);
            bw.Write(info.streamSoundInfo.isLoop);
            bw.Write(info.streamSoundInfo.channelCount);
            bw.Write(info.streamSoundInfo.regionCount);
            bw.Write(info.streamSoundInfo.sampleRate);
            bw.Write(info.streamSoundInfo.loopStart);
            bw.Write(info.streamSoundInfo.sampleCount);
            bw.Write(info.streamSoundInfo.blockCount);
            bw.Write(info.streamSoundInfo.oneBlockBytesize);
            bw.Write(info.streamSoundInfo.oneBlockSamples);
            bw.Write(info.streamSoundInfo.lastBlockBytesize);
            bw.Write(info.streamSoundInfo.lastBlockSamples);
            bw.Write(info.streamSoundInfo.lastBlockPaddedBytesize);
            bw.Write(info.streamSoundInfo.sizeOfSeekInfo);
            bw.Write(info.streamSoundInfo.seekInfoIntervalSamples);
            info.streamSoundInfo.sampleDataOffset.Write(ref bw);

            if (fileHeader.vMajor >= regionInfo)
            {
                bw.Write(info.streamSoundInfo.regionInfoBytesize);
                bw.Write(info.streamSoundInfo.padding);
                info.streamSoundInfo.regionDataOffset.Write(ref bw);
            }

            if (fileHeader.vMajor >= originalLoopInfo)
            {
                bw.Write(info.streamSoundInfo.originalLoopStart);
                bw.Write(info.streamSoundInfo.originalLoopEnd);
            }

            if (fileHeader.vMajor >= secretInfo)
            {
                bw.Write(info.streamSoundInfo.secretInfo);
            }

            //Write tables.
            if (info.trackInfoRefTable != null) info.trackInfoRefTable.Write(ref bw);
            if (info.channelInfoRefTable != null) info.channelInfoRefTable.Write(ref bw);

            //Write tracks.
            if (info.tracks != null)
            {
                foreach (TrackInfo t in info.tracks)
                {

                    bw.Write(t.volume);
                    bw.Write(t.pan);
                    bw.Write(t.span);
                    bw.Write(t.surroundMode);
                    t.globalChannelIndexTableRef.Write(ref bw);
                    if (t.globalChannelIndexTable != null)
                    {
                        bw.Write(t.globalChannelIndexTable.count);
                        foreach (byte b in t.globalChannelIndexTable.entries)
                        {
                            bw.Write(b);
                        }
                        while (bw.Position % 4 != 0)
                        {
                            bw.Write((byte)0);
                        }
                    }

                }
            }

            //Write channels.
            if (info.channels != null) {
                foreach (ChannelInfo c in info.channels) {
                    c.dspAdpcmInfoRef.Write(ref bw);
                }

                foreach (ChannelInfo c in info.channels) {
                    if (c.dspAdpcmInfo != null)
                    {
                        bw.Write(c.dspAdpcmInfo.coefs[0]);
                        bw.Write(c.dspAdpcmInfo.coefs[1]);
                        bw.Write(c.dspAdpcmInfo.coefs[2]);
                        bw.Write(c.dspAdpcmInfo.coefs[3]);
                        bw.Write(c.dspAdpcmInfo.coefs[4]);
                        bw.Write(c.dspAdpcmInfo.coefs[5]);
                        bw.Write(c.dspAdpcmInfo.coefs[6]);
                        bw.Write(c.dspAdpcmInfo.coefs[7]);
                        bw.Write(c.dspAdpcmInfo.pred_scale);
                        bw.Write(c.dspAdpcmInfo.yn1);
                        bw.Write(c.dspAdpcmInfo.yn2);
                        bw.Write(c.dspAdpcmInfo.loop_pred_scale);
                        bw.Write(c.dspAdpcmInfo.loop_yn1);
                        bw.Write(c.dspAdpcmInfo.loop_yn2);
                        bw.Write((UInt16)0);
                    }
                }
            }

            //Write padding.
            while (bw.Position % 0x20 != 0) {
                bw.Write((byte)0);
            }

            //Seek block (if needed).
            if (info.streamSoundInfo.encoding >= EncodingTypes.DSP_ADPCM)
            {
                seek.Write(ref bw, fileHeader);
            }

            //Region block (if needed).
            if (region != null)
            {
                region.Write(ref bw);
            }

            //Data block.
            data.WriteSTM(ref bw, info, info.streamSoundInfo.sampleCount);

            //Return finished file.
            return o.ToArray();

        }

    }


    /// <summary>
    /// Stream factory.
    /// </summary>
    public static class StreamFactory {

        /// <summary>
        /// Create a standard b_stm.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="numSamples">Number of samples.</param>
        /// <param name="samples">Pcm8[][] or Pcm16[][] audio samples.</param>
        /// <param name="encoding">If samples is Pcm8[][] always 0. Must be 1 or 2 for if samples is Pcm16[][].</param>
        /// <param name="version">The version of the file.</param>
        /// <returns></returns>
        public static b_stm CreateStream(UInt32 sampleRate, UInt32 numSamples, object samples, byte encoding, byte vMajor, byte vMinor, byte vRevision)
        {

            b_stm s = new b_stm();
            s.fileHeader = new FileHeader("FSTM", ByteOrder.BigEndian, vMajor, vMinor, vRevision, 0, new List<SizedReference>());

            s.info = new b_stm.InfoBlock();
            s.info.streamSoundInfo = new b_stm.StreamSoundInfo();
            s.info.tracks = new List<b_stm.TrackInfo>();
            s.info.channels = new List<b_stm.ChannelInfo>();

            //Stream info.
            s.info.streamSoundInfo = new b_stm.StreamSoundInfo();
            s.info.streamSoundInfo.encoding = encoding;
            s.info.streamSoundInfo.sampleCount = numSamples;
            s.info.streamSoundInfo.sampleRate = sampleRate;

            //Channels.
            switch (encoding)
            {

                case EncodingTypes.PCM8:
                    s.data = new SoundNStreamDataBlock(EncoderFactory.Pcm8ToSignedPcm8(samples as byte[][]), encoding);
                    for (int i = 0; i < (samples as byte[][]).Length; i++)
                    {
                        s.info.channels.Add(new b_stm.ChannelInfo());
                    }
                    break;

                case EncodingTypes.PCM16:
                    s.data = new SoundNStreamDataBlock(samples, encoding);
                    for (int i = 0; i < (samples as Int16[][]).Length; i++)
                    {
                        s.info.channels.Add(new b_stm.ChannelInfo());
                    }
                    break;

                case EncodingTypes.DSP_ADPCM:
                    s.data = new SoundNStreamDataBlock(EncoderFactory.Pcm16ToDspAdpcmSTM(samples as Int16[][], s), encoding);
                    s.Update(ByteOrder.BigEndian);

					//Get DSP-ADPCM info.
					DspAdpcmInfo[] context = new DspAdpcmInfo[s.data.dspAdpcm.Length];
					int cCount = 0;
					foreach (var channel in s.info.channels) {
						context[cCount] = channel.dspAdpcmInfo;
						cCount++;
					}

					//Create SEEK block.
					s.seek = new SoundNStreamSeekBlock(s.data.dspAdpcm, s.info.streamSoundInfo.sampleCount, context);

                    break;

            }

            //Tracks.
            for (int i = 0; i < s.info.channels.Count; i += 2) {

                s.info.tracks.Add(new b_stm.TrackInfo() { volume = 0x64, pan = 0x40, span = 0x0, surroundMode = 0, globalChannelIndexTable = new Table<byte>() { entries = new List<byte>() { (byte)i } } });
                if (i + 1 != s.info.channels.Count) { s.info.tracks[s.info.tracks.Count - 1].globalChannelIndexTable.entries.Add((byte)(i + 1)); }

            }

            s.Update(ByteOrder.BigEndian);

            return s;

        }

        /// <summary>
        /// Create a looping b_stm.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="numSamples">Number of samples.</param>
        /// <param name="samples">Pcm8[][] or Pcm16[][] audio samples.</param>
        /// <param name="encoding">If samples is Pcm8[][] always 0. Must be 1 or 2 for if samples is Pcm16[][].</param>
        /// <param name="version">The version of the file.</param>
        /// <param name="loopStart">Loop starting point.</param>
        /// <returns></returns>
        public static b_stm CreateStream(UInt32 sampleRate, UInt32 numSamples, object samples, byte encoding, byte vMajor, byte vMinor, byte vRevision, UInt32 loopStart)
        {

            b_stm s = CreateStream(sampleRate, numSamples, samples, encoding, vMajor, vMinor, vRevision);
            s.info.streamSoundInfo.loopStart = loopStart;
            s.info.streamSoundInfo.isLoop = true;
            return s;

        }

        /// <summary>
        /// Create a new b_stm from a RIFF Wave.
        /// </summary>
        /// <param name="r">The RIFF Wave.</param>
        /// <param name="encode">Whether or not to encode PCM16 data.</param>
        /// <param name="version">Version of the file.</param>
        /// <returns></returns>
        public static b_stm CreateStream(RiffWave r, bool encode, byte vMajor, byte vMinor, byte vRevision)
        {

            b_stm s = new b_stm();
            bool loops = false;
            if (r.smpl != null) { loops = true; if (r.smpl.numLoops < 1) { loops = false; } }
            if (r.fmt.bitsPerSample == RiffWave.FmtChunk.BitsPerSample.PCM8)
            {
                List<byte[]> pcm8 = new List<byte[]>();
                int count = 0;
                UInt32 endSample = (UInt32)r.data.channels[0].pcm8.Count();
                if (loops) { endSample = r.smpl.loops[0].endSample; }
                foreach (RiffWave.DataChunk.DataSamples d in r.data.channels)
                {
                    if (count <= endSample) { pcm8.Add(d.pcm8.ToArray()); }
                    count += 1;
                }
                if (!loops)
                {
                    s = CreateStream(r.fmt.sampleRate, endSample, pcm8.ToArray(), EncodingTypes.PCM8, vMajor, vMinor, vRevision);
                }
                else
                {
                    s = CreateStream(r.fmt.sampleRate, endSample, pcm8.ToArray(), EncodingTypes.PCM8, vMajor, vMinor, vRevision, r.smpl.loops[0].startSample);
                }
            }
            else
            {
                List<Int16[]> pcm16 = new List<Int16[]>();
                int count2 = 0;
                UInt32 endSample2 = (UInt32)r.data.channels[0].pcm16.Count();
                if (loops) { endSample2 = r.smpl.loops[0].endSample; }
                foreach (RiffWave.DataChunk.DataSamples d in r.data.channels)
                {
                    if (count2 <= endSample2) { pcm16.Add(d.pcm16.ToArray()); }
                    count2 += 1;
                }
                byte encoding = EncodingTypes.PCM16; ;
                if (encode) { encoding = EncodingTypes.DSP_ADPCM; }
                if (!loops)
                {
                    s = CreateStream(r.fmt.sampleRate, (UInt32)r.data.channels[0].pcm16.Count(), pcm16.ToArray(), encoding, vMajor, vMinor, vRevision);
                }
                else
                {
                    s = CreateStream(r.fmt.sampleRate, (UInt32)r.data.channels[0].pcm16.Count(), pcm16.ToArray(), encoding, vMajor, vMinor, vRevision, r.smpl.loops[0].startSample);
                }
            }

            return s;

        }

        /// <summary>
        /// Create a stream from wave.
        /// </summary>
        /// <param name="b">The b_wav.</param>
        /// <param name="version">Version of the output stream.</param>
        /// <returns></returns>
        public static b_stm CreateStream(b_wav b, byte vMajor, byte vMinor, byte vRevision)
        {

            //Simple hack, but doesn't hurt optimization since SEEK must be recreated anyway.
            return CreateStream(RiffWaveFactory.CreateRiffWave(b), b.info.encoding >= EncodingTypes.DSP_ADPCM, vMajor, vMinor, vRevision);

        }

        /// <summary>
        /// Create a stream from a binary wave.
        /// </summary>
        /// <param name="b">The binary wave.</param>
        /// <param name="version">Version of the output stream.</param>
        /// <returns></returns>
        public static b_stm CreateStream(BinaryWave b, byte vMajor, byte vMinor, byte vRevision) {

            //Simple hack, but doesn't hurt optimization since SEEK must be recreated anyway.
            return CreateStream(RiffWaveFactory.CreateRiffWave(b), true, vMajor, vMinor, vRevision);

        }

        /// <summary>
        /// Create a wave from a FISP.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static b_stm CreateStream(FISP f, byte vMajor, byte vMinor, byte vRevision)
        {

            //New stream.
            b_stm s = new b_stm();

            //Trim the fat from each loop.
            object channels = new short[f.data.data.Count()][];
            for (int i = 0; i < (channels as short[][]).Length; i++)
            {

                List<short> l = new List<short>();

                for (int j = 0; j < f.stream.loopEnd; j++)
                {
                    l.Add(f.data.data[i][j]);
                }

                (channels as short[][])[i] = l.ToArray();
            }

            //PCM8 conversion.
            if (f.stream.encoding == EncodingTypes.PCM8)
            {
                channels = EncoderFactory.Pcm16ToPcm8(channels as short[][]);
            }

            //If looped.
            if (f.stream.isLoop)
            {
                s = StreamFactory.CreateStream(f.stream.sampleRate, f.stream.loopEnd, channels, f.stream.encoding, vMajor, vMinor, vRevision, f.stream.loopStart);
            }

            //Not looped.
            else
            {
                s = StreamFactory.CreateStream(f.stream.sampleRate, f.stream.loopEnd, channels, f.stream.encoding, vMajor, vMinor, vRevision);
            }

            //Make tracks.
            s.info.tracks = new List<b_stm.TrackInfo>();
            foreach (FISP.TrackInfo i in f.tracks) {

                b_stm.TrackInfo t = new b_stm.TrackInfo();
                t.globalChannelIndexTable = new Table<byte>();
                t.globalChannelIndexTable.count = (uint)i.channels.Count();
                t.globalChannelIndexTable.entries = i.channels;
                t.pan = i.pan;
                t.span = i.span;
                t.surroundMode = i.surroundMode;
                t.volume = i.volume;
                s.info.tracks.Add(t);

            }

            //Nullify.
            if (f.tracks.Count() <= 0) {
                s.info.tracks = null;
            }

            //Make regions. EXPERIMENTAL! Yell at me if this doesn't work.
            s.region = null;
            if (f.regions.Count > 0) {

                s.region = new SoundNStreamRegionBlock();
                s.region.regions = new SoundNStreamRegionBlock.RegionInfo[f.regions.Count];
                int index = 0;
                foreach (FISP.RegionInfo i in f.regions) {

                    SoundNStreamRegionBlock.RegionInfo r = new SoundNStreamRegionBlock.RegionInfo();
                    r.start = i.start;
                    r.end = i.end;
                    r.loopInfo = new SoundNStreamRegionBlock.RegionInfo.DspAdpcmLoopInfo[s.info.channels.Count];
                    if (f.stream.encoding >= EncodingTypes.DSP_ADPCM) {

                        for (int j = 0; j < s.info.channels.Count; j++) {

                            short h1 = 0;
                            short h2 = 0;
                            if (r.start >= 1) {
                                h1 = f.data.data[j][r.start - 1];
                            }
                            if (r.start >= 2) {
                                h2 = f.data.data[j][r.start - 2];
                            }

                            r.loopInfo[j] = new SoundNStreamRegionBlock.RegionInfo.DspAdpcmLoopInfo()
                            {
                                loopPredScale = s.info.channels[j].dspAdpcmInfo.loop_pred_scale,
                                loopYn1 = h1,
                                loopYn2 = h2
                            };

                        }

                    }
                    s.region.regions[index] = r;

                    index++;

                }

            }

            //Set info.
            s.info.streamSoundInfo.originalLoopStart = f.stream.originalLoopStart;
            s.info.streamSoundInfo.originalLoopEnd = f.stream.originalLoopEnd;
            s.info.streamSoundInfo.secretInfo = f.stream.secretInfo;

            return s;

        }

    }

}
