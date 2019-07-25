using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Mod delay command.
    /// </summary>
    public class ModDelayCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ModDelayCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.ModDelay;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s16 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Delay = 0;

        }

        /// <summary>
        /// Speed.
        /// </summary>
        public short Delay {
            get { return (short)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
