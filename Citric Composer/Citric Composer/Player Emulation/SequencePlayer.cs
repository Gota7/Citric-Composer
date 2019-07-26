using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceEmulation {

    /// <summary>
    /// Sequence player.
    /// </summary>
    public class SequencePlayer {

        /// <summary>
        /// Tracks.
        /// </summary>
        public Track[] Tracks = new Track[16];

        /// <summary>
        /// Number of tracks open.
        /// </summary>
        public bool[] TracksOpen = { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        /// <summary>
        /// Sequence variables.
        /// </summary>
        public short[] SequenceVariables = new short[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        /// <summary>
        /// Global variables.
        /// </summary>
        public short[] GlobalVariables = new short[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        //TODO.
        public void PlaySequence() {

            //Reset variables.
            SequenceVariables = new short[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            Tracks = new Track[16];
            TracksOpen = new bool[] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            Tracks[0] = new Track();

        }

    }

}
