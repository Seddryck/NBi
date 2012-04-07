namespace NBi.UI
{
    partial class MetadataOpen
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
            this.trackSelected = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.sheetSelected = new System.Windows.Forms.ComboBox();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // trackSelected
            // 
            this.trackSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackSelected.FormattingEnabled = true;
            this.trackSelected.Location = new System.Drawing.Point(125, 38);
            this.trackSelected.Name = "trackSelected";
            this.trackSelected.Size = new System.Drawing.Size(217, 21);
            this.trackSelected.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select track (optional)";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(186, 67);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 2;
            this.ok.Text = "&OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select sheet";
            // 
            // sheetSelected
            // 
            this.sheetSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sheetSelected.FormattingEnabled = true;
            this.sheetSelected.Location = new System.Drawing.Point(125, 6);
            this.sheetSelected.Name = "sheetSelected";
            this.sheetSelected.Size = new System.Drawing.Size(217, 21);
            this.sheetSelected.TabIndex = 3;
            this.sheetSelected.SelectedIndexChanged += new System.EventHandler(this.sheetSelected_SelectedIndexChanged);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(267, 67);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // MetadataOpen
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(354, 102);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sheetSelected);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackSelected);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MetadataOpen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Open metadata definition";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TrackSelection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox trackSelected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sheetSelected;
        private System.Windows.Forms.Button cancel;
    }
}