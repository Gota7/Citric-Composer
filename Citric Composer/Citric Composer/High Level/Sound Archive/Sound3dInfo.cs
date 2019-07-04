using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// 3d Sound info.
    /// </summary>
    public class Sound3dInfo {

        /// <summary>
        /// Volume.
        /// </summary>
        public bool Volume;

        /// <summary>
        /// Priority.
        /// </summary>
        public bool Priority;

        /// <summary>
        /// Pan.
        /// </summary>
        public bool Pan;

        /// <summary>
        /// Surround pan.
        /// </summary>
        public bool Span;

        /// <summary>
        /// Filter.
        /// </summary>
        public bool Filter;

        /// <summary>
        /// Attenuation rate.
        /// </summary>
        public float AttenuationRate = .5f;

        /// <summary>
        /// Attenuation curve.
        /// </summary>
        public EAttenuationCurve AttenuationCurve;

        /// <summary>
        /// Doppler factor.
        /// </summary>
        public byte DopplerFactor;

        /// <summary>
        /// Unknown flag.
        /// </summary>
        public bool UnknownFlag;

        /// <summary>
        /// Attenuation curve.
        /// </summary>
        public enum EAttenuationCurve {
            Logarithmic, Linear
        }

    }

}
