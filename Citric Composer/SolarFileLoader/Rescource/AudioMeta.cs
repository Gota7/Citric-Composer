using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitraFileLoader;
using Syroot.BinaryData;

namespace SolarFileLoader {

    /// <summary>
    /// Audio metadata.
    /// </summary>
    public class AudioMeta : AudioFile {

        /// <summary>
        /// Name of this asset. Remember to call ResortList() after changing!
        /// </summary>
        public string Name;

        /// <summary>
        /// Number of samples in the corresponding FWAV. 0 when FSTP type is used.
        /// </summary>
        private UInt32 NumSamples;

        /// <summary>
        /// Actual asset.
        /// </summary>
        public ISoundFile Asset;

        /// <summary>
        /// Type of audio.
        /// </summary>
        public AudioType AudioType;

        /// <summary>
        /// Number of channels.
        /// </summary>
        public byte NumChannels;

        /// <summary>
        /// Number of tracks.
        /// </summary>
        public byte NumTracks;

        /// <summary>
        /// Flags.
        /// </summary>
        public byte Flags;

        /// <summary>
        /// Volume.
        /// </summary>
        public float Volume;

        /// <summary>
        /// Sample rate.
        /// </summary>
        private UInt32 SampleRate;

        /// <summary>
        /// Loop start.
        /// </summary>
        public UInt32 LoopStartSample;

        /// <summary>
        /// Loop end.
        /// </summary>
        public UInt32 LoopEndSample;

        /// <summary>
        /// Volume.
        /// </summary>
        public float Loudness;

        /// <summary>
        /// Track info.
        /// </summary>
        public TrackInfo[] TrackInfo = new TrackInfo[8];

        /// <summary>
        /// Amplitude peak value.
        /// </summary>
        public float AmplitudePeakValue = 0f;

        /// <summary>
        /// Markers.
        /// </summary>
        public List<MarkerMetaEntry> Markers = new List<MarkerMetaEntry>();

        /// <summary>
        /// Exts.
        /// </summary>
        public List<ExtMetaEntry> Exts = new List<ExtMetaEntry>();

