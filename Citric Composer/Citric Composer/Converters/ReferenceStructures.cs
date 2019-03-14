using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Reference structures.
    /// </summary>
    public static class ReferenceStructures {

        public class ReferenceInfo {

            public UInt16 ReferenceType;
            public Int32 Offset;
            public long pos { get; }
            public long structurePos { get; }

            public ReferenceInfo(BinaryDataReader br) {
                ReferenceType = br.ReadUInt16();
                br.ReadUInt16();
                Offset = br.ReadInt32();
            }

            public ReferenceInfo(long pos, long structurePos) {
                this.pos = pos;
                this.structurePos = structurePos;
            }

            public ReferenceInfo(UInt16 type, Int32 offset) {
                ReferenceType = type;
                Offset = offset;
            }

            public bool IsNullOffset() {
                return Offset == -1;
            }

        }

        public class SizedReferenceInfo {

            public UInt16 ReferenceType;
            public Int32 Offset;
            public UInt32 Size;
            public long pos { get; }
            public long structurePos { get; }

            public SizedReferenceInfo(BinaryDataReader br) {
                ReferenceType = br.ReadUInt16();
                br.ReadUInt16();
                Offset = br.ReadInt32();
                Size = br.ReadUInt32();
            }

            public SizedReferenceInfo(long pos, long structurePos) {
                this.pos = pos;
                this.structurePos = structurePos;
            }

            public SizedReferenceInfo(UInt16 type, Int32 offset, UInt32 size) {
                ReferenceType = type;
                Offset = offset;
                Size = size;
            }

            public bool IsNullOffset() {
                return Offset == -1;
            }

            public void AdjustSize(UInt32 newSize) {
                Size = newSize;
            }

        }

        public class SizedReferenceTableInfo {

            public List<UInt16> referenceTypes { get; }
            public List<Int32> offsets { get; }
            public List<UInt32> sizes { get; }
            public int max { get; }
            public long tablePos { get; }
            public long structurePos { get; }

            public SizedReferenceTableInfo(BinaryDataReader br) {

                tablePos = br.Position;
                max = (int)br.ReadUInt32();
                referenceTypes = new List<ushort>();
                offsets = new List<int>();
                sizes = new List<uint>();
                for (int i = 0; i < max; i++) {
                    referenceTypes.Add(br.ReadUInt16());
                    br.ReadUInt16();
                    offsets.Add(br.ReadInt32());
                    sizes.Add(br.ReadUInt32());
                }

            }

            public SizedReferenceTableInfo(int max, long tablePos, long structurePos) {
                referenceTypes = new List<ushort>();
                offsets = new List<int>();
                sizes = new List<uint>();
                this.max = max;
                this.tablePos = tablePos;
                this.structurePos = structurePos;
            }

            public void Add(UInt16 referenceType, Int32 offset, UInt32 size) {
                if (referenceTypes.Count < max) {
                    referenceTypes.Add(referenceType);
                    offsets.Add(offset);
                    sizes.Add(size);
                }
            }

            public void AdjustSize(UInt32 newSize) {
                sizes[sizes.Count - 1] = newSize;
            }

            public bool IsNullReference(int index) {
                return offsets[index] == -1;
            }

        }

        public class ReferenceTableInfo {

            public List<UInt16> referenceTypes { get; }
            public List<Int32> offsets { get; }
            public int max { get; }
            public long tablePos { get; }
            public long structurePos { get; }

            public ReferenceTableInfo(BinaryDataReader br) {

                tablePos = br.Position;
                max = (int)br.ReadUInt32();
                referenceTypes = new List<ushort>();
                offsets = new List<int>();
                for (int i = 0; i < max; i++) {
                    referenceTypes.Add(br.ReadUInt16());
                    br.ReadUInt16();
                    offsets.Add(br.ReadInt32());
                }

            }

            public ReferenceTableInfo(int max, long tablePos, long structurePos) {
                referenceTypes = new List<ushort>();
                offsets = new List<int>();
                this.max = max;
                this.tablePos = tablePos;
                this.structurePos = structurePos;
            }

            public void Add(UInt16 referenceType, Int32 offset) {
                if (referenceTypes.Count < max) {
                    referenceTypes.Add(referenceType);
                    offsets.Add(offset);
                }
            }

            public bool IsNullReference(int index) {
                return offsets[index] == -1;
            }

        }

        /// <summary>
        /// Sized reference table.
        /// </summary>
        public static class SizedReferenceTable {

            /// <summary>
            /// Initialize a sized reference table.
            /// </summary>
            /// <param name="count">Number of references to add.</param>
            public static void Init(BinaryDataWriter bw, int count) {

                //Count.
                bw.Write((UInt32)count);

                //References with sizes.
                for (int i = 0; i < count; i++) {
                    bw.Write((UInt64)0);
                    bw.Write((UInt32)0);
                }

            }

            public static void Close(BinaryDataWriter bw, List<UInt16> referenceTypes, List<Int32> offsets, List<UInt32> sizes, long tablePos) {

                //Set position.
                long backUpPos = bw.Position;
                bw.Position = tablePos + 4;

                //Write the references.
                for (int i = 0; i < referenceTypes.Count; i++) {
                    bw.Write(referenceTypes[i]);
                    bw.Write((UInt16)0);
                    bw.Write(offsets[i]);
                    bw.Write(sizes[i]);
                }

                //Restore original position.
                bw.Position = backUpPos;

            }

            /// <summary>
            /// Read a sized reference table.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <returns>A sized reference table.</returns>
            public static SizedReferenceTableInfo Read(BinaryDataReader br) {

                //Return it.
                return new SizedReferenceTableInfo(br);

            }

        }

        /// <summary>
        /// Reference table.
        /// </summary>
        public static class ReferenceTable {

            /// <summary>
            /// Initialize a reference table.
            /// </summary>
            /// <param name="count">Number of references to add.</param>
            public static void Init(BinaryDataWriter bw, int count) {

                //Count.
                bw.Write((UInt32)count);

                //References with sizes.
                for (int i = 0; i < count; i++) {
                    bw.Write((UInt64)0);
                }

            }

            /// <summary>
            /// Close a reference.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="referenceTypes">Reference types.</param>
            /// <param name="offsets">Offsets.</param>
            /// <param name="tablePos">Table position.</param>
            public static void Close(BinaryDataWriter bw, List<UInt16> referenceTypes, List<Int32> offsets, long tablePos) {

                //Set position.
                long backUpPos = bw.Position;
                bw.Position = tablePos + 4;

                //Write the references.
                for (int i = 0; i < referenceTypes.Count; i++) {
                    bw.Write(referenceTypes[i]);
                    bw.Write((UInt16)0);
                    bw.Write(offsets[i]);
                }

                //Restore original position.
                bw.Position = backUpPos;

            }

            /// <summary>
            /// Read a reference table.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <returns>A reference table.</returns>
            public static ReferenceTableInfo Read(BinaryDataReader br) {

                //Return it.
                return new ReferenceTableInfo(br);

            }

        }

        /// <summary>
        /// Reference.
        /// </summary>
        public static class Reference {

            /// <summary>
            /// Initialize a referenc.
            /// </summary>
            public static void Init(BinaryDataWriter bw) {

                //Count.
                bw.Write((UInt64)0);

            }

            /// <summary>
            /// Close the reference.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="referenceType">The reference type.</param>
            /// <param name="offset">Offset.</param>
            /// <param name="tablePos">Table position.</param>
            public static void Close(BinaryDataWriter bw, UInt16 referenceType, Int32 offset, long pos) {

                //Set position.
                long backUpPos = bw.Position;
                bw.Position = pos;

                //Write the references.
                bw.Write(referenceType);
                bw.Write((UInt16)0);
                bw.Write(offset);

                //Restore original position.
                bw.Position = backUpPos;

            }

            /// <summary>
            /// Read a reference.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <returns>A reference.</returns>
            public static ReferenceInfo Read(BinaryDataReader br) {

                //Return it.
                return new ReferenceInfo(br);

            }

        }

        /// <summary>
        /// Sized reference.
        /// </summary>
        public static class SizedReference {

            /// <summary>
            /// Initialize a referenc.
            /// </summary>
            public static void Init(BinaryDataWriter bw) {

                //Count.
                bw.Write((UInt64)0);
                bw.Write((UInt32)0);

            }

            /// <summary>
            /// Close the reference.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="referenceType">The reference type.</param>
            /// <param name="offset">Offset.</param>
            /// <param name="size">Size.</param>
            /// <param name="tablePos">Table position.</param>
            public static void Close(BinaryDataWriter bw, UInt16 referenceType, Int32 offset, UInt32 size, long pos) {

                //Set position.
                long backUpPos = bw.Position;
                bw.Position = pos;

                //Write the references.
                bw.Write(referenceType);
                bw.Write((UInt16)0);
                bw.Write(offset);
                bw.Write(size);

                //Restore original position.
                bw.Position = backUpPos;

            }

            /// <summary>
            /// Read a sized reference.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <returns>A sized reference.</returns>
            public static SizedReferenceInfo Read(BinaryDataReader br) {

                //Return it.
                return new SizedReferenceInfo(br);

            }

        }

    }

}
