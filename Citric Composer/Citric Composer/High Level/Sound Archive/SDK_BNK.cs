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
        /// Write bank file.
        /// </summary>
        /// <param name="folderPath">Directory to write BNK.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="b">Bank to export.</param>
        /// <param name="mode">Write mode.</param>
        public static void WriteBnk(string folderPath, SoundArchive a, BankEntry b, WriteMode mode) {

            using (FileStream fileStream = new FileStream(folderPath + "/Files/Banks/" + Path.GetFileNameWithoutExtension(b.File.FileName) + "." + (mode == WriteMode.CTR ? "c" : "f") + "bnk", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter x = new XmlTextWriter(sw)) {

                x.Formatting = Formatting.Indented;
                x.Indentation = 2;

                x.WriteStartDocument();
                x.WriteStartElement("Bank");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsi", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema-instance");
                x.WriteAttributeString(new XAttribute(XNamespace.Xmlns + "xsd", "h").ToString().Trim("=\"h\"".ToCharArray()), "http://www.w3.org/2001/XMLSchema");

                //Get version.
                string version = "1.";
                switch (mode) {

                    case WriteMode.Cafe:
                        version += "0";
                        break;

                    case WriteMode.CTR:
                        version += "0";
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

                //Write head with title.
                x.WriteStartElement("Head");
                x.WriteElementString("Title", b.Name);
                x.WriteEndElement(); //Head.

                //Body.
                x.WriteStartElement("Body");

                //Bank.
                x.WriteStartElement("Bank");

                //Items.
                x.WriteStartElement("Items");

                //Write each instrument.
                List<string> refs = new List<string>();
                var file = (b.File.File as SoundBank);
                int count = 1;
                int num = -1;
                foreach (var i in file.Instruments) {

                    //Increment number.
                    num++;
                    if (i == null) {
                        continue;
                    }

                    //Start instrument.
                    x.WriteStartElement("Instrument");
                    x.WriteAttributeString("Name", "INST_" + (count - 1));

                    //Useless parameters.
                    StartUselessParameters(x);
                    x.WriteElementString("ProgramNo", num + "");
                    x.WriteElementString("Volume", "127");
                    x.WriteElementString("PitchSemitones", "0");
                    x.WriteElementString("PitchCents", "0");
                    x.WriteStartElement("Envelope");
                    x.WriteStartElement("Parameters");
                    x.WriteElementString("Attack", "127");
                    x.WriteElementString("Decay", "127");
                    x.WriteElementString("Sustain", "127");
                    x.WriteElementString("Hold", "0");
                    x.WriteElementString("Release", "127");
                    x.WriteEndElement(); //Parameters.
                    x.WriteEndElement(); //Envelope.

                    //Envelope mode.
                    x.WriteElementString("InstrumentEnvelopeMode", (i as DirectInstrument) == null ? "VelocityRegion" : "Instrument");

                    //End parameters.
                    EndUselessParameters(x);

                    //Items.
                    x.WriteStartElement("Items");

                    //Switch type.
                    switch (i.GetInstrumentType()) {

                        //Direct.
                        case InstrumentType.Direct:
                            var d = i as DirectInstrument;
                            WriteKeyRegion(d.KeyRegion, a, b, 0, 127, x);
                            break;

                        //Index.
                        case InstrumentType.Index:
                            var ind = i as IndexInstrument;
                            int last = 0;
                            foreach (var k in ind) {
                                if (k.Value != null) WriteKeyRegion(k.Value, a, b, last, k.Key, x);
                                last = k.Key + 1;
                            }
                            break;

                        //Range.
                        case InstrumentType.Range:
                            var r = i as RangeInstrument;
                            int next = r.StartNote;
                            foreach (var k in r) {
                                if (k != null) WriteKeyRegion(k, a, b, next, next, x);
                                next++;
                            }
                            break;

                    }

                    //End items.
                    x.WriteEndElement();

                    //Add the program number to the list.
                    refs.Add("#define INST_" + (count - 1) + "\t" + count++);

                    //End instrument.
                    x.WriteEndElement();

                }
                File.WriteAllLines(folderPath + "/Files/Banks/" + Path.GetFileNameWithoutExtension(b.File.FileName) + "." + (mode == WriteMode.CTR ? "c" : "f") + "inl", refs.ToArray());

                //End it.
                x.WriteEndElement(); //Items.
                x.WriteEndElement(); //Bank.
                x.WriteEndElement(); //Body.
                x.WriteEndElement(); //Bank.
                x.WriteEndDocument();
                x.Flush();

            }

        }

        /// <summary>
        /// Write a key region.
        /// </summary>
        /// <param name="k">Key region.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="b">Bank entry.</param>
        /// <param name="start">Starting note.</param>
        /// <param name="end">Ending note.</param>
        /// <param name="x">The writer.</param>
        public static void WriteKeyRegion(IKeyRegion k, SoundArchive a, BankEntry b, int start, int end, XmlTextWriter x) {

            //Key region.
            x.WriteStartElement("KeyRegion");

            //Parameters.
            StartUselessParameters(x);
            x.WriteElementString("KeyMin", start + "");
            x.WriteElementString("KeyMax", end + "");
            EndUselessParameters(x);

            //Items.
            x.WriteStartElement("Items");

            //Write velocity regions.
            switch (k.GetKeyRegionType()) {

                //Direct.
                case KeyRegionType.Direct:
                    var d = k as DirectKeyRegion;
                    WriteVelocityRegion(d.VelocityRegion, a, b, 0, 127, x);
                    break;

                //Index.
                case KeyRegionType.Index:
                    var ind = k as IndexKeyRegion;
                    int last = 0;
                    foreach (var v in ind) {
                        if (v.Value != null) WriteVelocityRegion(v.Value, a, b, last, v.Key, x);
                        last = v.Key + 1;
                    }
                    break;

                //Range.
                case KeyRegionType.Range:
                    var r = k as RangeKeyRegion;
                    int next = 0;
                    foreach (var v in r) {
                        if (v != null) WriteVelocityRegion(v, a, b, next, next, x);
                        next++;
                    }
                    break;

            }

            //End items.
            x.WriteEndElement();

            //End key region.
            x.WriteEndElement();

        }

        /// <summary>
        /// Write a velocity region.
        /// </summary>
        /// <param name="v">Velocity region.</param>
        /// <param name="a">Sound archive.</param>
        /// <param name="b">Bank entry.</param>
        /// <param name="start">Starting note.</param>
        /// <param name="end">Ending note.</param>
        /// <param name="x">The writer.</param>
        public static void WriteVelocityRegion(VelocityRegion v, SoundArchive a, BankEntry b, int start, int end, XmlTextWriter x) {

            //Velocity region.
            x.WriteStartElement("VelocityRegion");

            //Parameters.
            StartUselessParameters(x);
            x.WriteElementString("FilePath", GetWaveFileName(v, a, b));
            x.WriteElementString("WaveEncoding", "Adpcm");
            x.WriteElementString("OriginalKey", v.OriginalKey + "");
            x.WriteStartElement("Envelope");
            x.WriteStartElement("Parameters");
            x.WriteElementString("Attack", v.Attack + "");
            x.WriteElementString("Decay", v.Decay + "");
            x.WriteElementString("Sustain", v.Sustain + "");
            x.WriteElementString("Hold", v.Hold + "");
            x.WriteElementString("Release", v.Release + "");
            x.WriteEndElement(); //Parameters.
            x.WriteEndElement(); //Envelope.
            x.WriteElementString("VelocityMin", start + "");
            x.WriteElementString("VelocityMax", end + "");
            x.WriteElementString("Volume", v.Volume + "");
            x.WriteElementString("Pan", v.Pan + "");
            Pitch pitch = new Pitch(v.Pitch);
            x.WriteElementString("PitchSemitones", pitch.Semitones + "");
            x.WriteElementString("PitchCents", pitch.Cents + "");
            x.WriteElementString("KeyGroup", v.KeyGroup + "");
            switch (v.InterPolationType) {

                case VelocityRegion.EInterPolationType.PolyPhase:
                    x.WriteElementString("InterpolationType", "Polyphase");
                    break;

                case VelocityRegion.EInterPolationType.Linear:
                    x.WriteElementString("InterpolationType", "Linear");
                    break;

            }
            x.WriteElementString("InstrumentNoteOffMode", v.PercussionMode ? "Ignore" : "Release");

            //End parameters.
            EndUselessParameters(x);

            //End velocity region.
            x.WriteEndElement();

        }

        /// <summary>
        /// Get file name of wave to use.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetWaveFileName(VelocityRegion v, SoundArchive a, BankEntry b) {

            //Base path.
            string path = "../Wave Archives";

            //Get entry.
            var e = (b.File.File as SoundBank).Waves[v.WaveIndex];

            //Get folder name.
            path += "/" + Path.GetFileNameWithoutExtension(a.WaveArchives[e.WarIndex].File.FileName);

            //Get file name.
            path += "/" + e.WaveIndex + ".wav";

            //Return the path.
            return path;

        }

    }

}
