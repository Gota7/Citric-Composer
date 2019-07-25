using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace SolarFileLoader {

    /// <summary>
    /// Attenuation (.baatn).
    /// </summary>
    public class AudioAttenuation : AudioFile {

        /// <summary>
        /// Asset name.
        /// </summary>
        public string AssetName;

        /// <summary>
        /// Volume curve type.
        /// </summary>
        public CurveType VolumeCurveType;

        /// <summary>
        /// Volume curve file path, no extension.
        /// </summary>
        public string VolumeCurveFile;

        /// <summary>
        /// EnvFx curve type.
        /// </summary>
        public CurveType EnvFxCurveType;

        /// <summary>
        /// EnvFx file path, no extension.
        /// </summary>
        public string EnvFxCurveFile;

        /// <summary>
        /// Filter curve type.
        /// </summary>
        public CurveType FilterCurveType;

        /// <summary>
        /// Filter file path, no extension.
        /// </summary>
        public string FilterCurveFile;

        /// <summary>
        /// Spread curve type.
        /// </summary>
        public CurveType SpreadCurveType;

        /// <summary>
        /// Spread curve file path, no extension.
        /// </summary>
        public string SpreadCurveFile;

        /// <summary>
        /// Priority curve type.
        /// </summary>
        public CurveType PriorityCurveType;

        /// <summary>
        /// Priority curve file path, no extension.
        /// </summary>
        public string PriorityCurveFile;

        /// <summary>
        /// Culling name.
        /// </summary>
        public string CullingName;

        /// <summary>
        /// Listener enabled.
        /// </summary>
        public bool ListenerEnabled;

        /// <summary>
        /// Occlusion enabled.
        /// </summary>
        public bool OcclusionEnabled;

        /// <summary>
        /// Attenuation.
        /// </summary>
        public AudioAttenuation() {
            Magic = "AATN";
            Version = new Version(0, 1);
        }

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void Read(BinaryDataReader br) {

            //Set up helper.
            ReadHelper r = new ReadHelper(br);

            //Read header.
            ReadMagic(br);
            ReadByteOrder(br);
            ReadVersion(br);

            //String table offset.
            int strgOffset = br.ReadInt32();

            //Asset name offset.
            int assetOffset = br.ReadInt32();

            //Get datas.
            VolumeCurveType = (CurveType)br.ReadUInt32();
            int volOffset = br.ReadInt32();
            EnvFxCurveType = (CurveType)br.ReadUInt32();
            int envFxOffset = br.ReadInt32();
            FilterCurveType = (CurveType)br.ReadUInt32();
            int filterOffset = br.ReadInt32();
            SpreadCurveType = (CurveType)br.ReadUInt32();
            int spreadOffset = br.ReadInt32();
            PriorityCurveType = (CurveType)br.ReadUInt32();
            int priorityOffset = br.ReadInt32();

            //Culling name.
            int cullingOffset = br.ReadInt32();

            //Bools.
            ListenerEnabled = br.ReadUInt32() > 0;
            OcclusionEnabled = br.ReadUInt32() > 0;

            //Read strings.
            r.JumpToOffset(br, strgOffset + assetOffset);
            AssetName = r.ReadNullTerminated(br);
            r.JumpToOffset(br, strgOffset + volOffset);
            VolumeCurveFile = r.ReadNullTerminated(br);
            r.JumpToOffset(br, strgOffset + envFxOffset);
            EnvFxCurveFile = r.ReadNullTerminated(br);
            r.JumpToOffset(br, strgOffset + filterOffset);
            FilterCurveFile = r.ReadNullTerminated(br);
            r.JumpToOffset(br, strgOffset + spreadOffset);
            SpreadCurveFile = r.ReadNullTerminated(br);
            r.JumpToOffset(br, strgOffset + priorityOffset);
            PriorityCurveFile = r.ReadNullTerminated(br);
            r.JumpToOffset(br, strgOffset + cullingOffset);
            CullingName = r.ReadNullTerminated(br);

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void Write(BinaryDataWriter bw) {
            throw new NotImplementedException();
        }

    }

}
