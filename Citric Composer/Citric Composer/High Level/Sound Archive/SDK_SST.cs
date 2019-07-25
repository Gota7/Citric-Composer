using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CitraFileLoader {

    /// <summary>
    /// SDK exporter.
    /// </summary>
    public partial class SdkExporter {

        /// <summary>
        /// Write project file.
        /// </summary>
        /// <param name="folderPath">Directory to write SST.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode">How to export it.</param>
        public static void WriteSst(string folderPath, string projectName, SoundArchive a, WriteMode mode) {

            using (FileStream fileStream = new FileStream(folderPath + "/" + projectName + "." + (mode == WriteMode.CTR ? "c" : "f") + "sst", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter x = new XmlTextWriter(sw)) {

                //Start data.
                x.Formatting = Formatting.Indented;
                x.Indentation = 2;
                x.WriteStartDocument();
                x.WriteStartElement("SoundSet");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsi", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema-instance");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsd", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema");

                //Get version.
                string version = "1.";
                switch (mode) {

                    case WriteMode.Cafe:
                        version += "3";
                        break;

                    case WriteMode.CTR:
                        version += "2";
                        break;

                    case WriteMode.NX:
                        version += "0";
                        break;

                }
                version += ".0.0";
                x.WriteAttributeString("Version", version);

                //Get platform.
                string platform = "Any";
                switch (mode) {

                    case WriteMode.Cafe:
                        platform = "Cafe";
                        break;

                    case WriteMode.CTR:
                        platform = "Ctr";
                        break;

                }
                x.WriteAttributeString("Platform", platform);
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "h", "h").ToString().Trim(":=\"hh\"".ToCharArray()), "NintendoWare.SoundFoundation.FileFormats.NintendoWare");

                //Write head.
                x.WriteStartElement("Head"); //Head.
                x.WriteElementString("Title", projectName);
                x.WriteEndElement(); //Head.

                //Write body.
                x.WriteStartElement("Body");

                //Write sound set.
                x.WriteStartElement("SoundSet");

                //Write items.
                x.WriteStartElement("Items");
                WriteTempItem("StreamSound", x);
                WriteTempItem("WaveSoundSet", x);
                WriteSequences(a, mode, x);
                WriteTempItem("SequenceSoundSet", x);
                WriteBanks(a, mode, x);
                WriteWaveArchives(a, mode, x);
                WriteTempItem("WaveArchive", x);
                WriteTempItem("Group", x);
                WritePlayers(a, mode, x);

                //End project.
                x.WriteEndElement(); //SoundSet.
                x.WriteEndElement(); //Body.
                x.WriteEndElement(); //SoundSet.
                x.WriteEndDocument();

                //Flush.
                x.Flush();

            }

        }

        /// <summary>
        /// Write sequences
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode"></param>
        /// <param name="x">The writer.</param>
        public static void WriteSequences(SoundArchive a, WriteMode mode, XmlTextWriter x) {

            //Start folder.
            string name = "SequenceSound";
            x.WriteStartElement("SoundSetItemFolder");
            x.WriteAttributeString("Name", "@" + name + "s");

            //Parameters.
            WriteUselessParameters(x);

            //Items.
            x.WriteStartElement("Items");

            //Write actual data.
            foreach (var d in a.Sequences) {

                //Start item.
                x.WriteStartElement(name);
                x.WriteAttributeString("Name", d.Name);

                //Start parameters.
                StartUselessParameters(x);

                //Write actual data.
                x.WriteElementString("FilePath", "Files/Sequences/" + Path.GetFileNameWithoutExtension(d.File.FileName) + "." + (mode == WriteMode.CTR ? "c" : "f") + "seq");
                x.WriteElementString("Volume", d.Volume + "");
                x.WriteElementString("PlayerPriority", d.PlayerPriority + "");
                x.WriteStartElement("PlayerReference");
                x.WriteAttributeString("Target", d.Player.Name);
                x.WriteEndElement();
                x.WriteElementString("ActorPlayer", d.PlayerActorId + "");
                x.WriteElementString("UserParameter", d.UserParameter[0] + "");
                x.WriteElementString("UserParameter1", d.UserParameter[1] + "");
                x.WriteElementString("UserParameter2", d.UserParameter[2] + "");
                x.WriteElementString("UserParameter3", d.UserParameter[3] + "");

                //SOUND 3D.
                x.WriteStartElement("Sound3D");
                x.WriteStartElement("Parameters");
                if (d.Sound3dInfo == null) {
                    x.WriteElementString("DecayCurve3D", "Log");
                    x.WriteElementString("DecayRatio3D", "0.5");
                    x.WriteElementString("DopplerFactor3D", "0");
                    x.WriteElementString("Enable3DVolume", "True");
                    x.WriteElementString("Enable3DPan", "True");
                    x.WriteElementString("Enable3DSurroundPan", "True");
                    x.WriteElementString("Enable3DPriority", "True");
                    x.WriteElementString("Enable3DFilter", "False");
                } else {
                    x.WriteElementString("DecayCurve3D", d.Sound3dInfo.AttenuationCurve == Sound3dInfo.EAttenuationCurve.Logarithmic ? "Log" : "Linear");
                    x.WriteElementString("DecayRatio3D", d.Sound3dInfo.AttenuationRate + "");
                    x.WriteElementString("DopplerFactor3D", d.Sound3dInfo.DopplerFactor + "");
                    x.WriteElementString("Enable3DVolume", d.Sound3dInfo.Volume ? "True" : "False");
                    x.WriteElementString("Enable3DPan", d.Sound3dInfo.Pan ? "True" : "False");
                    x.WriteElementString("Enable3DSurroundPan", d.Sound3dInfo.Span ? "True" : "False");
                    x.WriteElementString("Enable3DPriority", d.Sound3dInfo.Priority ? "True" : "False");
                    x.WriteElementString("Enable3DFilter", d.Sound3dInfo.Filter ? "True" : "False");
                }
                x.WriteEndElement();
                x.WriteEndElement();
                x.WriteElementString("SequenceSoundFileType", "Text");
                x.WriteElementString("StartPosition", (d.File.File as SoundSequence).SequenceData.GetClosestLabel((int)d.StartOffset));
                x.WriteStartElement("SoundSetBankReferences");
                x.WriteStartElement("SoundSetBankReference");
                x.WriteAttributeString("Target", d.Banks[0] == null ? "" : d.Banks[0].Name);
                x.WriteEndElement();
                x.WriteStartElement("SoundSetBankReference");
                x.WriteAttributeString("Target", d.Banks[1] == null ? "" : d.Banks[1].Name);
                x.WriteEndElement();
                x.WriteStartElement("SoundSetBankReference");
                x.WriteAttributeString("Target", d.Banks[2] == null ? "" : d.Banks[2].Name);
                x.WriteEndElement();
                x.WriteStartElement("SoundSetBankReference");
                x.WriteAttributeString("Target", d.Banks[3] == null ? "" : d.Banks[3].Name);
                x.WriteEndElement();
                x.WriteEndElement();
                x.WriteElementString("ChannelPriority", d.ChannelPriority + "");
                x.WriteElementString("ReleasePriorityFixed", d.IsReleasePriority ? "True" : "False");
                x.WriteElementString("FrontBypass", d.IsFrontBypass ? "True" : "False");
                if (mode != WriteMode.CTR) { x.WriteElementString("RemoteFilter", d.RemoteFilter + ""); }

                //End parameters.
                EndUselessParameters(x);

                //End item.
                x.WriteEndElement();

            }

            //End items.
            x.WriteEndElement();

            //End folder.
            x.WriteEndElement();

        }

        /// <summary>
        /// Write banks.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode"></param>
        /// <param name="x">The writer.</param>
        public static void WriteBanks(SoundArchive a, WriteMode mode, XmlTextWriter x) {

            //Start folder.
            string name = "SoundSetBank";
            x.WriteStartElement("SoundSetItemFolder");
            x.WriteAttributeString("Name", "@" + name + "s");

            //Parameters.
            WriteUselessParameters(x);

            //Items.
            x.WriteStartElement("Items");

            //Write actual data.
            foreach (var d in a.Banks) {

                //Start item.
                x.WriteStartElement(name);
                x.WriteAttributeString("Name", d.Name);

                //Start parameters.
                StartUselessParameters(x);

                //Write actual data.
                x.WriteElementString("FilePath", "Files/Banks/" + Path.GetFileNameWithoutExtension(d.File.FileName) + "." + (mode == WriteMode.CTR ? "c" : "f") + "bnk");
                x.WriteStartElement("WaveArchiveReference");
                var h = d.WaveArchives;
                x.WriteAttributeString("Target", h.Count == 0 ? "(AutoShared)" : d.WaveArchives[0].Name);
                x.WriteEndElement();

                //End parameters.
                EndUselessParameters(x);

                //End item.
                x.WriteEndElement();

            }

            //End items.
            x.WriteEndElement();

            //End folder.
            x.WriteEndElement();

        }

        /// <summary>
        /// Write wave archives.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode"></param>
        /// <param name="x">The writer.</param>
        public static void WriteWaveArchives(SoundArchive a, WriteMode mode, XmlTextWriter x) {

            //Start folder.
            string name = "WaveArchive";
            x.WriteStartElement("SoundSetItemFolder");
            x.WriteAttributeString("Name", "@" + name + "s");

            //Parameters.
            WriteUselessParameters(x);

            //Items.
            x.WriteStartElement("Items");

            //Write actual data.
            int num = 0;
            foreach (var d in a.WaveArchives) {

                //Start item.
                x.WriteStartElement(name);
                x.WriteAttributeString("Name", d.Name == null ? "WARC_NULL_" + num++ : d.Name);

                //Start parameters.
                StartUselessParameters(x);

                //Write actual data.
                x.WriteElementString("WaveArchiveLoadType", d.LoadIndividually ? "Individual" : "Whole");

                //End parameters.
                EndUselessParameters(x);

                //End item.
                x.WriteEndElement();

            }

            //End items.
            x.WriteEndElement();

            //End folder.
            x.WriteEndElement();

        }

        /// <summary>
        /// Write players.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode"></param>
        /// <param name="x">The writer.</param>
        public static void WritePlayers(SoundArchive a, WriteMode mode, XmlTextWriter x) {

            //Start folder.
            string name = "Player";
            x.WriteStartElement("SoundSetItemFolder");
            x.WriteAttributeString("Name", "@" + name + "s");

            //Parameters.
            WriteUselessParameters(x);

            //Items.
            x.WriteStartElement("Items");

            //Write actual data.
            foreach (var d in a.Players) {

                //Start item.
                x.WriteStartElement(name);
                x.WriteAttributeString("Name", d.Name);

                //Start parameters.
                StartUselessParameters(x);

                //Write actual data.
                x.WriteElementString("PlayerSoundLimit", d.SoundLimit + "");
                x.WriteElementString("PlayerHeapSize", d.IncludeHeapSize ? d.PlayerHeapSize + "" : 0 + "");

                //End parameters.
                EndUselessParameters(x);

                //End item.
                x.WriteEndElement();

            }

            //End items.
            x.WriteEndElement();

            //End folder.
            x.WriteEndElement();

        }

        /// <summary>
        /// Write useless parameters.
        /// </summary>
        /// <param name="x">The writer.</param>
        public static void WriteUselessParameters(XmlTextWriter x) {

            //Write parameters.
            x.WriteStartElement("Parameters");
            x.WriteStartElement("Comment");
            x.WriteEndElement();
            x.WriteStartElement("Comment1");
            x.WriteEndElement();
            x.WriteStartElement("Comment2");
            x.WriteEndElement();
            x.WriteStartElement("Comment3");
            x.WriteEndElement();
            x.WriteStartElement("Comment4");
            x.WriteEndElement();
            x.WriteStartElement("Comment5");
            x.WriteEndElement();
            x.WriteStartElement("Comment6");
            x.WriteEndElement();
            x.WriteStartElement("Comment7");
            x.WriteEndElement();
            x.WriteStartElement("Comment8");
            x.WriteEndElement();
            x.WriteStartElement("Comment9");
            x.WriteEndElement();
            x.WriteElementString("ColorIndex", "0");
            x.WriteElementString("IsEnabled", "True");
            x.WriteEndElement(); //Parameters.

        }

        /// <summary>
        /// Write useless parameters.
        /// </summary>
        /// <param name="x">The writer.</param>
        public static void StartUselessParameters(XmlTextWriter x) {

            //Write parameters.
            x.WriteStartElement("Parameters");
            x.WriteStartElement("Comment");
            x.WriteEndElement();
            x.WriteStartElement("Comment1");
            x.WriteEndElement();
            x.WriteStartElement("Comment2");
            x.WriteEndElement();
            x.WriteStartElement("Comment3");
            x.WriteEndElement();
            x.WriteStartElement("Comment4");
            x.WriteEndElement();
            x.WriteStartElement("Comment5");
            x.WriteEndElement();
            x.WriteStartElement("Comment6");
            x.WriteEndElement();
            x.WriteStartElement("Comment7");
            x.WriteEndElement();
            x.WriteStartElement("Comment8");
            x.WriteEndElement();
            x.WriteStartElement("Comment9");
            x.WriteEndElement();
            x.WriteElementString("ColorIndex", "0");
            x.WriteElementString("IsEnabled", "True");

        }

        /// <summary>
        /// Write useless parameters.
        /// </summary>
        /// <param name="x">The writer.</param>
        public static void EndUselessParameters(XmlTextWriter x) {

            //Wow.
            x.WriteEndElement(); //Parameters.

        }

        /// <summary>
        /// Write temp item.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="x">The writer.</param>
        public static void WriteTempItem(string name, XmlTextWriter x) {

            //Start folder.
            x.WriteStartElement("SoundSetItemFolder");
            x.WriteAttributeString("Name", "@" + name + "s");

            //Parameters.
            WriteUselessParameters(x);

            //End folder.
            x.WriteEndElement();

        }

    }

}