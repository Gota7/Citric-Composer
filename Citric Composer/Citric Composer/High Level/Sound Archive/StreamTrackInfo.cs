using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Track info.
    /// </summary>
    public class StreamTrackInfo {

        /// <summary>
        /// Lpf frequency. Between 0 and 64.
        /// </summary>
        public sbyte LpfFrequency = 64;

        /// <summary>
        /// Biquad type.
        /// </summary>
        public sbyte BiquadType;

        /// <summary>
        /// Biquad value, positive.
        /// </summary>
        public sbyte BiquadValue;

        /// <summary>
        /// Send value.
        /// </summary>
        public byte[] SendValue = { 127, 0, 0, 0 };       

        /// <summary>
        /// Byquad types.
        /// </summary>
        public enum EBiquadType {
            Unused, LPF, HPF, BPF512Hz, BPF1024Hz, BPF2048Hz, User0
        }

    }

}
