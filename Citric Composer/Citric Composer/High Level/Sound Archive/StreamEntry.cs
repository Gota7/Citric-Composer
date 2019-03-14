using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Stream entry.
    /// </summary>
    public class StreamEntry : SoundInfo {

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

        }

        /// <summary>
        /// Track info.
        /// </summary>
        public List<StreamTrackInfo> Tracks;

        /// <summary>
        /// Track flags to allocate.
        /// </summary>
        public bool[] TrackFlags;

        /// <summary>
        /// Pitch.
        /// </summary>
        public float Pitch = 1;

        /// <summary>
        /// Send value.
        /// </summary>
        public byte[] SendValue = { 127, 0, 0, 0 };

        /// <summary>
        /// Generate prefetch file.
        /// </summary>
        public bool GeneratePrefetchFile;

        /// <summary>
        /// Stream file type.
        /// </summary>
        public EStreamFileType StreamFileType;

        /// <summary>
        /// Stream file type.
        /// </summary>
        public enum EStreamFileType {
            Invalid, Binary, ADTS
        };

    }

}
