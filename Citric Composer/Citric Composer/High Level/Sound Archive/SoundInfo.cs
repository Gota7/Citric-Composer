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
        /// New sound info.
        /// </summary>
        public SoundInfo() {

            //User parameters.
            UserParameter = new uint[4];

            //User parameters enabled.
            UserParamsEnabled = new bool[4];

        }

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
        /// Player number.
        /// </summary>
        public int Player;

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
        public UInt32[] UserParameter;

        /// <summary>
        /// User parameters enabled.
        /// </summary>
        public bool[] UserParamsEnabled;

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

    }

}
