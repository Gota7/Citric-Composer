using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Main send command.
    /// </summary>
    public class MainSendCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public MainSendCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.MainSend;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            MainSend = 127;

        }

        /// <summary>
        /// Main send.
        /// </summary>
        public sbyte MainSend {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
