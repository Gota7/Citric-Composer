using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace SolarFileLoader {

    /// <summary>
    /// Audio unit distance (.baudc).
    /// </summary>
    public class AudioUnitDistanceCurve : AudioFile {

        /// <summary>
        /// Audio curve type.
        /// </summary>
        public AudioCurveType AudioCurveType;

        /// <summary>
        /// Start value.
        /// </summary>
        public float StartValue;
        
        /// <summary>
        /// End value.
        /// </summary>
        public float EndValue;

        /// <summary>
        /// Hold distance.
        /// </summary>
        public float HoldDistance;

        /// <summary>
        /// Unit distance.
        /// </summary>
        public float UnitDistance;

        /// <summary>
        /// Decay ration.
        /// </summary>
        public float DecayRation;

        /// <summary>
        /// Culling start distance.
        /// </summary>
        public float CullingStartDistance;

        /// <summary>
        /// New audio curve.
        /// </summary>
        public AudioUnitDistanceCurve() {
            Magic = "AUDC";
            Version = new Version(0, 1);
        }

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void Read(BinaryDataReader br) {

            //Read the header.
            ReadMagic(br);
            ReadByteOrder(br);
            ReadVersion(br);

            //Read data.
            AudioCurveType = (AudioCurveType)br.ReadUInt32();
            StartValue = br.ReadSingle();
            EndValue = br.ReadSingle();
            HoldDistance = br.ReadSingle();
            UnitDistance = br.ReadSingle();
            DecayRation = br.ReadSingle();
            CullingStartDistance = br.ReadSingle();

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void Write(BinaryDataWriter bw) {

            //Write the header.
            WriteMagic(bw);
            WriteByteOrder(bw);
            WriteVersion(bw);

            //Write data.
            bw.Write((uint)AudioCurveType);
            bw.Write(StartValue);
            bw.Write(EndValue);
            bw.Write(HoldDistance);
            bw.Write(UnitDistance);
            bw.Write(DecayRation);
            bw.Write(CullingStartDistance);

        }

    }

}
