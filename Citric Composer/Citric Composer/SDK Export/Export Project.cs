using CitraFileLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Citric_Composer {

    public static class ProjectExporter {

        /// <summary>
        /// Export type.
        /// </summary>
        public enum ExportType {

            Cafe, CTR, NX

        }


        /// <summary>
        /// Write an FSPJ file.
        /// </summary>
        /// <param name="folderPath">Directory to write FSPJ.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="type">Export type.</param>
        public static void WriteFSPJ(string folderPath, string projectName, SoundArchive a, ExportType type) {

            using (FileStream fileStream = new FileStream(folderPath + "/" + projectName + "." + (type == ExportType.CTR ? "c" : "f") + "spj", FileMode.Create))
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
                switch (type) {

                    case ExportType.Cafe:
                        version += "4";
                        break;

                    case ExportType.CTR:
                        version += "6";
                        break;

                    case ExportType.NX:
                        version += "1";
                        break;

                }
                version += ".0.0";

                x.WriteAttributeString("Version", version);

                //Get platform.
                string platform = "Any";
                switch (type) {

                    case ExportType.Cafe:
                        platform = "Cafe";
                        break;

                    case ExportType.CTR:
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
                x.WriteElementString("FilePath", projectName + "." + (type == ExportType.CTR ? "c" : "f") + "sst");
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
                if (type != ExportType.CTR) x.WriteElementString("DoWarnPCBinariesForAACNotFound", "True");
                x.WriteElementString("ExcludeStringTable", "False");
                if (type != ExportType.CTR) x.WriteElementString("DoOutputPCBinariesForAAC", "True");
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

                x.WriteStartElement("PreConvertCommands");
                x.WriteEndElement();
                x.WriteStartElement("PostConvertCommands");
                x.WriteEndElement();
                x.WriteEndElement(); //Parameters.
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
                x.WriteElementString("SyncPort", type == ExportType.CTR ? "10" : "54086");
                x.WriteElementString("SyncPort", type == ExportType.CTR ? "11" : "54087");
                x.WriteElementString("SyncPort", type == ExportType.CTR ? "12" : "54088");
                x.WriteEndElement();
                x.WriteEndElement();


                x.WriteEndElement(); //Sound project.

                x.WriteEndElement(); //Body.

                x.WriteEndElement(); //SoundProject.
                x.WriteEndDocument();
                x.Flush();

            }

        }


        /// <summary>
        /// Write an FSST file.
        /// </summary>
        /// <param name="folderPath">Directory to write FSST.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="type">Export type.</param>
        public static void WriteFSST(string folderPath, string projectName, SoundArchive a, ExportType type) {

            using (FileStream fileStream = new FileStream(folderPath + "/" + projectName + "." + (type == ExportType.CTR ? "c" : "f") + "sst", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter x = new XmlTextWriter(sw)) {

                x.Formatting = Formatting.Indented;
                x.Indentation = 2;

                x.WriteStartDocument();
                x.WriteStartElement("SoundSet");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsi", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema-instance");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsd", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema");

                //Get version.
                string version = "1.";
                switch (type) {

                    case ExportType.Cafe:
                        version += "3";
                        break;

                    case ExportType.CTR:
                        version += "1";
                        break;

                    case ExportType.NX:
                        version += "0";
                        break;

                }
                version += ".0.0";

                x.WriteAttributeString("Version", version);

                //Get platform.
                string platform = "Any";
                switch (type) {

                    case ExportType.Cafe:
                        platform = "Cafe";
                        break;

                    case ExportType.CTR:
                        platform = "Ctr";
                        break;

                }

                x.WriteAttributeString("Platform", platform);
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "h", "h").ToString().Trim(":=\"hh\"".ToCharArray()), "NintendoWare.SoundFoundation.FileFormats.NintendoWare");

                //Write head with title.
                x.WriteStartElement("Head");
                x.WriteElementString("Title", projectName + "SoundSet");
                x.WriteEndElement(); //Head.

                //Body.
                x.WriteStartElement("Body");

                //Sound set.
                x.WriteStartElement("SoundSet");
                x.WriteStartElement("Items");


                //Stream sounds.
                #region StreamSounds
                /*
                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@StreamSounds");
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
                x.WriteStartElement("Items");


                //New stream.
                int streamNumber = 0;
                foreach (var s in a.Streams) {

                    //Get info.
                    string sName = (s.Name == null) ?

                    //Write player.
                    x.WriteStartElement("StreamSound");
                    x.WriteAttributeString("Name", sName);

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

                    x.WriteElementString("RemoteFilter", "" + s.remoteFilter);
                    x.WriteElementString("Volume", "" + s.volume);

                    //TODO.
                    int priority = 64;
                    int actorNum = 0;
                    int panCurve = 0;
                    int userParam0 = 0; //Last bit (31).
                    int userParam1 = 0;
                    int userParam2 = 0;
                    int userParam3 = 0; //First bit (28).

                    //Get 3d info.
                    b_sar.InfoBlock.soundInfo.sound3dInfo d3 = new b_sar.InfoBlock.soundInfo.sound3dInfo()
                    {
                        flagBoxes = 0xF,
                        dimensionalAttenuation = .5f,
                        attenuationCurveType = 1,
                        dopplerFactor = 0,
                        padding = 0,
                        optionParameter = new FlagParameters(new Dictionary<int, uint>())
                    };

                    if (s.sound3d != null) {
                        d3 = s.sound3d;
                    }

                    string dCurve = d3.attenuationCurveType == 1 ? "Log" : "Linear";
                    bool dVol = BitHelperUInt.IsBitSet(d3.flagBoxes, 0);
                    bool dPri = BitHelperUInt.IsBitSet(d3.flagBoxes, 1);
                    bool dPan = BitHelperUInt.IsBitSet(d3.flagBoxes, 2);
                    bool dSpan = BitHelperUInt.IsBitSet(d3.flagBoxes, 3);
                    bool dFil = BitHelperUInt.IsBitSet(d3.flagBoxes, 4);

                    x.WriteElementString("PlayerPriority", "" + priority);
                    x.WriteStartElement("PlayerReference");
                    x.WriteAttributeString("Target", b.GetItemName(SDKSoundType.Player, (int)s.playerNumber.index));
                    x.WriteEndElement();
                    x.WriteElementString("ActorPlayer", "" + actorNum);
                    x.WriteElementString("UserParameter", "" + userParam0);
                    x.WriteElementString("UserParameter1", "" + userParam1);
                    x.WriteElementString("UserParameter2", "" + userParam2);
                    x.WriteElementString("UserParameter3", "" + userParam3);

                    x.WriteStartElement("Sound3D");
                    x.WriteStartElement("Parameters");

                    x.WriteElementString("DecayCurve3D", dCurve);
                    x.WriteElementString("DecayRatio3D", "" + d3.dimensionalAttenuation);
                    x.WriteElementString("DopplerFactor3D", "" + d3.dopplerFactor);
                    x.WriteElementString("Enable3DVolume", "" + dVol);
                    x.WriteElementString("Enable3DPan", "" + dPan);
                    x.WriteElementString("Enable3DSurroundPan", "" + dSpan);
                    x.WriteElementString("Enable3DPriority", "" + dPri);
                    x.WriteElementString("Enable3DFilter", "" + dFil);

                    x.WriteEndElement();
                    x.WriteEndElement();

                    x.WriteEndElement(); //Parameters.
                    x.WriteEndElement(); //Player

                    streamNumber++;

                }

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.
                */
                #endregion

                //Wave sound sets. TODO.
                #region WaveSoundSets

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@WaveSoundSets");
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
                x.WriteStartElement("Items");

                //Add items here.

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.
                
                #endregion

                //Sequence sounds. TODO.
                #region SequenceSounds

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@SequenceSounds");
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
                x.WriteStartElement("Items");

                //Add items here.

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.

                #endregion

                //Sequence sound sets. TODO.
                #region SequenceSoundSets

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@SequenceSoundSets");
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
                x.WriteStartElement("Items");

                //Add items here.

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.

                #endregion

                //Sound set banks. WARC NEEDS ATTENTION. TODO.
                #region SoundSetBanks

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@SoundSetBanks");
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
                x.WriteStartElement("Items");

                //New bank.
                int bankNumber = 0;
                foreach (var bnk in a.Banks) {

                    //Get info.
                    string bName = "BANK_" + bankNumber.ToString("D4");
                    if (bnk.Name != null) { bName = bnk.Name; }

                    //Write player.
                    x.WriteStartElement("SoundSetBank");
                    x.WriteAttributeString("Name", bName);

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

                    x.WriteElementString("FilePath", "BANK/" + bName + "." + (type == ExportType.CTR ? "c" : "f") + "bnk");
                    x.WriteStartElement("WaveArchiveReference");
                    x.WriteAttributeString("Target", "(AutoShared)");
                    x.WriteEndElement();

                    x.WriteEndElement(); //Parameters.
                    x.WriteEndElement(); //Bank.

                    bankNumber++;

                }

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.

                #endregion

                //Wave archives. TODO.
                #region WaveArchives

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@WaveArchives");
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
                x.WriteStartElement("Items");

                //New war.
                int warNumber = 0;
                foreach (var w in a.WaveArchives) {

                    //Get info.
                    string wName = "WARC_" + warNumber.ToString("D4");
                    if (w.Name != null) { wName = w.Name; }

                    //Write war.
                    x.WriteStartElement("WaveArchive");
                    x.WriteAttributeString("Name", wName);

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

                    x.WriteElementString("WaveArchiveLoadType", w.LoadIndividually ? "Individual" : "Whole");

                    x.WriteEndElement(); //Parameters.
                    x.WriteEndElement(); //War.

                    warNumber++;

                }

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.

                #endregion

                //Groups. TODO.
                #region Groups

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@Groups");
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
                x.WriteStartElement("Items");

                //Add items here.
                int groupNumber = 0;
                foreach (var g in a.Groups) {

                    //Get info.
                    string name = "GROUP_" + groupNumber.ToString("D4");
                    if (g.Name != null) { name = g.Name; }

                    //Write player.
                    x.WriteStartElement("Group");
                    x.WriteAttributeString("Name", name);

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

                    x.WriteElementString("PlayerSoundLimit", g.Name + "");

                    x.WriteEndElement(); //Parameters.
                    x.WriteEndElement(); //Player

                    groupNumber++;

                }

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.

                #endregion

                //Players.
                #region Players

                //Header.
                x.WriteStartElement("SoundSetItemFolder");
                x.WriteAttributeString("Name", "@Players");
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
                x.WriteStartElement("Items");

                //New player.
                int playerNumber = 0;
                foreach (var p in a.Players) {

                    //Get info.
                    string pName = "PLAYER_" + playerNumber.ToString("D4");
                    uint pSoundHeap = 0;
                    if (p.Name != null) { pName = p.Name; }

                    //Write player.
                    x.WriteStartElement("Player");
                    x.WriteAttributeString("Name", pName);

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

                    x.WriteElementString("PlayerSoundLimit", p.SoundLimit + "");
                    x.WriteElementString("PlayerHeapSize", pSoundHeap + "");

                    x.WriteEndElement(); //Parameters.
                    x.WriteEndElement(); //Player

                    playerNumber++;

                }

                //Footer.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set item folder.

                #endregion

                //Close elements.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Sound set.

                x.WriteEndElement(); //Body.

                x.WriteEndElement(); //Sound set.
                x.WriteEndDocument();
                x.Flush();

            }

        }

    }

}
