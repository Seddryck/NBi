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
            this.panel1 = new System.Windows.Forms.Panel();
            this.remove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.variables = new System.Windows.Forms.ComboBox();
            this.rename = new System.Windows.Forms.Button();
            this.csvContent = new System.Windows.Forms.DataGridView();
            this.bindingCsv = new System.Windows.Forms.BindingSource(this.components);
            this.bindingColumnNames = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.csvContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingCsv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingColumnNames)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.remove);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.variables);
            this.panel1.Controls.Add(this.rename);
            this.panel1.Location = new System.Drawing.Point(4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 31);
            this.panel1.TabIndex = 18;
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
            this.variables.Name = "columnHeaderChoice";
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
            this.csvContent.Location = new System.Drawing.Point(4, 35);
            this.csvContent.Name = "csvContent";
            this.csvContent.Size = new System.Drawing.Size(452, 249);
            this.csvContent.TabIndex = 17;
            // 
            // VariablesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.csvContent);
            this.Name = "VariablesControl";
            this.Size = new System.Drawing.Size(457, 286);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.csvContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingCsv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingColumnNames)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox variables;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.DataGridView csvContent;
        protected internal System.Windows.Forms.BindingSource bindingCsv;
        protected internal System.Windows.Forms.BindingSource bindingColumnNames;
        public System.Windows.Forms.Button remove;

    }
}
