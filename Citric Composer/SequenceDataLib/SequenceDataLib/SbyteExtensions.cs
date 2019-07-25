using Melanchall.DryWetMidi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods {

    /// <summary>
    /// Sbyte extensions.
    /// </summary>
    public static class SbyteExtensions {

        public static SevenBitNumber To7Bit(this sbyte s) {
            return new SevenBitNumber((byte)s);
        }

    }
}
