using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Main volume command.
    /// </summary>
    public class PitchBendCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public PitchBendCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.PitchBend;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            PitchBend = 0;

        }

        /// <summary>
        /// Pitch bend. Can be negative.
        /// </summary>
        public sbyte PitchBend {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
