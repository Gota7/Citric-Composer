using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Player entry.
    /// </summary>
    public class PlayerEntry {

        /// <summary>
        /// Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Max number of sounds at a time. Max is 255.
        /// </summary>
        public int SoundLimit;

        /// <summary>
        /// Max size of a sound. Actually is int.
        /// </summary>
        public int PlayerHeapSize;

        /// <summary>
        /// Include the heap size.
        /// </summary>
        public bool IncludeHeapSize;

    }

}
