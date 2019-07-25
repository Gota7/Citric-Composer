using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Release command.
    /// </summary>
    public class ReleaseCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ReleaseCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Release;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Release = 127;

        }

        /// <summary>
        /// Release.
        /// </summary>
        public sbyte Release {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
