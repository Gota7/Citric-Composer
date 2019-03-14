using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Direct key region.
    /// </summary>
    public class DirectKeyRegion : IKeyRegion {

        /// <summary>
        /// Velocity region.
        /// </summary>
        public VelocityRegion VelocityRegion;

        /// <summary>
        /// Get the type of a key region.
        /// </summary>
        /// <returns>The key region type.</returns>
        public KeyRegionType GetKeyRegionType() {
            return KeyRegionType.Direct;
        }

    }

}
