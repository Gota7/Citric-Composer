using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Volume command.
    /// </summary>
    public class VolumeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public VolumeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Volume;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set volume.
            Volume = 127;

        }

        /// <summary>
        /// Actual volume is this/127 squared times 100% times Volume2.
        /// </summary>
        public sbyte Volume {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
