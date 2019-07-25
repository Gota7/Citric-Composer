using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Open track command.
    /// </summary>
    public class OpenTrackCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public OpenTrackCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.OpenTrack;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.u8, SequenceParameterType.u24 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            TrackNumber = 0;
            Offset = 0;

        }

        /// <summary>
        /// Track number.
        /// </summary>
        public byte TrackNumber {
            get { return (byte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

        /// <summary>
        /// Offset from beginning of sequence data.
        /// </summary>
        public UInt24 Offset {
            get { return (UInt24)Parameters[1]; }
            set { Parameters[1] = value; }
        }

    }

}
