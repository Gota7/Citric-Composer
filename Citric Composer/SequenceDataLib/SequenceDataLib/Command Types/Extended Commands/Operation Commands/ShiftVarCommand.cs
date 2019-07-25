using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Shift var command.
    /// </summary>
    public class ShiftVarCommand : SequenceCommand {

        /// <summary>
        /// Set defaults.
        /// </summary>
        public ShiftVarCommand() {

            //Set identifier.
            Identifier = (byte)ExtendedCommandType.ShiftVar;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8, SequenceParameterType.s16};

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

        /// <summary>
        /// Variable.
        /// </summary>
        public byte Variable {
            get { return (byte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

        /// <summary>
        /// Actual value.
        /// </summary>
        public short Value {
            get { return (short)Parameters[1]; }
            set { Parameters[1] = value; }
        }

    }

}
