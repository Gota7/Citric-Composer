using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Fin command.
    /// </summary>
    public class FinCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public FinCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Fin;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

    }

}
