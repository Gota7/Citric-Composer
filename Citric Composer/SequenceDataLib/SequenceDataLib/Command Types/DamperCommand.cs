using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Damper command.
    /// </summary>
    public class DamperCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public DamperCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Damper;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.boolean };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Damper = false;

        }

        /// <summary>
        /// Damper.
        /// </summary>
        public bool Damper {
            get { return (bool)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
