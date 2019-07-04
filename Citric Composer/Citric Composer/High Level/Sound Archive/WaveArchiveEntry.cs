using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Wave archive entry.
    /// </summary>
    public class WaveArchiveEntry {

        /// <summary>
        /// Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Load individually.
        /// </summary>
        public bool LoadIndividually;

        /// <summary>
        /// Include the wave count.
        /// </summary>
        public bool IncludeWaveCount;

        /// <summary>
        /// Sound wave archive file.
        /// </summary>
        public SoundFile<ISoundFile> File;

    }

}
