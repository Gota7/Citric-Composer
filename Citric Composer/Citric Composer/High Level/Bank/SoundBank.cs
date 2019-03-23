using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Bank file. To hard for now, may come back later.
    /// </summary>
    public class SoundBank : ISoundFile {

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
        /// Instruments.
        /// </summary>
        public List<IInstrument> Instruments;


        /// <summary>
        /// Create a new sound bank.
        /// </summary>
        public SoundBank() {
            Waves = new List<WaveArchivePair>();
            Instruments = new List<IInstrument>();
        }

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(writeMode) + "BNK").ToLower();
        }

        /// <summary>
        /// Read the bank.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Open the bank file.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out writeMode, out Version);

            //Open info block.
            FileReader.OpenBlock(br, 0);

            //Add the reference to the wave id table.
            FileReader.OpenReference(br, "WaveIdTableRef");

            //Add the reference to the instrument reference table.
            FileReader.OpenReference(br, "InstrumentReferenceTableRef");

            //Jump to the wave id table.
            if (!FileReader.ReferenceIsNull("WaveIdTableRef")) {
                FileReader.JumpToReference(br, "WaveIdTableRef");

                //Read the wave id table.
                uint waveIdCount = br.ReadUInt32();
                for (int i = 0; i < waveIdCount; i++) {
                    Id warId = new Id(ref br);
                    uint wavId = br.ReadUInt32();
                    Waves.Add(new WaveArchivePair((int)warId.index, (int)wavId));
                }

            }
            
            //Null wave ids.
            else {
                Waves = null;
            }

            //Close the wave id table reference.
            FileReader.CloseReference("WaveIdTableRef");

            //Jump to the instrument reference table.
            if (!FileReader.ReferenceIsNull("InstrumentReferenceTableRef")) {
                FileReader.JumpToReference(br, "InstrumentReferenceTableRef");

                //Reference table is a structure.
                FileReader.StartStructure(br);

                //Read the instrument reference table.
                FileReader.OpenReferenceTable(br, "InstrumentReferenceTable");

                //Read each instrument reference.
                Instruments = new List<IInstrument>();
                for (int i = 0; i < FileReader.ReferenceTableCount("InstrumentReferenceTable"); i++) {

                    //If reference is null.
                    if (FileReader.ReferenceTableReferenceIsNull(i, "InstrumentReferenceTable")) {
                        Instruments.Add(null);
                    }
                    
                    //Reference is not null.
                    else {

                        //New instruments list.
                        IInstrument inst = null;

                        //Jump to reference.
                        FileReader.ReferenceTableJumpToReference(br, i, "InstrumentReferenceTable");

                        //Instrument is a new structure.
                        FileReader.StartStructure(br);

                        //Instrument type reference.
                        FileReader.OpenReference(br, "InstType");

                        //Jump to reference if not null.
                        if (!FileReader.ReferenceIsNull("InstType")) {

                            //Jump to the reference.
                            FileReader.JumpToReference(br, "InstType");

                            //Instrument is a new structure.
                            FileReader.StartStructure(br);

                            //Switch the type of reference.
                            switch (FileReader.ReferenceGetType("InstType")) {

                                //Direct.
                                case ReferenceTypes.BNK_RefTable_Direct:

                                    //Create direct instrument.
                                    DirectInstrument d = new DirectInstrument();

                                    //Reference to key region.
                                    FileReader.OpenReference(br, "KeyRegionRef");

                                    //If key region is not null.
                                    if (!FileReader.ReferenceIsNull("KeyRegionRef")) {

                                        //Read key region.
                                        d.KeyRegion = ReadKeyRegion(br, FileReader);

                                    } else {

                                        //Add null region.
                                        d.KeyRegion = null;

                                    }

                                    //Set instrument to the direct.
                                    inst = d;

                                    //Close reference to key region.
                                    FileReader.CloseReference("KeyRegionRef");
                                    break;

                                //Ranged.
                                case ReferenceTypes.BNK_RefTable_Range:

                                    //New ranged instrument.
                                    RangeInstrument ran = new RangeInstrument();

                                    //Get count.
                                    ran.StartNote = br.ReadByte();
                                    ran.EndNote = br.ReadByte();
                                    br.ReadUInt16();

                                    //Read each reference.
                                    for (int j = 0; j < ran.NoteCount; j++) {
                                        FileReader.OpenReference(br, "Ran" + j);
                                    }

                                    //Read each key region.
                                    for (int j = 0; j < ran.NoteCount; j++) {

                                        //Null region.
                                        if (FileReader.ReferenceIsNull("Ran" + j)) {
                                            ran.Add(null);
                                        }

                                        //Read key region.
                                        else {

                                            //Jump to reference.
                                            FileReader.JumpToReference(br, "Ran" + j);

                                            //Read the key region.
                                            ran.Add(ReadKeyRegion(br, FileReader));

                                        }

                                    }

                                    //Close each reference.
                                    for (int j = 0; j < ran.NoteCount; j++) {
                                        FileReader.CloseReference("Ran" + j);
                                    }

                                    //Set the instrument to this.
                                    inst = ran;
                                    break;

                                //Index.
                                case ReferenceTypes.BNK_RefTable_Index:

                                    //Read the table of indices.
                                    uint numInd = br.ReadUInt32();
                                    List<byte> indices = new List<byte>();
                                    for (int j = 0; j < numInd; j++) {
                                        indices.Add(br.ReadByte());
                                    }

                                    //Read padding.
                                    FileReader.SeekAlign(br, 4);

                                    //New index instrument.
                                    IndexInstrument ind = new IndexInstrument();
                                    
                                    //Read each index reference.
                                    for (int j = 0; j < numInd; j++) {
                                        FileReader.OpenReference(br, "Ind" + j);
                                    }

                                    //Read each index.
                                    for (int j = 0; j < numInd; j++) {

                                        //Null index.
                                        if (FileReader.ReferenceIsNull("Ind" + j)) {
                                            ind.Add((sbyte)indices[j], null);
                                        }

                                        //Index exist.
                                        else {

                                            //Jump to reference.
                                            FileReader.JumpToReference(br, "Ind" + j);

                                            //Read the key region.
                                            ind.Add((sbyte)indices[j], ReadKeyRegion(br, FileReader));

                                        }

                                    }

                                    //Close each index reference.
                                    for (int j = 0; j < numInd; j++) {
                                        FileReader.CloseReference("Ind" + j);
                                    }

                                    //Set the instrument to this.
                                    inst = ind;
                                    break;

                            }

                            //End the instrument.
                            FileReader.EndStructure();

                        }

                        //Add instrument.
                        Instruments.Add(inst);

                        //Close the instrument type reference.
                        FileReader.CloseReference("InstType");

                        //End the instrument structure.
                        FileReader.EndStructure();

                    }

                }

                //Close the instrument reference table.
                FileReader.CloseReferenceTable("InstrumentReferenceTable");

                //End the instrument reference table structure.
                FileReader.EndStructure();

            } else {
                Instruments = null;
            }

            //Close the instrument reference table reference.
            FileReader.CloseReference("InstrumentReferenceTableRef");

            //Close the info block.
            FileReader.CloseBlock(br);

            //Close the bank file.
            FileReader.CloseFile(br);
        
        }

        /// <summary>
        /// Return a key region.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <returns>A key region.</returns>
        public IKeyRegion ReadKeyRegion(BinaryDataReader br, FileReader FileReader) {

            //Key region is a structure.
            FileReader.StartStructure(br);

            //Set up key region.
            IKeyRegion keyRegion = null;

            //Get reference to the velocity type.
            FileReader.OpenReference(br, "KeyTypeRef");

            //Reference is not null.
            if (!FileReader.ReferenceIsNull("KeyTypeRef")) {

                //Jump to the reference.
                FileReader.JumpToReference(br, "KeyTypeRef");

                //It's a new structure.
                FileReader.StartStructure(br);

                //Switch reference type.
                switch (FileReader.ReferenceGetType("KeyTypeRef")) {

                    //Direct.
                    case ReferenceTypes.BNK_RefTable_Direct:

                        //New direct key region.
                        DirectKeyRegion dir = new DirectKeyRegion();

                        //Open reference to velocity region.
                        FileReader.OpenReference(br, "VelRegion");

                        //Not null velocity region.
                        if (!FileReader.ReferenceIsNull("VelRegion")) {

                            //Jump to the velocity region.
                            FileReader.JumpToReference(br, "VelRegion");

                            //Open the velocity region.
                            dir.VelocityRegion = ReadVelocityInfo(br, FileReader);

                        }

                        //Close reference to velocity region.
                        FileReader.CloseReference("VelRegion");

                        //Set the key region.
                        keyRegion = dir;
                        break;

                    //Range.
                    case ReferenceTypes.BNK_RefTable_Range:

                        //New ranged instrument.
                        RangeKeyRegion ran = new RangeKeyRegion();

                        //Get count.
                        ran.StartVelocity = br.ReadByte();
                        ran.EndVelocity = br.ReadByte();
                        br.ReadUInt16();

                        //Read each reference.
                        for (int j = 0; j < ran.VelocityCount; j++) {
                            FileReader.OpenReference(br, "RanKey" + j);
                        }

                        //Read each key region.
                        for (int j = 0; j < ran.VelocityCount; j++) {

                            //Null region.
                            if (FileReader.ReferenceIsNull("RanKey" + j)) {
                                ran.Add(null);
                            }

                            //Read key region.
                            else {

                                //Jump to reference.
                                FileReader.JumpToReference(br, "RanKey" + j);

                                //Read the velocity region.
                                ran.Add(ReadVelocityInfo(br, FileReader));

                            }

                        }

                        //Close each reference.
                        for (int j = 0; j < ran.VelocityCount; j++) {
                            FileReader.CloseReference("RanKey" + j);
                        }

                        //Set the key region to this.
                        keyRegion = ran;
                        break;

                    //Index.
                    case ReferenceTypes.BNK_RefTable_Index:

                        //Read the table of indices.
                        uint numInd = br.ReadUInt32();
                        List<byte> indices = new List<byte>();
                        for (int j = 0; j < numInd; j++) {
                            indices.Add(br.ReadByte());
                        }

                        //Read padding.
                        FileReader.SeekAlign(br, 4);

                        //New index key region.
                        IndexKeyRegion ind = new IndexKeyRegion();

                        //Read each index reference.
                        for (int j = 0; j < numInd; j++) {
                            FileReader.OpenReference(br, "IndKey" + j);
                        }

                        //Read each index.
                        for (int j = 0; j < numInd; j++) {

                            //Null index.
                            if (FileReader.ReferenceIsNull("IndKey" + j)) {
                                ind.Add((sbyte)indices[j], null);
                            }

                            //Index exist.
                            else {

                                //Jump to reference.
                                FileReader.JumpToReference(br, "IndKey" + j);

                                //Read the velocity region.
                                ind.Add((sbyte)indices[j], ReadVelocityInfo(br, FileReader));

                            }

                        }

                        //Close each index reference.
                        for (int j = 0; j < numInd; j++) {
                            FileReader.CloseReference("IndKey" + j);
                        }

                        //Set the key region to this.
                        keyRegion = ind;
                        break;

                }

                //End the vel type stuff.
                FileReader.EndStructure();

            }

            //Close reference to velocity type.
            FileReader.CloseReference("KeyTypeRef");

            //End the velocity region.
            FileReader.EndStructure();

            //Return the velocity region.
            return keyRegion;

        }

        /// <summary>
        /// Read a velocity region.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <returns>The velocity region.</returns>
        public VelocityRegion ReadVelocityInfo(BinaryDataReader br, FileReader FileReader) {

            //Start structure.
            FileReader.StartStructure(br);

            //New velocity region.
            VelocityRegion r = new VelocityRegion();

            //First is the wave id index.
            r.WaveIndex = (int)br.ReadUInt32();

            //Flags.
            FlagParameters f = new FlagParameters(ref br);

            //Original key. 0x000000KK.
            r.OriginalKey = (sbyte)(0xFF & SoundArchiveReader.GetFlagValue(f, 0, 60));

            //Volume. 0x000000VV.
            r.Volume = (byte)(0xFF & SoundArchiveReader.GetFlagValue(f, 1, 127));

            //Pan. 0x0000SSPP.   S = Span, P = Pan.
            r.Pan = (sbyte)(0xFF & SoundArchiveReader.GetFlagValue(f, 2, 64));
            r.SurroundPan = (sbyte)((0xFF00 & SoundArchiveReader.GetFlagValue(f, 2)) >> 8);

            //Pitch.
            r.Pitch = BitConverter.ToSingle(BitConverter.GetBytes(SoundArchiveReader.GetFlagValue(f, 3, 0x3F800000)), 0);

            //Note parameters. 0x00TTGGII.   T = Interpolation Type, G = Key group, I = Ignore note off.
            uint noteParam = SoundArchiveReader.GetFlagValue(f, 4, 0);
            r.PercussionMode = (noteParam & 0xFF) > 0;
            r.KeyGroup = (byte)((noteParam & 0xFF00) >> 8);
            r.InterPolationType = (VelocityRegion.EInterPolationType)((noteParam & 0xFF0000) >> 16);

            //ADSHR curve offset.
            if (f.isFlagEnabled[9]) {

                //ADSHR not null.
                if (f.flagValues[9] != 0xFFFFFFFF) {

                    //Jump to offset.
                    FileReader.JumpToOffsetManually(br, (int)f.flagValues[9]);

                    //Start structure.
                    FileReader.StartStructure(br);

                    //ADSHR curve reference is here.
                    FileReader.OpenReference(br, "ADSHR");

                    //Not null reference.
                    if (!FileReader.ReferenceIsNull("ADSHR")) {

                        //Jump to position.
                        FileReader.JumpToReference(br, "ADSHR");

                        //Read ADSHR curve.
                        r.Attack = br.ReadByte();
                        r.Decay = br.ReadByte();
                        r.Sustain = br.ReadByte();
                        r.Hold = br.ReadByte();
                        r.Release = br.ReadByte();

                    }

                    //End structure.
                    FileReader.EndStructure();

                    //Close reference.
                    FileReader.CloseReference("ADSHR");

                }

            }

            //End structure.
            FileReader.EndStructure();

            //Return the velocity region.
            return r;

        }

        /// <summary>
        /// Write a bank file.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {

            //Set the write mode.
            this.writeMode = writeMode;

            //Init the bank file.
            FileWriter FileWriter = new FileWriter();
            FileWriter.InitFile(bw, writeMode, "BNK", 1, Version);

            //Init info block.
            FileWriter.InitBlock(bw, ReferenceTypes.BNK_Block_Info, "INFO");

            //Init the reference to the wave id table.
            FileWriter.InitReference(bw, "WaveIdTableRef");

            //Init the reference to the instrument reference table.
            FileWriter.InitReference(bw, "InstrumentReferenceTableRef");

            //Write the instrument reference table.
            if (Instruments != null) {

                //Write the reference.
                FileWriter.CloseReference(bw, ReferenceTypes.Table_Reference, "InstrumentReferenceTableRef");

                //Reference table is a structure.
                FileWriter.StartStructure(bw);

                //Init the instrument reference table.
                FileWriter.InitReferenceTable(bw, Instruments.Count, "InstrumentReferenceTable");

                //Write each instrument.
                for (int i = 0; i < Instruments.Count; i++) {

                    //If instrument is null.
                    if (Instruments[i] == null) {

                        //Write the null reference.
                        FileWriter.AddReferenceTableNullReferenceWithType(ReferenceTypes.BNK_Info_Null, "InstrumentReferenceTable");

                    }

                    //Instrument is not null.
                    else {

                        //Add reference.
                        FileWriter.AddReferenceTableReference(bw, ReferenceTypes.BNK_Info_Instrument, "InstrumentReferenceTable");

                        //Instrument is a new structure.
                        FileWriter.StartStructure(bw);

                        //Instrument type reference.
                        FileWriter.InitReference(bw, "InstType");

                        //Get the type of reference.
                        switch (Instruments[i].GetInstrumentType()) {
                            
                            //Direct.
                            case InstrumentType.Direct:
                                FileWriter.CloseReference(bw, ReferenceTypes.BNK_RefTable_Direct, "InstType");
                                break;

                            //Index.
                            case InstrumentType.Index:
                                FileWriter.CloseReference(bw, ReferenceTypes.BNK_RefTable_Index, "InstType");
                                break;

                            //Range.
                            case InstrumentType.Range:
                                FileWriter.CloseReference(bw, ReferenceTypes.BNK_RefTable_Range, "InstType");
                                break;

                        }

                        //Instrument is a new structure.
                        FileWriter.StartStructure(bw);

                        //Switch the type of reference.
                        switch (Instruments[i].GetInstrumentType()) {

                            //Direct.
                            case InstrumentType.Direct:

                                //Direct.
                                DirectInstrument d = ((DirectInstrument)Instruments[i]);

                                //Write key region reference.
                                FileWriter.InitReference(bw, "KeyRegion");

                                //Null region.
                                if (d.KeyRegion == null) {
                                    FileWriter.CloseNullReference(bw, "KeyRegion");
                                }

                                //Not null.
                                else {

                                    //Close the reference.
                                    FileWriter.CloseReference(bw, ReferenceTypes.BNK_Info_KeyRegion, "KeyRegion");

                                    //Write key region.
                                    WriteKeyRegion(bw, d.KeyRegion, FileWriter);

                                }
                                break;

                            //Ranged.
                            case InstrumentType.Range:

                                //Set range info.
                                RangeInstrument ran = (RangeInstrument)Instruments[i];

                                //Write stuff.
                                bw.Write((byte)ran.StartNote);
                                bw.Write((byte)ran.EndNote);
                                bw.Write((UInt16)0);

                                //Init each reference.
                                for (int j = 0; j < ran.NoteCount; j++) {
                                    FileWriter.InitReference(bw, "Ran" + j);
                                }

                                //Write each key region.
                                for (int j = 0; j < ran.NoteCount; j++) {

                                    //Null region.
                                    if (ran[j] == null) {
                                        FileWriter.CloseNullReference(bw, "Ran" + j);
                                    }

                                    //Write key region.
                                    else {

                                        //Key region reference.
                                        FileWriter.CloseReference(bw, ReferenceTypes.BNK_Info_KeyRegion, "Ran" + j);

                                        //Write the key region.
                                        WriteKeyRegion(bw, ran[j], FileWriter);

                                    }

                                }
                                break;

                            //Index.
                            case InstrumentType.Index:

                                //Set index data.
                                IndexInstrument ind = (IndexInstrument)Instruments[i];

                                //Write the table of indices.
                                bw.Write((uint)ind.Count);
                                for (int j = 0; j < ind.Count; j++) {
                                    bw.Write((byte)ind.Keys.ElementAt(j));
                                }

                                //Write padding.
                                FileWriter.Align(bw, 4);

                                //Init each index reference.
                                for (int j = 0; j < ind.Count; j++) {
                                    FileWriter.InitReference(bw, "Ind" + j);
                                }

                                //Write each index.
                                for (int j = 0; j < ind.Count; j++) {

                                    //Null index.
                                    if (ind.Values.ElementAt(j) == null) {
                                        FileWriter.CloseNullReference(bw, "Ind" + j);
                                    }

                                    //Index exist.
                                    else {

                                        //Close reference.
                                        FileWriter.CloseReference(bw, ReferenceTypes.BNK_Info_KeyRegion, "Ind" + j);

                                        //Write the key region.
                                        WriteKeyRegion(bw, ind.Values.ElementAt(j), FileWriter);

                                    }

                                }
                                break;

                        }

                        //End the instrument.
                        FileWriter.EndStructure();

                        //End the instrument structure.
                        FileWriter.EndStructure();

                    }

                }

                //Close the instrument reference table.
                FileWriter.CloseReferenceTable(bw, "InstrumentReferenceTable");

                //End the instrument reference table structure.
                FileWriter.EndStructure();

            } else {

                //Write null offset.
                FileWriter.CloseNullReference(bw, "InstrumentReferenceTableRef");

            }

            //Write the wave id table.
            if (Waves != null) {

                //Write the reference.
                FileWriter.CloseReference(bw, ReferenceTypes.Tables, "WaveIdTableRef");

                //Write the wave id table.
                bw.Write((uint)Waves.Count);
                for (int i = 0; i < Waves.Count; i++) {
                    Id warId = new Id(SoundTypes.WaveArchive, (uint)Waves[i].WarIndex);
                    warId.Write(ref bw);
                    bw.Write((uint)Waves[i].WaveIndex);
                }

            }

            //Null wave ids.
            else {
                FileWriter.CloseNullReference(bw, "WaveIdTableRef");
            }

            //Close the info block.
            FileWriter.CloseBlock(bw);

            //Close the bank file.
            FileWriter.CloseFile(bw);

        }

        /// <summary>
        /// Write a bank file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(writeMode, bw);
        }

        /// <summary>
        /// Write a key region.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="key">Key region.</param>
        public void WriteKeyRegion(BinaryDataWriter bw, IKeyRegion key, FileWriter FileWriter) {

            //Instrument is a new structure.
            FileWriter.StartStructure(bw);

            //Key region type reference.
            FileWriter.InitReference(bw, "VelType");

            //Get the type of reference.
            switch (key.GetKeyRegionType()) {

                //Direct.
                case KeyRegionType.Direct:
                    FileWriter.CloseReference(bw, ReferenceTypes.BNK_RefTable_Direct, "VelType");
                    break;

                //Index.
                case KeyRegionType.Index:
                    FileWriter.CloseReference(bw, ReferenceTypes.BNK_RefTable_Index, "VelType");
                    break;

                //Range.
                case KeyRegionType.Range:
                    FileWriter.CloseReference(bw, ReferenceTypes.BNK_RefTable_Range, "VelType");
                    break;

            }

            //Key region is a new structure.
            FileWriter.StartStructure(bw);

            //Switch the type of reference.
            switch (key.GetKeyRegionType()) {

                //Direct.
                case KeyRegionType.Direct:

                    //Direct.
                    DirectKeyRegion d = (DirectKeyRegion)key;

                    //Write velocity region reference.
                    FileWriter.InitReference(bw, "VelRegion");

                    //Null region.
                    if (d.VelocityRegion == null) {
                        FileWriter.CloseNullReference(bw, "VelRegion");
                    }

                    //Not null.
                    else {

                        //Close the reference.
                        FileWriter.CloseReference(bw, ReferenceTypes.BNK_Info_VelocityRegion, "VelRegion");

                        //Write velocity region.
                        WriteVelocityInfo(bw, d.VelocityRegion);

                    }
                    break;

                //Ranged.
                case KeyRegionType.Range:

                    //Set range info.
                    RangeKeyRegion ran = (RangeKeyRegion)key;

                    //Write stuff.
                    bw.Write((byte)ran.StartVelocity);
                    bw.Write((byte)ran.EndVelocity);
                    bw.Write((UInt16)0);

                    //Init each reference.
                    for (int j = 0; j < ran.VelocityCount; j++) {
                        FileWriter.InitReference(bw, "RanKey" + j);
                    }

                    //Write each key region.
                    for (int j = 0; j < ran.VelocityCount; j++) {

                        //Null region.
                        if (ran[j] == null) {
                            FileWriter.CloseNullReference(bw, "RanKey" + j);
                        }

                        //Write key region.
                        else {

                            //Velocity region reference.
                            FileWriter.CloseReference(bw, ReferenceTypes.BNK_Info_VelocityRegion, "RanKey" + j);

                            //Write the velocity region.
                            WriteVelocityInfo(bw, ran[j]);

                        }

                    }
                    break;

                //Index.
                case KeyRegionType.Index:

                    //Set index info.
                    IndexKeyRegion ind = (IndexKeyRegion)key;

                    //Write the table of indices.
                    bw.Write((uint)ind.Count);
                    for (int j = 0; j < ind.Count; j++) {
                        bw.Write((byte)ind.Keys.ElementAt(j));
                    }

                    //Write padding.
                    FileWriter.Align(bw, 4);

                    //Init each index reference.
                    for (int j = 0; j < ind.Count; j++) {
                        FileWriter.InitReference(bw, "IndKey" + j);
                    }

                    //Write each index.
                    for (int j = 0; j < ind.Count; j++) {

                        //Null index.
                        if (ind.Values.ElementAt(j) == null) {
                            FileWriter.CloseNullReference(bw, "IndKey" + j);
                        }

                        //Index exist.
                        else {

                            //Close reference.
                            FileWriter.CloseReference(bw, ReferenceTypes.BNK_Info_VelocityRegion, "IndKey" + j);

                            //Write the velocity region.
                            WriteVelocityInfo(bw, ind.Values.ElementAt(j));

                        }

                    }
                    break;

            }

            //End the key region.
            FileWriter.EndStructure();

            //End the key region structure.
            FileWriter.EndStructure();

        }

        /// <summary>
        /// Write velocity region.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="r">Velocity region to write.</param>
        public void WriteVelocityInfo(BinaryDataWriter bw, VelocityRegion r) {

            //Write wave index.
            bw.Write((uint)r.WaveIndex);

            //Flags are always written the same, so.
            bw.Write((uint)0x21F);

            //Original note.
            bw.Write((uint)r.OriginalKey);

            //Volume.
            bw.Write((uint)r.Volume);

            //Pan.
            bw.Write((uint)(r.Pan + (r.SurroundPan << 8)));

            //Pitch.
            bw.Write(r.Pitch);

            //Note info.
            bw.Write((uint)((r.PercussionMode ? 1 : 0) + (r.KeyGroup << 8) + ((byte)r.InterPolationType << 16)));

            //ADSHR.
            bw.Write((UInt32)0x20);
            bw.Write((UInt32)0);
            bw.Write((UInt32)8);
            bw.Write(r.Attack);
            bw.Write(r.Decay);
            bw.Write(r.Sustain);
            bw.Write(r.Hold);
            bw.Write(r.Release);
            bw.Write(new byte[3]);

        }

    }

}
