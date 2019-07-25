using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Surround pan command.
    /// </summary>
    public class SurroundPanCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public SurroundPanCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.SurroundPan;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            TrackSurroundPan = 0;

        }

        /// <summary>
        /// Surround pan for the track. 0 is front, 127 is back.
        /// </summary>
        public sbyte TrackSurroundPan {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
