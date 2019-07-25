using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Monophonic command.
    /// </summary>
    public class MonophonicCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public MonophonicCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Monophonic;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.boolean };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Monophonic = false;

        }

        /// <summary>
        /// Monophonic.
        /// </summary>
        public bool Monophonic {
            get { return (bool)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
