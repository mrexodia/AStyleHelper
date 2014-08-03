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
                    string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                    bool changesMade = false;
                    AStyleWhore.AStyleDirectory(currentDir, ref changesMade);
                    if (changesMade)
                        return 1;
                }
                else
                {
                    MessageBox.Show(args[0]);
                }
                return 0;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            return 0;
        }
    }
}
