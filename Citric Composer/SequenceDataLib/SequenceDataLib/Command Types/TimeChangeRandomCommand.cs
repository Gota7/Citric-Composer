using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Time change random command.
    /// </summary>
    public class TimeChangeRandomCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public TimeChangeRandomCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.TimeRandom;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.SeqCommand, SequenceParameterType.u16, SequenceParameterType.u16 };

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
        /// Min timechange.
        /// </summary>
        public ushort Min {
            get { return (ushort)Parameters[1]; }
            set { Parameters[1] = value; }
        }

        /// <summary>
        /// Max timechange.
        /// </summary>
        public ushort Max {
            get { return (ushort)Parameters[2]; }
            set { Parameters[2] = value; }
        }

    }

}
