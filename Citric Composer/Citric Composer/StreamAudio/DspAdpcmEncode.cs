using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VGAudio.Codecs.GcAdpcm;

namespace CitraFileLoader
{

    /// <summary>
    /// The encoder.
    /// </summary>
    public class DspAdpcmEncoder
	{

        /// <summary>
        /// Encodes the samples.
        /// </summary>
        /// <returns>The samples.</returns>
        /// <param name="samples">Samples.</param>
		public static byte[] EncodeSamples(short[] samples, out DspAdpcmInfo info, uint loopStart) {

            //Encod data.
			short[] coeffs = GcAdpcmCoefficients.CalculateCoefficients(samples);
			byte[] dspAdpcm = GcAdpcmEncoder.Encode(samples, coeffs);

			info = new DspAdpcmInfo();
            info.coefs = new short[8][];
			info.coefs[0] = new short[2];
            info.coefs[1] = new short[2];
            info.coefs[2] = new short[2];
            info.coefs[3] = new short[2];
            info.coefs[4] = new short[2];
            info.coefs[5] = new short[2];
            info.coefs[6] = new short[2];
            info.coefs[7] = new short[2];
            info.coefs[0][0] = coeffs[0];
            info.coefs[0][1] = coeffs[1];
            info.coefs[1][0] = coeffs[2];
            info.coefs[1][1] = coeffs[3];
            info.coefs[2][0] = coeffs[4];
            info.coefs[2][1] = coeffs[5];
            info.coefs[3][0] = coeffs[6];
            info.coefs[3][1] = coeffs[7];
            info.coefs[4][0] = coeffs[8];
            info.coefs[4][1] = coeffs[9];
            info.coefs[5][0] = coeffs[10];
            info.coefs[5][1] = coeffs[11];
            info.coefs[6][0] = coeffs[12];
            info.coefs[6][1] = coeffs[13];
            info.coefs[7][0] = coeffs[14];
            info.coefs[7][1] = coeffs[15];

            //Loop stuff.
			if (loopStart > 0) info.loop_yn1 = samples[loopStart - 1];
			if (loopStart > 1) info.loop_yn2 = samples[loopStart - 2];

			return dspAdpcm;

		}

    }

}
