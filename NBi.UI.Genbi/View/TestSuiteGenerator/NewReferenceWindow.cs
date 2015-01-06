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
    public partial class NewReferenceWindow : Form
    {
        public NewReferenceWindow()
        {
            InitializeComponent();
        }

        public string ReferenceName
        {
            get
            {
                return name.Text;
            }
        }
    }
}
