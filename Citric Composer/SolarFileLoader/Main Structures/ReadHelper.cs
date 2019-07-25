using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {

    /// <summary>
    /// Read helper.
    /// </summary>
    public class ReadHelper {

        /// <summary>
        /// Structure positions.
        /// </summary>
        private Stack<long> structurePositions = new Stack<long>();

        /// <summary>
        /// Current structure position.
        /// </summary>
        private long currStructurePosition;

        /// <summary>
        /// Make a read helper.
        /// </summary>
        /// <param name="br">The reader.</param>
        public ReadHelper(BinaryDataReader br) {
            currStructurePosition = br.Position;
        }

        /// <summary>
        /// Start a structure.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void SS(BinaryDataReader br) {
            structurePositions.Push(currStructurePosition);
            currStructurePosition = br.Position;
        }

        /// <summary>
        /// End a structure.
        /// </summary>
        public void ES() {
            currStructurePosition = structurePositions.Pop();
        }

        /// <summary>
        /// Jump to an offset.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="offset">The offset.</param>
        public void JumpToOffset(BinaryDataReader br, int offset) {
            br.Position = currStructurePosition + offset;
        }

        /// <summary>
        /// Read a null terminatd string.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <returns>The string.</returns>
        public string ReadNullTerminated(BinaryDataReader br) {

            //Return the string.
            string ret = "";
            char c = br.ReadChar();
            while (c != 0) {
                ret += c;
                c = br.ReadChar();
            }
            return ret;

        }

    }

}
