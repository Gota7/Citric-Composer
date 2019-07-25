using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Envelope reset command.
    /// </summary>
    public class EnvelopeResetCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public EnvelopeResetCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.EnvelopeReset;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

    }

}
