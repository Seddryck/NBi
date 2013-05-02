namespace NBi.UI.Genbi.View.Generator
{
    partial class TemplateControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.useGrouping = new System.Windows.Forms.CheckBox();
            this.template = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Test case template:";
            // 
            // useGrouping
            // 
            this.useGrouping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.useGrouping.AutoSize = true;
            this.useGrouping.Location = new System.Drawing.Point(420, 6);
            this.useGrouping.Name = "useGrouping";
            this.useGrouping.Size = new System.Drawing.Size(89, 17);
            this.useGrouping.TabIndex = 22;
            this.useGrouping.Text = "Use grouping";
            this.useGrouping.UseVisualStyleBackColor = true;
            // 
            // template
            // 
            this.template.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.template.Location = new System.Drawing.Point(0, 29);
            this.template.Multiline = true;
            this.template.Name = "template";
            this.template.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.template.Size = new System.Drawing.Size(509, 384);
            this.template.TabIndex = 21;
            this.template.WordWrap = false;
            this.template.TextChanged += new System.EventHandler(this.Template_TextChanged);
            // 
            // TemplateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.useGrouping);
            this.Controls.Add(this.template);
            this.Name = "TemplateControl";
            this.Size = new System.Drawing.Size(512, 416);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        protected internal System.Windows.Forms.CheckBox useGrouping;
        private System.Windows.Forms.TextBox template;
    }
}
