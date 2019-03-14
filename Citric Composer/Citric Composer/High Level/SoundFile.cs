using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sub-file found in a group, archive, etc.
    /// </summary>
    public class SoundFile<ISoundFile> {

        /// <summary>
        /// Create groups.
        /// </summary>
        public SoundFile() {
            Reference = null;
            ReferencedBy = new List<SoundFile<ISoundFile>>();
            FileName = null;
        }

        /// <summary>
        /// If the file is in another structure, embed it there.
        /// </summary>
        public bool Embed;

        /// <summary>
        /// If this isn't a file directly in the SoundArchive, point to the actual sound file here.
        /// </summary>
        public SoundFile<CitraFileLoader.ISoundFile> Reference;

        /// <summary>
        /// Other soundfiles that use this file.
        /// </summary>
        public List<SoundFile<ISoundFile>> ReferencedBy;

        /// <summary>
        /// Get the file extension.
        /// </summary>
        public string FileExtension {

            get {

                string ret = "bin";
                if (File != null) {
                    ret = File.GetExtension();
                }
                if (ret.Equals("bin")) {
                    ret = BackupExtension;
                }
                return ret;

            }

        }

        /// <summary>
        /// Found by something that has this file referenced.
        /// </summary>
        public string FileName {
            get {
                if (Reference == null) {
                    return m_fileName;
                } else {
                    return Reference.FileName;
                }
            }
            set {
                if (Reference == null) {
                    m_fileName = value;
                } else {
                    Reference.FileName = value;
                }
            }
        }
        private String m_fileName;

        /// <summary>
        /// Backup extension.
        /// </summary>
        public string BackupExtension {
            get {
                if (Reference == null) {
                    return m_backupExtension;
                } else {
                    return Reference.m_backupExtension;
                }
            }
            set {
                if (Reference == null) {
                    m_backupExtension = value;
                } else {
                    Reference.m_backupExtension = value;
                }
            }
        }
        private string m_backupExtension = "bin";

        /// <summary>
        /// Only used for recreating a SoundArchive, or editing a sub-file in independent mode. ALL UNKNOWN FILES KEEP THEIR FILE ID.
        /// </summary>
        public int FileId {
            get {
                if (Reference == null) {
                    return m_fileId;
                } else {
                    return Reference.FileId;
                }
            }
            set {
                if (Reference == null) {
                    m_fileId = value;
                } else {
                    Reference.FileId = value;
                }
            }
        }
        private int m_fileId;

        /// <summary>
        /// External file name.
        /// </summary>
        public string ExternalFileName {
            get {
                if (Reference == null) {
                    return m_externalFileName;
                } else {
                    return Reference.ExternalFileName;
                }
            }
            set {
                if (Reference == null) {
                    m_externalFileName = value;
                } else {
                    Reference.ExternalFileName = value;
                }
            }
        }
        private string m_externalFileName;

        /// <summary>
        /// Actual sound file.
        /// </summary>
        public CitraFileLoader.ISoundFile File {
            get {
                if (Reference == null) {
                    return m_file;
                } else {
                    return Reference.File;
                }
            }
            set {
                if (Reference == null) {
                    m_file = value;
                } else {
                    Reference.File = value;
                }
            }
        }

        private CitraFileLoader.ISoundFile m_file;

        /// <summary>
        /// Groups the file is in.
        /// </summary>
        public List<int> Groups {
            get {
                if (Reference == null) {
                    return m_groups;
                } else {
                    return Reference.Groups;
                }
            }
            set {
                if (Reference == null) {
                    m_groups = value;
                } else {
                    Reference.Groups = value;
                }
            }
        }
        private List<int> m_groups;

        /// <summary>
        /// Type of the file.
        /// </summary>
        public EFileType FileType {
            get {
                if (Reference == null) {
                    return m_fileType;
                } else {
                    return Reference.FileType;
                }
            }
            set {
                if (Reference != null) {
                    Reference.FileType = value;
                } else {
                    m_fileType = value;
                }
            }
        }
        private EFileType m_fileType;

    }

    /// <summary>
    /// File type.
    /// </summary>
    public enum EFileType {
        Null, Internal, External
    }

}
