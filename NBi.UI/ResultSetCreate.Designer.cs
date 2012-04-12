namespace NBi.UI
{
    partial class ResultSetCreate
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
        protected virtual void InitializeComponent()
        {
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.connectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.queriesDirectory = new System.Windows.Forms.TextBox();
            this.resultsDirectory = new System.Windows.Forms.TextBox();
            this.resultsDirectorySelect = new System.Windows.Forms.Button();
            this.queriesDirectorySelect = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(267, 121);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(186, 121);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 8;
            this.ok.Text = "&OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // connectionString
            // 
            this.connectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionString.Location = new System.Drawing.Point(112, 60);
            this.connectionString.Multiline = true;
            this.connectionString.Name = "connectionString";
            this.connectionString.Size = new System.Drawing.Size(230, 55);
            this.connectionString.TabIndex = 11;
            this.connectionString.Text = "Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Connection String:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Results directory:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Queries directory:";
            // 
            // queriesDirectory
            // 
            this.queriesDirectory.Location = new System.Drawing.Point(112, 9);
            this.queriesDirectory.Name = "queriesDirectory";
            this.queriesDirectory.ReadOnly = true;
            this.queriesDirectory.Size = new System.Drawing.Size(187, 20);
            this.queriesDirectory.TabIndex = 14;
            // 
            // resultsDirectory
            // 
            this.resultsDirectory.Location = new System.Drawing.Point(112, 35);
            this.resultsDirectory.Name = "resultsDirectory";
            this.resultsDirectory.ReadOnly = true;
            this.resultsDirectory.Size = new System.Drawing.Size(187, 20);
            this.resultsDirectory.TabIndex = 15;
            // 
            // resultsDirectorySelect
            // 
            this.resultsDirectorySelect.Location = new System.Drawing.Point(306, 9);
            this.resultsDirectorySelect.Name = "resultsDirectorySelect";
            this.resultsDirectorySelect.Size = new System.Drawing.Size(37, 19);
            this.resultsDirectorySelect.TabIndex = 16;
            this.resultsDirectorySelect.Text = "...";
            this.resultsDirectorySelect.UseVisualStyleBackColor = true;
            this.resultsDirectorySelect.Click += new System.EventHandler(this.resultsDirectorySelect_Click);
            // 
            // queriesDirectorySelect
            // 
            this.queriesDirectorySelect.Location = new System.Drawing.Point(305, 36);
            this.queriesDirectorySelect.Name = "queriesDirectorySelect";
            this.queriesDirectorySelect.Size = new System.Drawing.Size(37, 19);
            this.queriesDirectorySelect.TabIndex = 17;
            this.queriesDirectorySelect.Text = "...";
            this.queriesDirectorySelect.UseVisualStyleBackColor = true;
            this.queriesDirectorySelect.Click += new System.EventHandler(this.queriesDirectorySelect_Click);
            // 
            // ResultSetCreate
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(354, 153);
            this.Controls.Add(this.queriesDirectorySelect);
            this.Controls.Add(this.resultsDirectorySelect);
            this.Controls.Add(this.resultsDirectory);
            this.Controls.Add(this.queriesDirectory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.connectionString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultSetCreate";
            this.Text = "Create resultset ...";
            this.Load += new System.EventHandler(this.ResultSetCreate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.TextBox connectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox queriesDirectory;
        private System.Windows.Forms.TextBox resultsDirectory;
        private System.Windows.Forms.Button resultsDirectorySelect;
        private System.Windows.Forms.Button queriesDirectorySelect;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}