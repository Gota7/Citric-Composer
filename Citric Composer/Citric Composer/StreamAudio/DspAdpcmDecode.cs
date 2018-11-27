using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    public static class DspAdpcmDecoder
    {

        static sbyte[] NibbleToSbyte = { 0, 1, 2, 3, 4, 5, 6, 7, -8, -7, -6, -5, -4, -3, -2, -1 };

        static uint DivideByRoundUp(uint dividend, uint divisor)
        {
            return (dividend + divisor - 1) / divisor;
        }

        static sbyte GetHighNibble(byte value)
        {
            return NibbleToSbyte[(value >> 4) & 0xF];
        }

        static sbyte GetLowNibble(byte value)
        {
            return NibbleToSbyte[value & 0xF];
        }

        static short Clamp16(int value)
        {
            if (value > 32767)
            {
                return 32767;
            }
            if (value < -32678)
            {
                return -32678;
            }
            return (short)value;
        }


        /// <summary>
        /// Decode DSP-ADPCM data.
        /// </summary>
        /// <param name="src">DSP-ADPCM source.</param>
        /// <param name="dst">Destination array of samples.</param>
        /// <param name="cxt">DSP-APCM context.</param>
        /// <param name="samples">Number of samples.</param>
        public static void Decode(byte[] src, ref Int16[] dst, ref DspAdpcmInfo cxt, UInt32 samples)
        {

            //Each DSP-APCM frame is 8 bytes long. It contains 1 header byte, and 7 sample bytes.

            //Set initial values.
            short hist1 = cxt.yn1;
            short hist2 = cxt.yn2;
            int dstIndex = 0;
            int srcIndex = 0;

            //Until all samples decoded.
            while (dstIndex < samples)
            {

                //Get the header.
                byte header = src[srcIndex++];

                //Get scale and co-efficient index.
                UInt16 scale = (UInt16)(1 << (header & 0xF));
                byte coef_index = (byte)(header >> 4);
                short coef1 = cxt.coefs[coef_index][0];
                short coef2 = cxt.coefs[coef_index][1];

                //7 sample bytes per frame.
                for (UInt32 b = 0; b < 7; b++)
                {

                    //Get byte.
                    byte byt = src[srcIndex++];

                    //2 samples per byte.
                    for (UInt32 s = 0; s < 2; s++)
                    {
                        sbyte adpcm_nibble = ((s == 0) ? GetHighNibble(byt) : GetLowNibble(byt));
                        short sample = Clamp16(((adpcm_nibble * scale) << 11) + 1024 + ((coef1 * hist1) + (coef2 * hist2)) >> 11);

                        hist2 = hist1;
                        hist1 = sample;
                        dst[dstIndex++] = sample;

                        if (dstIndex >= samples) break;
                    }
                    if (dstIndex >= samples) break;

                }

            }

        }

    }

}
