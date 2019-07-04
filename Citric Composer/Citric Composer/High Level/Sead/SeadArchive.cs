using Syroot.BinaryData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sead archive.
    /// </summary>
    public class SeadArchive : IList<SeadFileEntry>, ISoundFile {

        /// <summary>
        /// Actual files.
        /// </summary>
        private List<SeadFileEntry> files = new List<SeadFileEntry>();

        /// <summary>
        /// Amount to initially pad by.
        /// </summary>
        public uint InitialPaddingValue = 0x1000;

        /// <summary>
        /// Hash key.
        /// </summary>
        public uint HashKey = 0x65;

        /// <summary>
        /// File version.
        /// </summary>
        public ushort Version = 0x0100;

        /// <summary>
        /// Whether or not the SARC is to have hash names use a signed char.
        /// </summary>
        public bool SignedCharMode = true;

        /// <summary>
        /// Byte order.
        /// </summary>
        public Syroot.BinaryData.ByteOrder ByteOrder;

        /// <summary>
        /// Get the write mode.
        /// </summary>
        public WriteMode WriteMode {

            get {

                //NX.
                if (SignedCharMode && ByteOrder == Syroot.BinaryData.ByteOrder.LittleEndian) {
                    return WriteMode.NX;
                }

                //Cafe.
                else if (!SignedCharMode && ByteOrder == Syroot.BinaryData.ByteOrder.BigEndian) {
                    return WriteMode.Cafe;
                }

                //CTR.
                else if (!SignedCharMode && ByteOrder == Syroot.BinaryData.ByteOrder.LittleEndian) {
                    return WriteMode.Cafe;
                }

                //Signed mode with big endian for the mystery console.
                else {
                    return WriteMode.C_BE;
                }

            }

        }

        //List stuff.
        #region ListStuff

        public SeadFileEntry this[int index] { get => files[index]; set => files[index] = value; }

        public int Count => files.Count;

        public bool IsReadOnly => false;

        public void Add(SeadFileEntry item) {
            files.Add(item);
        }

        public void Clear() {
            files.Clear();
        }

        public bool Contains(SeadFileEntry item) {
            return files.Contains(item);
        }

        public void CopyTo(SeadFileEntry[] array, int arrayIndex) {
            files.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SeadFileEntry> GetEnumerator() {
            return files.GetEnumerator();
        }

        public int IndexOf(SeadFileEntry item) {
            return files.IndexOf(item);
        }

        public void Insert(int index, SeadFileEntry item) {
            files.Insert(index, item);
        }

        public bool Remove(SeadFileEntry item) {
            return files.Remove(item);
        }

        public void RemoveAt(int index) {
            files.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return files.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Get base position.
            long basePos = br.Position;

            //Set byte order.
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;

            //Skip magic and header size.
            br.Position += 6;

            //Get endian.
            if (br.ReadUInt16() == 0xFFFE) {
                br.ByteOrder = ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            }

            //Skip filesize.
            br.Position += 4;

            //Start offset.
            InitialPaddingValue = br.ReadUInt32();

            //Version and padding.
            Version = br.ReadUInt16();
            br.ReadUInt16();

            //Skip magic and header size.
            br.Position += 6;

            //Node count.
            ushort nodeCount = br.ReadUInt16();

            //Hash.
            HashKey = br.ReadUInt32();

            //Read each node.
            files = new List<SeadFileEntry>();
            List<Tuple<uint, int, int, uint, string>> fileOffsets = new List<Tuple<uint, int, int, uint, string>>(); //Hash, String offset, File offset, File end offset, Name.
            List<int> knownIndices = new List<int>();
            for (int i = 0; i < nodeCount; i++) {
                uint hash = br.ReadUInt32();
                uint rawStringIndex = br.ReadUInt32();
                int stringIndex = -1;
                if ((rawStringIndex & 0x01000000) > 0) {
                    stringIndex = (int)rawStringIndex & 0xFFFF;
                }
                int startOffset = br.ReadInt32();
                uint endOffset = br.ReadUInt32();
                fileOffsets.Add(new Tuple<uint, int, int, uint, string>(hash, stringIndex, startOffset, endOffset, null));
                if (!knownIndices.Contains(stringIndex)) {
                    knownIndices.Add(stringIndex);
                }
            }

            //Skip magic and size.         
            br.Position += 8;
            long nameBasePos = br.Position;

            //Get null terminated strings.
            for (int i = 0; i < fileOffsets.Count; i++) {
                if (fileOffsets[i].Item2 != -1) {
                    br.Position = nameBasePos + fileOffsets[i].Item2 * 4;
                    fileOffsets[i] = new Tuple<uint, int, int, uint, string>(fileOffsets[i].Item1, fileOffsets[i].Item2, fileOffsets[i].Item3, fileOffsets[i].Item4, SoundArchiveReader.ReadNullTerminated(br));
                    while ((basePos + br.Position) % 4 != 0) {
                        br.ReadByte();
                    }
                }
            }

            //If the padding is here, then there is no true padding.
            uint bakInitialPaddingValue = InitialPaddingValue;
            if (br.Position == InitialPaddingValue) {
                InitialPaddingValue = 0;
            }

            //Test if signed or unsigned char mode is used.
            var cleanOffsets = fileOffsets.Where(x => x.Item2 != -1).ToList();
            if (cleanOffsets.Count() > 0) {

                //Make a mock sead entry.
                var f = cleanOffsets[0];
                SeadFileEntry e = new SeadFileEntry(f.Item1, f.Item5, null);

                //If the hash matches in signed char mode, then it matches the original has.
                SignedCharMode = e.Hash(true, HashKey) == f.Item1;

            }

            //Read each file.
            foreach (var f in fileOffsets) {

                //Set defaults.
                byte[] fil = null;

                //File is not null.
                if (f.Item2 != -1) {

                    //Read file.
                    br.Position = bakInitialPaddingValue + f.Item3;
                    fil = br.ReadBytes((int)(f.Item4 - f.Item3));

                }

                //Add file.
                files.Add(new SeadFileEntry(f.Item1, f.Item5, fil));

            }

        }

        /// <summary>
        /// Forward the writer.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(WriteMode, bw);
        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {

            //Set properties.
            switch (writeMode) {

                case WriteMode.Cafe:
                    ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                    SignedCharMode = false;
                    break;

                case WriteMode.CTR:
                    ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                    SignedCharMode = false;
                    break;

                case WriteMode.NX:
                    ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                    SignedCharMode = true;
                    break;

                case WriteMode.C_BE:
                    ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                    SignedCharMode = true;
                    break;

            }

            //Get base position.
            long basePos = bw.Position;

            //Sort file entries.
            files.OrderBy(x => x.Hash(SignedCharMode, HashKey));

            //Write the header.
            bw.Write("SARC".ToCharArray());
            bw.Write((ushort)0x14);
            bw.ByteOrder = ByteOrder;
            bw.Write((ushort)0xFEFF);

            //Filesize will be here at 0x8.
            bw.Write((uint)0);

            //Padding and version.
            bw.Write(InitialPaddingValue);
            bw.Write(Version);
            bw.Write((ushort)0);

            //SFAT header.
            bw.Write("SFAT".ToCharArray());
            bw.Write((ushort)0xC);
            bw.Write((ushort)files.Count);
            bw.Write(HashKey);

            //Write each file entry.
            uint fileOff = 0;
            int nameOff = 0;
            for (int i = 0; i < files.Count; i++) {
                bw.Write(files[i].Hash(SignedCharMode, HashKey));
                bw.Write((uint)(files[i].Name != null ? (0x1000000 + nameOff/4) : 0));
                if (files[i].Name != null) {
                    nameOff += files[i].Name.Length + 1;
                    while (nameOff % 4 != 0) {
                        nameOff++;
                    }
                }
                if (files[i].File != null) {
                    bw.Write(fileOff);
                    bw.Write((uint)(files[i].File.Length + fileOff));
                    fileOff += (uint)files[i].File.Length;
                    while (fileOff % 4 != 0) {
                        fileOff++;
                    }
                } else {
                    bw.Write((int)-1);
                    bw.Write((uint)0);
                }
            }

            //Write SFNT.
            bw.Write("SFNT".ToCharArray());
            bw.Write((uint)8);
            foreach (var f in files.Where(x => x.Name != null)) {
                bw.Write(Encoding.UTF8.GetBytes(f.Name));
                bw.Write((byte)0);
                while (basePos + bw.Position % 4 != 0) {
                    bw.Write((byte)0);
                }
            }

            //Write padding if needed.
            if (InitialPaddingValue != 0) {
                while ((basePos + bw.Position) % InitialPaddingValue != 0) {
                    bw.Write((byte)0);
                }
            }

            //Go back and write correct initial padding value.
            if (InitialPaddingValue == 0) {
                long retPos = bw.Position;
                bw.Position = basePos + 0xC;
                bw.Write((uint)(retPos - basePos));
                bw.Position = retPos;
            }

            //Write each file.
            var pureFiles = files.Where(x => x.File != null).ToList();
            foreach (var f in pureFiles) {
                bw.Write(f.File);
                if (f != pureFiles[pureFiles.Count - 1]) {
                    while ((bw.Position + basePos) % 4 != 0) {
                        bw.Write((byte)0);
                    }
                }
            }

            //Go back and write the file size.
            long currPos = bw.Position;
            bw.Position = basePos + 8;
            bw.Write((uint)(currPos - basePos));

        }

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return "sarc";
        }

    }

}
