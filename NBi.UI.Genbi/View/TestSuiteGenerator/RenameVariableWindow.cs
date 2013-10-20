using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class RenameVariableWindow : Form
    {
        internal RenameVariableWindow()
        {           
            InitializeComponent();
        }


        public string NewName
        {
            get { return variable.Text;  }
            set { variable.Text = value; }
        }


        


    }
}
