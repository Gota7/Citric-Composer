using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Variable command.
    /// </summary>
    public class VariableCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public VariableCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Variable;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.SeqCommand, SequenceParameterType.u8 };

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
        /// Variable value.
        /// </summary>
        public byte Variable {
            get { return (byte)Parameters[1]; }
            set { Parameters[1] = value; }
        }

    }

}
