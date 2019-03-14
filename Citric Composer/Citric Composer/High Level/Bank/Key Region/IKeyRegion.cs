using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Key region.
    /// </summary>
    public interface IKeyRegion {

        /// <summary>
        /// Get the type of a key region.
        /// </summary>
        /// <returns>The key region type.</returns>
        KeyRegionType GetKeyRegionType();

    }

}
