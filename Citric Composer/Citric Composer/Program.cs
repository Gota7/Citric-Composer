using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitraFileLoader;
using Syroot.BinaryData;

namespace Citric_Composer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //Application set up.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //No arguments.
            if (args.Length < 1) {
                Application.Run(new MainWindow());
            }
            
            //Arguments.
            else {

                //Main window.
                if (args[0].ToLower().EndsWith("sar")) {
                    Application.Run(new MainWindow(args[0]));
                }

                //Other file.
                else {

                    //Editor base.
                    EditorBase e = null;

                    //Switch ending.
                    switch (args[0].Substring(args[0].Length - 3, 3).ToLower()) {

                        //Wave archive editor.
                        case "war":
                            e = new Brewster_WAR_Brewer(args[0], null);
                            break;

                        //Isabelle.
                        case "stm":
                        case "wav":
                        case "isp":
                            IsabelleSoundEditor i = new IsabelleSoundEditor(args[0]);
                            Application.Run(i);
                            break;

                    }

                    //Run editor if type is defined.
                    if (e != null) {
                        Application.Run(e);
                    }

                }

            }

        }
    }
}
