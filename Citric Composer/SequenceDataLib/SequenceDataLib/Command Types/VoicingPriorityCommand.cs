using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Voicing priority command.
    /// </summary>
    public class VoicingPriorityCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public VoicingPriorityCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.VoicingPriority;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Priority = 64;

        }

        /// <summary>
        /// Priority
        /// </summary>
        public sbyte Priority {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
