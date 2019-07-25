using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Allocate track command.
    /// </summary>
    public class AllocateTrackCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public AllocateTrackCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.AllocateTrack;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u16 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            AllocatedTracks = 0;

        }

        /// <summary>
        /// Allocated tracks. The lowest bit is ignored, 0 is always enabled.
        /// </summary>
        public UInt16 AllocatedTracks {
            get { return (UInt16)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
