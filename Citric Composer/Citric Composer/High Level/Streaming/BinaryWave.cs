using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsabelleLib;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Binary wave.
    /// </summary>
    public class BinaryWave : ISoundFile {

        /// <summary>
        /// Byte order.
        /// </summary>
        public Syroot.BinaryData.ByteOrder ByteOrder;

        /// <summary>
        /// Major version.
        /// </summary>
        public byte Major;

        /// <summary>
        /// Minor version.
        /// </summary>
        public byte Minor;

        /// <summary>
        /// Sample rate.
        /// </summary>
        public uint SampleRate;

        /// <summary>
        /// Number of samples.
        /// </summary>
        public uint NumSamples { get; private set; }

        /// <summary>
        /// DSP Info.
        /// </summary>
        public DspAdpcmInfo[] DspAdpcmInfo;

        /// <summary>
        /// Channel pans.
        /// </summary>
        public ChannelPan[] ChannelPans;

        /// <summary>
        /// Loop flag.
        /// </summary>
        public bool Loops;

        /// <summary>
        /// Loop start sample.
        /// </summary>
        public uint LoopStartSample;

        /// <summary>
        /// Loop end sample.
        /// </summary>
        public uint LoopEndSample;

        /// <summary>
        /// Data block.
        /// </summary>
        public SoundNStreamDataBlock Data;

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public BinaryWave() { }

        /// <summary>
        /// Channel pan.
        /// </summary>
        public enum ChannelPan : ushort {
            Left, Right, Middle
        }

        /// <summary>
        /// Load a wave file.
        /// </summary>
        /// <param name="b">The byte array.</param>
        public void Load(byte[] b) {

            //Read file.
            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);

            //Get byte order.
            br.ByteOrder = ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            br.Position = 4;
            if (br.ReadUInt16() == CitraFileLoader.ByteOrder.LittleEndian) {
                br.ByteOrder = ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            }

            //Get version.
            ushort version = br.ReadUInt16();
            Major = (byte)((version & 0xFF00) >> 8);
            Minor = (byte)(version & 0xFF);

            //Get num channels.
            br.Position = 0x0E;
            ushort numChannels = br.ReadUInt16();
            ChannelPans = new ChannelPan[numChannels];

            //Get info from first channel.
            br.Position = 0x12;
            ChannelPans[0] = (ChannelPan)br.ReadUInt16();
            SampleRate = br.ReadUInt32();
            NumSamples = br.ReadUInt32();
            br.ReadUInt32();
            DspAdpcmInfo = new DspAdpcmInfo[numChannels];
            DspAdpcmInfo[0] = new DspAdpcmInfo();
            DspAdpcmInfo[0].coefs = new short[8][];
            DspAdpcmInfo[0].coefs[0] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[1] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[2] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[3] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[4] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[5] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[6] = br.ReadInt16s(2);
            DspAdpcmInfo[0].coefs[7] = br.ReadInt16s(2);

            //Start offsets.
            uint[] startOffsets = new uint[numChannels];
            startOffsets[0] = br.ReadUInt32();
            br.Position += 4;

            //Loop info.
            Loops = br.ReadUInt32() > 0;
            LoopEndSample = br.ReadUInt32();
            LoopStartSample = br.ReadUInt32();

            //More DSP info.
            DspAdpcmInfo[0].pred_scale = DspAdpcmInfo[0].loop_pred_scale = br.ReadUInt16();
            DspAdpcmInfo[0].yn1 = DspAdpcmInfo[0].loop_yn1 = br.ReadInt16();
            DspAdpcmInfo[0].yn2 = DspAdpcmInfo[0].loop_yn2 = br.ReadInt16();

            //Read each channel start offset.
            for (int i = 1; i < numChannels; i++) {

                //Get channel pan.
                br.Position = i * 0x4C + 0x10 + 0x2;
                ChannelPans[i] = (ChannelPan)br.ReadUInt16();

                //Start offset.
                br.Position = i * 0x4C + 0x10 + 0x30;
                startOffsets[i] = br.ReadUInt32();

                //Get DSP info.
                br.Position = i * 0x4C + 0x10 + 0x10;
                DspAdpcmInfo[i] = new DspAdpcmInfo();
                DspAdpcmInfo[i].coefs = new short[8][];
                DspAdpcmInfo[i].coefs[0] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[1] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[2] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[3] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[4] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[5] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[6] = br.ReadInt16s(2);
                DspAdpcmInfo[i].coefs[7] = br.ReadInt16s(2);
                br.Position += 20;
                DspAdpcmInfo[i].pred_scale = DspAdpcmInfo[i].loop_pred_scale = br.ReadUInt16();
                DspAdpcmInfo[i].yn1 = DspAdpcmInfo[i].loop_yn1 = br.ReadInt16();
                DspAdpcmInfo[i].yn2 = DspAdpcmInfo[i].loop_yn2 = br.ReadInt16();

            }

            //Read the wave data.
            Data = new SoundNStreamDataBlock(br, startOffsets);
            try { br.Dispose(); } catch { }
            try { src.Dispose(); } catch { }

        }

        /// <summary>
        /// Convert this to bytes.
        /// </summary>
        /// <returns>This as bytes.</returns>
        public byte[] ToBytes() {

            //Writer.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            bw.ByteOrder = ByteOrder;

            //Header.
            bw.Write("BWAV".ToCharArray());
            bw.Write(ByteOrder == Syroot.BinaryData.ByteOrder.LittleEndian ? CitraFileLoader.ByteOrder.BigEndian : CitraFileLoader.ByteOrder.LittleEndian);
            ushort version = 0;
            version += (ushort)(Major << 8);
            version += Minor;
            bw.Write(version);
            bw.Write((uint)0);
            bw.Write((ushort)0);
            bw.Write((ushort)DspAdpcmInfo.Length);

            //Write each channel.
            uint offset = (uint)(0x4C * DspAdpcmInfo.Length + 0x10);
            while (offset % 0x40 != 0) {
                offset++;
            }
            for (int i = 0; i < DspAdpcmInfo.Length; i++) {

                //Info.
                bw.Write((ushort)1);
                bw.Write((ushort)ChannelPans[i]);
                bw.Write(SampleRate);
                bw.Write(NumSamples);
                bw.Write(NumSamples);
                bw.Write(DspAdpcmInfo[i].coefs[0]);
                bw.Write(DspAdpcmInfo[i].coefs[1]);
                bw.Write(DspAdpcmInfo[i].coefs[2]);
                bw.Write(DspAdpcmInfo[i].coefs[3]);
                bw.Write(DspAdpcmInfo[i].coefs[4]);
                bw.Write(DspAdpcmInfo[i].coefs[5]);
                bw.Write(DspAdpcmInfo[i].coefs[6]);
                bw.Write(DspAdpcmInfo[i].coefs[7]);
                bw.Write(offset);
                bw.Write(offset);
                bw.Write((uint)(Loops ? 1 : 0));
                bw.Write(LoopEndSample);
                bw.Write(LoopStartSample);
                bw.Write(DspAdpcmInfo[i].pred_scale);
                bw.Write(DspAdpcmInfo[i].yn1);
                bw.Write(DspAdpcmInfo[i].yn2);
                bw.Write((ushort)0);

                //Offset.
                offset += (uint)Data.dspAdpcm[i].Length;
                while (offset % 0x40 != 0) {
                    offset++;
                }

            }

            //Write the data.
            for (int i = 0; i < DspAdpcmInfo.Length; i++) {

                //Align.
                while (bw.Position % 0x40 != 0) {
                    bw.Position++;
                }

                //Write data.
                bw.Write(Data.dspAdpcm[i]);

            }

            //Get hash.
            MemoryStream chanData = new MemoryStream();
            BinaryDataWriter chanWriter = new BinaryDataWriter(chanData);
            foreach (byte[] b in Data.dspAdpcm) {
                chanWriter.Write(b);
            }
            bw.Position = 8;
            bw.Write(Crc32.Crc32Algorithm.Compute(chanData.ToArray()));

            //Return and clean.
            byte[] ret = o.ToArray();
            try { bw.Dispose(); } catch { }
            try { o.Dispose(); } catch { }
            try { chanWriter.Dispose(); } catch { }
            try { chanData.Dispose(); } catch { }         
            return ret;

        }

        /// <summary>
        /// New binary wave.
        /// </summary>
        /// <param name="wave">Wave to create this from.</param>
        public BinaryWave(b_wav wave) {

            //Set data.
            ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            Major = 0;
            Minor = 1;
            SampleRate = wave.info.sampleRate;
            NumSamples = wave.info.loopEnd;
            switch (wave.info.encoding) {
                case 0:
                    wave.data.dspAdpcm = EncoderFactory.Pcm16ToDspApdcmWAV(EncoderFactory.SignedPcm8ToPcm16(wave.data.pcm8), ref wave);
                    break;
                case 1:
                    wave.data.dspAdpcm = EncoderFactory.Pcm16ToDspApdcmWAV(wave.data.pcm16, ref wave);
                    break;
            }
            DspAdpcmInfo = new DspAdpcmInfo[wave.info.channelInfo.Count];
            for (int i = 0; i < DspAdpcmInfo.Length; i++) {
                DspAdpcmInfo[i] = wave.info.channelInfo[i].dspAdpcmInfo;
            }
            Loops = wave.info.isLoop;
            LoopStartSample = wave.info.loopStart;
            LoopEndSample = wave.info.loopEnd;
            Data = wave.data;

            //Do channel pans.
            ChannelPans = new ChannelPan[wave.info.channelInfo.Count()];
            for (int i = 0; i < wave.info.channelInfo.Count(); i++) {
                if (i == wave.info.channelInfo.Count() - 1) {
                    ChannelPans[i] = ChannelPan.Middle;
                } else if (i % 2 == 0) {
                    ChannelPans[i] = ChannelPan.Left;
                    ChannelPans[i] = ChannelPan.Right;
                }
            }

        }

        /// <summary>
        /// New binary wave.
        /// </summary>
        /// <param name="s">The stream.</param>
        public BinaryWave(b_stm s) {

            //Set data.
            ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            Major = 0;
            Minor = 1;
            SampleRate = s.info.streamSoundInfo.sampleRate;
            NumSamples = s.info.streamSoundInfo.sampleCount;
            switch (s.info.streamSoundInfo.encoding) {
                case 0:
                    s.data.dspAdpcm = EncoderFactory.Pcm16ToDspAdpcmSTM(EncoderFactory.SignedPcm8ToPcm16(s.data.pcm8), s);
                    break;
                case 1:
                    s.data.dspAdpcm = EncoderFactory.Pcm16ToDspAdpcmSTM(s.data.pcm16, s);
                    break;
            }
            DspAdpcmInfo = new DspAdpcmInfo[s.info.channels.Count];
            for (int i = 0; i < DspAdpcmInfo.Length; i++) {
                DspAdpcmInfo[i] = s.info.channels[i].dspAdpcmInfo;
            }
            Loops = s.info.streamSoundInfo.isLoop;
            LoopStartSample = s.info.streamSoundInfo.loopStart;
            LoopEndSample = s.info.streamSoundInfo.sampleCount;
            Data = s.data;

            //Do channel pans.
            ChannelPans = new ChannelPan[s.info.channels.Count];
            for (int i = 0; i < s.info.channels.Count; i++) {
                if (i == s.info.channels.Count - 1) {
                    ChannelPans[i] = ChannelPan.Middle;
                } else if (i % 2 == 0) {
                    ChannelPans[i] = ChannelPan.Left;
                    ChannelPans[i+1] = ChannelPan.Right;
                    i++;
                }
            }
            if (s.info.tracks != null) {
                foreach (var t in s.info.tracks) {
                    if (t.globalChannelIndexTable.count > 0) {
                        if (t.globalChannelIndexTable.count > 1) {
                            ChannelPans[t.globalChannelIndexTable.entries[0]] = ChannelPan.Left;
                            ChannelPans[t.globalChannelIndexTable.entries[1]] = ChannelPan.Right;
                        } else {
                            ChannelPans[t.globalChannelIndexTable.entries[0]] = ChannelPan.Middle;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Create a binary wave.
        /// </summary>
        /// <param name="f">FISP.</param>
        public static BinaryWave FromFISP(FISP f) {

            //Set data.
            return new BinaryWave(StreamFactory.CreateStream(f, 4, 0, 0));

        }

        /// <summary>
        /// Create a binary wave.
        /// </summary>
        /// <param name="r">Riff wave.</param>
        public static BinaryWave FromRiff(RiffWave r) {
            return new BinaryWave(StreamFactory.CreateStream(r, true, 4, 0, 0));
        }

        public void Write(WriteMode writeMode, BinaryDataWriter bw) {
            switch (writeMode) {
                case WriteMode.Cafe:
                case WriteMode.C_BE:
                    ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                    break;
                case WriteMode.NX:
                case WriteMode.CTR:
                    ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                    break;
            }
        }

        public void Write(BinaryDataWriter bw) {
            ByteOrder = bw.ByteOrder;
            bw.Write(ToBytes());
        }

        public void Read(BinaryDataReader br) {
            Load((br.BaseStream as MemoryStream).ToArray());
        }

        public string GetExtension() {
            return "BWAV";
        }

    }

}