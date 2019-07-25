using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Time change command.
    /// </summary>
    public class TimeChangeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public TimeChangeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.TimeChange;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.SeqCommand, SequenceParameterType.u16 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

        }

        /// <summary>
        /// Sequence command.
        /// </summary>
        public SequenceCommand SequenceCommand {
            get { return (SequenceCommand)Parameters[0]; }
            set { Parameters[0] = value; }
        }

        /// <summary>
        /// Number of ticks.
        /// </summary>
        public ushort Ticks {
            get { return (ushort)Parameters[1]; }
            set { Parameters[1] = value; }
        }

    }

}
