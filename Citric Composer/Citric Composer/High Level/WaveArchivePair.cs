using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    public class WaveArchivePair {

        public WaveArchivePair(int warIndex, int waveIndex) {
            WarIndex = warIndex;
            WaveIndex = waveIndex;
        }

        private Wave wave;
        public int WaveIndex;
        public int WarIndex;

    }

}
