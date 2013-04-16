using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.Generator;
using NBi.UI.Genbi.Interface.Generator.Events;

namespace NBi.UI.Genbi.View.Generator
{
    public partial class RenameVariableView : Form
    {
        public int Index { get; set; }
        protected CsvGeneratorView Origin { get; set; }

        public RenameVariableView(CsvGeneratorView origin)
        {
            this.Origin = origin;
            InitializeComponent();
            InvokeInitialize(new EventArgs());
        }

        public event EventHandler<VariableRenameEventArgs> Apply;
        public void InvokeApply(VariableRenameEventArgs e)
        {
            EventHandler<VariableRenameEventArgs> handler = Apply;
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
            Origin.InvokeVariableRename(new VariableRenameEventArgs(Index, Variable));
            this.Hide();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
