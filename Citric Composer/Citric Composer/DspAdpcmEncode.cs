using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    /// <summary>
    /// The encoder.
    /// </summary>
    public class DspAdpcmEncoder
    {

        //Path.
        static string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Encode samples.
        /// </summary>
        /// <param name="samples">PCM16 samples to encode.</param>
        /// <returns></returns>
        public static byte[] EncodeSamples(short[] samples, out DspAdpcmInfo info) {

            //Make a new RIFF.
            short[][] smps = new short[1][];
            smps[0] = samples;
            RiffWave r = RiffWaveFactory.CreateRiffWave(48000, 2, smps);
            File.WriteAllBytes(path + "/Data/Tools/tmp.wav", r.ToBytes());

            //Start conversion.
            Process p = new Process();
            Directory.SetCurrentDirectory(path + "/Data/Tools");
            p.StartInfo.FileName = "FreeDsp.exe";
            p.StartInfo.Arguments = "tmp.wav tmp.dsp";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            //Read data.
            dsp d = new dsp();
            d.load(File.ReadAllBytes("tmp.dsp"));
            byte[] data = d.data;
            Directory.SetCurrentDirectory(path);

            //Create info.
            info = new DspAdpcmInfo();
            info.yn1 = d.yn1;
            info.yn2 = d.yn2;
            info.pred_scale = d.predictor;
            info.loop_pred_scale = d.loopPredictor;
            info.loop_yn1 = d.loopYn1;
            info.loop_yn2 = d.loopYn2;
            info.coefs = new short[8][];
            info.coefs[0] = new short[2];
            info.coefs[1] = new short[2];
            info.coefs[2] = new short[2];
            info.coefs[3] = new short[2];
            info.coefs[4] = new short[2];
            info.coefs[5] = new short[2];
            info.coefs[6] = new short[2];
            info.coefs[7] = new short[2];
            info.coefs[0][0] = d.coefficients[0];
            info.coefs[0][1] = d.coefficients[1];
            info.coefs[1][0] = d.coefficients[2];
            info.coefs[1][1] = d.coefficients[3];
            info.coefs[2][0] = d.coefficients[4];
            info.coefs[2][1] = d.coefficients[5];
            info.coefs[3][0] = d.coefficients[6];
            info.coefs[3][1] = d.coefficients[7];
            info.coefs[4][0] = d.coefficients[8];
            info.coefs[4][1] = d.coefficients[9];
            info.coefs[5][0] = d.coefficients[10];
            info.coefs[5][1] = d.coefficients[11];
            info.coefs[6][0] = d.coefficients[12];
            info.coefs[6][1] = d.coefficients[13];
            info.coefs[7][0] = d.coefficients[14];
            info.coefs[7][1] = d.coefficients[15];

            //Remove temporary files.
            File.Delete("Data/Tools/tmp.wav");
            File.Delete("Data/Tools/tmp.dsp");

            //Return the samples.
            return data;

        }

        //Old stuff.
        /*
       /// <summary>
       /// Correlate coeffiencts.
       /// </summary>
       /// <param name="source">Source of the audio.</param>
       /// <param name="samples">Number of samples.</param>
       /// <param name="coefsOut">The output coefficients.</param>
       public void CorrelateCoefs(short[] source, int samples, ref short[][] coefsOut) {

           int numFrames = (samples + 13) / 14;
           int frameSamples;

           //Block buffer. [2][0x3800]
           short[][] blockBuffer = new short[2][];
           blockBuffer[0] = new short[0x3800];
           blockBuffer[1] = new short[0x3800];

           //[2][14]
           short[][] pcmHistBuffer = new short[2][];
           pcmHistBuffer[0] = new short[14];
           pcmHistBuffer[1] = new short[14];

           short[] vec1 = new short[3];
           short[] vec2 = new short[3];

           short[][] mtx = new short[3][];
           mtx[0] = new short[3];
           mtx[1] = new short[3];
           mtx[2] = new short[3];

           int[] vecIdxs = new int[3];



       }


        /// <summary>
        /// Encode a frame.
        /// </summary>
        /// <param name="pcmInOut"></param>
        /// <param name="sampleCount"></param>
        /// <param name="adpcmOut"></param>
        /// <param name="coefsIn"></param>
        public void DSPEncodeFrame(short[] pcmInOut, int sampleCount, out byte[] adpcmOut, short[][] coefsIn, out Int16 yn1, out Int16 yn2)
        {

            adpcmOut = new byte[8];

            int[][] inSamples = new int[8][];
            for (int i = 0; i < inSamples.Length; i++) {
                inSamples[i] = new int[16];
            }

            int[][] outSamples = new int[8][];
            for (int i = 0; i < outSamples.Length; i++)
            {
                outSamples[i] = new int[14];
            }

            int bestIndex = 0;

            int[] scale = new int[8];
            double[] distAccum = new double[8];

            //Maybe???
            yn1 = pcmInOut[0];
            yn2 = pcmInOut[1];

            //Go through each coeeficient set, finding one with the smallest error.
            for (int i = 0; i < 8; i++) {

                int v1, v2, v3;
                int distance, index;

                //Set yn values
                inSamples[i][0] = pcmInOut[0];
                inSamples[i][1] = pcmInOut[1];

                //Round and clamp samples for this coef set
                distance = 0;
                for (int s = 0; s < sampleCount; s++)
                {
                    //Multiply previous samples by coefs
                    inSamples[i][s + 2] = v1 = ((pcmInOut[s] * coefsIn[i][1]) + (pcmInOut[s + 1] * coefsIn[i][0])) / 2048;
                    //Subtract from current sample
                    v2 = pcmInOut[s + 2] - v1;
                    //Clamp
                    v3 = (v2 >= 32767) ? 32767 : (v2 <= -32768) ? -32768 : v2;
                    //Compare distance
                    if (Math.Abs(v3) > Math.Abs(distance))
                        distance = v3;
                }

                //Set initial scale
                for (scale[i] = 0; (scale[i] <= 12) && ((distance > 7) || (distance < -8)); scale[i]++, distance /= 2) { }
                scale[i] = (scale[i] <= 1) ? -1 : scale[i] - 2;

                do {

                    scale[i]++;
                    distAccum[i] = 0;
                    index = 0;

                    for (int s = 0; s < sampleCount; s++)
                    {
                        //Multiply previous
                        v1 = ((inSamples[i][s] * coefsIn[i][1]) + (inSamples[i][s + 1] * coefsIn[i][0]));
                        //Evaluate from real sample
                        v2 = ((pcmInOut[s + 2] << 11) - v1) / 2048;
                        //Round to nearest sample
                        v3 = (v2 > 0) ? (int)((double)v2 / (1 << scale[i]) + 0.4999999f) : (int)((double)v2 / (1 << scale[i]) - 0.4999999f);

                        //Clamp sample and set index
                        if (v3 < -8)
                        {
                            if (index < (v3 = -8 - v3))
                                index = v3;
                            v3 = -8;
                        }
                        else if (v3 > 7)
                        {
                            if (index < (v3 -= 7))
                                index = v3;
                            v3 = 7;
                        }

                        //Store result
                        outSamples[i][s] = v3;

                        //Round and expand
                        v1 = (v1 + ((v3 * (1 << scale[i])) << 11) + 1024) >> 11;
                        //Clamp and store
                        inSamples[i][s + 2] = v2 = (v1 >= 32767) ? 32767 : (v1 <= -32768) ? -32768 : v1;
                        //Accumulate distance
                        v3 = pcmInOut[s + 2] - v2;
                        distAccum[i] += v3 * (double)v3;
                    }

                    for (int x = index + 8; x > 256; x >>= 1)
                        if (++scale[i] >= 12)
                            scale[i] = 11;

                } while ((scale[i] < 12) && (index > 1));

            }

            double min = double.MaxValue;
            for (int i = 0; i < 8; i++)
            {
                if (distAccum[i] < min)
                {
                    min = distAccum[i];
                    bestIndex = i;
                }
            }

            //Write converted samples
            for (int s = 0; s < sampleCount; s++)
                pcmInOut[s + 2] = (short)inSamples[bestIndex][s + 2];

            //Write ps
            adpcmOut[0] = (byte)((bestIndex << 4) | (scale[bestIndex] & 0xF));

            //Zero remaining samples
            for (int s = sampleCount; s < 14; s++)
                outSamples[bestIndex][s] = 0;

            //Write output samples
            for (int y = 0; y < 7; y++)
            {
                adpcmOut[y + 1] = (byte)((outSamples[bestIndex][y * 2] << 4) | (outSamples[bestIndex][y * 2 + 1] & 0xF));
            }

        }*/

    }

    //DSP File.
    public class dsp
    {

        public UInt32 numSamples; //Wave data size divided by 4.
        public UInt32 adpcmNibbles; //ADPCM data size times 2.
        public UInt32 sampleRate; //Sampling rate of the data, expressed in Hertz. Used for WAV/AIFF header generation during decode.

        public UInt16 loopFlag; //Specifies whether the sample is looped. This parameter is stored in big-endian format and is used by the DSP for sample playback.
        public UInt16 format; //Always 0.
        public UInt32 loopStart; //2 for not looped. Is an offset.
        public UInt32 loopEnd; //Is the end offset. (Just use adpcmNibbles - 1)
        public UInt32 always2; //Always 2 because Nintendo logic.
        public Int16[] coefficients; //16 coefficients.

        public UInt16 gain; //Always 0.
        public UInt16 predictor; //Predictor scale.
        public Int16 yn1; //Yn1.
        public Int16 yn2; //Yn1.

        public UInt16 loopPredictor; //Loop Predictor.
        public Int16 loopYn1; //Yn1.
        public Int16 loopYn2; //Yn1.

        public UInt16 channelCount; //Channel count. 0
        public UInt16 blockFrameCount; //Block frame count. 0

        public UInt16[] padding; //9 Paddings.

        public byte[] data; //Remaining ADPCM data.


        /// <summary>
        /// Load a dsp file.
        /// </summary>
        /// <param name="b"></param>
        public void load(byte[] b)
        {

            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;

            numSamples = br.ReadUInt32();
            adpcmNibbles = br.ReadUInt32();
            sampleRate = br.ReadUInt32();

            loopFlag = br.ReadUInt16();
            format = br.ReadUInt16();
            loopStart = br.ReadUInt32();
            loopEnd = br.ReadUInt32();
            always2 = br.ReadUInt32();
            coefficients = br.ReadInt16s(16);

            gain = br.ReadUInt16();
            predictor = br.ReadUInt16();
            yn1 = br.ReadInt16();
            yn2 = br.ReadInt16();

            loopPredictor = br.ReadUInt16();
            loopYn1 = br.ReadInt16();
            loopYn2 = br.ReadInt16();

            channelCount = br.ReadUInt16();
            blockFrameCount = br.ReadUInt16();

            padding = br.ReadUInt16s(9);

            data = br.ReadBytes((int)adpcmNibbles / 2);

        }


        /// <summary>
        /// Write a dsp file.
        /// </summary>
        /// <param name="b"></param>
        public byte[] toBytes()
        {

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;

            bw.Write(numSamples);
            bw.Write(adpcmNibbles);
            bw.Write(sampleRate);

            bw.Write(loopFlag);
            bw.Write(format);
            bw.Write(loopStart);
            bw.Write(loopEnd);
            bw.Write(always2);
            bw.Write(coefficients);

            bw.Write(gain);
            bw.Write(predictor);
            bw.Write(yn1);
            bw.Write(yn2);

            bw.Write(loopPredictor);
            bw.Write(loopYn1);
            bw.Write(loopYn2);

            bw.Write(channelCount);
            bw.Write(blockFrameCount);

            bw.Write(padding);

            bw.Write(data);

            return o.ToArray();

        }

    }

}
