using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// File writer.
    /// </summary>
    public static class FileWriter {

        /// <summary>
        /// Header position.
        /// </summary>
        private static long headerPos;

        /// <summary>
        /// Max number of blocks.
        /// </summary>
        private static int maxBlocks;

        /// <summary>
        /// Block types.
        /// </summary>
        private static List<UInt16> blockTypes = new List<ushort>();

        /// <summary>
        /// Block lengths.
        /// </summary>
        private static List<UInt32> blockLengths = new List<uint>();

        /// <summary>
        /// Header size.
        /// </summary>
        private static UInt16 headerSize;

        /// <summary>
        /// Block position.
        /// </summary>
        private static long blockPos;

        /// <summary>
        /// Location of the last structure.
        /// </summary>
        private static long structurePos;

        /// <summary>
        /// Structure positions.
        /// </summary>
        private static Stack<long> structurePositions = new Stack<long>();

        /// <summary>
        /// Sized reference table info.
        /// </summary>
        private static Dictionary<string, ReferenceStructures.SizedReferenceTableInfo> sizedReferenceTables = new Dictionary<string, ReferenceStructures.SizedReferenceTableInfo>();

        /// <summary>
        /// Reference table info.
        /// </summary>
        private static Dictionary<string, ReferenceStructures.ReferenceTableInfo> referenceTables = new Dictionary<string, ReferenceStructures.ReferenceTableInfo>();

        /// <summary>
        /// References.
        /// </summary>
        private static Dictionary<string, ReferenceStructures.ReferenceInfo> references = new Dictionary<string, ReferenceStructures.ReferenceInfo>();

        /// <summary>
        /// Sized references.
        /// </summary>
        private static Dictionary<string, ReferenceStructures.SizedReferenceInfo> sizedReferences = new Dictionary<string, ReferenceStructures.SizedReferenceInfo>();

        //Backups.
        private static long bakBlockPos;
        private static long bakStructurePos;
        private static long[] bakStructurePositions;
        private static KeyValuePair<string, ReferenceStructures.SizedReferenceTableInfo>[] bakSizedReferenceTables;
        private static KeyValuePair<string, ReferenceStructures.ReferenceTableInfo>[] bakReferenceTables;
        private static KeyValuePair<string, ReferenceStructures.ReferenceInfo>[] bakReferences;
        private static KeyValuePair<string, ReferenceStructures.SizedReferenceInfo>[] bakSizedReferences;
        private static UInt16[] bakBlockTypes;
        private static UInt32[] bakBlockLengths;
        private static long bakPos;
        private static int bakMaxBlocks;

        /// <summary>
        /// Version.
        /// </summary>
        public class Version {

            public byte Major;
            public byte Minor;
            public byte Revision;

            public Version(byte major, byte minor, byte revision) {
                Major = major;
                Minor = minor;
                Revision = revision;
            }

            public Version(uint data, char identifier) {

                if (identifier == 'F') {
                    Major = (byte)((data & 0xFF0000) >> 16);
                    Minor = (byte)((data & 0xFF00) >> 8);
                    Revision = (byte)(data & 0xFF);
                } else {
                    Major = (byte)((data & 0xFF000000) >> 24);
                    Minor = (byte)((data & 0xFF0000) >> 16);
                    Revision = (byte)((data & 0xFF00) >> 8);
                }

            }

            public UInt32 GetWriteableVersion(WriteMode w) {

                UInt32 ret = 0;

                switch (w) {

                    case WriteMode.Cafe:
                    case WriteMode.NX:
                        ret = Revision;
                        ret += (uint)Minor << 8;
                        ret += (uint)Major << 16;
                        break;

                    default:
                        ret += (uint)Major << 24;
                        ret += (uint)Minor << 16;
                        ret += (uint)Revision << 8;
                        break;

                }

                return ret;

            }

        }


        /// <summary>
        /// Initialize file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <param name="numBlocks">Number of blocks.</param>
        /// <param name="version">File version.</param>
        public static void InitFile(BinaryDataWriter bw, WriteMode writeMode, string extension, int numBlocks, Version version) {

            //Make backups of everything.
            bakBlockPos = blockPos;
            bakStructurePos = structurePos;
            bakStructurePositions = structurePositions.ToArray();
            bakSizedReferenceTables = sizedReferenceTables.ToArray();
            bakReferenceTables = referenceTables.ToArray();
            bakSizedReferences = sizedReferences.ToArray();
            bakReferences = references.ToArray();
            bakBlockLengths = blockLengths.ToArray();
            bakBlockTypes = blockTypes.ToArray();
            bakPos = bw.Position;
            bakMaxBlocks = maxBlocks;

            //Reset the values.
            headerPos = bw.Position;
            maxBlocks = numBlocks;
            blockTypes = new List<ushort>();
            blockLengths = new List<uint>();
            sizedReferenceTables = new Dictionary<string, ReferenceStructures.SizedReferenceTableInfo>();
            referenceTables = new Dictionary<string, ReferenceStructures.ReferenceTableInfo>();

            //Write the magic.
            bw.Write((GetWriteModeChar(writeMode) + extension).ToCharArray());

            //Byte order.
            bw.ByteOrder = GetByteOrder(writeMode);
            bw.Write((UInt16)0xFEFF);

            //Header size.
            headerSize = (UInt16)(20 + numBlocks * 12);
            int extraNums = 0;
            while (headerSize % 0x20 != 0) {
                headerSize++;
                extraNums++;
            }
            bw.Write(headerSize);

            //Version.
            bw.Write(version.GetWriteableVersion(writeMode));

            //Size.
            bw.Write((UInt32)0);

            //Number of blocks.
            bw.Write((UInt16)numBlocks);

            //Padding.
            bw.Write((UInt16)0);

            //Block offsets.
            for (int i = 0; i < numBlocks; i++) {
                bw.Write((UInt32)0);
                bw.Write((UInt64)0);
            }

            //Padding.
            bw.Write(new byte[extraNums]);

        }

        /// <summary>
        /// Close a file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public static void CloseFile(BinaryDataWriter bw) {

            //Get the total length.
            UInt32 totalSize = headerSize + (uint)blockLengths.Sum(x => x);

            //Write the size.
            bw.Position = headerPos + 12;
            bw.Write(totalSize);

            //Write the blocks.
            bw.Position += 4;
            uint sizeFromFile = headerSize;
            for (int i = 0; i < blockTypes.Count; i++) {
                bw.Write(blockTypes[i]);
                bw.Write((UInt16)0);
                bw.Write(sizeFromFile);
                bw.Write(blockLengths[i]);
                sizeFromFile += blockLengths[i];
            }

            //Restore writer to the end.
            bw.Position = headerPos + totalSize;

            //Restore backup.
            blockPos = bakBlockPos;
            structurePos = bakStructurePos;
            structurePositions = new Stack<long>(bakStructurePositions);
            sizedReferenceTables = new Dictionary<string, ReferenceStructures.SizedReferenceTableInfo>();
            foreach (var h0 in bakSizedReferenceTables) {
                sizedReferenceTables.Add(h0.Key, h0.Value);
            }
            referenceTables = new Dictionary<string, ReferenceStructures.ReferenceTableInfo>();
            foreach (var h0 in bakReferenceTables) {
                referenceTables.Add(h0.Key, h0.Value);
            }
            sizedReferences = new Dictionary<string, ReferenceStructures.SizedReferenceInfo>();
            foreach (var h0 in bakSizedReferences) {
                sizedReferences.Add(h0.Key, h0.Value);
            }
            references = new Dictionary<string, ReferenceStructures.ReferenceInfo>();
            foreach (var h0 in bakReferences) {
                references.Add(h0.Key, h0.Value);
            }
            blockLengths = bakBlockLengths.ToList();
            blockTypes = bakBlockTypes.ToList();
            maxBlocks = bakMaxBlocks;

        }

        /// <summary>
        /// Initialize a block.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="referenceType">Reference type to the block.</param>
        /// <param name="magic">4-letter magic identifier.</param>
        public static void InitBlock(BinaryDataWriter bw, UInt16 referenceType, string magic) {

            //If max not reached.
            if (blockTypes.Count() < maxBlocks) {

                //Set position.
                blockPos = bw.Position;

                //Set structure.
                structurePos = blockPos + 8;

                //Add reference type.
                blockTypes.Add(referenceType);

                //Write the magic.
                bw.Write(magic.ToCharArray());

                //Write the size.
                bw.Write((UInt32)0);

            }

        }

        /// <summary>
        /// Close the block.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public static void CloseBlock(BinaryDataWriter bw) {

            //Get size.
            UInt32 blockSize = (UInt32)(bw.Position - blockPos);

            //Write the size.
            bw.Position = blockPos + 4;
            bw.Write(blockSize);

            //Add the size.
            blockLengths.Add(blockSize);

            //Return to normal position.
            bw.Position = blockPos + blockSize;

        }

        /// <summary>
        /// Write a null block.
        /// </summary>
        public static void WriteNullBlock() {

            //Simply just add to the block list.
            if (blockTypes.Count < maxBlocks) {
                blockTypes.Add(0);
                blockLengths.Add(0);
            }

        }

        /// <summary>
        /// While inside a block, write X amount until block size is divisible by the number to align by.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="alignBy">Number to align the block to.</param>
        public static void Align(BinaryDataWriter bw, int alignBy) {

            //Align.
            while ((bw.Position - blockPos) % alignBy != 0) {
                bw.Write((byte)0);
            }

        }

        /// <summary>
        /// Write a raw file, within a block.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="file">File to write.</param>
        /// <param name="alignBy">After writing the file, align a block to be divisible by a certain number.</param>
        /// <returns>Size of the file.</returns>
        public static UInt32 WriteFile(BinaryDataWriter bw, ISoundFile file, int alignBy = 1) {

            //Old pos.
            long oldPos = bw.Position;

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw2 = new BinaryDataWriter(o);
            file.Write(bw2);
            bw.Write(o.ToArray());

            //New pos.
            long newPos = bw.Position;

            //Align.
            Align(bw, alignBy);

            //Return size.
            return (UInt32)(newPos - oldPos);

        }


        /// <summary>
        /// Initialize a sized reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="count">Number of references to add.</param>
        /// <param name="name">Name of the reference table.</param>
        public static void InitSizedReferenceTable(BinaryDataWriter bw, int count, string name) {

            //Init a sized reference table.
            long tablePos = bw.Position;
            ReferenceStructures.SizedReferenceTable.Init(bw, count);

            //Add the reference table info to the reference tables.
            sizedReferenceTables.Add(name, new ReferenceStructures.SizedReferenceTableInfo(count, tablePos, structurePos - blockPos));

        }

        /// <summary>
        /// Add a reference to a sized reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="referenceType">Type of reference to add.</param>
        /// <param name="size">Size of the object that is referenced.</param>
        /// <param name="name">Name of the reference table.</param>
        public static void AddSizedReferenceTableReference(BinaryDataWriter bw, UInt16 referenceType, UInt32 size, string name) {

            //Add reference to the sized reference table.
            sizedReferenceTables[name].Add(referenceType, (int)(bw.Position - sizedReferenceTables[name].structurePos - blockPos), size);

        }

        /// <summary>
        /// Add a null reference to the sized reference table.
        /// </summary>
        /// <param name="name">Name of the reference table.</param>
        public static void AddSizedReferenceTableNullReference(string name) {

            //Add the null reference.
            sizedReferenceTables[name].Add(0, -1, 0);

        }

        /// <summary>
        /// Adjust the sized referenced table size for the last reference added.
        /// </summary>
        /// <param name="newSize">New size.</param>
        /// <param name="name">Name of the reference to add.</param>
        public static void AdjustSizedReferenceTableSize(UInt32 newSize, string name) {

            //Adjust the new size.
            sizedReferenceTables[name].AdjustSize(newSize);

        }

        /// <summary>
        /// Close a sized reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">The name of the reference table.</param>
        public static void CloseSizedReferenceTable(BinaryDataWriter bw, string name) {

            //Close the sized reference table.
            var s = sizedReferenceTables[name];
            ReferenceStructures.SizedReferenceTable.Close(bw, s.referenceTypes, s.offsets, s.sizes, s.tablePos);

            //Delete the table.
            sizedReferenceTables.Remove(name);

        }

        /// <summary>
        /// Initialize a reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="count">Number of references to add.</param>
        /// <param name="name">Name of the reference table.</param>
        public static void InitReferenceTable(BinaryDataWriter bw, int count, string name) {

            //Init a reference table.
            long tablePos = bw.Position;
            ReferenceStructures.ReferenceTable.Init(bw, count);

            //Add the reference table info to the reference tables.
            referenceTables.Add(name, new ReferenceStructures.ReferenceTableInfo(count, tablePos, structurePos - blockPos));

        }

        /// <summary>
        /// Add a reference to a reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="referenceType">Type of reference to add.</param>
        /// <param name="size">Size of the object that is referenced.</param>
        /// <param name="name">Name of the reference table.</param>
        public static void AddReferenceTableReference(BinaryDataWriter bw, UInt16 referenceType, string name) {

            //Add reference to the reference table.
            referenceTables[name].Add(referenceType, (int)(bw.Position - referenceTables[name].structurePos - blockPos));

        }

        /// <summary>
        /// Add a null reference to the reference table.
        /// </summary>
        /// <param name="name">Name of the reference table.</param>
        public static void AddReferenceTableNullReference(string name) {

            //Add the null reference.
            referenceTables[name].Add(0, -1);

        }

        /// <summary>
        /// Add a null reference to the reference table.
        /// </summary>
        /// <param name="referenceType">Type of reference to add.</param>
        /// <param name="name">Name of the reference table.</param>
        public static void AddReferenceTableNullReferenceWithType(UInt16 referenceType, string name) {

            //Add the null reference.
            referenceTables[name].Add(referenceType, -1);

        }

        /// <summary>
        /// Close a reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">The name of the reference table.</param>
        public static void CloseReferenceTable(BinaryDataWriter bw, string name) {

            //Close the sized reference table.
            var s = referenceTables[name];
            ReferenceStructures.ReferenceTable.Close(bw, s.referenceTypes, s.offsets, s.tablePos);

            //Delete the table.
            referenceTables.Remove(name);

        }

        /// <summary>
        /// Init reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">The name of the reference.</param>
        public static void InitReference(BinaryDataWriter bw, string name) {

            //Init a reference.
            long pos = bw.Position;
            ReferenceStructures.Reference.Init(bw);

            //Add reference.
            references.Add(name, new ReferenceStructures.ReferenceInfo(pos, structurePos - blockPos));

        }

        /// <summary>
        /// Close reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        /// <param name="type">Type.</param>
        public static void CloseReference(BinaryDataWriter bw, UInt16 type, string name) {

            //Close reference.
            ReferenceStructures.Reference.Close(bw, type, (int)(bw.Position - references[name].structurePos - blockPos), references[name].pos);

            //Remove reference.
            references.Remove(name);

        }

        /// <summary>
        /// Close a null reference that has a type.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        /// <param name="type">Type.</param>
        public static void CloseNullReferenceWithType(BinaryDataWriter bw, UInt16 type, string name) {

            //Close reference.
            ReferenceStructures.Reference.Close(bw, type, -1, references[name].pos);

            //Remove reference.
            references.Remove(name);

        }

        /// <summary>
        /// Close reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        public static void CloseNullReference(BinaryDataWriter bw, string name) {

            //Close reference.
            ReferenceStructures.Reference.Close(bw, 0, -1, references[name].pos);

            //Remove reference.
            references.Remove(name);

        }

        /// <summary>
        /// Init sized reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">The name of the reference.</param>
        public static void InitSizedReference(BinaryDataWriter bw, string name) {

            //Init a reference.
            long pos = bw.Position;
            ReferenceStructures.SizedReference.Init(bw);

            //Add reference.
            sizedReferences.Add(name, new ReferenceStructures.SizedReferenceInfo(pos, structurePos - blockPos));

        }

        /// <summary>
        /// Close sized reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        /// <param name="type">Type.</param>
        /// <param name="offset">Offset.</param>
        public static void CloseSizedReference(BinaryDataWriter bw, UInt16 type, UInt32 size, string name) {

            //Close reference.
            ReferenceStructures.SizedReference.Close(bw, type, (int)(bw.Position - sizedReferences[name].structurePos - blockPos - size), size, sizedReferences[name].pos);

            //Remove reference.
            sizedReferences.Remove(name);

        }

        /// <summary>
        /// Close sized null reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        public static void CloseSizedNullReference(BinaryDataWriter bw, string name) {

            //Close reference.
            ReferenceStructures.SizedReference.Close(bw, 0, -1, 0xFFFFFFFF, sizedReferences[name].pos);

            //Remove reference.
            sizedReferences.Remove(name);

        }

        /// <summary>
        /// Get the offset from the structure start manually.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <returns>Offset.</returns>
        public static Int32 GetOffsetManually(BinaryDataWriter bw) {

            //Return the position.
            return (int)(bw.Position - structurePos);

        }

        /// <summary>
        /// Start a new structure.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public static void StartStructure(BinaryDataWriter bw) {

            //Store the old structure position.
            structurePositions.Push(structurePos);

            //Set the new structure position.
            structurePos = bw.Position;

        }

        /// <summary>
        /// End a structure.
        /// </summary>
        public static void EndStructure() {

            //Restore the latest structure position.
            structurePos = structurePositions.Pop();

        }

        /// <summary>
        /// Get the write mode character.
        /// </summary>
        /// <param name="w">Write mode.</param>
        /// <returns>The char to write.</returns>
        public static char GetWriteModeChar(WriteMode w) {

            switch (w) {
                case WriteMode.C_BE:
                case WriteMode.CTR:
                    return 'C';

                default:
                    return 'F';
            }

        }

        /// <summary>
        /// Get the byte order from the write mode.
        /// </summary>
        /// <param name="w">The write mode.</param>
        /// <returns>The byte order.</returns>
        public static Syroot.BinaryData.ByteOrder GetByteOrder(WriteMode w) {

            switch (w) {

                //Big endian.
                case WriteMode.Cafe:
                case WriteMode.C_BE:
                    return Syroot.BinaryData.ByteOrder.BigEndian;

                //Rest is little.
                default:
                    return Syroot.BinaryData.ByteOrder.LittleEndian;

            }

        }

    }

}
