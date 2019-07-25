using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Portamento enabled command.
    /// </summary>
    public class PortamentoEnabledCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public PortamentoEnabledCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.PortamentoEnabled;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.boolean };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            PortamentoEnabled = false;

        }

        /// <summary>
        /// If portamento is enabled.
        /// </summary>
        public bool PortamentoEnabled {
            get { return (bool)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
