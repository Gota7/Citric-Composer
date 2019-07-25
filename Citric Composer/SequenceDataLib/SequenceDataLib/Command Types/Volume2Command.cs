using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Volume command.
    /// </summary>
    public class Volume2Command : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public Volume2Command() {

            //Set identifier.
            Identifier = (byte)CommandType.Volume2;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set volume.
            Volume2 = 127;

        }

        /// <summary>
        /// Actual volume is this/127 squared times 100% times Volume2.
        /// </summary>
        public sbyte Volume2 {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
