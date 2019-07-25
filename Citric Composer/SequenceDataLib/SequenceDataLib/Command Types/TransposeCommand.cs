using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Transpose command.
    /// </summary>
    public class TransposeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public TransposeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Transpose;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Transpose = 0;

        }

        /// <summary>
        /// Transpose. -64 to 63.
        /// </summary>
        public sbyte Transpose {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
