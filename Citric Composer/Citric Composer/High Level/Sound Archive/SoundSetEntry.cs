using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sound set entry.
    /// </summary>
    public class SoundSetEntry {

        /// <summary>
        /// Name of the sound set.
        /// </summary>
        public string Name;

        /// <summary>
        /// Type of entry.
        /// </summary>
        public SoundType SoundType;

        /// <summary>
        /// Starting index.
        /// </summary>
        public int StartIndex;

        /// <summary>
        /// Ending index.
        /// </summary>
        public int EndIndex;

        /// <summary>
        /// Wave archive list if the sound type is WSD.
        /// </summary>
        public List<int> WaveArchives;

    }

}
