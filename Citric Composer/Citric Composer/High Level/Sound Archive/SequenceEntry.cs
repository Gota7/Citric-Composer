using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sequence entry.
    /// </summary>
    public class SequenceEntry : SoundInfo {

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
        /// If a channel flag is enabled.
        /// </summary>
        /// <param name="index">Flag index.</param>
        /// <returns>If the flag is enabled.</returns>
        public bool ChannelFlagEnabled(int index) {
            return (m_channelFlags & (0b1 << index)) > 0;
        }

        /// <summary>
        /// Set the status of a channel flag.
        /// </summary>
        /// <param name="index">Flag to change the status of.</param>
        /// <param name="set">Whether to set it or not.</param>
        public void SetChannelFlag(int index, bool set) {
            if (!set) {
                if (ChannelFlagEnabled(index)) {
                    m_channelFlags -= (uint)(0b1 << index);
                }
            } else {
                if (!ChannelFlagEnabled(index)) {
                    m_channelFlags += (uint)(0b1 << index);
                }
            }
        }

        /// <summary>
        /// Set the channel flags.
        /// </summary>
        /// <param name="flags">Flags to set.</param>
        public void SetFlags(uint flags) {
            m_channelFlags = flags;
        }

        /// <summary>
        /// Get the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        public uint GetFlags() {
            return m_channelFlags;
        }

        /// <summary>
        /// Private flag value. There appear to be only 16 used.
        /// </summary>
        private uint m_channelFlags;

        /// <summary>
        /// Banks to use. 4 available.
        /// </summary>
        public BankEntry[] Banks;

        /// <summary>
        /// Start offset.
        /// </summary>
        public UInt32 StartOffset;

        /// <summary>
        /// Channel priority.
        /// </summary>
        public byte ChannelPriority;

        /// <summary>
        /// Is release priority.
        /// </summary>
        public bool IsReleasePriority;

    }

}
