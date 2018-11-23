using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Syroot.BinaryData;
using System.Diagnostics;
using CitraFileLoader;
using CitraFileLoaderz;

namespace IsabelleLib
{

    /// <summary>
    /// Citric Isabelle Sound Project (Depreciated).
    /// </summary>
    public class CISP {

        public char[] magic; //CISP.

        public streamInfo stream; //Stream Data.
        public List<trackInfo> tracks; //Tracks.

        public UInt32 seekSize; //Seek Size.
        public byte[] seekBlock; //Seek block.

        public char[] channelMagic; //CHAN.
        public List<UInt16[]> channelData; //Channel data.



        /// <summary>
        /// Stream Info.
        /// </summary>
        public struct streamInfo {

            public char[] magic; //STRM.
            public byte loop; //I'm no expert, but this probably signifies a loop.
            public byte numberOfChannels; //How many channels.
            public UInt32 sampleRate; //Rate of sampling.
            public UInt32 loopStart; //Loop start.
            public UInt32 loopEnd; //Loop end. If nonexistant, number of frames.
            public UInt32 numberOfTracks; //Number of tracks.
            public UInt32 sampleSize; //Size of a channel in bytes.

        }



        /// <summary>
        /// Track Info.
        /// </summary>
        public struct trackInfo {

            public char[] magic; //TRAC.
            public byte volume; //Volume.
            public byte pan; //Pan.
            public UInt16 flags; //Front Bypass???

            public UInt32 channelCount; //Channel Count.
            public List<byte> channels; //Channels.

        }


        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="b"></param>
        public void load(byte[] b) {

            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);
            br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            //Magic.
            magic = br.ReadChars(4);

            //Stream.
            stream.magic = br.ReadChars(4);
            stream.loop = br.ReadByte();
            stream.numberOfChannels = br.ReadByte();
            stream.sampleRate = br.ReadUInt32();
            stream.loopStart = br.ReadUInt32();
            stream.loopEnd = br.ReadUInt32();
            stream.numberOfTracks = br.ReadUInt32();
            stream.sampleSize = br.ReadUInt32();

            //Tracks.
            tracks = new List<trackInfo>();
            for (int i = 0; i < (int)stream.numberOfTracks; i++)
            {

                trackInfo t = new trackInfo();
                t.magic = br.ReadChars(4);
                t.volume = br.ReadByte();
                t.pan = br.ReadByte();
                t.flags = br.ReadUInt16();
                t.channelCount = br.ReadUInt32();
                t.channels = new List<byte>();
                for (int j = 0; j < t.channelCount; j++)
                {
                    t.channels.Add(br.ReadByte());
                }
                tracks.Add(t);

            }

            //Seek.
            seekSize = br.ReadUInt32();
            seekBlock = br.ReadBytes((int)seekSize);

            //Channels.
            channelMagic = br.ReadChars(4);
            channelData = new List<UInt16[]>();
            for (int i = 0; i < stream.numberOfChannels; i++)
            {
                List<UInt16> channel = new List<UInt16>();
                for (int j = 0; j < (int)(stream.sampleSize / 2); j++) {
                    channel.Add(br.ReadUInt16());
                }
                channelData.Add(channel.ToArray());
            }

        }


        /// <summary>
        /// Convert file to bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] toBytes() {

            update();

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            bw.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            //Magic.
            bw.Write(magic);

            //Stream.
            bw.Write(stream.magic);
            bw.Write(stream.loop);
            bw.Write(stream.numberOfChannels);
            bw.Write(stream.sampleRate);
            bw.Write(stream.loopStart);
            bw.Write(stream.loopEnd);
            bw.Write(stream.numberOfTracks);
            bw.Write(stream.sampleSize);

            //Tracks.
            foreach (trackInfo t in tracks) {

                bw.Write(t.magic);
                bw.Write(t.volume);
                bw.Write(t.pan);
                bw.Write(t.flags);
                bw.Write(t.channelCount);
                foreach (byte c in t.channels) {
                    bw.Write(c);
                }

            }

            //Seek.
            bw.Write(seekSize);
            bw.Write(seekBlock);

