namespace AStyleWhore
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnAStyleDirectory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAStyleDirectory
            // 
            this.btnAStyleDirectory.Location = new System.Drawing.Point(12, 12);
            this.btnAStyleDirectory.Name = "btnAStyleDirectory";
            this.btnAStyleDirectory.Size = new System.Drawing.Size(261, 23);
            this.btnAStyleDirectory.TabIndex = 0;
            this.btnAStyleDirectory.Text = "AStyle Current Directory";
            this.btnAStyleDirectory.UseVisualStyleBackColor = true;
            this.btnAStyleDirectory.Click += new System.EventHandler(this.btnAStyleDirectory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 49);
            this.Controls.Add(this.btnAStyleDirectory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AStyleWhore";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAStyleDirectory;
    }
}

