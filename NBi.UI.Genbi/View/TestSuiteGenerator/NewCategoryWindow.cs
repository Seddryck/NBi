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
    public partial class NewCategoryWindow : Form
    {
        public NewCategoryWindow()
        {
            InitializeComponent();
        }

        public string CategoryName
        {
            get
            {
                return categoryName.Text;
            }
        }
    }
}
