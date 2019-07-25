using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Velocity range command.
    /// </summary>
    public class VelocityRangeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public VelocityRangeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.VelocityRange;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            VelocityRange = 127;

        }

        /// <summary>
        /// Velocity range. Final value is this times note velocity divided by 127.
        /// </summary>
        public sbyte VelocityRange {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
