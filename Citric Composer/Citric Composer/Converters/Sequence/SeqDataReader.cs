using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sequence data reader.
    /// </summary>
    public static class SeqDataReader {

        /// <summary>
        /// Change the endian of the sequence data.
        /// </summary>
        /// <param name="seqData">Sequence data.</param>
        /// <param name="isBE">If the sequence data is big endian.</param>
        /// <returns>The sequence data in the swapped endian.</returns>
        public static byte[] ChangeEndian(byte[] seqData, bool isBE) {

            return null;

        }

        /// <summary>
        /// Get all local offsets.
        /// </summary>
        /// <param name="seqData">Sequence data.</param>
        /// <param name="isBe">If the sequence data is big endian.</param>
        /// <returns>Local offsets in the sequence data.</returns>
        public static List<int> GetLocalOffsets(byte[] seqData, bool isBe) {

            return null;

        }

        /// <summary>
        /// Convert sequence data to text seq.
        /// </summary>
        /// <param name="seqData">Sequence data.</param>
        /// <param name="isBe">If the sequence data is big endian.</param>
        /// <returns>Converted sequence data.</returns>
        public static string[] ConvertSeqData(byte[] seqData, bool isBe) {

            return null;

        }

    }

}
