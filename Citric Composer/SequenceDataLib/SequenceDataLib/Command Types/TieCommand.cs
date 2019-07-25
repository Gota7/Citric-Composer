using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Tie command.
    /// </summary>
    public class TieCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public TieCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Tie;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.boolean };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            TieMode = false;

        }

        /// <summary>
        /// Tie mode.
        /// </summary>
        public bool TieMode {
            get { return (bool)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
