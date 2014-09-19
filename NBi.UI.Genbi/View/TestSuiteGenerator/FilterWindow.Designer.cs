namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class FilterWindow
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
            this.negation = new System.Windows.Forms.CheckBox();
            this.@operator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.text = new System.Windows.Forms.TextBox();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // variableLabel
            // 
            this.variableLabel.AutoSize = true;
            this.variableLabel.Location = new System.Drawing.Point(12, 9);
            this.variableLabel.Name = "variableLabel";
            this.variableLabel.Size = new System.Drawing.Size(51, 13);
            this.variableLabel.TabIndex = 3;
            this.variableLabel.Text = "Operator:";
            // 
            // negation
            // 
            this.negation.AutoSize = true;
            this.negation.Location = new System.Drawing.Point(347, 6);
            this.negation.Name = "negation";
            this.negation.Size = new System.Drawing.Size(43, 17);
            this.negation.TabIndex = 4;
            this.negation.Text = "Not";
            this.negation.UseVisualStyleBackColor = true;
            // 
            // @operator
            // 
            this.@operator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@operator.FormattingEnabled = true;
            this.@operator.Items.AddRange(new object[] {
            "Equal",
            "Like"});
            this.@operator.Location = new System.Drawing.Point(69, 4);
            this.@operator.Name = "@operator";
            this.@operator.Size = new System.Drawing.Size(121, 21);
            this.@operator.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Text:";
            // 
            // text
            // 
            this.text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text.Location = new System.Drawing.Point(70, 31);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(320, 20);
            this.text.TabIndex = 9;
            // 
            // apply
            // 
            this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.apply.Location = new System.Drawing.Point(234, 59);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 8;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(315, 59);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // FilterWindow
            // 
            this.AcceptButton = this.apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(402, 94);
            this.Controls.Add(this.text);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.@operator);
            this.Controls.Add(this.negation);
            this.Controls.Add(this.variableLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Filter test-cases";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label variableLabel;
        private System.Windows.Forms.CheckBox negation;
        private System.Windows.Forms.ComboBox @operator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
    }
}