using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Fx send A command.
    /// </summary>
    public class FxSendBCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public FxSendBCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.FxSendB;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            FxSendB = 0;

        }

        /// <summary>
        /// Fx send B.
        /// </summary>
        public sbyte FxSendB {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
