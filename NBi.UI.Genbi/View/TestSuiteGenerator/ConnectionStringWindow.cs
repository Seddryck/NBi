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
    public partial class ConnectionStringWindow : Form
    {
        public ConnectionStringWindow()
        {
            InitializeComponent();
        }

        public bool IsNameEditable
        {
            get
            {
                return !name.ReadOnly;
            }
            set
            {
                name.ReadOnly = !value;
            }
        }

        public string NameId
        {
            get
            {
                return name.Text;
            }
            set
            {
                name.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return connectionString.Text;
            }
            set
            {
                connectionString.Text = value;
            }
        }
    }
}
