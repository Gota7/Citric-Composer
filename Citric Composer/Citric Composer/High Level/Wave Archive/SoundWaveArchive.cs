using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Sound wave archive. It just contains a list of Wave sound files.
    /// </summary>
    public class SoundWaveArchive : IList<Wave>, ISoundFile {

        private WriteMode writeMode;

        public FileWriter.Version Version = new FileWriter.Version(1, 0, 0);

        private List<Wave> waves = new List<Wave>();

        public Wave this[int index] { get => waves[index]; set => waves[index] = value; }

        public int Count => waves.Count;

        public bool IsReadOnly => false;

        public void Add(Wave item) {
            waves.Add(item);
        }

        public void Clear() {
            waves.Clear();
        }

        public bool Contains(Wave item) {
            return waves.Contains(item);
        }

        public void CopyTo(Wave[] array, int arrayIndex) {
            waves.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Wave> GetEnumerator() {
            return waves.GetEnumerator();
        }

        public int IndexOf(Wave item) {
            return IndexOf(item);
        }

        public void Insert(int index, Wave item) {
            waves.Insert(index, item);
        }

        public bool Remove(Wave item) {
            return waves.Remove(item);
        }

        public void RemoveAt(int index) {
            waves.RemoveAt(index);
        }

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(writeMode) + "WAR").ToLower();
        }

        /// <summary>
        /// Write the wave archive.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {

            //Write the data.
            this.writeMode = writeMode;

            //Initialize file.
            FileWriter FileWriter = new FileWriter();
            FileWriter.InitFile(bw, writeMode, "WAR", 2, Version);

            //Info block.
            FileWriter.InitBlock(bw, ReferenceTypes.WAR_Block_Info, "INFO");

            //There is simply just a sized reference table, which will be written to later.
            FileWriter.InitSizedReferenceTable(bw, waves.Count, "WavFiles");

            //Align block to 0x20 bytes.
            FileWriter.Align(bw, 0x20);

            //Close info block.
            FileWriter.CloseBlock(bw);

            //File block.
            FileWriter.InitBlock(bw, ReferenceTypes.WAR_Block_File, "FILE");

            //Align to 0x20 bytes.
            FileWriter.Align(bw, 0x20);

            //Write each wave file, and align.
            int wCount = 0;
            foreach (var w in waves) {

                //Wav file is null.
                if (w == null) {

                    //Add null reference.
                    FileWriter.AddSizedReferenceTableNullReference("WavFiles");

                }

                //Wav file is valid.
                else {

                    //Add the reference.
                    FileWriter.AddSizedReferenceTableReference(bw, ReferenceTypes.General, 0, "WavFiles");

                    //File size.
                    UInt32 fileSize = 0;

                    //Last file.
                    if (wCount == waves.Count - 1) {

                        //Get the proper file size.
                        fileSize = FileWriter.WriteFile(bw, w, 1, writeMode);

                    }
                    
                    //Not last file.
                    else {

                        //Get the proper file size.
                        fileSize = FileWriter.WriteFile(bw, w, 0x20, writeMode);

                    }

                    //Adjust the file size.
                    FileWriter.AdjustSizedReferenceTableSize(fileSize, "WavFiles");

                }

                wCount++;
                
            }

            //All the references are written, close the table.
            FileWriter.CloseSizedReferenceTable(bw, "WavFiles");


            //Close file block.
            FileWriter.CloseBlock(bw);

            //Close the file.
            FileWriter.CloseFile(bw);

        }

        /// <summary>
        /// Write the wave archive.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(writeMode, bw);
        }

        /// <summary>
        /// Read the wave archive.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Open the file.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out writeMode, out Version);

            //Open the info block.
            FileReader.OpenBlock(br, 0);

            //Gather references.
            FileReader.OpenSizedReferenceTable(br, "WavFiles");

            //Close the info block.
            FileReader.CloseBlock(br);

            //Open the file block.
            FileReader.OpenBlock(br, 1);

            //Go through each reference, and add the sound file.
            waves = new List<Wave>();
            for (int i = 0; i < FileReader.SizedReferenceTableCount("WavFiles"); i++) {

                //Wave is null.
                if (FileReader.SizedReferenceTableReferenceIsNull(i, "WavFiles")) {
                    waves.Add(null);
                }
                
                //Wave is valid.
                else {

                    //Warp to reference.
                    FileReader.SizedReferenceTableJumpToReference(br, i, "WavFiles");

                    //Read the file.
                    waves.Add((Wave)SoundArchiveReader.ReadFile(br.ReadBytes((int)FileReader.SizedReferenceTableGetSize(i, "WavFiles"))));

                }
                
            }

            //Close the sized reference table.
            FileReader.CloseSizedReferenceTable("WavFiles");

            //Close the file block.
            FileReader.CloseBlock(br);

            //Close the file.
            FileReader.CloseFile(br);

        }

        IEnumerator IEnumerable.GetEnumerator() {
            return waves.GetEnumerator();
        }

    }

}
