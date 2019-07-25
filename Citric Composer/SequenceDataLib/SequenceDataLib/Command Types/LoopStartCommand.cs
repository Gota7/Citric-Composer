using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Loop start command.
    /// </summary>
    public class LoopStartCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public LoopStartCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.LoopStart;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Count = 0;

        }

        /// <summary>
        /// Loop count.
        /// </summary>
        public byte Count {
            get { return (byte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
