using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Pan command.
    /// </summary>
    public class PanCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public PanCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Pan;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            TrackPan = 64;

        }

        /// <summary>
        /// Pan for the track, middle is 64.
        /// </summary>
        public sbyte TrackPan {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
