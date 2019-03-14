using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    public enum WriteMode {
        F_LE, F_BE, C_LE, C_BE,
        CTR = C_LE, Cafe = F_BE, NX = F_LE
    }

}
