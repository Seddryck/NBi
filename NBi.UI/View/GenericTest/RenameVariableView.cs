using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Interface;

namespace NBi.UI.View.GenericTest
{
    public partial class RenameVariableView : Form
    {
        private readonly int index;
        private readonly CsvImporterView parentView;

        public RenameVariableView(CsvImporterView parentView, int index)
        {
            this.index = index;
            this.parentView = parentView;
            InitializeComponent();

            InvokeInitialize(new EventArgs());
        }

        public event EventHandler<VariableRenamedEventArgs> Apply;
        public void InvokeApply(VariableRenamedEventArgs e)
        {
            EventHandler<VariableRenamedEventArgs> handler = Apply;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler Cancel;
        public void InvokeCancel(EventArgs e)
        {
            EventHandler handler = Cancel;
            if (handler != null)
                handler(this, e);
        }

        public string Variable
        {
            get { return variable.Text;  }
            set { variable.Text = value; }
        }

        public event EventHandler Initialize;
        public void InvokeInitialize(EventArgs e)
        {
            EventHandler handler = Initialize;
            if (handler != null)
                handler(this, e);
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Variable))
            {
                MessageBox.Show(this, "Variable's name cannot be empty", "Rename variable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            parentView.InvokeVariableRenamed(new VariableRenamedEventArgs(index, Variable));
            Close();
        }

    }
}