            //Channels.
            bw.Write(channelMagic);
            foreach (UInt16[] a in channelData) {
                foreach (UInt16 c in a) {
                bw.Write(c);}
            }

            //Return final file.
            return o.ToArray();

        }


        /// <summary>
        /// Update the file.
        /// </summary>
        public void update() {

            magic = "CISP".ToCharArray();
            stream.magic = "STRM".ToCharArray();
            stream.numberOfChannels = (byte)channelData.Count;
            stream.numberOfTracks = (UInt32)tracks.Count;
            if (stream.numberOfChannels == 0)
            {
                stream.sampleSize = 0;
            }
            else {
                stream.sampleSize = (UInt32)(channelData[0].Count() * 2);
            }
            seekBlock = new byte[0];
            seekSize = (UInt32)seekBlock.Length;
            channelMagic = "CHAN".ToCharArray();
            for (int i = 0; i < tracks.Count; i++) {
                trackInfo t = tracks[i];
                t.channelCount = (UInt32)tracks[i].channels.Count;
                t.magic = "TRAC".ToCharArray();
                tracks[i] = t;
            }

        }

    }


    /// <summary>
    /// Cafe Isabelle Sound Project.
    /// </summary>
    public class FISP {

        /// <summary>
        /// 1 - FISP.
        /// </summary>
        public char[] magic;

        /// <summary>
        /// 2 - Track count.
        /// </summary>
        public UInt32 trackCount;

        /// <summary>
        /// 3 - Region count.
        /// </summary>
        public UInt32 regionCount;

        /// <summary>
        /// 4 - Stream info.
        /// </summary>
        public StreamInfo stream;

        /// <summary>
        /// 5 - Tracks.
        /// </summary>
        public List<TrackInfo> tracks;

        /// <summary>
        /// 6 - Regions.
        /// </summary>
        public List<RegionInfo> regions;

        /// <summary>
        /// 7 - Data.
        /// </summary>
        public DataInfo data;


        /// <summary>
        /// Stream information.
        /// </summary>
        public class StreamInfo {

            /// <summary>
            /// 1 - STM.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Preferred encoding.
            /// </summary>
            public byte encoding = 2;

            /// <summary>
            /// 3 - Major version.
            /// </summary>
            public byte vMajor = 4;

            /// <summary>
            /// 4 - Minor version.
            /// </summary>
            public byte vMinor;

            /// <summary>
            /// 5 - Revision version.
            /// </summary>
            public byte vRevision;

            /// <summary>
            /// 6 - If song loops.
            /// </summary>
            public bool isLoop;

            /// <summary>
            /// 7 - Sampling rate.
            /// </summary>
            public UInt32 sampleRate;

            /// <summary>
            /// 8 - Loop start.
            /// </summary>
            public UInt32 loopStart;

            /// <summary>
            /// 9 - Loop end.
            /// </summary>
            public UInt32 loopEnd;

            /// <summary>
            /// 10 - Original loop start.
            /// </summary>
            public UInt32 originalLoopStart;

            /// <summary>
            /// 11 - Original loop end.
            /// </summary>
            public UInt32 originalLoopEnd;

            /// <summary>
            /// 12 - Secret info. Idk what this does.
            /// </summary>
            public UInt32 secretInfo;

        }

        /// <summary>
        /// Track information.
        /// </summary>
        public class TrackInfo {

            /// <summary>
            /// 1 - TRAC.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Volume.
            /// </summary>
            public byte volume;

            /// <summary>
            /// 3 - Pan.
            /// </summary>
            public byte pan;

            /// <summary>
            /// 4 - Span.
            /// </summary>
            public byte span;

            /// <summary>
            /// 5 - Surround mode.
            /// </summary>
            public byte surroundMode;

            /// <summary>
            /// 6 - Number of channels for the track.
            /// </summary>
            public UInt32 numChannels;

            /// <summary>
            /// 7 - Channels for the track.
            /// </summary>
            public List<byte> channels;

        }

        /// <summary>
        /// Region information.
        /// </summary>
        public class RegionInfo {

            /// <summary>
            /// 1 - REGN.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Starting sample.
            /// </summary>
            public UInt32 start;

            /// <summary>
            /// 3 - Ending sample.
            /// </summary>
            public UInt32 end;

        }

        /// <summary>
        /// Data info.
        /// </summary>
        public class DataInfo {

            /// <summary>
            /// 1 - DATA.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Number of channels.
            /// </summary>
            public UInt32 numChannels;

            /// <summary>
            /// 3 - Number of samples.
            /// </summary>
            public UInt32 numSamples;

            /// <summary>
            /// 4 - Audio data.
            /// </summary>
            public List<Int16[]> data;

        }


        /// <summary>
        /// Blank constructor.
        /// </summary>
        public FISP() {

        }

        /// <summary>
        /// Load from a file.
        /// </summary>
        /// <param name="file"></param>
        public FISP(byte[] file) {

            Load(file);

        }

        /// <summary>
        /// FISP from a CISP.
        /// </summary>
        /// <param name="c"></param>
        public FISP(CISP c) {

            trackCount = (uint)c.tracks.Count();
            regions = new List<RegionInfo>();
            tracks = new List<TrackInfo>();
            data = new DataInfo();

            //Stream info.
            stream = new StreamInfo()
            {

                encoding = 2,
                isLoop = c.stream.loop != 0,
                loopEnd = c.stream.loopEnd,
                loopStart = c.stream.loopStart, 
                sampleRate = c.stream.sampleRate,
                vMajor = 4

            };

            //Audio data.
            data.data = new List<short[]>();
            for (int i = 0; i < c.channelData.Count; i++) {

                short[] s = new short[c.channelData[i].Length];
                for (int j = 0; j < s.Length; j++) {
                    s[j] = (short)c.channelData[i][j];
                }
                data.data.Add(s);

            }
            data.numChannels = (uint)c.channelData.Count();

            //Tracks.
            foreach (CISP.trackInfo i in c.tracks) {

                TrackInfo t = new TrackInfo();
                t.pan = i.pan;
                t.span = 0;
                t.surroundMode = 0;
                t.volume = i.volume;
                t.channels = i.channels;
                tracks.Add(t);

            }

        }

        /// <summary>
        /// From a RIFF wave. WIP.
        /// </summary>
        /// <param name="r"></param>
        public FISP(RiffWave r) {

            regions = new List<RegionInfo>();
            tracks = new List<TrackInfo>();
            data = new DataInfo();
            stream = new StreamInfo();

            //Data.
            if (r.fmt.bitsPerSample == 16)
            {
                short[][] pcm16 = new short[r.data.channels.Count][];
                for (int i = 0; i < r.data.channels.Count; i++)
                {
                    pcm16[i] = r.data.channels[i].pcm16.ToArray();
                }
                data.data = pcm16.ToList();
            }
            else
            {
                byte[][] pcm8 = new byte[r.data.channels.Count][];
                for (int i = 0; i < r.data.channels.Count; i++)
                {
                    pcm8[i] = r.data.channels[i].pcm8.ToArray();
                }
                data.data = EncoderFactory.Pcm8ToPcm16(pcm8).ToList();
            }

            //Stream info.
            stream.sampleRate = r.fmt.sampleRate;
            stream.encoding = 2;
            if (data.data.Count > 0) { stream.loopEnd = (uint)data.data[0].Length; }
            if (r.smpl != null) {
                if (r.smpl.loops.Count > 0) {
                    stream.isLoop = true;
                    stream.loopStart = stream.originalLoopStart = r.smpl.loops[0].startSample;
                    stream.loopEnd = stream.originalLoopEnd = r.smpl.loops[0].endSample;
                }
            }

        }

        /// <summary>
        /// From a b_wav.
        /// </summary>
        /// <param name="b"></param>
        public FISP(b_wav b) {

            regions = new List<RegionInfo>();
            tracks = new List<TrackInfo>();
            data = new DataInfo();
            stream = new StreamInfo();

            //Stream info.
            stream.sampleRate = b.info.sampleRate;
            stream.encoding = b.info.encoding;
            stream.isLoop = b.info.isLoop;
            stream.loopEnd = b.info.loopEnd;
            stream.loopStart = b.info.loopStart;
            stream.originalLoopEnd = b.info.loopEnd;
            stream.originalLoopStart = b.info.originalLoopStart;
            stream.vMajor = b.fileHeader.vMajor;
            stream.vMinor = b.fileHeader.vMinor;
            stream.vRevision = b.fileHeader.vRevision;

            //Data.
            switch (b.info.encoding) {

                //PCM8.
                case EncodingTypes.PCM8:
                    data.data = EncoderFactory.SignedPcm8ToPcm16(b.data.pcm8).ToList();
                    break;

                //PCM16.
                case EncodingTypes.PCM16:
                    data.data = b.data.pcm16.ToList();
                    break;

                //DSP-ADPCM.
                case EncodingTypes.DSP_ADPCM:
                    data.data = (b.data.GetDataWAV(b.info) as short[][]).ToList();
                    break;

            }

        }

        /// <summary>
        /// From a stream.
        /// </summary>
        /// <param name="s"></param>
        public FISP(b_stm s) {

            regions = new List<RegionInfo>();
            tracks = new List<TrackInfo>();
            data = new DataInfo();
            stream = new StreamInfo();

            //Stream info.
            stream.sampleRate = s.info.streamSoundInfo.sampleRate;
            stream.encoding = s.info.streamSoundInfo.encoding;
            stream.isLoop = s.info.streamSoundInfo.isLoop;
            stream.loopEnd = s.info.streamSoundInfo.sampleCount;
            stream.loopStart = s.info.streamSoundInfo.loopStart;
            stream.originalLoopEnd = s.info.streamSoundInfo.originalLoopEnd;
            stream.originalLoopStart = s.info.streamSoundInfo.originalLoopStart;
            stream.vMajor = s.fileHeader.vMajor;
            stream.vMinor = s.fileHeader.vMinor;
            stream.vRevision = s.fileHeader.vRevision;
            stream.secretInfo = s.info.streamSoundInfo.secretInfo;

            //Data.
            switch (s.info.streamSoundInfo.encoding)
            {

                //PCM8.
                case EncodingTypes.PCM8:
                    data.data = EncoderFactory.SignedPcm8ToPcm16(s.data.pcm8).ToList();
                    break;

                //PCM16.
                case EncodingTypes.PCM16:
                    data.data = s.data.pcm16.ToList();
                    break;

                //DSP-ADPCM.
                case EncodingTypes.DSP_ADPCM:
                    data.data = (s.data.GetDataSTM(s.info.streamSoundInfo, s.info) as short[][]).ToList();
                    break;

            }

            //Tracks.
            if (s.info.tracks != null)
            {
                foreach (b_stm.TrackInfo i in s.info.tracks)
                {

                    TrackInfo t = new TrackInfo()
                    {

                        pan = i.pan,
                        span = i.span,
                        surroundMode = i.surroundMode,
                        volume = i.volume,
                        channels = i.globalChannelIndexTable.entries

                    };
                    tracks.Add(t);

                }
            }

            //Regions.
            if (s.region != null) {

                foreach (SoundNStreamRegionBlock.RegionInfo i in s.region.regions) {

                    regions.Add(new RegionInfo()
                    {

                        start = i.start,
                        end = i.end,

                    });

                }

            }

        }


        /// <summary>
        /// Load a file. WIP.
        /// </summary>
        /// <param name="b"></param>
        public void Load(byte[] b) {

            //Reader.
            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);

            magic = br.ReadChars(4);
            trackCount = br.ReadUInt32();
            regionCount = br.ReadUInt32();

            stream = new StreamInfo()
            {

                magic = br.ReadChars(3),
                encoding = br.ReadByte(),
                vMajor = br.ReadByte(),
                vMinor = br.ReadByte(),
                vRevision = br.ReadByte(),
                isLoop = br.ReadBoolean(),
                sampleRate = br.ReadUInt32(),
                loopStart = br.ReadUInt32(),
                loopEnd = br.ReadUInt32(),
                originalLoopStart = br.ReadUInt32(),
                originalLoopEnd = br.ReadUInt32(),
                secretInfo = br.ReadUInt32()

            };

            tracks = new List<TrackInfo>();
            for (int i = 0; i < trackCount; i++) {

                TrackInfo t = new TrackInfo
                {

                    magic = br.ReadChars(4),
                    volume = br.ReadByte(),
                    pan = br.ReadByte(),
                    span = br.ReadByte(),
                    surroundMode = br.ReadByte(),
                    numChannels = br.ReadUInt32(),

                };
                t.channels = br.ReadBytes((int)t.numChannels).ToList();

                tracks.Add(t);

            }

            regions = new List<RegionInfo>();
            for (int i = 0; i < regionCount; i++) {

                RegionInfo r = new RegionInfo()
                {
                    
                   magic = br.ReadChars(4),
                   start = br.ReadUInt32(),
                   end = br.ReadUInt32()

                };
                regions.Add(r);

            }

            data = new DataInfo()
            {

                magic = br.ReadChars(4),
                numChannels = br.ReadUInt32(),
                numSamples = br.ReadUInt32(),
                data = new List<short[]>()

            };

            for (int i = 0; i < data.numChannels; i++) {

                data.data.Add(br.ReadInt16s((int)data.numSamples));

            }

        }

        /// <summary>
        /// Update the file.
        /// </summary>
        public void Update() {

            magic = "FISP".ToCharArray();
            trackCount = (uint)tracks.Count();
            regionCount = (uint)regions.Count();

            stream.magic = "STM".ToCharArray();
            data.magic = "DATA".ToCharArray();
            data.numChannels = (uint)data.data.Count();
            data.numSamples = data.data.Count > 0 ? (uint)data.data[0].Length : 0;

            for (int i = 0; i < tracks.Count; i++) {
                tracks[i].magic = "TRAC".ToCharArray();
                tracks[i].numChannels = (uint)tracks[i].channels.Count();
            }

            for (int i = 0; i < regions.Count; i++) {
                regions[i].magic = "REGN".ToCharArray();
            }

        }

        /// <summary>
        /// Convert file to bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {

            //Update the file.
            Update();

            //Write the file.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);

            bw.Write(magic);
            bw.Write(trackCount);
            bw.Write(regionCount);

            bw.Write(stream.magic);
            bw.Write(stream.encoding);
            bw.Write(stream.vMajor);
            bw.Write(stream.vMinor);
            bw.Write(stream.vRevision);
            bw.Write(stream.isLoop);
            bw.Write(stream.sampleRate);
            bw.Write(stream.loopStart);
            bw.Write(stream.loopEnd);
            bw.Write(stream.originalLoopStart);
            bw.Write(stream.originalLoopEnd);
            bw.Write(stream.secretInfo);

            foreach (TrackInfo t in tracks) {

                bw.Write(t.magic);
                bw.Write(t.volume);
                bw.Write(t.pan);
                bw.Write(t.span);
                bw.Write(t.surroundMode);
                bw.Write(t.numChannels);
                bw.Write(t.channels.ToArray());

            }

            foreach (RegionInfo r in regions) {

                bw.Write(r.magic);
                bw.Write(r.start);
                bw.Write(r.end);

            }

            bw.Write(data.magic);
            bw.Write(data.numChannels);
            bw.Write(data.numSamples);
            foreach (short[] channel in data.data) {
                bw.Write(channel);
            }

            return o.ToArray();

        }

        /// <summary>
        /// Get the file version.
        /// </summary>
        /// <param name="ds">If for 3ds.</param>
        /// <returns></returns>
        public UInt32 GetVersion(bool ds) {

            UInt32 temp = 0;
            if (!ds)
            {
                temp = stream.vRevision;
                temp += (uint)stream.vMinor << 8;
                temp += (uint)stream.vMajor << 16;
            }
            else
            {

                temp += (uint)stream.vMajor;
                temp += (uint)stream.vMinor << 8;
                temp += (uint)stream.vRevision << 16;

            }
            return temp;

        }

    }

}
