namespace NBi.UI.Genbi.View.Generator
{
    partial class TestListControl
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
            this.progressBarTest = new System.Windows.Forms.ProgressBar();
            this.testsList = new System.Windows.Forms.ListBox();
            this.bindingTests = new System.Windows.Forms.BindingSource(this.components);
            this.testsListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteTest = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTests)).BeginInit();
            this.testsListMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBarTest
            // 
            this.progressBarTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarTest.Location = new System.Drawing.Point(0, 322);
            this.progressBarTest.Name = "progressBarTest";
            this.progressBarTest.Size = new System.Drawing.Size(578, 25);
            this.progressBarTest.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTest.TabIndex = 22;
            // 
            // testsList
            // 
            this.testsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testsList.FormattingEnabled = true;
            this.testsList.Location = new System.Drawing.Point(0, 26);
            this.testsList.Name = "testsList";
            this.testsList.Size = new System.Drawing.Size(578, 290);
            this.testsList.TabIndex = 21;
            this.testsList.SelectedIndexChanged += new System.EventHandler(this.TestsList_SelectedIndexChanged);
            this.testsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TestsList_MouseDown);
            this.testsList.DoubleClick += new System.EventHandler(this.TestsList_DoubleClick);
            // 
            // testsListMenu
            // 
            this.testsListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteTest});
            this.testsListMenu.Name = "deleteTest";
            this.testsListMenu.Size = new System.Drawing.Size(130, 26);
            this.testsListMenu.Text = "Delete test";
            // 
            // deleteTest
            // 
            this.deleteTest.Name = "deleteTest";
            this.deleteTest.Size = new System.Drawing.Size(129, 22);
            this.deleteTest.Text = "Delete test";
            this.deleteTest.Click += new System.EventHandler(this.DeleteTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Tests:";
            // 
            // TestListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarTest);
            this.Controls.Add(this.testsList);
            this.Name = "TestListControl";
            this.Size = new System.Drawing.Size(581, 356);
            ((System.ComponentModel.ISupportInitialize)(this.bindingTests)).EndInit();
            this.testsListMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarTest;
        private System.Windows.Forms.ListBox testsList;
        protected internal System.Windows.Forms.BindingSource bindingTests;
        private System.Windows.Forms.ContextMenuStrip testsListMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteTest;
        private System.Windows.Forms.Label label1;


    }
}
