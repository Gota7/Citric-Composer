using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citric_Composer
{

    /// <summary>
    /// Lopus auidio format.
    /// </summary>
    public class Lopus {

        /// <summary>
        /// 1 - Head.
        /// </summary>
        public Head head;

        /// <summary>
        /// 2 - Head2.
        /// </summary>
        public Head2 head2;

        /// <summary>
        /// 3 - Data.
        /// </summary>
        public Data data;


        /// <summary>
        /// Head block.
        /// </summary>
        public class Head {

            /// <summary>
            /// 1 - OPUS.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Padding, or version?
            /// </summary>
            public UInt32 padding;

            /// <summary>
            /// 3 - Number of samples.
            /// </summary>
            public UInt32 numSamples;

            /// <summary>
            /// 4 - Number of channels.
            /// </summary>
            public UInt32 numChannels;

            /// <summary>
            /// 5 - Sampling rate.
            /// </summary>
            public UInt32 sampleRate;

            /// <summary>
            /// 6 - Loop start.
            /// </summary>
            public UInt32 loopStart;

            /// <summary>
            /// 7 - Loop end. If 0, there is no loop.
            /// </summary>
            public UInt32 loopEnd;

            /// <summary>
            /// 8 - Bits per sample.
            /// </summary>
            public UInt32 bitsPerSample;

            /// <summary>
            /// 9 - Where data begins.
            /// </summary>
            public UInt32 dataOffset;

            /// <summary>
            /// 10 - Unknown data.
            /// </summary>
            public UInt32 unk1;

        }

        /// <summary>
        /// Head2 block.
        /// </summary>
        public class Head2 {

            /// <summary>
            /// 1 - 0x80000001.
            /// </summary>
            public UInt32 magic;

            public UInt32 unk1;
            public byte unk2;

            /// <summary>
            /// Number of channels.
            /// </summary>
            public byte numChannels;

            public UInt16 unk3;

            /// <summary>
            /// Sampling rate.
            /// </summary>
            public UInt32 sampleRate;

            /// <summary>
            /// Data offset. Relative to start of head2.
            /// </summary>
            public UInt32 dataOffset;

            /// <summary>
            /// Padding.
            /// </summary>
            public UInt32 padding1;

            /// <summary>
            /// More padding.
            /// </summary>
            public UInt32 padding2;

            /// <summary>
            /// Skip value.
            /// </summary>
            public UInt32 skip;

        }

        /// <summary>
        /// Data block.
        /// </summary>
        public class Data {

            /// <summary>
            /// 1 - 0x80000004.
            /// </summary>
            public UInt32 magic;

            /// <summary>
            /// 2 - Size of data.
            /// </summary>
            public UInt32 dataSize;

            /// <summary>
            /// 3 - Start offset of data.
            /// </summary>
            public UInt32 startOffset;

            /// <summary>
            /// 4 - Data.
            /// </summary>
            public byte[] data;

        }

    }

}
