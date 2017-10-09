using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace AStyleWhore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("Silent", StringComparison.CurrentCultureIgnoreCase))
                {
                    string currentDir = Environment.CurrentDirectory;
                    bool changesMade;
                    AStyleWhore.AStyleDirectory(currentDir, true, out changesMade);
                    if (changesMade)
                        return 1;
                }
                else if(args[0].Equals("Check", StringComparison.CurrentCultureIgnoreCase))
                {
                    string currentDir = Environment.CurrentDirectory;
                    bool changesMade;
                    var result = AStyleWhore.AStyleDirectory(currentDir, false, out changesMade);
                    //AttachConsole(-1);
                    if (changesMade)
                    {
                        Console.Error.WriteLine("Nonconforming files:");
                        Console.Error.Write(result);
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("Formatting fully conforming!");
                    }
                }
                else
                {
                    MessageBox.Show(args[0]);
                }
                return 0;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            return 0;
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();
    }
}
