using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {
    
    /// <summary>
    /// Sound info.
    /// </summary>
    public class SoundInfo {

        /// <summary>
        /// Name of this item.
        /// </summary>
        public string Name;

        /// <summary>
        /// File.
        /// </summary>
        public SoundFile<ISoundFile> File;

        /// <summary>
        /// 3d Sound info.
        /// </summary>
        public Sound3dInfo Sound3dInfo;

        /// <summary>
        /// Player
        /// </summary>
        public PlayerEntry Player;

        /// <summary>
        /// Volume.
        /// </summary>
        public byte Volume = 96;

        /// <summary>
        /// Remote filter, positive.
        /// </summary>
        public sbyte RemoteFilter;

        /// <summary>
        /// Pan mode.
        /// </summary>
        public EPanMode PanMode = EPanMode.Balance;

        /// <summary>
        /// Pan curve.
        /// </summary>
        public EPanCurve PanCurve;

        /// <summary>
        /// Player actor id. 0-3.
        /// </summary>
        public sbyte PlayerActorId;

        /// <summary>
        /// Player priority.
        /// </summary>
        public sbyte PlayerPriority = 64;

        /// <summary>
        /// Front bypass.
        /// </summary>
        public bool IsFrontBypass;

        /// <summary>
        /// User parameter.
        /// </summary>
        public UInt32[] UserParameter = new uint[4];

        /// <summary>
        /// User parameters enabled.
        /// </summary>
        public bool[] UserParamsEnabled = new bool[4];

        /// <summary>
        /// Pan mode.
        /// </summary>
        public enum EPanMode {
            Dual, Balance
        }

        /// <summary>
        /// Pan curve.
        /// </summary>
        public enum EPanCurve {
            SqrtNegative3, Sqrt0, Sqrt0Clamp, SinCoseNegative3, SinCos0, SinCos0Clamp, LinearNegative6, Linear0, Linear0Clamp
        }


        /// <summary>
        /// Write the sound info.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="w">File writer.</param>
        /// <param name="a">The sound archive.</param>
        /// <param name="strings">Strings.</param>
        public void WriteSoundInfo(BinaryDataWriter bw, FileWriter w, SoundArchive a, List<string> strings) {

            //Keep track of position.
            long pos = bw.Position;

            //Write data.
            if (File != null) {
                bw.Write((uint)File.FileId);
            } else { bw.Write((uint)0xFFFFFFFF); }
            new Id(SoundTypes.Player, (uint)a.Players.IndexOf(Player)).Write(ref bw);
            bw.Write(Volume);
            bw.Write(RemoteFilter);
            bw.Write((ushort)0);
            w.InitReference(bw, "ToDetRef");

            //3d info offset.
            long threeDeeInfoOffOff = bw.Position;

            //New flags.
            Dictionary<int, uint> f = new Dictionary<int, uint>();

            //Write string data.
            if (a.CreateStrings && Name != null) {
                f.Add(0, (uint)strings.IndexOf(Name));
            }

            //Other flags.
            f.Add(1, (uint)(((byte)PanCurve << 8) | (byte)PanMode));
            f.Add(2, (uint)(((byte)PlayerActorId << 8) | (byte)PlayerPriority));

            //Get 3d info offset offset.
            threeDeeInfoOffOff += (a.CreateStrings && Name != null ? 0x10 : 0xC);
            if (Sound3dInfo != null) {
                f.Add(8, 0);
            }

            //Front bypass.
            f.Add(17, (uint)(IsFrontBypass ? 1 : 0));

            //User parameters.
            if (UserParamsEnabled[0]) { f.Add(31, UserParameter[0]); }
            if (UserParamsEnabled[1]) { f.Add(30, UserParameter[1]); }
            if (UserParamsEnabled[2]) { f.Add(29, UserParameter[2]); }
            if (UserParamsEnabled[3]) { f.Add(28, UserParameter[3]); }

            //Write the flags.
            new FlagParameters(f).Write(ref bw);

            //Write the 3d info.
            if (Sound3dInfo != null) {

                long newPos = bw.Position;
                bw.Position = threeDeeInfoOffOff;
                bw.Write((uint)(newPos - pos));
                bw.Position = newPos;

                //Flags.
                uint tDFlags = 0;
                tDFlags += (uint)(Sound3dInfo.Volume ? 0b1 : 0);
                tDFlags += (uint)(Sound3dInfo.Priority ? 0b10 : 0);
                tDFlags += (uint)(Sound3dInfo.Pan ? 0b100 : 0);
                tDFlags += (uint)(Sound3dInfo.Span ? 0b1000 : 0);
                tDFlags += (uint)(Sound3dInfo.Filter ? 0b10000 : 0);
                bw.Write(tDFlags);

                //Other info.
                bw.Write(Sound3dInfo.AttenuationRate);
                bw.Write((byte)Sound3dInfo.AttenuationCurve);
                bw.Write(Sound3dInfo.DopplerFactor);
                bw.Write((ushort)0);
                bw.Write((uint)0);

                //For 'F' type.
                if (FileWriter.GetWriteModeChar(a.WriteMode) == 'F') {
                    bw.Write((uint)(Sound3dInfo.UnknownFlag ? 1 : 0));
                }

            }

        }

    }

}
