using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class OpenQueryWindow : Form
    {
        internal TestSuiteView Origin { get; set; }
        public string QueryFile
        {
            get
            {
                return queryFile.Text;
            }
        }
        public string ConnectionString
        {
            get
            {
                return connectionString.Text;
            }
        }


        public OpenQueryWindow()
        {
            InitializeComponent();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
        }

        private void QueryFileSelection_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|SQL (*.sql)|*.sql|MDX (*.mdx)|*.mdx";
            openFileDialog.FilterIndex = 2;
            DialogResult result = openFileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
                queryFile.Text = openFileDialog.FileName;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
