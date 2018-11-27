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
    /// Wave file.
    /// </summary>
    public class b_wav
    {

        /// <summary>
        /// 1 - File header.
        /// </summary>
        public FileHeader fileHeader;

        /// <summary>
        /// 2 - Info block.
        /// </summary>
        public InfoBlock info;

        /// <summary>
        /// 3 - Data block.
        /// </summary>
        public SoundNStreamDataBlock data;


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
            /// 3 - Encoding.
            /// </summary>
            public byte encoding;

            /// <summary>
            /// 4 - If loops.
            /// </summary>
            public bool isLoop;

            /// <summary>
            /// 5 - Padding.
            /// </summary>
            public UInt16 padding;

            /// <summary>
            /// 6 - Sampling rate.
            /// </summary>
            public UInt32 sampleRate;

            /// <summary>
            /// 7 - Loop start.
            /// </summary>
            public UInt32 loopStart;

            /// <summary>
            /// 8 - Loop end.
            /// </summary>
            public UInt32 loopEnd;

            /// <summary>
            /// 9 - Original loop start.
            /// </summary>
            public UInt32 originalLoopStart;

            /// <summary>
            /// 10 - Channel info reference table.
            /// </summary>
            public ReferenceTable channelInfoRefTable;

            /// <summary>
            /// 11 - Channel info.
            /// </summary>
            public List<ChannelInfo> channelInfo;


            /// <summary>
            /// Channel info.
            /// </summary>
            public class ChannelInfo
            {

                /// <summary>
                /// 1 - Reference to samples location in data block.
                /// </summary>
                public Reference samplesRef;

                /// <summary>
                /// 2 - Reference to dspAdpcm info.
                /// </summary>
                public Reference dspAdpcmInfoRef;

                /// <summary>
                /// 3 - Reserved.
                /// </summary>
                public UInt32 reserved;

                /// <summary>
                /// 4 - DspAdpcm info.
                /// </summary>
                public DspAdpcmInfo dspAdpcmInfo;

            }

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

            //Read file header.
            fileHeader = new FileHeader(ref br);

            //Read info block.
            info = null;
            if (fileHeader.blockOffsets[0].offset != Reference.NULL_PTR && fileHeader.blockOffsets[0].typeId == ReferenceTypes.WAV_Block_Info)
            {

                br.Position = fileHeader.blockOffsets[0].offset;
                info = new InfoBlock
                {

                    magic = br.ReadChars(4),
                    blockSize = br.ReadUInt32(),
                    encoding = br.ReadByte(),
                    isLoop = br.ReadBoolean(),
                    padding = br.ReadUInt16(),
                    sampleRate = br.ReadUInt32(),
                    loopStart = br.ReadUInt32(),
                    loopEnd = br.ReadUInt32(),
                    originalLoopStart = br.ReadUInt32(),
                    channelInfoRefTable = new ReferenceTable(ref br),
                    channelInfo = new List<InfoBlock.ChannelInfo>()

                };

                //Read channel info.
                foreach (Reference r in info.channelInfoRefTable.references)
                {

                    //Set position.
                    br.Position = 0x5C + r.offset;

                    //New channel info.
                    InfoBlock.ChannelInfo c = new InfoBlock.ChannelInfo()
                    {

                        samplesRef = new Reference(ref br),
                        dspAdpcmInfoRef = new Reference(ref br),
                        reserved = br.ReadUInt32(),
                        dspAdpcmInfo = null

                    };

                    //Read Dsp-Apdcm info.
                    if (c.dspAdpcmInfoRef.offset != Reference.NULL_PTR)
                    {

                        br.Position = 0x5C + r.offset + c.dspAdpcmInfoRef.offset;

                        c.dspAdpcmInfo = new DspAdpcmInfo();
                        c.dspAdpcmInfo.coefs = new Int16[8][];
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

                    info.channelInfo.Add(c);

                }

            }

            //Read data block.
            if (fileHeader.blockOffsets[1].offset != Reference.NULL_PTR && fileHeader.blockOffsets[1].typeId == ReferenceTypes.WAV_Block_Data)
            {

                br.Position = fileHeader.blockOffsets[1].offset;
                data = new SoundNStreamDataBlock(ref br, info);

            }

        }


        /// <summary>
        /// Update the file.
        /// </summary>
        public void Update(UInt16 byteOrder)
        {

            //Find the size.
            UInt32 fileSize = 0;
            UInt32 infoSize = 32;
            UInt32 dataSize = data.GetSize(info.encoding, ref info);
            fileSize += dataSize;

            //Get size of info.
            info.magic = "INFO".ToCharArray();
            info.channelInfoRefTable = new ReferenceTable(new List<Reference>());
            info.channelInfoRefTable.count = (UInt32)info.channelInfo.Count();

            //Ref pointer.
            Int32 refPointer = 4 + 8 * (Int32)info.channelInfoRefTable.count;
            infoSize += 8 * info.channelInfoRefTable.count;

            //Channel info.
            for (int i = 0; i < info.channelInfoRefTable.count; i++)
            {

                //Add pointer.
                info.channelInfoRefTable.references.Add(new Reference(ReferenceTypes.WAV_ItemInfos, refPointer));

                refPointer += 20;
                infoSize += 20;

            }

            //Channel info.
            Int32 dspAdpcmOffset = 20 * info.channelInfo.Count();
            for (int i = 0; i < info.channelInfoRefTable.count; i++)
            {

                if (info.channelInfo[i].dspAdpcmInfo != null)
                {

                    info.channelInfo[i].dspAdpcmInfoRef = new Reference(0x300, dspAdpcmOffset);
                    refPointer += 46;
                    infoSize += 46;
                    dspAdpcmOffset += 46;

                }
                else
                {

                    info.channelInfo[i].dspAdpcmInfoRef = new Reference(0, Reference.NULL_PTR);

                }

                dspAdpcmOffset -= 20;

            }

            //Padding.
            while ((infoSize % 0x20) != 0)
            {

                infoSize += 1;

            }

            fileSize += infoSize;
            info.blockSize = infoSize;

            //Update the file header.
            string magic = "FWAV";
            if (byteOrder == ByteOrder.LittleEndian) { magic = "CWAV"; }
            fileHeader = new FileHeader(magic, byteOrder, fileHeader.version, fileSize, new List<SizedReference>() { new SizedReference(ReferenceTypes.WAV_Block_Info, 0, infoSize), new SizedReference(ReferenceTypes.WAV_Block_Data, (Int32)infoSize, dataSize) });

        }


        /// <summary>
        /// Convert the file to bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes(UInt16 byteOrder)
        {

            //Update file.
            Update(byteOrder);

            //New stream.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);

            //File header.
            fileHeader.Write(ref bw);

            //Write info.
            bw.Write(info.magic);
            bw.Write(info.blockSize);
            bw.Write(info.encoding);
            bw.Write(info.isLoop);
            bw.Write(info.padding);
            bw.Write(info.sampleRate);
            bw.Write(info.loopStart);
            bw.Write(info.loopEnd);
            bw.Write(info.originalLoopStart);
            info.channelInfoRefTable.Write(ref bw);

            //Write channel info reference table.
            foreach (InfoBlock.ChannelInfo c in info.channelInfo)
            {

                c.samplesRef.Write(ref bw);
                c.dspAdpcmInfoRef.Write(ref bw);
                bw.Write(c.reserved);

            }

            //Write channel info reference table.
            foreach (InfoBlock.ChannelInfo c in info.channelInfo)
            {

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

            //Padding.
            while ((bw.Position % 0x20) != 0)
            {

                bw.Write((byte)0);

            }

            //Write data block.
            data.WriteWAV(ref bw, info);

            //Return file.
            return o.ToArray();

        }

    }


    /// <summary>
    /// Create a b_wav painlessly.
    /// </summary>
    public static class WaveFactory
    {

        /// <summary>
        /// Create a standard b_wav.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="numSamples">Number of samples.</param>
        /// <param name="samples">Pcm8[][] or Pcm16[][] audio samples.</param>
        /// <param name="encoding">If samples is Pcm8[][] always 0. Must be 1 or 2 for if samples is Pcm16[][].</param>
        /// <param name="version">The version of the file.</param>
        /// <returns></returns>
        public static b_wav CreateWave(UInt32 sampleRate, UInt32 numSamples, object samples, byte encoding, UInt32 version)
        {

            //Create wav.
            b_wav b = new b_wav();

            b.fileHeader = new FileHeader("FWAV", ByteOrder.BigEndian, version, 0, new List<SizedReference>());
            b.info = new b_wav.InfoBlock();

            b.info.magic = "INFO".ToCharArray();
            b.info.encoding = encoding;
            b.info.isLoop = false;
            b.info.loopStart = b_wav.InfoBlock.NULL_LOOP;
            b.info.loopEnd = numSamples;
            b.info.originalLoopStart = b_wav.InfoBlock.NULL_LOOP;
            b.info.padding = 0;
            b.info.sampleRate = sampleRate;
            b.info.channelInfo = new List<b_wav.InfoBlock.ChannelInfo>();

            //Encoding.
            switch (encoding)
            {

                case EncodingTypes.PCM8:
                    b.data = new SoundNStreamDataBlock(EncoderFactory.Pcm8ToSignedPcm8(samples as byte[][]), encoding);
                    for (int i = 0; i < (samples as byte[][]).Length; i++)
                    {
                        b.info.channelInfo.Add(new b_wav.InfoBlock.ChannelInfo() { reserved = 0, dspAdpcmInfo = null });
                    }
                    break;

                case EncodingTypes.PCM16:
                    b.data = new SoundNStreamDataBlock(samples, encoding);
                    for (int i = 0; i < (samples as Int16[][]).Length; i++)
                    {
                        b.info.channelInfo.Add(new b_wav.InfoBlock.ChannelInfo() { reserved = 0, dspAdpcmInfo = null });
                    }
                    break;

                case EncodingTypes.DSP_ADPCM:
                    b.data = new SoundNStreamDataBlock(EncoderFactory.Pcm16ToDspApdcmWAV(samples as Int16[][], ref b), encoding);
                    break;

            }

            b.Update(ByteOrder.BigEndian);

            //Return wav.
            return b;

        }


        /// <summary>
        /// Create a looping b_wav.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="numSamples">Number of samples.</param>
        /// <param name="samples">Pcm8[][] or Pcm16[][] audio samples.</param>
        /// <param name="encoding">If samples is Pcm8[][] always 0. Must be 1 or 2 for if samples is Pcm16[][].</param>
        /// <param name="version">The version of the file.</param>
        /// <param name="loopStart">Loop starting point.</param>
        /// <returns></returns>
        public static b_wav CreateWave(UInt32 sampleRate, UInt32 numSamples, object samples, byte encoding, UInt32 version, UInt32 loopStart)
        {

            b_wav b = CreateWave(sampleRate, numSamples, samples, encoding, version);
            b.info.loopStart = loopStart;
            b.info.isLoop = true;
            return b;

        }


        /// <summary>
        /// Create a new b_wav from a RIFF Wave.
        /// </summary>
        /// <param name="r">The RIFF Wave.</param>
        /// <param name="encode">Whether or not to encode PCM16 data.</param>
        /// <param name="version">Version of the file.</param>
        /// <returns></returns>
        public static b_wav CreateWave(RiffWave r, bool encode, UInt32 version)
        {

            b_wav b = new b_wav();
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
                    b = CreateWave(r.fmt.sampleRate, endSample, pcm8.ToArray(), EncodingTypes.PCM8, version);
                }
                else
                {
                    b = CreateWave(r.fmt.sampleRate, endSample, pcm8.ToArray(), EncodingTypes.PCM8, version, r.smpl.loops[0].startSample);
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
                    b = CreateWave(r.fmt.sampleRate, (UInt32)r.data.channels[0].pcm16.Count(), pcm16.ToArray(), encoding, version);
                }
                else
                {
                    b = CreateWave(r.fmt.sampleRate, (UInt32)r.data.channels[0].pcm16.Count(), pcm16.ToArray(), encoding, version, r.smpl.loops[0].startSample);
                }
            }

            return b;

        }


        /// <summary>
        /// Create a wave from a stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        /// <param name="version">Version of the file.</param>
        /// <returns></returns>
        public static b_wav CreateWave(b_stm s, UInt32 version) {

            b_wav b = new b_wav();
            b.fileHeader = new FileHeader("FWAV", ByteOrder.BigEndian, version, 0, new List<SizedReference>());
            b.data = s.data;

            b.info = new b_wav.InfoBlock();
            b.info.sampleRate = s.info.streamSoundInfo.sampleRate;
            b.info.originalLoopStart = s.info.streamSoundInfo.originalLoopStart;
            b.info.loopStart = s.info.streamSoundInfo.loopStart;
            b.info.loopEnd = s.info.streamSoundInfo.sampleCount;
            b.info.isLoop = s.info.streamSoundInfo.isLoop;
            b.info.encoding = s.info.streamSoundInfo.encoding;

            b.info.channelInfo = new List<b_wav.InfoBlock.ChannelInfo>();
            foreach (b_stm.ChannelInfo c in s.info.channels) {

                b_wav.InfoBlock.ChannelInfo i = new b_wav.InfoBlock.ChannelInfo();
                i.dspAdpcmInfo = c.dspAdpcmInfo;
                b.info.channelInfo.Add(i);

            }

            b.Update(ByteOrder.BigEndian);

            return b;

        }


        /// <summary>
        /// Create a wave from a FISP.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static b_wav CreateWave(FISP f, UInt32 version)
        {

            //New wave.
            b_wav b = new b_wav();

            //Trim the fat from each loop.
            object channels = new short[f.data.data.Count()][];
            for (int i = 0; i < (channels as short[][]).Length; i++) {

                List<short> l = new List<short>();

                for (int j = 0; j < f.stream.loopEnd; j++) {
                    l.Add(f.data.data[i][j]);
                }

                (channels as short[][])[i] = l.ToArray();
            }

            //PCM8 conversion.
            if (f.stream.encoding == EncodingTypes.PCM8) {
                channels = EncoderFactory.Pcm16ToPcm8(channels as short[][]);
            }

            //If looped.
            if (f.stream.isLoop)
            {
                b = WaveFactory.CreateWave(f.stream.sampleRate, f.stream.loopEnd, channels, f.stream.encoding, version, f.stream.loopStart);
            }

            //Not looped.
            else
            {
                b = WaveFactory.CreateWave(f.stream.sampleRate, f.stream.loopEnd, channels, f.stream.encoding, version);
            }

            return b;

        }

    }

}
