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
    public class FileWriter {

        /// <summary>
        /// Header position.
        /// </summary>
        private long headerPos;

        /// <summary>
        /// Max number of blocks.
        /// </summary>
        private int maxBlocks;

        /// <summary>
        /// Block types.
        /// </summary>
        private List<UInt16> blockTypes = new List<ushort>();

        /// <summary>
        /// Block lengths.
        /// </summary>
        private List<UInt32> blockLengths = new List<uint>();

        /// <summary>
        /// Header size.
        /// </summary>
        private UInt16 headerSize;

        /// <summary>
        /// Block position.
        /// </summary>
        private long blockPos;

        /// <summary>
        /// Location of the last structure.
        /// </summary>
        private long structurePos;

        /// <summary>
        /// Structure positions.
        /// </summary>
        private Stack<long> structurePositions = new Stack<long>();

        /// <summary>
        /// Sized reference table info.
        /// </summary>
        private Dictionary<string, ReferenceStructures.SizedReferenceTableInfo> sizedReferenceTables = new Dictionary<string, ReferenceStructures.SizedReferenceTableInfo>();

        /// <summary>
        /// Reference table info.
        /// </summary>
        private Dictionary<string, ReferenceStructures.ReferenceTableInfo> referenceTables = new Dictionary<string, ReferenceStructures.ReferenceTableInfo>();

        /// <summary>
        /// References.
        /// </summary>
        private Dictionary<string, ReferenceStructures.ReferenceInfo> references = new Dictionary<string, ReferenceStructures.ReferenceInfo>();

        /// <summary>
        /// Sized references.
        /// </summary>
        private Dictionary<string, ReferenceStructures.SizedReferenceInfo> sizedReferences = new Dictionary<string, ReferenceStructures.SizedReferenceInfo>();

        /// <summary>
        /// Version.
        /// </summary>
        public class Version : IComparable<Version> {

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

            /// <summary>
            /// Compare to another version.
            /// </summary>
            /// <param name="other">Other version to compare to.</param>
            /// <returns>The version.</returns>
            public int CompareTo(Version other) {

                //Majors match.
                if (Major == other.Major) {

                    //Minors match.
                    if (Minor == other.Minor) {

                        //Revisions match.
                        if (Revision == other.Revision) {
                            return 0;
                        }

                        //Greater revision.
                        else if (Revision > other.Revision) {
                            return 1;
                        }

                        //Lesser revision.
                        else {
                            return -1;
                        }

                    }

                    //Greater minor.
                    else if (Minor > other.Minor) {
                        return 2;
                    }

                    //Lesser minor.
                    else {
                        return -2;
                    }

                }

                //Greater major.
                else if (Major > other.Major) {
                    return 3;
                }

                //Lesser major.
                else {
                    return -3;
                }

            }

            public static bool operator <(Version v1, Version v2) {
                return v1.CompareTo(v2) < 0;
            }

            public static bool operator >(Version v1, Version v2) {
                return v1.CompareTo(v2) > 0;
            }

            public static bool operator <=(Version v1, Version v2) {
                return v1.CompareTo(v2) <= 0;
            }

            public static bool operator >=(Version v1, Version v2) {
                return v1.CompareTo(v2) >= 0;
            }

            public static bool operator ==(Version v1, Version v2) {
                return v1.CompareTo(v2) == 0;
            }

            public static bool operator !=(Version v1, Version v2) {
                return v1.CompareTo(v2) != 0;
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
        public void InitFile(BinaryDataWriter bw, WriteMode writeMode, string extension, int numBlocks, Version version) {

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
        public void CloseFile(BinaryDataWriter bw) {

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

                //Null block.
                if (blockLengths[i] == 0) {
                    bw.Write((Int32)(-1));
                    bw.Write((Int32)(-1));
                }

                //Valid block.
                else {
                    bw.Write(sizeFromFile);
                    bw.Write(blockLengths[i]);
                }

                sizeFromFile += blockLengths[i];
            }

            //Restore writer to the end.
            bw.Position = headerPos + totalSize;

        }

        /// <summary>
        /// Initialize a block.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="referenceType">Reference type to the block.</param>
        /// <param name="magic">4-letter magic identifier.</param>
        public void InitBlock(BinaryDataWriter bw, UInt16 referenceType, string magic) {

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
        public void CloseBlock(BinaryDataWriter bw) {

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
        public void WriteNullBlock(ushort type = 0) {

            //Simply just add to the block list.
            if (blockTypes.Count < maxBlocks) {
                blockTypes.Add(type);             
                blockLengths.Add(0);
            }

        }

        /// <summary>
        /// While inside a block, write X amount until block size is divisible by the number to align by.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="alignBy">Number to align the block to.</param>
        public void Align(BinaryDataWriter bw, int alignBy) {

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
        public UInt32 WriteFile(BinaryDataWriter bw, ISoundFile file, int alignBy = 1, WriteMode? writeMode = null) {

            //Old pos.
            long oldPos = bw.Position;

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw2 = new BinaryDataWriter(o);
            if (writeMode == null) {
                file.Write(bw2);
            } else {
                file.Write(writeMode.GetValueOrDefault(), bw2);
            }
            bw.Write(o.ToArray());

            //New pos.
            long newPos = bw.Position;

            //Align.
            Align(bw, alignBy);

            //Free memory.
            bw2.Dispose();

            //Return size.
            return (UInt32)(newPos - oldPos);

        }


        /// <summary>
        /// Initialize a sized reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="count">Number of references to add.</param>
        /// <param name="name">Name of the reference table.</param>
        public void InitSizedReferenceTable(BinaryDataWriter bw, int count, string name) {

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
        public void AddSizedReferenceTableReference(BinaryDataWriter bw, UInt16 referenceType, UInt32 size, string name) {

            //Add reference to the sized reference table.
            sizedReferenceTables[name].Add(referenceType, (int)(bw.Position - sizedReferenceTables[name].structurePos - blockPos), size);

        }

        /// <summary>
        /// Add a null reference to the sized reference table.
        /// </summary>
        /// <param name="name">Name of the reference table.</param>
        public void AddSizedReferenceTableNullReference(string name) {

            //Add the null reference.
            sizedReferenceTables[name].Add(0, -1, 0);

        }

        /// <summary>
        /// Adjust the sized referenced table size for the last reference added.
        /// </summary>
        /// <param name="newSize">New size.</param>
        /// <param name="name">Name of the reference to add.</param>
        public void AdjustSizedReferenceTableSize(UInt32 newSize, string name) {

            //Adjust the new size.
            sizedReferenceTables[name].AdjustSize(newSize);

        }

        /// <summary>
        /// Close a sized reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">The name of the reference table.</param>
        public void CloseSizedReferenceTable(BinaryDataWriter bw, string name) {

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
        public void InitReferenceTable(BinaryDataWriter bw, int count, string name) {

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
        public void AddReferenceTableReference(BinaryDataWriter bw, UInt16 referenceType, string name) {

            //Add reference to the reference table.
            referenceTables[name].Add(referenceType, (int)(bw.Position - referenceTables[name].structurePos - blockPos));

        }

        /// <summary>
        /// Add a null reference to the reference table.
        /// </summary>
        /// <param name="name">Name of the reference table.</param>
        public void AddReferenceTableNullReference(string name) {

            //Add the null reference.
            referenceTables[name].Add(0, -1);

        }

        /// <summary>
        /// Add a null reference to the reference table.
        /// </summary>
        /// <param name="referenceType">Type of reference to add.</param>
        /// <param name="name">Name of the reference table.</param>
        public void AddReferenceTableNullReferenceWithType(UInt16 referenceType, string name) {

            //Add the null reference.
            referenceTables[name].Add(referenceType, -1);

        }

        /// <summary>
        /// Close a reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">The name of the reference table.</param>
        public void CloseReferenceTable(BinaryDataWriter bw, string name) {

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
        public void InitReference(BinaryDataWriter bw, string name) {

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
        public void CloseReference(BinaryDataWriter bw, UInt16 type, string name) {

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
        public void CloseNullReferenceWithType(BinaryDataWriter bw, UInt16 type, string name) {

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
        public void CloseNullReference(BinaryDataWriter bw, string name) {

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
        public void InitSizedReference(BinaryDataWriter bw, string name) {

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
        public void CloseSizedReference(BinaryDataWriter bw, UInt16 type, UInt32 size, string name) {

            //Close reference.
            ReferenceStructures.SizedReference.Close(bw, type, (int)(bw.Position - sizedReferences[name].structurePos - blockPos - size), size, sizedReferences[name].pos);

            //Remove reference.
            sizedReferences.Remove(name);

        }

        /// <summary>
        /// Close sized reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        /// <param name="type">Type.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="size">Size.</param>
        public void CloseSizedReference(BinaryDataWriter bw, UInt16 type, Int32 offset, UInt32 size, string name) {

            //Close reference.
            ReferenceStructures.SizedReference.Close(bw, type, offset, size, sizedReferences[name].pos);

            //Remove reference.
            sizedReferences.Remove(name);

        }

        /// <summary>
        /// Close sized null reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="name">Name of reference.</param>
        public void CloseSizedNullReference(BinaryDataWriter bw, string name) {

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
        public Int32 GetOffsetManually(BinaryDataWriter bw) {

            //Return the position.
            return (int)(bw.Position - structurePos);

        }

        /// <summary>
        /// Start a new structure.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void StartStructure(BinaryDataWriter bw) {

            //Store the old structure position.
            structurePositions.Push(structurePos);

            //Set the new structure position.
            structurePos = bw.Position;

        }

        /// <summary>
        /// End a structure.
        /// </summary>
        public void EndStructure() {

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
        public Syroot.BinaryData.ByteOrder GetByteOrder(WriteMode w) {

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
