using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Wait command.
    /// </summary>
    public class WaitCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public WaitCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Wait;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.VariableLength };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set length.
            Length = 48;

        }

        /// <summary>
        /// Length of the note. Variable length.
        /// </summary>
        public uint Length {
            get { return (uint)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
