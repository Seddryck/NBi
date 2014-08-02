namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class SettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.settingsValue = new System.Windows.Forms.TextBox();
            this.settingsName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.removeReference = new System.Windows.Forms.Button();
            this.addReference = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // settingsValue
            // 
            this.settingsValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsValue.Location = new System.Drawing.Point(0, 30);
            this.settingsValue.Multiline = true;
            this.settingsValue.Name = "settingsValue";
            this.settingsValue.Size = new System.Drawing.Size(571, 107);
            this.settingsValue.TabIndex = 3;
            // 
            // settingsName
            // 
            this.settingsName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsName.FormattingEnabled = true;
            this.settingsName.Location = new System.Drawing.Point(54, 3);
            this.settingsName.Name = "settingsName";
            this.settingsName.Size = new System.Drawing.Size(212, 21);
            this.settingsName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Settings:";
            // 
            // removeReference
            // 
            this.removeReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeReference.Image = global::NBi.UI.Genbi.Properties.Resources.tag_blue_delete;
            this.removeReference.Location = new System.Drawing.Point(301, 3);
            this.removeReference.Name = "removeReference";
            this.removeReference.Size = new System.Drawing.Size(24, 24);
            this.removeReference.TabIndex = 20;
            this.removeReference.UseVisualStyleBackColor = true;
            // 
            // addReference
            // 
            this.addReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addReference.Image = global::NBi.UI.Genbi.Properties.Resources.tag_blue_add;
            this.addReference.Location = new System.Drawing.Point(272, 3);
            this.addReference.Name = "addReference";
            this.addReference.Size = new System.Drawing.Size(24, 24);
            this.addReference.TabIndex = 19;
            this.addReference.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.removeReference);
            this.Controls.Add(this.addReference);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingsValue);
            this.Controls.Add(this.settingsName);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(574, 140);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox settingsValue;
        private System.Windows.Forms.ComboBox settingsName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button removeReference;
        private System.Windows.Forms.Button addReference;
    }
}
