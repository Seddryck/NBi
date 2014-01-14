namespace NBi.UI.Genbi.View.RunnerConfig
{
    partial class RunnerConfigView
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
            this.testSuiteFile = new NBi.UI.Genbi.View.RunnerConfig.FileControl();
            this.frameworkPath = new NBi.UI.Genbi.View.RunnerConfig.PathControl();
            this.rootPath = new NBi.UI.Genbi.View.RunnerConfig.PathControl();
            this.buildNUnit = new System.Windows.Forms.CheckBox();
            this.buildGallio = new System.Windows.Forms.CheckBox();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testSuiteFile
            // 
            this.testSuiteFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testSuiteFile.Label = "Test-Suite File:";
            this.testSuiteFile.Location = new System.Drawing.Point(12, 70);
            this.testSuiteFile.Name = "testSuiteFile";
            this.testSuiteFile.Path = "";
            this.testSuiteFile.Size = new System.Drawing.Size(426, 23);
            this.testSuiteFile.TabIndex = 2;
            // 
            // frameworkPath
            // 
            this.frameworkPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frameworkPath.Label = "Framework Path:";
            this.frameworkPath.Location = new System.Drawing.Point(12, 41);
            this.frameworkPath.Name = "frameworkPath";
            this.frameworkPath.Path = "";
            this.frameworkPath.Size = new System.Drawing.Size(426, 23);
            this.frameworkPath.TabIndex = 1;
            // 
            // rootPath
            // 
            this.rootPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rootPath.Label = "Root Path:";
            this.rootPath.Location = new System.Drawing.Point(12, 12);
            this.rootPath.Name = "rootPath";
            this.rootPath.Path = "";
            this.rootPath.Size = new System.Drawing.Size(426, 23);
            this.rootPath.TabIndex = 0;
            // 
            // buildNUnit
            // 
            this.buildNUnit.AutoSize = true;
            this.buildNUnit.Checked = true;
            this.buildNUnit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buildNUnit.Location = new System.Drawing.Point(19, 109);
            this.buildNUnit.Name = "buildNUnit";
            this.buildNUnit.Size = new System.Drawing.Size(53, 17);
            this.buildNUnit.TabIndex = 3;
            this.buildNUnit.Text = "NUnit";
            this.buildNUnit.UseVisualStyleBackColor = true;
            // 
            // buildGallio
            // 
            this.buildGallio.AutoSize = true;
            this.buildGallio.Checked = true;
            this.buildGallio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buildGallio.Location = new System.Drawing.Point(19, 132);
            this.buildGallio.Name = "buildGallio";
            this.buildGallio.Size = new System.Drawing.Size(92, 17);
            this.buildGallio.TabIndex = 4;
            this.buildGallio.Text = "Gallio (Icarius)";
            this.buildGallio.UseVisualStyleBackColor = true;
            // 
            // apply
            // 
            this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply.Location = new System.Drawing.Point(282, 151);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 6;
            this.apply.Text = "&Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(363, 151);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // RunnerConfigView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 186);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.buildGallio);
            this.Controls.Add(this.buildNUnit);
            this.Controls.Add(this.testSuiteFile);
            this.Controls.Add(this.frameworkPath);
            this.Controls.Add(this.rootPath);
            this.Name = "RunnerConfigView";
            this.Text = "Runners\' config builder";
            this.Load += new System.EventHandler(this.RunnerConfigView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PathControl rootPath;
        private PathControl frameworkPath;
        private FileControl testSuiteFile;
        private System.Windows.Forms.CheckBox buildNUnit;
        private System.Windows.Forms.CheckBox buildGallio;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
    }
}