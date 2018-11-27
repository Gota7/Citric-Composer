using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VGAudio.Utilities;

namespace CitraFileLoader
{

    /// <summary>
    /// DSP-ADPCM info.
    /// </summary>
    public class DspAdpcmInfo
    {

        /// <summary>
        /// 1 - [8][2] array of coefficients.
        /// </summary>
        public Int16[][] coefs;

        /// <summary>
        /// 2 - Predictor scale.
        /// </summary>
        public UInt16 pred_scale;

        /// <summary>
        /// 3 - History 1.
        /// </summary>
        public Int16 yn1;

        /// <summary>
        /// 4 - History 2.
        /// </summary>
        public Int16 yn2;

        /// <summary>
        /// 5 - Loop predictor scale.
        /// </summary>
        public UInt16 loop_pred_scale;

        /// <summary>
        /// 6 - Loop history 1.
        /// </summary>
        public Int16 loop_yn1;

        /// <summary>
        /// 7 - Loop history 2.
        /// </summary>
        public Int16 loop_yn2;

    }


    /// <summary>
    /// SoundN'Stream data block.
    /// </summary>
    public class SoundNStreamDataBlock
    {

        /// <summary>
        /// Read a soundN'Stream block.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="info">Stream info.</param>
        /// <param name="seek">Seek info for history.</param>
        public SoundNStreamDataBlock(ref BinaryDataReader br, b_stm.InfoBlock info)
        {

            //Read magic.
            br.ReadUInt32s(2);

            br.Position += info.streamSoundInfo.sampleDataOffset.offset;

            //Read the channels.
            switch (info.streamSoundInfo.encoding) {

                //PCM8.
                case EncodingTypes.PCM8:

                    //New data array.
                    pcm8 = new sbyte[info.channels.Count()][];

                    //Read channel blocks.
                    List<sbyte>[] channelData = new List<sbyte>[info.channels.Count()];

                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        channelData[j] = new List<sbyte>();

                    }

                    for (int i = 0; i < info.streamSoundInfo.blockCount - 1; i++)
                    {

                        //Read data.
                        for (int j = 0; j < info.channels.Count; j++) {

                            channelData[j].AddRange(br.ReadSBytes((int)info.streamSoundInfo.oneBlockBytesize));

                        }

                    }

                    //Read last block.
                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        channelData[j].AddRange(br.ReadSBytes((int)info.streamSoundInfo.lastBlockBytesize));
                        br.ReadBytes((int)(info.streamSoundInfo.lastBlockPaddedBytesize - info.streamSoundInfo.lastBlockBytesize));

                    }

                    //Convert data.
                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        pcm8[j] = channelData[j].ToArray();

                    }

                    break;

                //PCM16.
                case EncodingTypes.PCM16:

                    //New data array.
                    pcm16 = new Int16[info.channels.Count()][];

