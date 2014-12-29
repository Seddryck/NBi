namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class OpenQueryWindow
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
            this.queryFile = new System.Windows.Forms.TextBox();
            this.openQueryFile = new System.Windows.Forms.Button();
            this.variableLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.connectionString = new System.Windows.Forms.TextBox();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // queryFile
            // 
            this.queryFile.Location = new System.Drawing.Point(80, 12);
            this.queryFile.Name = "queryFile";
            this.queryFile.Size = new System.Drawing.Size(271, 20);
            this.queryFile.TabIndex = 11;
            // 
            // openQueryFile
            // 
            this.openQueryFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.openQueryFile.Location = new System.Drawing.Point(357, 10);
            this.openQueryFile.Name = "openQueryFile";
            this.openQueryFile.Size = new System.Drawing.Size(34, 23);
            this.openQueryFile.TabIndex = 10;
            this.openQueryFile.Text = "...";
            this.openQueryFile.UseVisualStyleBackColor = true;
            this.openQueryFile.Click += new System.EventHandler(this.QueryFileSelection_Click);
            // 
            // variableLabel
            // 
            this.variableLabel.AutoSize = true;
            this.variableLabel.Location = new System.Drawing.Point(13, 15);
            this.variableLabel.Name = "variableLabel";
            this.variableLabel.Size = new System.Drawing.Size(61, 13);
            this.variableLabel.TabIndex = 12;
            this.variableLabel.Text = "Query\'s file:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Connection string:";
            // 
            // connectionString
            // 
            this.connectionString.Location = new System.Drawing.Point(110, 41);
            this.connectionString.Name = "connectionString";
            this.connectionString.Size = new System.Drawing.Size(280, 20);
            this.connectionString.TabIndex = 14;
            // 
            // apply
            // 
            this.apply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.apply.Location = new System.Drawing.Point(235, 67);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 16;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(316, 67);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 15;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // OpenQueryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 94);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.connectionString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variableLabel);
            this.Controls.Add(this.queryFile);
            this.Controls.Add(this.openQueryFile);
            this.Name = "OpenQueryWindow";
            this.Text = "Select query and connection-string";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox queryFile;
        private System.Windows.Forms.Button openQueryFile;
        private System.Windows.Forms.Label variableLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox connectionString;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
    }
}