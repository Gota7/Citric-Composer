using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFileLoader {

    /// <summary>
    /// Help with writing files.
    /// </summary>
    public class WriteHelper {

        /// <summary>
        /// Structure positions.
        /// </summary>
        private Stack<long> structurePositions = new Stack<long>();

        /// <summary>
        /// Current structure position.
        /// </summary>
        private long currStructurePosition;

        /// <summary>
        /// Offset table positions.
        /// </summary>
        private Dictionary<string, long> offsetTablePositions = new Dictionary<string, long>();

        /// <summary>
        /// Helper for the writer.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public WriteHelper(BinaryDataWriter bw) {
            currStructurePosition = bw.Position;
        }

        /// <summary>
        /// Allocate memory to write structures to later.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="numStructures">Number of structures to allocate.</param>
        /// <param name="sizePerStructure">Size per each structure.</param>
        public void AllocateStructures(BinaryDataWriter bw, int numStructures, uint sizePerStructure) {
            bw.Write(new byte[numStructures * sizePerStructure]);   
        }

        /// <summary>
        /// Initialize an offset table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of the table.</param>
        public void InitOffsetTable(BinaryDataWriter bw, string name) {
            offsetTablePositions.Add(name, bw.Position);
        }

        /// <summary>
        /// Write an offset table entry.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="index">Number offset to write.</param>
        /// <param name="name">Name of the table.</param>
        /// <param name="offsetModifier">Value to add to the offset.</param>
        public void WriteOffsetTableEntry(BinaryDataWriter bw, int index, string name, int offsetModifier = 0) {
            long bakPos = bw.Position;
            bw.Position = offsetTablePositions[name] + 4 * index;
            bw.Write((Int32)(bakPos - currStructurePosition) + offsetModifier);
            bw.Position = bakPos;
        }

        /// <summary>
        /// Close an offset table.
        /// </summary>
        /// <param name="name">Name of the table.</param>
        public void CloseOffsetTable(string name) {
            offsetTablePositions.Remove(name);
        }

        /// <summary>
        /// Write an offset manually.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="offsetPos">Offset position.</param>
        /// <param name="offset">Actual offset to write.</param>
        public void WriteManualOffset(BinaryDataWriter bw, long offsetPos, int offset) {
            long bakPos = bw.Position;
            bw.Position = offsetPos;
            bw.Write(offset);
            bw.Position = bakPos;
        }

        /// <summary>
        /// Start a structure.
        /// </summary>
        /// <param name="br">The Wwiter.</param>
        public void SS(BinaryDataWriter bw) {
            structurePositions.Push(currStructurePosition);
            currStructurePosition = bw.Position;
        }

        /// <summary>
        /// End a structure.
        /// </summary>
        public void ES() {
            currStructurePosition = structurePositions.Pop();
        }

        /// <summary>
        /// Write a null terminated string.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="str">String to write.</param>
        public void WriteNullTerminated(BinaryDataWriter bw, string str) {
            bw.Write((str + "\0").ToCharArray());
        }

    }

}
