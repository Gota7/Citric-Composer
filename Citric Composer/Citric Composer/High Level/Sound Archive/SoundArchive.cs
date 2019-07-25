using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Sound archive.
    /// </summary>
    public class SoundArchive : ISoundFile {

        /// <summary>
        /// File path.
        /// </summary>
        public static string FilePath = "";

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

        /// <summary>
        /// New file entry type.
        /// </summary>
        public enum NewFileEntryType {
            Stream, WaveSoundData, Sequence, Bank, WaveArchive, Group, Prefetch
        }

        /// <summary>
        /// Write this to an SDK project.
        /// </summary>
        /// <param name="path">Path to write this to.</param>
        public void WriteSDKProject(string path) {

            //Get project info.
            string projectName = Path.GetFileNameWithoutExtension(path);
            string dir = Path.GetDirectoryName(path);

            //Write project files.
            SdkExporter.WriteSpj(dir, projectName, this, WriteMode);
            SdkExporter.WriteSst(dir, projectName, this, WriteMode);
            SdkExporter.WriteFiles(dir, this, WriteMode);

        }

        /// <summary>
        /// Get extension.
        /// </summary>
        /// <returns></returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(WriteMode) + "SAR").ToLower();
        }

        /// <summary>
        /// Read file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {
            var s = SoundArchiveReader.ReadSoundArchive(FilePath);
            Streams = s.Streams;
            WaveSoundDatas = s.WaveSoundDatas;
            Sequences = s.Sequences;
            SoundSets = s.SoundSets;
            Banks = s.Banks;
            WaveArchives = s.WaveArchives;
            Groups = s.Groups;
            Players = s.Players;
            Files = s.Files;
            MaxSequences = s.MaxSequences;
            MaxSequenceTracks = s.MaxSequenceTracks;
            MaxStreamSounds = s.MaxStreamSounds;
            MaxStreamTracks = s.MaxStreamTracks;
            MaxStreamChannels = s.MaxStreamChannels;
            MaxWaveSounds = s.MaxWaveSounds;
            MaxWaveTracks = s.MaxWaveTracks;
            StreamBufferTimes = s.StreamBufferTimes;
            Options = s.Options;
            WriteMode = s.WriteMode;
            Version = s.Version;
            CreateStrings = s.CreateStrings;
        }

        /// <summary>
        /// Write file.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {
            WriteMode = writeMode;
            SoundArchiveWriter.WriteSoundArchive(this, Path.GetDirectoryName(FilePath), FilePath);
        }

        /// <summary>
        /// Write file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(WriteMode, bw);
        }

        public bool FileUnique(int fileIndex) {
            return true;
        }

        /// <summary>
        /// Add a new sound file, and return it.
        /// </summary>
        /// <param name="type">New file entry type.</param>
        /// <param name="lastEntry">Last entry.</param>
        /// <param name="file">File to add.</param>
        /// <param name="keepReference">Whether or not to keep the reference to the ISoundFile provided.</param>
        /// <param name="externalPath">Use this if the file is a stream.</param>
        /// <returns>The new file.</returns>
        public SoundFile<ISoundFile> AddNewFile(NewFileEntryType type, int lastEntry, ISoundFile file, bool keepReference = false, string externalPath = null) {

            //Get proper file.
            ISoundFile f = file;
            if (f != null && !keepReference) {
                f = SoundArchiveReader.ReadFile(SoundArchiveWriter.WriteFile(f));
            }

            int index = -1;
            switch (type) {

                //Stream.
                case NewFileEntryType.Stream:
                    break;

                //Wave sound data.
                case NewFileEntryType.WaveSoundData:
                    break;

                //Sequence.
                case NewFileEntryType.Sequence:
                    break;

                //Bank.
                case NewFileEntryType.Bank:
                    break;

                //Wave archive.
                case NewFileEntryType.WaveArchive:
                    break;

                //Group.
                case NewFileEntryType.Group:
                    break;

            }

            //Insert file at the proper index.
            var filef = new SoundFile<ISoundFile>() { ExternalFileName = externalPath, File = f, FileType = (externalPath == null ? EFileType.Internal : EFileType.External) };
            if (externalPath != null) {
                filef.BackupExtension = "stm";
            }
            Files.Insert(index, filef);

            //Recreate file Ids.
            RecreateFileIds();

            //Programmed to fail if it messes up.
            return Files[index];

        }

        /// <summary>
        /// Recreate file id.
        /// </summary>
        public void RecreateFileIds() {
            for (int i = 0; i < Files.Count; i++) {
                Files[i].FileId = i;
            }
        }

        /// <summary>
        /// Get file names.
        /// </summary>
        public void RefreshFileNames() {

            //Go through each file.
            for (int i = 0; i < Files.Count; i++) {

                //Get the filename.
                foreach (var e in Streams) {
                    if (e.File != null) {
                        if (e.File.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "stm";
                            break;
                        }
                    }
                    if (e.PrefetchFile != null) {
                        if (e.PrefetchFile.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "stp";
                            break;
                        }
                    }
                }
                foreach (var e in WaveSoundDatas) {
                    if (e.File != null) {
                        if (e.File.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "wsd";
                            break;
                        }
                    }
                }
                foreach (var e in Sequences) {
                    if (e.File != null) {
                        if (e.File.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "seq";
                            break;
                        }
                    }
                }
                foreach (var e in WaveArchives) {
                    if (e.File != null) {
                        if (e.File.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "war";
                            break;
                        }
                    }
                }
                foreach (var e in Banks) {
                    if (e.File != null) {
                        if (e.File.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "bnk";
                            break;
                        }
                    }
                }
                foreach (var e in Groups) {
                    if (e.File != null) {
                        if (e.File.FileId == Files[i].FileId) {
                            Files[i].FileName = e.Name;
                            Files[i].BackupExtension = "b" + (WriteMode == WriteMode.NX || WriteMode == WriteMode.Cafe ? "f" : "c") + "grp";
                            break;
                        }
                    }
                }

            }

        }

    }

}
