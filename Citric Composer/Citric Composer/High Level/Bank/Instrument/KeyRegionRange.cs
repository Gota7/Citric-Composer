using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Key region range. NOT USED, SO THIS IS STUPID.
    /// </summary>
    public class KeyRegionRange {

        /// <summary>
        /// Create a new instrument range.
        /// </summary>
        /// <param name="startNote">Starting note.</param>
        /// <param name="endNote">Ending note.</param>
        /// <param name="keyRegion">Key region.</param>
        public KeyRegionRange(sbyte startNote, sbyte endNote, IKeyRegion keyRegion) {
            StartNote = startNote;
            EndNote = endNote;
            KeyRegion = keyRegion;
        }

        /// <summary>
        /// Starting note.
        /// </summary>
        public sbyte StartNote;

        /// <summary>
        /// Ending note.
        /// </summary>
        public sbyte EndNote;

        /// <summary>
        /// Key region.
        /// </summary>
        public IKeyRegion KeyRegion;

    }

}
