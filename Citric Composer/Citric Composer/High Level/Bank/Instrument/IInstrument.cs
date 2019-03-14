using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Instrument.
    /// </summary>
    public interface IInstrument {

        /// <summary>
        /// Get the type of an instrument.
        /// </summary>
        /// <returns>The instrument type.</returns>
        InstrumentType GetInstrumentType();

    }

}
