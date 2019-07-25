using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Random command.
    /// </summary>
    public class RandomCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public RandomCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Random;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.SeqCommand, SequenceParameterType.s16, SequenceParameterType.s16 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

        /// <summary>
        /// Sequence command.
        /// </summary>
        public SequenceCommand SequenceCommand {
            get { return (SequenceCommand)Parameters[0]; }
            set { Parameters[0] = value; }
        }

        /// <summary>
        /// Minimum random value.
        /// </summary>
        public short Min {
            get { return (short)Parameters[1]; }
            set { Parameters[1] = value; }
        }

        /// <summary>
        /// Maximum random value.
        /// </summary>
        public short Max {
            get { return (short)Parameters[2]; }
            set { Parameters[2] = value; }
        }

    }

}
