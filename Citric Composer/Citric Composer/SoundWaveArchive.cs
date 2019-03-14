using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    /// <summary>
    /// Sound wave archive.
    /// </summary>
    public class b_war
    {

        /// <summary>
        /// 1 - File header.
        /// </summary>
        public FileHeader fileHeader;

        /// <summary>
        /// 2 - Info block.
        /// </summary>
        public InfoBlock info;

        /// <summary>
        /// 3 - File block.
        /// </summary>
        public FileBlock file;


        /// <summary>
        /// Info block.
        /// </summary>
        public class InfoBlock {

            /// <summary>
            /// 1 - INFO.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Block size.
            /// </summary>
            public UInt32 blockSize;

            /// <summary>
            /// 3 - File table.
            /// </summary>
            public SizedReferenceTable fileTable;

        }

        /// <summary>
        /// File block.
        /// </summary>
        public class FileBlock {

            /// <summary>
            /// 1 - FILE.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Block size.
            /// </summary>
            public UInt32 blockSize;

            /// <summary>
            /// 3 - 0x18 padding.
            /// </summary>
            public byte[] padding;

            /// <summary>
            /// 4 - Files, loaded from the b_sar.
            /// </summary>
            public byte[][] files;

        }

    }

}
