using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Sound file.
    /// </summary>
    public interface ISoundFile {

        /// <summary>
        /// Write a sound file in it's correct form.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        void Write(WriteMode writeMode, BinaryDataWriter bw);
        void Write(BinaryDataWriter bw);

        /// <summary>
        /// Read a sound file.
        /// </summary>
        /// <param name="br">The reader.</param>
        void Read(BinaryDataReader br);

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>Get the extension.</returns>
        string GetExtension();

    }

}
