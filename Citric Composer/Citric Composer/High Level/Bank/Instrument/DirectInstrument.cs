using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Direct instrument.
    /// </summary>
    public class DirectInstrument : IInstrument {

        /// <summary>
        /// Key region.
        /// </summary>
        public IKeyRegion KeyRegion;

        /// <summary>
        /// Get the type of an instrument.
        /// </summary>
        /// <returns>The instrument type.</returns>
        public InstrumentType GetInstrumentType() {
            return InstrumentType.Direct;
        }

    }

}
