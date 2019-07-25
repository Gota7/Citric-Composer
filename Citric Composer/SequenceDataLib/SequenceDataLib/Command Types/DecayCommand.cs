using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Decay command.
    /// </summary>
    public class DecayCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public DecayCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Decay;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Decay = 127;

        }

        /// <summary>
        /// Decay.
        /// </summary>
        public sbyte Decay {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
