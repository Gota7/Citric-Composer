using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Program change command.
    /// </summary>
    public class ProgramChangeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ProgramChangeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.ProgramChange;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.VariableLength };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set program.
            Program = 0;

        }

        /// <summary>
        /// Number to change to, is not more than 0x3FF. Variable length.
        /// </summary>
        public uint Program {
            get { return (uint)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
