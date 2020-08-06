using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sound archive reader.
    /// </summary>
    public static class SoundArchiveReader {

        /// <summary>
        /// Read a sound archive.
        /// </summary>
        /// <param name="b">Sound archive to read.</param>
        /// <returns>A new sound archive.</returns>
        public static SoundArchive ReadArchive(b_sar b) {

            //Create new archive.
            SoundArchive a = new SoundArchive();

            //Do the project info first.
            a.MaxSequences = b.info.projectInfo.maxSeq;
            a.MaxSequenceTracks = b.info.projectInfo.maxSeqTracks;
            a.MaxStreamChannels = b.info.projectInfo.maxStreamChannels;
            a.MaxStreamSounds = b.info.projectInfo.maxStreamSounds;
            a.MaxStreamTracks = b.info.projectInfo.maxStreamTracks;
            a.MaxWaveSounds = b.info.projectInfo.maxWaveSounds;
            a.MaxWaveTracks = b.info.projectInfo.maxWaveTracks;
            a.Options = b.info.projectInfo.options;
            a.StreamBufferTimes = b.info.projectInfo.streamBufferTimes;

            //Players.
            foreach (var p in b.info.players) {
                PlayerEntry e = new PlayerEntry();
                e.Name = GetString(p.flags, b.strg);
                e.PlayerHeapSize = (int)GetFlagValue(p.flags, 1);
                e.SoundLimit = (int)p.playableSoundLimit;
                a.Players.Add(e);
            }

            //Sound set.
            foreach (var s in b.info.soundGroups) {
                SoundSetEntry e = new SoundSetEntry();
                e.Name = GetString(s.flags, b.strg);
                e.StartIndex = (int)s.firstId.index;
                e.EndIndex = (int)s.lastId.index;
                e.SoundType = GetType(s.firstId.type);
                //try { e.WaveArchives.AddRange(s.waveSoundGroupTable.waveArchiveIdTable.entries.Select(x => (int)x.index)); } catch { }
                a.SoundSets.Add(e);
            }

            //Files.
            int fileNum = 0;
            foreach (var f in b.info.files) {
                SoundFile<ISoundFile> e = new SoundFile<ISoundFile>();
                if (f.externalFileName != null) {
                    e.ExternalFileName = f.externalFileName;
                    //e.IsExternal = true;
                } else {
                    try {
                        e.File = ReadFile(f.file);
                    } catch { }
                }
                try {
                    e.Groups.AddRange(f.internalFileGroupTable.entries.Select(x => (int)x));
                } catch { }
                e.FileId = fileNum;
                fileNum++;
                a.Files.Add(e);
            }

            //Wave archives.
            foreach (var w in b.info.wars) {
                WaveArchiveEntry e = new WaveArchiveEntry();
                e.Name = GetString(w.flags, b.strg);
                e.LoadIndividually = w.loadIndividual != 0;
                e.File = new SoundFile<ISoundFile>() { Reference = a.Files[(int)w.fileId] };
                e.File.FileName = e.Name + ".b" + FileWriter.GetWriteModeChar(a.WriteMode) + "war";
                a.Files[(int)w.fileId].ReferencedBy.Add(e.File);
                a.WaveArchives.Add(e);
            }

            //Banks.
            foreach (var k in b.info.banks) {
                BankEntry e = new BankEntry();
                e.Name = GetString(k.flags, b.strg);
                try {
                    //e.WaveArchives.AddRange(k.waveArchiveItemIdTable.entries.Select(x => (int)x.index));
                } catch { }
                e.File = new SoundFile<ISoundFile>() { Reference = a.Files[(int)k.fileId] };
                a.Files[(int)k.fileId].ReferencedBy.Add(e.File);
                a.Banks.Add(e);
            }

            //Groups.
            foreach (var g in b.info.groups) {
                GroupEntry e = new GroupEntry();
                e.Name = GetString(g.flags, b.strg);
                e.File = new SoundFile<ISoundFile>() { Reference = a.Files[(int)g.fileId] };
                a.Files[(int)g.fileId].ReferencedBy.Add(e.File);
                a.Groups.Add(e);
            }

            //Sounds.
            foreach (var s in b.info.sounds) {

                //Get sound info. TODO.
                SoundInfo i = new SoundInfo();

                //Sequence.
                if (s.sequenceInfo != null) {
                    
                }
                
                //Wave sound data.
                else if (s.waveInfo != null) {
                    WaveSoundDataEntry e = new WaveSoundDataEntry();
                    //e.SoundInfo = i;
                    //TODO.
                }
                
                //Stream info.
                else if (s.streamInfo != null) {

                }

            }

            //Return the new archive.
            return a;

        }

        /// <summary>
        /// Read a sound archive file.
        /// </summary>
        /// <param name="b">File to read.</param>
        /// <returns>The sound archive.</returns>
        public static SoundArchive ReadSoundArchive(string filePath) {

            //New sound archive.
            SoundArchive a = new SoundArchive();

            //Prepare the reader.
            FileStream src = new FileStream(filePath, FileMode.Open);
            BinaryDataReader br = new BinaryDataReader(src);

            //Open the file.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out a.WriteMode, out a.Version);

            //Strings.
            List<string> strg = new List<string>();

            //String block reading.
            #region StrgBlock

            //Open string block if exists.
            if (!FileReader.IsBlockNull(0)) {

                //Create strings.
                a.CreateStrings = true;

                //Open the string block.
                FileReader.OpenBlock(br, 0);

                //All I care about is the reference to the string table.
                FileReader.OpenReference(br, "StringTableRef");

                //If table is not null.
                if (!FileReader.ReferenceIsNull("StringTableRef")) {

                    //Jump to it.
                    FileReader.JumpToReference(br, "StringTableRef");

                    //Start structure.
                    FileReader.StartStructure(br);
                    
                    //Open the sized reference table.
                    FileReader.OpenSizedReferenceTable(br, "StringTable");

                    //Read each reference.
                    for (int i = 0; i < FileReader.SizedReferenceTableCount("StringTable"); i++) {

                        //If null.
                        if (FileReader.SizedReferenceTableReferenceIsNull(i, "StringTable")) {
                            strg.Add(null);
                        }
                        
                        //Read string.
                        else {

                            //Jump to position.
                            FileReader.SizedReferenceTableJumpToReference(br, i, "StringTable");

                            //Read the string, which is the size minus one.
                            strg.Add(new string(br.ReadChars((int)FileReader.SizedReferenceTableGetSize(i, "StringTable") - 1)));

                        }

                    }

                    //Close the sized reference table.
                    FileReader.CloseSizedReferenceTable("StringTable");

                    //End structure.
                    FileReader.EndStructure();

                }

                //Close reference.
                FileReader.CloseReference("StringTableRef");

                //Close the block.
                FileReader.CloseBlock(br);

            }

            #endregion

            //Info block reading.
            #region InfoBlock

            //Open the info block.
            FileReader.OpenBlock(br, 1);

            //Open all the references.
            FileReader.OpenReference(br, "SoundRefTableRef");
            FileReader.OpenReference(br, "SoundGroupRefTableRef");
            FileReader.OpenReference(br, "BankRefTableRef");
            FileReader.OpenReference(br, "WaveArchiveRefTableRef");
            FileReader.OpenReference(br, "GroupRefTableRef");
            FileReader.OpenReference(br, "PlayerRefTableRef");
            FileReader.OpenReference(br, "FileRefTableRef");
            FileReader.OpenReference(br, "ProjectInfo");

            //File info. Must be loaded first.
            Dictionary<int, SizedReference> fileRefs = new Dictionary<int, SizedReference>();

            //File info.
            #region FileInfo

            //File reference table is not null.
            if (!FileReader.ReferenceIsNull("FileRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "FileRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New file data.
                a.Files = new List<SoundFile<ISoundFile>>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "SoundGroupRefTable");

                //Read each file.
                for (int i = 0; i < FileReader.ReferenceTableCount("SoundGroupRefTable"); i++) {

                    //Null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "SoundGroupRefTable")) {
                        a.Files.Add(null);
                    }

                    //Valid.
                    else {

                        //Jump to reference.
                        FileReader.ReferenceTableJumpToReference(br, i, "SoundGroupRefTable");

                        //Start structure.
                        FileReader.StartStructure(br);

                        //New file data.
                        SoundFile<ISoundFile> s = new SoundFile<ISoundFile>();

                        //Set id.
                        s.FileId = i;

                        //File data consists of a reference and flags, but the flags do nothing, so ignore them.
                        FileReader.OpenReference(br, "FileRef");

                        //File type is null by default.
                        s.FileType = EFileType.Undefined;

                        //Valid file data.
                        if (!FileReader.ReferenceIsNull("FileRef")) {

                            //Jump to reference.
                            FileReader.JumpToReference(br, "FileRef");

                            //Start structure.
                            FileReader.StartStructure(br);

                            //External file.
                            if (FileReader.ReferenceGetType("FileRef") == ReferenceTypes.SAR_Info_ExternalFile) {

                                //External name is a string.
                                s.ExternalFileName = ReadNullTerminated(br);

                                //Set file type.
                                s.FileType = EFileType.External;

                            }

                            //Internal file.
                            else if (FileReader.ReferenceGetType("FileRef") == ReferenceTypes.SAR_Info_InternalFile) {

                                //There is a sized reference to the file in the file block here.
                                fileRefs.Add(i, new SizedReference(ref br));

                                //Set file type.
                                s.FileType = EFileType.Internal;

                                //Reference to attached group table here. This does not exist for 'C' type b_sars, so I'll have to experiment to see how these are made.
                                if (a.WriteMode != WriteMode.CTR && a.WriteMode != WriteMode.C_BE) {

                                    //Open reference.
                                    FileReader.OpenReference(br, "AttGroupTableRef");

                                    //Not null ref.
                                    if (!FileReader.ReferenceIsNull("AttGroupTableRef")) {

                                        //Jump to table.
                                        FileReader.JumpToReference(br, "AttGroupTableRef");

                                        //New table.
                                        s.Groups = new List<int>();

                                        //Read the table.
                                        uint attCount = br.ReadUInt32();
                                        for (int j = 0; j < attCount; j++) {
                                            s.Groups.Add((int)new Id(ref br).index);
                                        }

                                    }

                                    //Close reference.
                                    FileReader.CloseReference("AttGroupTableRef");

                                }

                            }

                            //End structure.
                            FileReader.EndStructure();

                        }

                        //Close the reference.
                        FileReader.CloseReference("FileRef");

                        //Add the sound file.
                        a.Files.Add(s);

                        //End structure.
                        FileReader.EndStructure();

                    }

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("SoundGroupRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Wave archive info.
            #region WaveArchiveInfo

            //Wave archive reference table is not null.
            if (!FileReader.ReferenceIsNull("WaveArchiveRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "WaveArchiveRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New wave archive info.
                a.WaveArchives = new List<WaveArchiveEntry>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "WaveArchiveRefTable");

                //Read each wave archive.
                for (int i = 0; i < FileReader.ReferenceTableCount("WaveArchiveRefTable"); i++) {

                    //Null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "WaveArchiveRefTable")) {
                        a.WaveArchives.Add(null);
                    }

                    //Valid.
                    else {

                        //Jump to the wave archive entry.
                        FileReader.ReferenceTableJumpToReference(br, i, "WaveArchiveRefTable");

                        //New wave archive entry.
                        WaveArchiveEntry w = new WaveArchiveEntry();

                        //Get file id.
                        int fileId = (int)br.ReadUInt32();

                        //File is not null.
                        if (fileId != -1) {

                            //Safe check.
                            if (a.Files[fileId] == null) {
                                a.Files[fileId] = new SoundFile<ISoundFile>();
                            }

                            //Link the files.
                            w.File = new SoundFile<ISoundFile>() { Reference = a.Files[fileId] };
                            a.Files[fileId].ReferencedBy.Add(w.File);
                            w.File.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "war").ToLower();

                        }

                        //Load individual.
                        w.LoadIndividually = br.ReadBoolean();
                        br.ReadBytes(3);

                        //Flags. F0 = String Index, F1 = Wave Count.
                        FlagParameters f = new FlagParameters(ref br);
                        w.Name = GetString(f, strg, "WARC", i, true);
                        if (w.File != null) {
                            if (w.File.FileName == null) {
                                w.File.FileName = w.Name;
                            }
                        }
                        if (f.isFlagEnabled[1]) {
                            w.IncludeWaveCount = true;
                        }

                        //Wave count is negligible, so ignore it, but add wave archive.
                        a.WaveArchives.Add(w);

                    }

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("WaveArchiveRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Bank info.
            #region BankInfo

            //Bank reference table is not null.
            if (!FileReader.ReferenceIsNull("BankRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "BankRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New bank info.
                a.Banks = new List<BankEntry>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "BankRefTable");

                //Read each bank.
                for (int i = 0; i < FileReader.ReferenceTableCount("BankRefTable"); i++) {

                    //Null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "BankRefTable")) {
                        a.Banks.Add(null);
                    }

                    //Valid.
                    else {

                        //Jump to the bank entry.
                        FileReader.ReferenceTableJumpToReference(br, i, "BankRefTable");

                        //Start structure.
                        FileReader.StartStructure(br);

                        //New bank entry.
                        BankEntry e = new BankEntry();

                        //Get file id.
                        int fileId = (int)br.ReadUInt32();

                        //File is not null.
                        if (fileId != -1) {

                            //Safe check.
                            if (a.Files[fileId] == null) {
                                a.Files[fileId] = new SoundFile<ISoundFile>();
                            }

                            //Link the files.
                            e.File = new SoundFile<ISoundFile>() { Reference = a.Files[fileId] };
                            a.Files[fileId].ReferencedBy.Add(e.File);
                            e.File.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "bnk").ToLower();

                        }

                        //Reference to wave archive table.
                        FileReader.OpenReference(br, "WarTableRef");

                        //Flags. F0 = String Index.
                        FlagParameters f = new FlagParameters(ref br);
                        e.Name = GetString(f, strg, "BANK", i);
                        if (e.File != null) {
                            if (e.File.FileName == null) {
                                e.File.FileName = e.Name;
                            }
                        }

                        //If WAR table is not null.
                        if (!FileReader.ReferenceIsNull("WarTableRef")) {

                            //Jump to the table.
                            FileReader.JumpToReference(br, "WarTableRef");

                            //Init table.
                            e.WaveArchives = new List<WaveArchiveEntry>();

                            //Count.
                            uint warCount = br.ReadUInt32();

                            //Add each wave archive.
                            for (int j = 0; j < warCount; j++) {
                                e.WaveArchives.Add(a.WaveArchives[(int)new Id(ref br).index]);
                            }

                        }

                        //Close the wave archive table.
                        FileReader.CloseReference("WarTableRef");

                        //Add the bank.
                        a.Banks.Add(e);

                        //End structure.
                        FileReader.EndStructure();

                    }

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("WaveArchiveRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Group info.
            #region GroupInfo

            //Group reference table is not null.
            if (!FileReader.ReferenceIsNull("GroupRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "GroupRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New group info.
                a.Groups = new List<GroupEntry>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "GroupRefTable");

                //Read each group.
                for (int i = 0; i < FileReader.ReferenceTableCount("GroupRefTable"); i++) {

                    //Null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "GroupRefTable")) {
                        a.Groups.Add(null);
                    }

                    //Valid.
                    else {

                        //Jump to the group entry.
                        FileReader.ReferenceTableJumpToReference(br, i, "GroupRefTable");

                        //New wave archive entry.
                        GroupEntry g = new GroupEntry();

                        //Get file id.
                        int fileId = (int)br.ReadUInt32();

                        //File is not null.
                        if (fileId != -1) {

                            //Safe check.
                            if (a.Files[fileId] == null) {
                                a.Files[fileId] = new SoundFile<ISoundFile>();
                            }

                            //Link the files.
                            g.File = new SoundFile<ISoundFile>() { Reference = a.Files[fileId] };
                            a.Files[fileId].ReferencedBy.Add(g.File);
                            g.File.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "grp").ToLower();

                        }

                        //Flags. F0 = String Index.
                        FlagParameters f = new FlagParameters(ref br);
                        g.Name = GetString(f, strg, "GROUP", i);
                        if (g.File != null) {
                            if (g.File.FileName == null) {
                                g.File.FileName = g.Name;
                            }
                        }

                        //Add group.
                        a.Groups.Add(g);

                    }                 

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("GroupRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Player info.
            #region PlayerInfo

            //Player reference table is not null.
            if (!FileReader.ReferenceIsNull("PlayerRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "PlayerRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New player info.
                a.Players = new List<PlayerEntry>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "PlayerRefTable");

                //Read each player.
                for (int i = 0; i < FileReader.ReferenceTableCount("PlayerRefTable"); i++) {

                    //Null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "PlayerRefTable")) {
                        a.Players.Add(null);
                    }

                    //Valid.
                    else {

                        //Jump to the player entry.
                        FileReader.ReferenceTableJumpToReference(br, i, "PlayerRefTable");

                        //New player entry.
                        PlayerEntry p = new PlayerEntry();

                        //Get max.
                        p.SoundLimit = (int)br.ReadUInt32();

                        //Flags. F0 = String Index, F1 = Player Heap Size.
                        FlagParameters f = new FlagParameters(ref br);
                        p.Name = GetString(f, strg, "PLAYER", i);
                        p.PlayerHeapSize = (int)GetFlagValue(f, 1);
                        if (f.isFlagEnabled[1]) {
                            p.IncludeHeapSize = true;
                        }

                        //Add player.
                        a.Players.Add(p);

                    }

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("PlayerRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Sound info.
            #region SoundInfo

            //Sound reference table is not null.
            if (!FileReader.ReferenceIsNull("SoundRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "SoundRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New sound info.
                a.Sequences = new List<SequenceEntry>();
                a.Streams = new List<StreamEntry>();
                a.WaveSoundDatas = new List<WaveSoundDataEntry>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "SoundRefTable");

                //Read each sound.
                for (int i = 0; i < FileReader.ReferenceTableCount("SoundRefTable"); i++) {

                    //If Nintendo managed to fuck up so badly that there is a null index in the sound table, then this won't work out well...
                    if (FileReader.ReferenceTableReferenceIsNull(i, "SoundRefTable")) {
                        //Hope that this never happens, which it shouldn't, due to the nature of sound archives.
                    }

                    //Valid.
                    else {

                        //Jump to the sound entry.
                        FileReader.ReferenceTableJumpToReference(br, i, "SoundRefTable");

                        //New sound info.
                        SoundInfo s = new SoundInfo();

                        //Start structure.
                        FileReader.StartStructure(br);

                        //Get file id.
                        int fileId = (int)br.ReadUInt32();

                        //File is not null.
                        if (fileId != -1) {

                            //Safe check.
                            if (a.Files[fileId] == null) {
                                a.Files[fileId] = new SoundFile<ISoundFile>();
                            }

                            //Link the files.
                            s.File = new SoundFile<ISoundFile>() { Reference = a.Files[fileId] };
                            a.Files[fileId].ReferencedBy.Add(s.File);

                        }

                        //Player id.
                        try {
                            s.Player = a.Players[(int)new Id(ref br).index];
                        } catch { }

                        //Volume.
                        s.Volume = br.ReadByte();

                        //Remote filter.
                        s.RemoteFilter = (sbyte)br.ReadByte();

                        //Padding.
                        br.ReadUInt16();

                        //Reference to the detailed info.
                        FileReader.OpenReference(br, "DetSoundRef");

                        /* Flags List:
                         * F0 = String Index.
                         * F1 = Pan Stuff.  0x0000XXYY, XX = Pan Curve, YY = Pan Mode.
                         * F2 = Player Stuff.   0x0000XXYY, XX = Actor Player Id, YY = Player Priority.
                         * F8 = 3D Info Offset.
                         * F17 = Is Front Bypass.
                         * F28 = User Param 3.
                         * F29 = User Param 2.
                         * F30 = User Param 1.
                         * F31 = User Param 0.
                         */
                        FlagParameters f = new FlagParameters(ref br);

                        //Get the name.
                        s.Name = GetString(f, strg, "SOUND", i);
                        if (s.File != null) {
                            if (s.File.FileName == null) {
                                s.File.FileName = s.Name;
                            }
                        }

                        //Get pan info.
                        if (f.isFlagEnabled[1]) {

                            //Pan mode.
                            s.PanMode = (SoundInfo.EPanMode)(f.flagValues[1] & 0xFF);

                            //Pan curve.
                            s.PanCurve = (SoundInfo.EPanCurve)((f.flagValues[1] & 0xFF00) >> 8);

                        }

                        //Get player info.
                        if (f.isFlagEnabled[2]) {

                            //Player priority.
                            s.PlayerPriority = (sbyte)(f.flagValues[2] & 0xFF);

                            //Player actor id.
                            s.PlayerActorId = (sbyte)((f.flagValues[2] & 0xFF00) >> 8);

                        }

                        //Is front bypass.
                        s.IsFrontBypass = GetFlagValue(f, 17) > 0;

                        //User parameter 0.
                        if (f.isFlagEnabled[31]) {

                            //Set values.
                            s.UserParamsEnabled[0] = true;
                            s.UserParameter[0] = f.flagValues[31];

                        }

                        //User parameter 1.
                        if (f.isFlagEnabled[30]) {

                            //Set values.
                            s.UserParamsEnabled[1] = true;
                            s.UserParameter[1] = f.flagValues[30];

                        }

                        //User parameter 2.
                        if (f.isFlagEnabled[29]) {

                            //Set values.
                            s.UserParamsEnabled[2] = true;
                            s.UserParameter[2] = f.flagValues[29];

                        }

                        //User parameter 3.
                        if (f.isFlagEnabled[28]) {

                            //Set values.
                            s.UserParamsEnabled[3] = true;
                            s.UserParameter[3] = f.flagValues[28];

                        }

                        //3D Info offset.
                        uint infoOffset = GetFlagValue(f, 8, 0xFFFFFFFF);
                        if (infoOffset != 0xFFFFFFFF) {

                            //Jump to 3d info.
                            FileReader.JumpToOffsetManually(br, (int)infoOffset);

                            //Read the 3d sound info.
                            s.Sound3dInfo = new Sound3dInfo();

                            //Flags.
                            UInt32 flags = br.ReadUInt32();

                            //3D volume.
                            s.Sound3dInfo.Volume = (flags & 0b1) > 0;

                            //3D priority.
                            s.Sound3dInfo.Priority = (flags & 0b10) > 0;

                            //3D pan.
                            s.Sound3dInfo.Pan = (flags & 0b100) > 0;

                            //3D span.
                            s.Sound3dInfo.Span = (flags & 0b1000) > 0;

                            //3D filter.
                            s.Sound3dInfo.Filter = (flags & 0b10000) > 0;

                            //Decay ratio.
                            s.Sound3dInfo.AttenuationRate = br.ReadSingle();
                            s.Sound3dInfo.AttenuationCurve = (Sound3dInfo.EAttenuationCurve)br.ReadByte();
                            s.Sound3dInfo.DopplerFactor = br.ReadByte();

                            //There is some padding and optional filter here, but don't bother, as it's useless.
                            br.ReadUInt16();
                            br.ReadUInt32();

                            //Some flag for 'F' type?
                            if (FileWriter.GetWriteModeChar(a.WriteMode) == 'F') {
                                s.Sound3dInfo.UnknownFlag = br.ReadUInt32() > 0;
                            }

                        }

                        //Detailed sound reference is not null. There should never be a case where it is null.
                        if (!FileReader.ReferenceIsNull("DetSoundRef")) {

                            //Jump to offset.
                            FileReader.JumpToReference(br, "DetSoundRef");

                            //Start structure.
                            FileReader.StartStructure(br);

                            //Switch the type of content.
                            switch (FileReader.ReferenceGetType("DetSoundRef")) {

                                //Wave Sound Data.
                                case ReferenceTypes.SAR_Info_WaveSound:

                                    //New wave sound data.
                                    WaveSoundDataEntry w = new WaveSoundDataEntry();
                                    s.File.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "wsd").ToLower();

                                    //Set info.
                                    w.LoadSoundInfo(s);

                                    //Index.
                                    w.WaveIndex = (int)br.ReadUInt32();

                                    //Allocate track amount.
                                    w.AllocateTrackCount = (int)br.ReadUInt32();

                                    //Get flags. F0 = 0x0000RRCC    R = Is Release Priority, C = Channel Priority.
                                    FlagParameters fl = new FlagParameters(ref br);
                                    if (fl.isFlagEnabled[0]) {

                                        //Channel priority.
                                        w.ChannelPriority = (byte)(fl.flagValues[0] & 0xFF);

                                        //Is release priority.
                                        w.IsReleasePriority = ((fl.flagValues[0] & 0xFF00) >> 8) > 0;

                                    }

                                    //Add the wave sound data.
                                    a.WaveSoundDatas.Add(w);
                                    break;

                                //Sequence.
                                case ReferenceTypes.SAR_Info_SequenceSound:

                                    //New sequence.
                                    SequenceEntry e = new SequenceEntry();
                                    s.File.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "seq").ToLower();

                                    //Set info.
                                    e.LoadSoundInfo(s);

                                    //Reference to bank id table.
                                    FileReader.OpenReference(br, "BnkTableRef");

                                    //Allocate track flags.
                                    e.SetFlags(br.ReadUInt32());

                                    //Get flags. F0 = Start Offset. F1 = 0x0000RRCC    R = Is Release Priority, C = Channel Priority.
                                    FlagParameters fla = new FlagParameters(ref br);
                                    if (fla.isFlagEnabled[1]) {

                                        //Channel priority.
                                        e.ChannelPriority = (byte)(fla.flagValues[1] & 0xFF);

                                        //Is release priority.
                                        e.IsReleasePriority = ((fla.flagValues[1] & 0xFF00) >> 8) > 0;

                                    }

                                    //Start offset.
                                    e.StartOffset = GetFlagValue(fla, 0, 0);

                                    //Bank id table is not null.
                                    if (!FileReader.ReferenceIsNull("BnkTableRef")) {

                                        //New table.
                                        e.Banks = new BankEntry[4];

                                        //Jump to position.
                                        FileReader.JumpToReference(br, "BnkTableRef");

                                        //Read table.
                                        uint bnkCount = br.ReadUInt32();
                                        for (int j = 0; j < bnkCount; j++) {
                                            uint index = new Id(ref br).index;
                                            if (index != 0xFFFFFF) {
                                                e.Banks[j] = a.Banks[(int)index];
                                            }
                                        }

                                    }

                                    //Close bank id table.
                                    FileReader.CloseReference("BnkTableRef");

                                    //Add the sequence.
                                    a.Sequences.Add(e);
                                    break;

                                //Stream.
                                case ReferenceTypes.SAR_Info_StreamSound:

                                    //New stream.
                                    StreamEntry m = new StreamEntry();
                                    s.File.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "stm").ToLower();

                                    //Set info.
                                    m.LoadSoundInfo(s);

                                    //Allocate track flags.
                                    m.SetFlags(br.ReadUInt16());

                                    //Allocate channel tracks.
                                    m.AllocateChannelCount = br.ReadUInt16();

                                    //Send and filter are not available.
                                    if (!SoundArchiveVersions.SupportsExtraStreamInfo(a)) {

                                        //Set properties.
                                        m.Pitch = 1f;
                                        m.SendValue = new byte[] { 127, 0, 0, 0 };
                                        m.Tracks = null;

                                        //Only exists in C type.
                                        if (FileWriter.GetWriteModeChar(a.WriteMode) == 'C') {

                                            //Option parameters. F0 = Type, F1 = Loop start frame, F2 = Loop end frame.
                                            FlagParameters option = new FlagParameters(ref br);

                                            //Get the stream file type.
                                            m.StreamFileType = (StreamEntry.EStreamFileType)GetFlagValue(option, 0, 1);

                                            //Loop start frame.
                                            m.LoopStartFrame = GetFlagValue(option, 1);

                                            //Loop end frame.
                                            m.LoopEndFrame = GetFlagValue(option, 2);

                                        }

                                    }

                                    //Send and filter exist.
                                    else {

                                        //Track info table.
                                        FileReader.OpenReference(br, "TrackInfoTableRef");

                                        //Pitch.
                                        m.Pitch = br.ReadSingle();

                                        //Reference to send value.
                                        FileReader.OpenReference(br, "SendValueRef");

                                        //Reference to stream sound extension.
                                        FileReader.OpenReference(br, "StreamSoundExtensionRef");

                                        //Prefetch file.
                                        if (SoundArchiveVersions.SupportsPrefetchInfo(a)) {
                                            uint prefetchIndex = br.ReadUInt32();
                                            if (prefetchIndex != 0xFFFFFFFF) {
                                                m.PrefetchFile = new SoundFile<ISoundFile>() { Reference = a.Files[(int)prefetchIndex] };
                                                a.Files[(int)prefetchIndex].ReferencedBy.Add(m.PrefetchFile);
                                                m.PrefetchFile.FileName = m.Name;
                                                m.PrefetchFile.BackupExtension = ("b" + FileWriter.GetWriteModeChar(a.WriteMode) + "stp").ToLower();
                                                m.GeneratePrefetchFile = true;
                                            }
                                            
                                        }

                                        //Track info table.
                                        if (!FileReader.ReferenceIsNull("TrackInfoTableRef")) {

                                            //Jump to reference.
                                            FileReader.JumpToReference(br, "TrackInfoTableRef");

                                            //Start structure.
                                            FileReader.StartStructure(br);

                                            //Read track table.
                                            m.Tracks = new List<StreamTrackInfo>();
                                            FileReader.OpenReferenceTable(br, "TrackInfoTable");
                                            for (int j = 0; j < FileReader.ReferenceTableCount("TrackInfoTable"); j++) {

                                                //Track is null.
                                                if (FileReader.ReferenceTableReferenceIsNull(j, "TrackInfoTable")) {
                                                    m.Tracks.Add(null);
                                                }

                                                //Track is valid.
                                                else {

                                                    //Jump to track.
                                                    FileReader.ReferenceTableJumpToReference(br, j, "TrackInfoTable");

                                                    //Start structure.
                                                    FileReader.StartStructure(br);

                                                    //New track.
                                                    StreamTrackInfo t = new StreamTrackInfo();

                                                    //Track properties.
                                                    t.Volume = br.ReadByte();
                                                    t.Pan = br.ReadSByte();
                                                    t.Span = br.ReadSByte();
                                                    t.SurroundMode = br.ReadBoolean();

                                                    //References.
                                                    FileReader.OpenReference(br, "GlobalChannelTableRef");
                                                    FileReader.OpenReference(br, "SendValue2Ref");

                                                    //More properties.
                                                    t.LpfFrequency = br.ReadSByte();
                                                    t.BiquadType = br.ReadSByte();
                                                    t.BiquadValue = br.ReadSByte();
                                                    br.ReadByte();

                                                    //Global channel table reference.
                                                    if (FileReader.ReferenceIsNull("GlobalChannelTableRef")) {
                                                        t.Channels = null;
                                                    } else {
                                                        FileReader.JumpToReference(br, "GlobalChannelTableRef");
                                                        t.Channels = new List<byte>();
                                                        uint count = br.ReadUInt32();
                                                        for (int k = 0; k < count; k++) {
                                                            t.Channels.Add(br.ReadByte());
                                                        }
                                                    }

                                                    //Send value.
                                                    if (FileReader.ReferenceIsNull("SendValue2Ref")) {
                                                        t.SendValue = null;
                                                    } else {
                                                        FileReader.JumpToReference(br, "SendValue2Ref");
                                                        t.SendValue = br.ReadBytes(4);
                                                    }

                                                    //Close references.
                                                    FileReader.CloseReference("GlobalChannelTableRef");
                                                    FileReader.CloseReference("SendValue2Ref");

                                                    //Add track.
                                                    m.Tracks.Add(t);

                                                    //End structure.
                                                    FileReader.EndStructure();

                                                }

                                            }

                                            //Close reference table.
                                            FileReader.CloseReferenceTable("TrackInfoTable");

                                            //End structure.
                                            FileReader.EndStructure();

                                        } else {
                                            m.Tracks = null;
                                        }

                                        //Send value.
                                        if (FileReader.ReferenceIsNull("SendValueRef")) {
                                            m.SendValue = null;
                                        } else {
                                            FileReader.JumpToReference(br, "SendValueRef");
                                            m.SendValue = br.ReadBytes(4);
                                        }

                                        //Sound extension.
                                        if (FileReader.ReferenceIsNull("StreamSoundExtensionRef")) {
                                            m.SoundExtensionIncluded = false;
                                        } else {
                                            FileReader.JumpToReference(br, "StreamSoundExtensionRef");
                                            m.SoundExtensionIncluded = true;

                                            // FileType = static_cast<SoundArchive::StreamFileType>( Util::DevideBy8bit( streamTypeInfo, 0 ) );
                                            m.StreamFileType = (StreamEntry.EStreamFileType)(br.ReadUInt32() & 0xff);
                                            if (m.StreamFileType > StreamEntry.EStreamFileType.Max) {
                                                throw new IndexOutOfRangeException();
                                            }

                                            // TODO: IsLoop = Util::DevideBy8bit( streamTypeInfo, 1 ) == 1;
                                            // TODO: Unknown = Util::DevideBy8bit( streamTypeInfo, 2 );

                                            m.LoopStartFrame = br.ReadUInt32();
                                            m.LoopEndFrame = br.ReadUInt32();
                                        }

                                        //Close track info table reference.
                                        FileReader.CloseReference("TrackInfoTableRef");

                                        //Close send value reference.
                                        FileReader.CloseReference("SendValueRef");

                                        //Close stream sound extension reference.
                                        FileReader.CloseReference("StreamSoundExtensionRef");

                                    }

                                    //Add stream.
                                    a.Streams.Add(m);
                                    break;

                            }

                            //End structure.
                            FileReader.EndStructure();

                        }

                        //Close the detailed info reference.
                        FileReader.CloseReference("DetSoundRef");

                        //End structure.
                        FileReader.EndStructure();

                    }

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("SoundRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Sound group info.
            #region SoundGroupInfo

            //Sound group reference table is not null.
            if (!FileReader.ReferenceIsNull("SoundGroupRefTableRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "SoundGroupRefTableRef");

                //Start structure.
                FileReader.StartStructure(br);

                //New sound group info.
                a.SoundSets = new List<SoundSetEntry>();

                //Open the reference table.
                FileReader.OpenReferenceTable(br, "SoundGroupRefTable");

                //Read each sound group.
                for (int i = 0; i < FileReader.ReferenceTableCount("SoundGroupRefTable"); i++) {

                    //Null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "SoundGroupRefTable")) {
                        a.SoundSets.Add(null);
                    }

                    //Valid.
                    else {

                        //Jump to the sound group entry.
                        FileReader.ReferenceTableJumpToReference(br, i, "SoundGroupRefTable");

                        //Start new structure.
                        FileReader.StartStructure(br);

                        //New sound group entry.
                        SoundSetEntry s = new SoundSetEntry();

                        //Get start and end id.
                        Id startId = new Id(ref br);
                        Id endId = new Id(ref br);

                        //Get start and end index.
                        s.StartIndex = (int)startId.index;
                        s.EndIndex = (int)endId.index;

                        //Get sound type.
                        s.SoundType = GetType(startId.type);

                        //There is a reference to a file id table here.
                        FileReader.OpenReference(br, "FileTableRef");

                        //Wave archive table.
                        FileReader.OpenReference(br, "WarRef");

                        //Flags. F0 = String Index.
                        FlagParameters f = new FlagParameters(ref br);
                        s.Name = GetString(f, strg, "SOUND_SET", i);

                        //If the file table is not null.
                        if (!FileReader.ReferenceIsNull("FileTableRef")) {

                            //Jump to the reference.
                            FileReader.JumpToReference(br, "FileTableRef");

                            //Read data.
                            s.Files = new List<SoundFile<ISoundFile>>();
                            uint fileCount = br.ReadUInt32();
                            for (int j = 0; j < fileCount; j++) {
                                s.Files.Add(a.Files[(int)br.ReadUInt32()]);
                            }

                        }

                        //Close the reference.
                        FileReader.CloseReference("FileTableRef");

                        //If the wave archive table is not null.
                        if (!FileReader.ReferenceIsNull("WarRef")) {

                            //Jump to reference.
                            FileReader.JumpToReference(br, "WarRef");

                            //Start structure.
                            FileReader.StartStructure(br);

                            //There is a reference to the table.
                            FileReader.OpenReference(br, "WarTableRef");

                            //If table is not null.
                            if (!FileReader.ReferenceIsNull("WarTableRef")) {

                                //Jump to table.
                                FileReader.JumpToReference(br, "WarTableRef");

                                //New wave archive list.
                                s.WaveArchives = new List<WaveArchiveEntry>();

                                //Read the wave archive table.
                                uint warCount = br.ReadUInt32();
                                for (int j = 0; j < warCount; j++) {

                                    //Get the ID.
                                    Id temp = new Id(ref br);

                                    //Add the WAR id.
                                    s.WaveArchives.Add(a.WaveArchives[(int)temp.index]);

                                }

                            }

                            //Close reference.
                            FileReader.CloseReference("WarTableRef");

                            //End structure.
                            FileReader.EndStructure();

                        }

                        //Close wave archive table ref.
                        FileReader.CloseReference("WarRef");

                        //Add sound group entry.
                        a.SoundSets.Add(s);

                        //End structure.
                        FileReader.EndStructure();

                    }

                }

                //Close the reference table.
                FileReader.CloseReferenceTable("SoundGroupRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            #endregion

            //Project info.
            #region ProjectInfo

            //If reference is not null.
            if (!FileReader.ReferenceIsNull("ProjectInfo")) {

                //Jump to reference.
                FileReader.JumpToReference(br, "ProjectInfo");

                //Read the project info.
                a.MaxSequences = br.ReadUInt16();
                a.MaxSequenceTracks = br.ReadUInt16();
                a.MaxStreamSounds = br.ReadUInt16();
                a.MaxStreamTracks = br.ReadUInt16();
                a.MaxStreamChannels = br.ReadUInt16();
                a.MaxWaveSounds = br.ReadUInt16();
                a.MaxWaveTracks = br.ReadUInt16();
                a.StreamBufferTimes = br.ReadByte();
                br.ReadByte();
                a.Options = br.ReadUInt32();

            }

            #endregion

            //Load group files that are external.
            for (int i = a.Groups.Count - 1; i >= 0; i--) {

                //Group has a null file, and the name is not null.
                if (a.Groups[i].Name != null && a.Groups[i].File == null) {

                    //Make sure the ARAS file exists.
                    if (File.Exists(Path.GetDirectoryName(filePath) + "/" + a.Groups[i].Name + ".aras")) {
                        a.Groups[i].File = new SoundFile<ISoundFile>();
                        a.Groups[i].File.FileType = EFileType.Aras;
                        a.Groups[i].File.BackupExtension = "grp";

                        //Get file ID.
                        for (int j = a.Files.Count - 1; j >= 0; j--) {
                            if (a.Files[j].FileType == EFileType.Internal && a.Files[j].File == null && a.Files[j].ReferencedBy.Count == 0) {
                                a.Groups[i].File.Reference = a.Files[j];
                                byte[] arasFile = File.ReadAllBytes(Path.GetDirectoryName(filePath) + "/" + a.Groups[i].Name + ".aras");
                                a.Groups[i].File.Aras = (SeadArchive)ReadFile(arasFile);
                                a.Groups[i].File.FileName = a.Groups[i].Name;
                                MemoryStream o2 = new MemoryStream(a.Groups[i].File.Aras[0].File);
                                BinaryDataReader br2 = new BinaryDataReader(o2);
                                Group grp = new Group();
                                grp.Read(br2, a.Files);
                                a.Groups[i].File.FileType = EFileType.Aras;
                                a.Groups[i].File.File = grp;
                                br2.Dispose();
                                break;
                            }
                        }

                    }

                }

            }

            //Close all the references.
            FileReader.CloseReference("SoundRefTableRef");
            FileReader.CloseReference("SoundGroupRefTableRef");
            FileReader.CloseReference("BankRefTableRef");
            FileReader.CloseReference("WaveArchiveRefTableRef");
            FileReader.CloseReference("GroupRefTableRef");
            FileReader.CloseReference("PlayerRefTableRef");
            FileReader.CloseReference("FileRefTableRef");
            FileReader.CloseReference("ProjectInfo");

            //Close the info block.
            FileReader.CloseBlock(br);

            #endregion

            //File block reading.
            #region FileBlock

            //Open the file block.
            FileReader.OpenBlock(br, 2);

            //Read each file.
            foreach (var k in fileRefs) {

                //If not null.
                if (k.Value != null && k.Value.offset != -1) {

                    //Jump to position.
                    FileReader.JumpToOffsetManually(br, k.Value.offset);

                    //Read file.
                    a.Files[k.Key].File = ReadFile(br.ReadBytes((int)k.Value.size));

                    //Get the group references.
                    if (a.Files[k.Key].File as Group != null) {

                        //Group.
                        Group g = a.Files[k.Key].File as Group;

                        //Get references.
                        for (int i = 0; i < g.SoundFiles.Count; i++) {

                            //Not null file.
                            if (g.SoundFiles[i] != null) {

                                //Set up null reference type if file is null.
                                if (a.Files[g.SoundFiles[i].FileId] == null) {
                                    a.Files[g.SoundFiles[i].FileId] = new SoundFile<ISoundFile>();
                                    a.Files[g.SoundFiles[i].FileId].FileId = g.SoundFiles[i].FileId;
                                    a.Files[g.SoundFiles[i].FileId].FileType = EFileType.NullReference;
                                }

                                //Set up reference.
                                if (g.SoundFiles[i].File != null) { a.Files[g.SoundFiles[i].FileId].File = g.SoundFiles[i].File; }
                                g.SoundFiles[i].Reference = a.Files[g.SoundFiles[i].FileId];
                                a.Files[g.SoundFiles[i].FileId].ReferencedBy.Add(g.SoundFiles[i]);

                            }

                        }

                    }

                }

                //Group only.
                else if (a.Files[k.Key].FileType != EFileType.Aras) {
                    a.Files[k.Key].FileType = EFileType.InGroupOnly;
                }

            }

            //Close the file block.
            FileReader.CloseBlock(br);

            #endregion

            //Close the file.
            FileReader.CloseFile(br);

            //Free memory.
            br.Dispose();
            src.Dispose();

            //Guess names.
            GuessWaveArchiveNames(a);
            GuessWaveNames(a);

            //Fix wave archive names.
            for (int i = 0; i < a.WaveArchives.Count; i++) {
                if (a.WaveArchives[i].Name == null) {
                    a.WaveArchives[i].Name = "WARC_" + i;
                }
            }

            //Return the archive.
            return a;

        }

        /// <summary>
        /// Try and guess the name of a wave archive.
        /// </summary>
        /// <param name="a">The sound archive.</param>
        public static void GuessWaveArchiveNames(SoundArchive a) {

            //Try and guess each wave archive name.
            for (int i = 0; i < a.WaveArchives.Count; i++) {
                if (a.WaveArchives[i] != null) {
                    if (a.WaveArchives[i].Name == null && a.WaveArchives[i].File != null) {
                        a.WaveArchives[i].Name = a.WaveArchives[i].File.FileName = GuessWaveArchiveName(a, i);
                    }
                }
            }

        }

        /// <summary>
        /// Guess the name of a wave archive.
        /// </summary>
        /// <param name="a">The sound archive.</param>
        /// <param name="waveArchiveNum">The wave archive number.</param>
        /// <returns>The name of the wave archive.</returns>
        private static string GuessWaveArchiveName(SoundArchive a, int waveArchiveNum) {

            //Get a default name.
            string name = null;

            //See if it is in a group, but only once. If more than one is in a group, then it doesn't reflect the group name.
            foreach (GroupEntry g in a.Groups) {

                //If the file is not null.
                int warCount = 0;
                bool found = false;
                if (g.File != null && g.Name != null) {

                    if (g.File.File != null) {

                        //See if the wave archive is in an entry.
                        foreach (var e in (g.File.File as Group).SoundFiles) {
                            if (e.FileExtension.EndsWith("war")) {
                                warCount++;
                                if (e.FileId == a.WaveArchives[waveArchiveNum].File.FileId) { found = true; name = "WARC_" + waveArchiveNum + "_GUESS_" + g.Name; }
                            }
                        }

                    }

                }

                //Make sure it is practical.
                if (warCount != 1 || !found) {
                    name = null;
                } else {
                    return name;
                }

            }

            //Wave archive is used within a bank.
            foreach (BankEntry b in a.Banks) {

                //File is not null.
                if (b.File != null && b.Name != null) {

                    //File in file is not null.
                    if (b.File.File != null) {

                        //See if it contains the wave archive.
                        foreach (var e in (b.File.File as SoundBank).Waves) {
                            if (e.WarIndex == waveArchiveNum) {
                                return "WARC_" + waveArchiveNum + "_GUESS_" + b.Name;
                            }
                        }

                    }

                }

            }

            //Last resort is using a WSD, which will be the most innacurate...
            foreach (WaveSoundDataEntry w in a.WaveSoundDatas) {

                //File is not null.
                if (w.File != null && w.Name != null) {

                    //File in file is not null.
                    if (w.File.File != null) {

                        //See if it contains the wave archive.
                        foreach (var e in (w.File.File as WaveSoundData).Waves) {
                            if (e.WarIndex == waveArchiveNum) {
                                return "WARC_" + waveArchiveNum + "_GUESS_" + w.Name;
                            }
                        }

                    }

                }

            }

            //Return the result.
            return name;

        }

        /// <summary>
        /// Try and guess the name of a wave.
        /// </summary>
        /// <param name="a">The sound archive.</param>
        public static void GuessWaveNames(SoundArchive a) {

            //For every wave archive.
            for (int i = 0; i < a.WaveArchives.Count; i++) {

                //Make sure file is valid.
                if (a.WaveArchives[i].File != null) {

                    //Really make sure it is valid.
                    if (a.WaveArchives[i].File.File != null) {

                        //For every wave.
                        for (int j = 0; j < (a.WaveArchives[i].File.File as SoundWaveArchive).Count; j++) {

                            //If the wave is not null.
                            if ((a.WaveArchives[i].File.File as SoundWaveArchive)[j] != null) {
                                if ((a.WaveArchives[i].File.File as SoundWaveArchive)[j].Name == null) {
                                    (a.WaveArchives[i].File.File as SoundWaveArchive)[j].Name = GuessWaveName(a, i, j);
                                }
                            }

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Guess the wave name. TODO: DO SEQUENCE PREDICTIONS!!!
        /// </summary>
        /// <param name="a">The sound archive.</param>
        /// <param name="warIndex">Wave archive number.</param>
        /// <param name="wavIndex">Wave number.</param>
        /// <returns>The name of the wave.</returns>
        private static string GuessWaveName(SoundArchive a, int warIndex, int wavIndex) {

            //Best source is a WSD.
            foreach (WaveSoundDataEntry w in a.WaveSoundDatas) {

                //Make sure file is not null.
                if (w.File != null && w.Name != null) {

                    //Really make sure file is not null.
                    if (w.File.File != null) {

                        //Get the entry.
                        try {
                            var e = (w.File.File as WaveSoundData).Waves[(w.File.File as WaveSoundData).DataItems[w.WaveIndex].Notes[0].WaveIndex];
                            if (e.WarIndex == warIndex && e.WaveIndex == wavIndex) {
                                return "WAV_" + wavIndex + "_GUESS_" + w.Name;
                            }
                        } catch { }

                    }

                }

            }

            //Sequence uses the wave only.
            foreach (SequenceEntry s in a.Sequences) {

                //Make sure file is not null.
                if (s.File != null && s.Name != null) {

                    //Really make sure file is not null.
                    if (s.File.File != null) {

                        //First see if banks used in the sequence actually contain the wave.
                        bool foundBank = false;
                        foreach (BankEntry b in s.Banks) {

                            //Bank is not null.
                            if (b != null) {

                                //Make sure file is not null.
                                if (b.File != null) {

                                    //Really make sure file is not null.
                                    if (b.File.File != null) {

                                        //See if wave matches.
                                        foreach (var e in (b.File.File as SoundBank).Waves) {
                                            if (e.WarIndex == warIndex && e.WaveIndex == wavIndex) {
                                                foundBank = true;
                                            }
                                        }

                                    }

                                }

                            }

                        }

                        //Bank is found, so sequence has a chance at having a matching instrument.
                        if (foundBank) {

                            //Somehow use the sequence commands to see what instrument is used, then read it from bank and check match.

                        }

                    }

                }

            }

            //Bank contains the wave as an instrument.
            foreach (BankEntry b in a.Banks) {

                //Make sure file is not null.
                if (b.File != null && b.Name != null) {

                    //Really make sure file is not null.
                    if (b.File.File != null) {

                        //See if wave matches.
                        foreach (var e in (b.File.File as SoundBank).Waves) {
                            if (e.WarIndex == warIndex && e.WaveIndex == wavIndex) {
                                return "WAV_" + wavIndex + "_GUESS_" + b.Name;
                            }
                        }

                    }

                }

            }

            //Return the wave archive name as a last resort.
            if (a.WaveArchives[warIndex].Name == null) {
                return null;
            } else {
                return "WAV_" + wavIndex + "_GUESS_" + a.WaveArchives[warIndex].Name;
            }

        }

        /// <summary>
        /// Import a sound archive files from a folder. MOVE THIS TO READER!!!
        /// </summary>
        /// <param name="a">The sound archive.</param>
        /// <param name="archiveFolder">Folder containing the sound archive.</param>
        /// <param name="pathToReadFrom">Path to write to.</param>
        public static void ImportSoundArchiveFromFolder(SoundArchive a, string archiveFolder, string pathToReadFrom) {

            //Get a list of file names.

        }

        /// <summary>
        /// Import symbols from a file.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="filePath">File to import symbols from.</param>
        public static void ImportSymbols(SoundArchive a, string filePath) {

            //Get the file strings.
            string[] txt = File.ReadAllLines(filePath);

            //Import mode.
            ImportSymbolsMode m = ImportSymbolsMode.Sound;

            //Read each line.
            foreach (string line in txt) {

                //Prepare string.
                string s = line.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");

                //Blank line.
                if (s.Equals("")) {
                    continue;
                }

                //Get mode.
                if (line.StartsWith("Sounds:")) {
                    m = ImportSymbolsMode.Sound;
                    continue;
                } else if (line.StartsWith("Sound Sets:")) {
                    m = ImportSymbolsMode.SoundSet;
                    continue;
                } else if (line.StartsWith("Banks:")) {
                    m = ImportSymbolsMode.Bank;
                    continue;
                } else if (line.StartsWith("Wave Archives:")) {
                    m = ImportSymbolsMode.WaveArchive;
                    continue;
                } else if (line.StartsWith("Groups:")) {
                    m = ImportSymbolsMode.Group;
                    continue;
                } else if (line.StartsWith("Players:")) {
                    m = ImportSymbolsMode.Player;
                    continue;
                } else if (line.StartsWith("Files:")) {
                    m = ImportSymbolsMode.File;
                    continue;
                }

                //Regular sound.
                int currWave = 0;
                if (!s.StartsWith("w")) {

                    //Get index.
                    int index = int.Parse(s.Split("-".ToCharArray())[0]);
                    string name = s.Split("-".ToCharArray())[1];
                    if (name.Equals("%NULLNAME%")) {
                        name = null;
                    }

                    //Dependent on sound mode.
                    switch (m) {

                        case ImportSymbolsMode.Sound:
                            if (index < a.Streams.Count) {
                                a.Streams[index].Name = name;
                            } else if (index < a.Streams.Count + a.WaveSoundDatas.Count) {
                                a.WaveSoundDatas[index - a.Streams.Count].Name = name;
                            } else {
                                a.Sequences[index - a.WaveSoundDatas.Count - a.Streams.Count].Name = name;
                            }
                            break;

                        case ImportSymbolsMode.SoundSet:
                            a.SoundSets[index].Name = name;
                            break;

                        case ImportSymbolsMode.Bank:
                            a.Banks[index].Name = name;
                            break;

                        case ImportSymbolsMode.WaveArchive:
                            a.WaveArchives[index].Name = name;
                            currWave = index;
                            break;

                        case ImportSymbolsMode.Group:
                            a.Groups[index].Name = name;
                            break;

                        case ImportSymbolsMode.Player:
                            a.Players[index].Name = name;
                            break;

                        case ImportSymbolsMode.File:
                            a.Files[index].FileName = name;
                            break;

                    }

                }

                //Wave.
                else {

                    //Get correct string.
                    s = s.Substring(1, s.Length - 1);

                    //Get index.
                    int index = int.Parse(s.Split(" - ".ToCharArray())[0]);
                    string name = s.Split(" - ".ToCharArray())[1];
                    if (name.Equals("%NULLNAME%")) {
                        name = null;
                    }

                    //Set name.
                    (a.WaveArchives[currWave].File.File as SoundWaveArchive)[index].Name = name;

                }

            }

        }

        /// <summary>
        /// Import symbols.
        /// </summary>
        private enum ImportSymbolsMode {
            Sound, SoundSet, Bank, WaveArchive, Group, Player, File
        }

        /// <summary>
        /// Get a string for a flag.
        /// </summary>
        /// <param name="f">Flag parameters.</param>
        /// <param name="strg">String block.</param>
        /// <returns>String to return.</returns>
        public static string GetString(FlagParameters f, b_sar.StrgBlock strg) {

            //Set up string.
            string s = null;

            //If string flag is enabled.
            if (f.isFlagEnabled[0]) {

                //Fetch the string specified by the flag.
                s = new string(strg.stringEntries[(int)f.flagValues[0]].data);

            }

            //Return the found string.
            return s;

        }

        /// <summary>
        /// Get the string.
        /// </summary>
        /// <param name="f">Flag parameters.</param>
        /// <param name="strg">Strings.</param>
        /// <param name="prefix">Item prefix.</param>
        /// <param name="index">Index of the item.</param>
        /// <param name="allowNull">Allow a null return.</param>
        /// <returns>The string.</returns>
        public static string GetString(FlagParameters f, List<string> strg, string prefix, int index, bool allowNull = false) {

            //Set up string.
            string s = prefix + "_" + index;

            //If string flag is enabled.
            if (f.isFlagEnabled[0]) {

                //Fetch the string specified by the flag.
                s = strg[(int)f.flagValues[0]];

            }

            //Return the found string.
            return allowNull ? null : s;

        }

        /// <summary>
        /// Retrieve a flag value if available.
        /// </summary>
        /// <param name="f">Flag parameters.</param>
        /// <param name="flagNumber">Number flag to check.</param>
        /// <param name="defaultValue">Default value to return if not found.</param>
        /// <returns>Flag value.</returns>
        public static UInt32 GetFlagValue(FlagParameters f, int flagNumber, UInt32 defaultValue = 0) {

            //Set up return value.
            UInt32 ret = defaultValue;

            //If flag enabled.
            if (f.isFlagEnabled[flagNumber]) {
                ret = f.flagValues[flagNumber];
            }

            //Return the value.
            return ret;
            
        }

        /// <summary>
        /// Convert a type to the enumeration type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Type enumeration.</returns>
        public static SoundType GetType(byte type) {

            //Simple cast.
            return (SoundType)type;

        }

        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="file">File to read.</param>
        /// <returns>The file.</returns>
        public static ISoundFile ReadFile(byte[] file) {

            MemoryStream src = new MemoryStream(file);
            BinaryDataReader br = new BinaryDataReader(src);
            string trueMagic = new string(br.ReadChars(4));
            string magic = trueMagic.Substring(1);
            br.BaseStream.Position = 0;
            ISoundFile ret = null;

            //Identify the file.
            switch (magic) {

                //Sequence.
                case "SEQ":
                    ret = new SoundSequence();
                    break;

                //Bank.
                case "BNK":
                    ret = new SoundBank();
                    break;

                //Wave archive.
                case "WAR":
                    ret = new SoundWaveArchive();
                    break;

                //Wave sound data.
                case "WSD":
                    ret = new WaveSoundData();
                    break;

                //Group.
                case "GRP":
                    ret = new Group();
                    break;

                //Wave.
                case "WAV":
                    if (!trueMagic.Equals("BWAV")) {
                        ret = new Wave();
                    } else {
                        ret = new BinaryWave();
                    }
                    break;

                //Stream.
                case "STM":
                    ret = new Stream();
                    break;

                //Prefetch.
                case "STP":
                    ret = new PrefetchFile();
                    break;

                //Sead.
                case "ARC":
                    ret = new SeadArchive();
                    break;

                //Unknown.
                default:
                    ret = new UnknownFile(file);
                    break;
                
            }

            //Read data.
            ret.Read(br);

            //Dispose of reader.
            br.Dispose();
            src.Dispose();

            //Return the file.
            return ret;

        }

        /// <summary>
        /// Read a null terminated string.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <returns>The string.</returns>
        public static string ReadNullTerminated(BinaryDataReader br) {

            //Read a null terminated string.
            List<char> chars = new List<char>();
            bool ended = false;
            while (!ended) {
                char c = br.ReadChar();
                if (c != 0) { chars.Add(c); } else { ended = true; }
            }
            return new string(chars.ToArray());

        }

    }

}
