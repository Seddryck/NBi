namespace NBi.UI
{
    partial class TestSuiteCreate
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
        protected virtual void InitializeComponent()
        {
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.queriesDirectoryActual = new System.Windows.Forms.TextBox();
            this.queriesDirectoryActualSelect = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectionStringActual = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultSetsDirectoryExpect = new System.Windows.Forms.TextBox();
            this.resultSetsDirectoryExpectSelect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.connectionStringExpect = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.queriesDirectoryExpectSelect = new System.Windows.Forms.Button();
            this.queriesDirectoryExpect = new System.Windows.Forms.TextBox();
            this.isQueriesDirectory = new System.Windows.Forms.RadioButton();
            this.isResultSetsDirectory = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(475, 302);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(394, 302);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 8;
            this.ok.Text = "&OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Queries directory:";
            // 
            // queriesDirectoryActual
            // 
            this.queriesDirectoryActual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queriesDirectoryActual.Location = new System.Drawing.Point(118, 25);
            this.queriesDirectoryActual.Name = "queriesDirectoryActual";
            this.queriesDirectoryActual.ReadOnly = true;
            this.queriesDirectoryActual.Size = new System.Drawing.Size(357, 20);
            this.queriesDirectoryActual.TabIndex = 14;
            // 
            // queriesDirectoryActualSelect
            // 
            this.queriesDirectoryActualSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queriesDirectoryActualSelect.Location = new System.Drawing.Point(481, 25);
            this.queriesDirectoryActualSelect.Name = "queriesDirectoryActualSelect";
            this.queriesDirectoryActualSelect.Size = new System.Drawing.Size(37, 19);
            this.queriesDirectoryActualSelect.TabIndex = 17;
            this.queriesDirectoryActualSelect.Text = "...";
            this.queriesDirectoryActualSelect.UseVisualStyleBackColor = true;
            this.queriesDirectoryActualSelect.Click += new System.EventHandler(this.queriesDirectoryActualSelect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.connectionStringActual);
            this.groupBox1.Controls.Add(this.queriesDirectoryActualSelect);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.queriesDirectoryActual);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 122);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actuals";
            // 
            // connectionStringActual
            // 
            this.connectionStringActual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionStringActual.Location = new System.Drawing.Point(118, 51);
            this.connectionStringActual.Multiline = true;
            this.connectionStringActual.Name = "connectionStringActual";
            this.connectionStringActual.Size = new System.Drawing.Size(398, 55);
            this.connectionStringActual.TabIndex = 13;
            this.connectionStringActual.Text = "Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Connection String:";
            // 
            // resultSetsDirectoryExpect
            // 
            this.resultSetsDirectoryExpect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultSetsDirectoryExpect.Location = new System.Drawing.Point(148, 24);
            this.resultSetsDirectoryExpect.Name = "resultSetsDirectoryExpect";
            this.resultSetsDirectoryExpect.ReadOnly = true;
            this.resultSetsDirectoryExpect.Size = new System.Drawing.Size(327, 20);
            this.resultSetsDirectoryExpect.TabIndex = 18;
            // 
            // resultSetsDirectoryExpectSelect
            // 
            this.resultSetsDirectoryExpectSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultSetsDirectoryExpectSelect.Location = new System.Drawing.Point(481, 24);
            this.resultSetsDirectoryExpectSelect.Name = "resultSetsDirectoryExpectSelect";
            this.resultSetsDirectoryExpectSelect.Size = new System.Drawing.Size(37, 19);
            this.resultSetsDirectoryExpectSelect.TabIndex = 19;
            this.resultSetsDirectoryExpectSelect.Text = "...";
            this.resultSetsDirectoryExpectSelect.UseVisualStyleBackColor = true;
            this.resultSetsDirectoryExpectSelect.Click += new System.EventHandler(this.resultSetsDirectoryExpectSelect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.connectionStringExpect);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.queriesDirectoryExpectSelect);
            this.groupBox2.Controls.Add(this.queriesDirectoryExpect);
            this.groupBox2.Controls.Add(this.isQueriesDirectory);
            this.groupBox2.Controls.Add(this.isResultSetsDirectory);
            this.groupBox2.Controls.Add(this.resultSetsDirectoryExpectSelect);
            this.groupBox2.Controls.Add(this.resultSetsDirectoryExpect);
            this.groupBox2.Location = new System.Drawing.Point(14, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(536, 146);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Expectations";
            // 
            // connectionStringExpect
            // 
            this.connectionStringExpect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionStringExpect.Location = new System.Drawing.Point(148, 76);
            this.connectionStringExpect.Multiline = true;
            this.connectionStringExpect.Name = "connectionStringExpect";
            this.connectionStringExpect.Size = new System.Drawing.Size(370, 55);
            this.connectionStringExpect.TabIndex = 25;
            this.connectionStringExpect.Text = "Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Connection String:";
            // 
            // queriesDirectoryExpectSelect
            // 
            this.queriesDirectoryExpectSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queriesDirectoryExpectSelect.Location = new System.Drawing.Point(481, 48);
            this.queriesDirectoryExpectSelect.Name = "queriesDirectoryExpectSelect";
            this.queriesDirectoryExpectSelect.Size = new System.Drawing.Size(37, 19);
            this.queriesDirectoryExpectSelect.TabIndex = 23;
            this.queriesDirectoryExpectSelect.Text = "...";
            this.queriesDirectoryExpectSelect.UseVisualStyleBackColor = true;
            this.queriesDirectoryExpectSelect.Click += new System.EventHandler(this.queriesDirectoryExpectSelect_Click);
            // 
            // queriesDirectoryExpect
            // 
            this.queriesDirectoryExpect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queriesDirectoryExpect.Location = new System.Drawing.Point(148, 50);
            this.queriesDirectoryExpect.Name = "queriesDirectoryExpect";
            this.queriesDirectoryExpect.ReadOnly = true;
            this.queriesDirectoryExpect.Size = new System.Drawing.Size(327, 20);
            this.queriesDirectoryExpect.TabIndex = 22;
            // 
            // isQueriesDirectory
            // 
            this.isQueriesDirectory.AutoSize = true;
            this.isQueriesDirectory.Location = new System.Drawing.Point(21, 49);
            this.isQueriesDirectory.Name = "isQueriesDirectory";
            this.isQueriesDirectory.Size = new System.Drawing.Size(109, 17);
            this.isQueriesDirectory.TabIndex = 21;
            this.isQueriesDirectory.Text = "Queries Directory:";
            this.isQueriesDirectory.UseVisualStyleBackColor = true;
            this.isQueriesDirectory.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // isResultSetsDirectory
            // 
            this.isResultSetsDirectory.AutoSize = true;
            this.isResultSetsDirectory.Checked = true;
            this.isResultSetsDirectory.Location = new System.Drawing.Point(21, 25);
            this.isResultSetsDirectory.Name = "isResultSetsDirectory";
            this.isResultSetsDirectory.Size = new System.Drawing.Size(125, 17);
            this.isResultSetsDirectory.TabIndex = 20;
            this.isResultSetsDirectory.TabStop = true;
            this.isResultSetsDirectory.Text = "Result Sets directory:";
            this.isResultSetsDirectory.UseVisualStyleBackColor = true;
            this.isResultSetsDirectory.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // TestSuiteCreate
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(562, 334);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestSuiteCreate";
            this.Text = "Create TestSuite ...";
            this.Load += new System.EventHandler(this.ResultSetCreate_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox queriesDirectoryActual;
        private System.Windows.Forms.Button queriesDirectoryActualSelect;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox connectionStringActual;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox resultSetsDirectoryExpect;
        private System.Windows.Forms.Button resultSetsDirectoryExpectSelect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton isResultSetsDirectory;
        private System.Windows.Forms.RadioButton isQueriesDirectory;
        private System.Windows.Forms.Button queriesDirectoryExpectSelect;
        private System.Windows.Forms.TextBox queriesDirectoryExpect;
        private System.Windows.Forms.TextBox connectionStringExpect;
        private System.Windows.Forms.Label label2;
    }
}
