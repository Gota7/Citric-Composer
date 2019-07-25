using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Bend range command.
    /// </summary>
    public class BendRangeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public BendRangeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.BendRange;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            BendRange = 2;

        }

        /// <summary>
        /// Bend range.
        /// </summary>
        public sbyte BendRange {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
