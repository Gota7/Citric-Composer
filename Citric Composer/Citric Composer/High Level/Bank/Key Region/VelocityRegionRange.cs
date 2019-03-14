using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Velocity region range. USELESS.
    /// </summary>
    public class VelocityRegionRange {

        /// <summary>
        /// Create a new Velocity region range.
        /// </summary>
        /// <param name="startNote">Starting note.</param>
        /// <param name="endVelocity">Ending note.</param>
        /// <param name="keyRegion">Velocity region.</param>
        public VelocityRegionRange(sbyte startVelocity, sbyte endVelocity, VelocityRegion velocityRegion) {
            StartVelocity = startVelocity;
            EndVelocity = endVelocity;
            VelocityRegion = velocityRegion;
        }

        /// <summary>
        /// Starting velocity.
        /// </summary>
        public sbyte StartVelocity;

        /// <summary>
        /// Ending velocity.
        /// </summary>
        public sbyte EndVelocity;

        /// <summary>
        /// Velocity region.
        /// </summary>
        public VelocityRegion VelocityRegion;

    }

}
