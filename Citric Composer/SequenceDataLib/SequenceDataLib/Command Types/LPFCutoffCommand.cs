using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// LPF cutoff command.
    /// </summary>
    public class LPFCutoffCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public LPFCutoffCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.LPFCutoff;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            LPFCutoff = 64;

        }

        /// <summary>
        /// LPF cutoff.
        /// </summary>
        public sbyte LPFCutoff {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
