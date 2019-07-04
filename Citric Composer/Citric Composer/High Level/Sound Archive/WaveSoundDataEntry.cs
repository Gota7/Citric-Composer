using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Wave sound data entry.
    /// </summary>
    public class WaveSoundDataEntry : SoundInfo {

        /// <summary>
        /// Load sound info.
        /// </summary>
        /// <param name="s">Sound info to load.</param>
        public void LoadSoundInfo(SoundInfo s) {

            //Load info.
            Name = s.Name;
            File = s.File;
            Sound3dInfo = s.Sound3dInfo;
            Player = s.Player;
            Volume = s.Volume;
            RemoteFilter = s.RemoteFilter;
            PanMode = s.PanMode;
            PanCurve = s.PanCurve;
            PlayerActorId = s.PlayerActorId;
            PlayerPriority = s.PlayerPriority;
            IsFrontBypass = s.IsFrontBypass;
            UserParameter = s.UserParameter;
            UserParamsEnabled = s.UserParamsEnabled;

        }

        /// <summary>
        /// What wave in the WSD file.
        /// </summary>
        public int WaveIndex;

        /// <summary>
        /// How many tracks to allocate.
        /// </summary>
        public int AllocateTrackCount;

        /// <summary>
        /// Channel priority.
        /// </summary>
        public byte ChannelPriority;

        /// <summary>
        /// Fix priority at release.
        /// </summary>
        public bool IsReleasePriority;

    }

}
