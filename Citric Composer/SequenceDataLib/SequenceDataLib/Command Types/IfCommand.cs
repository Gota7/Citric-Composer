using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// If command.
    /// </summary>
    public class IfCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public IfCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.If;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.SeqCommand };

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

    }

}
