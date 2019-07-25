using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Biquad type command.
    /// </summary>
    public class BiquadTypeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public BiquadTypeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.BiquadType;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            BiquadType = 0;

        }

        /// <summary>
        /// Biquad type.
        /// </summary>
        public sbyte BiquadType {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
