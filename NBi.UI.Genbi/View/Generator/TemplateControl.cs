using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NBi.UI.Genbi.View.Generator
{
    public partial class TemplateControl : UserControl
    {
        internal TestSuiteViewAdapter Adapter { get; set; }

        public TemplateControl()
        {
            InitializeComponent();
        }


        public bool UseGrouping
        {
            get
            {
                return useGrouping.Checked;
            }
            set
            {
                useGrouping.Checked = value;
            }
        }

        public string Template
        {
            get
            {
                return template.Text;
            }
            set
            {
                template.Text = value;
            }
        }

        private void Template_TextChanged(object sender, EventArgs e)
        {
            Adapter.InvokeTemplateUpdate(EventArgs.Empty);
        }

    }
}
