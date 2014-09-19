namespace NBi.UI
{
    partial class MetadataSave
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
            this.useExistingSheet = new System.Windows.Forms.RadioButton();
            this.createNewSheet = new System.Windows.Forms.RadioButton();
            this.existingSheetSelected = new System.Windows.Forms.ComboBox();
            this.newSheetName = new System.Windows.Forms.TextBox();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // useExistingSheet
            // 
            this.useExistingSheet.AutoSize = true;
            this.useExistingSheet.Checked = true;
            this.useExistingSheet.Location = new System.Drawing.Point(13, 13);
            this.useExistingSheet.Name = "useExistingSheet";
            this.useExistingSheet.Size = new System.Drawing.Size(111, 17);
            this.useExistingSheet.TabIndex = 0;
            this.useExistingSheet.TabStop = true;
            this.useExistingSheet.Text = "Use existing sheet";
            this.useExistingSheet.UseVisualStyleBackColor = true;
            this.useExistingSheet.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // createNewSheet
            // 
            this.createNewSheet.AutoSize = true;
            this.createNewSheet.Location = new System.Drawing.Point(13, 41);
            this.createNewSheet.Name = "createNewSheet";
            this.createNewSheet.Size = new System.Drawing.Size(108, 17);
            this.createNewSheet.TabIndex = 1;
            this.createNewSheet.Text = "Create new sheet";
            this.createNewSheet.UseVisualStyleBackColor = true;
            this.createNewSheet.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // existingSheetSelected
            // 
            this.existingSheetSelected.FormattingEnabled = true;
            this.existingSheetSelected.Location = new System.Drawing.Point(131, 13);
            this.existingSheetSelected.Name = "existingSheetSelected";
            this.existingSheetSelected.Size = new System.Drawing.Size(211, 21);
            this.existingSheetSelected.TabIndex = 2;
            this.existingSheetSelected.SelectedIndexChanged += new System.EventHandler(this.existingSheetSelected_SelectedIndexChanged);
            // 
            // newSheetName
            // 
            this.newSheetName.Enabled = false;
            this.newSheetName.Location = new System.Drawing.Point(131, 40);
            this.newSheetName.Name = "newSheetName";
            this.newSheetName.Size = new System.Drawing.Size(211, 20);
            this.newSheetName.TabIndex = 3;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(267, 67);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(186, 67);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 6;
            this.ok.Text = "&OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // MetadataSave
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(354, 102);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.newSheetName);
            this.Controls.Add(this.existingSheetSelected);
            this.Controls.Add(this.createNewSheet);
            this.Controls.Add(this.useExistingSheet);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MetadataSave";
            this.Text = "Save metadata definition ...";
            this.Load += new System.EventHandler(this.MetadataSave_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton useExistingSheet;
        private System.Windows.Forms.RadioButton createNewSheet;
        private System.Windows.Forms.ComboBox existingSheetSelected;
        private System.Windows.Forms.TextBox newSheetName;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
    }
}