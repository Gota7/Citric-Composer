using CitraFileLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.Crc32;

namespace SolarFileLoader {

    /// <summary>
    /// Rescource asset.
    /// </summary>
    public class RescourceAsset {

        /// <summary>
        /// Hash for the string.
        /// </summary>
        public UInt32 StringHash {

            get {

                //Hash does not exist.
                if (Meta == null) {
                    return 0;
                }
                
                //Hash exists.
                else {
                    return Crc32Algorithm.Compute(Encoding.ASCII.GetBytes(Meta.Name));
                }

            }

        }

        /// <summary>
        /// Metadata for the sound.
        /// </summary>
        public AudioMeta Meta;

        /// <summary>
        /// Sound file that is either a prefetch or a wave.
        /// </summary>
        public ISoundFile SoundFile;

    }

}
