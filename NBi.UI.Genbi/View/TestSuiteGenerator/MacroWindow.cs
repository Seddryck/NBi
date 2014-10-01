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
            actionInfoText.Text = actionInfoText.Text + message + "\r\n";
        }
    }
}
