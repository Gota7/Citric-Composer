using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceEmulation {

    /// <summary>
    /// Track.
    /// </summary>
    public class Track {

        /// <summary>
        /// Instrument number.
        /// </summary>
        public int InstrumentNumber = 0;

        /// <summary>
        /// Current command offset.
        /// </summary>
        public int Offset = 0;

        /// <summary>
        /// Offsets to return to after a call.
        /// </summary>
        public Stack<int> ReturnToOffsets = new Stack<int>();

        /// <summary>
        /// Time base.
        /// </summary>
        public int TimeBase = 48;

        /// <summary>
        /// Tempo.
        /// </summary>
        public int Tempo = 120;

        /// <summary>
        /// Hold, default is the current instrument's, which is marked as -1.
        /// </summary>
        public int Hold = -1;

        /// <summary>
        /// If a note is currently playing.
        /// </summary>
        public bool NotePlaying = false;

        /// <summary>
        /// If true, then slide notes. If not then play multiple notes.
        /// </summary>
        public bool Monophonic = false;

        /// <summary>
        /// Velocity range.
        /// </summary>
        public int VelocityRange = 127;

        /// <summary>
        /// Biquad type.
        /// </summary>
        public int BiquadType = 0;

        /// <summary>
        /// Biquad value.
        /// </summary>
        public int BiquadValue = 0;

        /// <summary>
        /// Bank number.
        /// </summary>
        public int BankNumber = 0;

        /// <summary>
        /// Variables.
        /// </summary>
        public short[] Variables = new short[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        /// <summary>
        /// Conditional flag.
        /// </summary>
        public bool ConditionFlag = false;

        /// <summary>
        /// Blank constructor for track 0.
        /// </summary>
        public Track() {

        }

        /// <summary>
        /// Create a track using the defaults of another track.
        /// </summary>
        /// <param name="t">The track to inherit.</param>
        public Track(Track t) {
            TimeBase = t.TimeBase;
            Tempo = t.Tempo;
            Hold = t.Hold;
            Monophonic = t.Monophonic;
            VelocityRange = t.VelocityRange;
            BiquadType = t.BiquadType;
            BiquadValue = t.BiquadValue;
            BankNumber = t.BankNumber;
            ConditionFlag = t.ConditionFlag;
        }

    }

}
