using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Portamento time command.
    /// </summary>
    public class PortamentoTimeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public PortamentoTimeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.PortamentoTime;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            PortamentoTime = 0;

        }

        /// <summary>
        /// Portamento time.
        /// </summary>
        public byte PortamentoTime {
            get { return (byte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
