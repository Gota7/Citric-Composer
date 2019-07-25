using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Time base command.
    /// </summary>
    public class TimeBaseCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public TimeBaseCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.TimeBase;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set length.
            QuarterNoteLength = 48;

        }

        /// <summary>
        /// Length of a quarter note in ticks.
        /// </summary>
        public byte QuarterNoteLength {
            get { return (byte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
