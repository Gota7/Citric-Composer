using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Fx send A command.
    /// </summary>
    public class FxSendACommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public FxSendACommand() {

            //Set identifier.
            Identifier = (byte)CommandType.FxSendA;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            FxSendA = 0;

        }

        /// <summary>
        /// Fx send A.
        /// </summary>
        public sbyte FxSendA {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
