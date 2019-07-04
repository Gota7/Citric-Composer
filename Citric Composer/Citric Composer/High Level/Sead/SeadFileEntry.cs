using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sead file entry.
    /// </summary>
    public class SeadFileEntry {

        /// <summary>
        /// Create a new sead file entry.
        /// </summary>
        /// <param name="hash">Name hash.</param>
        /// <param name="name">Name of the file.</param>
        /// <param name="file">File data.</param>
        public SeadFileEntry(uint hash, string name, byte[] file) {
            m_hash = hash;
            Name = name;
            File = file;
        }

        /// <summary>
        /// File name.
        /// </summary>
        public string Name;

        /// <summary>
        /// File.
        /// </summary>
        public byte[] File;

        /// <summary>
        /// The file hash.
        /// </summary>
        private uint m_hash;

        /// <summary>
        /// Get the name hash.
        /// </summary>
        /// <param name="signedCharMode">If the hash is written using a signed char.</param>
        /// <param name="hashKey">Hash key.</param>
        /// <returns>The name hash.</returns>
        public uint Hash(bool signedCharMode, uint hashKey) {

            //If null name, return existing hash.
            if (Name == null) {
                return m_hash;
            }

            //Generate the name hash.
            else {
                byte[] rawName = Encoding.UTF8.GetBytes(Name);
                uint ret = 0;
                for (int i = 0; i < rawName.Length; i++) {
                    int val = rawName[i];
                    if (signedCharMode) { val = (sbyte)rawName[i]; }
                    ret = (uint)(ret * hashKey + val);
                }
                return ret;
            }

        }

    }

}
