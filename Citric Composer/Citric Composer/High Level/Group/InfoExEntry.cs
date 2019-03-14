using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Extra info entry.
    /// </summary>
    public class InfoExEntry {

        /// <summary>
        /// Item type.
        /// </summary>
        public EItemType ItemType;

        /// <summary>
        /// Index of the item.
        /// </summary>
        public int ItemIndex;

        /// <summary>
        /// Load flags.
        /// </summary>
        public ELoadFlags LoadFlags;

        /// <summary>
        /// Item type.
        /// </summary>
        public enum EItemType {
            Sequence, SequenceSetOrWaveData, Bank, WaveArchive
        }

        /// <summary>
        /// Load flags. Combinations are for Seqs or SeqSets only.
        /// </summary>
        public enum ELoadFlags {
            All, Warc, Bank, Wsd, Seq, BankAndWarc, SeqAndWarc, SeqAndBank
        }

    }

}
