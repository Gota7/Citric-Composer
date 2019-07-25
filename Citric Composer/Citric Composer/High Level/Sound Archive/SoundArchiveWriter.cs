using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sound archive writer.
    /// </summary>
    public class SoundArchiveWriter {

        /// <summary>
        /// Write a sound archive.
        /// </summary>
        /// <param name="a">The sound archive.</param>
        /// <param name="archiveFolder">Folder path containing the archive.</param>
        /// <param name="pathToWriteTo">Path to write to.</param>
        /// <returns>The sound archive file.</returns>
        public static void WriteSoundArchive(SoundArchive a, string archiveFolder, string pathToWriteTo) {

            //New writer.
            FileStream o = new FileStream(pathToWriteTo, FileMode.Create);
            BinaryDataWriter bw = new BinaryDataWriter(o);

            //Start the file.
            FileWriter w = new FileWriter();
            w.InitFile(bw, a.WriteMode, "SAR", 3, a.Version);

            //String block.
            List<string> strings = new List<string>();
            #region StringBlock

            //String block.
            if (a.CreateStrings) {

                //Start block.
                w.InitBlock(bw, ReferenceTypes.SAR_Block_String, "STRG");

                //Init offsets.
                w.InitReference(bw, "StringTableRef");
                w.InitReference(bw, "StringLookupTableRef");

                //Get the list of strings, and item indices of things with valid strings.
                List<Tuple<int, string>> itemIndices = new List<Tuple<int, string>>();
                int stmStrStartInd = 0;
                int cnt = 0;
                foreach (var e in a.Streams) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int wsdStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.WaveSoundDatas) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int seqStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.Sequences) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int sstStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.SoundSets) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int bnkStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.Banks) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int warStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.WaveArchives) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int grpStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.Groups) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }
                int plyrStrStartInd = strings.Count;
                cnt = 0;
                foreach (var e in a.Players) {
                    if (e.Name != null) {
                        if (!strings.Contains(e.Name)) {
                            strings.Add(e.Name);
                        }
                        itemIndices.Add(new Tuple<int, string>(cnt, e.Name));
                    }
                    cnt++;
                }

                //String table is here.
                w.CloseReference(bw, 0x2400, "StringTableRef");

                //Table is another structure.
                w.StartStructure(bw);

                //Write string count.
                bw.Write((uint)strings.Count);

                //Write each string entry.
                for (int i = 0; i < strings.Count; i++) {

                    //Init reference.
                    w.InitReference(bw, "StringEntry" + i);

                    //Write string length plus null terminator.
                    bw.Write((uint)(strings[i].Length + 1));

                }

                //Write each actual string.
                for (int i = 0; i < strings.Count; i++) {

                    //Write the reference.
                    w.CloseReference(bw, 0x1F01, "StringEntry" + i);

                    //Write the string plus null terminator.
                    bw.Write((strings[i] + "\0").ToCharArray());

                }

                //Make sure to align to 4 bytes.
                w.Align(bw, 4);

                //Reference to the lookup table is here.
                w.CloseReference(bw, 0x2401, "StringLookupTableRef");

                //Patricia trie time.
                PatriciaTree p = new PatriciaTree();

                //Add each stream.
                int totalCount = 0;
                for (int i = 0; i < a.Streams.Count; i++) {
                    if (a.Streams[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.Streams[i].Name, SoundType = SoundType.Sound, StringTableIndex = strings.IndexOf(a.Streams[i].Name), ItemIndex = totalCount });
                    }
                    totalCount++;
                }

                //Add each wave sound data.
                for (int i = 0; i < a.WaveSoundDatas.Count; i++) {
                    if (a.WaveSoundDatas[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.WaveSoundDatas[i].Name, SoundType = SoundType.Sound, StringTableIndex = strings.IndexOf(a.WaveSoundDatas[i].Name), ItemIndex = totalCount });
                    }
                    totalCount++;
                }

                //Add each sequence.
                for (int i = 0; i < a.Sequences.Count; i++) {
                    if (a.Sequences[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.Sequences[i].Name, SoundType = SoundType.Sound, StringTableIndex = strings.IndexOf(a.Sequences[i].Name), ItemIndex = totalCount });
                    }
                    totalCount++;
                }

                //Add each sound set.
                for (int i = 0; i < a.SoundSets.Count; i++) {
                    if (a.SoundSets[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.SoundSets[i].Name, SoundType = SoundType.SoundGroup, StringTableIndex = strings.IndexOf(a.SoundSets[i].Name), ItemIndex = i });
                    }
                }

                //Add each bank.
                for (int i = 0; i < a.Banks.Count; i++) {
                    if (a.Banks[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.Banks[i].Name, SoundType = SoundType.Bank, StringTableIndex = strings.IndexOf(a.Banks[i].Name), ItemIndex = i });
                    }
                }

                //Add each wave archive.
                for (int i = 0; i < a.WaveArchives.Count; i++) {
                    if (a.WaveArchives[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.WaveArchives[i].Name, SoundType = SoundType.WaveArchive, StringTableIndex = strings.IndexOf(a.WaveArchives[i].Name), ItemIndex = i });
                    }
                }

                //Add each group.
                for (int i = 0; i < a.Groups.Count; i++) {
                    if (a.Groups[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.Groups[i].Name, SoundType = SoundType.Group, StringTableIndex = strings.IndexOf(a.Groups[i].Name), ItemIndex = i });
                    }
                }

                //Add each player.
                for (int i = 0; i < a.Players.Count; i++) {
                    if (a.Players[i].Name != null) {
                        p.Add(new PatriciaTree.PatriciaTreeItem() { Key = a.Players[i].Name, SoundType = SoundType.Player, StringTableIndex = strings.IndexOf(a.Players[i].Name), ItemIndex = i });
                    }
                }

                //Write the patricia trie info.
                bw.Write((uint)p.Root.Index);
                bw.Write((uint)p.Nodes.Count);

                //Write each patricia trie node.
                foreach (var n in p.Nodes) {

                    //If leaf node or not. Also write bit index.
                    if (n as PatriciaTree.ILeaf != null) {
                        bw.Write((ushort)1);
                        bw.Write((ushort)0xFFFF);
                    } else {
                        bw.Write((ushort)0);
                        bw.Write((ushort)n.Bit);
                    }

                    //Write the indices.
                    bw.Write(n.Left != null ? (uint)n.Left.Index : 0xFFFFFFFF);
                    bw.Write(n.Right != null ? (uint)n.Right.Index : 0xFFFFFFFF);
                    bw.Write(n.ContainsData ? (uint)n.StringTableIndex : 0xFFFFFFFF);

                    //Write the id.
                    if (n.ContainsData) {
                        bw.Write((uint)(((byte)n.SoundType << 24) | (n.ItemIndex & 0xFFFFFF)));
                    } else {
                        bw.Write((uint)0xFFFFFFFF);
                    }

                }

                //End table.
                w.EndStructure();

                //Block padding.
                w.Align(bw, 0x20);

                //End block.
                w.CloseBlock(bw);

            }

            //No strings.
            else {
                w.WriteNullBlock(ReferenceTypes.SAR_Block_String);
            }

            #endregion

            //Prepare files. TODO ARAS AND GROUP WRITING!!!
            bool[] FileInGroup = new bool[a.Files.Count];
            int[] GroupNumbers = new int[a.Files.Count];
            int[] GroupFileIndices = new int[a.Files.Count];
            #region PrepareFiles

            //Pre-file checks. See what groups the file is in, and only embed if internal, not null, and not embedded in group.
            for (int i = 0; i < a.Files.Count; i++) {

                //Assign the file id.
                a.Files[i].FileId = i;

                //Read each group.
                for (int j = 0; j < a.Groups.Count; j++) {

                    //Not null group.
                    if (a.Groups[j] != null) {

                        //Not null file.
                        if (a.Groups[j].File != null) {

                            //Internal file.
                            if (a.Groups[j].File.File != null) {

                                //Each file in the group.
                                for (int k = 0; k < (a.Groups[j].File.File as Group).SoundFiles.Count; k++) {

                                    //File is not null.
                                    if ((a.Groups[j].File.File as Group).SoundFiles[k].File != null) {

                                        //File exist in group.
                                        if (a.Files[i] == (a.Groups[j].File.File as Group).SoundFiles[k].Reference && (a.Groups[j].File.File as Group).SoundFiles[k].Embed) {
                                            FileInGroup[a.Files[i].FileId] = true;
                                            GroupNumbers[a.Files[i].FileId] = j;
                                            GroupFileIndices[a.Files[i].FileId] = k;
                                        }

                                    }

                                }

                            }

                        }

                    }

                }

                //Embed file if not in group, and file data is not null.
                if (a.Files[i].File != null && !FileInGroup[i]) {
                    a.Files[i].Embed = true;
                }

            }

            #endregion

            //Info block. TODO.
            #region InfoBlock

            //Init the info block.
            w.InitBlock(bw, ReferenceTypes.SAR_Block_Info, "INFO");
            long infoBlockPos = bw.Position - 8;

            //Init references.
            w.InitReference(bw, "SoundInfoRefTableRef");
            w.InitReference(bw, "SoundGroupInfoRefTableRef");
            w.InitReference(bw, "BankInfoRefTableRef");
            w.InitReference(bw, "WaveArchiveInfoRefTableRef");
            w.InitReference(bw, "GroupInfoRefTableRef");
            w.InitReference(bw, "PlayerInfoRefTableRef");
            w.InitReference(bw, "FileInfoRefTableRef");
            w.InitReference(bw, "ProjectInfoRefTableRef");

            //Sound info.
            #region SoundInfo

            //Sound info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_SoundInfo, "SoundInfoRefTableRef");

            //Init the reference table.
            w.InitReferenceTable(bw, a.Sequences.Count + a.Streams.Count + a.WaveSoundDatas.Count, "SoundInfoRefTable");

            //Write each stream.
            foreach (StreamEntry e in a.Streams) {

                //Add offset.
                w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_Sound, "SoundInfoRefTable");

                //New structure.
                w.StartStructure(bw);

                //Write sound info.
                e.WriteSoundInfo(bw, w, a, strings);

                //Detailed offset.
                w.CloseReference(bw, ReferenceTypes.SAR_Info_StreamSound, "ToDetRef");

                //New structure.
                w.StartStructure(bw);

                //Write the stream track flags.
                bw.Write(e.GetFlags());
                bw.Write(e.AllocateChannelCount);

                //Send and filter are not available.
                if (!SoundArchiveVersions.SupportsExtraStreamInfo(a)) {

                    //Only exists in C type.
                    if (FileWriter.GetWriteModeChar(a.WriteMode) == 'C') {

                        //New flags.
                        Dictionary<int, uint> streamTypeFlags = new Dictionary<int, uint>();

                        //Add flags if needed.
                        if (e.LoopStartFrame != 0 || e.LoopEndFrame != 0) {
                            streamTypeFlags.Add(0, (uint)e.StreamFileType);
                            streamTypeFlags.Add(1, e.LoopStartFrame);
                            streamTypeFlags.Add(2, e.LoopEndFrame);
                        }

                        //Write flags.
                        new FlagParameters(streamTypeFlags).Write(ref bw);

                    }

                }

                //Send and filter exist.
                else {

                    //Track info table.
                    w.InitReference(bw, "TrackInfoTableRef");

                    //Pitch.
                    bw.Write(e.Pitch);

                    //Send value.
                    w.InitReference(bw, "SendValueRef");

                    //Stream sound extension.
                    w.InitReference(bw, "StreamSoundExtension");

                    //Prefetch file.
                    if (SoundArchiveVersions.SupportsPrefetchInfo(a)) {
                        if (e.GeneratePrefetchFile) {
                            bw.Write((uint)e.PrefetchFile.FileId);
                        } else {
                            bw.Write((uint)0xFFFFFFFF);
                        }
                    }

                    //Track info table.
                    if (e.Tracks == null) {
                        w.CloseNullReference(bw, "TrackInfoTableRef");
                    } else {

                        //Close reference.
                        w.CloseReference(bw, ReferenceTypes.Tables + 1, "TrackInfoTableRef");

                        //Start structure.
                        w.StartStructure(bw);

                        //Init reference table.
                        w.InitReferenceTable(bw, e.Tracks.Count, "TrackInfoTable");

                        //Write each track.
                        foreach (var t in e.Tracks) {

                            //Null.
                            if (t == null) {
                                w.AddReferenceTableNullReference("TrackInfoTable");
                            } else {

                                //Add track info.
                                w.AddReferenceTableReference(bw, 0x220E, "TrackInfoTable");

                                //Start structure.
                                w.StartStructure(bw);

                                //Write track.
                                bw.Write(t.Volume);
                                bw.Write(t.Pan);
                                bw.Write(t.Span);
                                bw.Write(t.SurroundMode);

                                //References.
                                w.InitReference(bw, "GlobalChannelTableRef");
                                w.InitReference(bw, "SendValue2Ref");

                                //More properties.
                                bw.Write(t.LpfFrequency);
                                bw.Write(t.BiquadType);
                                bw.Write(t.BiquadValue);
                                bw.Write((byte)0);

                                //Global channel references.
                                if (t.Channels == null) {
                                    w.CloseNullReference(bw, "GlobalChannelTableRef");
                                } else {

                                    //Write channels.
                                    w.CloseReference(bw, ReferenceTypes.Tables, "GlobalChannelTableRef");
                                    bw.Write((uint)t.Channels.Count);
                                    bw.Write(t.Channels.ToArray());
                                    w.Align(bw, 4);

                                }

                                //Send value.
                                if (t.SendValue == null) {
                                    w.CloseNullReference(bw, "SendValue2Ref");
                                } else {
                                    w.CloseReference(bw, ReferenceTypes.SAR_Info_Send, "SendValue2Ref");
                                    bw.Write(t.SendValue);
                                    w.Align(bw, 4);
                                    if (a.WriteMode != WriteMode.CTR) { bw.Write((uint)0); }
                                }

                                //End structure.
                                w.EndStructure();

                            }

                        }

                        //Close reference table.
                        w.CloseReferenceTable(bw, "TrackInfoTable");

                        //End structure.
                        w.EndStructure();

                    }

                    //Send value.
                    if (e.SendValue == null) {
                        w.CloseNullReference(bw, "SendValueRef");
                    } else {
                        w.CloseReference(bw, ReferenceTypes.SAR_Info_Send, "SendValueRef");
                        bw.Write(e.SendValue);
                        w.Align(bw, 4);
                        if (a.WriteMode != WriteMode.CTR) { bw.Write((uint)0); }
                    }

                    //Sound extension.
                    if (!e.SoundExtensionIncluded) {
                        w.CloseNullReference(bw, "StreamSoundExtension");
                    } else {
                        w.CloseReference(bw, ReferenceTypes.SAR_Info_StreamSoundExtension, "StreamSoundExtension");
                        bw.Write((uint)e.StreamFileType);
                        bw.Write(e.LoopStartFrame);
                        bw.Write(e.LoopEndFrame);
                    }

                }

                //End structure.
                w.EndStructure();

                //End structure.
                w.EndStructure();

            }

            //Write each wave sound data.
            foreach (WaveSoundDataEntry e in a.WaveSoundDatas) {

                //Add offset.
                w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_Sound, "SoundInfoRefTable");

                //New structure.
                w.StartStructure(bw);

                //Write sound info.
                e.WriteSoundInfo(bw, w, a, strings);

                //Detailed offset.
                w.CloseReference(bw, ReferenceTypes.SAR_Info_WaveSound, "ToDetRef");

                //Write the wave sound data.
                bw.Write((uint)e.WaveIndex);
                bw.Write((uint)e.AllocateTrackCount);
                bw.Write((uint)1);
                bw.Write((uint)((e.IsReleasePriority ? 0x0100 : 0x0000) | e.ChannelPriority));

                //End structure.
                w.EndStructure();

            }

            //Write each sequence.
            foreach (SequenceEntry e in a.Sequences) {

                //Add offset.
                w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_Sound, "SoundInfoRefTable");

                //New structure.
                w.StartStructure(bw);

                //Write sound info.
                e.WriteSoundInfo(bw, w, a, strings);

                //Detailed offset.
                w.CloseReference(bw, ReferenceTypes.SAR_Info_SequenceSound, "ToDetRef");

                //New structure.
                w.StartStructure(bw);

                //Write the sequence data.
                w.InitReference(bw, "BnkTableRef");
                bw.Write(e.GetFlags());
                bw.Write((uint)3);
                bw.Write(e.StartOffset);
                bw.Write((uint)((e.IsReleasePriority ? 0x0100 : 0x0000) | e.ChannelPriority));

                //Null entry.
                if (e.Banks == null) {
                    w.CloseNullReference(bw, "BnkTableRef");
                }

                //Valid entry.
                else {

                    //Add reference.
                    w.CloseReference(bw, ReferenceTypes.Tables, "BnkTableRef");

                    //Count the banks.
                    int bankNum = e.Banks.Length;
                    for (int i = e.Banks.Length - 1; i >= 0; i--) {
                        if (e.Banks[i] == null) { bankNum--; } else { break; }
                    }
                    bw.Write((uint)bankNum);
                    for (int i = 0; i < bankNum; i++) {
                        if (e.Banks[i] != null) {
                            new Id(SoundTypes.Bank, (uint)a.Banks.IndexOf(e.Banks[i])).Write(ref bw);
                        } else {
                            bw.Write((uint)0xFFFFFFFF);
                        }
                    }

                }

                //End structure.
                w.EndStructure();

                //End structure.
                w.EndStructure();

            }

            //Close stuff.
            w.CloseReferenceTable(bw, "SoundInfoRefTable");
            w.EndStructure();

            #endregion

            //Sound group info.
            #region SoundGroupInfo

            //Sound group info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_SoundGroupInfo, "SoundGroupInfoRefTableRef");

            //Write each entry.
            w.InitReferenceTable(bw, a.SoundSets.Count, "SoundGroupInfoRefTable");
            foreach (SoundSetEntry e in a.SoundSets) {

                //Add reference.
                if (e == null) {

                    //Add reference.
                    w.AddReferenceTableNullReference("SoundGroupInfoRefTable");

                } else {

                    //Add reference.
                    w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_SoundGroup, "SoundGroupInfoRefTable");

                    //Start structure.
                    w.StartStructure(bw);

                    //Write data.
                    new Id((byte)e.SoundType, (uint)e.StartIndex).Write(ref bw);
                    new Id((byte)e.SoundType, (uint)e.EndIndex).Write(ref bw);
                    w.InitReference(bw, "SoundGroupFileIdTableRef");
                    w.InitReference(bw, "WaveArchiveTableRefRef");

                    //New flags.
                    Dictionary<int, uint> flags = new Dictionary<int, uint>();

                    //Write string data.
                    if (a.CreateStrings && e.Name != null) {
                        flags.Add(0, (uint)strings.IndexOf(e.Name));
                    }

                    //Write flags.
                    new FlagParameters(flags).Write(ref bw);

                    //Write file table.
                    if (e.Files != null) {

                        //Write the reference.
                        w.CloseReference(bw, ReferenceTypes.Tables, "SoundGroupFileIdTableRef");

                        //Write the table.
                        bw.Write((uint)e.Files.Count);
                        foreach (var file in e.Files) {
                            bw.Write((uint)file.FileId);
                        }

                    } else {
                        w.CloseNullReference(bw, "SoundGroupFileIdTableRef");
                    }

                    //Write the wave archive table reference.
                    if (e.WaveArchives != null) {

                        //Write the reference.
                        w.CloseReference(bw, ReferenceTypes.SAR_Info_WaveSoundGroup, "WaveArchiveTableRefRef");

                        //Write the local reference.
                        bw.Write(ReferenceTypes.Tables);
                        bw.Write((ushort)0);
                        bw.Write((uint)0xC);

                        //Write the table.
                        bw.Write((uint)e.WaveArchives.Count);
                        foreach (var war in e.WaveArchives) {
                            new Id(SoundTypes.WaveArchive, (uint)a.WaveArchives.IndexOf(war));
                        }

                        //There are unused flags here.
                        bw.Write((uint)0);

                    } else {
                        w.CloseNullReference(bw, "WaveArchiveTableRefRef");
                    }

                    //End structure.
                    w.EndStructure();

                }

            }
            w.CloseReferenceTable(bw, "SoundGroupInfoRefTable");
            w.EndStructure();

            #endregion

            //Bank info.
            #region BankInfo

            //Bank info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_BankInfo, "BankInfoRefTableRef");

            //Write each entry.
            w.InitReferenceTable(bw, a.Banks.Count, "BankInfoRefTable");
            foreach (BankEntry e in a.Banks) {

                //Add reference.
                if (e == null) {

                    //Add reference.
                    w.AddReferenceTableNullReference("BankInfoRefTable");

                } else {

                    //Add reference.
                    w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_Bank, "BankInfoRefTable");

                    //New structure.
                    w.StartStructure(bw);

                    //Write data.
                    if (e.File != null) {
                        bw.Write((uint)e.File.FileId);
                    } else { bw.Write((uint)0xFFFFFFFF); }
                    w.InitReference(bw, "BankWarTableRef");

                    //New flags.
                    Dictionary<int, uint> flags = new Dictionary<int, uint>();

                    //Write string data.
                    if (a.CreateStrings && e.Name != null) {
                        flags.Add(0, (uint)strings.IndexOf(e.Name));
                    }

                    //Write flags.
                    new FlagParameters(flags).Write(ref bw);

                    //Wave archive table.
                    if (e.WaveArchives != null) {

                        //Write reference.
                        w.CloseReference(bw, ReferenceTypes.Tables, "BankWarTableRef");

                        //Write wave archives.
                        bw.Write((uint)e.WaveArchives.Count);
                        foreach (WaveArchiveEntry war in e.WaveArchives) {
                            new Id(SoundTypes.WaveArchive, (uint)a.WaveArchives.IndexOf(war));
                        }

                    } else {
                        w.CloseNullReference(bw, "BankWarTableRef");
                    }

                    //End structure.
                    w.EndStructure();

                }

            }
            w.CloseReferenceTable(bw, "BankInfoRefTable");
            w.EndStructure();

            #endregion

            //Wave archive info.
            #region WaveArchiveInfo

            //Wave archive info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_WaveArchiveInfo, "WaveArchiveInfoRefTableRef");

            //Write each entry.
            w.InitReferenceTable(bw, a.WaveArchives.Count, "WaveArchiveInfoRefTable");
            foreach (WaveArchiveEntry e in a.WaveArchives) {

                //Add reference.
                if (e == null) {

                    //Add reference.
                    w.AddReferenceTableNullReference("WaveArchiveInfoRefTable");

                } else {

                    //Add reference.
                    w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_WaveArchive, "WaveArchiveInfoRefTable");

                    //Write data.
                    if (e.File != null) {
                        bw.Write((uint)e.File.FileId);
                    } else { bw.Write((uint)0xFFFFFFFF); }
                    bw.Write(e.LoadIndividually);
                    bw.Write(new byte[3]);

                    //New flags.
                    Dictionary<int, uint> flags = new Dictionary<int, uint>();

                    //Write string data.
                    if (a.CreateStrings && e.Name != null) {
                        flags.Add(0, (uint)strings.IndexOf(e.Name));
                    }

                    //Number of waves.
                    if (e.IncludeWaveCount) {
                        uint waveCount = 0;
                        if (e.File != null) {
                            if (e.File.File != null) {
                                waveCount = (uint)(e.File.File as SoundWaveArchive).Count;
                            }
                        }
                        flags.Add(1, waveCount);
                    }

                    //Write flags.
                    new FlagParameters(flags).Write(ref bw);

                }

            }
            w.CloseReferenceTable(bw, "WaveArchiveInfoRefTable");
            w.EndStructure();

            #endregion

            //Group info.
            #region GroupInfo

            //Group info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_GroupInfo, "GroupInfoRefTableRef");

            //Write each entry.
            w.InitReferenceTable(bw, a.Groups.Count, "GroupInfoRefTable");
            foreach (GroupEntry g in a.Groups) {

                //Add reference.
                if (g == null) {

                    //Add reference.
                    w.AddReferenceTableNullReference("GroupInfoRefTable");

                } else {

                    //Add reference.
                    w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_Group, "GroupInfoRefTable");

                    //Write data.
                    if (g.File != null) {

                        //There is no file ID if aras.
                        if (g.File.FileType != EFileType.Aras) {
                            bw.Write((uint)g.File.FileId);
                        } else {
                            bw.Write((uint)0xFFFFFFFF);
                        }

                    } else { bw.Write((uint)0xFFFFFFFF); }

                    //New flags.
                    Dictionary<int, uint> flags = new Dictionary<int, uint>();

                    //Write string data.
                    if (a.CreateStrings && g.Name != null) {
                        flags.Add(0, (uint)strings.IndexOf(g.Name));
                    }

                    //Write flags.
                    new FlagParameters(flags).Write(ref bw);

                }

            }
            w.CloseReferenceTable(bw, "GroupInfoRefTable");
            w.EndStructure();

            #endregion

            //Player info.
            #region PlayerInfo

            //Player info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_PlayerInfo, "PlayerInfoRefTableRef");

            //Write each entry.
            w.InitReferenceTable(bw, a.Players.Count, "PlayerInfoRefTable");
            foreach (PlayerEntry p in a.Players) {

                //Add reference.
                if (p == null) {

                    //Add reference.
                    w.AddReferenceTableNullReference("PlayerInfoRefTable");

                } else {

                    //Add reference.
                    w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_Player, "PlayerInfoRefTable");

                    //Write data.
                    bw.Write((uint)p.SoundLimit);

                    //New flags.
                    Dictionary<int, uint> flags = new Dictionary<int, uint>();

                    //Write string data.
                    if (a.CreateStrings && p.Name != null) {
                        flags.Add(0, (uint)strings.IndexOf(p.Name));
                    }

                    //Player heap.
                    if (p.IncludeHeapSize) {
                        flags.Add(1, (uint)p.PlayerHeapSize);
                    }

                    //Write flags.
                    new FlagParameters(flags).Write(ref bw);

                }

            }
            w.CloseReferenceTable(bw, "PlayerInfoRefTable");
            w.EndStructure();

            #endregion

            //File info.
            #region FileInfo

            //File info.
            w.StartStructure(bw);
            w.CloseReference(bw, ReferenceTypes.SAR_Section_FileInfo, "FileInfoRefTableRef");

            //Write each entry.
            w.InitReferenceTable(bw, a.Files.Count, "FileInfoRefTable");
            for (int i = 0; i < a.Files.Count; i++) {

                //If the file is null.
                if (a.Files[i] == null) {
                    w.AddReferenceTableNullReference("FileInfoRefTable");
                }

                //Valid
                else {

                    //Add reference.
                    w.AddReferenceTableReference(bw, ReferenceTypes.SAR_Info_File, "FileInfoRefTable");

                    //Start stucture.
                    w.StartStructure(bw);

                    //Init every reference to detailed sound info.
                    w.InitReference(bw, "DetFileInfo");

                    //Blank flags.
                    bw.Write((uint)0);

                    //New structure.
                    w.StartStructure(bw);

                    //Write the information depending on the file type.
                    switch (a.Files[i].FileType) {

                        //Null reference.
                        case EFileType.NullReference:
                        case EFileType.Undefined:
                            w.CloseNullReference(bw, "DetFileInfo");
                            break;

                        //External.
                        case EFileType.External:
                            w.CloseReference(bw, ReferenceTypes.SAR_Info_ExternalFile, "DetFileInfo");
                            bw.Write((a.Files[i].ExternalFileName + "\0").ToCharArray());
                            w.Align(bw, 4);
                            break;

                        //Internal.
                        case EFileType.Internal:

                            //Add reference.
                            w.CloseReference(bw, ReferenceTypes.SAR_Info_InternalFile, "DetFileInfo");
                            w.InitSizedReference(bw, "File" + i);

                            //Write groups.
                            if (a.WriteMode == WriteMode.Cafe || a.WriteMode == WriteMode.NX) {
                                w.InitReference(bw, "FileGroupTableRef");
                                if (a.Files[i].Groups == null) {
                                    w.CloseNullReference(bw, "FileGroupTableRef");
                                } else {

                                    //Write reference.
                                    w.CloseReference(bw, ReferenceTypes.Tables, "FileGroupTableRef");
                                    bw.Write((uint)a.Files[i].Groups.Count);
                                    foreach (int groupNum in a.Files[i].Groups) {
                                        new Id(SoundTypes.Group, (uint)groupNum).Write(ref bw);
                                    }

                                }
                            }
                            break;

                        //Located within a group.
                        case EFileType.InGroupOnly:
                        case EFileType.Aras:

                            //Add reference.
                            w.CloseReference(bw, ReferenceTypes.SAR_Info_InternalFile, "DetFileInfo");
                            w.InitSizedReference(bw, "TempFileSize");
                            w.CloseSizedNullReference(bw, "TempFileSize");

                            //Write groups.
                            if (a.WriteMode == WriteMode.Cafe || a.WriteMode == WriteMode.NX) {
                                w.InitReference(bw, "FileGroupTableRef");
                                if (a.Files[i].Groups == null) {
                                    w.CloseNullReference(bw, "FileGroupTableRef");
                                } else {

                                    //Write reference.
                                    w.CloseReference(bw, ReferenceTypes.Tables, "FileGroupTableRef");
                                    bw.Write((uint)a.Files[i].Groups.Count);
                                    foreach (int groupNum in a.Files[i].Groups) {
                                        new Id(SoundTypes.Group, (uint)groupNum).Write(ref bw);
                                    }

                                }
                            }
                            break;

                    }

                    //End structure.
                    w.EndStructure();

                    //End structure.
                    w.EndStructure();

                }

            }
            w.CloseReferenceTable(bw, "FileInfoRefTable");
            w.EndStructure();

            #endregion

            //Project info.
            #region ProjectInfo

            //Reference.
            w.CloseReference(bw, ReferenceTypes.SAR_Info_Project, "ProjectInfoRefTableRef");

            //Write the project info.
            bw.Write(a.MaxSequences);
            bw.Write(a.MaxSequenceTracks);
            bw.Write(a.MaxStreamSounds);
            bw.Write(a.MaxStreamTracks);
            bw.Write(a.MaxStreamChannels);
            bw.Write(a.MaxWaveSounds);
            bw.Write(a.MaxWaveTracks);
            bw.Write(a.StreamBufferTimes);
            bw.Write((byte)0);
            bw.Write(a.Options);

            #endregion

            //Align and close the block.
            w.Align(bw, 0x20);
            w.CloseBlock(bw);

            #endregion

            //File block.
            #region FileBlock

            //Init the file block.
            w.InitBlock(bw, ReferenceTypes.SAR_Block_File, "FILE");
            long fileBlockPos = bw.Position - 8;

            //Write each file.
            bw.Write(new byte[0x18]);
            foreach (var f in a.Files) {

                //Embedded.
                if (f.FileType == EFileType.Internal) {

                    //Keep track of position.
                    long pos = bw.Position;

                    //Write file.
                    w.WriteFile(bw, f.File, 0x20, a.WriteMode);

                    //Close reference.
                    w.CloseSizedReference(bw, ReferenceTypes.General, (int)(bw.Position - fileBlockPos - (bw.Position - pos) - 8), (uint)(bw.Position - pos), "File" + f.FileId);

                }

                //Aras type.
                if (f.FileType == EFileType.Aras && f.File != null && f.FileName != null) {

                    //Write the aras file.
                    File.WriteAllBytes(archiveFolder + "/" + f.FileName + ".aras", WriteFile(f.File));

                }

            }

            //Correct each file needed.
            /*foreach (var f in a.Files) {

                //Write offset into group file data.
                if (f.FileType == EFileType.Internal && !f.Embed) {

                    //Jump to the group file offset.
                    File.Copy(pathToWriteTo, pathToWriteTo + "CITRIC_TMP.CCT");
                    FileStream src = new FileStream(pathToWriteTo + "CITRIC_TMP.CCT", FileMode.Open, FileAccess.Read);
                    BinaryDataReader br = new BinaryDataReader(src);
                    br.ByteOrder = bw.ByteOrder;

                    //Position writer to start of file.
                    long origPos = bw.Position;
                    bw.Position = 0x24;
                    bw.Position = infoBlockPos;
                    bw.Position += 0x3C;
                    br.Position = bw.Position;
                    bw.Position = br.ReadUInt32() + 8 + infoBlockPos;
                    long bas = bw.Position;
                    bw.Position += 8 + 8 * a.Groups[GroupNumbers[f.FileId]].File.FileId;
                    br.Position = bw.Position;
                    bw.Position = bas + br.ReadUInt32();
                    bw.Position += 0x10;
                    br.Position = bw.Position;
                    uint grpOffset = br.ReadUInt32();
                    bw.Position = 0x30;
                    bw.Position = fileBlockPos;
                    bw.Position += grpOffset + 8;
                    long grpPos = bw.Position;
                    bw.Position = 0x4C + 4 + 8 * GroupFileIndices[f.FileId] + grpPos;
                    br.Position = bw.Position;
                    uint filePos = br.ReadUInt32();
                    br.Position = grpPos + 0x24;
                    long grpFileBlockPos = br.ReadUInt32() + grpPos;
                    br.Position = grpPos + 0x48 + filePos + 8;
                    bw.Position = grpFileBlockPos + 8 + br.ReadUInt32() + 0xC;
                    br.Position = bw.Position;
                    uint fileSize = br.ReadUInt32();
                    bw.Position -= 0xC;
                    bw.Position = origPos;

                    //Close reference.
                    //w.CloseSizedReference(bw, ReferenceTypes.General, (int)(bw.Position - fileBlockPos - 8), fileSize, "File" + f.FileId);

                    //Close rescources.
                    br.Dispose();
                    src.Dispose();
                    File.Delete(pathToWriteTo + "CITRIC_TMP.CCT");

                }

            }*/

            //Close the file block.
            w.CloseBlock(bw);

            #endregion

            //End the file.
            w.CloseFile(bw);

            //Free memory.
            bw.Dispose();

        }

        /// <summary>
        /// Write a sound archive files to a folder.
        /// </summary>
        /// <param name="a">The sound archive.</param>
        /// <param name="archiveFolder">Folder containing the sound archive.</param>
        /// <param name="pathToWriteTo">Path to write to.</param>
        public static void WriteSoundArchiveToFolder(SoundArchive a, string archiveFolder, string pathToWriteTo) {

            //Create folders.
            Directory.CreateDirectory(pathToWriteTo + "/Sequences");
            Directory.CreateDirectory(pathToWriteTo + "/Banks");
            Directory.CreateDirectory(pathToWriteTo + "/Wave Archives");
            Directory.CreateDirectory(pathToWriteTo + "/Wave Sound Datas");
            Directory.CreateDirectory(pathToWriteTo + "/Streams");
            Directory.CreateDirectory(pathToWriteTo + "/Stream Prefetches");

            //Write each file.
            int fCount = 0;
            foreach (var f in a.Files) {

                //File to write.
                byte[] fileData = null;

                //File name.
                string fileName = "Unknown Name";
                if (f.FileName != null) {
                    fileName = f.FileName;
                }

                //Internal file.
                if (f.File != null) {
                    fileData = WriteFile(f.File);
                }

                //External file.
                else if (f.ExternalFileName != null) {
                    fileData = File.ReadAllBytes(archiveFolder + "/" + f.ExternalFileName);
                }

                //Get file type.
                string type = "";
                string rawType = f.FileExtension.Substring(f.FileExtension.Length - 3, 3);
                if (rawType.Equals("seq")) {
                    type = "Sequences";
                } else if (rawType.Equals("bnk")) {
                    type = "Banks";
                } else if (rawType.Equals("war")) {
                    type = "Wave Archives";
                } else if (rawType.Equals("wsd")) {
                    type = "Wave Sound Datas";
                } else if (rawType.Equals("stm")) {
                    type = "Streams";
                } else if (rawType.Equals("stp")) {
                    type = "Prefetches";
                }

                //Write the file.
                if (fileData == null) {
                    File.WriteAllBytes(pathToWriteTo + "/" + type + "/" + fCount.ToString("D5") + " - (Null Data) " + fileName + "." + f.FileExtension, new byte[0]);
                }

                //Wave archive.
                else if (rawType.Equals("war")) {

                    //Create the wave archive folder.
                    string warDir = pathToWriteTo + "/Wave Archives/" + fCount.ToString("D5") + " - " + fileName;
                    Directory.CreateDirectory(warDir);

                    //Write each wave.
                    int wavCount = 0;
                    foreach (var w in f.File as SoundWaveArchive) {
                        if (w.Name != null) {
                            File.WriteAllBytes(warDir + "/" + "Wave " + wavCount.ToString("D5") + " - " + w.Name + "." + w.GetExtension(), WriteFile(w));
                        } else {
                            File.WriteAllBytes(warDir + "/" + "Wave " + wavCount.ToString("D5") + " - " + "Unknown Name" + "." + w.GetExtension(), WriteFile(w));
                        }
                        wavCount++;
                    }

                }

                //Regular file.
                else if (!type.Equals("")) {
                    File.WriteAllBytes(pathToWriteTo + "/" + type + "/" + fCount.ToString("D5") + " - " + fileName + "." + f.FileExtension, fileData);
                }

                //Increment count.
                fCount++;

            }

        }

        /// <summary>
        /// Write the symbols.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="pathToWriteTo">Location of text file to write.</param>
        public static void ExportSymbols(SoundArchive a, string pathToWriteTo) {

            //Make txt file.
            List<string> txt = new List<string>();

            //Write sounds.
            int soundCount = a.Sequences.Count + a.WaveSoundDatas.Count + a.Streams.Count;
            txt.Add("Sounds:");
            for (int i = 0; i < soundCount; i++) {

                //Streams.
                if (i < a.Streams.Count) {

                    //Write name.
                    if (a.Streams[i] != null) {
                        txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.Streams[i].Name));
                    }

                }

                //Wave sound datas.
                else if (i < a.Streams.Count + a.WaveSoundDatas.Count) {

                    //Write name.
                    if (a.WaveSoundDatas[i - a.Streams.Count] != null) {
                        txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.WaveSoundDatas[i - a.Streams.Count].Name));
                    }

                }

                //Sequences.
                else {

                    //Write name.
                    if (a.Sequences[i - a.WaveSoundDatas.Count - a.Streams.Count] != null) {
                        txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.Sequences[i - a.WaveSoundDatas.Count - a.Streams.Count].Name));
                    }

                }

            }

            //Write sound sets.
            txt.Add("\nSound Sets:");
            for (int i = 0; i < a.SoundSets.Count; i++) {

                //Write name.
                if (a.SoundSets[i] != null) {
                    txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.SoundSets[i].Name));
                }

            }

            //Write banks.
            txt.Add("\nBanks:");
            for (int i = 0; i < a.Banks.Count; i++) {

                //Write name.
                if (a.Banks[i] != null) {
                    txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.Banks[i].Name));
                }

            }

            //Write wave archives.
            txt.Add("\nWave Archives:");
            for (int i = 0; i < a.WaveArchives.Count; i++) {

                //Write name.
                if (a.WaveArchives[i] != null) {

                    //Write name.
                    txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.WaveArchives[i].Name));

                    //Write wave names.
                    if (a.WaveArchives[i].File != null) {
                        for (int j = 0; j < (a.WaveArchives[i].File.File as SoundWaveArchive).Count; j++) {
                            if ((a.WaveArchives[i].File.File as SoundWaveArchive)[j] != null) {
                                txt.Add("\t\tw" + j + " - " + ExportSymbolGetSymbolName((a.WaveArchives[i].File.File as SoundWaveArchive)[j].Name));
                            }
                        }
                    }

                }

            }

            //Write groups.
            txt.Add("\nGroups:");
            for (int i = 0; i < a.Groups.Count; i++) {

                //Write name.
                if (a.Groups[i] != null) {
                    txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.Groups[i].Name));
                }

            }

            //Write players.
            txt.Add("\nPlayers:");
            for (int i = 0; i < a.Players.Count; i++) {

                //Write name.
                if (a.Players[i] != null) {
                    txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.Players[i].Name));
                }

            }

            //Write files.
            txt.Add("\nFiles:");
            for (int i = 0; i < a.Files.Count; i++) {

                //Write name.
                if (a.Files[i] != null) {
                    txt.Add("\t" + i + " - " + ExportSymbolGetSymbolName(a.Files[i].FileName));
                }

            }

            //Write text file.
            File.WriteAllLines(pathToWriteTo, txt);

        }

        /// <summary>
        /// Get symbol name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Another name.</returns>
        private static string ExportSymbolGetSymbolName(string name) {
            if (name == null) {
                return "%NULLNAME%";
            } else {
                return name;
            }
        }

        /// <summary>
        /// Convert file to effect.
        /// </summary>
        /// <returns>The file.</returns>
        public static byte[] WriteFile(ISoundFile f) {
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            f.Write(bw);
            byte[] ret = o.ToArray();
            bw.Dispose();
            return ret;
        }

    }

}
