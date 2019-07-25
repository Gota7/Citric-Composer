using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Mod type command.
    /// </summary>
    public class ModTypeCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ModTypeCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.ModType;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Type = 0;

        }

        /// <summary>
        /// Mod type.
        /// </summary>
        public ModType Type {
            get { return (ModType)Parameters[0]; }
            set { Parameters[0] = (byte)value; }
        }

    }

}
