namespace NBi.UI.Genbi.View.Generator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.settingsValue = new System.Windows.Forms.TextBox();
            this.settingsName = new System.Windows.Forms.ComboBox();
            this.bindingSettings = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.settingsValue);
            this.groupBox1.Controls.Add(this.settingsName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(571, 99);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // settingsValue
            // 
            this.settingsValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsValue.Location = new System.Drawing.Point(7, 48);
            this.settingsValue.Multiline = true;
            this.settingsValue.Name = "settingsValue";
            this.settingsValue.Size = new System.Drawing.Size(558, 39);
            this.settingsValue.TabIndex = 1;
            this.settingsValue.TextChanged += new System.EventHandler(this.SettingsValue_TextChanged);
            // 
            // settingsName
            // 
            this.settingsName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsName.FormattingEnabled = true;
            this.settingsName.Location = new System.Drawing.Point(7, 20);
            this.settingsName.Name = "settingsName";
            this.settingsName.Size = new System.Drawing.Size(235, 21);
            this.settingsName.TabIndex = 0;
            this.settingsName.SelectedIndexChanged += new System.EventHandler(this.SettingsName_SelectedIndexChanged);
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(588, 102);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox settingsValue;
        private System.Windows.Forms.ComboBox settingsName;
        protected internal System.Windows.Forms.BindingSource bindingSettings;
    }
}
