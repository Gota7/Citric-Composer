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
        /// If a track flag is enabled.
        /// </summary>
        /// <param name="index">Flag index.</param>
        /// <returns>If the flag is enabled.</returns>
        public bool TrackFlagEnabled(int index) {
            return (m_trackFlags & (0b1 << index)) > 0;
        }

        /// <summary>
        /// Set the status of a track flag.
        /// </summary>
        /// <param name="index">Flag to change the status of.</param>
        /// <param name="set">Whether to set it or not.</param>
        public void SetChannelFlag(int index, bool set) {
            if (!set) {
                if (TrackFlagEnabled(index)) {
                    m_trackFlags -= (ushort)(0b1 << index);
                }
            } else {
                if (!TrackFlagEnabled(index)) {
                    m_trackFlags += (ushort)(0b1 << index);
                }
            }
        }

        /// <summary>
        /// Set the track flags.
        /// </summary>
        /// <param name="flags">Flags to set.</param>
        public void SetFlags(ushort flags) {
            m_trackFlags = flags;
        }

        /// <summary>
        /// Get the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        public ushort GetFlags() {
            return m_trackFlags;
        }

        /// <summary>
        /// Private flag values.
        /// </summary>
        private ushort m_trackFlags;

        /// <summary>
        /// Number of channels to allocate.
        /// </summary>
        public UInt16 AllocateChannelCount;

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
        /// Prefetch file.
        /// </summary>
        public SoundFile<ISoundFile> PrefetchFile;

        /// <summary>
        /// Loop start frame.
        /// </summary>
        public UInt32 LoopStartFrame;

        /// <summary>
        /// Loop end frame.
        /// </summary>
        public UInt32 LoopEndFrame;

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
