using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {

    /// <summary>
    /// File version.
    /// </summary>
    public class Version {

        /// <summary>
        /// Major byte.
        /// </summary>
        public byte Major;

        /// <summary>
        /// Minor byte.
        /// </summary>
        public byte Minor;

        /// <summary>
        /// Version.
        /// </summary>
        /// <param name="major">Major.</param>
        /// <param name="minor">Minor.</param>
        public Version(byte major, byte minor) {
            Major = major;
            Minor = minor;
        }

        /// <summary>
        /// Create a new version.
        /// </summary>
        /// <param name="raw">Raw data.</param>
        public Version(ushort raw) {
            Major = (byte)((raw & 0xFF00) >> 8);
            Minor = (byte)(raw & 0xFF);
        }

        /// <summary>
        /// Convert the version to a short.
        /// </summary>
        /// <returns>The version.</returns>
        public ushort ToShort() {
            return (ushort)((Major << 8) | Minor);
        }

    }

}