        /// <summary>
        /// New audio metadata.
        /// </summary>
        public AudioMeta() {
            Magic = "BARS";
            Version = new Version(1, 0);
            TrackInfo = new TrackInfo[8];
            Markers = new List<MarkerMetaEntry>();
            Exts = new List<ExtMetaEntry>();
            for (int i = 0; i < TrackInfo.Count(); i++) {
                TrackInfo[i] = new TrackInfo();
            }
        }

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void Read(BinaryDataReader br) {

            //New read helper.
            ReadHelper r = new ReadHelper(br);

            //Read magic.
            ReadMagic(br);

            //Read endian.
            ReadByteOrder(br);

            //Read version.
            ReadVersion(br);

            //Skip file size.
            br.ReadUInt32();

            //Get offsets.
            int dataOffset = br.ReadInt32();
            int markOffset = br.ReadInt32();

            //Ext offset exists only in version 4+.
            int extOffset = -1;
            if (Version.Major >= 4) {
                extOffset = br.ReadInt32();
            }

            //String table offset.
            int strgTableOffset = br.ReadInt32();

            //Read the string table.
            List<string> strings = new List<string>();
            r.JumpToOffset(br, strgTableOffset);
            br.Position += 4;
            uint remainingSize = br.ReadUInt32();

            //Each block seems to be aligned to 4 bytes, read the ext block if exists.
            if (extOffset != -1) {

                //Get data.
                r.JumpToOffset(br, extOffset);

                //Skip header.
                br.ReadUInt64();

                //Get entries.
                uint count = br.ReadUInt32();
                for (int i = 0; i < count; i++) {
                    Exts.Add(new ExtMetaEntry() { Unk1 = br.ReadUInt32(), Unk2 = br.ReadUInt32() });
                }

            }

            //Get data.
            r.JumpToOffset(br, dataOffset);

            //Header and size don't matter.
            br.ReadUInt64();

            //Read name.
            Name = ReadString(br, r, strgTableOffset, br.ReadInt32());

            //Data.
            NumSamples = br.ReadUInt32();
            AudioType = (AudioType)br.ReadByte();
            NumChannels = br.ReadByte();
            NumTracks = br.ReadByte();
            Flags = br.ReadByte();
            Volume = br.ReadSingle();
            SampleRate = br.ReadUInt32();
            LoopStartSample = br.ReadUInt32();
            LoopEndSample = br.ReadUInt32();
            Loudness = br.ReadSingle();

            //Track info.
            for (int i = 0; i < 8; i++) {
                TrackInfo[i].Unk = br.ReadUInt32();
                TrackInfo[i].Volume = br.ReadSingle();
            }

            //Amplitude peak value, if version is at least 4.
            if (Version.Major >= 4) {
                AmplitudePeakValue = br.ReadSingle();
            }

            //Markers.
            r.JumpToOffset(br, markOffset);

            //Skip header.
            br.ReadUInt64();

            //Read entries.
            uint markCount = br.ReadUInt32();
            for (int i = 0; i < markCount; i++) {
                Markers.Add(new MarkerMetaEntry() { Id = br.ReadUInt32(), Name = ReadString(br, r, strgTableOffset, br.ReadInt32()), StartPosition = br.ReadUInt32(), Length = br.ReadUInt32() });
            }

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void Write(BinaryDataWriter bw) {

            //Write helper.
            WriteHelper w = new WriteHelper(bw);

            //Write magic.
            WriteMagic(bw);

            //Write endian.
            WriteByteOrder(bw);

            //Write version.
            WriteVersion(bw);

            //Skip file header for now.
            w.InitOffsetTable(bw, "FileSize");
            bw.Write((uint)0);

            //File offsets.
            w.InitOffsetTable(bw, "BlockRefs");
            bw.Write((uint)0);
            bw.Write((uint)0);
            if (Version.Major >= 4) { bw.Write((uint)0); }
            bw.Write((uint)0);

            //Create string table here.
            Dictionary<string, int> strg = new Dictionary<string, int>();
            int currOffset = 0;

            //Asset name.
            strg.Add(Name, currOffset);
            currOffset += Name.Length + 1;

            //Add mark entries.
            foreach (var m in Markers) {
                if (!strg.ContainsKey(m.Name)) {
                    strg.Add(m.Name, currOffset);
                    currOffset += m.Name.Length + 1;
                }
            }

            //Data block header.
            w.WriteOffsetTableEntry(bw, 0, "BlockRefs");
            bw.Write("DATA".ToCharArray());
            w.InitOffsetTable(bw, "DataSize");
            bw.Write((uint)0);
            w.SS(bw);

            //Asset name.
            bw.Write((uint)0);

            //Write number of frames.
            bw.Write(NumSamples);

            //Number type.
            bw.Write((byte)AudioType);

            //Number of wave channels.
            if (Asset != null) {

                //Update.
                if (AudioType == AudioType.Wave) {
                    NumChannels = (byte)(Asset as Wave).Wav.info.channelInfo.Count;
                }

            }

            //Write number of channels.
            bw.Write(NumChannels);

            //Number of tracks.
            if (AudioType == AudioType.Wave) {
                bw.Write((byte)0);
            } else {
                bw.Write(NumTracks);
            }

            //Flags.
            bw.Write(Flags);

            //Volume.
            bw.Write(Volume);

            //Sample rate.
            if (Asset != null) {

                //Update.
                if (AudioType == AudioType.Wave) {
                    SampleRate = (Asset as Wave).Wav.info.sampleRate;
                }

            }

            //Write sample rate.
            bw.Write(SampleRate);

            //Write loop info.
            bw.Write(LoopStartSample);
            bw.Write(LoopEndSample);

            //Loudness.
            bw.Write(Loudness);

            //Stream tracks.
            foreach (var t in TrackInfo) {
                bw.Write(t.Unk);
                bw.Write(t.Volume);
            }

            //Amplitude.
            if (Version.Major >= 4) {
                bw.Write(AmplitudePeakValue);
            }

            //End data block.
            w.WriteOffsetTableEntry(bw, 0, "DataSize");
            w.ES();

            //Mark header.
            w.WriteOffsetTableEntry(bw, 1, "BlockRefs");
            bw.Write("MARK".ToCharArray());
            w.InitOffsetTable(bw, "MarkSize");
            bw.Write((uint)0);
            w.SS(bw);

            //Mark data.
            bw.Write((uint)Markers.Count);
            foreach (var m in Markers) {
                bw.Write(m.Id);
                bw.Write(strg[m.Name]);
                bw.Write(m.StartPosition);
                bw.Write(m.Length);
            }

            //End mark block.
            w.WriteOffsetTableEntry(bw, 0, "MarkSize");
            w.ES();

            //Ext block.
            if (Version.Major >= 4) {

                //Ext header.
                w.WriteOffsetTableEntry(bw, 2, "BlockRefs");
                bw.Write("EXT_".ToCharArray());
                w.InitOffsetTable(bw, "ExtSize");
                bw.Write((uint)0);
                w.SS(bw);

                //Write data.
                bw.Write((uint)Exts.Count);
                foreach (var e in Exts) {
                    bw.Write(e.Unk1);
                    bw.Write(e.Unk2);
                }

                //Close block.
                w.WriteOffsetTableEntry(bw, 0, "ExtSize");
                w.ES();

            }

            //String header.
            w.WriteOffsetTableEntry(bw, Version.Major >= 4 ? 3 : 2, "BlockRefs");
            bw.Write("STRG".ToCharArray());
            w.InitOffsetTable(bw, "StrgSize");
            bw.Write((uint)0);
            w.SS(bw);

            //Write data.
            foreach (var s in strg) {
                w.WriteNullTerminated(bw, s.Key);
            }

            //Close block.
            w.WriteOffsetTableEntry(bw, 0, "StrgSize");
            w.ES();

            //Write padding.
            while (bw.Position % 4 != 0) {
                bw.Write((byte)0);
            }

            //Write size.
            w.WriteOffsetTableEntry(bw, 0, "FileSize");

        }

        /// <summary>
        /// Read a string entry.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="r">The reader helper.</param>
        /// <param name="strgOffset">String table offset.</param>
        /// <param name="stringOffset">Offset from string table to string.</param>
        /// <returns>The string.</returns>
        public string ReadString(BinaryDataReader br, ReadHelper r, int strgOffset, int stringOffset) {

            //Read the string data.
            long bakPos = br.Position;
            r.JumpToOffset(br, strgOffset + 8 + stringOffset);
            string x = r.ReadNullTerminated(br);
            br.Position = bakPos;
            return x;

        }

    }

}
