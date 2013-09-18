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
            CategoryName_TextChanged(this, EventArgs.Empty);
        }

        public string CategoryName
        {
            get
            {
                return categoryName.Text;
            }
        }

        public IEnumerable<char> ForbiddenChars { get; set; }

        private void CategoryName_TextChanged(object sender, EventArgs e)
        {
            apply.Enabled = ForbiddenChars!=null && CategoryName.Intersect(ForbiddenChars).Count() == 0 && CategoryName.Length > 0;
        }
    }
}
