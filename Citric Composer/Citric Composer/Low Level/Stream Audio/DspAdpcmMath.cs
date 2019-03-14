using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    /// <summary>
    /// Dsp adpcm math functions.
    /// </summary>
    public static class DspAdpcmMath
    {

        /// <summary>
        /// Get number of bytes for ADPCM buffer. Use this to figure out the amount of bytes BFWAV data will have.
        /// </summary>
        /// <param name="samples">Number of samples.</param>
        /// <returns></returns>
        public static UInt32 GetBytesForAdpcmBuffer(UInt32 samples)
        {
            UInt32 frames = samples / DspAdpcmConstants.SAMPLES_PER_FRAME;
            if ((samples % DspAdpcmConstants.SAMPLES_PER_FRAME) != 0)
            {
                frames++;
            }

            return frames * DspAdpcmConstants.BYTES_PER_FRAME;
        }


        /*
        /// <summary>
        /// Get how many samples from the number of nibbles.
        /// </summary>
        /// <param name="nibble"></param>
        /// <returns></returns>
        public static UInt32 GetSamplesForPcmBuffer(UInt32 nibble) {
            UInt32 frames = nibble / DspAdpcmConstants.NIBBLES_PER_FRAME;
            if ((nibble % DspAdpcmConstants.NIBBLES_PER_FRAME) != 0) {
                frames++;
            }
            UInt32 samples = DspAdpcmConstants.SAMPLES_PER_FRAME * frames;

            return samples;
        }*/


        /// <summary>
        /// Get bytes for adpcm samples.
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static UInt32 GetBytesForAdpcmSamples(UInt32 samples)
        {
            UInt32 extraBytes = 0;
            UInt32 frames = samples / DspAdpcmConstants.SAMPLES_PER_FRAME;
            UInt32 extraSamples = samples % DspAdpcmConstants.SAMPLES_PER_FRAME;

            if (extraSamples != 0)
            {
                extraBytes = (extraSamples / 2) + (extraSamples % 2) + 1;
            }

            return DspAdpcmConstants.BYTES_PER_FRAME * frames + extraBytes;
        }


        /// <summary>
        /// Get bytes for PCM buffer.
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static UInt32 GetBytesForPcmBuffer(UInt32 samples)
        {
            UInt32 frames = samples / DspAdpcmConstants.SAMPLES_PER_FRAME;
            if ((samples % DspAdpcmConstants.SAMPLES_PER_FRAME) != 0)
            {
                frames++;
            }

            return frames * DspAdpcmConstants.SAMPLES_PER_FRAME * 2;
        }


        /// <summary>
        /// Get bytes for pcm samples.
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static UInt32 GetBytesForPcmSamples(UInt32 samples)
        {
            return samples * 2;
        }


        /// <summary>
        /// Get nibble address.
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static UInt32 GetNibbleAddress(UInt32 samples)
        {
            UInt32 frames = samples / DspAdpcmConstants.SAMPLES_PER_FRAME;
            UInt32 extraSamples = samples % DspAdpcmConstants.SAMPLES_PER_FRAME;

            return DspAdpcmConstants.NIBBLES_PER_FRAME * frames + extraSamples + 2;
        }


        /// <summary>
        /// Get nibbles for N samples.
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static UInt32 GetNibblesForNSamples(UInt32 samples)
        {
            UInt32 frames = samples / DspAdpcmConstants.SAMPLES_PER_FRAME;
            UInt32 extraSamples = samples % DspAdpcmConstants.SAMPLES_PER_FRAME;
            UInt32 extraNibbles = extraSamples == 0 ? 0 : extraSamples + 2;

            return DspAdpcmConstants.NIBBLES_PER_FRAME * frames + extraNibbles;
        }


        /// <summary>
        /// Get sample for adpcm nibble.
        /// </summary>
        /// <param name="nibble"></param>
        /// <returns></returns>
        public static UInt32 GetSampleForAdpcmNibble(UInt32 nibble)
        {
            UInt32 frames = nibble / DspAdpcmConstants.NIBBLES_PER_FRAME;
            UInt32 extraNibbles = nibble % DspAdpcmConstants.NIBBLES_PER_FRAME;
            UInt32 samples = DspAdpcmConstants.SAMPLES_PER_FRAME * frames;

            return samples + extraNibbles - 2;
        }

    }

}
