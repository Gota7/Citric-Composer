using System;
using System.IO;
using Syroot.BinaryData;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Retsuko_Sound_Tool
{
    /// <summary>
    /// Extractor.
    /// </summary>
    public static class extractor {

        public static void extractFiles(byte[] file, string path) {

            MemoryStream src = new MemoryStream(file);
            BinaryDataReader br = new BinaryDataReader(src);

            //Read files.
            int number = 0;

            //Map file.
            List<string> mapFile = new List<string>();
            // Map File: Offset; Size; Filename; Add_Name_Here
            mapFile.Add("How the format works: Offset; Size; Filename; Name For Injector To Read");

            string[] magicNumbers = { "CWSD", "FWSD", "CSEQ", "FSEQ", "CBNK", "FBNK", "CGRP", "FGRP", "CSTM", "FSTM", "CWAV", "FWAV", "CWAR", "FWAR" };
            UInt32[] magicNumbersBin = new UInt32[magicNumbers.Length];
            for (int i = 0; i < magicNumbers.Count(); i++)
            {
                byte[] h = Encoding.UTF8.GetBytes(magicNumbers[i]);
                MemoryStream o2 = new MemoryStream(h);
                BinaryDataReader br2 = new BinaryDataReader(o2);
                br2.ByteOrder = ByteOrder.BigEndian;
                magicNumbersBin[i] = br2.ReadUInt32();
            }

            br.ReadUInt32();
            while (br.Position <= file.Length - 4) {

                br.ByteOrder = ByteOrder.BigEndian;
                UInt32 magic = br.ReadUInt32();
                byte[] f;
                UInt16 t;
                UInt32 l;

                    Dictionary<string, Dictionary<int, int>> dic = new Dictionary<string, Dictionary<int, int>>();

                for (int i = 0; i < magicNumbersBin.Count(); i++)
                {

                    if (magic.Equals(magicNumbersBin[i]))
                    {

                        t = br.ReadUInt16();
                        if (t == 0xFFFE)
                        {
                            br.ByteOrder = ByteOrder.LittleEndian;
                        }
                        else
                        {
                            br.ByteOrder = ByteOrder.BigEndian;
                        }
                        br.ReadUInt16s(3);
                        l = br.ReadUInt32();

                        br.Position -= 16;
                        long position = br.Position;

                        f = br.ReadBytes((int)l);

                        Directory.CreateDirectory(path + "/" + magicNumbers[i]);
                        File.WriteAllBytes(path + "/" + magicNumbers[i] + "/" + number.ToString("D4") + ".b" + magicNumbers[i].ToLower(), f);

                        if (i == 0 || i == 1 || i == 6 || i == 7 || i == 12 || i == 13) {

                            Directory.CreateDirectory(path + "/" + magicNumbers[i] + "/" + number.ToString("D4") + magicNumbers[i]);
                            extractFiles(f, path + "/" + magicNumbers[i] + "/" + number.ToString("D4") + magicNumbers[i]);

                        }

                        mapFile.Add(position + "; " + l + "; " + magicNumbers[i] + "/" + number.ToString("D4") + ".b" + magicNumbers[i].ToLower() + "; " + "ADD_CUSTOM_NAME_HERE!");

                        number += 1;

                    }

                }

            }

            File.WriteAllLines(path + "/fileMap.txt", mapFile.ToArray());

        }

    }


    /// <summary>
    /// Injector.
    /// </summary>
    public static class injector {

        public static void injectOneFile(MainWindow.fileReplacement fR, List<byte> newFile, byte[] sourceFile, string mapFilePath, string sourcePath) {

            if (newFile.Count() > fR.size)
            {
                throw new Exception("New file greater than source file, which is " + fR.size + " bytes!");
            }
            else {

                MemoryStream src = new MemoryStream(sourceFile);
                BinaryDataReader br = new BinaryDataReader(src);
                MemoryStream o = new MemoryStream();
                BinaryDataWriter bw = new BinaryDataWriter(o);

                byte[] firstJunk = br.ReadBytes(fR.offset);
                bw.Write(firstJunk);
                br.ReadBytes(fR.size);

                //Make size correct.
                while (newFile.Count() < fR.size) {
                    newFile.Add(0);
                }

                bw.Write(newFile.ToArray());

                byte[] lastJunk = br.ReadBytes(sourceFile.Length - (int)br.Position);
                bw.Write(lastJunk);

                File.WriteAllBytes(sourcePath, o.ToArray());

            }

        }



        public static bool injectAllFiles(MainWindow.fileReplacement[] fR, string mapDirectory, byte[] sourceFile, string sourcePath) {

            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);

            MemoryStream sourceReader = new MemoryStream(sourceFile);
            BinaryDataReader sR = new BinaryDataReader(sourceReader);

            bool cancel = false;

            foreach (MainWindow.fileReplacement f in fR) {

                byte[] junk = sR.ReadBytes(f.offset - (int)sR.Position);
                bw.Write(junk);

                MemoryStream src = new MemoryStream(File.ReadAllBytes(mapDirectory + "/" + f.path));
                BinaryDataReader br = new BinaryDataReader(src);
                List<byte> newFile = br.ReadBytes((int)src.Length).ToList();

                if (f.path.StartsWith("FGRP") || f.path.StartsWith("CGRP") || f.path.StartsWith("FWAR") || f.path.StartsWith("CWAR") || f.path.StartsWith("FWSD") || f.path.StartsWith("CWSD")) {

                    List<MainWindow.fileReplacement> h = new List<MainWindow.fileReplacement>();
                    string[] map = File.ReadAllLines(Path.GetDirectoryName(mapDirectory + "/" + f.path) + "/" + Path.GetFileNameWithoutExtension(f.path) + f.path.Substring(0, 4) + "/fileMap.txt");
                    for (int i = 1; i < map.Length; i++)
                    {

                        MainWindow.fileReplacement r = new MainWindow.fileReplacement();
                        string[] split = map[i].Split(';');
                        r.offset = int.Parse(split[0]);
                        r.size = int.Parse(split[1].Substring(1));
                        r.path = split[2].Substring(1);
                        r.name = split[3].Substring(1);
                        h.Add(r);

                    }

                    cancel = injectAllFiles(h.ToArray(), Path.GetDirectoryName(mapDirectory + "/" + f.path) + "/" + Path.GetFileNameWithoutExtension(f.path) + f.path.Substring(0, 4), File.ReadAllBytes(mapDirectory + "/" + f.path), mapDirectory + "/" + f.path);
                }

                while (newFile.Count() < f.size) {
                    newFile.Add(0);
                }
                bw.Write(newFile.ToArray());
                sR.ReadBytes(newFile.Count());

                if (newFile.Count() > f.size) {
                    cancel = true;
                    throw new Exception("Filesize over limit! File " + f.path + " is over " + f.size + " bytes!");
                }

            }

            byte[] endingJunk = sR.ReadBytes(sourceFile.Length - (int)sR.Position);
            bw.Write(endingJunk);

            if (!cancel) { File.WriteAllBytes(sourcePath, o.ToArray()); } else { throw new Exception("Source File was NOT overwritten due to errors."); }
            return cancel;

        }

    }

}
