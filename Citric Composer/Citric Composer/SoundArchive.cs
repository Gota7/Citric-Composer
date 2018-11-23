using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    /// <summary>
    /// BCSAR or BFSAR. Format: 3DS; WiiU
    /// </summary>
    public class b_sar
    {

        //File header.
        public FileHeader fileHeader;

        //Blocks.
        public StrgBlock strg; //Strg block.
        public InfoBlock info; //Info block.
        public FileBlock file; //File block.
        public List<byte[]> miscBlock; //Other type of block.

        /// <summary>
        /// Load a file.
        /// </summary>
        public void Load(byte[] b)
        {

            //Set up stream.
            MemoryStream src = new MemoryStream(b);
            BinaryDataReader br = new BinaryDataReader(src);

            //File header.
            fileHeader = new FileHeader(ref br);

            //Read each block.
            miscBlock = new List<byte[]>();
            foreach (SizedReference p in fileHeader.blockOffsets)
            {

                src.Position = (long)p.offset;
                byte[] n = br.ReadBytes((int)p.size);
                switch (p.typeId)
                {

                    case ReferenceTypes.SAR_Block_String:
                        strg = new StrgBlock();
                        strg.Load(n, fileHeader.byteOrder);
                        break;

                    case ReferenceTypes.SAR_Block_Info:
                        info = new InfoBlock();
                        info.Load(n, fileHeader.byteOrder, fileHeader.version);
                        break;

                    case ReferenceTypes.SAR_Block_File:
                        file = new FileBlock();
                        file.Load(n, fileHeader.byteOrder, ref info);
                        break;

                    default:
                        miscBlock.Add(n);
                        break;

                }

            }

            LoadFileBlock();


        }


        /// <summary>
        /// Load file block files.
        /// </summary>
        public void LoadFileBlock() {

            MemoryStream src = new MemoryStream(file.h);
            BinaryReader br = new BinaryReader(src);

            for (int i = 0; i < info.files.Count(); i++) {

                if (info.files[i] != null) {

                    if (info.files[i].internalFileInfo != null) {
                        if (info.files[i].internalFileInfo.offset != Reference.NULL_PTR)
                        {
                            src.Position = info.files[i].internalFileInfo.offset + 8;
                            info.files[i].file = br.ReadBytes((int)info.files[i].internalFileInfo.size);
                        }
                    }

                }

            }

        }


        /// <summary>
        /// Update the file.
        /// </summary>
        public void Update(UInt16 byteOrder)
        {

            strg.update(byteOrder);
            info.Update(fileHeader.version);
            file.Update(info);

            UInt32 strgSize = (UInt32)strg.toBytes(byteOrder).Length;
            UInt32 infoSize = (UInt32)info.ToBytes(byteOrder, fileHeader.version).Length;
            UInt32 fileSize = (UInt32)file.ToBytes(byteOrder, info).Length;

            string magic = "FSAR";
            if (byteOrder == ByteOrder.LittleEndian) { magic = "CSAR"; }
            fileHeader = new FileHeader(magic, byteOrder, fileHeader.version, strgSize + infoSize + fileSize, new List<SizedReference> { new SizedReference(ReferenceTypes.SAR_Block_String, 0, strgSize), new SizedReference(ReferenceTypes.SAR_Block_Info, (Int32)strgSize, infoSize), new SizedReference(ReferenceTypes.SAR_Block_File, (Int32)(strgSize + infoSize), fileSize) });

        }


        /// <summary>
        /// Convert to bytes.
        /// </summary>
        public byte[] ToBytes(UInt16 endian)
        {

            //New stream with writers.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o, false);

            //Update to correct endian.
            Update(endian);

            //Write the file header.
            fileHeader.Write(ref bw);

            //Write blocks.
            bw.Write(strg.toBytes(endian));
            bw.Write(info.ToBytes(endian, fileHeader.version));
            bw.Write(file.ToBytes(endian, info));

            //Return bytes.
            return o.ToArray();

        }


        /// <summary>
        /// Extract the archive.
        /// </summary>
        /// <param name="path"></param>
        public void Extract(string path, UInt16 byteOrder) {

            Directory.CreateDirectory(path + "/iSequence");
            Directory.CreateDirectory(path + "/iBank");
            Directory.CreateDirectory(path + "/iWaveSoundSet");
            Directory.CreateDirectory(path + "/iWaveArchive");
            Directory.CreateDirectory(path + "/iGroup");

            Update(byteOrder);

            File.WriteAllBytes(path + "/info.bin", info.ToBytes(byteOrder, fileHeader.version));
            File.WriteAllBytes(path + "/strg.bin", strg.toBytes(byteOrder));

            //Get the files.
            int fileId = 0;
            foreach (InfoBlock.fileInfo f in info.files) {

                if (f != null) {

                    if (f.internalFileInfo != null)
                    {

                        if (f.internalFileInfo.offset != Reference.NULL_PTR)
                        {

                            int garbage = 0;
                            string name = FindFileName(fileId, ref garbage);
                            if (name.EndsWith("seq")) {
                                File.WriteAllBytes(path + "/iSequence/" + fileId.ToString("D4") + "_" + name, f.file);
                            } else if (name.EndsWith("bnk")) {
                                File.WriteAllBytes(path + "/iBank/" + fileId.ToString("D4") + "_" + name, f.file);
                            } else if (name.EndsWith("wsd")) {
                                File.WriteAllBytes(path + "/iWaveSoundSet/" + fileId.ToString("D4") + "_" + name, f.file);
                            } else if (name.EndsWith("war")) {
                                File.WriteAllBytes(path + "/iWaveArchive/" + fileId.ToString("D4") + "_" + name, f.file);
                            } else if (name.EndsWith("grp")) {
                                File.WriteAllBytes(path + "/iGroup/" + fileId.ToString("D4") + "_" + name, f.file);
                            }

                        }

                    }

                }

                fileId++;

            }

        }


        /// <summary>
        /// Find the file name of a file from fileId.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public string FindFileName(int fileId, ref int iconId)
        {

            //Nothing found.
            string name = "NO_NAME";
            string prefix = "bf";
            if (fileHeader.byteOrder == ByteOrder.LittleEndian) {
                prefix = "bc";
            }
            iconId = 0;

            //Load sounds.
            int ssCount = 0;
            foreach (InfoBlock.soundInfo s in info.sounds)
            {

                //Not null.
                if (s != null && s.fileId == (UInt32)fileId)
                {

                    //Not stream info.
                    if (s.streamInfo == null)
                    {

                        //Not wave info, so sequence info.
                        if (s.waveInfo == null)
                        {

                            if (s.sequenceInfo != null)
                            {

                                iconId = 3;
                                if (s.flags.isFlagEnabled[0])
                                {
                                    name = new string(strg.stringEntries[(int)s.flags.flagValues[0]].data) + "." + prefix + "seq";
                                }
                                else {
                                    name += "." + prefix + "seq";
                                }

                            }

                        }

                        //Wave info.
                        else
                        {

                            iconId = 2;
                            if (s.flags.isFlagEnabled[0])
                            {
                                name = new string(strg.stringEntries[(int)s.flags.flagValues[0]].data) + "." + prefix + "wsd";
                            }
                            else {
                                name += "." + prefix + "wsd";
                            }

                        }

                    }

                    //Stream info.
                    else
                    {

                        iconId = 1;
                        if (s.flags.isFlagEnabled[0])
                        {
                            name = new string(strg.stringEntries[(int)s.flags.flagValues[0]].data) + "." + prefix + "stm";
                        }
                        else {
                            name += "." + prefix + "stm";
                        }

                    }

                }

                ssCount += 1;

            }

            //Load banks.
            int bCount = 0;
            foreach (InfoBlock.bankInfo b in info.banks)
            {

                //Null.
                if (b != null && b.fileId == fileId)
                {

                    //Get node name.
                    iconId = 5;
                    if (b.flags.isFlagEnabled[0])
                    {
                        name = new string(strg.stringEntries[(int)b.flags.flagValues[0]].data) + "." + prefix + "bnk";
                    }
                    else {
                        name += "." + prefix + "bnk";
                    }

                }

                bCount += 1;

            }

            //Load groups.
            int gCount = 0;
            foreach (InfoBlock.groupInfo g in info.groups)
            {

                //Null.
                if (g != null && g.fileId == fileId)
                {

                    iconId = 7;
                    if (g.flags.isFlagEnabled[0])
                    {
                        name = new string(strg.stringEntries[(int)g.flags.flagValues[0]].data) + "." + prefix + "grp";
                    }
                    else {
                        name += "." + prefix + "grp";
                    }

                }

                gCount += 1;

            }

            //Load wave archives.
            int wCount = 0;
            foreach (InfoBlock.waveArchiveInfo w in info.wars)
            {

                //Null.
                if (w != null && w.fileId == fileId)
                {

                    iconId = 4;
                    if (w.flags.isFlagEnabled[0]) {
                        name = new string(strg.stringEntries[(int)w.flags.flagValues[0]].data) + "." + prefix + "war";
                    } else {
                        name += "." + prefix + "war";
                    }

                }

                wCount += 1;

            }

            return name;

        }


        /// <summary>
        /// String block.
        /// </summary>
        public class StrgBlock
        {

            //General stuff.
            public char[] magic; //STRG.
            public UInt32 fileSize; //File size.

            public Reference stringTableRef;
            public Reference lookupTableRef;

            public SizedReferenceTable stringTable; //String tables.
            public List<stringEntry> stringEntries; //String entries.
            public byte[] stringReserved; //String reserved for alignment. 0x4.

            public lookupTableRecords lookupRecord; //Lookup record.
            public byte[] lookupReserved; //Lookup reserved for alignment. 0x20.


            //String entries.
            public struct stringEntry
            {

                public char[] data; //String data.
                public byte seperator; //Seperator.

                //Found in lookup table.
                public int index; //Index in INFO.
                public int type; //Type of entry.

            }


            /// <summary>
            /// Lookup table records.
            /// </summary>
            public struct lookupTableRecords
            {

                public UInt32 rootNode; //Root node.
                public UInt32 amountOfNodes; //Amount of records.
                public List<lookupTableRecord> record; //Records

            }


            /// <summary>
            /// Lookup table record.
            /// </summary>
            public struct lookupTableRecord
            {

                public UInt16 leafNodeFlag; //1 if leaf node.
                public UInt16 searchIndex; //Bit index from left.
                public UInt32 leftIndex; //Bit index from left.
                public UInt32 rightIndex; //Bit index from right.
                public UInt32 stringIndex; //Index of string.
                public UInt32 id; //Id. &0xFF000000 >> 24 = Type, &0x00FFFFFF = Entry Number.

                public int index; //Index for temp purposes.

            }



            /// <summary>
            /// Load a file.
            /// </summary>
            /// <param name="b">Bytes of the file.</param>
            public void Load(byte[] b, UInt16 endian)
            {

                //Reader.
                MemoryStream src = new MemoryStream(b);
                BinaryDataReader br = new BinaryDataReader(src);

                //Endian.
                if (endian == ByteOrder.BigEndian)
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }

                //Stuff.
                magic = br.ReadChars(4);
                fileSize = br.ReadUInt32();

                stringTableRef = new Reference(ref br);
                lookupTableRef = new Reference(ref br);

                //String table.
                br.Position = (int)stringTableRef.offset + 8;
                stringTable = new SizedReferenceTable(ref br);

                //Read strings.
                stringEntries = new List<stringEntry>();
                for (int i = 0; i < stringTable.count; i++)
                {

                    br.Position = stringTable.sizedReferences[i].offset + 24;
                    stringEntry s = new stringEntry();
                    s.data = br.ReadChars((int)stringTable.sizedReferences[i].size - 1);
                    s.seperator = br.ReadByte();
                    stringEntries.Add(s);

                }

                //Get alignment.
                int lengthStringR = 0;
                int pos1 = (int)br.Position;
                while (pos1 % 4 != 0)
                {
                    lengthStringR += 1;
                    pos1 += 1;
                }
                stringReserved = br.ReadBytes(lengthStringR);

                //Read lookup table.
                br.Position = (int)lookupTableRef.offset + 8;
                lookupRecord.rootNode = br.ReadUInt32();
                lookupRecord.amountOfNodes = br.ReadUInt32();
                lookupRecord.record = new List<lookupTableRecord>();
                for (int i = 0; i < (int)lookupRecord.amountOfNodes; i++)
                {

                    lookupTableRecord s = new lookupTableRecord();
                    s.leafNodeFlag = br.ReadUInt16();
                    s.searchIndex = br.ReadUInt16();
                    s.leftIndex = br.ReadUInt32();
                    s.rightIndex = br.ReadUInt32();
                    s.stringIndex = br.ReadUInt32();
                    s.id = br.ReadUInt32();
                    lookupRecord.record.Add(s);

                }

                //Get alignment.
                int lengthLookupR = 0;
                pos1 = (int)br.Position;
                while (pos1 % 32 != 0)
                {
                    lengthLookupR += 1;
                    pos1 += 1;
                }
                lookupReserved = br.ReadBytes(lengthLookupR);

                for (int i = 0; i < stringEntries.Count; i++)
                {

                    lookupTableRecord r = lookupString(stringEntries[i].data);
                    stringEntry e = stringEntries[i];
                    e.index = (int)(r.id & 0x00FFFFFF);
                    e.type = (int)((r.id & 0xFF000000) >> 24);
                    stringEntries[i] = e;

                }

            }


            /// <summary>
            /// Convert to bytes.
            /// </summary>
            /// <returns>The bytes.</returns>
            /// <param name="endian">Endian.</param>
            public byte[] toBytes(UInt16 endian)
            {

                //Update.
                update(endian);

                //Reader.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                //Endian.
                if (endian == ByteOrder.BigEndian)
                {
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else
                {
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }

                //Stuff.
                bw.Write(magic);
                bw.Write(fileSize);

                bw.Write(stringTableRef.typeId);
                bw.Write(stringTableRef.padding);
                bw.Write(stringTableRef.offset);

                bw.Write(lookupTableRef.typeId);
                bw.Write(lookupTableRef.padding);
                bw.Write(lookupTableRef.offset);

                //String table.
                bw.Write(stringTable.count);
                foreach (SizedReference s in stringTable.sizedReferences)
                {

                    bw.Write(s.typeId);
                    bw.Write(s.padding);
                    bw.Write(s.offset);
                    bw.Write(s.size);

                }

                //Write strings.
                for (int i = 0; i < (int)stringTable.count; i++)
                {

                    stringEntry s = stringEntries[i];
                    bw.Write(s.data);
                    bw.Write(s.seperator);

                }
                bw.Write(stringReserved);

                //Write lookup table.
                bw.Write(lookupRecord.rootNode);
                bw.Write(lookupRecord.amountOfNodes);
                for (int i = 0; i < (int)lookupRecord.amountOfNodes; i++)
                {

                    lookupTableRecord s = lookupRecord.record[i];
                    bw.Write(s.leafNodeFlag);
                    bw.Write(s.searchIndex);
                    bw.Write(s.leftIndex);
                    bw.Write(s.rightIndex);
                    bw.Write(s.stringIndex);
                    bw.Write(s.id);

                }
                bw.Write(lookupReserved);

                return o.ToArray();

            }


            /// <summary>
            /// Update to the specified endian.
            /// </summary>
            /// <param name="endian">Endian.</param>
            public void update(UInt16 endian)
            {

                magic = "STRG".ToCharArray();

                stringTableRef = new Reference(ReferenceTypes.SAR_General, 0x10);
                lookupTableRef = new Reference(ReferenceTypes.SAR_General + 1, 0);

                //Take care of records later.
                stringTable = new SizedReferenceTable(new SizedReference[stringEntries.Count]);

                Int32 stringOffset = 0x4 + 0xC * (Int32)stringTable.count;
                for (int i = 0; i < stringEntries.Count; i++)
                {

                    stringEntry e = stringEntries[i];
                    e.seperator = 0;
                    stringEntries[i] = e;

                    stringTable.sizedReferences[i] = new SizedReference(ReferenceTypes.General + 1, stringOffset, (UInt32)stringEntries[i].data.Length + 1);

                    stringOffset += stringEntries[i].data.Length + 1;

                }
                int stringPadCount = 0;
                while (stringOffset % 4 != 0)
                {
                    stringPadCount += 1;
                    stringOffset += 1;
                }
                stringReserved = new byte[stringPadCount];
                lookupTableRef.offset = 0x10 + stringOffset;

                lookupRecord.amountOfNodes = (UInt32)lookupRecord.record.Count;
                fileSize = (UInt32)(0x10 + lookupTableRef.offset + 0x14 * lookupRecord.amountOfNodes);
                int lookupPadCount = 0;
                while (fileSize % 0x20 != 0)
                {
                    fileSize += 1;
                    lookupPadCount += 1;
                }
                lookupReserved = new byte[lookupPadCount];

                //Update table.
                updateLookupTable();

            }


            /// <summary>
            /// Get a string's node in the lookup table.
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public lookupTableRecord lookupString(char[] str)
            {

                lookupTableRecord r = new lookupTableRecord();

                //Search the tree until you get the correct value.
                string bin = "";
                foreach (char c in str)
                {
                    //string b = Reverse(Convert.ToString(c, 2).PadLeft(8, '0'));
                    bin += Convert.ToString(c, 2).PadLeft(8, '0');
                }
                lookupTableRecord currentNode = lookupRecord.record[(int)lookupRecord.rootNode];
                while (currentNode.leafNodeFlag != 1)
                {

                    try
                    {
                        char c = bin[(int)currentNode.searchIndex];
                        if (c == '0')
                        {
                            currentNode = lookupRecord.record[(int)currentNode.leftIndex];
                        }
                        else
                        {
                            currentNode = lookupRecord.record[(int)currentNode.rightIndex];
                        }
                    }
                    catch
                    {
                        currentNode = lookupRecord.record[(int)currentNode.leftIndex];
                    }

                }
                r = currentNode;

                return r;

            }


            /// <summary>
            /// Update lookup table.
            /// </summary>
            public void updateLookupTable()
            {

                //Make a new patricia tree, and add all the keys.
                PatriciaTree tree = new PatriciaTree();
                lookupRecord = new lookupTableRecords();
                foreach (stringEntry e in stringEntries)
                {
                    PatriciaTree.PatriciaTreeItem p = new PatriciaTree.PatriciaTreeItem();
                    p.Key = new string(e.data);
                    tree.Add(p);
                }

                //Convert the patricia tree to my lookup nodes.
                lookupRecord.record = new List<lookupTableRecord>();
                lookupRecord.amountOfNodes = (UInt32)tree.Nodes.Count;
                lookupRecord.rootNode = (UInt32)tree.Root.Index;
                foreach (PatriciaTree.INode n in tree.Nodes)
                {
                    lookupTableRecord r = new lookupTableRecord();
                    r.id = 0xFFFFFFFF;
                    r.leafNodeFlag = 0;
                    if (n.Left == null && n.Right == null) { r.leafNodeFlag = 1; }
                    if (n.Left == null) { r.leftIndex = 0xFFFFFFFF; } else { r.leftIndex = (UInt32)n.Left.Index; }
                    if (n.Right == null) { r.rightIndex = 0xFFFFFFFF; } else { r.rightIndex = (UInt32)n.Right.Index; }
                    r.searchIndex = (UInt16)n.Bit;
                    r.stringIndex = 0xFFFFFFFF;
                    r.index = n.Index;
                    lookupRecord.record.Add(r);
                }

                for (int i = 0; i < stringEntries.Count; i++)
                {

                    lookupTableRecord r = lookupString(stringEntries[i].data);
                    r.stringIndex = (UInt32)i;

                    UInt32 id = 0;
                    id += (UInt32)((stringEntries[i].type << 24) & 0xFF000000);
                    id += (UInt32)(stringEntries[i].index & 0x00FFFFFF);
                    r.id = id;
                    r.searchIndex = 0xFFFF;
                    r.leftIndex = 0xFFFFFFFF;
                    r.rightIndex = 0xFFFFFFFF;

                    lookupRecord.record[r.index] = r;

                }

            }

        }


        /// <summary>
        /// File block.
        /// </summary>
        public class FileBlock
        {

            public char[] magic; //FILE.
            public UInt32 blockSize; //Size of block.
            public byte[] reserved; //0x18, reserved space.

            public byte[] h;

            /// <summary>
            /// Load a file.
            /// </summary>
            /// <param name="b">Input file block bytes.</param>
            /// <param name="endian">Byte order.</param>
            /// <param name="info">Info block.</param>
            public void Load(byte[] b, UInt16 endian, ref InfoBlock info)
            {

                h = b;

                /*
                //Setup the source.
                MemoryStream src = new MemoryStream(b);
                BinaryDataReader br = new BinaryDataReader(src);

                //Byte order.
                if (endian == ByteOrder.BigEndian)
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }

                //Read the main data.
                magic = br.ReadChars(4);
                blockSize = br.ReadUInt32();
                reserved = br.ReadBytes(0x18);

                //Read the files.
                for (int i = 0; i < info.files.Count(); i++)
                {

                    if (info.files[i].internalFileInfo != null)
                    {
                        if (info.files[i].internalFileInfo.offset != Reference.NULL_PTR)
                        {
                            InfoBlock.fileInfo f = info.files[i];
                            br.Position = f.internalFileInfo.offset + 8;
                            f.file = br.ReadBytes((int)f.internalFileInfo.size);
                            info.files[i] = f;
                        }
                    }

                }*/

            }


            /// <summary>
            /// Convert a file to bytes.
            /// </summary>
            /// <param name="endian"></param>
            /// <param name="info"></param>
            /// <returns></returns>
            public byte[] ToBytes(UInt16 endian, InfoBlock info)
            {
                return h;
            }


            /// <summary>
            /// Update the file.
            /// </summary>
            /// <param name="info"></param>
            public void Update(InfoBlock info)
            {



            }

        }


        /// <summary>
        /// Info block. This one makes me cry.
        /// </summary>
        public class InfoBlock
        {

            /// <summary>
            /// 1 - INFO.
            /// </summary>
            public char[] magic;

            /// <summary>
            /// 2 - Size of the block.
            /// </summary>
            public UInt32 size;


            /// <summary>
            /// 3 - 8 offset references in this order: Sound, Sound Group, Bank, Wave Archive, Group, Player, File, Project Info.
            /// </summary>
            public Reference[] offsetRefs;


            /// <summary>
            /// 4 - Sound reference table.
            /// </summary>
            public ReferenceTable soundRefTable;

            /// <summary>
            /// 5 - The actual list of sounds.
            /// </summary>
            public List<soundInfo> sounds;


            /// <summary>
            /// 6 - Sound group reference table.
            /// </summary>
            public ReferenceTable soundGrpRefTable;

            /// <summary>
            /// 7 - The actual list of sound groups.
            /// </summary>
            public List<soundGroupInfo> soundGroups;


            /// <summary>
            /// 8 - Bank reference table.
            /// </summary>
            public ReferenceTable bankRefTable;

            /// <summary>
            /// 9 - The actual list of banks.
            /// </summary>
            public List<bankInfo> banks;


            /// <summary>
            /// 10 - Wave archive reference table.
            /// </summary>
            public ReferenceTable warRefTable;

            /// <summary>
            /// 11 - Wave archives.
            /// </summary>
            public List<waveArchiveInfo> wars;


            /// <summary>
            /// 12 - Group reference table.
            /// </summary>
            public ReferenceTable grpRefTable;

            /// <summary>
            /// 13 - The actual list of groups.
            /// </summary>
            public List<groupInfo> groups;


            /// <summary>
            /// 14 - Player reference table.
            /// </summary>
            public ReferenceTable playerRefTable;

            /// <summary>
            /// 15 - The actual list of players.
            /// </summary>
            public List<playerInfo> players;


            /// <summary>
            /// 16 - Files reference table.
            /// </summary>
            public ReferenceTable fileRefTable;

            /// <summary>
            /// 17 - The actual list of files.
            /// </summary>
            public List<fileInfo> files;


            /// <summary>
            /// 18 - Project info.
            /// </summary>
            public soundArchivePlayerInfo projectInfo;



            /// <summary>
            /// Sound info. For sequences, streams, and waves.
            /// </summary>
            public class soundInfo
            {

                /// <summary>
                /// 1 - File Id of the sound file.
                /// </summary>
                public UInt32 fileId;

                /// <summary>
                /// 2 - Player number.
                /// </summary>
                public Id playerNumber;

                /// <summary>
                /// 3 - Volume of the sound.
                /// </summary>
                public byte volume;

                /// <summary>
                /// 4 - Remote filter, whatever that does.
                /// </summary>
                public byte remoteFilter;

                /// <summary>
                /// 5 - Padding. It's everywhere.
                /// </summary>
                public UInt16 padding;

                /// <summary>
                /// 6 - Reference to detailed sound.
                /// </summary>
                public Reference detailSoundRef;

                /// <summary>
                /// 7 - Flags. F0 = String Index, F1 = Pan Stuff. 0x0000XXYY, XX = pan curve, YY = pan mode, F2 = Player Stuff. 0x0000XXYY, XX = actor player id, YY = player priority, F8 = 3D info offset, F17 = Is front bypass.
                /// </summary>
                public FlagParameters flags;

                /// <summary>
                /// 8 - The Sound 3D info.
                /// </summary>
                public sound3dInfo sound3d;

                /// <summary>
                /// 9 - The detail sound info if sequence.
                /// </summary>
                public sequenceSoundInfo sequenceInfo;

                /// <summary>
                /// 10 - The detail sound info if stream.
                /// </summary>
                public streamSoundInfo streamInfo;

                /// <summary>
                /// 11 - The detail sound info if wave.
                /// </summary>
                public waveSoundInfo waveInfo;


                /// <summary>
                /// 3Dimensional sound info.
                /// </summary>
                public class sound3dInfo
                {

                    /// <summary>
                    /// 1 - Flags for 3d info. (&0x000000FF) -> Reverse Bits -> [0]Vol [1]Priority [2]Pan [3]SPan [4]Filter. (Is there more after this if flags are set?)
                    /// </summary>
                    public UInt32 flagBoxes;

                    /// <summary>
                    /// 2 - Attenuation Rate.
                    /// </summary>
                    public float dimensionalAttenuation;

                    /// <summary>
                    /// 3 - Decay curve type.
                    /// </summary>
                    public byte attenuationCurveType;

                    /// <summary>
                    /// 4 - Doppler factor.
                    /// </summary>
                    public byte dopplerFactor;

                    /// <summary>
                    /// 5 - Padding.
                    /// </summary>
                    public UInt16 padding;

                    /// <summary>
                    /// 6 - Extra flags.
                    /// </summary>
                    public FlagParameters optionParameter;

                }


                /// <summary>
                /// If sound info uses a sequence.
                /// </summary>
                public class sequenceSoundInfo
                {

                    /// <summary>
                    /// -1 - Padding value, before the info really.
                    /// </summary>
                    public UInt32 paddingValue;

                    /// <summary>
                    /// 1 - Reference to the bank id table.
                    /// </summary>
                    public Reference bankIdTableRef;

                    /// <summary>
                    /// 2 - Flags for track allocation.
                    /// </summary>
                    public UInt32 allocateTrackFlags;

                    /// <summary>
                    /// 3 - Flags. F0 = 0x0000XXYY, XX = Is fix priority at release, YY = Channel priority, F1 = Start offset???
                    /// </summary>
                    public FlagParameters flags;

                    /// <summary>
                    /// 4 - The bank id table.
                    /// </summary>
                    public Table<Id> bankIdTable;

                }


                /// <summary>
                /// Stream sound info.
                /// </summary>
                public class streamSoundInfo
                {

                    /// <summary>
                    /// 1 - Bit flags indicating the valid tracks.
                    /// </summary>
                    public UInt16 allocateTrackFlags;

                    /// <summary>
                    /// 2 - Total number of channels.
                    /// </summary>
                    public UInt16 channelCount;

                    /// <summary>
                    /// 3 - I'm mostly sure the last UInt32 for the old stream info is flags.
                    /// </summary>
                    public FlagParameters oldFlags;

                    /// <summary>
                    /// 3 - Reference to the track info table.
                    /// </summary>
                    public Reference trackInfoTableRef;

                    /// <summary>
                    /// 4 - The pitch.
                    /// </summary>
                    public float pitch;

                    /// <summary>
                    /// 5 - Reference to the send value.
                    /// </summary>
                    public Reference sendValueRef;

                    /// <summary>
                    /// 6 - Reference to the stream sound extension.
                    /// </summary>
                    public Reference streamSoundExtensionRef;

                    /// <summary>
                    /// 7 - Pre-fetch File ID.
                    /// </summary>
                    public UInt32 prefetchFileId;

                    /// <summary>
                    /// 8 - The track info table.
                    /// </summary>
                    public ReferenceTable trackInfoTable;

                    /// <summary>
                    /// 9 - The track info.
                    /// </summary>
                    public List<trackInfoEntry> trackInfo;

                    /// <summary>
                    /// 10 - Send value.
                    /// </summary>
                    public sendValueStruct sendValue;

                    /// <summary>
                    /// 11 - Stream sound extension.
                    /// </summary>
                    public streamSoundExtensionStruct streamSoundExtension;


                    /// <summary>
                    /// Track Info Entry.
                    /// </summary>
                    public class trackInfoEntry
                    {

                        /// <summary>
                        /// 1 - Volume.
                        /// </summary>
                        public byte volume;

                        /// <summary>
                        /// 2 - Pan.
                        /// </summary>
                        public byte pan;

                        /// <summary>
                        /// 3 - Span.
                        /// </summary>
                        public byte span;

                        /// <summary>
                        /// 4 - Surround mode.
                        /// </summary>
                        public byte surroundMode;

                        /// <summary>
                        /// 5 - Reference to the global channel index table.
                        /// </summary>
                        public Reference globalChannelIndexTableRef;

                        /// <summary>
                        /// 6 - Reference to the send value.
                        /// </summary>
                        public Reference sendValueRef;

                        /// <summary>
                        /// 7 - Lpf frequency.
                        /// </summary>
                        public byte lpfFreq;

                        /// <summary>
                        /// 8 - Biquad type.
                        /// </summary>
                        public byte biquadType;

                        /// <summary>
                        /// 9 - Biquad value.
                        /// </summary>
                        public byte biquadValue;

                        /// <summary>
                        /// 10 - Padding.
                        /// </summary>
                        public byte padding;

                        /// <summary>
                        /// 11 - Global channel index table.
                        /// </summary>
                        public Table<byte> globalChannelIndexTable;

                        /// <summary>
                        /// 12 - The send value.
                        /// </summary>
                        public sendValueStruct sendValue;

                    }


                    /// <summary>
                    /// Send value.
                    /// </summary>
                    public class sendValueStruct
                    {

                        /// <summary>
                        /// 1 - Main send.
                        /// </summary>
                        public byte mainSend;

                        /// <summary>
                        /// 2 - fxSend[AUX_BUS_NUM].
                        /// </summary>
                        public byte[] fxSend;

                    }


                    /// <summary>
                    /// Stream sound extension.
                    /// </summary>
                    public class streamSoundExtensionStruct
                    {

                        /// <summary>
                        /// 1 - Stream type info. FileType = return static_cast<SoundArchive::StreamFileType>( Util::DevideBy8bit( streamTypeInfo, 0 ) ); IsLoop = return Util::DevideBy8bit( streamTypeInfo, 1 ) == 1;
                        /// </summary>
                        public UInt32 streamTypeInfo;

                        /// <summary>
                        /// 2 - Loop start frame.
                        /// </summary>
                        public UInt32 loopStartFrame;

                        /// <summary>
                        /// 3 - Loop end frame.
                        /// </summary>
                        public UInt32 loopEndFrame;

                    }

                }


                /// <summary>
                /// If sound info uses a wave.
                /// </summary>
                public class waveSoundInfo
                {

                    /// <summary>
                    /// -1 - Padding value, before the info really.
                    /// </summary>
                    public UInt32 paddingValue;

                    /// <summary>
                    /// 1 - WSD index.
                    /// </summary>
                    public UInt32 index;

                    /// <summary>
                    /// 2 - The required number of tracks.
                    /// </summary>
                    public UInt32 allocateTrackCount;

                    /// <summary>
                    /// 3 - Flags. There is a channel priority and release priority here somewhere.
                    /// </summary>
                    public FlagParameters flags;

                }

            }


            /// <summary>
            /// Sound group info.
            /// </summary>
            public class soundGroupInfo
            {

                /// <summary>
                /// 1 - First ID of the sound group. Type must match the last id.
                /// </summary>
                public Id firstId;

                /// <summary>
                /// 2 - Last ID of the sound group. Type must match the first id.
                /// </summary>
                public Id lastId;

                /// <summary>
                /// 3 - Reference to a file id table, of the file ids used for the group. Uses the table type id for the reference type id.
                /// </summary>
                public Reference fileIdTableRef;

                /// <summary>
                /// 4 - Reference to the wave sound group table, but only if the type of the first and last id is wave sound group info.
                /// </summary>
                public Reference waveSoundGroupTableRef;

                /// <summary>
                /// 5 - Flags. F0 = String Index.
                /// </summary>
                public FlagParameters flags;

                /// <summary>
                /// 6 - File Id Table. All the file ids used for the sounds in the group are here.
                /// </summary>
                public Table<UInt32> fileIdTable;

                /// <summary>
                /// 7 - Wave sound group table. Contains info if the entries are WSDs.
                /// </summary>
                public waveSoundGroupTableStruct waveSoundGroupTable;

                /// <summary>
                /// Wave sound group table.
                /// </summary>
                public class waveSoundGroupTableStruct
                {

                    /// <summary>
                    /// 1 - Reference to the wave archive ids used. Uses the table type id for the reference type id.
                    /// </summary>
                    public Reference waveArchiveIdTableRef;

                    /// <summary>
                    /// 2 - Flags. "Provisional", so it's probably useless.
                    /// </summary>
                    public FlagParameters flags;

                    /// <summary>
                    /// 3 - Table with the wave archive ids to use.
                    /// </summary>
                    public Table<Id> waveArchiveIdTable;

                }

            }


            /// <summary>
            /// Bank info.
            /// </summary>
            public class bankInfo
            {

                /// <summary>
                /// 1 - File ID of the bank file.
                /// </summary>
                public UInt32 fileId;

                /// <summary>
                /// 2 - Type ID is Tables, references the wave archive item id table.
                /// </summary>
                public Reference waveArchiveItemIdTableRef;

                /// <summary>
                /// 3 - Flags. F0 = String Index.
                /// </summary>
                public FlagParameters flags;

                /// <summary>
                /// 4 - Wave Archive table.
                /// </summary>
                public Table<Id> waveArchiveItemIdTable;

            }


            /// <summary>
            /// Wave archive info.
            /// </summary>
            public class waveArchiveInfo
            {

                /// <summary>
                /// 1 - File Id of the wave archive to use.
                /// </summary>
                public UInt32 fileId;

                /// <summary>
                /// 2 - If the wave archive is to load individually.
                /// </summary>
                public byte loadIndividual;

                /// <summary>
                /// 3 - 3 bytes worth of padding.
                /// </summary>
                public byte[] padding;

                /// <summary>
                /// 4 - Flags. F0 = String Index, F1 = Wave Count.
                /// </summary>
                public FlagParameters flags;

            }


            /// <summary>
            /// Group info.
            /// </summary>
            public class groupInfo
            {

                /// <summary>
                /// 1 - The group file ID.
                /// </summary>
                public UInt32 fileId;

                /// <summary>
                /// 2 - Flags. F0 = String Index.
                /// </summary>
                public FlagParameters flags;

            }


            /// <summary>
            /// Player info of the players.
            /// </summary>
            public class playerInfo
            {

                /// <summary>
                /// 1 - Max number of sounds the player can play.
                /// </summary>
                public UInt32 playableSoundLimit;

                /// <summary>
                /// 2 - Flag parameters. F0 = String Index, F1 = Player Heap Size.
                /// </summary>
                public FlagParameters flags;

            }


            /// <summary>
            /// File info.
            /// </summary>
            public class fileInfo
            {

                /// <summary>
                /// 1 - Reference to file location.
                /// </summary>
                public Reference fileLocationRef;


                /// <summary>
                /// 2 - Flag parameters. Any flags are extra.
                /// </summary>
                public FlagParameters flags;


                /// <summary>
                /// 3 - If the file is internal, a reference to it in the FILE block.
                /// </summary>
                public SizedReference internalFileInfo;


                /// <summary>
                /// 4 - Reference to the internal file group table, if file is internal.
                /// </summary>
                public Reference internalFileGroupTableRef;


                /// <summary>
                /// 5 - Tells what groups the file belongs in, if file is internal.
                /// </summary>
                public Table<UInt32> internalFileGroupTable;


                /// <summary>
                /// 6 - If the file is external, it's just a filename.
                /// </summary>
                public string externalFileName;

                /// <summary>
                /// 5 - The actual file. Woo.
                /// </summary>
                public byte[] file;

            }


            /// <summary>
            /// Basic INFO universal for the file.
            /// </summary>
            public class soundArchivePlayerInfo
            {

                /// <summary>
                /// 1 - Max number of SEQs.
                /// </summary>
                public UInt16 maxSeq;

                /// <summary>
                /// 2 - Max number of SEQ tracks.
                /// </summary>
                public UInt16 maxSeqTracks;

                /// <summary>
                /// 3 - Max number of stream sounds.
                /// </summary>
                public UInt16 maxStreamSounds;

                /// <summary>
                /// 4 - Max number of stream tracks.
                /// </summary>
                public UInt16 maxStreamTracks;

                /// <summary>
                /// 5 - Max number of stream channels.
                /// </summary>
                public UInt16 maxStreamChannels;

                /// <summary>
                /// 6 - Max number of wave sounds.
                /// </summary>
                public UInt16 maxWaveSounds;

                /// <summary>
                /// 7 - Max number of wave tracks.
                /// </summary>
                public UInt16 maxWaveTracks;

                /// <summary>
                /// 8 - Who tf knows what that means
                /// </summary>
                public byte streamBufferTimes;

                /// <summary>
                /// 9 - Yay, more useless things.
                /// </summary>
                public byte padding;

                /// <summary>
                /// 10 - Probably useless, it's "provisional".
                /// </summary>
                public UInt32 options;

            }




            /// <summary>
            /// Load a file.
            /// </summary>
            /// <param name="b">The blue component.</param>
            public void Load(byte[] b, UInt16 endian, UInt32 version)
            {

                //Setup source.
                MemoryStream src = new MemoryStream(b);
                BinaryDataReader br = new BinaryDataReader(src);

                //Setup endian.
                if (endian == ByteOrder.BigEndian)
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else
                {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }

                //Read header.
                magic = br.ReadChars(4);
                size = br.ReadUInt32();

                //Set up a place for reference.
                int beginingOffset = (int)br.Position;


                //Read offset table.
                offsetRefs = new Reference[8];
                for (int i = 0; i < 8; i++) { offsetRefs[i] = new Reference(ref br); }

                //Read info data.
                foreach (Reference infoSectionRef in offsetRefs)
                {

                    //Go to offset if not null.
                    if (infoSectionRef.offset != Reference.NULL_PTR) { br.Position = beginingOffset + infoSectionRef.offset; }

                    //Switch to see the type of item.
                    switch (infoSectionRef.typeId)
                    {

                        //Sound Info.
                        case ReferenceTypes.SAR_Section_SoundInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                sounds = null;
                                soundRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                sounds = new List<soundInfo>();

                                //Get the current pos.
                                long soundBasePosRef = br.Position;

                                //Read the table of sounds.
                                soundRefTable = new ReferenceTable(ref br);

                                //Get the entries.
                                foreach (Reference r in soundRefTable.references)
                                {

                                    //New sound info.
                                    soundInfo s = null;

                                    //If not null.
                                    if (r.offset != Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_Sound)
                                    {

                                        //Set position.
                                        br.Position = soundBasePosRef + r.offset;

                                        //Start reading info.
                                        s = new soundInfo
                                        {
                                            fileId = br.ReadUInt32(),
                                            playerNumber = new Id(ref br),
                                            volume = br.ReadByte(),
                                            remoteFilter = br.ReadByte(),
                                            padding = br.ReadUInt16(),
                                            detailSoundRef = new Reference(ref br),
                                            flags = new FlagParameters(ref br),
                                            sound3d = null,
                                            sequenceInfo = null,
                                            streamInfo = null,
                                            waveInfo = null
                                        };

                                        //Get detail sound.
                                        if (s.detailSoundRef.offset != Reference.NULL_PTR)
                                        {

                                            //Set position.
                                            br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset;

                                            //Switch the detail id types.
                                            switch (s.detailSoundRef.typeId)
                                            {

                                                //Stream.
                                                case ReferenceTypes.SAR_ItemInfos + 1:

                                                    //If version is old.
                                                    if (version < NWConstants.NEW_STREAM_VERSION)
                                                    {

                                                        s.streamInfo = new soundInfo.streamSoundInfo
                                                        {
                                                            allocateTrackFlags = br.ReadUInt16(),
                                                            channelCount = br.ReadUInt16(),
                                                            oldFlags = new FlagParameters(ref br),
                                                            trackInfoTableRef = null,
                                                            pitch = 0xFFFFFFFF,
                                                            sendValueRef = null,
                                                            streamSoundExtensionRef = null,
                                                            prefetchFileId = 0xFFFFFFFF,
                                                            trackInfoTable = null,
                                                            trackInfo = null,
                                                            sendValue = null,
                                                            streamSoundExtension = null
                                                        };

                                                    }

                                                    //If version is new.
                                                    else
                                                    {
                                                        s.streamInfo = new soundInfo.streamSoundInfo
                                                        {
                                                            allocateTrackFlags = br.ReadUInt16(),
                                                            channelCount = br.ReadUInt16(),
                                                            oldFlags = null,
                                                            trackInfoTableRef = new Reference(ref br),
                                                            pitch = br.ReadSingle(),
                                                            sendValueRef = new Reference(ref br),
                                                            streamSoundExtensionRef = new Reference(ref br),
                                                            prefetchFileId = br.ReadUInt32(),
                                                            trackInfoTable = null,
                                                            trackInfo = null,
                                                            sendValue = null,
                                                            streamSoundExtension = null
                                                        };

                                                        //Get track info table.
                                                        if (s.streamInfo.trackInfoTableRef.offset != Reference.NULL_PTR && s.streamInfo.trackInfoTableRef.typeId == ReferenceTypes.Tables + 1)
                                                        {

                                                            //Set position.
                                                            br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.streamInfo.trackInfoTableRef.offset;

                                                            //Read the table.
                                                            s.streamInfo.trackInfoTable = new ReferenceTable(ref br);

                                                            //Get the track info.
                                                            s.streamInfo.trackInfo = new List<soundInfo.streamSoundInfo.trackInfoEntry>();
                                                            foreach (Reference r2 in s.streamInfo.trackInfoTable.references)
                                                            {

                                                                if (r2.offset != Reference.NULL_PTR && r2.typeId != 0)
                                                                {

                                                                    //Set position.
                                                                    br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.streamInfo.trackInfoTableRef.offset + r2.offset;

                                                                    soundInfo.streamSoundInfo.trackInfoEntry e = new soundInfo.streamSoundInfo.trackInfoEntry
                                                                    {
                                                                        volume = br.ReadByte(),
                                                                        pan = br.ReadByte(),
                                                                        span = br.ReadByte(),
                                                                        surroundMode = br.ReadByte(),
                                                                        globalChannelIndexTableRef = new Reference(ref br),
                                                                        sendValueRef = new Reference(ref br),
                                                                        lpfFreq = br.ReadByte(),
                                                                        biquadType = br.ReadByte(),
                                                                        biquadValue = br.ReadByte(),
                                                                        padding = br.ReadByte(),
                                                                        globalChannelIndexTable = null,
                                                                        sendValue = null
                                                                    };

                                                                    //Get the global channel index table.
                                                                    if (e.globalChannelIndexTableRef.typeId == ReferenceTypes.Tables && e.globalChannelIndexTableRef.offset != Reference.NULL_PTR)
                                                                    {

                                                                        //Set position.
                                                                        br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.streamInfo.trackInfoTableRef.offset + r2.offset + e.globalChannelIndexTableRef.offset;

                                                                        e.globalChannelIndexTable = new Table<byte>();
                                                                        e.globalChannelIndexTable.count = br.ReadUInt32();
                                                                        e.globalChannelIndexTable.entries = new List<byte>();
                                                                        for (int i = 0; i < e.globalChannelIndexTable.count; i++)
                                                                        {
                                                                            e.globalChannelIndexTable.entries.Add(br.ReadByte());
                                                                        }

                                                                    }

                                                                    //Get the send value.
                                                                    if (e.sendValueRef.typeId == ReferenceTypes.SAR_Info_Send && e.sendValueRef.offset != Reference.NULL_PTR)
                                                                    {

                                                                        //Set position.
                                                                        br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.streamInfo.trackInfoTableRef.offset + r2.offset + e.sendValueRef.offset;

                                                                        //Read send value.
                                                                        e.sendValue = new soundInfo.streamSoundInfo.sendValueStruct
                                                                        {
                                                                            mainSend = br.ReadByte(),
                                                                            fxSend = br.ReadBytes(NWConstants.AUX_BUS_NUM)
                                                                        };

                                                                    }

                                                                    //Add entry.
                                                                    s.streamInfo.trackInfo.Add(e);

                                                                }

                                                            }

                                                        }

                                                        //Get send value.
                                                        if (s.streamInfo.sendValueRef.offset != Reference.NULL_PTR && s.streamInfo.sendValueRef.typeId == ReferenceTypes.SAR_Info_Send)
                                                        {

                                                            //Set position.
                                                            br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.streamInfo.sendValueRef.offset;

                                                            s.streamInfo.sendValue = new soundInfo.streamSoundInfo.sendValueStruct
                                                            {
                                                                mainSend = br.ReadByte(),
                                                                fxSend = br.ReadBytes(NWConstants.AUX_BUS_NUM),
                                                            };

                                                        }

                                                        //Get stream sound extension.
                                                        if (s.streamInfo.streamSoundExtensionRef.offset != Reference.NULL_PTR && s.streamInfo.streamSoundExtensionRef.typeId != 0)
                                                        {

                                                            //Set position.
                                                            br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.streamInfo.streamSoundExtensionRef.offset;

                                                            s.streamInfo.streamSoundExtension = new soundInfo.streamSoundInfo.streamSoundExtensionStruct
                                                            {
                                                                streamTypeInfo = br.ReadUInt32(),
                                                                loopStartFrame = br.ReadUInt32(),
                                                                loopEndFrame = br.ReadUInt32()
                                                            };

                                                        }

                                                    }

                                                    break;

                                                //Wave.
                                                case ReferenceTypes.SAR_ItemInfos + 2:
                                                    br.Position -= 4;
                                                    s.waveInfo = new soundInfo.waveSoundInfo
                                                    {
                                                        paddingValue = br.ReadUInt32(),
                                                        index = br.ReadUInt32(),
                                                        allocateTrackCount = br.ReadUInt32(),
                                                        flags = new FlagParameters(ref br)
                                                    };
                                                    break;

                                                //Sequence.
                                                case ReferenceTypes.SAR_ItemInfos + 3:
                                                    br.Position -= 4;
                                                    s.sequenceInfo = new soundInfo.sequenceSoundInfo
                                                    {
                                                        paddingValue = br.ReadUInt32(),
                                                        bankIdTableRef = new Reference(ref br),
                                                        allocateTrackFlags = br.ReadUInt32(),
                                                        flags = new FlagParameters(ref br),
                                                        bankIdTable = null
                                                    };

                                                    //Get bank id table.
                                                    if (s.sequenceInfo.bankIdTableRef.offset != Reference.NULL_PTR && s.sequenceInfo.bankIdTableRef.typeId == ReferenceTypes.Tables)
                                                    {

                                                        //Set position.
                                                        br.Position = soundBasePosRef + r.offset + s.detailSoundRef.offset + s.sequenceInfo.bankIdTableRef.offset;

                                                        //Read the table.
                                                        s.sequenceInfo.bankIdTable = new Table<Id>();
                                                        s.sequenceInfo.bankIdTable.count = br.ReadUInt32();
                                                        s.sequenceInfo.bankIdTable.entries = new List<Id>();
                                                        for (int i = 0; i < s.sequenceInfo.bankIdTable.count; i++)
                                                        {
                                                            s.sequenceInfo.bankIdTable.entries.Add(new Id(ref br));
                                                        }

                                                    }
                                                    break;

                                            }

                                        }

                                        //Get sound 3d info.
                                        if (s.flags.isFlagEnabled[8])
                                        {

                                            //Set position.
                                            br.Position = soundBasePosRef + r.offset + s.flags.flagValues[8];

                                            //New 3d info.
                                            s.sound3d = new soundInfo.sound3dInfo
                                            {
                                                flagBoxes = br.ReadUInt32(),
                                                dimensionalAttenuation = br.ReadSingle(),
                                                attenuationCurveType = br.ReadByte(),
                                                dopplerFactor = br.ReadByte(),
                                                padding = br.ReadUInt16(),
                                                optionParameter = new FlagParameters(ref br)
                                            };

                                        }

                                    }

                                    //Add sound group.
                                    sounds.Add(s);

                                }

                            }

                            break;

                        //Sound Group Info.
                        case ReferenceTypes.SAR_Section_SoundGroupInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                soundGroups = null;
                                soundGrpRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                soundGroups = new List<soundGroupInfo>();

                                //Get the current pos.
                                long soundGrpBasePosRef = br.Position;

                                //Read the table of files.
                                soundGrpRefTable = new ReferenceTable(ref br);

                                //Get the entries.
                                foreach (Reference r in soundGrpRefTable.references)
                                {

                                    //New sound group.
                                    soundGroupInfo g = null;

                                    //If not null.
                                    if (r.offset != Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_SoundGroup)
                                    {

                                        //Set position.
                                        br.Position = soundGrpBasePosRef + r.offset;
                                        g = new soundGroupInfo
                                        {
                                            firstId = new Id(ref br),
                                            lastId = new Id(ref br),
                                            fileIdTableRef = new Reference(ref br),
                                            waveSoundGroupTableRef = new Reference(ref br),
                                            flags = new FlagParameters(ref br)
                                        };

                                        //If Null.
                                        if (g.fileIdTableRef.offset == Reference.NULL_PTR || g.fileIdTableRef.typeId != ReferenceTypes.Tables)
                                        {

                                            g.fileIdTable = null;

                                        }

                                        //Normal data.
                                        else
                                        {

                                            //Go to position.
                                            br.Position = soundGrpBasePosRef + r.offset + g.fileIdTableRef.offset;

                                            //Read table.
                                            g.fileIdTable = new Table<UInt32>();
                                            g.fileIdTable.count = br.ReadUInt32();
                                            g.fileIdTable.entries = new List<UInt32>();
                                            for (int i = 0; i < g.fileIdTable.count; i++)
                                            {
                                                g.fileIdTable.entries.Add(br.ReadUInt32());
                                            }

                                        }


                                        //If Null.
                                        g.waveSoundGroupTable = new soundGroupInfo.waveSoundGroupTableStruct();
                                        if (g.waveSoundGroupTableRef.offset == Reference.NULL_PTR || g.waveSoundGroupTableRef.typeId != ReferenceTypes.SAR_Info_WaveSoundGroup)
                                        {

                                            g.waveSoundGroupTable = null;

                                        }

                                        //Normal data.
                                        else
                                        {

                                            //Go to position.
                                            br.Position = soundGrpBasePosRef + r.offset + g.waveSoundGroupTableRef.offset;

                                            //Read data.
                                            g.waveSoundGroupTable = new soundGroupInfo.waveSoundGroupTableStruct
                                            {

                                                waveArchiveIdTableRef = new Reference(ref br),
                                                flags = new FlagParameters(ref br),

                                            };

                                            //If Null.
                                            if (g.waveSoundGroupTable.waveArchiveIdTableRef.offset == Reference.NULL_PTR || g.waveSoundGroupTable.waveArchiveIdTableRef.typeId != ReferenceTypes.Tables)
                                            {

                                                g.waveSoundGroupTable.waveArchiveIdTable = null;

                                            }

                                            //Normal data.
                                            else
                                            {

                                                //Go to position.
                                                br.Position = soundGrpBasePosRef + r.offset + g.waveSoundGroupTableRef.offset + g.waveSoundGroupTable.waveArchiveIdTableRef.offset;

                                                //Read table.
                                                g.waveSoundGroupTable.waveArchiveIdTable = new Table<Id>();
                                                g.waveSoundGroupTable.waveArchiveIdTable.count = br.ReadUInt32();
                                                g.waveSoundGroupTable.waveArchiveIdTable.entries = new List<Id>();
                                                for (int i = 0; i < g.waveSoundGroupTable.waveArchiveIdTable.count; i++)
                                                {
                                                    g.waveSoundGroupTable.waveArchiveIdTable.entries.Add(new Id(ref br));
                                                }

                                            }

                                        }

                                    }

                                    //Add sound group.
                                    soundGroups.Add(g);

                                }

                            }

                            break;

                        //Bank Info.
                        case ReferenceTypes.SAR_Section_BankInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                banks = null;
                                bankRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                banks = new List<bankInfo>();

                                //Get the current pos.
                                long bankBasePosRef = br.Position;

                                //Read the table of files.
                                bankRefTable = new ReferenceTable(ref br);

                                //Get the entries.
                                foreach (Reference r in bankRefTable.references)
                                {

                                    //New player.
                                    bankInfo bi = null;

                                    //If not null.
                                    if (r.offset != Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_Bank)
                                    {

                                        //Set position.
                                        br.Position = bankBasePosRef + r.offset;
                                        bi = new bankInfo
                                        {
                                            fileId = br.ReadUInt32(),
                                            waveArchiveItemIdTableRef = new Reference(ref br),
                                            flags = new FlagParameters(ref br),
                                        };

                                        //If Null.
                                        if (bi.waveArchiveItemIdTableRef.offset == Reference.NULL_PTR || bi.waveArchiveItemIdTableRef.typeId != ReferenceTypes.Tables)
                                        {

                                            bi.waveArchiveItemIdTable = null;

                                        }

                                        //Normal data.
                                        else
                                        {

                                            //Go to position.
                                            br.Position = bankBasePosRef + r.offset + bi.waveArchiveItemIdTableRef.offset;

                                            //Read table.
                                            bi.waveArchiveItemIdTable = new Table<Id>();
                                            bi.waveArchiveItemIdTable.count = br.ReadUInt32();
                                            bi.waveArchiveItemIdTable.entries = new List<Id>();
                                            for (int i = 0; i < bi.waveArchiveItemIdTable.count; i++)
                                            {
                                                bi.waveArchiveItemIdTable.entries.Add(new Id(ref br));
                                            }

                                        }

                                    }

                                    //Add bank.
                                    banks.Add(bi);

                                }

                            }

                            break;

                        //War Info.
                        case ReferenceTypes.SAR_Section_WaveArchiveInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                wars = null;
                                warRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                wars = new List<waveArchiveInfo>();

                                //Get the current pos.
                                long warBasePosRef = br.Position;

                                //Read the table of files.
                                warRefTable = new ReferenceTable(ref br);

                                //Get the entries.
                                foreach (Reference r in warRefTable.references)
                                {

                                    //New war.
                                    waveArchiveInfo w = null;

                                    //If not null.
                                    if (r.offset != Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_WaveArchive)
                                    {

                                        //Set position.
                                        br.Position = warBasePosRef + r.offset;
                                        w = new waveArchiveInfo
                                        {
                                            fileId = br.ReadUInt32(),
                                            loadIndividual = br.ReadByte(),
                                            padding = br.ReadBytes(3),
                                            flags = new FlagParameters(ref br),
                                        };

                                    }

                                    //Add war.
                                    wars.Add(w);

                                }

                            }

                            break;

                        //Group Info.
                        case ReferenceTypes.SAR_Section_GroupInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                groups = null;
                                grpRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                groups = new List<groupInfo>();

                                //Get the current pos.
                                long groupBasePosRef = br.Position;

                                //Read the table of files.
                                grpRefTable = new ReferenceTable(ref br);

                                //Get the entries.
                                foreach (Reference r in grpRefTable.references)
                                {

                                    //New grp.
                                    groupInfo g = null;

                                    //If not null.
                                    if (r.offset != Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_Group)
                                    {

                                        //Set position.
                                        br.Position = groupBasePosRef + r.offset;
                                        g = new groupInfo
                                        {
                                            fileId = br.ReadUInt32(),
                                            flags = new FlagParameters(ref br),
                                        };

                                    }

                                    //Add grp.
                                    groups.Add(g);

                                }

                            }

                            break;

                        //Player Info.
                        case ReferenceTypes.SAR_Section_PlayerInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                players = null;
                                playerRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                players = new List<playerInfo>();

                                //Get the current pos.
                                long playerBasePosRef = br.Position;

                                //Read the table of files.
                                playerRefTable = new ReferenceTable(ref br);

                                //Get the entries.
                                foreach (Reference r in playerRefTable.references)
                                {

                                    //New player.
                                    playerInfo p = null;

                                    //If not null.
                                    if (r.offset != Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_Player)
                                    {

                                        //Set position.
                                        br.Position = playerBasePosRef + r.offset;
                                        p = new playerInfo
                                        {
                                            playableSoundLimit = br.ReadUInt32(),
                                            flags = new FlagParameters(ref br)
                                        };

                                    }

                                    //Add player.
                                    players.Add(p);

                                }

                            }

                            break;

                        //File Info.
                        case ReferenceTypes.SAR_Section_FileInfo:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {
                                files = null;
                                fileRefTable = null;
                            }

                            //Normal data.
                            else
                            {

                                files = new List<fileInfo>();

                                //Get the current pos.
                                long fileBasePosRef = br.Position;

                                //Read the table of files.
                                fileRefTable = new ReferenceTable(ref br);

                                //Now get the files.
                                foreach (Reference r in fileRefTable.references)
                                {

                                    //New file info.
                                    fileInfo f = null;

                                    //Null pointer.
                                    if (r.offset == Reference.NULL_PTR || r.typeId != ReferenceTypes.SAR_Info_File)
                                    {
                                        // ;)
                                    }

                                    //Normal info.
                                    else
                                    {

                                        //Set position.
                                        br.Position = fileBasePosRef + r.offset;

                                        //Read general data.
                                        f = new fileInfo
                                        {

                                            fileLocationRef = new Reference(ref br),
                                            flags = new FlagParameters(ref br),
                                            internalFileGroupTableRef = null,
                                            internalFileGroupTable = null,
                                            internalFileInfo = null,
                                            externalFileName = null

                                        };


                                        //Set position.
                                        br.Position = fileBasePosRef + r.offset + f.fileLocationRef.offset;

                                        //Switch file location type.
                                        switch (f.fileLocationRef.typeId)
                                        {

                                            case ReferenceTypes.SAR_Info_ExternalFile:
                                                if (f.fileLocationRef.offset != Reference.NULL_PTR)
                                                {
                                                    List<char> chars = new List<char>();
                                                    bool ended = false;
                                                    while (!ended)
                                                    {
                                                        char c = br.ReadChar();
                                                        if (c != 0) { chars.Add(c); } else { ended = true; }
                                                    }
                                                    f.externalFileName = new string(chars.ToArray());
                                                }
                                                break;

                                            case ReferenceTypes.SAR_Info_InternalFile:
                                                if (f.fileLocationRef.offset != Reference.NULL_PTR)
                                                {
                                                    f.internalFileInfo = new SizedReference(ref br);

                                                    if (version >= NWConstants.NEW_FILE_VERSION)
                                                    {
                                                        f.internalFileGroupTableRef = new Reference(ref br);

                                                        if (f.internalFileGroupTableRef.offset != Reference.NULL_PTR && f.internalFileGroupTableRef.typeId == ReferenceTypes.Tables)
                                                        {

                                                            br.Position = fileBasePosRef + r.offset + f.fileLocationRef.offset + f.internalFileGroupTableRef.offset;
                                                            f.internalFileGroupTable = new Table<UInt32>();
                                                            f.internalFileGroupTable.count = br.ReadUInt32();
                                                            f.internalFileGroupTable.entries = new List<uint>();
                                                            for (int j = 0; j < f.internalFileGroupTable.count; j++)
                                                            {
                                                                f.internalFileGroupTable.entries.Add(br.ReadUInt32());
                                                            }

                                                        }
                                                    }
                                                }
                                                break;

                                        }

                                    }

                                    //Add file to the list.
                                    files.Add(f);

                                }

                            }

                            break;

                        //Project Info.
                        case ReferenceTypes.SAR_Info_Project:

                            //Null pointer.
                            if (infoSectionRef.offset == Reference.NULL_PTR)
                            {

                                projectInfo = null;

                            }

                            //Normal data.
                            else
                            {

                                projectInfo = new soundArchivePlayerInfo
                                {
                                    maxSeq = br.ReadUInt16(),
                                    maxSeqTracks = br.ReadUInt16(),
                                    maxStreamSounds = br.ReadUInt16(),
                                    maxStreamTracks = br.ReadUInt16(),
                                    maxStreamChannels = br.ReadUInt16(),
                                    maxWaveSounds = br.ReadUInt16(),
                                    maxWaveTracks = br.ReadUInt16(),
                                    streamBufferTimes = br.ReadByte(),
                                    padding = br.ReadByte(),
                                    options = br.ReadUInt32()
                                };

                            }

                            break;

                    }

                }

            }


            /// <summary>
            /// Convert the INFO block to bytes.
            /// </summary>
            /// <param name="endian"></param>
            /// <returns>The new INFO block binary.</returns>
            public byte[] ToBytes(UInt16 endian, UInt32 version)
            {

                //Update.
                Update(version);

                //Setup the stream output.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                //Change endian.
                if (endian == ByteOrder.BigEndian)
                {
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                }
                else
                {
                    bw.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }


                //Write the block header.
                bw.Write(magic);
                bw.Write(size);

                //Write the offset references.
                foreach (Reference r in offsetRefs)
                {
                    r.Write(ref bw);
                }

                //Write sound info.
                soundRefTable.Write(ref bw);
                foreach (soundInfo s in sounds)
                {

                    bw.Write(s.fileId);
                    s.playerNumber.Write(ref bw);
                    bw.Write(s.volume);
                    bw.Write(s.remoteFilter);
                    bw.Write(s.padding);
                    s.detailSoundRef.Write(ref bw);
                    s.flags.Write(ref bw);

                    //3D sound info.
                    if (s.flags.isFlagEnabled[8])
                    {
                        bw.Write(s.sound3d.flagBoxes);
                        bw.Write(s.sound3d.dimensionalAttenuation);
                        bw.Write(s.sound3d.attenuationCurveType);
                        bw.Write(s.sound3d.dopplerFactor);
                        bw.Write(s.sound3d.padding);
                        s.sound3d.optionParameter.Write(ref bw);
                    }


                    //Detail sound.
                    switch (s.detailSoundRef.typeId)
                    {

                        case ReferenceTypes.SAR_Info_SequenceSound:

                            bw.Write(s.sequenceInfo.paddingValue);
                            s.sequenceInfo.bankIdTableRef.Write(ref bw);
                            bw.Write(s.sequenceInfo.allocateTrackFlags);
                            s.sequenceInfo.flags.Write(ref bw);

                            if (s.sequenceInfo.bankIdTable != null)
                            {
                                bw.Write(s.sequenceInfo.bankIdTable.count);
                                foreach (Id id in s.sequenceInfo.bankIdTable.entries)
                                {
                                    id.Write(ref bw);
                                }
                            }
                            break;

                        case ReferenceTypes.SAR_Info_WaveSound:

                            bw.Write(s.waveInfo.paddingValue);
                            bw.Write(s.waveInfo.index);
                            bw.Write(s.waveInfo.allocateTrackCount);
                            s.waveInfo.flags.Write(ref bw);
                            break;

                        case ReferenceTypes.SAR_Info_StreamSound:

                            if (version < NWConstants.NEW_STREAM_VERSION)
                            {

                                bw.Write(s.streamInfo.allocateTrackFlags);
                                bw.Write(s.streamInfo.channelCount);
                                s.streamInfo.oldFlags.Write(ref bw);

                            }
                            else
                            {

                                bw.Write(s.streamInfo.allocateTrackFlags);
                                bw.Write(s.streamInfo.channelCount);
                                s.streamInfo.trackInfoTableRef.Write(ref bw);
                                bw.Write(s.streamInfo.pitch);
                                s.streamInfo.sendValueRef.Write(ref bw);
                                s.streamInfo.streamSoundExtensionRef.Write(ref bw);
                                bw.Write(s.streamInfo.prefetchFileId);

                                if (s.streamInfo.trackInfoTable != null)
                                {
                                    s.streamInfo.trackInfoTable.Write(ref bw);
                                }

                                if (s.streamInfo.trackInfo != null)
                                {
                                    foreach (soundInfo.streamSoundInfo.trackInfoEntry t in s.streamInfo.trackInfo)
                                    {
                                        if (t != null)
                                        {
                                            bw.Write(t.volume);
                                            bw.Write(t.pan);
                                            bw.Write(t.span);
                                            bw.Write(t.surroundMode);
                                            t.globalChannelIndexTableRef.Write(ref bw);
                                            t.sendValueRef.Write(ref bw);
                                            bw.Write(t.lpfFreq);
                                            bw.Write(t.biquadType);
                                            bw.Write(t.biquadValue);
                                            bw.Write(t.padding);

                                            if (t.globalChannelIndexTable != null)
                                            {
                                                bw.Write(t.globalChannelIndexTable.count);
                                                foreach (byte by in t.globalChannelIndexTable.entries)
                                                {
                                                    bw.Write(by);
                                                }
                                                while ((bw.Position % 4) != 0)
                                                {
                                                    bw.Write((byte)0);
                                                }
                                            }

                                            if (t.sendValue != null)
                                            {
                                                bw.Write(t.sendValue.mainSend);
                                                bw.Write(t.sendValue.fxSend);
                                                bw.Write((UInt32)0);
                                            }

                                        }
                                    }
                                }

                                if (s.streamInfo.sendValue != null)
                                {
                                    bw.Write(s.streamInfo.sendValue.mainSend);
                                    bw.Write(s.streamInfo.sendValue.fxSend);
                                    bw.Write((UInt32)0);
                                }

                                if (s.streamInfo.streamSoundExtension != null)
                                {

                                    bw.Write(s.streamInfo.streamSoundExtension.streamTypeInfo);
                                    bw.Write(s.streamInfo.streamSoundExtension.loopStartFrame);
                                    bw.Write(s.streamInfo.streamSoundExtension.loopEndFrame);

                                }

                            }

                            break;

                    }

                }


                //Write sound group info.
                soundGrpRefTable.Write(ref bw);
                foreach (soundGroupInfo g in soundGroups)
                {

                    g.firstId.Write(ref bw);
                    g.lastId.Write(ref bw);
                    g.fileIdTableRef.Write(ref bw);
                    g.waveSoundGroupTableRef.Write(ref bw);
                    g.flags.Write(ref bw);

                    if (g.fileIdTable != null)
                    {
                        bw.Write(g.fileIdTable.count);
                        foreach (UInt32 u in g.fileIdTable.entries)
                        {
                            bw.Write(u);
                        }
                    }

                    if (g.waveSoundGroupTable != null)
                    {
                        g.waveSoundGroupTable.waveArchiveIdTableRef.Write(ref bw);
                        g.waveSoundGroupTable.flags.Write(ref bw);

                        if (g.waveSoundGroupTable.waveArchiveIdTable != null)
                        {
                            bw.Write(g.waveSoundGroupTable.waveArchiveIdTable.count);
                            foreach (Id id in g.waveSoundGroupTable.waveArchiveIdTable.entries)
                            {
                                id.Write(ref bw);
                            }
                        }
                    }

                }


                //Write bank info.
                bankRefTable.Write(ref bw);
                foreach (bankInfo b in banks)
                {

                    bw.Write(b.fileId);
                    b.waveArchiveItemIdTableRef.Write(ref bw);
                    b.flags.Write(ref bw);

                    if (b.waveArchiveItemIdTable != null)
                    {

                        bw.Write(b.waveArchiveItemIdTable.count);
                        foreach (Id id in b.waveArchiveItemIdTable.entries)
                        {
                            id.Write(ref bw);
                        }

                    }

                }


                //Write war info.
                warRefTable.Write(ref bw);
                foreach (waveArchiveInfo w in wars)
                {

                    bw.Write(w.fileId);
                    bw.Write(w.loadIndividual);
                    bw.Write(w.padding);
                    w.flags.Write(ref bw);

                }


                //Write group info.
                grpRefTable.Write(ref bw);
                foreach (groupInfo g in groups)
                {

                    bw.Write(g.fileId);
                    g.flags.Write(ref bw);

                }


                //Write player info.
                playerRefTable.Write(ref bw);
                foreach (playerInfo p in players)
                {

                    bw.Write(p.playableSoundLimit);
                    p.flags.Write(ref bw);

                }


                //File info.
                fileRefTable.Write(ref bw);
                foreach (fileInfo f in files)
                {

                    f.fileLocationRef.Write(ref bw);
                    f.flags.Write(ref bw);

                    if (f.internalFileInfo != null)
                    {
                        f.internalFileInfo.Write(ref bw);

                        if (version >= NWConstants.NEW_FILE_VERSION)
                        {
                            f.internalFileGroupTableRef.Write(ref bw);
                            if (f.internalFileGroupTable != null)
                            {
                                bw.Write(f.internalFileGroupTable.count);
                                foreach (UInt32 u in f.internalFileGroupTable.entries)
                                {
                                    bw.Write(u);
                                }
                            }
                        }
                    }
                    else if (f.externalFileName != null)
                    {
                        bw.Write(f.externalFileName.ToCharArray());
                        bw.Write((byte)0);
                        while ((bw.Position % 4) != 0)
                        {
                            bw.Write((byte)0);
                        }
                    }

                }


                //Project info.
                bw.Write(projectInfo.maxSeq);
                bw.Write(projectInfo.maxSeqTracks);
                bw.Write(projectInfo.maxStreamSounds);
                bw.Write(projectInfo.maxStreamTracks);
                bw.Write(projectInfo.maxStreamChannels);
                bw.Write(projectInfo.maxWaveSounds);
                bw.Write(projectInfo.maxWaveTracks);
                bw.Write(projectInfo.streamBufferTimes);
                bw.Write(projectInfo.padding);
                bw.Write(projectInfo.options);


                //Write padding.
                while ((bw.Position % 32) != 0)
                {
                    bw.Write((byte)0);
                }


                //Return the stream output.
                return o.ToArray();

            }


            /// <summary>
            /// Update the file offsets and stuff.
            /// </summary>
            /// <param name="version"></param>
            public void Update(UInt32 version)
            {

                magic = "INFO".ToCharArray();
                //size = 0xFFFFFFFF;
                //offsetRefs = new Reference[8];

                //Sound table reference table reference.
                offsetRefs[0] = new Reference(ReferenceTypes.SAR_Section_SoundInfo, 0x40);

                //Sound table reference table.
                soundRefTable = new ReferenceTable(new List<Reference>());
                soundRefTable.count = (UInt32)sounds.Count();

                //Add sounds.
                Int32 currentSoundOffset = (Int32)soundRefTable.count * 8 + 4;
                for (int i = 0; i < sounds.Count(); i++)
                {

                    //If sound is not null.
                    if (sounds[i] != null)
                    {

                        //Inside sound info offset.
                        Int32 insideSoundInfoOffset = 0;

                        //Add reference to sound.
                        soundRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_Sound, currentSoundOffset));

                        //Sound info.
                        currentSoundOffset += 20;
                        insideSoundInfoOffset += 20;

                        //Flags in sound info.
                        currentSoundOffset += sounds[i].flags.GetSize();
                        insideSoundInfoOffset += sounds[i].flags.GetSize();

                        //3D sound info.
                        if (sounds[i].sound3d != null)
                        {
                            sounds[i].flags.isFlagEnabled[8] = true;
                        }
                        else
                        {
                            sounds[i].flags.isFlagEnabled[8] = false;
                        }

                        if (sounds[i].flags.isFlagEnabled[8])
                        {
                            sounds[i].flags.flagValues[8] = (UInt32)insideSoundInfoOffset;
                            currentSoundOffset += 12;
                            currentSoundOffset += sounds[i].sound3d.optionParameter.GetSize();
                            insideSoundInfoOffset += 12;
                            insideSoundInfoOffset += sounds[i].sound3d.optionParameter.GetSize();
                        }

                        //Sequence info.
                        if (sounds[i].sequenceInfo != null)
                        {

                            //Padding.
                            currentSoundOffset += 4;
                            insideSoundInfoOffset += 4;

                            //New detail reference.
                            sounds[i].detailSoundRef = new Reference(ReferenceTypes.SAR_Info_SequenceSound, insideSoundInfoOffset);

                            //Sequence info offset.
                            Int32 sequenceInfoOffset = 0;

                            currentSoundOffset += 12 + sounds[i].sequenceInfo.flags.GetSize();
                            insideSoundInfoOffset += 12 + sounds[i].sequenceInfo.flags.GetSize();
                            sequenceInfoOffset += 12 + sounds[i].sequenceInfo.flags.GetSize();

                            //Bank Id table.
                            if (sounds[i].sequenceInfo.bankIdTable != null)
                            {

                                sounds[i].sequenceInfo.bankIdTableRef = new Reference(ReferenceTypes.Tables, sequenceInfoOffset);
                                sounds[i].sequenceInfo.bankIdTable.count = (UInt32)sounds[i].sequenceInfo.bankIdTable.entries.Count();
                                currentSoundOffset += (Int32)sounds[i].sequenceInfo.bankIdTable.count * 4 + 4;
                                insideSoundInfoOffset += (Int32)sounds[i].sequenceInfo.bankIdTable.count * 4 + 4;

                            }
                            else
                            {

                                sounds[i].sequenceInfo.bankIdTableRef = new Reference(0, Reference.NULL_PTR);

                            }

                        }

                        //Wave info.
                        else if (sounds[i].waveInfo != null)
                        {

                            //Padding.
                            currentSoundOffset += 4;
                            insideSoundInfoOffset += 4;

                            //New detail reference.
                            sounds[i].detailSoundRef = new Reference(ReferenceTypes.SAR_Info_WaveSound, insideSoundInfoOffset);

                            //Wave info size.
                            currentSoundOffset += 8 + sounds[i].waveInfo.flags.GetSize();
                            insideSoundInfoOffset += 8 + sounds[i].waveInfo.flags.GetSize();

                        }

                        //Stream info.
                        else if (sounds[i].streamInfo != null)
                        {

                            //New detail reference.
                            sounds[i].detailSoundRef = new Reference(ReferenceTypes.SAR_Info_StreamSound, insideSoundInfoOffset);

                            //Old version.
                            if (version < NWConstants.NEW_STREAM_VERSION)
                            {

                                insideSoundInfoOffset += 4 + sounds[i].streamInfo.oldFlags.GetSize();
                                currentSoundOffset += 4 + sounds[i].streamInfo.oldFlags.GetSize();

                            }

                            //New version, where all hell breaks loose.
                            else
                            {

                                //Stream position.
                                Int32 streamInfoOffset = 0;

                                //General data.
                                currentSoundOffset += 36;
                                insideSoundInfoOffset += 36;
                                streamInfoOffset += 36;

                                //Track info.
                                if (sounds[i].streamInfo.trackInfo != null)
                                {

                                    //Reference to track info table.
                                    sounds[i].streamInfo.trackInfoTableRef = new Reference(ReferenceTypes.Tables + 1, streamInfoOffset);

                                    //Start of track info table reference table.
                                    Int32 trackInfoTableOffsetBase = 0;
                                    sounds[i].streamInfo.trackInfoTable = new ReferenceTable(new List<Reference>());
                                    sounds[i].streamInfo.trackInfoTable.count = (UInt32)sounds[i].streamInfo.trackInfo.Count();
                                    trackInfoTableOffsetBase += (Int32)sounds[i].streamInfo.trackInfoTable.count * 8 + 4;
                                    streamInfoOffset += (Int32)sounds[i].streamInfo.trackInfoTable.count * 8 + 4;
                                    insideSoundInfoOffset += (Int32)sounds[i].streamInfo.trackInfoTable.count * 8 + 4;
                                    currentSoundOffset += (Int32)sounds[i].streamInfo.trackInfoTable.count * 8 + 4;

                                    //Get the track info table offsets.
                                    for (int j = 0; j < sounds[i].streamInfo.trackInfo.Count(); j++)
                                    {

                                        //Track is not null.
                                        if (sounds[i].streamInfo.trackInfo[j] != null)
                                        {

                                            //Add new reference.
                                            sounds[i].streamInfo.trackInfoTable.references.Add(new Reference(ReferenceTypes.SAR_Info_StreamSoundTrack, trackInfoTableOffsetBase));

                                            //Start of track info.
                                            Int32 trackOffset = 0;

                                            //Basic data.
                                            trackOffset += 24;
                                            trackInfoTableOffsetBase += 24;
                                            streamInfoOffset += 24;
                                            insideSoundInfoOffset += 24;
                                            currentSoundOffset += 24;


                                            //Global channel index table.
                                            if (sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable != null)
                                            {

                                                //New reference.
                                                sounds[i].streamInfo.trackInfo[j].globalChannelIndexTableRef = new Reference(ReferenceTypes.Tables, trackOffset);

                                                //The index table.
                                                sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.count = (UInt32)sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.entries.Count();
                                                trackOffset += 4 + (Int32)sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.count;
                                                trackInfoTableOffsetBase += 4 + (Int32)sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.count;
                                                streamInfoOffset += 4 + (Int32)sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.count;
                                                insideSoundInfoOffset += 4 + (Int32)sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.count;
                                                currentSoundOffset += 4 + (Int32)sounds[i].streamInfo.trackInfo[j].globalChannelIndexTable.count;

                                                //Padding.
                                                while ((trackOffset % 4) != 0)
                                                {
                                                    trackOffset += 1;
                                                    trackInfoTableOffsetBase += 1;
                                                    streamInfoOffset += 1;
                                                    insideSoundInfoOffset += 1;
                                                    currentSoundOffset += 1;
                                                }

                                            }
                                            else
                                            {

                                                sounds[i].streamInfo.trackInfo[j].globalChannelIndexTableRef = new Reference(0, Reference.NULL_PTR);

                                            }


                                            //Send value.
                                            if (sounds[i].streamInfo.trackInfo[j].sendValue != null)
                                            {

                                                //Send value reference.
                                                sounds[i].streamInfo.trackInfo[j].sendValueRef = new Reference(ReferenceTypes.SAR_Info_Send, trackOffset);

                                                //Size.
                                                currentSoundOffset += 5 + NWConstants.AUX_BUS_NUM;
                                                insideSoundInfoOffset += 5 + NWConstants.AUX_BUS_NUM;
                                                streamInfoOffset += 5 + NWConstants.AUX_BUS_NUM;
                                                trackOffset += 5 + NWConstants.AUX_BUS_NUM;
                                                trackInfoTableOffsetBase += 5 + NWConstants.AUX_BUS_NUM;

                                            }
                                            else
                                            {

                                                sounds[i].streamInfo.trackInfo[j].sendValueRef = new Reference(0, Reference.NULL_PTR);

                                            }

                                        }

                                        //Track is null.
                                        else
                                        {

                                            //Add new reference.
                                            sounds[i].streamInfo.trackInfoTable.references.Add(new Reference(0, Reference.NULL_PTR));

                                        }

                                    }

                                }

                                //Send value.
                                if (sounds[i].streamInfo.sendValue != null)
                                {

                                    sounds[i].streamInfo.sendValueRef = new Reference(ReferenceTypes.SAR_Info_Send, streamInfoOffset);
                                    currentSoundOffset += 5 + NWConstants.AUX_BUS_NUM;
                                    insideSoundInfoOffset += 5 + NWConstants.AUX_BUS_NUM;
                                    streamInfoOffset += 5 + NWConstants.AUX_BUS_NUM;

                                }
                                else
                                {

                                    sounds[i].streamInfo.sendValueRef = new Reference(0, Reference.NULL_PTR);

                                }

                                //Stream sound extension.
                                if (sounds[i].streamInfo.streamSoundExtension != null)
                                {

                                    sounds[i].streamInfo.streamSoundExtensionRef = new Reference(ReferenceTypes.SAR_Info_StreamSoundExtension, streamInfoOffset);
                                    currentSoundOffset += 12;
                                    insideSoundInfoOffset += 12;
                                    streamInfoOffset += 12;

                                }
                                else
                                {

                                    sounds[i].streamInfo.streamSoundExtensionRef = new Reference(0, Reference.NULL_PTR);

                                }

                            }

                        }


                    }

                    //Sound is null.
                    else
                    {

                        soundRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //Sound group table reference table reference.
                offsetRefs[1] = new Reference(ReferenceTypes.SAR_Section_SoundGroupInfo, 0x40 + currentSoundOffset);

                //Sound group table reference table.
                soundGrpRefTable = new ReferenceTable(new List<Reference>());
                soundGrpRefTable.count = (UInt32)soundGroups.Count();

                //Add sound groups.
                Int32 currentSoundGroupOffset = (Int32)soundGrpRefTable.count * 8 + 4;
                for (int i = 0; i < soundGroups.Count(); i++)
                {

                    //Not null.
                    if (soundGroups[i] != null)
                    {

                        //Current sound group offset.
                        Int32 sgOffset = 0;

                        //Add reference.
                        soundGrpRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_SoundGroup, currentSoundGroupOffset));

                        //General data.
                        currentSoundGroupOffset += 24 + soundGroups[i].flags.GetSize();
                        sgOffset += 24 + soundGroups[i].flags.GetSize();

                        //File id table.
                        if (soundGroups[i].fileIdTable != null)
                        {

                            //New reference.
                            soundGroups[i].fileIdTableRef = new Reference(ReferenceTypes.Tables, sgOffset);

                            soundGroups[i].fileIdTable.count = (UInt32)soundGroups[i].fileIdTable.entries.Count();
                            currentSoundGroupOffset += (Int32)soundGroups[i].fileIdTable.count * 4 + 4;
                            sgOffset += (Int32)soundGroups[i].fileIdTable.count * 4 + 4;

                        }
                        else
                        {

                            soundGroups[i].fileIdTableRef = new Reference(0, Reference.NULL_PTR);

                        }

                        //Wave sound group table.
                        if (soundGroups[i].waveSoundGroupTable != null)
                        {

                            //New reference.
                            soundGroups[i].waveSoundGroupTableRef = new Reference(ReferenceTypes.SAR_Info_WaveSoundGroup, sgOffset);

                            //Offset in table.
                            Int32 waveSoundGroupOffset = 0;

                            //Basic data.
                            waveSoundGroupOffset += 8 + soundGroups[i].waveSoundGroupTable.flags.GetSize();
                            currentSoundGroupOffset += 8 + soundGroups[i].waveSoundGroupTable.flags.GetSize();
                            sgOffset += 8 + soundGroups[i].waveSoundGroupTable.flags.GetSize();

                            //Wave archive id table.
                            if (soundGroups[i].waveSoundGroupTable.waveArchiveIdTable != null)
                            {

                                //New reference.
                                soundGroups[i].waveSoundGroupTable.waveArchiveIdTableRef = new Reference(ReferenceTypes.Tables, waveSoundGroupOffset);

                                soundGroups[i].waveSoundGroupTable.waveArchiveIdTable.count = (UInt32)soundGroups[i].waveSoundGroupTable.waveArchiveIdTable.entries.Count();
                                waveSoundGroupOffset += (Int32)soundGroups[i].waveSoundGroupTable.waveArchiveIdTable.count * 4 + 4;
                                currentSoundGroupOffset += (Int32)soundGroups[i].waveSoundGroupTable.waveArchiveIdTable.count * 4 + 4;
                                sgOffset += (Int32)soundGroups[i].waveSoundGroupTable.waveArchiveIdTable.count * 4 + 4;

                            }
                            else
                            {

                                soundGroups[i].waveSoundGroupTable.waveArchiveIdTableRef = new Reference(0, Reference.NULL_PTR);

                            }

                        }
                        else
                        {

                            soundGroups[i].waveSoundGroupTableRef = new Reference(0, Reference.NULL_PTR);

                        }

                    }

                    //Null.
                    else
                    {

                        soundGrpRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //Bank group table reference table reference.
                offsetRefs[2] = new Reference(ReferenceTypes.SAR_Section_BankInfo, 0x40 + currentSoundOffset + currentSoundGroupOffset);

                //Bank table reference table.
                bankRefTable = new ReferenceTable(new List<Reference>());
                bankRefTable.count = (UInt32)banks.Count();

                //Add banks.
                Int32 currentBankOffset = (Int32)bankRefTable.count * 8 + 4;
                for (int i = 0; i < banks.Count(); i++)
                {

                    //Not null
                    if (banks[i] != null)
                    {

                        //Add reference.
                        bankRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_Bank, currentBankOffset));

                        //Bank offset.
                        Int32 bankOffset = 0;

                        //General data.
                        bankOffset += 12 + banks[i].flags.GetSize();
                        currentBankOffset += 12 + banks[i].flags.GetSize();

                        //Wave archive item id table.
                        if (banks[i].waveArchiveItemIdTable != null)
                        {

                            //New reference.
                            banks[i].waveArchiveItemIdTableRef = new Reference(ReferenceTypes.Tables, bankOffset);

                            banks[i].waveArchiveItemIdTable.count = (UInt32)banks[i].waveArchiveItemIdTable.entries.Count();

                            bankOffset += (Int32)banks[i].waveArchiveItemIdTable.count * 4 + 4;
                            currentBankOffset += (Int32)banks[i].waveArchiveItemIdTable.count * 4 + 4;

                        }
                        else
                        {

                            banks[i].waveArchiveItemIdTableRef = new Reference(0, Reference.NULL_PTR);

                        }

                    }

                    //Null.
                    else
                    {

                        bankRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //Wave archive table reference table reference.
                offsetRefs[3] = new Reference(ReferenceTypes.SAR_Section_WaveArchiveInfo, 0x40 + currentSoundOffset + currentSoundGroupOffset + currentBankOffset);

                //Wave archive table reference table.
                warRefTable = new ReferenceTable(new List<Reference>());
                warRefTable.count = (UInt32)wars.Count();

                //Add wave archives.
                Int32 currentWarOffset = (Int32)warRefTable.count * 8 + 4;
                for (int i = 0; i < wars.Count(); i++)
                {

                    //Not null.
                    if (wars[i] != null)
                    {

                        //Add reference.
                        warRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_WaveArchive, currentWarOffset));

                        //Basic data.
                        currentWarOffset += 8 + wars[i].flags.GetSize();

                    }

                    //Null.
                    else
                    {

                        warRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //Group table reference table reference.
                offsetRefs[4] = new Reference(ReferenceTypes.SAR_Section_GroupInfo, 0x40 + currentSoundOffset + currentSoundGroupOffset + currentBankOffset + currentWarOffset);

                //Group table reference table.
                grpRefTable = new ReferenceTable(new List<Reference>());
                grpRefTable.count = (UInt32)groups.Count();

                //Add groups.
                Int32 currentGroupOffset = (Int32)grpRefTable.count * 8 + 4;
                for (int i = 0; i < groups.Count(); i++)
                {

                    //Not null.
                    if (groups[i] != null)
                    {

                        //Add reference.
                        grpRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_Group, currentGroupOffset));

                        //Basic data.
                        currentGroupOffset += 4 + groups[i].flags.GetSize();

                    }

                    //Null.
                    else
                    {

                        grpRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //Player table reference table reference.
                offsetRefs[5] = new Reference(ReferenceTypes.SAR_Section_PlayerInfo, 0x40 + currentSoundOffset + currentSoundGroupOffset + currentBankOffset + currentWarOffset + currentGroupOffset);

                //Player table reference table.
                playerRefTable = new ReferenceTable(new List<Reference>());
                playerRefTable.count = (UInt32)players.Count();

                //Add players.
                Int32 currentPlayerOffset = (Int32)playerRefTable.count * 8 + 4;
                for (int i = 0; i < players.Count(); i++)
                {

                    //Not null.
                    if (players[i] != null)
                    {

                        //Add reference.
                        playerRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_Player, currentPlayerOffset));

                        //Basic data.
                        currentPlayerOffset += 4 + players[i].flags.GetSize();

                    }

                    //Null.
                    else
                    {

                        playerRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //File table reference table reference.
                offsetRefs[6] = new Reference(ReferenceTypes.SAR_Section_FileInfo, 0x40 + currentSoundOffset + currentSoundGroupOffset + currentBankOffset + currentWarOffset + currentGroupOffset + currentPlayerOffset);

                //File table reference table.
                fileRefTable = new ReferenceTable(new List<Reference>());
                fileRefTable.count = (UInt32)files.Count();

                //Add Files.
                Int32 currentFileOffset = (Int32)fileRefTable.count * 8 + 4;
                Int32 currentFileSizes = 0x18;
                for (int i = 0; i < files.Count(); i++)
                {

                    //Not null.
                    if (files[i] != null)
                    {

                        //Add reference.
                        fileRefTable.references.Add(new Reference(ReferenceTypes.SAR_Info_File, currentFileOffset));

                        //File offset.
                        Int32 fileOffset = 0;

                        //Basic data.
                        fileOffset += 8 + files[i].flags.GetSize();
                        currentFileOffset += 8 + files[i].flags.GetSize();


                        //External file info.
                        if (files[i].externalFileName != null)
                        {

                            //New reference.
                            files[i].fileLocationRef = new Reference(ReferenceTypes.SAR_Info_ExternalFile, fileOffset);

                            fileOffset += files[i].externalFileName.Length + 1;
                            currentFileOffset += files[i].externalFileName.Length + 1;

                            //Padding.
                            while ((fileOffset % 4) != 0)
                            {

                                fileOffset += 1;
                                currentFileOffset += 1;

                            }

                        }

                        //Internal file info.
                        else if (files[i].internalFileInfo != null)
                        {

                            //New reference.
                            files[i].fileLocationRef = new Reference(ReferenceTypes.SAR_Info_InternalFile, fileOffset);

                            //Write file reference.
                            if (files[i].file != null)
                            {

                                //Offset to file.
                                /*files[i].internalFileInfo = new SizedReference(ReferenceTypes.General, currentFileSizes, (UInt32)files[i].file.Length);
                                currentFileSizes += files[i].file.Length;
                                while ((currentFileSizes % 0x20) != 0) {
                                    currentFileSizes += 1;
                                }*/

                            }

                            //Null file.
                            else
                            {

                                files[i].internalFileInfo = new SizedReference(0, Reference.NULL_PTR, 0xFFFFFFFF);

                            }

                            //Increase offsets.
                            fileOffset += 12;
                            currentFileOffset += 12;

                            //If new version.
                            if (version >= NWConstants.NEW_FILE_VERSION)
                            {

                                if (files[i].internalFileGroupTable != null)
                                {

                                    files[i].internalFileGroupTableRef = new Reference(ReferenceTypes.Tables, 0x14);
                                    files[i].internalFileGroupTable.count = (UInt32)files[i].internalFileGroupTable.entries.Count();

                                    fileOffset += (Int32)files[i].internalFileGroupTable.count * 4 + 4;
                                    currentFileOffset += (Int32)files[i].internalFileGroupTable.count * 4 + 4;

                                }
                                else
                                {

                                    files[i].internalFileGroupTableRef = new Reference(0, Reference.NULL_PTR);

                                }

                                fileOffset += 8;
                                currentFileOffset += 8;

                            }

                        }

                        //Null info.
                        else
                        {

                            files[i].fileLocationRef = new Reference(0, Reference.NULL_PTR);

                        }

                    }

                    //Null.
                    else
                    {

                        fileRefTable.references.Add(new Reference(0, Reference.NULL_PTR));

                    }

                }


                //Project information.
                offsetRefs[7] = new Reference(ReferenceTypes.SAR_Info_Project, 0x40 + currentSoundOffset + currentSoundGroupOffset + currentBankOffset + currentWarOffset + currentGroupOffset + currentPlayerOffset + currentFileOffset);
                if (projectInfo == null)
                {
                    offsetRefs[7] = new Reference(0, Reference.NULL_PTR);
                }

                size = (UInt32)(0x40 + currentSoundOffset + currentSoundGroupOffset + currentBankOffset + currentWarOffset + currentGroupOffset + currentPlayerOffset + currentFileOffset + 20 + 8);
                if (projectInfo == null) { size -= 20; }
                while ((size % 0x20) != 0)
                {
                    size += 1;
                }

            }


        }

    }


    /// <summary>
    /// NWConstants.
    /// </summary>
    public static class NWConstants
    {

        /// <summary>
        /// Number of auxilary busses.
        /// </summary>
        public const int AUX_BUS_NUM = 3;

        /// <summary>
        /// Contains extra stream info if this version.
        /// </summary>
        public const UInt32 NEW_STREAM_VERSION = 0x00020200;

        /// <summary>
        /// Contains extra file info if this version.
        /// </summary>
        public const UInt32 NEW_FILE_VERSION = 0x00020000;

    }


}
