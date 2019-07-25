using Melanchall.DryWetMidi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods {

    /// <summary>
    /// Byte extensions.
    /// </summary>
    public static class ByteExtensions {

        public static SevenBitNumber To7Bit(this byte b) {
            return new SevenBitNumber(b);
        }

    }

}
