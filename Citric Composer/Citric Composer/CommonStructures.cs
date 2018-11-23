/*  ________________________________________________________________________________________________________
 * |                                                                                                   - * X|
 * |                                             CITRIC COMPOSER                                            |
 * |                                                 by Gota7                                               |
 * |________________________________________________________________________________________________________|
 * |                                                                                                        |
 * | This file takes on the role of holding structures that are commonly used throughout the sound archive. |
 * |           It also includes static classes and constants to help make some operations easier.           |
 * |________________________________________________________________________________________________________|
*/


using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader
{

    /// <summary>
    /// Reference to something.
    /// </summary>
    public class Reference
    {

        /// <summary>
        /// Null pointer.
        /// </summary>
        public const Int32 NULL_PTR = -1;

        /// <summary>
        /// Make a new reference.
        /// </summary>
        /// <param name="typeId">New type ID.</param>
        /// <param name="offset">New offset.</param>
        public Reference(UInt16 typeId, Int32 offset)
        {

            this.typeId = typeId;
            this.offset = offset;
            padding = 0;

        }

        /// <summary>
        /// Read a reference.
        /// </summary>
        /// <param name="br">The reader.</param>
        public Reference(ref BinaryDataReader br)
        {

            typeId = br.ReadUInt16();
            padding = br.ReadUInt16();
            offset = br.ReadInt32();

        }

        /// <summary>
        /// Write a reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            bw.Write(typeId);
            bw.Write(padding);
            bw.Write(offset);

        }


        /// <summary>
        /// 1 - Use the ReferenceTypes constants.
        /// </summary>
        public UInt16 typeId;

        /// <summary>
        /// 2 - Padding.
        /// </summary>
        public UInt16 padding;

        /// <summary>
        /// 3 - -1 means invalid.
        /// </summary>
        public Int32 offset;

    }


    /// <summary>
    /// It's just a reference, but with size.
    /// </summary>
    public class SizedReference : Reference
    {

        /// <summary>
        /// Make a new sized reference.
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public SizedReference(UInt16 typeId, Int32 offset, UInt32 size) : base(typeId, offset)
        {

            this.typeId = typeId;
            this.offset = offset;
            padding = 0;
            this.size = size;

        }

        /// <summary>
        /// Read a sized reference.
        /// </summary>
        /// <param name="br">The reader.</param>
        public SizedReference(ref BinaryDataReader br) : base(ref br)
        {

            size = br.ReadUInt32();

        }

        /// <summary>
        /// Write a sized reference.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public new void Write(ref BinaryDataWriter bw)
        {

            bw.Write(typeId);
            bw.Write(padding);
            bw.Write(offset);
            bw.Write(size);

        }


        /// <summary>
        /// 4 - Size of the object.
        /// </summary>
        public UInt32 size;

    }


    /// <summary>
    /// Reference table.
    /// </summary>
    public class ReferenceTable
    {

        /// <summary>
        /// Make a new reference table.
        /// </summary>
        /// <param name="references">Collection of references.</param>
        public ReferenceTable(IEnumerable<Reference> references)
        {

            if (references != null)
            {
                this.count = (UInt32)references.Count();
                this.references = new List<Reference>(references);
            }
            else
            {
                this.count = 0xFFFFFFFF;
                this.references = new List<Reference>();
            }

        }

        /// <summary>
        /// Read a reference table.
        /// </summary>
        /// <param name="br">The reader.</param>
        public ReferenceTable(ref BinaryDataReader br)
        {

            count = br.ReadUInt32();
            references = new List<Reference>();
            for (int i = 0; i < count; i++)
            {
                references.Add(new Reference(ref br));
            }

        }

        /// <summary>
        /// Write a reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            bw.Write(count);
            foreach (Reference r in references)
            {
                r.Write(ref bw);
            }

        }


        /// <summary>
        /// 1 - Number of references.
        /// </summary>
        public UInt32 count;

        /// <summary>
        /// 2 - List of references.
        /// </summary>
        public List<Reference> references;

    }


    /// <summary>
    /// Reference table, but with sizes.
    /// </summary>
    public class SizedReferenceTable
    {

        /// <summary>
        /// Make a new sized reference table.
        /// </summary>
        /// <param name="references">Collection of sized references.</param>
        public SizedReferenceTable(IEnumerable<SizedReference> sizedReferences)
        {

            if (sizedReferences != null)
            {
                this.count = (UInt32)sizedReferences.Count();
                this.sizedReferences = new List<SizedReference>(sizedReferences);
            }
            else
            {
                this.count = 0xFFFFFFFF;
                this.sizedReferences = new List<SizedReference>();
            }

        }

        /// <summary>
        /// Read a sized reference table.
        /// </summary>
        /// <param name="br"></param>
        public SizedReferenceTable(ref BinaryDataReader br)
        {

            count = br.ReadUInt32();
            sizedReferences = new List<SizedReference>();
            for (int i = 0; i < count; i++)
            {
                sizedReferences.Add(new SizedReference(ref br));
            }

        }

        /// <summary>
        /// Write a sized reference table.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            bw.Write(count);
            foreach (SizedReference sr in sizedReferences)
            {
                sr.Write(ref bw);
            }

        }


        /// <summary>
        /// 1 - Number of references.
        /// </summary>
        public UInt32 count;

        /// <summary>
        /// 2 - List of sized references.
        /// </summary>
        public List<SizedReference> sizedReferences;

    }


    /// <summary>
    /// Table of a particular type.
    /// </summary>
    public class Table<T>
    {

        /// <summary>
        /// Make a new table from entries.
        /// </summary>
        /// <param name="entries">Entries for the table.</param>
        public Table(List<T> entries)
        {

            this.entries = entries;
            this.count = (UInt32)entries.Count();

        }

        /// <summary>
        /// Blank contructor.
        /// </summary>
        public Table()
        {

        }


        /// <summary>
        /// 1 - Number of entries.
        /// </summary>
        public UInt32 count;

        /// <summary>
        /// 2 - Table entries.
        /// </summary>
        public List<T> entries;

    }


    /// <summary>
    /// Wave Id.
    /// </summary>
    public class WaveId
    {

        /// <summary>
        /// Make a new wave id.
        /// </summary>
        /// <param name="br">The reader.</param>
        public WaveId(ref BinaryDataReader br)
        {

            waveArchiveId = new Id(ref br);
            waveIndex = br.ReadUInt32();

        }

        /// <summary>
        /// Make a new wave id.
        /// </summary>
        /// <param name="waveArchiveId">The id of the wave archive.</param>
        /// <param name="waveIndex">The index of the wave in the wave archive.</param>
        public WaveId(Id waveArchiveId, UInt32 waveIndex)
        {

            this.waveArchiveId = waveArchiveId;
            this.waveIndex = waveIndex;

        }

        /// <summary>
        /// Write a wave id.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            waveArchiveId.Write(ref bw);
            bw.Write(waveIndex);

        }


        /// <summary>
        /// 1 - The wave archive id.
        /// </summary>
        public Id waveArchiveId;

        /// <summary>
        /// 2 - The wave index.
        /// </summary>
        public UInt32 waveIndex;

    }


    /// <summary>
    /// An ID.
    /// </summary>
    public class Id
    {

        /// <summary>
        /// Make a new id.
        /// </summary>
        /// <param name="br">The reader.</param>
        public Id(ref BinaryDataReader br)
        {

            //Get the type and index.
            _val = br.ReadUInt32();
            type = (byte)((_val & 0xFF000000) >> 24);
            index = _val & 0x00FFFFFF;

        }

        /// <summary>
        /// Make a new id.
        /// </summary>
        /// <param name="type">Type of item.</param>
        /// <param name="index">The item indez.</param>
        public Id(byte type, UInt32 index)
        {

            if (index <= 0x00FFFFFF)
            {

                //Index is valid, set stuff.
                this.type = type;
                this.index = index;

            }
            else
            {

                //Index invalid.
                throw new ArgumentOutOfRangeException();

            }

        }

        /// <summary>
        /// Write an id.
        /// </summary>
        /// <param name="bw"></param>
        public void Write(ref BinaryDataWriter bw)
        {

            //Set real value.
            _val = 0;
            _val += index;
            _val += (UInt32)(type << 24);

            //Write the real value.
            bw.Write(_val);

        }


        /// <summary>
        /// Private real value.
        /// </summary>
        private UInt32 _val;

        /// <summary>
        /// 1 - Type of id. See sound types.
        /// </summary>
        public byte type;

        /// <summary>
        /// 2 - 3 byte unsigned index.
        /// </summary>
        public UInt32 index;

    }


    /// <summary>
    /// File Header.
    /// </summary>
    public class FileHeader
    {

        /// <summary>
        /// Make a new file header.
        /// </summary>
        /// <param name="magic">Type of file.</param>
        /// <param name="version">Version of filetype.</param>
        /// <param name="size">Size of the file.</param>
        /// <param name="blockOffsets">Offsets to the blocks (Do not account for header size).</param>
        public FileHeader(string magic, UInt16 byteOrder, UInt32 version, UInt32 size, List<SizedReference> blockOffsets)
        {

            this.magic = magic.ToCharArray();
            this.byteOrder = byteOrder;
            this.version = version;
            this.size = size;
            this.nBlocks = (UInt16)blockOffsets.Count();
            this.padding = 0;
            this.blockOffsets = blockOffsets;

            //Find the size of the header, and add the reserved.
            UInt16 currentSize = (UInt16)(20 + this.nBlocks * 12);
            List<byte> newReserved = new List<byte>();
            while (currentSize % 0x20 != 0)
            {
                currentSize += 1;
                newReserved.Add(0);
            }
            this.reserved = newReserved.ToArray();
            this.headerSize = currentSize;

            //Update block offsets.
            for (int i = 0; i < this.blockOffsets.Count(); i++)
            {
                if (this.blockOffsets[i].offset != -1) { this.blockOffsets[i].offset += this.headerSize; }
            }

            this.size += this.headerSize;

            //Version.
            if (magic[0] != 'C')
            {

                vMajor = (byte)((version & 0x00FF0000) >> 16);
                vMinor = (byte)((version & 0x0000FF00) >> 8);
                vRevision = (byte)(version & 0x000000FF);

            }
            else
            {

                vMajor = (byte)(version & 0x000000FF);
                vMinor = (byte)((version & 0x0000FF00) >> 8);
                vRevision = (byte)((version & 0x00FF0000) >> 16);

            }

        }

        /// <summary>
        /// Read a file header, byte order is NOT changed.
        /// </summary>
        /// <param name="br">The reader.</param>
        public FileHeader(ref BinaryDataReader br)
        {

            long startingPos = br.Position;
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            magic = br.ReadChars(4);
            byteOrder = br.ReadUInt16();

            //Change the byte order of the reader.
            if (byteOrder == ByteOrder.LittleEndian)
            {
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            }

            headerSize = br.ReadUInt16();

            //Byte order for version is always big endian (not really, but for the sake of coding some 3ds/WiiU things differently).
            br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            version = br.ReadUInt32();

            //Change the byte order of the reader.
            if (byteOrder == ByteOrder.LittleEndian)
            {
                br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
            }

            size = br.ReadUInt32();
            nBlocks = br.ReadUInt16();
            padding = br.ReadUInt16();
            blockOffsets = new List<SizedReference>();
            for (int i = 0; i < nBlocks; i++)
            {
                blockOffsets.Add(new SizedReference(ref br));
            }
            long currentSize = br.Position - startingPos;
            reserved = br.ReadBytes(headerSize - (int)currentSize);

            //Version.
            if (magic[0] != 'C')
            {

                vMajor = (byte)((version & 0x00FF0000) >> 16);
                vMinor = (byte)((version & 0x0000FF00) >> 8);
                vRevision = (byte)(version & 0x000000FF);

            }
            else
            {

                vMajor = (byte)(version & 0x000000FF);
                vMinor = (byte)((version & 0x0000FF00) >> 8);
                vRevision = (byte)((version & 0x00FF0000) >> 16);

            }

        }


        /// <summary>
        /// Write a file header.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            //Make big endian for now.
            bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            bw.Write(magic);
            bw.Write(byteOrder);

            //Get real byte order.
            Syroot.BinaryData.ByteOrder bo = Syroot.BinaryData.ByteOrder.BigEndian;
            if (byteOrder == ByteOrder.LittleEndian)
            {
                bo = Syroot.BinaryData.ByteOrder.LittleEndian;
            }
            bw.ByteOrder = bo;

            bw.Write(headerSize);

            //Version is big endian.
            bw.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
            bw.Write(version);

            //Restore real byte order.
            bw.ByteOrder = bo;
            bw.Write(size);
            bw.Write(nBlocks);
            bw.Write(padding);
            foreach (SizedReference sr in blockOffsets)
            {
                sr.Write(ref bw);
            }
            bw.Write(reserved);

        }

        /// <summary>
        /// Change version.
        /// </summary>
        public void ChangeVersion() {

            UInt32 temp = 0;
            if (magic[0] != 'C')
            {
                temp = vRevision;
                temp += (uint)vMinor << 8;
                temp += (uint)vMajor << 16;
            }
            else {

                temp += (uint)vMajor << 24;
                temp += (uint)vMinor << 16;
                temp += (uint)vRevision << 8;

            }

            if (byteOrder == ByteOrder.BigEndian)
            {
                version = temp;
            }
            else {
                version = 0;
                version += temp & 0xFF000000 >> 24;
                version += temp & 0x00FF0000 >> 8;
                version += temp & 0x0000FF00 << 8;
                version += (uint)(temp & 0x000000FF << 24);
            }

        }

        /// <summary>
        /// 1 - 4 Letters that identify the file.
        /// </summary>
        public char[] magic;

        /// <summary>
        /// 2 - Byte order, only useful for loading.
        /// </summary>
        public UInt16 byteOrder;

        /// <summary>
        /// 3 - Size of the header.
        /// </summary>
        public UInt16 headerSize;

        /// <summary>
        /// 4 - Version of the file: Padding, Major, Minor, Patch.
        /// </summary>
        public UInt32 version;

        /// <summary>
        /// 5 - Size of the file.
        /// </summary>
        public UInt32 size;

        /// <summary>
        /// 6 - Number of blocks.
        /// </summary>
        public UInt16 nBlocks;

        /// <summary>
        /// 7 - Always 0.
        /// </summary>
        public UInt16 padding;

        /// <summary>
        /// 8 - Block offsets in the file.
        /// </summary>
        public List<SizedReference> blockOffsets;

        /// <summary>
        /// 9 - It is all the space left in the header.
        /// </summary>
        public byte[] reserved;

        public byte vMajor;
        public byte vMinor;
        public byte vRevision;

    }


    /// <summary>
    /// Flag parameters.
    /// </summary>
    public class FlagParameters
    {

        /// <summary>
        /// Make new flag parameters.
        /// </summary>
        /// <param name="br">The reader.</param>
        public FlagParameters(ref BinaryDataReader br)
        {

            //Read the UInt32 that has flags enabled, and convert to an array of booleans.
            UInt32 flagsEnabled = br.ReadUInt32();
            isFlagEnabled = new bool[32];
            for (int i = 0; i < 32; i++)
            {

                //See if bit at i is set.
                if (BitHelperUInt.IsBitSet(flagsEnabled, i)) { isFlagEnabled[i] = true; }

            }

            //Read enabled values.
            flagValues = new UInt32[32];
            for (int i = 0; i < isFlagEnabled.Length; i++)
            {
                if (isFlagEnabled[i]) { flagValues[i] = br.ReadUInt32(); }
            }

        }

        /// <summary>
        /// Make new flag parameters from a dictionary.
        /// </summary>
        /// <param name="flags">Dictionary of the bit and corresponding value.</param>
        public FlagParameters(Dictionary<int, UInt32> flags)
        {

            //See what bits are in dictionary.
            isFlagEnabled = new bool[32];
            flagValues = new UInt32[32];
            foreach (KeyValuePair<int, UInt32> flag in flags)
            {

                //Enable the bit at the key, and set the value.
                isFlagEnabled[flag.Key] = true;
                flagValues[flag.Key] = flag.Value;

            }

        }

        /// <summary>
        /// Write flag parameters.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            //Write which flags are enabled.
            UInt32 flagsEnabled = 0;
            for (int i = 0; i < 32; i++)
            {
                if (isFlagEnabled[i]) { BitHelperUInt.SetBit(ref flagsEnabled, i, true); }
            }
            bw.Write(flagsEnabled);

            //Write the enabled values.
            for (int i = 0; i < 32; i++)
            {
                if (isFlagEnabled[i]) { bw.Write(flagValues[i]); }
            }

        }


        /// <summary>
        /// Get the size in bytes the flags will take up.
        /// </summary>
        /// <returns></returns>
        public Int32 GetSize()
        {

            Int32 size = 4;
            for (int i = 0; i < 32; i++)
            {
                if (isFlagEnabled[i]) { size += 4; }
            }
            return size;

        }


        /// <summary>
        /// 1 - Originally a UInt32 saying what flags are used.
        /// </summary>
        public bool[] isFlagEnabled;

        /// <summary>
        /// 2 - Collection of 32 UInt32s specified by a flag set.
        /// </summary>
        public UInt32[] flagValues;

    }


    /// <summary>
    /// Adshr curve.
    /// </summary>
    public class AdshrCurve
    {

        /// <summary>
        /// Make a new adshr curve.
        /// </summary>
        /// <param name="br">The reader.</param>
        public AdshrCurve(ref BinaryDataReader br)
        {

            //Read properties.
            attack = br.ReadByte();
            decay = br.ReadByte();
            sustain = br.ReadByte();
            hold = br.ReadByte();
            release = br.ReadByte();
            padding = br.ReadBytes(3);

        }

        /// <summary>
        /// Make a new curve.
        /// </summary>
        /// <param name="attack">Attack.</param>
        /// <param name="decay">Decay.</param>
        /// <param name="sustain">Sustain.</param>
        /// <param name="hold">Hold.</param>
        /// <param name="release">Release.</param>
        public AdshrCurve(byte attack, byte decay, byte sustain, byte hold, byte release)
        {

            //Set properties.
            this.attack = attack;
            this.decay = decay;
            this.sustain = sustain;
            this.hold = hold;
            this.release = release;

            //Padding.
            padding = new byte[3];

        }

        /// <summary>
        /// Write a curve.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(ref BinaryDataWriter bw)
        {

            //Write properties.
            bw.Write(attack);
            bw.Write(decay);
            bw.Write(sustain);
            bw.Write(hold);
            bw.Write(release);
            bw.Write(padding);

        }


        /// <summary>
        /// 1 - Attack.
        /// </summary>
        public byte attack;

        /// <summary>
        /// 2 - Decay.
        /// </summary>
        public byte decay;

        /// <summary>
        /// 3 - Sustain.
        /// </summary>
        public byte sustain;

        /// <summary>
        /// 4 - Hold.
        /// </summary>
        public byte hold;

        /// <summary>
        /// 5 - Release.
        /// </summary>
        public byte release;

        /// <summary>
        /// 6 - 3 byte padding.
        /// </summary>
        public byte[] padding;

    }


    /// <summary>
    /// For helping with bit operations (UInt).
    /// </summary>
    public static class BitHelperUInt
    {

        /// <summary>
        /// Check if a bit is set.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="bit">Bit number to check.</param>
        /// <returns>Whether the bit is set.</returns>
        public static bool IsBitSet(uint value, int bit)
        {

            if ((value & (0b1 << bit)) >> bit == 0b1) { return true; }
            return false;

        }

        /// <summary>
        /// Set a bit.
        /// </summary>
        /// <param name="value">Value of what to set the bit of.</param>
        /// <param name="bit">Bit number to set.</param>
        /// <param name="enabled">Whether to enable the bit, or disable it.</param>
        public static void SetBit(ref uint value, int bit, bool enabled)
        {

            //Enable bit if desired, but if not already enabled.
            if (enabled)
            {
                if (!IsBitSet(value, bit)) { value += (uint)(0b1 << bit); }
            }

            //Disable bit if desired, but if not already disabled.
            else
            {
                if (IsBitSet(value, bit)) { value -= (uint)(0b1 << bit); }
            }

        }

    }


    /// <summary>
    /// Byte order constants.
    /// </summary>
    public static class ByteOrder
    {

        public const UInt16 BigEndian = 0xFEFF;
        public const UInt16 LittleEndian = 0xFFFE;

    }


    /// <summary>
    /// Sound type constants.
    /// </summary>
    public static class SoundTypes
    {

        public const byte Null = 0;
        public const byte Sound = 1;
        public const byte SoundGroup = 2;
        public const byte Bank = 3;
        public const byte Player = 4;
        public const byte WaveArchive = 5;
        public const byte Group = 6;

    }


    /// <summary>
    /// Reference type constants.
    /// </summary>
    public static class ReferenceTypes
    {

        //Base types.
        public const UInt16 Tables = 0x100;
        public const UInt16 Parameters = 0x200;
        public const UInt16 Codecs = 0x300;
        public const UInt16 General = 0x1F00;

        public const UInt16 SAR_Blocks = 0x2000;
        public const UInt16 SAR_InfoSections = 0x2100;
        public const UInt16 SAR_ItemInfos = 0x2200;
        public const UInt16 SAR_Parameters = 0x2300;
        public const UInt16 SAR_General = 0x2400;

        public const UInt16 STRM_Blocks = 0x4000;
        public const UInt16 STRM_ItemInfos = 0x4100;

        public const UInt16 WSF_Blocks = 0x4800;
        public const UInt16 WSF_ItemInfos = 0x4900;

        public const UInt16 SEQ_Blocks = 0x5000;
        public const UInt16 SEQ_ItemInfos = 0x5100;

        public const UInt16 BNK_Blocks = 0x5800;
        public const UInt16 BNK_Items = 0x5900;
        public const UInt16 BNK_ItemTables = 0x6000;

        public const UInt16 WAR_Blocks = 0x6800;

        public const UInt16 WAV_Blocks = 0x7000;
        public const UInt16 WAV_ItemInfos = 0x7100;

        public const UInt16 GRP_Blocks = 0x7800;
        public const UInt16 GRP_ItemInfos = 0x7900;

        public const UInt16 ASF_Blocks = 0x8000;
        public const UInt16 ASF_Items = 0x8100;


        //Common sound.
        public const UInt16 Table_Embedding = Tables;
        public const UInt16 Table_Reference = Tables + 1;
        public const UInt16 Table_ReferenceWithSize = Tables + 2;

        public const UInt16 Param_Sound3D = Parameters;
        public const UInt16 Param_Sends = Parameters + 1;
        public const UInt16 Param_Envelope = Parameters + 2;
        public const UInt16 Param_AdshrEnvelop = Parameters + 3;

        public const UInt16 Codec_DspAdpcmInfo = Codecs;
        public const UInt16 Codec_ImaAdpcmInfo = Codecs + 1;

        public const UInt16 General_ByteStream = General;
        public const UInt16 String = General + 1;


        //Sound archive file.
        public const UInt16 SAR_Block_String = SAR_Blocks;
        public const UInt16 SAR_Block_Info = SAR_Blocks + 1;
        public const UInt16 SAR_Block_File = SAR_Blocks + 2;

        public const UInt16 SAR_Section_SoundInfo = SAR_InfoSections;
        public const UInt16 SAR_Section_BankInfo = SAR_InfoSections + 1;
        public const UInt16 SAR_Section_PlayerInfo = SAR_InfoSections + 2;
        public const UInt16 SAR_Section_WaveArchiveInfo = SAR_InfoSections + 3;
        public const UInt16 SAR_Section_SoundGroupInfo = SAR_InfoSections + 4;
        public const UInt16 SAR_Section_GroupInfo = SAR_InfoSections + 5;
        public const UInt16 SAR_Section_FileInfo = SAR_InfoSections + 6;

        public const UInt16 SAR_Info_Sound = SAR_ItemInfos;
        public const UInt16 SAR_Info_StreamSound = SAR_ItemInfos + 1;
        public const UInt16 SAR_Info_WaveSound = SAR_ItemInfos + 2;
        public const UInt16 SAR_Info_SequenceSound = SAR_ItemInfos + 3;
        public const UInt16 SAR_Info_SoundGroup = SAR_ItemInfos + 4;
        public const UInt16 SAR_Info_WaveSoundGroup = SAR_ItemInfos + 5;
        public const UInt16 SAR_Info_Bank = SAR_ItemInfos + 6;
        public const UInt16 SAR_Info_WaveArchive = SAR_ItemInfos + 7;
        public const UInt16 SAR_Info_Group = SAR_ItemInfos + 8;
        public const UInt16 SAR_Info_Player = SAR_ItemInfos + 9;
        public const UInt16 SAR_Info_File = SAR_ItemInfos + 10;
        public const UInt16 SAR_Info_Project = SAR_ItemInfos + 11;
        public const UInt16 SAR_Info_InternalFile = SAR_ItemInfos + 12;
        public const UInt16 SAR_Info_ExternalFile = SAR_ItemInfos + 13;
        public const UInt16 SAR_Info_StreamSoundTrack = SAR_ItemInfos + 14;
        public const UInt16 SAR_Info_Send = SAR_ItemInfos + 15;
        public const UInt16 SAR_Info_StreamSoundExtension = SAR_ItemInfos + 16;

        public const UInt16 SAR_StringTable = SAR_General;
        public const UInt16 SAR_PatriciaTree = SAR_General + 1;


        //Stream file.
        public const UInt16 STRM_Block_Info = STRM_Blocks;
        public const UInt16 STRM_Block_Seek = STRM_Blocks + 1;
        public const UInt16 STRM_Block_Data = STRM_Blocks + 2;
        public const UInt16 STRM_Block_Region = STRM_Blocks + 3;
        public const UInt16 STRM_Block_PrefetchData = STRM_Blocks + 4;

        public const UInt16 STRM_Info_StreamSound = STRM_ItemInfos;
        public const UInt16 STRM_Info_Track = STRM_ItemInfos + 1;
        public const UInt16 STRM_Info_Channel = STRM_ItemInfos + 2;


        //Wave sound file.
        public const UInt16 WSF_Block_Info = WSF_Blocks;

        public const UInt16 WSF_WaveSoundMetaData = WSF_ItemInfos;
        public const UInt16 WSF_WaveSoundInfo = WSF_ItemInfos + 1;
        public const UInt16 WSF_NoteInfo = WSF_ItemInfos + 2;
        public const UInt16 WSF_TrackInfo = WSF_ItemInfos + 3;
        public const UInt16 WSF_NoteEvent = WSF_ItemInfos + 4;


        //Wave archive file.
        public const UInt16 WAR_Block_Info = WAR_Blocks;
        public const UInt16 WAR_Block_File = WAR_Blocks + 1;


        //Wave file.
        public const UInt16 WAV_Block_Info = WAV_Blocks;
        public const UInt16 WAV_Block_Data = WAV_Blocks + 1;

        public const UInt16 WAV_ChannelInfo = WAV_ItemInfos;


        //Sequence file.
        public const UInt16 SEQ_Block_Data = SEQ_Blocks;
        public const UInt16 SEQ_Block_Label = SEQ_Blocks + 1;

        public const UInt16 SEQ_LabelInfo = SEQ_ItemInfos;


        //Bank file.
        public const UInt16 BNK_Block_Info = BNK_Blocks;

        public const UInt16 BNK_Info_Instrument = BNK_Items;
        public const UInt16 BNK_Info_KeyRegion = BNK_Items + 1;
        public const UInt16 BNK_Info_VelocityRegion = BNK_Items + 2;
        public const UInt16 BNK_Info_Null = BNK_Items + 3;

        public const UInt16 BNK_RefTable_Direct = BNK_ItemTables;
        public const UInt16 BNK_RefTable_Index = BNK_ItemTables + 1;
        public const UInt16 BNK_RefTable_Range = BNK_ItemTables + 2;


        //Group file.
        public const UInt16 GRP_Block_Info = GRP_Blocks;
        public const UInt16 GRP_Block_File = GRP_Blocks + 1;
        public const UInt16 GRP_Block_Infx = GRP_Blocks + 2;

        public const UInt16 GRP_Info_Item = GRP_ItemInfos;
        public const UInt16 GRP_Infx_Item = GRP_ItemInfos + 1;


        //Animation sound file.
        public const UInt16 ASF_Block_Data = ASF_Blocks;
        public const UInt16 ASF_EventInfo = ASF_Items;

    }


}
