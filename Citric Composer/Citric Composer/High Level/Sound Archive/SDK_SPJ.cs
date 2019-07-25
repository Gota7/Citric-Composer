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
        /// <param name="folderPath">Directory to write SPJ.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="mode">How to export it.</param>
        public static void WriteSpj(string folderPath, string projectName, SoundArchive a, WriteMode mode) {

            using (FileStream fileStream = new FileStream(folderPath + "/" + projectName + "." + (mode == WriteMode.CTR ? "c" : "f") + "spj", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter x = new XmlTextWriter(sw)) {

                x.Formatting = Formatting.Indented;
                x.Indentation = 2;

                x.WriteStartDocument();
                x.WriteStartElement("SoundProject");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsi", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema-instance");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsd", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema");

                //Get version.
                string version = "1.";
                switch (mode) {

                    case WriteMode.Cafe:
                        version += "4";
                        break;

                    case WriteMode.CTR:
                        version += "6";
                        break;

                    case WriteMode.NX:
                        version += "1";
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

                //Write head with title.
                x.WriteStartElement("Head");
                x.WriteElementString("Title", projectName + "SoundProject");
                x.WriteEndElement(); //Head.

                //Body.
                x.WriteStartElement("Body");

                //Sound project.
                x.WriteStartElement("SoundProject");

                //Project sound sets.
                x.WriteStartElement("ProjectSoundSets");
                x.WriteStartElement("ProjectSoundSet");
                x.WriteAttributeString("Name", projectName);
                x.WriteStartElement("Parameters");
                x.WriteElementString("FilePath", projectName + "." + (mode == WriteMode.CTR ? "c" : "f") + "sst");
                x.WriteEndElement(); //Parameters.
                x.WriteEndElement(); //Project sound set.
                x.WriteEndElement(); //Project sound sets.

                //Sound archive player.
                x.WriteStartElement("SoundArchivePlayer");
                x.WriteStartElement("Parameters");
                x.WriteElementString("SoundArchivePlayerSequenceSoundCount", "" + a.MaxSequences);
                x.WriteElementString("SoundArchivePlayerSequenceTrackCount", "" + a.MaxSequenceTracks);
                x.WriteElementString("SoundArchivePlayerStreamChannelCount", "" + a.MaxStreamChannels);
                x.WriteElementString("SoundArchivePlayerStreamBufferTimes", "" + a.StreamBufferTimes);
                x.WriteElementString("SoundArchivePlayerStreamSoundCount", "" + a.MaxStreamSounds);
                x.WriteElementString("SoundArchivePlayerWaveSoundCount", "" + a.MaxWaveSounds);
                x.WriteEndElement(); //Parameters.
                x.WriteEndElement(); //Sound archive player.

                //Convert.
                x.WriteStartElement("Convert");
                x.WriteStartElement("Parameters");

                x.WriteElementString("DoWarnUnreferencedItems", "False");
                x.WriteElementString("DoWarnDisableGroupItemTargets", "False");
                if (mode != WriteMode.CTR) x.WriteElementString("DoWarnPCBinariesForAACNotFound", "True");
                x.WriteElementString("ExcludeStringTable", "False");
                if (mode != WriteMode.CTR) x.WriteElementString("DoOutputPCBinariesForAAC", "True");
                x.WriteElementString("ExternalFileDirectoryPath", "stream");
                x.WriteElementString("UserManagementFileOutputDirectoryPath", "userManagementFiles");
                x.WriteElementString("IntermediateOutputDirectoryPath", "cache");
                x.WriteElementString("InGameEditCacheOutputDirectoryPath", "editCache");
                x.WriteElementString("IsPreConvertCommandsEnabled", "True");
                x.WriteElementString("IsPostConvertCommandsEnabled", "True");
                x.WriteElementString("KeepIntermediateTextSequence", "True");
                x.WriteElementString("OutputLabel", "False");
                x.WriteElementString("OutputDirectoryPath", "output");
                x.WriteElementString("SmfTimebase", "96");
               
                x.WriteEndElement(); //Parameters.
                x.WriteStartElement("PreConvertCommands");
                x.WriteEndElement();
                x.WriteStartElement("PostConvertCommands");
                x.WriteEndElement();
                x.WriteEndElement(); //Convert.

                //Item naming.
                x.WriteStartElement("ItemNaming");
                x.WriteStartElement("Parameters");

                x.WriteElementString("BankNamePrefix", "BANK_");
                x.WriteElementString("CaseChange", "ToUpper");
                x.WriteElementString("GroupNamePrefix", "GROUP_");
                x.WriteElementString("HasPrefix", "True");
                x.WriteElementString("InstrumentNamePrefix", "INST_");
                x.WriteElementString("InvalidCharChange", "ReplaceToUnderscore");
                x.WriteElementString("PlayerNamePrefix", "PLAYER_");
                x.WriteElementString("SequenceSoundNamePrefix", "SEQ_");
                x.WriteElementString("SequenceSoundSetNamePrefix", "SEQSET_");
                x.WriteElementString("StreamSoundNamePrefix", "STRM_");
                x.WriteElementString("WaveArchiveNamePrefix", "WARC_");
                x.WriteElementString("WaveSoundNamePrefix", "WSD_");
                x.WriteElementString("WaveSoundSetNamePrefix", "WSDSET_");
                x.WriteElementString("ItemPastePostfix", "_copy");
                x.WriteElementString("EnabledNameDelimiter", "True");
                x.WriteElementString("NameDelimiter", ".");

                x.WriteEndElement(); //Parameters.
                x.WriteEndElement(); //Item naming.

                //Comment column text.
                x.WriteStartElement("CommentColumnText");
                x.WriteStartElement("Parameters");

                x.WriteElementString("CommentColumnText", "Comments");
                x.WriteElementString("Comment1ColumnText", "Comment 1");
                x.WriteElementString("Comment2ColumnText", "Comment 2");
                x.WriteElementString("Comment3ColumnText", "Comment 3");
                x.WriteElementString("Comment4ColumnText", "Comment 4");
                x.WriteElementString("Comment5ColumnText", "Comment 5");
                x.WriteElementString("Comment6ColumnText", "Comment 6");
                x.WriteElementString("Comment7ColumnText", "Comment 7");
                x.WriteElementString("Comment8ColumnText", "Comment 8");
                x.WriteElementString("Comment9ColumnText", "Comment 9");

                x.WriteEndElement(); //Parameters.
                x.WriteEndElement(); //Comment column text.

                //Color comment.
                x.WriteStartElement("ColorComment");
                x.WriteStartElement("Parameters");

                x.WriteStartElement("ColorComment0");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment1");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment2");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment3");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment4");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment5");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment6");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment7");
                x.WriteEndElement();
                x.WriteStartElement("ColorComment8");
                x.WriteEndElement();

                x.WriteEndElement(); //Parameters.
                x.WriteEndElement(); //Color comment.

                //User commands.
                x.WriteStartElement("UserCommands");

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteStartElement("UserCommand");
                x.WriteAttributeString("Name", "");
                x.WriteAttributeString("Command", "");
                x.WriteAttributeString("IconFilePath", "");
                x.WriteEndElement();

                x.WriteEndElement(); //User commands.

                x.WriteStartElement("SoundListOutputs");
                x.WriteEndElement();

                //User parameter settings.
                x.WriteStartElement("UserParameterSettings");

                x.WriteStartElement("UserParameterSetting");
                x.WriteAttributeString("Enabled", "true");
                x.WriteStartElement("UserParameterStructures");
                x.WriteEndElement();
                x.WriteEndElement();

                x.WriteStartElement("UserParameterSetting");
                x.WriteAttributeString("Enabled", "true");
                x.WriteStartElement("UserParameterStructures");
                x.WriteEndElement();
                x.WriteEndElement();

                x.WriteStartElement("UserParameterSetting");
                x.WriteAttributeString("Enabled", "true");
                x.WriteStartElement("UserParameterStructures");
                x.WriteEndElement();
                x.WriteEndElement();

                x.WriteStartElement("UserParameterSetting");
                x.WriteAttributeString("Enabled", "true");
                x.WriteStartElement("UserParameterStructures");
                x.WriteEndElement();
                x.WriteEndElement();

                x.WriteEndElement();

                //Project setting.
                x.WriteStartElement("ProjectSetting");
                x.WriteStartElement("Parameters");
                x.WriteStartElement("ProjectComment");
                x.WriteEndElement();
                x.WriteEndElement();
                x.WriteEndElement();

                //File event.
                x.WriteStartElement("FileEvent");
                x.WriteStartElement("Parameters");
                x.WriteElementString("IsFileSavePreCommandEnabled", "False");
                x.WriteElementString("IsFileSavePostCommandEnabled", "False");
                x.WriteStartElement("FileSavePreCommandPath");
                x.WriteEndElement();
                x.WriteStartElement("FileSavePostCommandPath");
                x.WriteEndElement();
                x.WriteEndElement();
                x.WriteEndElement();

                //Snd edit.
                x.WriteStartElement("SndEdit");
                x.WriteStartElement("Parameters");
                x.WriteElementString("SyncPort", mode == WriteMode.CTR ? "10" : "54086");
                x.WriteElementString("SyncPort", mode == WriteMode.CTR ? "11" : "54087");
                x.WriteElementString("SyncPort", mode == WriteMode.CTR ? "12" : "54088");
                x.WriteEndElement();
                x.WriteEndElement();

                x.WriteEndElement(); //Sound project.

                x.WriteEndElement(); //Body.

                x.WriteEndElement(); //SoundProject.
                x.WriteEndDocument();
                x.Flush();

            }

        }

    }

}
