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
            this.components = new System.ComponentModel.Container();
            this.bindingSettings = new System.Windows.Forms.BindingSource(this.components);
            this.settingsValue = new System.Windows.Forms.TextBox();
            this.settingsName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSettings)).BeginInit();
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
            this.settingsValue.TextChanged += new System.EventHandler(this.SettingsValue_TextChanged);
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
            this.settingsName.SelectedIndexChanged += new System.EventHandler(this.SettingsName_SelectedIndexChanged);
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
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingsValue);
            this.Controls.Add(this.settingsName);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(574, 140);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.BindingSource bindingSettings;
        private System.Windows.Forms.TextBox settingsValue;
        private System.Windows.Forms.ComboBox settingsName;
        private System.Windows.Forms.Label label1;
    }
}
