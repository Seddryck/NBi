namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class MacroWindow
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
            this.actionInfoText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // actionInfoText
            // 
            this.actionInfoText.CausesValidation = false;
            this.actionInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionInfoText.Location = new System.Drawing.Point(0, 0);
            this.actionInfoText.Multiline = true;
            this.actionInfoText.Name = "actionInfoText";
            this.actionInfoText.ReadOnly = true;
            this.actionInfoText.Size = new System.Drawing.Size(402, 380);
            this.actionInfoText.TabIndex = 0;
            // 
            // MacroWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 380);
            this.Controls.Add(this.actionInfoText);
            this.Name = "MacroWindow";
            this.Text = "Macro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox actionInfoText;
    }
}