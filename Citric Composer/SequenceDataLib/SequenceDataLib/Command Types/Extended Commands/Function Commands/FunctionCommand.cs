using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Function command.
    /// </summary>
    public class FunctionCommand : SequenceCommand {

        /// <summary>
        /// Set defaults.
        /// </summary>
        public FunctionCommand() {

            //Set identifier.
            Identifier = (byte)ExtendedCommandType.Function;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s16};

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

        /// <summary>
        /// Function
        /// </summary>
        public short Function {
            get { return (short)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
