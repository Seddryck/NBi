namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class TestCasesControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.filter = new System.Windows.Forms.Button();
            this.moveRight = new System.Windows.Forms.Button();
            this.moveLeft = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.variables = new System.Windows.Forms.ComboBox();
            this.rename = new System.Windows.Forms.Button();
            this.csvContent = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.runQuery = new System.Windows.Forms.Button();
            this.editConnectionString = new System.Windows.Forms.Button();
            this.removeConnectionString = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionStringNames = new System.Windows.Forms.ComboBox();
            this.addConnectionString = new System.Windows.Forms.Button();
            this.bindingCsv = new System.Windows.Forms.BindingSource(this.components);
            this.bindingColumnNames = new System.Windows.Forms.BindingSource(this.components);
            this.sqlEditor = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.csvContent)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingCsv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingColumnNames)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(457, 286);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.csvContent);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(449, 260);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Test-cases";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.filter);
            this.panel1.Controls.Add(this.moveRight);
            this.panel1.Controls.Add(this.moveLeft);
            this.panel1.Controls.Add(this.remove);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.variables);
            this.panel1.Controls.Add(this.rename);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 31);
            this.panel1.TabIndex = 20;
            // 
            // filter
            // 
            this.filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filter.Image = global::NBi.UI.Genbi.Properties.Resources.funnel;
            this.filter.Location = new System.Drawing.Point(401, 3);
            this.filter.Name = "filter";
            this.filter.Size = new System.Drawing.Size(24, 24);
            this.filter.TabIndex = 21;
            this.filter.UseVisualStyleBackColor = true;
            // 
            // moveRight
            // 
            this.moveRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveRight.Image = global::NBi.UI.Genbi.Properties.Resources.text_padding_right;
            this.moveRight.Location = new System.Drawing.Point(371, 3);
            this.moveRight.Name = "moveRight";
            this.moveRight.Size = new System.Drawing.Size(24, 24);
            this.moveRight.TabIndex = 20;
            this.moveRight.UseVisualStyleBackColor = true;
            // 
            // moveLeft
            // 
            this.moveLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveLeft.Image = global::NBi.UI.Genbi.Properties.Resources.text_padding_left;
            this.moveLeft.Location = new System.Drawing.Point(341, 3);
            this.moveLeft.Name = "moveLeft";
            this.moveLeft.Size = new System.Drawing.Size(24, 24);
            this.moveLeft.TabIndex = 19;
            this.moveLeft.UseVisualStyleBackColor = true;
            // 
            // remove
            // 
            this.remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remove.Image = global::NBi.UI.Genbi.Properties.Resources.textfield_delete;
            this.remove.Location = new System.Drawing.Point(311, 2);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(24, 24);
            this.remove.TabIndex = 18;
            this.remove.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Column\'s name:";
            // 
            // variables
            // 
            this.variables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.variables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variables.FormattingEnabled = true;
            this.variables.Location = new System.Drawing.Point(87, 3);
            this.variables.Name = "variables";
            this.variables.Size = new System.Drawing.Size(189, 21);
            this.variables.TabIndex = 16;
            // 
            // rename
            // 
            this.rename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rename.Image = global::NBi.UI.Genbi.Properties.Resources.textfield_rename;
            this.rename.Location = new System.Drawing.Point(282, 2);
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(24, 24);
            this.rename.TabIndex = 14;
            this.rename.UseVisualStyleBackColor = true;
            // 
            // csvContent
            // 
            this.csvContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.csvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.csvContent.Location = new System.Drawing.Point(0, 33);
            this.csvContent.Name = "csvContent";
            this.csvContent.Size = new System.Drawing.Size(452, 249);
            this.csvContent.TabIndex = 19;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.sqlEditor);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(449, 260);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Query";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.runQuery);
            this.panel2.Controls.Add(this.editConnectionString);
            this.panel2.Controls.Add(this.removeConnectionString);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.connectionStringNames);
            this.panel2.Controls.Add(this.addConnectionString);
            this.panel2.Location = new System.Drawing.Point(-2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 29);
            this.panel2.TabIndex = 21;
            // 
            // runQuery
            // 
            this.runQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runQuery.Image = global::NBi.UI.Genbi.Properties.Resources.control;
            this.runQuery.Location = new System.Drawing.Point(421, 3);
            this.runQuery.Name = "runQuery";
            this.runQuery.Size = new System.Drawing.Size(24, 24);
            this.runQuery.TabIndex = 20;
            this.runQuery.UseVisualStyleBackColor = true;
            // 
            // editConnectionString
            // 
            this.editConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editConnectionString.Image = global::NBi.UI.Genbi.Properties.Resources.database__pencil;
            this.editConnectionString.Location = new System.Drawing.Point(341, 2);
            this.editConnectionString.Name = "editConnectionString";
            this.editConnectionString.Size = new System.Drawing.Size(24, 24);
            this.editConnectionString.TabIndex = 19;
            this.editConnectionString.UseVisualStyleBackColor = true;
            // 
            // removeConnectionString
            // 
            this.removeConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeConnectionString.Image = global::NBi.UI.Genbi.Properties.Resources.database__minus;
            this.removeConnectionString.Location = new System.Drawing.Point(311, 2);
            this.removeConnectionString.Name = "removeConnectionString";
            this.removeConnectionString.Size = new System.Drawing.Size(24, 24);
            this.removeConnectionString.TabIndex = 18;
            this.removeConnectionString.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Connection";
            // 
            // connectionStringNames
            // 
            this.connectionStringNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionStringNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionStringNames.FormattingEnabled = true;
            this.connectionStringNames.Location = new System.Drawing.Point(68, 3);
            this.connectionStringNames.Name = "connectionStringNames";
            this.connectionStringNames.Size = new System.Drawing.Size(208, 21);
            this.connectionStringNames.TabIndex = 16;
            // 
            // addConnectionString
            // 
            this.addConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addConnectionString.Image = global::NBi.UI.Genbi.Properties.Resources.database__plus;
            this.addConnectionString.Location = new System.Drawing.Point(282, 2);
            this.addConnectionString.Name = "addConnectionString";
            this.addConnectionString.Size = new System.Drawing.Size(24, 24);
            this.addConnectionString.TabIndex = 14;
            this.addConnectionString.UseVisualStyleBackColor = true;
            // 
            // sqlEditor
            // 
            this.sqlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sqlEditor.Location = new System.Drawing.Point(0, 39);
            this.sqlEditor.Multiline = true;
            this.sqlEditor.Name = "sqlEditor";
            this.sqlEditor.Size = new System.Drawing.Size(450, 215);
            this.sqlEditor.TabIndex = 22;
            // 
            // TestCasesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "TestCasesControl";
            this.Size = new System.Drawing.Size(457, 286);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.csvContent)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingCsv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingColumnNames)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        protected internal System.Windows.Forms.BindingSource bindingCsv;
        protected internal System.Windows.Forms.BindingSource bindingColumnNames;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button filter;
        public System.Windows.Forms.Button moveRight;
        public System.Windows.Forms.Button moveLeft;
        public System.Windows.Forms.Button remove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox variables;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.DataGridView csvContent;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox connectionStringNames;
        private System.Windows.Forms.Button addConnectionString;
        public System.Windows.Forms.Button removeConnectionString;
        public System.Windows.Forms.Button editConnectionString;
        public System.Windows.Forms.Button runQuery;
        private System.Windows.Forms.TextBox sqlEditor;

    }
}
