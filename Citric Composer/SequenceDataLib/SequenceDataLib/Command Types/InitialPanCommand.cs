using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Initial pan command.
    /// </summary>
    public class InitialPanCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public InitialPanCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.InitialPan;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            NotePan = 64;

        }

        /// <summary>
        /// Pan to play for the next note only.
        /// </summary>
        public sbyte NotePan {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
