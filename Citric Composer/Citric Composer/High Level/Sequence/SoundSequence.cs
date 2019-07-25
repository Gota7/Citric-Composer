using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SequenceDataLib;
using Syroot.BinaryData;

namespace CitraFileLoader {

    /// <summary>
    /// Sound sequence.
    /// </summary>
    public class SoundSequence : ISoundFile {

        /// <summary>
        /// Write mode.
        /// </summary>
        private WriteMode writeMode;

        /// <summary>
        /// File version.
        /// </summary>
        public FileWriter.Version Version;

        /// <summary>
        /// Actual sequence information.
        /// </summary>
        public SequenceData SequenceData;

        /// <summary>
        /// Labels.
        /// </summary>
        public List<SequenceLabel> Labels;

        /// <summary>
        /// Only update when needed, so that I don't break everything.
        /// </summary>
        private byte[] goodData;

        /// <summary>
        /// Get the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension() {
            return ("B" + FileWriter.GetWriteModeChar(writeMode) + "SEQ").ToLower();
        }

        /// <summary>
        /// Read the sequence data.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Open file.
            FileReader FileReader = new FileReader();
            FileReader.OpenFile(br, out writeMode, out Version);

            //Data block.
            FileReader.OpenBlock(br, 0);

            //Get size.
            br.Position -= 4;
            uint size = br.ReadUInt32();

            //Read the sequence data. BE for WiiU and 3ds, LE for NX. Must therefore write a parser.
            List<byte> tempSeqData = br.ReadBytes((int)size - 8).ToList();

            //Trim data.
            bool foundEnd = false;
            while (!foundEnd) {

                //Remove padding stuff.
                if (tempSeqData[tempSeqData.Count - 1] == 0) {
                    tempSeqData.RemoveAt(tempSeqData.Count - 1);
                }

                //Found end.
                else {
                    foundEnd = true;
                }

            }

            //Close data block.
            FileReader.CloseBlock(br);

            //Label block.
            FileReader.OpenBlock(br, 1);

            //Open the reference table.
            FileReader.OpenReferenceTable(br, "Labels");

            //Go through all the references.
            Labels = new List<SequenceLabel>();
            for (int i = 0; i < FileReader.ReferenceTableCount("Labels"); i++) {

                //Reference is null.
                if (FileReader.ReferenceTableReferenceIsNull(i, "Labels")) {
                    Labels.Add(null);
                }
                
                //Label exist.
                else {

                    //Jump to label.
                    FileReader.ReferenceTableJumpToReference(br, i, "Labels");

                    //Start label info structure.
                    FileReader.StartStructure(br);

                    //Get reference to data.
                    FileReader.OpenReference(br, "ToSeqData");

                    //String size.
                    uint stringSize = br.ReadUInt32();

                    //String data.
                    string labelNow = new string(br.ReadChars((int)stringSize));

                    //Get label info.
                    Labels.Add(new SequenceLabel() { Offset = FileReader.ReferenceGetOffset("ToSeqData"), Label = labelNow });

                    //Close reference.
                    FileReader.CloseReference("ToSeqData");

                    //End label info structure.
                    FileReader.EndStructure();

                }

            }

            //Close the reference table.
            FileReader.CloseReferenceTable("Labels");

            //Close label block.
            FileReader.CloseBlock(br);

            //Convert labels to dictionary.
            Dictionary<string, int> publicLabels = new Dictionary<string, int>();
            foreach (var e in Labels) {
                if (e != null) {
                    publicLabels.Add(e.Label, e.Offset);
                }
            }

            //Set sequence data.
            SequenceData = new SequenceData();
            SequenceData.PublicLabelOffsets = publicLabels;
            Syroot.BinaryData.ByteOrder bo = Syroot.BinaryData.ByteOrder.BigEndian;
            if (writeMode == WriteMode.NX || writeMode == WriteMode.C_BE) {
                bo = Syroot.BinaryData.ByteOrder.LittleEndian;
            }
            SequenceData.Read(tempSeqData.ToArray(), bo);
            goodData = tempSeqData.ToArray();

            //Close file.
            FileReader.CloseFile(br);

        }

        /// <summary>
        /// Update sequence data.
        /// </summary>
        public void UpdateSeqData(WriteMode writeMode) {

            //Write sequence data.
            Syroot.BinaryData.ByteOrder bo = Syroot.BinaryData.ByteOrder.BigEndian;
            if (writeMode == WriteMode.NX || writeMode == WriteMode.C_BE) {
                bo = Syroot.BinaryData.ByteOrder.LittleEndian;
            }
            goodData = SequenceData.ToBytes(bo);

        }

        /// <summary>
        /// Write the sequence file.
        /// </summary>
        /// <param name="writeMode">Write mode.</param>
        /// <param name="bw">The writer.</param>
        public void Write(WriteMode writeMode, BinaryDataWriter bw) {

            //Update sequence data if write mode changed.
            if (this.writeMode != writeMode) {
                UpdateSeqData(writeMode);
            }

            //Set write mode.
            this.writeMode = writeMode;

            //Init file.
            FileWriter FileWriter = new FileWriter();
            FileWriter.InitFile(bw, writeMode, "SEQ", 2, Version);

            //Data block.
            FileWriter.InitBlock(bw, ReferenceTypes.SEQ_Block_Data, "DATA");

            //Write sequence data.
            Syroot.BinaryData.ByteOrder bo = Syroot.BinaryData.ByteOrder.BigEndian;
            if (writeMode == WriteMode.NX || writeMode == WriteMode.C_BE) {
                bo = Syroot.BinaryData.ByteOrder.LittleEndian;
            }
            //bw.Write(SequenceData.ToBytes(bo));
            bw.Write(goodData);

            //Align.
            FileWriter.Align(bw, 0x20);

            //Close data block.
            FileWriter.CloseBlock(bw);

            //Label block.
            FileWriter.InitBlock(bw, ReferenceTypes.SEQ_Block_Label, "LABL");

            //Label table.
            FileWriter.InitReferenceTable(bw, Labels.Count, "Labels");

            //Write each label.
            foreach (SequenceLabel l in Labels) {

                //Label is null.
                if (l == null) {
                    FileWriter.AddReferenceTableNullReference("Labels");
                }

                //Not null.
                else {

                    //Add reference.
                    FileWriter.AddReferenceTableReference(bw, ReferenceTypes.SEQ_LabelInfo, "Labels");

                    //Write data.
                    bw.Write(ReferenceTypes.General);
                    bw.Write((UInt16)0);
                    bw.Write(l.Offset);
                    bw.Write((UInt32)l.Label.Length);
                    bw.Write(l.Label.ToCharArray());

                    //Add null terminator.
                    bw.Write('\0');

                    //Align to 4 bytes.
                    FileWriter.Align(bw, 4);

                }

            }

            //Close labels.
            FileWriter.CloseReferenceTable(bw, "Labels");

            //Align.
            FileWriter.Align(bw, 0x20);

            //Close label block.
            FileWriter.CloseBlock(bw);

            //Close file.
            FileWriter.CloseFile(bw);

        }

        /// <summary>
        /// Write the sequence file.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {
            Write(writeMode, bw);
        }

    }

}
