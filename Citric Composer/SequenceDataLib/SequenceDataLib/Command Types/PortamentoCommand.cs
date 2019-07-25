using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Portomento command.
    /// </summary>
    public class PortamentoCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public PortamentoCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Portamento;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Portamento = 0;

        }

        /// <summary>
        /// Portamento.
        /// </summary>
        public sbyte Portamento {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
