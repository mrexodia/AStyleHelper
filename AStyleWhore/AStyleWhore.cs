using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AStyleWhore
{
    class AStyleWhore
    {
        private static string[] GetFilesInDir(string dir, string pattern)
        {
            string[] patterns = pattern.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (patterns.Length == 0)
                return Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
            string[] retlist = new string[] { };
            foreach (string searchPattern in patterns)
                retlist = retlist.Concat(Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)).ToArray();
            return retlist;
        }

        public static string AStyleDirectory(string dir, ref bool changesMade, string pattern = "*.c;*.h;*.cpp;*.hpp", string options = "style=allman, convert-tabs")
        {
            changesMade = false;
            //get a list of source files
            string[] sources = GetFilesInDir(dir, pattern);

            if (sources.Length == 0)
            {
                return "No source files found!";
            }

            //format the files
            string errors = "";
            AStyleInterface AStyle = new AStyleInterface();
            foreach (string file in sources)
            {
                if (file.Contains("bin\\") || file.Contains("release\\")) //skip Qt build files
                    continue;
                string fileText = File.ReadAllText(file);
                string formatText = AStyle.FormatSource(fileText, options);
                if (formatText == String.Empty)
                {
                    errors += "Cannot format " + file + "\r\n";
                    continue;
                }
                if (!fileText.Equals(formatText))
                {
                    changesMade = true;
                    File.WriteAllText(file, formatText);
                }
            }
            return errors;
        }
    }
}
