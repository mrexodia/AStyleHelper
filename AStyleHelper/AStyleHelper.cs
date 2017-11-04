﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using AStyleHelper.Properties;
using System.Text.RegularExpressions;

namespace AStyleHelper
{
    class AStyleHelper
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
                p.StartInfo.Arguments = "ls-files --cached --others " + pattern.Replace(';', ' ');
                p.Start();
                var output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (p.ExitCode != 0)
                    return GetFilesInDir(dir, pattern);
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
            var license = Settings.Default.License.Replace("\r", "").Replace("\n", "\r\n");
            var ignore = Settings.Default.Ignore.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            changesMade = false;

            // Get a list of source files
            string[] sources = GitLsFiles(dir, pattern);

            if (sources.Length <= 0)
                return "No source files found!";

            // Format the files
            var output = new StringBuilder();

            var AStyle = new AStyleInterface();
            var encoding = new UTF8Encoding(false);
            foreach (string file in sources)
            {
                try
                {
                    if (ignore.Any(p => Regex.IsMatch(file, p)))
                        continue;

                    var fileText = encoding.GetString(File.ReadAllBytes(file));
                    var formatText = AStyle.FormatSource(fileText, options)
                        .Replace("\r\n", "\n")
                        .Replace("\n", "\r\n")
                        .Trim('\uFEFF', '\u200B');

                    if (license.Length > 0 && !formatText.StartsWith(license))
                        formatText = license + fileText;

                    if (formatText == String.Empty)
                    {
                        output.AppendLine("Cannot format " + file);
                        continue;
                    }
                    if (!fileText.Equals(formatText))
                    {
                        changesMade = true;
                        if (writeChanges)
                            File.WriteAllText(file, formatText, encoding);
                        else
                            output.AppendLine(file);
                    }
                }
                catch
                {
                    // Ignored. Any files that can't be read or written will be skipped.
                }
            }

            return output.ToString();
        }
    }
}
