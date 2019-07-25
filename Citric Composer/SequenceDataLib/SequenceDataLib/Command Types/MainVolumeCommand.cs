using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Main volume command.
    /// </summary>
    public class MainVolumeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public MainVolumeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.MainVolume;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set volume.
            MainVolume = 127;

        }

        /// <summary>
        /// Main volume.
        /// </summary>
        public sbyte MainVolume {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
