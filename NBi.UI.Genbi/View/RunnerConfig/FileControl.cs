using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NBi.UI.Genbi.View.RunnerConfig
{
    public partial class FileControl : UserControl
    {
        public FileControl()
        {
            InitializeComponent();
        }

        public string Label 
        {
            get
            {
                return label.Text;
            }
            set
            {
                label.Text = value;
            }
        }

        public string Path
        {
            get
            {
                return path.Text;
            }
            set
            {
                path.Text = value;
            }
        }

        private void OpenDialog_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|NBi Test Suite Files (*.nbits)|*.nbits|Xml Files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 2;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                path.Text = openFileDialog.FileName;
        }

        private void Path_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
