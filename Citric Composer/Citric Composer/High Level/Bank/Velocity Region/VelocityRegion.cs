using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Velocity region.
    /// </summary>
    public class VelocityRegion {

        /// <summary>
        /// Wave index.
        /// </summary>
        public int WaveIndex;

        /// <summary>
        /// Original key, positive.
        /// </summary>
        public sbyte OriginalKey = 60;

        /// <summary>
        /// Volume.
        /// </summary>
        public byte Volume = 127;

        /// <summary>
        /// Pan, positive.
        /// </summary>
        public sbyte Pan = 64;

        /// <summary>
        /// Surround pan.
        /// </summary>
        public sbyte SurroundPan;

        /// <summary>
        /// Pitch.
        /// </summary>
        public float Pitch = 1f;

        /// <summary>
        /// Key group, 0-15.
        /// </summary>
        public byte KeyGroup;

        /// <summary>
        /// Attack.
        /// </summary>
        public byte Attack = 127;

        /// <summary>
        /// Decay.
        /// </summary>
        public byte Decay = 127;

        /// <summary>
        /// Sustain.
        /// </summary>
        public byte Sustain = 127;

        /// <summary>
        /// Hold.
        /// </summary>
        public byte Hold = 127;

        /// <summary>
        /// Release.
        /// </summary>
        public byte Release = 127;

        /// <summary>
        /// Percussing mode. (Is ignore off.)
        /// </summary>
        public bool PercussionMode;

        /// <summary>
        /// Interpolation type.
        /// </summary>
        public EInterPolationType InterPolationType;

        /// <summary>
        /// Interpolation type.
        /// </summary>
        public enum EInterPolationType {
            PolyPhase, Linear
        }

    }

}
