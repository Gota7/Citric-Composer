using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Mod phase command.
    /// </summary>
    public class Mod3PhaseCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public Mod3PhaseCommand() {

            //Set identifier.
            Identifier = (byte)ExtendedCommandType.Mod3Phase;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Phase = 0;

        }

        /// <summary>
        /// Phase.
        /// </summary>
        public sbyte Phase {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
