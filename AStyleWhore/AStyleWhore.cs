using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using AStyleWhore.Properties;
using System.Text.RegularExpressions;

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

        private static string[] GitLsFiles(string dir, string pattern)
        {
            try
            {
                var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "git";
                p.StartInfo.Arguments = "ls-files " + pattern.Replace(';', ' ');
                p.Start();
                var output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                return output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch
            {
                return GetFilesInDir(dir, pattern);
            }
        }

        public static string AStyleDirectory(string dir, bool writeChanges, out bool changesMade)
        {
            var pattern = Settings.Default.Pattern;
            var options = Settings.Default.Options;
            var ignore = Settings.Default.Ignore.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            changesMade = false;

            // Get a list of source files
            string[] sources = GitLsFiles(dir, pattern);

            if (sources.Length <= 0)
                return "No source files found!";

            // Format the files
            var output = new StringBuilder();

            var AStyle = new AStyleInterface();
            foreach (string file in sources)
            {
                try
                {
                    if (ignore.Any(p => Regex.IsMatch(file, p)))
                        continue;

                    string fileText = File.ReadAllText(file);
                    string formatText = AStyle.FormatSource(fileText, options).Replace("\r\n", "\n").Replace("\n", "\r\n");

                    if (formatText == String.Empty)
                    {
                        output.AppendLine("Cannot format " + file);
                        continue;
                    }
                    if (!fileText.Equals(formatText))
                    {
                        changesMade = true;
                        if (writeChanges)
                            File.WriteAllText(file, formatText);
                        else
                            output.AppendLine(file);
                    }
                }
                catch (Exception)
                {
                    // Ignored. Any files that can't be read or written will be skipped.
                }
            }

            return output.ToString();
        }
    }
}
