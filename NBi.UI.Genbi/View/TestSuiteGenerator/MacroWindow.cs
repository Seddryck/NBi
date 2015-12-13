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
    public partial class MacroWindow : Form
    {
        public MacroWindow()
        {
            InitializeComponent();
        }

        public void AppendText(string message)
        {
            actionInfoText.AppendText(message + "\r\n");
            actionInfoText.Refresh();
        }

        private void MacroWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }
    }
}
