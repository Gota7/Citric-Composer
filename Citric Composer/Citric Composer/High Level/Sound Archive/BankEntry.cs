using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Bank entry.
    /// </summary>
    public class BankEntry {

        /// <summary>
        /// New bank entry.
        /// </summary>
        public BankEntry() {
            File = new SoundFile<ISoundFile>();
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Wave archives.
        /// </summary>
        public List<WaveArchiveEntry> WaveArchives;

        /// <summary>
        /// Bank file.
        /// </summary>
        public SoundFile<ISoundFile> File;

    }

}
