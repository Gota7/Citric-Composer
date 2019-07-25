using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.Crc32;
using Syroot.BinaryData;

namespace SolarFileLoader {

    /// <summary>
    /// Loop asset list.
    /// </summary>
    public class LoopAssetList : AudioFile {

        /// <summary>
        /// Names of assets that loop.
        /// </summary>
        public List<string> LoopingAudioNames = new List<string>();

        /// <summary>
        /// Create a loop asset list from a list of loop audio names, without the extension.
        /// </summary>
        /// <param name="loopingAudioNames">The looping audio names.</param>
        public LoopAssetList(List<string> loopingAudioNames) {
            Magic = "BLAL";
            Version = new Version(0, 1);
            LoopingAudioNames = loopingAudioNames;
        }

        /// <summary>
        /// Read a file.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void Read(BinaryDataReader br) {
            throw new Exception("Reading a loop asset list is stupid!");
        }

        /// <summary>
        /// Write a file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void Write(BinaryDataWriter bw) {

            //Create the hash list.
            List<uint> hashes = new List<uint>();
            foreach (var name in LoopingAudioNames) {
                hashes.Add(Crc32Algorithm.Compute(Encoding.ASCII.GetBytes(name)));
            }

            //Sort hash list.
            hashes.OrderBy(o => o);

            //Write file data.
            WriteMagic(bw);
            WriteByteOrder(bw);
            WriteVersion(bw);
            bw.Write((uint)hashes.Count);
            bw.Write(hashes.ToArray());

        }

    }

}
