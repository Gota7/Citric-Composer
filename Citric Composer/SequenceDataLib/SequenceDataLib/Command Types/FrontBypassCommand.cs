using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Front bypass command.
    /// </summary>
    public class FrontBypassCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public FrontBypassCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.FrontBypass;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.boolean };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            FrontBypass = false;

        }

        /// <summary>
        /// Front bypass.
        /// </summary>
        public bool FrontBypass {
            get { return (bool)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