                    //Read channel blocks.
                    List<Int16>[] channelData2 = new List<Int16>[info.channels.Count()];

                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        channelData2[j] = new List<Int16>();

                    }

                    for (int i = 0; i < info.streamSoundInfo.blockCount - 1; i++)
                    {

                        //Read data.
                        for (int j = 0; j < info.channels.Count; j++)
                        {

                            channelData2[j].AddRange(br.ReadInt16s((int)info.streamSoundInfo.oneBlockSamples));

                        }

                    }

                    //Read last block.
                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        channelData2[j].AddRange(br.ReadInt16s((int)info.streamSoundInfo.lastBlockSamples));
                        br.ReadBytes((int)(info.streamSoundInfo.lastBlockPaddedBytesize - info.streamSoundInfo.lastBlockBytesize));

                    }

                    //Convert data.
                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        pcm16[j] = channelData2[j].ToArray();

                    }

                    break;

                //DSP-ADPCM.
                case EncodingTypes.DSP_ADPCM:

                    //New data array.
                    dspAdpcm = new byte[info.channels.Count()][];

                    //Read channel blocks.
                    List<byte>[] channelData3 = new List<byte>[info.channels.Count()];

                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        channelData3[j] = new List<byte>();

                    }

                    for (int i = 0; i < info.streamSoundInfo.blockCount - 1; i++)
                    {

                        //Read data.
                        for (int j = 0; j < info.channels.Count; j++)
                        {

                            channelData3[j].AddRange(br.ReadBytes((int)info.streamSoundInfo.oneBlockBytesize));

                        }

                    }

                    //Read last block.
                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        channelData3[j].AddRange(br.ReadBytes((int)info.streamSoundInfo.lastBlockBytesize));
                        br.ReadBytes((int)(info.streamSoundInfo.lastBlockPaddedBytesize - info.streamSoundInfo.lastBlockBytesize));

                    }

                    //Convert data.
                    for (int j = 0; j < info.channels.Count; j++)
                    {

                        dspAdpcm[j] = channelData3[j].ToArray();

                    }

                    break;

            }

        }


        /// <summary>
        /// Read a soundN'Stream block from a b_wav.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="wavInfo"></param>
        public SoundNStreamDataBlock(ref BinaryDataReader br, b_wav.InfoBlock wavInfo)
        {

            //Read magic.
            br.ReadUInt32();
            UInt32 size = br.ReadUInt32();

            //Set data base postion.
            long dataBasePosition = br.Position;

            //Read the channels.
            switch (wavInfo.encoding)
            {

                //PCM8.
                case EncodingTypes.PCM8:

                    //Make new data array.
                    pcm8 = new sbyte[wavInfo.channelInfo.Count()][];

                    //See how big the length of each channel is.
                    int channelSize8 = (int)(size - wavInfo.channelInfo[pcm8.Length - 1].samplesRef.offset) - 8;

                    //Read channels.
                    for (int i = 0; i < pcm8.Count(); i++)
                    {

                        //Set postion.
                        br.Position = wavInfo.channelInfo[i].samplesRef.offset + dataBasePosition;

                        //Read data.
                        pcm8[i] = br.ReadSBytes(channelSize8);

                    }
                    break;


                //PCM16.
                case EncodingTypes.PCM16:

                    //Make new data array.
                    pcm16 = new short[wavInfo.channelInfo.Count()][];

                    //See how big the length of each channel is.
                    int channelSize16 = (int)((size - wavInfo.channelInfo[pcm16.Length - 1].samplesRef.offset) - 8) / 2;

                    //Read channels.
                    for (int i = 0; i < pcm16.Count(); i++)
                    {

                        //Set postion.
                        br.Position = wavInfo.channelInfo[i].samplesRef.offset + dataBasePosition;

                        //Read data.
                        pcm16[i] = br.ReadInt16s(channelSize16);

                    }
                    break;


                //DSP-ADPCM.
                case EncodingTypes.DSP_ADPCM:

                    //Make new data array.
                    dspAdpcm = new byte[wavInfo.channelInfo.Count()][];

                    //See how big the length of each channel is.
                    int channelSize4 = (int)(size - wavInfo.channelInfo[dspAdpcm.Length - 1].samplesRef.offset) - 8;

                    //Read channels.
                    for (int i = 0; i < dspAdpcm.Count(); i++)
                    {

                        //Set postion.
                        br.Position = wavInfo.channelInfo[i].samplesRef.offset + dataBasePosition;

                        //Read data.
                        dspAdpcm[i] = br.ReadBytes(channelSize4);

                    }
                    break;

            }

        }


        /// <summary>
        /// Make a new data block.
        /// </summary>
        /// <param name="samples">The samples.</param>
        /// <param name="encoding">The encoding type of the samples.</param>
        public SoundNStreamDataBlock(object samples, byte encoding)
        {

            switch (encoding)
            {

                case EncodingTypes.PCM8:
                    pcm8 = samples as sbyte[][];
                    break;

                case EncodingTypes.PCM16:
                    pcm16 = samples as Int16[][];
                    break;

                case EncodingTypes.DSP_ADPCM:
                    dspAdpcm = samples as byte[][];
                    break;

            }

        }


        /// <summary>
        /// Get data from a b_wav.
        /// </summary>
        /// <param name="wavInfo"></param>
        /// <returns></returns>
        public object GetDataWAV(b_wav.InfoBlock wavInfo)
        {

            object returnValue = null;

            //See encoding.
            switch (wavInfo.encoding)
            {

                case EncodingTypes.PCM8:
                    return EncoderFactory.SignedPcm8ToPcm8(pcm8);

                case EncodingTypes.PCM16:
                    return pcm16;

                case EncodingTypes.DSP_ADPCM:
                    List<DspAdpcmInfo> context = new List<DspAdpcmInfo>();
                    foreach (b_wav.InfoBlock.ChannelInfo c in wavInfo.channelInfo)
                    {
                        context.Add(c.dspAdpcmInfo);
                    }
                    return EncoderFactory.DspApcmToPcm16(dspAdpcm, wavInfo.loopEnd, context.ToArray());

            }

            return returnValue;

        }


        /// <summary>
        /// Get data from a b_stm.
        /// </summary>
        /// <param name="wavInfo"></param>
        /// <returns></returns>
        public object GetDataSTM(b_stm.StreamSoundInfo wavInfo, b_stm.InfoBlock info)
        {

            object returnValue = null;

            //See encoding.
            switch (wavInfo.encoding)
            {

                case EncodingTypes.PCM8:
                    return EncoderFactory.SignedPcm8ToPcm8(pcm8);

                case EncodingTypes.PCM16:
                    return pcm16;

                case EncodingTypes.DSP_ADPCM:
                    List<DspAdpcmInfo> context = new List<DspAdpcmInfo>();
                    foreach (b_stm.ChannelInfo c in info.channels)
                    {
                        context.Add(c.dspAdpcmInfo);
                    }
                    return EncoderFactory.DspApcmToPcm16(dspAdpcm, wavInfo.sampleCount, context.ToArray());

            }

            return returnValue;

        }


        /// <summary>
        /// Get the size of the data block.
        /// </summary>
        /// <returns></returns>
        public UInt32 GetSize(byte encoding, ref b_wav.InfoBlock wavInfo)
        {

            UInt32 size = 0x20;

            switch (encoding)
            {

                case EncodingTypes.PCM8:
                    size += (UInt32)(pcm8.Length * pcm8[0].Length);
                    int padd0 = pcm8[0].Length;
                    while ((padd0) % 0x20 != 0)
                    {
                        padd0 += 1;
                    }
                    padd0 -= pcm8[0].Length;
                    size += (UInt32)(padd0 * (pcm8.Length - 1));
                    break;

                case EncodingTypes.PCM16:
                    size += (UInt32)(pcm16.Length * pcm16[0].Length * 2);
                    int padd1 = pcm16[0].Length * 2;
                    while ((padd1) % 0x20 != 0)
                    {
                        padd1 += 1;
                    }
                    padd1 -= (pcm16[0].Length * 2);
                    size += (UInt32)(padd1 * (pcm16.Length - 1));
                    break;

                case EncodingTypes.DSP_ADPCM:
                    size += (UInt32)(dspAdpcm.Length * dspAdpcm[0].Length);
                    int padd2 = dspAdpcm[0].Length;
                    while ((padd2) % 0x20 != 0)
                    {
                        padd2 += 1;
                    }
                    padd2 -= dspAdpcm[0].Length;
                    size += (UInt32)(padd2 * (dspAdpcm.Length - 1));
                    break;

            }


            //Update offsets in b_wav file.
            if (wavInfo != null)
            {

                Int32 wavOffset = 0x18;
                wavInfo.channelInfoRefTable = new ReferenceTable(new List<Reference>());
                wavInfo.channelInfoRefTable.count = (UInt32)wavInfo.channelInfo.Count();
                switch (encoding)
                {

                    //PCM8.
                    case EncodingTypes.PCM8:

                        for (int i = 0; i < wavInfo.channelInfo.Count(); i++)
                        {

                            //New reference.
                            wavInfo.channelInfo[i].samplesRef = new Reference(ReferenceTypes.General, wavOffset);
                            wavOffset += pcm8[i].Length;
                            while ((wavOffset + 8) % 0x20 != 0)
                            {
                                wavOffset += 1;
                            }

                        }

                        break;

                    //PCM16.
                    case EncodingTypes.PCM16:

                        for (int i = 0; i < wavInfo.channelInfo.Count(); i++)
                        {

                            //New reference.
                            wavInfo.channelInfo[i].samplesRef = new Reference(ReferenceTypes.General, wavOffset);
                            wavOffset += pcm16[i].Length * 2;
                            while ((wavOffset + 8) % 0x20 != 0)
                            {
                                wavOffset += 1;
                            }

                        }

                        break;

                    //DSP-ADPCM.
                    case EncodingTypes.DSP_ADPCM:

                        for (int i = 0; i < wavInfo.channelInfo.Count(); i++)
                        {

                            //New reference.
                            wavInfo.channelInfo[i].samplesRef = new Reference(ReferenceTypes.General, wavOffset);
                            wavOffset += dspAdpcm[i].Length;
                            while ((wavOffset + 8) % 0x20 != 0)
                            {
                                wavOffset += 1;
                            }

                        }

                        break;

                }

            }


            return size;

        }


        /// <summary>
        /// Get the size of the data block.
        /// </summary>
        /// <returns></returns>
        public UInt32 GetSize(byte encoding, ref b_stm.InfoBlock wavInfo)
        {

            UInt32 size = 0x20;

            switch (encoding)
            {

                case EncodingTypes.PCM8:
                    size += (UInt32)(pcm8.Length * pcm8[0].Length);
                    int padd0 = pcm8[0].Length;
                    while ((padd0) % 0x20 != 0)
                    {
                        padd0 += 1;
                    }
                    padd0 -= pcm8[0].Length;
                    size += (UInt32)(padd0 * pcm8.Length);
                    break;

                case EncodingTypes.PCM16:
                    size += (UInt32)(pcm16.Length * pcm16[0].Length * 2);
                    int padd1 = pcm16[0].Length * 2;
                    while ((padd1) % 0x20 != 0)
                    {
                        padd1 += 1;
                    }
                    padd1 -= (pcm16[0].Length * 2);
                    size += (UInt32)(padd1 * pcm16.Length);
                    break;

                case EncodingTypes.DSP_ADPCM:
                    size += (UInt32)(dspAdpcm.Length * dspAdpcm[0].Length);
                    int padd2 = dspAdpcm[0].Length;
                    while ((padd2) % 0x20 != 0)
                    {
                        padd2 += 1;
                    }
                    padd2 -= dspAdpcm[0].Length;
                    size += (UInt32)(padd2 * dspAdpcm.Length);
                    break;

            }

            return size;

        }


        /// <summary>
        /// Write for BFWAV.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="wavInfo"></param>
        public void WriteWAV(ref BinaryDataWriter bw, b_wav.InfoBlock wavInfo)
        {

            bw.Write("DATA".ToCharArray());
            bw.Write(GetSize(wavInfo.encoding, ref wavInfo));
            bw.Write(new byte[0x18]);

            //Write data.
            for (int i = 0; i < wavInfo.channelInfo.Count(); i++)
            {

                switch (wavInfo.encoding)
                {

                    //PCM8.
                    case EncodingTypes.PCM8:
                        foreach (sbyte s in pcm8[i])
                        {
                            bw.Write(s);
                        }
                        break;

                    //PCM16.
                    case EncodingTypes.PCM16:
                        bw.Write(pcm16[i]);
                        break;

                    //DSP-ADPCM.
                    case EncodingTypes.DSP_ADPCM:
                        bw.Write(dspAdpcm[i]);
                        break;

                }

                //Padding.
                if (i != wavInfo.channelInfo.Count() - 1)
                {
                    while ((bw.Position % 0x20) != 0)
                    {
                        bw.Write((byte)0);
                    }
                }

            }

        }

        /// <summary>
        /// Write for BFSTM.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="wavInfo"></param>
        public void WriteSTM(ref BinaryDataWriter bw, b_stm.InfoBlock wavInfo, UInt32 numSamples) {

            bw.Write("DATA".ToCharArray());
            bw.Write(GetSize(wavInfo.streamSoundInfo.encoding, ref wavInfo));
            bw.Write(new byte[0x18]);

            //Update data.
            wavInfo.streamSoundInfo.oneBlockBytesize = 0x2000;
            switch (wavInfo.streamSoundInfo.encoding) {

                //PCM8.
                case EncodingTypes.PCM8:
                    wavInfo.streamSoundInfo.oneBlockSamples = wavInfo.streamSoundInfo.seekInfoIntervalSamples = 0x2000;
                    wavInfo.streamSoundInfo.lastBlockSamples = (uint)pcm8[0].Length % 0x2000;
                    wavInfo.streamSoundInfo.lastBlockBytesize = wavInfo.streamSoundInfo.lastBlockPaddedBytesize = (uint)pcm8[0].Length % 0x2000;
                    while (wavInfo.streamSoundInfo.lastBlockPaddedBytesize % 0x20 != 0) {
                        wavInfo.streamSoundInfo.lastBlockPaddedBytesize++;
                    }
                    wavInfo.streamSoundInfo.blockCount = (uint)pcm8[0].Length / 0x2000 + 1;
                    break;

                //PCM16.
                case EncodingTypes.PCM16:
                    wavInfo.streamSoundInfo.oneBlockSamples = wavInfo.streamSoundInfo.seekInfoIntervalSamples = 0x1000;
                    wavInfo.streamSoundInfo.lastBlockSamples = (uint)pcm16[0].Length % 0x1000;
                    wavInfo.streamSoundInfo.lastBlockBytesize = wavInfo.streamSoundInfo.lastBlockPaddedBytesize = (uint)pcm16[0].Length % 0x2000;
                    while (wavInfo.streamSoundInfo.lastBlockPaddedBytesize % 0x20 != 0)
                    {
                        wavInfo.streamSoundInfo.lastBlockPaddedBytesize++;
                    }
                    wavInfo.streamSoundInfo.blockCount = (uint)pcm16[0].Length / 0x1000 + 1;
                    break;

                //DSP-ADPCM.
                case EncodingTypes.DSP_ADPCM:
                    wavInfo.streamSoundInfo.oneBlockSamples = wavInfo.streamSoundInfo.seekInfoIntervalSamples = 0x3800;
                    wavInfo.streamSoundInfo.lastBlockSamples = (uint)numSamples % 0x3800;
                    wavInfo.streamSoundInfo.lastBlockBytesize = wavInfo.streamSoundInfo.lastBlockPaddedBytesize = (uint)dspAdpcm[0].Length % 0x2000;
                    while (wavInfo.streamSoundInfo.lastBlockPaddedBytesize % 0x20 != 0)
                    {
                        wavInfo.streamSoundInfo.lastBlockPaddedBytesize++;
                    }
                    wavInfo.streamSoundInfo.blockCount = (uint)dspAdpcm[0].Length / 0x2000 + 1;
                    wavInfo.streamSoundInfo.sampleCount = numSamples;
                    break;

            }

            //Chunk the data first.
            List<byte>[][] dspAdpcmBlocks = new List<byte>[wavInfo.channels.Count()][];
            List<Int16>[][] pcm16Blocks = new List<Int16>[wavInfo.channels.Count()][];
            List<sbyte>[][] pcm8Blocks = new List<sbyte>[wavInfo.channels.Count()][];
            for (int c = 0; c < wavInfo.channels.Count(); c++)
            {

                dspAdpcmBlocks[c] = new List<byte>[(int)wavInfo.streamSoundInfo.blockCount];
                pcm16Blocks[c] = new List<Int16>[(int)wavInfo.streamSoundInfo.blockCount];
                pcm8Blocks[c] = new List<sbyte>[(int)wavInfo.streamSoundInfo.blockCount];

                for (int i = 0; i < wavInfo.streamSoundInfo.blockCount; i++)
                {

                    pcm8Blocks[c][i] = new List<sbyte>();
                    pcm16Blocks[c][i] = new List<short>();
                    dspAdpcmBlocks[c][i] = new List<byte>();

                    //Normal block.
                    if (i != wavInfo.streamSoundInfo.blockCount - 1)
                    {

                        //Add to block.
                        switch (wavInfo.streamSoundInfo.encoding)
                        {

                            //PCM8.
                            case EncodingTypes.PCM8:
                                for (int j = i * 0x2000; j < i * 0x2000 + 0x2000; j++)
                                {
                                    pcm8Blocks[c][i].Add(pcm8[c][j]);
                                }
                                break;

                            //PCM16.
                            case EncodingTypes.PCM16:
                                for (int j = i * 0x1000; j < i * 0x1000 + 0x1000; j++)
                                {
                                    pcm16Blocks[c][i].Add(pcm16[c][j]);
                                }
                                break;

                            //DSP-ADPCM.
                            case EncodingTypes.DSP_ADPCM:
                                for (int j = i * 0x2000; j < i * 0x2000 + 0x2000; j++)
                                {
                                    dspAdpcmBlocks[c][i].Add(dspAdpcm[c][j]);
                                }
                                break;

                        }

                    }

                    //Last block.
                    else
                    {

                        //Add to block.
                        switch (wavInfo.streamSoundInfo.encoding)
                        {

                            //PCM8.
                            case EncodingTypes.PCM8:
                                for (int j = i * 0x2000; j < pcm8[0].Length; j++)
                                {
                                    pcm8Blocks[c][i].Add(pcm8[c][j]);
                                }
                                break;

                            //PCM16.
                            case EncodingTypes.PCM16:
                                for (int j = i * 0x1000; j < pcm16[0].Length; j++)
                                {
                                    pcm16Blocks[c][i].Add(pcm16[c][j]);
                                }
                                break;

                            //DSP-ADPCM.
                            case EncodingTypes.DSP_ADPCM:
                                for (int j = i * 0x2000; j < dspAdpcm[0].Length; j++)
                                {
                                    dspAdpcmBlocks[c][i].Add(dspAdpcm[c][j]);
                                }
                                break;

                        }

                    }

                }
            }

            //Write each block.
            for (int b = 0; b < wavInfo.streamSoundInfo.blockCount; b++) {

                //Write each channel.
                for (int c = 0; c < wavInfo.channels.Count(); c++) {

                    switch (wavInfo.streamSoundInfo.encoding) {
                        
                        //PCM8.
                        case EncodingTypes.PCM8:
                            foreach (sbyte s in pcm8Blocks[c][b].ToArray()) {
                                bw.Write(s);
                            }
                            break;

                        //PCM16.
                        case EncodingTypes.PCM16:
                            bw.Write(pcm16Blocks[c][b]);
                            break;

                        //DSP-ADPCM.
                        case EncodingTypes.DSP_ADPCM:
                            bw.Write(dspAdpcmBlocks[c][b].ToArray());
                            break;

                    }

                    //Write padding if last block.
                    if (b == wavInfo.streamSoundInfo.blockCount - 1) {
                        while (bw.Position % 0x20 != 0) {
                            bw.Write((byte)0);
                        }
                    }

                }

            }

        }

        /// <summary>
        /// Pcm8 data.
        /// </summary>
        public sbyte[][] pcm8;

        /// <summary>
        /// Pcm16 data.
        /// </summary>
        public Int16[][] pcm16;

        /// <summary>
        /// Dspadpcm data.
        /// </summary>
        public byte[][] dspAdpcm;

    }


    /// <summary>
    /// SoundN'Stream seek block.
    /// </summary>
    public class SoundNStreamSeekBlock
    {

        /// <summary>
        /// 1 - History info[NumChannels][NumBlocks].
        /// </summary>
        public HistoryInfo[][] history;


        /// <summary>
        /// Blank constructor.
        /// </summary>
        public SoundNStreamSeekBlock() {

        }

		/// <summary>
		/// Read a seek block.
		/// </summary>
		/// <param name="br">The reader.</param>
		/// <param name="info">The stream info.</param>
		public SoundNStreamSeekBlock(ref BinaryDataReader br, b_stm.StreamSoundInfo info)
		{

			//Size and other useless data.
			br.ReadUInt32s(2);

			//New history info.
			history = new HistoryInfo[info.blockCount][];
			for (int i = 0; i < history.Length; i++)
			{

				history[i] = new HistoryInfo[info.channelCount];
				for (int j = 0; j < history[i].Length; j++)
				{
					history[i][j] = new HistoryInfo()
					{
						yn1 = br.ReadInt16(),
						yn2 = br.ReadInt16()
					};

					//Random padding.
					br.ReadBytes((int)info.sizeOfSeekInfo - 4);

				}

			}

		}

        /// <summary>
        /// Create a seek block from a bunch of samples.
        /// </summary>
        /// <param name="pcm16">Pcm16.</param>
		public SoundNStreamSeekBlock(byte[][] dspAdpcm, uint numSamples, DspAdpcmInfo[] context) {

			short[][] pcm16 = EncoderFactory.DspApcmToPcm16(dspAdpcm, numSamples, context);
			InitializeSamples(pcm16, 0x3800);

		}

        /// <summary>
        /// Initialize the samples of the block.
        /// </summary>
        /// <param name="pcm16">Pcm16.</param>
		public void InitializeSamples(short[][] pcm16, int samplesPerEntry) {

            int entryCount = pcm16[0].Length.DivideByRoundUp(samplesPerEntry);
            history = new HistoryInfo[entryCount][];

            //First samples are 0.
            history[0] = new HistoryInfo[pcm16.Length];
            for (int i = 0; i < pcm16.Length; i++) {
                history[0][i] = new HistoryInfo();
            }

            for (int i = 1; i < entryCount; i++)
            {

                history[i] = new HistoryInfo[pcm16.Length];

                for (int j = 0; j < pcm16.Length; j++) {
                    history[i][j] = new HistoryInfo
                    {
                        yn1 = pcm16[j][i * samplesPerEntry - 1],
                        yn2 = pcm16[j][i * samplesPerEntry - 2]
                    };
                }

            }

        }

        /// <summary>
        /// Get the size of the seek data.
        /// </summary>
        /// <returns></returns>
        public UInt32 GetSize() {

            UInt32 size = (UInt32)(8 + 4 * history.Length * history[0].Length);
            while (size % 0x20 != 0) {
                size++;
            }
            return size;

        }

        /// <summary>
        /// Write seek data.
        /// </summary>
        public void Write(ref BinaryDataWriter bw) {

            //Write stuff.
            bw.Write("SEEK".ToCharArray());
            bw.Write(GetSize());

            foreach (HistoryInfo[] block in history) {

                foreach (HistoryInfo channel in block) {

                    bw.Write(channel.yn1);
                    bw.Write(channel.yn2);

                }

            }

            while (bw.Position % 0x20 != 0) {
                bw.Write((byte)0);
            }

        }


        /// <summary>
        /// History info.
        /// </summary>
        public class HistoryInfo
        {

            /// <summary>
            /// 1 - First history sample.
            /// </summary>
            public Int16 yn1;

            /// <summary>
            /// 2 - Second history sample.
            /// </summary>
            public Int16 yn2;

        }
        
    }


    /// <summary>
    /// SoundN'Stream region block.
    /// </summary>
    public class SoundNStreamRegionBlock
    {

        /// <summary>
        /// 1 - Regions[NumRegions].
        /// </summary>
        public RegionInfo[] regions;

        /// <summary>
        /// Region info. Padded to the size of region info in info block.
        /// </summary>
        public class RegionInfo
        {

            /// <summary>
            /// 1 - Starting sample of the region (inclusive).
            /// </summary>
            public UInt32 start;

            /// <summary>
            /// 2 - Ending sample of the region (exclusive).
            /// </summary>
            public UInt32 end;

            /// <summary>
            /// 3 - Loop info[NumChannels].
            /// </summary>
            public DspAdpcmLoopInfo[] loopInfo;


            /// <summary>
            /// Dsp Adpcm loop info.
            /// </summary>
            public class DspAdpcmLoopInfo
            {

                /// <summary>
                /// 1 - Loop predictor scale.
                /// </summary>
                public UInt16 loopPredScale;

                /// <summary>
                /// 2 - Loop history 1.
                /// </summary>
                public Int16 loopYn1;

                /// <summary>
                /// 3 - Loop history 2.
                /// </summary>
                public Int16 loopYn2;

            }

        }

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public SoundNStreamRegionBlock() {

        }

        /// <summary>
        /// Make a new region block.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="info">The stream info.</param>
        public SoundNStreamRegionBlock(ref BinaryDataReader br, b_stm.StreamSoundInfo info)
        {

            //Block header stuff.
            br.ReadUInt32s(2);

            //Go to offset.
            br.Position += info.regionDataOffset.offset;

            //New region infos.
            regions = new RegionInfo[info.regionCount];

            //Read regions.
            for (int i = 0; i < regions.Length; i++)
            {

                regions[i] = new RegionInfo()
                {
                    start = br.ReadUInt32(),
                    end = br.ReadUInt32(),
                    loopInfo = new RegionInfo.DspAdpcmLoopInfo[info.channelCount]

                };

                //Read loop info.
                for (int j = 0; j < info.channelCount; j++)
                {

                    regions[i].loopInfo[j] = new RegionInfo.DspAdpcmLoopInfo()
                    {
                        loopPredScale = br.ReadUInt16(),
                        loopYn1 = br.ReadInt16(),
                        loopYn2 = br.ReadInt16()
                    };

                }

                //Read padding.
                int readBytes = 8 + (info.channelCount * 6);
                br.ReadBytes(info.regionInfoBytesize - readBytes);

            }

        }

        /// <summary>
        /// Get size of the region block.
        /// </summary>
        /// <returns></returns>
        public UInt32 GetSize() {

            UInt32 size = (uint)(0x20 + regions.Length * 0x100);
            while (size % 0x20 != 0) {
                size++;
            }
            return size;

        }

        /// <summary>
        /// Write region data.
        /// </summary>
        /// <param name="bw"></param>
        public void Write(ref BinaryDataWriter bw) {

            bw.Write("REGN".ToCharArray());
            bw.Write(GetSize());

            bw.Write(new byte[0x18]);

            //Write regions.
            for (int i = 0; i < regions.Length; i++)
            {

                int basePos = (int)bw.Position;

                bw.Write(regions[i].start);
                bw.Write(regions[i].end);

                //Write loop info.
                for (int j = 0; j < regions[i].loopInfo.Length; j++)
                {

                    bw.Write(regions[i].loopInfo[j].loopPredScale);
                    bw.Write(regions[i].loopInfo[j].loopYn1);
                    bw.Write(regions[i].loopInfo[j].loopYn2);

                }

                //Write padding.
                while (bw.Position < basePos + 0x100) {
                    if (bw.Position % (0x68 + basePos) != 0) {
                        bw.Write((byte)0);
                    } else {
                        bw.Write((byte)1);
                    }
                }

            }

            //Write padding.
            while (bw.Position % 0x20 != 0)
            {
                bw.Write((byte)0);
            }

        }

    }


    /// <summary>
    /// Encoding types.
    /// </summary>
    public static class EncodingTypes
    {

        public const byte PCM8 = 0;
        public const byte PCM16 = 1;
        public const byte DSP_ADPCM = 2;
        public const byte IMA_ADPCM = 3;

    }


    /// <summary>
    /// Surround modes.
    /// </summary>
    public static class SurroundMode
    {

        public const byte Normal = 0;
        public const byte FrontBypass = 1;
        public const byte PreProcessed = 2;
        public const byte NotModified = 3;
        public const byte Count = 4;

    }


    /// <summary>
    /// Handle codec conversions.
    /// </summary>
    public static class EncoderFactory
    {


        /// <summary>
        /// Convert pcm8 audio to signed pcm8 audio.
        /// </summary>
        /// <param name="pcm8">Pcm8 data.</param>
        /// <returns>Signed pcm8 data.</returns>
        public static sbyte[][] Pcm8ToSignedPcm8(byte[][] pcm8)
        {

            sbyte[][] newData = new sbyte[pcm8.Length][];

            for (int i = 0; i < newData.Length; i++)
            {

                //Signed audio.
                List<sbyte> signedAudio = new List<sbyte>();

                //Convert the audio.
                foreach (byte sample in pcm8[i])
                {

                    //Signed audio is really just centered at 0 rather than 128.
                    signedAudio.Add((sbyte)(sample - 128));

                }

                newData[i] = signedAudio.ToArray();

            }

            return newData;

        }


        /// <summary>
        /// Convert signed pcm8 audio to pcm8 audio.
        /// </summary>
        /// <param name="sPcm8">Signed pcm8 audio.</param>
        /// <returns>Pcm8 data.</returns>
        public static byte[][] SignedPcm8ToPcm8(sbyte[][] sPcm8)
        {

            byte[][] newData = new byte[sPcm8.Length][];

            for (int i = 0; i < newData.Length; i++)
            {

                //Audio.
                List<byte> audio = new List<byte>();

                //Convert the audio.
                foreach (sbyte sample in sPcm8[i])
                {

                    //Audio is really just centered at 128 rather than 0.
                    audio.Add((byte)(sample + 128));

                }

                newData[i] = audio.ToArray();

            }

            return newData;

        }


        /// <summary>
        /// Convert unsigned pcm8 audio to pcm16 audio.
        /// </summary>
        /// <param name="pcm8"></param>
        /// <returns></returns>
        public static short[][] Pcm8ToPcm16(byte[][] pcm8) {

            return SignedPcm8ToPcm16(Pcm8ToSignedPcm8(pcm8));           

        }


        /// <summary>
        /// Convert signed pcm8 audio to pcm16 audio.
        /// </summary>
        /// <param name="sPcm8"></param>
        /// <returns></returns>
        public static short[][] SignedPcm8ToPcm16(sbyte[][] sPcm8) {

            short[][] pcm16 = new short[sPcm8.Length][];

            //Convert to Pcm16.
            for (int i = 0; i < pcm16.Length; i++)
            {
                pcm16[i] = new short[sPcm8[i].Length];
                for (int j = 0; j < pcm16[i].Length; j++)
                {

                    pcm16[i][j] = (short)(sPcm8[i][j] << 8);

                }

            }

            return pcm16;

        }


        /// <summary>
        /// Convert pcm16 audio to signed pcm8.
        /// </summary>
        /// <param name="pcm16"></param>
        /// <returns></returns>
        public static sbyte[][] Pcm16ToSignedPcm8(short[][] pcm16) {

            sbyte[][] arr = new sbyte[pcm16.Length][];
            for (int i = 0; i < arr.Length; i++) {

                arr[i] = new sbyte[pcm16[i].Length];
                for (int j = 0; j < pcm16[i].Length; j++) {

                    arr[i][j] = (sbyte)((pcm16[i][j] >> 8) & 0xFF);

                }

            }
            return arr;

        }


        /// <summary>
        /// Convert pcm16 audio to pcm8.
        /// </summary>
        /// <param name="pcm16"></param>
        /// <returns></returns>
        public static byte[][] Pcm16ToPcm8(short[][] pcm16)
        {

            sbyte[][] arr = new sbyte[pcm16.Length][];
            for (int i = 0; i < arr.Length; i++)
            {

                arr[i] = new sbyte[pcm16[i].Length];
                for (int j = 0; j < pcm16[i].Length; j++)
                {

                    arr[i][j] = (sbyte)((pcm16[i][j] >> 8) & 0xFF);

                }

            }
            return SignedPcm8ToPcm8(arr);

        }


        /// <summary>
        /// Convert dsp adpcm to pcm16.
        /// </summary>
        /// <param name="dspApdcm">The dsp apdcm samples.</param>
        /// <param name="numSamples">Number of samples.</param>
        /// <param name="context">Dsp apdcm context.</param>
        /// <returns></returns>
        public static Int16[][] DspApcmToPcm16(byte[][] dspApdcm, UInt32 numSamples, DspAdpcmInfo[] context)
        {

            Int16[][] newData = new Int16[dspApdcm.Length][];

            for (int i = 0; i < newData.Length; i++)
            {

                newData[i] = new Int16[numSamples];
                DspAdpcmDecoder.Decode(dspApdcm[i], ref newData[i], ref context[i], numSamples);

            }

            return newData;

        }


        /// <summary>
        /// Pcm16 to dsp adpcm for wav, and update the b_wav channel info.
        /// </summary>
        /// <param name="pcm16">The pcm16 audio data.</param>
        /// <param name="b">The b_wav.</param>
        /// <returns></returns>
        public static byte[][] Pcm16ToDspApdcmWAV(Int16[][] pcm16, ref b_wav b)
        {

            byte[][] data = new byte[pcm16.Length][];
            b.info.channelInfo = new List<b_wav.InfoBlock.ChannelInfo>();
            for (int i = 0; i < data.Length; i++) {
                b.info.channelInfo.Add(new b_wav.InfoBlock.ChannelInfo());
                data[i] = DspAdpcmEncoder.EncodeSamples(pcm16[i], out b.info.channelInfo[i].dspAdpcmInfo, b.info.loopStart);
            }
            return data;

        }


        /// <summary>
        /// Convert pcm16 audio to dsp adpcm with a seek block, and update channel info.
        /// </summary>
        /// <param name="pcm16">Pcm 16 audio.</param>
        /// <returns></returns>
        public static byte[][] Pcm16ToDspAdpcmSTM(Int16[][] pcm16, b_stm s)
        {

            byte[][] data = new byte[pcm16.Length][];
            s.info.channels = new List<b_stm.ChannelInfo>();
            for (int i = 0; i < data.Length; i++)
            {
                s.info.channels.Add(new b_stm.ChannelInfo());
                data[i] = DspAdpcmEncoder.EncodeSamples(pcm16[i], out s.info.channels[i].dspAdpcmInfo, s.info.streamSoundInfo.loopStart);
            }
            return data;

        }

    }


    /// <summary>
    /// Dsp adpcm constants.
    /// </summary>
    public static class DspAdpcmConstants
    {

        public const int BYTES_PER_FRAME = 8;
        public const int SAMPLES_PER_FRAME = 14;
        public const int NIBBLES_PER_FRAME = 16;

    }

}
