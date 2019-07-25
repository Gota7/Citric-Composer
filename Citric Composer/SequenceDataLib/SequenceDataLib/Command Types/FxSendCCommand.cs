using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Fx send C command.
    /// </summary>
    public class FxSendCCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public FxSendCCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.FxSendC;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            FxSendC = 0;

        }

        /// <summary>
        /// Fx send C.
        /// </summary>
        public sbyte FxSendC {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
