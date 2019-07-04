using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sound archive.
    /// </summary>
    public class SoundArchive {

        /// <summary>
        /// Streams.
        /// </summary>
        public List<StreamEntry> Streams;

        /// <summary>
        /// Sequences.
        /// </summary>
        public List<SequenceEntry> Sequences;

        /// <summary>
        /// Wave sound datas.
        /// </summary>
        public List<WaveSoundDataEntry> WaveSoundDatas;

        /// <summary>
        /// Sound sets.
        /// </summary>
        public List<SoundSetEntry> SoundSets;

        /// <summary>
        /// Banks.
        /// </summary>
        public List<BankEntry> Banks;

        /// <summary>
        /// Wave archives.
        /// </summary>
        public List<WaveArchiveEntry> WaveArchives;

        /// <summary>
        /// Groups.
        /// </summary>
        public List<GroupEntry> Groups;

        /// <summary>
        /// Players.
        /// </summary>
        public List<PlayerEntry> Players;

        /// <summary>
        /// Files.
        /// </summary>
        public List<SoundFile<ISoundFile>> Files;

        /// <summary>
        /// Max number of sequences.
        /// </summary>
        public UInt16 MaxSequences = 64;

        /// <summary>
        /// Max number of SEQ tracks.
        /// </summary>
        public UInt16 MaxSequenceTracks = 64;

        /// <summary>
        /// Max number of stream sounds.
        /// </summary>
        public UInt16 MaxStreamSounds = 4;

        /// <summary>
        /// Max number of stream tracks.
        /// </summary>
        public UInt16 MaxStreamTracks = 4;

        /// <summary>
        /// Max number of stream channels.
        /// </summary>
        public UInt16 MaxStreamChannels = 8;

        /// <summary>
        /// Max number of wave sounds.
        /// </summary>
        public UInt16 MaxWaveSounds = 64;

        /// <summary>
        /// Max number of wave tracks.
        /// </summary>
        public UInt16 MaxWaveTracks = 4;

        /// <summary>
        /// Who tf knows what that means.
        /// </summary>
        public byte StreamBufferTimes;

        /// <summary>
        /// Probably useless, it's "provisional".
        /// </summary>
        public UInt32 Options;

        /// <summary>
        /// Write mode.
        /// </summary>
        public WriteMode WriteMode;

        /// <summary>
        /// File version.
        /// </summary>
        public FileWriter.Version Version;

        /// <summary>
        /// Whether or not to include the names.
        /// </summary>
        public bool CreateStrings;

    }

}
