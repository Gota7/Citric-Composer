using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// File reader.
    /// </summary>
    public class FileReader {

        /// <summary>
        /// Block offsets.
        /// </summary>
        private List<Int32> blockOffsets = new List<int>();

        /// <summary>
        /// Start position.
        /// </summary>
        private long fileStartPos;

        /// <summary>
        /// File size.
        /// </summary>
        private uint fileSize;

        /// <summary>
        /// Block position.
        /// </summary>
        private long blockPos;

        /// <summary>
        /// Block size.
        /// </summary>
        private uint blockSize;

        /// <summary>
        /// Structure position.
        /// </summary>
        private long structurePos;

        /// <summary>
        /// Structure positions.
        /// </summary>
        private Stack<long> structurePositions = new Stack<long>();

        /// <summary>
        /// Sized reference tables.
        /// </summary>
        private Dictionary<string, ReferenceStructures.SizedReferenceTableInfo> sizedReferenceTables = new Dictionary<string, ReferenceStructures.SizedReferenceTableInfo>();

        /// <summary>
        /// References.
        /// </summary>
        private Dictionary<string, ReferenceStructures.ReferenceInfo> references = new Dictionary<string, ReferenceStructures.ReferenceInfo>();

        /// <summary>
        /// Reference tables.
        /// </summary>
        private Dictionary<string, ReferenceStructures.ReferenceTableInfo> referenceTables = new Dictionary<string, ReferenceStructures.ReferenceTableInfo>();

        /// <summary>
        /// Sized references.
        /// </summary>
        private Dictionary<string, ReferenceStructures.SizedReferenceInfo> sizedReferences = new Dictionary<string, ReferenceStructures.SizedReferenceInfo>();

        /// <summary>
        /// Open file.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="version">Version.</param>
        public void OpenFile(BinaryDataReader br, out WriteMode writeMode, out FileWriter.Version version) {

            //Set up variables.
            blockOffsets = new List<int>();
            fileStartPos = br.Position;
            sizedReferenceTables = new Dictionary<string, ReferenceStructures.SizedReferenceTableInfo>();
            referenceTables = new Dictionary<string, ReferenceStructures.ReferenceTableInfo>();
            references = new Dictionary<string, ReferenceStructures.ReferenceInfo>();
            sizedReferences = new Dictionary<string, ReferenceStructures.SizedReferenceInfo>();
            structurePositions = new Stack<long>();

            //Get F or C.
            char identifier = br.ReadChar();

            //Skip rest of magic.
            br.Position += 3;

            //Get byte order.
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            bool bigEndian = true;
            if (br.ReadUInt16() == ByteOrder.LittleEndian) {

                //Little endian.
                bigEndian = false;
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;

            }

            //Get header size.
            UInt16 headerSize = br.ReadUInt16();

            //Get version.
            version = new FileWriter.Version(br.ReadUInt32(), identifier);

            //Get size.
            fileSize = br.ReadUInt32();

            //Number of blocks and padding.
            UInt16 numBlocks = br.ReadUInt16();
            br.ReadUInt16();

            //Read offsets.
            for (int i = 0; i < numBlocks; i++) {
                br.ReadUInt32();
                blockOffsets.Add(br.ReadInt32());
                br.ReadUInt32();
            }

            //If the format is an F type.
            bool FFiles = identifier == 'F';

            //Determine the write mode.
            writeMode = WriteMode.Cafe;
            if (FFiles && !bigEndian) {
                writeMode = WriteMode.NX;
            }
            if (!FFiles && !bigEndian) {
                writeMode = WriteMode.CTR;
            }
            if (!FFiles && bigEndian) {
                writeMode = WriteMode.C_BE;
            }

            //Set the position.
            br.Position = fileStartPos + headerSize;

        }

        /// <summary>
        /// Close the file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void CloseFile(BinaryDataReader br) {

            //Set position to the end.
            br.Position = fileStartPos + fileSize;

        }

        /// <summary>
        /// Open a block.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="blockNum">The block number.</param>
        public void OpenBlock(BinaryDataReader br, int blockNum) {

            //Set position.
            br.Position = fileStartPos + blockOffsets[blockNum];

            //Set block position.
            blockPos = br.Position;

            //Skip magic.
            br.ReadUInt32();

            //Block size.
            blockSize = br.ReadUInt32();

            //Set the structure position.
            structurePos = br.Position;

        }

        /// <summary>
        /// If a block is null.
        /// </summary>
        /// <param name="blockNum">Block number.</param>
        /// <returns>If the block is null.</returns>
        public bool IsBlockNull(int blockNum) {

            //Block offset is null.
            return blockOffsets[blockNum] == -1;

        }

        /// <summary>
        /// Close the block.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void CloseBlock(BinaryDataReader br) {

            //Set position to the end.
            br.Position = blockPos + blockSize;

        }

        /// <summary>
        /// Go until the reader has seeked to a place divisible by the align.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="alignBy">Factor to align by.</param>
        public void SeekAlign(BinaryDataReader br, int alignBy) {

            //Increment position until it can be aligned.
            while ((br.Position - fileStartPos) % alignBy != 0) {
                br.Position++;
            }

        }

        /// <summary>
        /// Read a sound file. DONT USE.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="type">Type of sound file to read.</param>
        /// <param name="alignBy">Amount to align by after reading the file.</param>
        /// <returns>The read file.</returns>
        public ISoundFile ReadFile(BinaryDataReader br, Type type, int alignBy = 1) {

            //Read the file.
            ISoundFile f = (ISoundFile)Activator.CreateInstance(type);
            f.Read(br);

            //Align.
            SeekAlign(br, alignBy);

            //Return the sound file.
            return f;

        }

        /// <summary>
        /// Open a sized reference table.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="name">Name of the table to add.</param>
        public void OpenSizedReferenceTable(BinaryDataReader br, string name) {

            //Add the reference table.
            sizedReferenceTables.Add(name, ReferenceStructures.SizedReferenceTable.Read(br));

        }

        /// <summary>
        /// Check if a sized reference table reference is null.
        /// </summary>
        /// <param name="index">Reference number to check.</param>
        /// <param name="name">Name of the table.</param>
        /// <returns>If the reference is null.</returns>
        public bool SizedReferenceTableReferenceIsNull(int index, string name) {

            //Return if it is a null reference.
            return sizedReferenceTables[name].IsNullReference(index);

        }

        /// <summary>
        /// Jump to where a reference points.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="index">Index of the reference.</param>
        /// <param name="name">Name of the sized table.</param>
        public void SizedReferenceTableJumpToReference(BinaryDataReader br, int index, string name) {

            //Jump to the offset.
            br.Position = structurePos + sizedReferenceTables[name].offsets[index];

        }

        /// <summary>
        /// Get the size of an entry in a sized reference table.
        /// </summary>
        /// <param name="index">Index of the reference.</param>
        /// <param name="name">Name of the sized table.</param>
        /// <returns>The size.</returns>
        public uint SizedReferenceTableGetSize(int index, string name) {

            //Get the size.
            return sizedReferenceTables[name].sizes[index];

        }

        /// <summary>
        /// Get the number of items in a sized reference table.
        /// </summary>
        /// <param name="name">The sized table name.</param>
        /// <returns>The number of references.</returns>
        public int SizedReferenceTableCount(string name) {

            //Return the number of references.
            return sizedReferenceTables[name].max;

        }

        /// <summary>
        /// Close a sized reference table.
        /// </summary>
        /// <param name="name">Name of the sized table.</param>
        public void CloseSizedReferenceTable(string name) {

            //Remove the table.
            sizedReferenceTables.Remove(name);

        }

        /// <summary>
        /// Open a reference table.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="name">Name of the table to add.</param>
        public void OpenReferenceTable(BinaryDataReader br, string name) {

            //Add the reference table.
            referenceTables.Add(name, ReferenceStructures.ReferenceTable.Read(br));

        }

        /// <summary>
        /// Check if a reference table reference is null.
        /// </summary>
        /// <param name="index">Reference number to check.</param>
        /// <param name="name">Name of the table.</param>
        /// <returns>If the reference is null.</returns>
        public bool ReferenceTableReferenceIsNull(int index, string name) {

            //Return if it is a null reference.
            return referenceTables[name].IsNullReference(index);

        }

        /// <summary>
        /// Jump to where a reference points.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="index">Index of the reference.</param>
        /// <param name="name">Name of the table.</param>
        public void ReferenceTableJumpToReference(BinaryDataReader br, int index, string name) {

            //Jump to the offset.
            br.Position = structurePos + referenceTables[name].offsets[index];

        }

        /// <summary>
        /// Get the number of items in a reference table.
        /// </summary>
        /// <param name="name">The sized table name.</param>
        /// <returns>The number of references.</returns>
        public int ReferenceTableCount(string name) {

            //Return the number of references.
            return referenceTables[name].max;

        }

        /// <summary>
        /// Close a reference table.
        /// </summary>
        /// <param name="name">Name of the sized table.</param>
        public void CloseReferenceTable(string name) {

            //Remove the table.
            referenceTables.Remove(name);

        }

        /// <summary>
        /// Open a reference. 
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="name">Name of the reference.</param>
        public void OpenReference(BinaryDataReader br, string name) {

            //Add a reference.
            references.Add(name, ReferenceStructures.Reference.Read(br));

        }

        /// <summary>
        /// Check if a reference is null.
        /// </summary>
        /// <param name="name">Name of the reference.</param>
        /// <returns>If the reference is null.</returns>
        public bool ReferenceIsNull(string name) {

            //If a refrence is null.
            return references[name].IsNullOffset();

        }

        /// <summary>
        /// Jump to a reference.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="name">Name of the reference to jump to.</param>
        public void JumpToReference(BinaryDataReader br, string name) {

            //Jump to the offset.
            br.Position = structurePos + references[name].Offset;

        }

        /// <summary>
        /// Get reference type.
        /// </summary>
        /// <param name="name">Name of the reference.</param>
        /// <returns>The reference type.</returns>
        public UInt16 ReferenceGetType(string name) {

            //Get the type.
            return references[name].ReferenceType;

        }

        /// <summary>
        /// Get reference offset.
        /// </summary>
        /// <param name="name">Name of the reference.</param>
        /// <returns>The reference offset.</returns>
        public Int32 ReferenceGetOffset(string name) {

            //Get the type.
            return references[name].Offset;

        }

        /// <summary>
        /// Close a reference.
        /// </summary>
        /// <param name="name">Name of the reference.</param>
        public void CloseReference(string name) {

            //Remove the reference.
            references.Remove(name);

        }

        /// <summary>
        /// Open a sized reference. 
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="name">Name of the sized reference.</param>
        public void OpenSizedReference(BinaryDataReader br, string name) {

            //Add a reference.
            sizedReferences.Add(name, ReferenceStructures.SizedReference.Read(br));

        }

        /// <summary>
        /// Check if a sized reference is null.
        /// </summary>
        /// <param name="name">Name of the sized reference.</param>
        /// <returns>If the sized reference is null.</returns>
        public bool SizedReferenceIsNull(string name) {

            //If a refrence is null.
            return sizedReferences[name].IsNullOffset();

        }

        /// <summary>
        /// Jump to a sized reference.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="name">Name of the sized reference to jump to.</param>
        public void JumpToSizedReference(BinaryDataReader br, string name) {

            //Jump to the offset.
            br.Position = structurePos + sizedReferences[name].Offset;

        }

        /// <summary>
        /// Get sized reference type.
        /// </summary>
        /// <param name="name">Name of the sized reference.</param>
        /// <returns>The sized reference type.</returns>
        public UInt16 SizedReferenceGetType(string name) {

            //Get the type.
            return sizedReferences[name].ReferenceType;

        }

        /// <summary>
        /// Get sized reference offset.
        /// </summary>
        /// <param name="name">Name of the sized reference.</param>
        /// <returns>The sized reference offset.</returns>
        public Int32 SizedReferenceGetOffset(string name) {

            //Get the type.
            return sizedReferences[name].Offset;

        }

        /// <summary>
        /// Close a sized reference.
        /// </summary>
        /// <param name="name">Name of sized the reference.</param>
        public void CloseSizedReference(string name) {

            //Remove the reference.
            sizedReferences.Remove(name);

        }

        /// <summary>
        /// Jump to the offset manually.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="offset">Offset to jump to.</param>
        public void JumpToOffsetManually(BinaryDataReader br, int offset) {

            //Jump to offset.
            br.Position = structurePos + offset;

        }

        /// <summary>
        /// Start a new structure.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void StartStructure(BinaryDataReader br) {

            //Store the old structure position.
            structurePositions.Push(structurePos);

            //Set the new structure position.
            structurePos = br.Position;

        }

        /// <summary>
        /// End a structure.
        /// </summary>
        public void EndStructure() {

            //Restore the latest structure position.
            structurePos = structurePositions.Pop();

        }

    }

}
