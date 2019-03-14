using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Note info.
    /// </summary>
    public class NoteInfo {

        /// <summary>
        /// Flags to use for now, since other info goes unused?
        /// </summary>
        public FlagParameters Flags;

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
        public float Pitch = 1;

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
        /// Send value.
        /// </summary>
        public byte[] SendValue = { 127, 0, 0, 0 };

    }

}
