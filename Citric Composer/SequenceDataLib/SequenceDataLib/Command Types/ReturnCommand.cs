using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Return command.
    /// </summary>
    public class ReturnCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ReturnCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Return;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

    }

}
