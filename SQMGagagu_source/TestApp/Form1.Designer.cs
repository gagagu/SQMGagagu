namespace TestApp
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btSQMImport = new System.Windows.Forms.Button();
            this.btSQMExport = new System.Windows.Forms.Button();
            this.btCreateMissionString = new System.Windows.Forms.Button();
            this.btExportFirstImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 41);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(950, 525);
            this.textBox1.TabIndex = 0;
            // 
            // btSQMImport
            // 
            this.btSQMImport.Location = new System.Drawing.Point(346, 12);
            this.btSQMImport.Name = "btSQMImport";
            this.btSQMImport.Size = new System.Drawing.Size(135, 23);
            this.btSQMImport.TabIndex = 3;
            this.btSQMImport.Text = "Load Mission File";
            this.btSQMImport.UseVisualStyleBackColor = true;
            this.btSQMImport.Click += new System.EventHandler(this.btSQMImport_Click);
            // 
            // btSQMExport
            // 
            this.btSQMExport.Location = new System.Drawing.Point(186, 12);
            this.btSQMExport.Name = "btSQMExport";
            this.btSQMExport.Size = new System.Drawing.Size(154, 23);
            this.btSQMExport.TabIndex = 4;
            this.btSQMExport.Text = "Save Mission File";
            this.btSQMExport.UseVisualStyleBackColor = true;
            this.btSQMExport.Click += new System.EventHandler(this.btSQMExport_Click);
            // 
            // btCreateMissionString
            // 
            this.btCreateMissionString.Location = new System.Drawing.Point(12, 12);
            this.btCreateMissionString.Name = "btCreateMissionString";
            this.btCreateMissionString.Size = new System.Drawing.Size(168, 23);
            this.btCreateMissionString.TabIndex = 5;
            this.btCreateMissionString.Text = "Fill SQM Class with Data";
            this.btCreateMissionString.UseVisualStyleBackColor = true;
            this.btCreateMissionString.Click += new System.EventHandler(this.btCreateMissionString_Click);
            // 
            // btExportFirstImport
            // 
            this.btExportFirstImport.Location = new System.Drawing.Point(487, 12);
            this.btExportFirstImport.Name = "btExportFirstImport";
            this.btExportFirstImport.Size = new System.Drawing.Size(198, 23);
            this.btExportFirstImport.TabIndex = 6;
            this.btExportFirstImport.Text = "Export Mission but import before";
            this.btExportFirstImport.UseVisualStyleBackColor = true;
            this.btExportFirstImport.Click += new System.EventHandler(this.btExportFirstImport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 578);
            this.Controls.Add(this.btExportFirstImport);
            this.Controls.Add(this.btCreateMissionString);
            this.Controls.Add(this.btSQMExport);
            this.Controls.Add(this.btSQMImport);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Test App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btSQMImport;
        private System.Windows.Forms.Button btSQMExport;
        private System.Windows.Forms.Button btCreateMissionString;
        private System.Windows.Forms.Button btExportFirstImport;
    }
}

