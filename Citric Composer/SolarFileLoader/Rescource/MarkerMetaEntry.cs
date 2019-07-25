using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {

    /// <summary>
    /// Marker meta entry in the meta block.
    /// </summary>
    public class MarkerMetaEntry {

        /// <summary>
        /// Id.
        /// </summary>
        public UInt32 Id;

        /// <summary>
        /// Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Start position.
        /// </summary>
        public UInt32 StartPosition;

        /// <summary>
        /// Length.
        /// </summary>
        public UInt32 Length;

    }

}
