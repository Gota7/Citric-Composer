using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {
    class Program {
        static void Main(string[] args) {

            AudioRescource a = new AudioRescource();
            a.Read("BGM.bars");
            a.Write("BGMOUT.bars");

        }
    }
}
