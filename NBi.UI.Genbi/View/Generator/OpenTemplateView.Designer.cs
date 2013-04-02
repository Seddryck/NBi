namespace NBi.UI.Genbi.View.Generator
{
    partial class OpenTemplateView
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
            this.components = new System.ComponentModel.Container();
            this.cancel = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            this.predefinedTemplateName = new System.Windows.Forms.ComboBox();
            this.openUserTemplate = new System.Windows.Forms.Button();
            this.isPredefinedTemplate = new System.Windows.Forms.RadioButton();
            this.isUserTemplate = new System.Windows.Forms.RadioButton();
            this.userTemplateFullPath = new System.Windows.Forms.TextBox();
            this.BindingEmbeddedTemplates = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.BindingEmbeddedTemplates)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(313, 65);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 0;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // apply
            // 
            this.apply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.apply.Location = new System.Drawing.Point(232, 65);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 1;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // predefinedTemplateName
            // 
            this.predefinedTemplateName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.predefinedTemplateName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.predefinedTemplateName.FormattingEnabled = true;
            this.predefinedTemplateName.Location = new System.Drawing.Point(145, 8);
            this.predefinedTemplateName.Name = "predefinedTemplateName";
            this.predefinedTemplateName.Size = new System.Drawing.Size(243, 21);
            this.predefinedTemplateName.TabIndex = 3;
            // 
            // openUserTemplate
            // 
            this.openUserTemplate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.openUserTemplate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.openUserTemplate.Location = new System.Drawing.Point(354, 34);
            this.openUserTemplate.Name = "openUserTemplate";
            this.openUserTemplate.Size = new System.Drawing.Size(34, 23);
            this.openUserTemplate.TabIndex = 6;
            this.openUserTemplate.Text = "...";
            this.openUserTemplate.UseVisualStyleBackColor = true;
            this.openUserTemplate.Click += new System.EventHandler(this.UserTemplateFileSelection_Click);
            // 
            // isPredefinedTemplate
            // 
            this.isPredefinedTemplate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.isPredefinedTemplate.AutoSize = true;
            this.isPredefinedTemplate.Checked = true;
            this.isPredefinedTemplate.Location = new System.Drawing.Point(15, 9);
            this.isPredefinedTemplate.Name = "isPredefinedTemplate";
            this.isPredefinedTemplate.Size = new System.Drawing.Size(124, 17);
            this.isPredefinedTemplate.TabIndex = 7;
            this.isPredefinedTemplate.TabStop = true;
            this.isPredefinedTemplate.Text = "Predefined templates";
            this.isPredefinedTemplate.UseVisualStyleBackColor = true;
            this.isPredefinedTemplate.CheckedChanged += new System.EventHandler(this.TemplateChoice_CheckedChanged);
            // 
            // isUserTemplate
            // 
            this.isUserTemplate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.isUserTemplate.AutoSize = true;
            this.isUserTemplate.Location = new System.Drawing.Point(15, 36);
            this.isUserTemplate.Name = "isUserTemplate";
            this.isUserTemplate.Size = new System.Drawing.Size(98, 17);
            this.isUserTemplate.TabIndex = 8;
            this.isUserTemplate.TabStop = true;
            this.isUserTemplate.Text = "User templates:";
            this.isUserTemplate.UseVisualStyleBackColor = true;
            this.isUserTemplate.CheckedChanged += new System.EventHandler(this.TemplateChoice_CheckedChanged);
            // 
            // userTemplateFullPath
            // 
            this.userTemplateFullPath.Location = new System.Drawing.Point(145, 36);
            this.userTemplateFullPath.Name = "userTemplateFullPath";
            this.userTemplateFullPath.Size = new System.Drawing.Size(203, 20);
            this.userTemplateFullPath.TabIndex = 9;
            // 
            // OpenTemplateView
            // 
            this.AcceptButton = this.apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(402, 95);
            this.ControlBox = false;
            this.Controls.Add(this.userTemplateFullPath);
            this.Controls.Add(this.isUserTemplate);
            this.Controls.Add(this.isPredefinedTemplate);
            this.Controls.Add(this.openUserTemplate);
            this.Controls.Add(this.predefinedTemplateName);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.cancel);
            this.Name = "OpenTemplateView";
            this.Text = "Select template";
            this.Load += new System.EventHandler(this.OpenTemplateView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BindingEmbeddedTemplates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.ComboBox predefinedTemplateName;
        private System.Windows.Forms.Button openUserTemplate;
        private System.Windows.Forms.RadioButton isPredefinedTemplate;
        private System.Windows.Forms.RadioButton isUserTemplate;
        private System.Windows.Forms.TextBox userTemplateFullPath;
        public System.Windows.Forms.BindingSource BindingEmbeddedTemplates;
    }
}