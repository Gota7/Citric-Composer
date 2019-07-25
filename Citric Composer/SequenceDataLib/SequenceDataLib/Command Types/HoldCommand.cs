using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Hold command.
    /// </summary>
    public class HoldCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public HoldCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Hold;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Hold = 127;

        }

        /// <summary>
        /// Hold.
        /// </summary>
        public sbyte Hold {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
