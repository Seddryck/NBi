using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class TestCasesControl : UserControl
    {
        public RenameVariableWindow Window { get; set;}
        private BindingSource testCasesBindingSource;
        private BindingSource variablesBindingSource;

        public TestCasesControl()
        {
            InitializeComponent();
        }

        internal void DataBind(TestCasesPresenter presenter)
        {
            if (presenter != null)
            {
                testCasesBindingSource = new BindingSource();
                testCasesBindingSource.DataSource = presenter.TestCases;
                csvContent.DataSource = testCasesBindingSource;

                variablesBindingSource = new BindingSource();
                variablesBindingSource.DataSource = presenter.Variables;
                variables.DataSource = variablesBindingSource;

                variables.DataBindings.Add("SelectedIndex", presenter, "VariableSelectedIndex", true, DataSourceUpdateMode.OnValidation);
                variables.SelectedIndexChanged += (s, args) => variables.DataBindings["SelectedIndex"].WriteValue();
            }
        }

        public Button RemoveCommand
        {
            get
            {
                return remove;
            }
        }

        public Button RenameCommand
        {
            get
            {
                return rename;
            }
        }

        public Button MoveLeftCommand
        {
            get
            {
                return moveLeft;
            }
        }

        public Button MoveRightCommand
        {
            get
            {
                return moveRight;
            }
        }

    }
}
