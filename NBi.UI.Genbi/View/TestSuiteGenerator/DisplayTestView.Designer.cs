using System.Windows.Forms;
using NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class DisplayTestView
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
            this.close = new System.Windows.Forms.Button();
            this.xmlTextEditor = new NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor.XmlTextEditor();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(501, 268);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "&Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.Close_Click);
            // 
            // xmlTextEditor
            // 
            this.xmlTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlTextEditor.IsIconBarVisible = true;
            this.xmlTextEditor.IsReadOnly = false;
            this.xmlTextEditor.Location = new System.Drawing.Point(12, 12);
            this.xmlTextEditor.Name = "xmlTextEditor";
            this.xmlTextEditor.Presenter = null;
            this.xmlTextEditor.ShowVRuler = false;
            this.xmlTextEditor.Size = new System.Drawing.Size(564, 236);
            this.xmlTextEditor.TabIndent = 2;
            this.xmlTextEditor.TabIndex = 0;
            // 
            // DisplayTestView
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(588, 298);
            this.Controls.Add(this.close);
            this.Controls.Add(this.xmlTextEditor);
            this.Name = "DisplayTestView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Display and Edit Test Case";
            this.Load += new System.EventHandler(this.DisplayTestView_Load);
            this.FormClosing += new FormClosingEventHandler(this.Form_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private XmlTextEditor xmlTextEditor;
    }
}