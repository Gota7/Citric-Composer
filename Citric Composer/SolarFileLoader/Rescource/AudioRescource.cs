using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitraFileLoader;
using Syroot.BinaryData;

namespace SolarFileLoader {

    /// <summary>
    /// Audio rescource (.bars).
    /// </summary>
    public class AudioRescource : AudioFile, IList<RescourceAsset> {

        /// <summary>
        /// Sorted asset list by string hash.
        /// </summary>
        private List<RescourceAsset> assets = new List<RescourceAsset>();

        /// <summary>
        /// Get the rescource asset at a particular index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RescourceAsset this[int index] { get => assets[index]; set { assets[index] = value; ResortList(); } }

        /// <summary>
        /// Number of assets.
        /// </summary>
        public int Count => assets.Count;

        /// <summary>
        /// If the asset list is read only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// New audio rescource.
        /// </summary>
        public AudioRescource() {
            Magic = "BARS";
            Version = new Version(1, 1);
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

            //Skip filesize.
            br.ReadUInt32();

            //Read endian and version.
            ReadByteOrder(br);
            ReadVersion(br);

            //Number of assets.
            uint numAssets = br.ReadUInt32();

            //Make the assets list.
            assets = new List<RescourceAsset>();

            //Skip hashes.
            br.ReadUInt32s((int)numAssets);

            //Get the offsets.
            List<int> metaOffsets = new List<int>();
            List<int> audioOffsets = new List<int>();
            for (int i = 0; i < numAssets; i++) {
                assets.Add(new RescourceAsset());
                metaOffsets.Add(br.ReadInt32());
                audioOffsets.Add(br.ReadInt32());
            }

            //Read each audio file.
            for (int i = 0; i < numAssets; i++) {
                r.JumpToOffset(br, audioOffsets[i]);
                FileReader f = new FileReader();
                br.Position += 4;
                Syroot.BinaryData.ByteOrder bakOrder = br.ByteOrder;
                br.ByteOrder = Syroot.BinaryData.ByteOrder.BigEndian;
                if (br.ReadUInt16() == 0xFFFE) {
                    br.ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian;
                }
                br.Position += 6;
                uint audioSize = br.ReadUInt32();
                br.Position -= 0x10;
                assets[i].SoundFile = SoundArchiveReader.ReadFile(br.ReadBytes((int)audioSize));
                br.ByteOrder = bakOrder;
            }

            //Read each meta file.
            for (int i = 0; i < numAssets; i++) {
                r.JumpToOffset(br, metaOffsets[i]);
                Syroot.BinaryData.ByteOrder bakOrder = br.ByteOrder;
                assets[i].Meta = new AudioMeta();
                assets[i].Meta.Read(br);
                assets[i].Meta.Asset = assets[i].SoundFile;
                br.ByteOrder = bakOrder;
            }

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void Write(BinaryDataWriter bw) {

            //Resort the list.
            ResortList();

            //New write helper.
            WriteHelper w = new WriteHelper(bw);

            //Write magic.
            WriteMagic(bw);

            //Skip filesize.
            w.InitOffsetTable(bw, "FileSize");
            bw.Write((uint)0);

            //Endian.
            WriteByteOrder(bw);

            //Version.
            WriteVersion(bw);

            //Number of assets.
            bw.Write((uint)assets.Count);

            //Write the hashes.
            foreach (var a in assets) {
                bw.Write(a.StringHash);
            }

            //Init main reference table.
            long refTablePos = bw.Position;
            w.InitOffsetTable(bw, "RefTable");
            bw.Write(new UInt64[assets.Count]);

            //Write each meta file, there is no padding.
            for (int i = 0; i < assets.Count; i++) {
                w.WriteOffsetTableEntry(bw, i * 2, "RefTable");
                assets[i].Meta.Write(bw);
            }

            //Pad to 0x40.
            while (bw.Position % 0x40 != 0) {
                bw.Write((byte)0);
            }

            //Write each audio file, padding to 0x40 except for the last audio file.
            Dictionary<byte[], int> audioOffs = new Dictionary<byte[], int>(new ByteArrayComparer());
            for (int i = 0; i < assets.Count; i++) {

                //Get data.
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw2 = new BinaryDataWriter(o);
                if (assets[i].SoundFile as Wave != null) {
                    bw2.Write((assets[i].SoundFile as Wave).Wav.ToBytes(CitraFileLoader.ByteOrder.LittleEndian, true, 0x40));
                } else {
                    assets[i].SoundFile.Write(WriteMode.NX, bw2);
                }
                byte[] audioData = o.ToArray();

                //Add dictionary if needed.
                if (audioOffs.ContainsKey(audioData)) {
                    w.WriteManualOffset(bw, refTablePos + (i * 2 + 1) * 4, audioOffs[audioData]);
                } else {

                    //Write data.
                    audioOffs.Add(audioData, (int)bw.Position);
                    w.WriteOffsetTableEntry(bw, i * 2 + 1, "RefTable");
                    bw.Write(audioData);

                }
                            

                //Pad as needed.
                if (i != assets.Count - 1) {

                    //Pad to 0x40.
                    while (bw.Position % 0x40 != 0) {
                        bw.Write((byte)0);
                    }

                }

            }

            //Write the file size.
            w.WriteOffsetTableEntry(bw, 0, "FileSize");

        }

        /// <summary>
        /// Resort the list.
        /// </summary>
        public void ResortList() {
            assets = assets.OrderBy(o => o.StringHash).ToList();
        }

        //List stuff.
        #region ListStuff

        public void Add(RescourceAsset item) {
            assets.Add(item);
            ResortList();
        }

        public void Clear() {
            assets.Clear();
        }

        public bool Contains(RescourceAsset item) {
            return assets.Contains(item);
        }

        public void CopyTo(RescourceAsset[] array, int arrayIndex) {
            assets.CopyTo(array, arrayIndex);
        }

        public IEnumerator<RescourceAsset> GetEnumerator() {
            return assets.GetEnumerator();
        }

        public int IndexOf(RescourceAsset item) {
            return assets.IndexOf(item);
        }

        public void Insert(int index, RescourceAsset item) {
            assets.Insert(index, item);
            ResortList();
        }

        public bool Remove(RescourceAsset item) {
            return assets.Remove(item);
        }

        public void RemoveAt(int index) {
            assets.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return assets.GetEnumerator();
        }

        #endregion

    }

}
