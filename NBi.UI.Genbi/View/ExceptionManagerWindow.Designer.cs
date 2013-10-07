namespace NBi.UI.Genbi.View
{
    partial class ExceptionManagerWindow
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.exceptionLog = new System.Windows.Forms.RichTextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(527, 40);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "An unhandled exception occurred in Genbi. Please report the following message to " +
    "the author, at http://nbi.codeplex.com.\n";
            // 
            // exceptionLog
            // 
            this.exceptionLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exceptionLog.BackColor = System.Drawing.Color.White;
            this.exceptionLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionLog.Location = new System.Drawing.Point(12, 58);
            this.exceptionLog.Name = "exceptionLog";
            this.exceptionLog.ReadOnly = true;
            this.exceptionLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.exceptionLog.Size = new System.Drawing.Size(527, 308);
            this.exceptionLog.TabIndex = 1;
            this.exceptionLog.Text = "";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(463, 377);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.Ok_Click);
            // 
            // ExceptionManagerWindow0
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 412);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.exceptionLog);
            this.Controls.Add(this.richTextBox1);
            this.MinimizeBox = false;
            this.Name = "ExceptionManagerWindow0";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Unhandled exception";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox exceptionLog;
        private System.Windows.Forms.Button okButton;
    }
}