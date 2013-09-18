namespace NBi.UI.Genbi.View.TestSuiteGenerator
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
            this.testsListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.bindingTests = new System.Windows.Forms.BindingSource(this.components);
            this.useGrouping = new System.Windows.Forms.CheckBox();
            this.addCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsListMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTests)).BeginInit();
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
            this.testsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TestsList_MouseDown);
            // 
            // testsListMenu
            // 
            this.testsListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteTestToolStripMenuItem,
            this.editTestToolStripMenuItem,
            this.addCategoryToolStripMenuItem});
            this.testsListMenu.Name = "deleteTest";
            this.testsListMenu.Size = new System.Drawing.Size(153, 92);
            this.testsListMenu.Text = "Delete test";
            // 
            // deleteTestToolStripMenuItem
            // 
            this.deleteTestToolStripMenuItem.Image = global::NBi.UI.Genbi.Properties.Resources.note_delete;
            this.deleteTestToolStripMenuItem.Name = "deleteTestToolStripMenuItem";
            this.deleteTestToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteTestToolStripMenuItem.Text = "Delete test";
            // 
            // editTestToolStripMenuItem
            // 
            this.editTestToolStripMenuItem.Image = global::NBi.UI.Genbi.Properties.Resources.note_edit;
            this.editTestToolStripMenuItem.Name = "editTestToolStripMenuItem";
            this.editTestToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editTestToolStripMenuItem.Text = "Edit test";
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
            // useGrouping
            // 
            this.useGrouping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.useGrouping.AutoSize = true;
            this.useGrouping.Location = new System.Drawing.Point(489, 4);
            this.useGrouping.Name = "useGrouping";
            this.useGrouping.Size = new System.Drawing.Size(89, 17);
            this.useGrouping.TabIndex = 24;
            this.useGrouping.Text = "Use grouping";
            this.useGrouping.UseVisualStyleBackColor = true;
            // 
            // addCategoryToolStripMenuItem
            // 
            this.addCategoryToolStripMenuItem.Name = "addCategoryToolStripMenuItem";
            this.addCategoryToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.addCategoryToolStripMenuItem.Text = "Add category";
            // 
            // TestListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useGrouping);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarTest);
            this.Controls.Add(this.testsList);
            this.Name = "TestListControl";
            this.Size = new System.Drawing.Size(581, 356);
            this.testsListMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingTests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarTest;
        private System.Windows.Forms.ListBox testsList;
        protected internal System.Windows.Forms.BindingSource bindingTests;
        private System.Windows.Forms.ContextMenuStrip testsListMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteTestToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem editTestToolStripMenuItem;
        protected internal System.Windows.Forms.CheckBox useGrouping;
        private System.Windows.Forms.ToolStripMenuItem addCategoryToolStripMenuItem;


    }
}
