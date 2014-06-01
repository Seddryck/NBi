namespace NBi.UI.Genbi.View.RunnerConfig
{
    partial class PathControl
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
            this.label = new System.Windows.Forms.Label();
            this.path = new System.Windows.Forms.TextBox();
            this.openDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(3, 3);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(58, 13);
            this.label.TabIndex = 0;
            this.label.Text = "Root Path:";
            // 
            // path
            // 
            this.path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.path.Location = new System.Drawing.Point(98, 0);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(249, 20);
            this.path.TabIndex = 1;
            this.path.TextChanged += new System.EventHandler(this.Path_TextChanged);
            // 
            // openDialog
            // 
            this.openDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openDialog.Location = new System.Drawing.Point(353, 0);
            this.openDialog.Name = "openDialog";
            this.openDialog.Size = new System.Drawing.Size(28, 22);
            this.openDialog.TabIndex = 2;
            this.openDialog.Text = "...";
            this.openDialog.UseVisualStyleBackColor = true;
            this.openDialog.Click += new System.EventHandler(this.OpenDialog_Click);
            // 
            // PathControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openDialog);
            this.Controls.Add(this.path);
            this.Controls.Add(this.label);
            this.Name = "PathControl";
            this.Size = new System.Drawing.Size(386, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.Button openDialog;
    }
}
