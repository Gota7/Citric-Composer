using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Group entry.
    /// </summary>
    public class GroupEntry {

        /// <summary>
        /// Name of the entry.
        /// </summary>
        public string Name;

        /// <summary>
        /// Sound file.
        /// </summary>
        public SoundFile<ISoundFile> File;

    }

}
