using System;
using System.Linq;
using System.Windows.Forms;
using NBi.GenbiL.Action;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class FilterWindow : Form
    {
        public FilterWindow()
        {
            InitializeComponent();
            if (@operator.Items.Count>0)
                @operator.SelectedIndex = 0;
        }

        public OperatorType Operator
        {
            get
            {
                if (@operator.Text == "Equal")
                    return OperatorType.Equal;
                if (@operator.Text == "Like")
                    return OperatorType.Like;
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
