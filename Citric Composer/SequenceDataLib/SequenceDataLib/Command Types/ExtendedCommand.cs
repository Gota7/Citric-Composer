using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Extended command.
    /// </summary>
    public class ExtendedCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ExtendedCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Extended;

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
