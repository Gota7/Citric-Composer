using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {

    /// <summary>
    /// Byte order.
    /// </summary>
    public enum ByteOrder : UInt16 {
        BigEndian = 0xFEFF,
        LittleEndian = 0xFFFE
    }

}
