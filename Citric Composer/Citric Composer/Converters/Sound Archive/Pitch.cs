using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// An pitch.
    /// </summary>
    public class Pitch {

        /// <summary>
        /// Number of semitones.
        /// </summary>
        public int Semitones;

        /// <summary>
        /// Number of cents.
        /// </summary>
        public int Cents;

        /// <summary>
        /// Create pitch from a float.
        /// </summary>
        /// <param name="pitch">Pitch.</param>
        public Pitch(float pitch) {

            //Get total cents.
            int totalCents = (int)Math.Round(Math.Log(pitch, 2) * 1200, MidpointRounding.AwayFromZero);
            Semitones = totalCents / 100;
            Cents = totalCents % 100;

        }

        /// <summary>
        /// Convert to a float.
        /// </summary>
        /// <returns>This as a float.</returns>
        public float ToFloat() {

            //Get total cents.
            int totalCents = Semitones * 100 + Cents;

            //Do conversion.
            return (float)Math.Pow(2, totalCents / (float)1200);

        }

    }

}
