using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Mod curve command.
    /// </summary>
    public class ModCurveCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public ModCurveCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.ModCurve;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Curve = 0;

        }

        /// <summary>
        /// Curve
        /// </summary>
        public ModCurveType Curve {
            get { return (ModCurveType)Parameters[0]; }
            set { Parameters[0] = (byte)value; }
        }

    }

}
