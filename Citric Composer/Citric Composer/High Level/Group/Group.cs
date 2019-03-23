using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Group.
    /// </summary>
    public class Group : ISoundFile {

        /// <summary>
        /// File version.
        /// </summary>
        public FileWriter.Version Version;

        /// <summary>
        /// Write mode.
        /// </summary>
        private WriteMode writeMode;

        /// <summary>
        /// Sound files.
        /// </summary>
        public List<SoundFile<ISoundFile>> SoundFiles;

        /// <summary>
        /// Extra info.
        /// </summary>
        public List<InfoExEntry> ExtraInfo;

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(writeMode) + "GRP").ToLower();
        }

        /// <summary>
        /// Read a group file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Read with null files.
            Read(br, null);

        }

        /// <summary>
        /// Read a group file.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="files">List of sound files to make a reference to.</param>
        public void Read(BinaryDataReader br, List<SoundFile<ISoundFile>> files) {

            //Open file.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out writeMode, out Version);

            //Open info block.
            FileReader.OpenBlock(br, 0);

            //Load reference table.
            FileReader.OpenReferenceTable(br, "FileRefs");

            //Load each reference.
            SoundFiles = new List<SoundFile<ISoundFile>>();
            List<SizedReference> sizedReferences = new List<SizedReference>();
            for (int i = 0; i < FileReader.ReferenceTableCount("FileRefs"); i++) {

                //Null reference.
                if (FileReader.ReferenceTableReferenceIsNull(i, "FileRefs")) {
                    SoundFiles.Add(null);
                    sizedReferences.Add(null);
                }

                //Valid reference.
                else {

                    //Jump to reference.
                    FileReader.ReferenceTableJumpToReference(br, i, "FileRefs");

                    //Get file id.
                    UInt32 fileId = br.ReadUInt32();

                    //Add the sized reference.
                    sizedReferences.Add(new SizedReference(ref br));

                    //If the file references exist.
                    if (files != null) {

                        //If file id is found in the files.
                        if (files.Where(x => x.FileId == (int)fileId).Count() > 0) {

                            //Add sound file.
                            SoundFiles.Add(new SoundFile<ISoundFile>());
                            files.Where(x => x.FileId == (int)fileId).ToList()[0].ReferencedBy.Add(SoundFiles[SoundFiles.Count - 1]);
                            SoundFiles[SoundFiles.Count - 1].Reference = files.Where(x => x.FileId == (int)fileId).ToList()[0];

                        }
                        
                        //Independent mode.
                        else {

                            //Add sound file.
                            SoundFiles.Add(new SoundFile<ISoundFile>() { FileId = (int)fileId });

                        }

                    }

                    //Independent mode.
                    else {

                        //Add sound file.
                        SoundFiles.Add(new SoundFile<ISoundFile>() { FileId = (int)fileId });

                    }

                }

            }

            //Close table.
            FileReader.CloseReferenceTable("FileRefs");

            //Close info block.
            FileReader.CloseBlock(br);

            //Open file block.
            FileReader.OpenBlock(br, 1);

            //Read the files.
            for (int i = 0; i < SoundFiles.Count; i++) {

                //File exist in first place.
                if (SoundFiles[i] != null) {

                    //Sound file reference is null, so no embed.
                    if (sizedReferences[i].offset == Reference.NULL_PTR) {
                        SoundFiles[i].Embed = false;
                    }
                    
                    //Sound file reference exist, embed and read it.
                    else {

                        //Embed file.
                        SoundFiles[i].Embed = true;

                        //Jump to file.
                        FileReader.JumpToOffsetManually(br, sizedReferences[i].offset);

                        //Read file.
                        SoundFiles[i].File = SoundArchiveReader.ReadFile(br.ReadBytes((int)sizedReferences[i].size));

                    }

                }

            }

            //Close file block.
            FileReader.CloseBlock(br);

            //Open extended info block.
            FileReader.OpenBlock(br, 2);

            //Open the table.
            FileReader.OpenReferenceTable(br, "Ex");

            //Reach each extra info entry.
            ExtraInfo = new List<InfoExEntry>();
            for (int i = 0; i < FileReader.ReferenceTableCount("Ex"); i++) {

                //Entry is null.
                if (FileReader.ReferenceTableReferenceIsNull(i, "Ex")) {
                    ExtraInfo.Add(null);
                }

                //Entry exist.
                else {

                    //Read data.
                    Id id = new Id(ref br);
                    UInt32 flags = br.ReadUInt32();

                    //New entry.
                    InfoExEntry e = new InfoExEntry();
                    e.ItemIndex = (int)id.index;

                    //Type.
                    switch (id.type) {

                        //Sound.
                        case SoundTypes.Sound:
                            e.ItemType = InfoExEntry.EItemType.Sequence;
                            break;

                        //Seq sound set or wave data.
                        case SoundTypes.SoundGroup:
                            e.ItemType = InfoExEntry.EItemType.SequenceSetOrWaveData;
                            break;

                        //Bank.
                        case SoundTypes.Bank:
                            e.ItemType = InfoExEntry.EItemType.Bank;
                            break;

                        //Wave archive.
                        case SoundTypes.WaveArchive:
                            e.ItemType = InfoExEntry.EItemType.WaveArchive;
                            break;

                    }

                    //Find flags.
                    bool seq = (flags & 0b1) > 0;
                    bool wsd = (flags & 0b10) > 0;
                    bool bnk = (flags & 0b100) > 0;
                    bool war = (flags & 0b1000) > 0;

                    //Test for flags.
                    if (flags == 0xFFFFFFFF) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.All;
                    } else if (seq && bnk) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.SeqAndBank;
                    } else if (seq && war) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.SeqAndWarc;
                    } else if (bnk && war) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.BankAndWarc;
                    } else if (seq) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.Seq;
                    } else if (bnk) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.Bank;
                    } else if (wsd) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.Wsd;
                    } else if (war) {
                        e.LoadFlags = InfoExEntry.ELoadFlags.Warc;
                    }

                    //Add entry.
                    ExtraInfo.Add(e);

                }

            }

            //Close the table.
            FileReader.CloseReferenceTable("Ex");

            //Close extended info block.
            FileReader.CloseBlock(br);

            //Close file.
            FileReader.CloseFile(br);

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {

            //Set write mode.
            this.writeMode = writeMode;

            //Init file.
            FileWriter FileWriter = new FileWriter();
            FileWriter.InitFile(bw, writeMode, "GRP", 3, Version);

            //Info block.
            FileWriter.InitBlock(bw, ReferenceTypes.GRP_Block_Info, "INFO");

            //Reference table to group items.
            FileWriter.InitReferenceTable(bw, SoundFiles.Count, "FileRefs");

            //Write each sound thing.
            for (int i = 0; i < SoundFiles.Count; i++) {

                //Null.
                if (SoundFiles[i] == null) {
                    FileWriter.AddReferenceTableNullReference("FileRefs");
                }

                //Exists.
                else {

                    //Add reference.
                    FileWriter.AddReferenceTableReference(bw, ReferenceTypes.GRP_Info_Item, "FileRefs");

                    //Write data.
                    bw.Write((uint)SoundFiles[i].FileId);
                    FileWriter.InitSizedReference(bw, "FileRef" + i);

                }

            }

            //Align.
            FileWriter.Align(bw, 0x20);

            //Close reference table.
            FileWriter.CloseReferenceTable(bw, "FileRefs");
            
            //Close info block.
            FileWriter.CloseBlock(bw);

            //File block.
            FileWriter.InitBlock(bw, ReferenceTypes.GRP_Block_File, "FILE");

            //Align to 0x20 bytes.
            FileWriter.Align(bw, 0x20);

            //Write each file and align.
            for (int i = 0; i < SoundFiles.Count; i++) {

                //Null.
                if (SoundFiles[i] == null) {
                    FileWriter.CloseSizedNullReference(bw, "FileRef" + i);
                }
                
                //Not null.
                else {

                    //If the file is embedded.
                    if (SoundFiles[i].Embed) {

                        //Keep track of position.
                        long pos = bw.Position;

                        //Write file.
                        FileWriter.WriteFile(bw, SoundFiles[i].File, 0x20, writeMode);

                        //Close reference.
                        FileWriter.CloseSizedReference(bw, ReferenceTypes.General, (uint)(bw.Position - pos), "FileRef" + i);

                    }
                    
                    //Don't write data.
                    else {
                        FileWriter.CloseSizedNullReference(bw, "FileRef" + i);
                    }

                }

            }

            //Close file block.
            FileWriter.CloseBlock(bw);

            //Extra info block.
            FileWriter.InitBlock(bw, ReferenceTypes.GRP_Block_Infx, "INFX");

            //Ex table.
            FileWriter.InitReferenceTable(bw, ExtraInfo.Count, "Ex");

            //Write each info entry.
            for (int i = 0; i < ExtraInfo.Count; i++) {

                //Null.
                if (ExtraInfo[i] == null) {
                    FileWriter.AddReferenceTableNullReference("Ex");
                }

                //Not null.
                else {

                    //Add reference.
                    FileWriter.AddReferenceTableReference(bw, ReferenceTypes.GRP_Infx_Item, "Ex");

                    //Write the data.
                    InfoExEntry e = ExtraInfo[i];
                    UInt32 flags = 0;
                    switch (e.LoadFlags) {

                        case InfoExEntry.ELoadFlags.All:
                            flags = 0xFFFFFFFF;
                            break;

                        case InfoExEntry.ELoadFlags.Bank:
                            flags = 0b100;
                            break;

                        case InfoExEntry.ELoadFlags.BankAndWarc:
                            flags = 0b1100;
                            break;

                        case InfoExEntry.ELoadFlags.Seq:
                            flags = 0b1;
                            break;

                        case InfoExEntry.ELoadFlags.SeqAndBank:
                            flags = 0b101;
                            break;

                        case InfoExEntry.ELoadFlags.SeqAndWarc:
                            flags = 0b1100;
                            break;

                        case InfoExEntry.ELoadFlags.Warc:
                            flags = 0b1000;
                            break;

                        case InfoExEntry.ELoadFlags.Wsd:
                            flags = 0b10;
                            break;

                    }

                    //Get type.
                    byte type = 0;
                    switch (e.ItemType) {

                        case InfoExEntry.EItemType.Bank:
                            type = SoundTypes.Bank;
                            break;

                        case InfoExEntry.EItemType.Sequence:
                            type = SoundTypes.Sound;
                            break;

                        case InfoExEntry.EItemType.SequenceSetOrWaveData:
                            type = SoundTypes.SoundGroup;
                            break;

                        case InfoExEntry.EItemType.WaveArchive:
                            type = SoundTypes.WaveArchive;
                            break;

                    }

                    //Item id.
                    Id id = new Id(type, (uint)e.ItemIndex);
                    id.Write(ref bw);

                    //Write flags.
                    bw.Write(flags);

                }

            }

            //Close Ex table.
            FileWriter.CloseReferenceTable(bw, "Ex");

            //Align.
            FileWriter.Align(bw, 0x20);

            //Close extra info block.
            FileWriter.CloseBlock(bw);

            //Close file.
            FileWriter.CloseFile(bw);

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(writeMode, bw);
        }

    }

}
