using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AStyleHelper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAStyleDirectory_Click(object sender, EventArgs e)
        {
            //ask if the users wants to format
            string currentDir = Environment.CurrentDirectory;
            if (MessageBox.Show(currentDir, "Do you want to AStyle this directory?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            bool changesMade;
            string errors = AStyleHelper.AStyleDirectory(currentDir, true, out changesMade);

            //finalize
            if (errors.Length > 0)
                MessageBox.Show(errors, "Error(s) Found!");
            else if (!changesMade)
                MessageBox.Show("All files were already formatted...");
            else
                MessageBox.Show("Files formatted!");
        }
    }
}
