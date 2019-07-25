using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Print var command.
    /// </summary>
    public class PrintVarCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public PrintVarCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.PrintVar;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Var = 0;

        }

        /// <summary>
        /// Variable.
        /// </summary>
        public sbyte Var {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
