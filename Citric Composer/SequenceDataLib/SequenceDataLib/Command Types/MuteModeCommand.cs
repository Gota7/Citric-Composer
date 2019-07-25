using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Mute mode command.
    /// </summary>
    public class MuteModeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public MuteModeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.ModType;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

        /// <summary>
        /// Mute mode.
        /// </summary>
        public MuteMode Mode {
            get { return (MuteMode)(byte)Parameters[0]; }
            set { Parameters[0] = (byte)value; }
        }

    }

}
