using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Sustain command.
    /// </summary>
    public class SustainCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public SustainCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Sustain;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Sustain = 127;

        }

        /// <summary>
        /// Sustain.
        /// </summary>
        public sbyte Sustain {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
