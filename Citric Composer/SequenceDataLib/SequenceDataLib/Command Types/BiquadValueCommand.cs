using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Biquad type command.
    /// </summary>
    public class BiquadValueCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public BiquadValueCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.BiquadValue;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            BiquadValue = 0;

        }

        /// <summary>
        /// Biquad value.
        /// </summary>
        public sbyte BiquadValue {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
