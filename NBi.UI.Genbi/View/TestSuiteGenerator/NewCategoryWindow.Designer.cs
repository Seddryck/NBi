namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class NewCategoryWindow
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
            this.variableLabel = new System.Windows.Forms.Label();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.categoryName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // variableLabel
            // 
            this.variableLabel.AutoSize = true;
            this.variableLabel.Location = new System.Drawing.Point(11, 14);
            this.variableLabel.Name = "variableLabel";
            this.variableLabel.Size = new System.Drawing.Size(88, 13);
            this.variableLabel.TabIndex = 6;
            this.variableLabel.Text = "Category\'s name:";
            // 
            // apply
            // 
            this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.apply.Location = new System.Drawing.Point(234, 43);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 5;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(315, 43);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // categoryName
            // 
            this.categoryName.FormattingEnabled = true;
            this.categoryName.Location = new System.Drawing.Point(105, 11);
            this.categoryName.Name = "categoryName";
            this.categoryName.Size = new System.Drawing.Size(284, 21);
            this.categoryName.TabIndex = 7;
            // 
            // NewCategoryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 77);
            this.Controls.Add(this.categoryName);
            this.Controls.Add(this.variableLabel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.cancel);
            this.Name = "NewCategoryWindow";
            this.Text = "Add a category";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label variableLabel;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ComboBox categoryName;
    }
}