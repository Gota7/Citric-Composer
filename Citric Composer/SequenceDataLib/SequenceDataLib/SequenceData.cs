using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Sequence data.
    /// </summary>
    public partial class SequenceData {

        /// <summary>
        /// Sequence commands.
        /// </summary>
        public List<SequenceCommand> Commands = new List<SequenceCommand>();

        /// <summary>
        /// Private offsets to labels. Number is first sequence index for label. Are recreated when reading and before saving, to help write seq files easier.
        /// </summary>
        public List<int> PrivateLabelOffsets = new List<int>();

        /// <summary>
        /// Public offsets to labels. Referenced by a name, the number returns what commands the label starts on.
        /// </summary>
        public Dictionary<string, int> PublicLabelOffsets = new Dictionary<string, int>();

        /// <summary>
        /// Read the sequence data.
        /// </summary>
        /// <param name="data">Sequence data.</param>
        /// <param name="byteOrder">Byte order.</param>
        public void Read(byte[] data, ByteOrder byteOrder) {

            //New reader.
            MemoryStream src = new MemoryStream(data);
            BinaryDataReader br = new BinaryDataReader(src) { ByteOrder = byteOrder };

            //Read the data.
            Commands = new List<SequenceCommand>();
            bool ended = false;
            while (br.Position < data.Length && !ended) {

                //Add command.
                var s = SequenceCommand.Read(br);
                Commands.Add(s);

                //If the command is end, and the rest is just padding.
                if (s as FinCommand != null) {
                    long currPos = br.Position;
                    byte[] raw = br.ReadBytes((int)(data.Length - currPos));
                    if (raw.Length > 0) {
                        if (raw.Max() == 0) {
                            ended = true;
                        }
                    } else { ended = true; }
                    br.Position = currPos;
                }

            }

            //Go ahead and convert the public offsets to command indices.
            for (int i = 0; i < PublicLabelOffsets.Count; i++) {

                //Convert the offset to an index.
                PublicLabelOffsets[PublicLabelOffsets.ElementAt(i).Key] = OffsetToCommandIndex(Commands, PublicLabelOffsets.ElementAt(i).Value);

            }

            //Fix control commands.
            for (int i = 0; i < Commands.Count; i++) {

                //Make sure the command is a control command.
                if ((CommandType)Commands[i].Identifier == CommandType.Call || (CommandType)Commands[i].Identifier == CommandType.Jump || (CommandType)Commands[i].Identifier == CommandType.OpenTrack || (CommandType)Commands[i].Identifier == CommandType.If) {

                    if ((CommandType)Commands[i].Identifier == CommandType.If) {

                        //The first parameter is going to be the offset.
                        int offset = 0;
                        if ((CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.Call || (CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.Jump) {
                            offset = (int)((UInt24)((IfCommand)Commands[i]).SequenceCommand.Parameters[0]).Value;
                        } else if ((CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.OpenTrack) {
                            offset = (int)((UInt24)((IfCommand)Commands[i]).SequenceCommand.Parameters[1]).Value;
                        }

                        //Get the index.
                        int index = OffsetToCommandIndex(Commands, offset);

                        //Set the property to this.
                        if ((CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.Call || (CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.Jump) {
                            ((UInt24)((IfCommand)Commands[i]).SequenceCommand.Parameters[0]).Value = (uint)index;
                        } else if ((CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.OpenTrack) {
                            ((UInt24)((IfCommand)Commands[i]).SequenceCommand.Parameters[1]).Value = (uint)index;
                        }

                    } else {

                        //The first parameter is going to be the offset.
                        int offset = 0;
                        if ((CommandType)Commands[i].Identifier == CommandType.Call || (CommandType)Commands[i].Identifier == CommandType.Jump) {
                            offset = (int)((UInt24)Commands[i].Parameters[0]).Value;
                        } else {
                            offset = (int)((UInt24)Commands[i].Parameters[1]).Value;
                        }

                        //Get the index.
                        int index = OffsetToCommandIndex(Commands, offset);

                        //Set the property to this.
                        if ((CommandType)Commands[i].Identifier == CommandType.Call || (CommandType)Commands[i].Identifier == CommandType.Jump) {
                            ((UInt24)Commands[i].Parameters[0]).Value = (uint)index;
                        } else {
                            ((UInt24)Commands[i].Parameters[1]).Value = (uint)index;
                        }

                    }

                }

            }

            //Update private offsets.
            RefreshPrivateOffsets();

            //Free.
            br.Dispose();

        }

        /// <summary>
        /// Refresh private offsets.
        /// </summary>
        public void RefreshPrivateOffsets() {

            //Fix offsets, and create proper labels.
            PrivateLabelOffsets = new List<int>();
            for (int i = 0; i < Commands.Count; i++) {

                //Make sure the command is a control command.
                if ((CommandType)Commands[i].Identifier == CommandType.Call || (CommandType)Commands[i].Identifier == CommandType.Jump || (CommandType)Commands[i].Identifier == CommandType.OpenTrack || (CommandType)Commands[i].Identifier == CommandType.If) {

                    //Index in the commands list.
                    int index = 0;
                    if ((CommandType)Commands[i].Identifier == CommandType.Call || (CommandType)Commands[i].Identifier == CommandType.Jump) {
                        index = (int)((UInt24)Commands[i].Parameters[0]).Value;
                    } else if ((CommandType)Commands[i].Identifier != CommandType.If) {
                        index = (int)((UInt24)Commands[i].Parameters[1]).Value;
                    }

                    //If type.
                    if ((CommandType)Commands[i].Identifier == CommandType.If) {
                        if ((CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.Call || (CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.Jump) {
                            index = (int)((UInt24)((IfCommand)Commands[i]).SequenceCommand.Parameters[0]).Value;
                        } else if ((CommandType)((IfCommand)Commands[i]).SequenceCommand.Identifier == CommandType.OpenTrack) {
                            index = (int)((UInt24)((IfCommand)Commands[i]).SequenceCommand.Parameters[1]).Value;
                        }
                    }

                    //Add the offset to private labels if not in public.
                    if (!PublicLabelOffsets.Values.Contains(index) && !PrivateLabelOffsets.Contains(index)) {
                        PrivateLabelOffsets.Add(index);
                    }

                }

            }

        }

        /// <summary>
        /// Convert an offset to a command index.
        /// </summary>
        /// <param name="commands">Commands.</param>
        /// <param name="offset">The command offset.</param>
        /// <returns>The command index.</returns>
        public static int OffsetToCommandIndex(List<SequenceCommand> commands, int offset) {

            //Read commands until the offset is 
            int size = 0;
            int commandIndex = 0;
            while (size < offset) {

                //Increase size.
                size += commands[commandIndex].ByteSize;

                //Increment command index.
                commandIndex++;

            }

            //Return the index.
            return commandIndex;

        }

        /// <summary>
        /// Convert a command index to an offset.
        /// </summary>
        /// <param name="commands">Commands.</param>
        /// <param name="commandIndex">The command index.</param>
        /// <returns>The offset.</returns>
        public static int CommandIndexToOffset(List<SequenceCommand> commands, int commandIndex) {

            //Size.
            int size = 0;
            for (int i = 0; i < commandIndex; i++) {
                size += commands[i].ByteSize;
            }

            //Return the offset.
            return size;

        }

        /// <summary>
        /// Convert the sequence data to bytes.
        /// </summary>
        /// <param name="byteOrder">Byte order.</param>
        /// <returns>The sequence data.</returns>
        public byte[] ToBytes(ByteOrder byteOrder) {

            //New writer.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);
            bw.ByteOrder = byteOrder;

            //Write each command.
            foreach (var c in Commands) {
                c.Write(bw, Commands);
            }

            //Free.
            byte[] ret = o.ToArray();
            bw.Dispose();
            return ret;

        }

        /// <summary>
        /// Convert the sequence files.
        /// </summary>
        /// <param name="name">Name of the sequence.</param>
        /// <param name="type">Type of the file.</param>
        /// <returns>The sequence file.</returns>
        public string[] ToSeqFile(string name = "Sequence", string type = "F") {

            //New sequence file.
            List<string> seq = new List<string>();

            //Header.
            seq.Add(";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;");
            seq.Add(";");
            seq.Add("; " + name + "." +type.ToLower() + "seq");
            seq.Add(";     Generated By Citric Composer");
            seq.Add(";");
            seq.Add(";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;");

            //Write each command.
            for (int i = 0; i < Commands.Count; i++) {
            
                //Write public label if exists.
                foreach (KeyValuePair<string, int> k in PublicLabelOffsets) {
                    if (k.Value == i) {
                        seq.Add("\n" + k.Key + ":");
                    }
                }

                //Write the private label if exists.
                if (PrivateLabelOffsets.Contains(i)) {
                    seq.Add("\n_label_" + i + ":");
                }

                //Write the commands.
                seq.Add("\t" + Commands[i].ToSeqString(PublicLabelOffsets));

            }

            //Return the file.
            return seq.ToArray();

        }

        /// <summary>
        /// Read a sequence from a sequence file.
        /// </summary>
        /// <param name="seq">The sequence file.</param>
        public void FromSeqFile(string[] seq) {

            //New info.
            PrivateLabelOffsets = new List<int>();
            PublicLabelOffsets = new Dictionary<string, int>();
            Commands = new List<SequenceCommand>();

            //Temp private labels.
            Dictionary<string, int> privateLabels = new Dictionary<string, int>();

            //Commands with offsets.
            Dictionary<int, string> commandOffs = new Dictionary<int, string>();

            //Read each line.
            int commandNum = 0;
            foreach (string seqLine in seq) {

                //Remove comment area.
                string s = seqLine;
                if (seqLine.Contains(";")) {
                    s = seqLine.Split(';')[0];
                }

                //Remove spaces.
                s = s.Replace(" ", "").Replace("\t", "").Replace("\r", "");

                //The line is a label.
                if (s.EndsWith(":")) {

                    //Private label.
                    if (s.StartsWith("_")) {
                        privateLabels.Add(s.Split(':')[0], commandNum);
                        PrivateLabelOffsets.Add(commandNum);
                    }

                    //Public label.
                    else {
                        PublicLabelOffsets.Add(s.Split(':')[0], commandNum);
                    }

                }

                //Command is valid.
                else if (!s.Equals("")) {
                    SequenceCommand com = CommandFromString(s, commandNum, PrivateLabelOffsets, PublicLabelOffsets, commandOffs);
                    if (com != null) {
                        Commands.Add(com);
                        commandNum++;
                    }
                }

            }

        }

        /// <summary>
        /// Get closest labe to an offset.
        /// </summary>
        /// <param name="offset">Offset of label.</param>
        /// <returns>Closest label.</returns>
        public string GetClosestLabel(int offset) {
            return PublicLabelOffsets.OrderBy(e => Math.Abs(e.Value - (offset - 3))).FirstOrDefault().Key;
        }

    }

}
