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
    public partial class PathControl : UserControl
    {
        public PathControl()
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
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Path;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                path.Text = folderBrowserDialog.SelectedPath;
        }

        private void Path_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
