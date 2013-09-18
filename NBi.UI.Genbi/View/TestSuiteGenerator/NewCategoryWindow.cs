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
            CategoryName_Changed(this, EventArgs.Empty);
            this.categoryName.SelectedIndexChanged += CategoryName_Changed;
            this.categoryName.TextChanged += CategoryName_Changed;
        }

        public string CategoryName
        {
            get
            {
                return categoryName.Text;
            }
        }

        public IEnumerable<char> ForbiddenChars { private get; set; }
        public IEnumerable<string> ExistingCategories 
        { 
            set
            {
                categoryName.Items.Clear();
                categoryName.Items.AddRange(value.ToArray());
                categoryName.SelectedIndex = -1;
            }
         }

        private void CategoryName_Changed(object sender, EventArgs e)
        {
            apply.Enabled = ForbiddenChars!=null && CategoryName.Intersect(ForbiddenChars).Count() == 0 && CategoryName.Length > 0;
        }

        
    }
}
