using System;
using System.Linq;
using System.Windows.Forms;
using NBi.Service;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class FilterWindow : Form
    {
        public FilterWindow()
        {
            InitializeComponent();
            @operator.SelectedIndex = 0;
        }

        public Operator Operator
        {
            get
            {
                if (@operator.Text == "Equal")
                    return Operator.Equal;
                if (@operator.Text == "Like")
                    return Operator.Like;
                throw new ArgumentException();
            }
        }

        public bool Negation
        {
            get
            {
                return negation.Checked;
            }
        }

        public string FilterText
        {
            get
            {
                return text.Text;
            }
        }
    }
}
