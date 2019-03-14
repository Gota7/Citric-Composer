using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Wave sound data item.
    /// </summary>
    public class WaveSoundDataItem {

        /// <summary>
        /// Notes.
        /// </summary>
        public List<NoteInfo> Notes;

        /// <summary>
        /// List of tracks, and each track has a list of note events.
        /// </summary>
        public List<List<NoteEvent>> NoteEvents;

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
        /// </summary
        public float Pitch = 1;

        /// <summary>
        /// Send value.
        /// </summary>
        public byte[] SendValue = new byte[] { 127, 0, 0, 0 };

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
        /// Lpf frequency. Between 0 and 64.
        /// </summary>
        public sbyte LpfFrequency = 64;

        /// <summary>
        /// Biquad type.
        /// </summary>
        public EBiquadType BiquadType;

        /// <summary>
        /// Biquad value, positive.
        /// </summary>
        public sbyte BiquadValue;

        /// <summary>
        /// Byquad types.
        /// </summary>
        public enum EBiquadType {
            Unused, LPF, HPF, BPF512Hz, BPF1024Hz, BPF2048Hz, User0
        }


        /// <summary>
        /// Create a wave sound data item.
        /// </summary>
        public WaveSoundDataItem() {
            Notes = new List<NoteInfo>();
            NoteEvents = new List<List<NoteEvent>>();
        }

    }

}
