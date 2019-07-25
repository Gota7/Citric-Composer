using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Sweep pitch command.
    /// </summary>
    public class SweepPitchCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public SweepPitchCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.SweepPitch;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s16 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            SweepPitch = 0;

        }

        /// <summary>
        /// Sweep pitch.
        /// </summary>
        public Int16 SweepPitch {
            get { return (Int16)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
