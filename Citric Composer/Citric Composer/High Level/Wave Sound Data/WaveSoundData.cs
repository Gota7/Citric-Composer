using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Wave sound data.
    /// </summary>
    public class WaveSoundData : ISoundFile {

        /// <summary>
        /// Write mode.
        /// </summary>
        private WriteMode writeMode;

        /// <summary>
        /// Version.
        /// </summary>
        public FileWriter.Version Version;

        /// <summary>
        /// Wave files.
        /// </summary>
        public List<WaveArchivePair> Waves;

        /// <summary>
        /// Data items.
        /// </summary>
        public List<WaveSoundDataItem> DataItems;


        /// <summary>
        /// Wave sound data.
        /// </summary>
        public WaveSoundData() {
            Waves = new List<WaveArchivePair>();
            DataItems = new List<WaveSoundDataItem>();
        }

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(writeMode) + "WSD").ToLower();
        }

        /// <summary>
        /// Read the file. ID War. U32 Index
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Open the file.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out writeMode, out Version);

            //Open the info block.
            FileReader.OpenBlock(br, 0);

            //Wave id table reference.
            FileReader.OpenReference(br, "WaveIdTableRef");

            //Wave sound data reference table reference.
            FileReader.OpenReference(br, "WaveSoundDataRefTableRef");

            //If table is null.
            if (FileReader.ReferenceIsNull("WaveIdTableRef")) {
                Waves = null;
            }
            
            //Go to table.
            else {

                //New wave list.
                Waves = new List<WaveArchivePair>();

                //Jump to table.
                FileReader.JumpToReference(br, "WaveIdTableRef");

                //Table is here.
                UInt32 count = br.ReadUInt32();
                for (int i = 0; i < count; i++) {

                    //New id.
                    Id war = new Id(ref br);

                    //Add entry.
                    Waves.Add(new WaveArchivePair((int)war.index, (int)br.ReadUInt32()));

                }

            }

            //Close wave id table reference.
            FileReader.CloseReference("WaveIdTableRef");

            //If reference table is null.
            if (FileReader.ReferenceIsNull("WaveSoundDataRefTableRef")) {
                DataItems = null;
            }

            //Read reference table.
            else {

                //New sound data list.
                DataItems = new List<WaveSoundDataItem>();

                //Jump to the reference table.
                FileReader.JumpToReference(br, "WaveSoundDataRefTableRef");

                //This is a structure.
                FileReader.StartStructure(br);

                //Open reference table.
                FileReader.OpenReferenceTable(br, "WaveSoundDataRefTable");

                //Read each entry.
                for (int i = 0; i < FileReader.ReferenceTableCount("WaveSoundDataRefTable"); i++) {

                    //Reference is null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "WaveSoundDataRefTable")) {
                        DataItems.Add(null);
                    }

                    //Valid item.
                    else {

                        //Jump to position.
                        FileReader.ReferenceTableJumpToReference(br, i, "WaveSoundDataRefTable");

                        //Start structure.
                        FileReader.StartStructure(br);

                        //New data.
                        WaveSoundDataItem d = new WaveSoundDataItem();

                        //Wave sound info reference.
                        FileReader.OpenReference(br, "WaveSoundInfoRef");

                        //Track info reference table reference.
                        FileReader.OpenReference(br, "TrackInfoRefTableRef");

                        //Note info reference table reference.
                        FileReader.OpenReference(br, "NoteInfoRefTableRef");

                        //Wave sound info.
                        if (!FileReader.ReferenceIsNull("WaveSoundInfoRef")) {

                            //Jump to the structure.
                            FileReader.JumpToReference(br, "WaveSoundInfoRef");

                            //Start structure.
                            FileReader.StartStructure(br);

                            //They are just flags.
                            FlagParameters f = new FlagParameters(ref br);

                            //Flag list:
                            /*
                             * 0 - Pan info. 0x0000SSPP  S-Span P-Pan.
                             * 1 - Pitch, float.
                             * 2 - Biquad and LPF info. 0x00VVTTLL  L-LPF T-Biquad Type V-Biquad Value.
                             * 8 - Offset to send value.
                             * 9 - Offset to ADSHR curve offset.
                             */

                            //Pan info.
                            d.Pan = (sbyte)(SoundArchiveReader.GetFlagValue(f, 0, (uint)d.Pan) & 0xFF);
                            d.SurroundPan = (sbyte)(SoundArchiveReader.GetFlagValue(f, 0, (uint)d.SurroundPan & 0xFF00) >> 8);

                            //Pitch.
                            if (f.isFlagEnabled[1]) {
                                d.Pitch = BitConverter.ToSingle(BitConverter.GetBytes(SoundArchiveReader.GetFlagValue(f, 1)), 0);
                            }

                            //LPF.
                            d.LpfFrequency = (sbyte)SoundArchiveReader.GetFlagValue(f, 2, (uint)d.LpfFrequency);

                            //Biquad type.
                            d.BiquadType = (WaveSoundDataItem.EBiquadType)((SoundArchiveReader.GetFlagValue(f, 2, 0) & 0xFF00) >> 8);

                            //Biquad value.
                            d.BiquadValue = (sbyte)((SoundArchiveReader.GetFlagValue(f, 2, 0) & 0xFF0000) >> 16);

                            //Offset to send value.
                            uint sendValueOff = SoundArchiveReader.GetFlagValue(f, 8, 0xFFFFFFFF);
                            if (sendValueOff != 0xFFFFFFFF) {

                                //Send value. All bytes: (Main, Count (3), AuxA, AuxB, AuxC), followed by padding to 8 bytes.
                                FileReader.JumpToOffsetManually(br, (int)sendValueOff);
                                d.SendValue[0] = br.ReadByte();
                                int sendCount = br.ReadByte();
                                for (int j = 0; j < 3 && j < sendCount; j++) {
                                    d.SendValue[j + 1] = br.ReadByte();
                                }

                            }


                            //Offset to ADSHR curve.
                            uint adshrOff = SoundArchiveReader.GetFlagValue(f, 9, 0xFFFFFFFF);
                            if (adshrOff != 0xFFFFFFFF) {

                                //Go to the reference.
                                FileReader.JumpToOffsetManually(br, (int)adshrOff);

                                //Start structure.
                                FileReader.StartStructure(br);

                                //Open the reference.
                                FileReader.OpenReference(br, "AdshrRef");

                                //Jump to reference if not null.
                                if (!FileReader.ReferenceIsNull("AdshrRef")) {

                                    //Jump.
                                    FileReader.JumpToReference(br, "AdshrRef");

                                    //ADSHR curve.
                                    d.Attack = br.ReadByte();
                                    d.Decay = br.ReadByte();
                                    d.Sustain = br.ReadByte();
                                    d.Hold = br.ReadByte();
                                    d.Release = br.ReadByte();

                                }

                                //Close the reference.
                                FileReader.CloseReference("AdshrRef");

                                //End structure.
                                FileReader.EndStructure();

                            }

                            //Reference to ADSHR curve.

                            //Read ADSHR curve.

                            //End structure.
                            FileReader.EndStructure();

                        }

                        //Close wave sound info reference.
                        FileReader.CloseReference("WaveSoundInfoRef");

                        //Go to the reference table for tracks.
                        if (!FileReader.ReferenceIsNull("TrackInfoRefTableRef")) {

                            //New list.
                            d.NoteEvents = new List<List<NoteEvent>>();

                            //Go to offset.
                            FileReader.JumpToReference(br, "TrackInfoRefTableRef");

                            //Start track info.
                            FileReader.StartStructure(br);

                            //Read the reference table.
                            FileReader.OpenReferenceTable(br, "TrackInfoRefTable");

                            //Read each track.
                            for (int j = 0; j < FileReader.ReferenceTableCount("TrackInfoRefTable"); j++) {

                                //New null track.
                                List<NoteEvent> track = null;

                                //Reference is not null.
                                if (!FileReader.ReferenceTableReferenceIsNull(j, "TrackInfoRefTable")) {

                                    //Jump to track info.
                                    FileReader.ReferenceTableJumpToReference(br, j, "TrackInfoRefTable");

                                    //Start structure.
                                    FileReader.StartStructure(br);

                                    //Note event table reference.
                                    FileReader.OpenReference(br, "NoteEventRefTableRef");

                                    //Reference is not null.
                                    if (!FileReader.ReferenceIsNull("NoteEventRefTableRef")) {

                                        //Make track valid.
                                        track = new List<NoteEvent>();

                                        //Jump to the table.
                                        FileReader.JumpToReference(br, "NoteEventRefTableRef");

                                        //Start structure.
                                        FileReader.StartStructure(br);

                                        //Read the reference table.
                                        FileReader.OpenReferenceTable(br, "NoteEventRefTable");

                                        //Read each note event.
                                        for (int k = 0; k < FileReader.ReferenceTableCount("NoteEventRefTable"); k++) {

                                            //Null data.
                                            if (FileReader.ReferenceTableReferenceIsNull(j, "NoteEventRefTable")) {
                                                track.Add(null);
                                            }

                                            //Valid note event.
                                            else {

                                                //Jump to note event.
                                                FileReader.ReferenceTableJumpToReference(br, k, "NoteEventRefTable");

                                                //Read note event.
                                                NoteEvent e = new NoteEvent();
                                                e.Position = br.ReadSingle();
                                                e.Length = br.ReadSingle();
                                                e.NoteIndex = (int)br.ReadUInt32();
                                                br.ReadUInt32();

                                                //Add note event.
                                                track.Add(e);

                                            }

                                        }

                                        //Close the reference table.
                                        FileReader.CloseReferenceTable("NoteEventRefTable");

                                        //End structure.
                                        FileReader.EndStructure();

                                    }

                                    //Close note event table reference.
                                    FileReader.CloseReference("NoteEventRefTableRef");

                                    //Ends structure.
                                    FileReader.EndStructure();

                                }

                                //Add track.
                                d.NoteEvents.Add(track);

                            }

                            //Close the reference table.
                            FileReader.CloseReferenceTable("TrackInfoRefTable");

                            //End track info.
                            FileReader.EndStructure();

                        }
                        
                        //Null track info.
                        else {
                            d.NoteEvents = null;
                        }

                        //Close track info reference table reference.
                        FileReader.CloseReference("TrackInfoRefTableRef");

                        //Close note info reference.
                        FileReader.CloseReference("NoteEventRefTableRef");

                        //Go to the reference table for notes.
                        if (!FileReader.ReferenceIsNull("NoteInfoRefTableRef")) {

                            //New list of notes.
                            d.Notes = new List<NoteInfo>();

                            //Go to offset.
                            FileReader.JumpToReference(br, "NoteInfoRefTableRef");

                            //Start note reference table info.
                            FileReader.StartStructure(br);

                            //Open the reference table.
                            FileReader.OpenReferenceTable(br, "NoteInfoRefTable");

                            //Read each note info.
                            for (int j = 0; j < FileReader.ReferenceTableCount("NoteInfoRefTable"); j++) {

                                //If null info.
                                if (FileReader.ReferenceTableReferenceIsNull(j, "NoteInfoRefTable")) {
                                    d.Notes.Add(null);
                                }

                                //Valid note info.
                                else {

                                    //Jump to note info.
                                    FileReader.ReferenceTableJumpToReference(br, j, "NoteInfoRefTable");

                                    //Read data.
                                    d.Notes.Add(ReadNoteInfo(br));

                                }

                            }

                            //Close the reference table.
                            FileReader.CloseReferenceTable("NoteInfoRefTable");

                            //End track info.
                            FileReader.EndStructure();

                        }

                        //Null note info.
                        else {
                            d.Notes = null;
                        }

                        //Close note info reference.
                        FileReader.CloseReference("NoteInfoRefTableRef");

                        //End structure.
                        FileReader.EndStructure();

                        //Add data.
                        DataItems.Add(d);

                    }

                }

                //Close reference table.
                FileReader.CloseReferenceTable("WaveSoundDataRefTable");

                //End structure.
                FileReader.EndStructure();

            }

            //Close wave sound data reference table reference.
            FileReader.CloseReference("WaveSoundDataRefTableRef");

            //Close the info block.
            FileReader.CloseBlock(br);

            //Close the file.
            FileReader.CloseFile(br);

        }

        /// <summary>
        /// Read the note info. TODO.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <returns>The note info.</returns>
        public NoteInfo ReadNoteInfo(BinaryDataReader br) {

            //Start structure.
            FileReader FileReader = new FileReader();
            FileReader.StartStructure(br);

            //New note info.
            NoteInfo n = new NoteInfo();

            //Read data.
            n.WaveIndex = (int)br.ReadUInt32();
            FlagParameters f = new FlagParameters(ref br);
            n.Flags = f;

            //To use for debugging in an instance where note info actually has info.
            if (f.isFlagEnabled[0] || f.isFlagEnabled[1] || f.isFlagEnabled[2] || f.isFlagEnabled[3] || f.isFlagEnabled[4] || f.isFlagEnabled[5] || f.isFlagEnabled[6] || f.isFlagEnabled[7] || f.isFlagEnabled[8] || f.isFlagEnabled[9] || f.isFlagEnabled[10] || f.isFlagEnabled[11] || f.isFlagEnabled[12] || f.isFlagEnabled[13] || f.isFlagEnabled[14] || f.isFlagEnabled[15] || f.isFlagEnabled[16] || f.isFlagEnabled[17] || f.isFlagEnabled[18] || f.isFlagEnabled[19] || f.isFlagEnabled[20] || f.isFlagEnabled[21] || f.isFlagEnabled[22] || f.isFlagEnabled[23] || f.isFlagEnabled[24] || f.isFlagEnabled[25] || f.isFlagEnabled[26] || f.isFlagEnabled[27] || f.isFlagEnabled[28] || f.isFlagEnabled[29] || f.isFlagEnabled[30] || f.isFlagEnabled[31]) {

            }

            //End structure.
            FileReader.EndStructure();

            //Return the note info.
            return n;

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
            FileWriter.InitFile(bw, writeMode, "WSD", 1, Version);

            //Init info block.
            FileWriter.InitBlock(bw, ReferenceTypes.WAR_Blocks, "INFO");

            //Init wave id table reference.
            FileWriter.InitReference(bw, "ToWaveIdTableRef");

            //Init wave sound data table reference.
            FileWriter.InitReference(bw, "ToWaveSoundDataRefTable");

            //Close wave id reference table reference.
            if (Waves != null) {
                FileWriter.CloseReference(bw, ReferenceTypes.Tables, "ToWaveIdTableRef");
            } else {
                FileWriter.CloseNullReference(bw, "ToWaveIdTableRef");
            }

            //Write the table.
            if (Waves != null) {

                //Write count.
                bw.Write((uint)Waves.Count);

                //Write each id.
                for (int i = 0; i < Waves.Count; i++) {

                    //Write id.
                    Id id = new Id(SoundTypes.WaveArchive, (uint)Waves[i].WarIndex);
                    id.Write(ref bw);
                    bw.Write((uint)Waves[i].WaveIndex);

                }

            }

            //Close the wave sound table reference.
            if (DataItems != null) {
                FileWriter.CloseReference(bw, ReferenceTypes.Table_Reference, "ToWaveSoundDataRefTable");
            } else {
                FileWriter.CloseNullReference(bw, "ToWaveSoundDataRefTable");
            }

            //Write the reference table.
            if (DataItems != null) {

                //Start structure.
                FileWriter.StartStructure(bw);

                //Init the table.
                FileWriter.InitReferenceTable(bw, DataItems.Count, "WaveSoundDataTable");

                //Write each entry.
                for (int i = 0; i < DataItems.Count; i++) {

                    //Entry is null.
                    if (DataItems[i] == null) {
                        FileWriter.AddReferenceTableNullReference("WaveSoundDataTable");
                    }
                    
                    //Valid entry.
                    else {

                        //Add the reference.
                        FileWriter.AddReferenceTableReference(bw, ReferenceTypes.WSF_ItemInfos, "WaveSoundDataTable");

                        //New structure.
                        FileWriter.StartStructure(bw);

                        //Init wave sound info reference.
                        FileWriter.InitReference(bw, "WaveSoundInfoRef");

                        //Track info reference table reference.
                        FileWriter.InitReference(bw, "TrackInfoRefTableRef");

                        //Note info reference table reference.
                        FileWriter.InitReference(bw, "NoteInfoRefTableRef");

                        //Close wave sound info reference.
                        FileWriter.CloseReference(bw, ReferenceTypes.WSF_WaveSoundInfo, "WaveSoundInfoRef");

                        //Write the wave sound info.
                        Dictionary<int, uint> flags = new Dictionary<int, uint>();

                        //Flag list:
                        /*
                         * 0 - Pan info. 0x0000SSPP  S-Span P-Pan.
                         * 1 - Pitch, float.
                         * 2 - Biquad and LPF info. 0x00VVTTLL  L-LPF T-Biquad Type V-Biquad Value.
                         * 8 - Offset to send value.
                         * 9 - Offset to ADSHR curve offset.
                         */

                        //Pan info.
                        uint panInfo = 0;
                        panInfo += (uint)(DataItems[i].SurroundPan << 8);
                        panInfo += (uint)DataItems[i].Pan;
                        flags.Add(0, panInfo);

                        //Pitch.
                        flags.Add(1, BitConverter.ToUInt32(BitConverter.GetBytes(DataItems[i].Pitch), 0));

                        //Biquad and LPF.
                        if (DataItems[i].BiquadType != WaveSoundDataItem.EBiquadType.Unused) {
                            uint bL = 0;
                            bL += (uint)(DataItems[i].BiquadValue << 16);
                            bL += (uint)((byte)DataItems[i].BiquadType << 8);
                            bL += (uint)DataItems[i].LpfFrequency;
                            flags.Add(2, bL);
                        }

                        //Offset to send value.
                        flags.Add(8, (uint)(DataItems[i].BiquadType != WaveSoundDataItem.EBiquadType.Unused ? 0x18 : 0x14));

                        //Offset to ADSHR curve offset.
                        flags.Add(9, (uint)(DataItems[i].BiquadType != WaveSoundDataItem.EBiquadType.Unused ? 0x20 : 0x1C));

                        //Write flag sound info.
                        FlagParameters f = new FlagParameters(flags);
                        f.Write(ref bw);

                        //Send value for 3.
                        if (writeMode == WriteMode.Cafe || writeMode == WriteMode.NX) {
                            bw.Write(DataItems[i].SendValue[0]);
                            bw.Write((byte)3);
                            bw.Write(DataItems[i].SendValue[1]);
                            bw.Write(DataItems[i].SendValue[2]);
                            bw.Write(DataItems[i].SendValue[3]);
                            bw.Write(new byte[3]);
                        }
                        
                        //Send value for 2.
                        else {
                            bw.Write(DataItems[i].SendValue[0]);
                            bw.Write((byte)2);
                            bw.Write(DataItems[i].SendValue[1]);
                            bw.Write(DataItems[i].SendValue[2]);
                            bw.Write(new byte[4]);
                        }

                        //Write the ADSHR curve.
                        bw.Write((UInt32)0);
                        bw.Write((UInt32)8);
                        bw.Write(DataItems[i].Attack);
                        bw.Write(DataItems[i].Decay);
                        bw.Write(DataItems[i].Sustain);
                        bw.Write(DataItems[i].Hold);
                        bw.Write(DataItems[i].Release);
                        bw.Write(new byte[3]);

                        //Close track info reference table reference.
                        if (DataItems[i].NoteEvents == null) {
                            FileWriter.CloseNullReference(bw, "TrackInfoRefTableRef");
                        } else {

                            //Close it properly.
                            FileWriter.CloseReference(bw, ReferenceTypes.Table_Reference, "TrackInfoRefTableRef");

                            //New structure.
                            FileWriter.StartStructure(bw);

                            //Init the track info reference table.
                            FileWriter.InitReferenceTable(bw, DataItems[i].NoteEvents.Count, "TrackInfoRefTable");

                            //Write each track.
                            for (int j = 0; j < DataItems[i].NoteEvents.Count; j++) {

                                //Add reference. (There is ambiguity between these note events, and the ones below. Should be fine?
                                if (DataItems[i].NoteEvents[j] == null) {
                                    FileWriter.AddReferenceTableNullReference("TrackInfoRefTable");
                                } else {

                                    //Add reference.
                                    FileWriter.AddReferenceTableReference(bw, ReferenceTypes.WSF_TrackInfo, "TrackInfoRefTable");

                                    //Start structure.
                                    FileWriter.StartStructure(bw);

                                    //Reference to data.
                                    FileWriter.InitReference(bw, "TracksRef");

                                    //Write the track reference table.
                                    FileWriter.CloseReference(bw, 0, "TracksRef");

                                    //Start structure.
                                    FileWriter.StartStructure(bw);

                                    //Reference table.
                                    FileWriter.InitReferenceTable(bw, DataItems[i].NoteEvents[j].Count, "Tracks");

                                    //Write each track.
                                    for (int k = 0; k < DataItems[i].NoteEvents[j].Count; k++) {

                                        //Write the info.
                                        if (DataItems[i].NoteEvents[j][k] == null) {
                                            FileWriter.AddReferenceTableNullReference("Tracks");
                                        } else {

                                            //Proper reference.
                                            FileWriter.AddReferenceTableReference(bw, ReferenceTypes.WSF_NoteEvent, "Tracks");

                                            //Write data.
                                            bw.Write(DataItems[i].NoteEvents[j][k].Position);
                                            bw.Write(DataItems[i].NoteEvents[j][k].Length);
                                            bw.Write((uint)DataItems[i].NoteEvents[j][k].NoteIndex);
                                            bw.Write((uint)0);

                                        }

                                    }

                                    //Close reference table.
                                    FileWriter.CloseReferenceTable(bw, "Tracks");

                                    //End structure.
                                    FileWriter.EndStructure();

                                    //End structure. (Yes, again. There is that pointless reference from earlier.)
                                    FileWriter.EndStructure();

                                }

                            }

                            //Close the track info reference table.
                            FileWriter.CloseReferenceTable(bw, "TrackInfoRefTable");

                            //End structure.
                            FileWriter.EndStructure();

                        }

                        //Close note info reference table reference.
                        if (DataItems[i].Notes == null) {
                            FileWriter.CloseNullReference(bw, "NoteInfoRefTableRef");
                        } else {

                            //Close it properly.
                            FileWriter.CloseReference(bw, ReferenceTypes.Table_Reference, "NoteInfoRefTableRef");

                            //New structure.
                            FileWriter.StartStructure(bw);

                            //Init the note info reference table.
                            FileWriter.InitReferenceTable(bw, DataItems[i].Notes.Count, "Notes");
                            for (int j = 0; j < DataItems[i].Notes.Count; j++) {

                                //Add reference.
                                if (DataItems[i].Notes[j] == null) {
                                    FileWriter.AddReferenceTableNullReference("Notes");
                                } else {

                                    //Add reference.
                                    FileWriter.AddReferenceTableReference(bw, ReferenceTypes.WSF_NoteInfo, "Notes");

                                    //Write data. TEMPORARY UNTIL DATA IS FOUND.
                                    bw.Write((uint)DataItems[i].Notes[j].WaveIndex);
                                    bw.Write((uint)0);

                                }

                            }

                            //Close reference table.
                            FileWriter.CloseReferenceTable(bw, "Notes");

                            //End structure.
                            FileWriter.EndStructure();

                        }

                    }

                }

                //Close the table.
                FileWriter.CloseReferenceTable(bw, "WaveSoundDataTable");

                //End structure.
                FileWriter.EndStructure();

            }

            //Close info block.
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
