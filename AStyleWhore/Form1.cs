using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AStyleWhore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAStyleDirectory_Click(object sender, EventArgs e)
        {
            //ask if the users wants to format
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            if (MessageBox.Show(currentDir, "Do you want to AStyle this directory?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            //get a list of source files
            string[] headers = Directory.GetFiles(currentDir, "*.h", SearchOption.AllDirectories);
            string[] cpps = Directory.GetFiles(currentDir, "*.cpp", SearchOption.AllDirectories);
            string[] sources = headers.Concat(cpps).ToArray();

            //format the files
            string errors = "";
            string options = "style=allman, convert-tabs";
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
                    File.WriteAllText(file, formatText);
            }

            //finalize
            if (errors.Length > 0)
                MessageBox.Show(errors, "Error(s) Found!");
            else
                MessageBox.Show("All done!");
        }
    }
}
