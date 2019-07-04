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
    /// RIFF Wave.
    /// </summary>
    public class RiffWave
    {

        /// <summary>
        /// 1 - RIFF.
        /// </summary>
        public string magic;

        /// <summary>
        /// 2 - Size of the chunk after this.
        /// </summary>
        public UInt32 chunkSize;

        /// <summary>
        /// 3 - WAVE.
        /// </summary>
        public string format;

        /// <summary>
        /// 4 - Fmt.
        /// </summary>
        public FmtChunk fmt;

        /// <summary>
        /// 5 - Data.
        /// </summary>
        public DataChunk data;

        /// <summary>
        /// 6 - Sample.
        /// </summary>
        public SmplChunk smpl;


        /// <summary>
        /// Chunk.
        /// </summary>
        public class Chunk
        {

            /// <summary>
            /// 1 - Chunk identifier.
            /// </summary>
            public string identifier;

            /// <summary>
            /// 2 - Chunk size.
            /// </summary>
            public UInt32 size;


            /// <summary>
            /// Load a chunk.
            /// </summary>
            /// <param name="br">The reader.</param>
            public void Load(ref BinaryDataReader br)
            {

                identifier = new string(br.ReadChars(4));
                size = br.ReadUInt32();

            }


            /// <summary>
            /// Update the chunk.
            /// </summary>
            /// <param name="identifier"></param>
            /// <param name="size"></param>
            public void Update(string identifier, UInt32 size)
            {

                this.identifier = identifier;
                this.size = size;

            }


            /// <summary>
            /// Write the chunk.
            /// </summary>
            /// <param name="bw">The writer.</param>
            public void Write(ref BinaryDataWriter bw)
            {

                bw.Write(identifier.ToCharArray());
                bw.Write(size);

            }

        }


        /// <summary>
        /// Fmt Chunk.
        /// </summary>
        public class FmtChunk : Chunk
        {

            /// <summary>
            /// PCM audio format.
            /// </summary>
            public const UInt16 FormatPCM = 1;

            /// <summary>
            /// 3 - Audio format.
            /// </summary>
            public UInt16 audioFormat;

            /// <summary>
            /// 4 - Number of channels.
            /// </summary>
            public UInt16 numChannels;

            /// <summary>
            /// 5 - Sample rate.
            /// </summary>
            public UInt32 sampleRate;

            /// <summary>
            /// 6 - SampleRate * NumChannels * BitsPerSample/8.
            /// </summary>
            public UInt32 byteRate;

            /// <summary>
            /// 7 - NumChannels * BitsPerSample/8.
            /// </summary>
            public UInt16 blockAlign;

            /// <summary>
            /// 8 - Number of bits per sample.
            /// </summary>
            public UInt16 bitsPerSample;


            /// <summary>
            /// Bits per sample.
            /// </summary>
            public static class BitsPerSample
            {

                public const UInt16 PCM8 = 8;
                public const UInt16 PCM16 = 16;

            }


            /// <summary>
            /// Blank constructor.
            /// </summary>
            public FmtChunk()
            {



            }


            /// <summary>
            /// Make a new fmt chunk.
            /// </summary>
            /// <param name="numChannels">Number of channels.</param>
            /// <param name="sampleRate">Sample rate.</param>
            /// <param name="bytesPerSample">Bytes per sample.</param>
            public FmtChunk(UInt16 numChannels, UInt32 sampleRate, UInt16 bytesPerSample)
            {

                this.numChannels = numChannels;
                this.sampleRate = sampleRate;
                bitsPerSample = (UInt16)(bytesPerSample * 8);
                audioFormat = FormatPCM;

            }


            /// <summary>
            /// Load the fmt chunk.
            /// </summary>
            /// <param name="br">The reader.</param>
            public new void Load(ref BinaryDataReader br)
            {

                //Call load.
                base.Load(ref br);

                audioFormat = br.ReadUInt16();
                numChannels = br.ReadUInt16();
                sampleRate = br.ReadUInt32();
                byteRate = br.ReadUInt32();
                blockAlign = br.ReadUInt16();
                bitsPerSample = br.ReadUInt16();

            }


            /// <summary>
            /// Update the fmt chunk.
            /// </summary>
            public void Update()
            {

                byteRate = sampleRate * numChannels * bitsPerSample / 8;
                blockAlign = (UInt16)(numChannels * bitsPerSample / 8);

                //Call update.
                base.Update("fmt ", 16);

            }


            /// <summary>
            /// Write the fmt chunk.
            /// </summary>
            /// <param name="bw">The writer.</param>
            public new void Write(ref BinaryDataWriter bw)
            {

                //Call update.
                Update();

                //Call write.
                base.Write(ref bw);

                bw.Write(audioFormat);
                bw.Write(numChannels);
                bw.Write(sampleRate);
                bw.Write(byteRate);
                bw.Write(blockAlign);
                bw.Write(bitsPerSample);

            }


        }


        /// <summary>
        /// Data chunk.
        /// </summary>
        public class DataChunk : Chunk
        {

            /// <summary>
            /// 3 - The channels, containing sample data.
            /// </summary>
            public List<DataSamples> channels;


            /// <summary>
            /// Data samples.
            /// </summary>
            /// <typeparam name="T">Depends on bits per sample.</typeparam>
            public class DataSamples
            {

                public List<byte> pcm8;
                public List<Int16> pcm16;

            }


            /// <summary>
            /// Blank constructor.
            /// </summary>
            public DataChunk()
            {



            }


            /// <summary>
            /// Make a new data chunk.
            /// </summary>
            /// <param name="samples">The samples to contain. Is a double array, first index is channel number.</param>
            /// <param name="fmt">Fmt chunk.</param>
            public DataChunk(object samples, FmtChunk fmt)
            {

                channels = new List<DataSamples>();
                if (fmt.bitsPerSample == 8)
                {

                    byte[][] pcm8 = (samples as byte[][]);
                    for (int i = 0; i < fmt.numChannels; i++)
                    {
                        channels.Add(new DataSamples() { pcm8 = pcm8[i].ToList() });
                    }

                }
                else
                {

                    Int16[][] pcm16 = (samples as Int16[][]);
                    for (int i = 0; i < fmt.numChannels; i++)
                    {
                        channels.Add(new DataSamples() { pcm16 = pcm16[i].ToList() });
                    }

                }

            }


            /// <summary>
            /// Load a data chunk.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="fmt">Fmt chunk.</param>
            public void Load(ref BinaryDataReader br, FmtChunk fmt)
            {

                //Call load.
                base.Load(ref br);

                //Create channels.
                channels = new List<DataSamples>();
                for (int i = 0; i < fmt.numChannels; i++)
                {
                    channels.Add(new DataSamples() { pcm8 = new List<byte>(), pcm16 = new List<Int16>() });
                }

                //Read each sample frame.
                for (int i = 0; i < size / fmt.blockAlign; i++)
                {

                    //Read each channel.
                    for (int j = 0; j < fmt.numChannels; j++)
                    {

                        if (fmt.bitsPerSample == FmtChunk.BitsPerSample.PCM8)
                        {

                            channels[j].pcm8.Add(br.ReadByte());

                        }
                        else if (fmt.bitsPerSample == FmtChunk.BitsPerSample.PCM16)
                        {

                            channels[j].pcm16.Add(br.ReadInt16());

                        }

                    }

                }

            }


            /// <summary>
            /// Update the data chunk.
            /// </summary>
            /// <param name="fmt">Fmt chunk.</param>
            public void Update(FmtChunk fmt)
            {

                //Call update.
                if (fmt.bitsPerSample == FmtChunk.BitsPerSample.PCM8) { base.Update("data", (UInt32)(fmt.blockAlign * channels[0].pcm8.Count())); }
                else if (fmt.bitsPerSample == FmtChunk.BitsPerSample.PCM16) { base.Update("data", (UInt32)(fmt.blockAlign * channels[0].pcm16.Count())); }

            }


            /// <summary>
            /// Write the data chunk.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="fmt">Fmt chunk.</param>
            public void Write(ref BinaryDataWriter bw, FmtChunk fmt)
            {

                //Call write.
                base.Write(ref bw);

                //Write each sample.
                if (fmt.bitsPerSample == FmtChunk.BitsPerSample.PCM8)
                {

                    for (int i = 0; i < channels[0].pcm8.Count(); i++)
                    {

                        //Write each channel.
                        for (int j = 0; j < fmt.numChannels; j++)
                        {
                            bw.Write(channels[j].pcm8[i]);
                        }

                    }

                }
                else if (fmt.bitsPerSample == FmtChunk.BitsPerSample.PCM16)
                {

                    for (int i = 0; i < channels[0].pcm16.Count(); i++)
                    {

                        //Write each channel.
                        for (int j = 0; j < fmt.numChannels; j++)
                        {
                            bw.Write(channels[j].pcm16[i]);
                        }

                    }

                }

            }


        }


        /// <summary>
        /// Sample chunk.
        /// </summary>
        public class SmplChunk : Chunk
        {

            /// <summary>
            /// 3 - V0.
            /// </summary>
            public UInt32 v0;

            /// <summary>
            /// 4 - V1.
            /// </summary>
            public UInt32 v1;

            /// <summary>
            /// 5 - V2.
            /// </summary>
            public UInt32 v2;

            /// <summary>
            /// 6 - V3.
            /// </summary>
            public UInt32 v3;

            /// <summary>
            /// 7 - V4.
            /// </summary>
            public UInt32 v4;

            /// <summary>
            /// 8 - V5.
            /// </summary>
            public UInt32 v5;

            /// <summary>
            /// 9 - V6.
            /// </summary>
            public UInt32 v6;

            /// <summary>
            /// 10 - Number of loops.
            /// </summary>
            public UInt32 numLoops;

            /// <summary>
            /// 11 - Size in bytes the loops take up.
            /// </summary>
            public UInt32 samplerDataSize;

            /// <summary>
            /// 12 - The loops.
            /// </summary>
            public List<Loop> loops;


            /// <summary>
            /// A smpl loop.
            /// </summary>
            public class Loop
            {

                /// <summary>
                /// 1 - Cue identifier. Aka some shit I don't care about.
                /// </summary>
                public UInt32 identifier;

                /// <summary>
                /// 2 - Loop type. Just do 0 for everyone's sanity.
                /// </summary>
                public UInt32 type;

                /// <summary>
                /// 3 - Starting sample of the loop.
                /// </summary>
                public UInt32 startSample;

                /// <summary>
                /// 4 - Ending sample of the loop.
                /// </summary>
                public UInt32 endSample;

                /// <summary>
                /// 5 - Fraction. Again, just 0. Who needs a fraction of a sample???
                /// </summary>
                public UInt32 fraction;

                /// <summary>
                /// 6 - Number of times to play the loop. Don't ask, 0 when in doubt.
                /// </summary>
                public UInt32 playCount;

            }


            /// <summary>
            /// Bank constructor.
            /// </summary>
            public SmplChunk()
            {



            }


            /// <summary>
            /// Smpl chunk constructor.
            /// </summary>
            /// <param name="loopStart">The starting sample.</param>
            /// <param name="loopEnd">The ending sample.</param>
            public SmplChunk(UInt32 loopStart, UInt32 loopEnd)
            {

                numLoops = 1;
                loops = new List<Loop>();
                loops.Add(new Loop { startSample = loopStart, endSample = loopEnd });

            }


            /// <summary>
            /// Read a smpl chunk.
            /// </summary>
            /// <param name="br">The reader.</param>
            public new void Load(ref BinaryDataReader br)
            {

                //Call load.
                base.Load(ref br);

                //Read the chunk.
                br.ReadUInt32s(7);
                numLoops = br.ReadUInt32();
                samplerDataSize = br.ReadUInt32();
                loops = new List<Loop>();

                for (int i = 0; i < numLoops; i++)
                {

                    Loop l = new Loop();

                    br.ReadUInt32s(2);
                    l.startSample = br.ReadUInt32();
                    l.endSample = br.ReadUInt32();
                    br.ReadUInt32s(2);

                    loops.Add(l);

                }

            }


            /// <summary>
            /// Update the smpl chunk.
            /// </summary>
            public void Update()
            {

                v0 = 0;
                v1 = 0;
                v2 = 0;
                v3 = 0;
                v4 = 0;
                v5 = 0;
                v6 = 0;
                numLoops = (UInt32)loops.Count();
                samplerDataSize = numLoops * 24;

                for (int i = 0; i < numLoops; i++)
                {

                    loops[i].fraction = 0;
                    loops[i].identifier = 0;
                    loops[i].playCount = 0;
                    loops[i].type = 0;

                }

                //Call update.
                base.Update("smpl", samplerDataSize + 36);

            }


            /// <summary>
            /// Write the smpl chunk.
            /// </summary>
            /// <param name="bw">The writer.</param>
            public new void Write(ref BinaryDataWriter bw)
            {

                //Call write.
                base.Write(ref bw);

                bw.Write(new UInt32[7]);
                bw.Write(numLoops);
                bw.Write(samplerDataSize);

                foreach (Loop l in loops)
                {

                    bw.Write(new UInt32[2]);
                    bw.Write(l.startSample);
                    bw.Write(l.endSample);
                    bw.Write(new UInt32[2]);

                }

            }

        }


        /// <summary>
        /// Load a RIFF WAVE.
        /// </summary>
        /// <param name="b">The file.</param>
        public void Load(byte[] b)
        {

            //Reader.
            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);

            magic = new string(br.ReadChars(4));
            chunkSize = br.ReadUInt32();
            format = new string(br.ReadChars(4));

            //The chunks.
            fmt = new FmtChunk();
            data = new DataChunk();

            fmt.Load(ref br);
            data.Load(ref br, fmt);

            while (br.Position <= (b.Length - 4))
            {

                string magic = new string(br.ReadChars(4));
                if (magic == "smpl")
                {
                    br.Position -= 4;
                    smpl = new SmplChunk();
                    smpl.Load(ref br);
                }

            }

        }


        /// <summary>
        /// Update the RIFF WAVE.
        /// </summary>
        public void Update()
        {

            magic = "RIFF";
            format = "WAVE";

            fmt.Update();
            data.Update(fmt);
            if (smpl != null) { smpl.Update(); }

            chunkSize = fmt.size + data.size + 20;
            if (smpl != null) { chunkSize += smpl.size + 8; }

        }


        /// <summary>
        /// Convert to bytes.
        /// </summary>
        /// <returns>A file.</returns>
        public byte[] ToBytes()
        {

            //The writer.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);

            //Update.
            Update();

            bw.Write(magic.ToCharArray());
            bw.Write(chunkSize);
            bw.Write(format.ToCharArray());

            //Chunks.
            fmt.Write(ref bw);
            data.Write(ref bw, fmt);
            if (smpl != null) { smpl.Write(ref bw); }

            //Return file.
            return o.ToArray();

        }


    }


    /// <summary>
    /// Create a RIFF Wave painlessly.
    /// </summary>
    public static class RiffWaveFactory
    {


        /// <summary>
        /// Make a standard RIFF Wave.
        /// </summary>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="bytesPerSample">Number of bytes per sample.</param>
        /// <param name="samples">Array of samples, can be byte[][] or Int16[][]. First index is channel number.</param>
        /// <returns></returns>
        public static RiffWave CreateRiffWave(UInt32 sampleRate, UInt16 bytesPerSample, object samples)
        {

            RiffWave r = new RiffWave();

            byte[][] pcm8;
            Int16[][] pcm16;

            UInt16 numChannels = 1;
            if (bytesPerSample == 1)
            {

                pcm8 = (samples as byte[][]);
                numChannels = (UInt16)pcm8.Length;

            }
            else
            {

                pcm16 = (samples as Int16[][]);
                numChannels = (UInt16)pcm16.Length;

            }

            r.fmt = new RiffWave.FmtChunk(numChannels, sampleRate, bytesPerSample);
            r.data = new RiffWave.DataChunk(samples, r.fmt);
            r.smpl = null;

            return r;

        }


        /// <summary>
        /// Make a looping RIFF Wave.
        /// </summary>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="bytesPerSample">Number of bytes per sample.</param>
        /// <param name="samples">Array of samples, can be byte[][] or Int16[][]. First index is channel number.</param>
        /// <param name="loopStart">Loop start in samples.</param>
        /// <param name="loopEnd">Loop end in samples.</param>
        /// <returns></returns>
        public static RiffWave CreateRiffWave(UInt32 sampleRate, UInt16 bytesPerSample, object samples, UInt32 loopStart, UInt32 loopEnd)
        {

            RiffWave r = CreateRiffWave(sampleRate, bytesPerSample, samples);

            r.smpl = new RiffWave.SmplChunk(loopStart, loopEnd);

            return r;

        }


        /// <summary>
        /// Make a RIFF Wave from a b_wav.
        /// </summary>
        /// <param name="b">The b_wav.</param>
        /// <returns></returns>
        public static RiffWave CreateRiffWave(b_wav b)
        {

            RiffWave r = new RiffWave();
            UInt16 bytesPerSample = 2;
            if (b.info.encoding == EncodingTypes.PCM8) { bytesPerSample = 1; }

            //Non-looping.
            if (!b.info.isLoop)
            {

                r = CreateRiffWave(b.info.sampleRate, bytesPerSample, b.data.GetDataWAV(b.info));

            }

            //Looping.
            else
            {

                r = CreateRiffWave(b.info.sampleRate, bytesPerSample, b.data.GetDataWAV(b.info), b.info.loopStart, b.info.loopEnd);

            }

            return r;

        }

        /// <summary>
        /// Create a RIFF wave from a binary wave.
        /// </summary>
        /// <param name="b">Binary wave.</param>
        /// <returns>The binary wave.</returns>
        public static RiffWave CreateRiffWave(BinaryWave b) {

            RiffWave r = new RiffWave();
            UInt16 bytesPerSample = 2;

            //Non-looping.
            if (!b.Loops) {

                r = CreateRiffWave(b.SampleRate, bytesPerSample, b.Data.GetDataWAV(b.DspAdpcmInfo, b.LoopEndSample));

            }

            //Looping.
            else {

                r = CreateRiffWave(b.SampleRate, bytesPerSample, b.Data.GetDataWAV(b.DspAdpcmInfo, b.LoopEndSample), b.LoopStartSample, b.LoopEndSample);

            }

            return r;

        }


        /// <summary>
        /// Make a RIFF Wave from a b_stm.
        /// </summary>
        /// <param name="b">The b_stm.</param>
        /// <returns></returns>
        public static RiffWave CreateRiffWave(b_stm b)
        {

            RiffWave r = new RiffWave();
            UInt16 bytesPerSample = 2;
            if (b.info.streamSoundInfo.encoding == EncodingTypes.PCM8) { bytesPerSample = 1; }

            //Non-looping.
            if (!b.info.streamSoundInfo.isLoop)
            {

                r = CreateRiffWave(b.info.streamSoundInfo.sampleRate, bytesPerSample, b.data.GetDataSTM(b.info.streamSoundInfo, b.info));

            }

            //Looping.
            else
            {

                r = CreateRiffWave(b.info.streamSoundInfo.sampleRate, bytesPerSample, b.data.GetDataSTM(b.info.streamSoundInfo, b.info), b.info.streamSoundInfo.loopStart, b.info.streamSoundInfo.sampleCount);

            }

            return r;

        }


        /// <summary>
        /// Make a RIFF Wave from a FISP.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static RiffWave CreateRiffWave(FISP f) {

            //New wave.
            RiffWave r = new RiffWave();

            //Not looped.
            if (!f.stream.isLoop) {
                r = RiffWaveFactory.CreateRiffWave(f.stream.sampleRate, 2, f.data.data.ToArray());
            }

            //Looped.
            else {
                r = RiffWaveFactory.CreateRiffWave(f.stream.sampleRate, 2, f.data.data.ToArray(), f.stream.loopStart, f.stream.loopEnd);
            }

            return r;

        }

        /// <summary>
        /// Create a RIFF wave from a vibration.
        /// </summary>
        /// <param name="v">Vibration file.</param>
        /// <returns>The RIFF wave as a vibration.</returns>
        public static RiffWave CreateRiffWave(Vibration v) {

            //New wave.
            RiffWave r = new RiffWave();

            //Not looped.
            if (!v.Loops) {
                r = RiffWaveFactory.CreateRiffWave(200, 1, EncoderFactory.SignedPcm8ToPcm8(new sbyte[][] { v.pcm8 }));
            }

            //Looped.
            else {
                r = RiffWaveFactory.CreateRiffWave(200, 1, EncoderFactory.SignedPcm8ToPcm8(new sbyte[][] { v.pcm8 }), v.LoopStart, v.LoopEnd);
            }

            return r;

        }

    }

}
