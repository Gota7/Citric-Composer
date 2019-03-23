using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    public static class SoundArchiveVersions {

        /// <summary>
        /// C Versions.
        /// </summary>
        public static class CVersions {

            /// <summary>
            /// Stream has extra info, such as send value with filter and is not in legacy mode.
            /// </summary>
            public static readonly FileWriter.Version StreamExtraInfo = new FileWriter.Version(2, 3, 1);

            /// <summary>
            /// Stream can support prefetch files.
            /// </summary>
            public static readonly FileWriter.Version StreamPrefetch = new FileWriter.Version(2, 3, 2);

        }

        /// <summary>
        /// F Version.
        /// </summary>
        public static class FVersions {

            /// <summary>
            /// Stream has send and filter info.
            /// </summary>
            public static readonly FileWriter.Version StreamSendAndFilter = new FileWriter.Version(2, 1, 0);

            /// <summary>
            /// Stream has prefetch information.
            /// </summary>
            public static readonly FileWriter.Version StreamPrefetch = new FileWriter.Version(2, 2, 0);

        }

        /// <summary>
        /// If the extra stream info is supported and is non-legacy mode.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <returns>If extra stream info is supported.</returns>
        public static bool SupportsExtraStreamInfo(SoundArchive a) {

            //F type.
            if (FileWriter.GetWriteModeChar(a.WriteMode) == 'F') {
                return a.Version >= FVersions.StreamSendAndFilter;
            }

            //C type.
            else {
                return a.Version >= CVersions.StreamExtraInfo;
            }       

        }

        /// <summary>
        /// If a sound archive supports prefetch info.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <returns>If prefetch info is supported.</returns>
        public static bool SupportsPrefetchInfo(SoundArchive a) {

            //F type.
            if (FileWriter.GetWriteModeChar(a.WriteMode) == 'F') {
                return a.Version >= FVersions.StreamPrefetch;
            }

            //C type.
            else {
                return a.Version >= CVersions.StreamPrefetch;
            }

        }

    }

}
