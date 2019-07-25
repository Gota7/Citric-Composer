using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Loop end command.
    /// </summary>
    public class LoopEndCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public LoopEndCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.LoopEnd;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

    }

}
