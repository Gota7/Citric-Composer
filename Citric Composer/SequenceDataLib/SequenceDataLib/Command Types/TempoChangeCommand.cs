using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Tempo change command.
    /// </summary>
    public class TempoChangeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public TempoChangeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.TempoChange;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s16 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set tempo.
            Tempo = 120;

        }

        /// <summary>
        /// Tempo. Max is 0x7FFF.
        /// </summary>
        public short Tempo {
            get { return (short)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
