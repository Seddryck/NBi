using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.TestSuiteGenerator.Events;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class VariablesControl : UserControl
    {
        internal TestSuiteViewAdapter Adapter { get; set; }

        public VariablesControl()
        {
            InitializeComponent();
        }

        public DataTable CsvContent
        {
            get
            {
                return (DataTable)(bindingCsv.DataSource);
            }
            set
            {
                bindingCsv.DataSource = value;
                csvContent.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        public BindingList<string> Variables
        {
            get
            {
                return (BindingList<string>)(bindingColumnNames.DataSource);
            }
            set
            {
                bindingColumnNames.DataSource = value;
            }

        }

        public bool CanRename
        {
            set
            {
                rename.Enabled = value;
            }
        }

        public bool CanRemove
        {
            set
            {
                remove.Enabled = value;
            }
        }

        
        private void Rename_Click(object sender, EventArgs e)
        {
            Adapter.VariableSelectedIndex = columnHeaderChoice.SelectedIndex;
            Adapter.RenameVariableForm.Variable = Adapter.Variables[Adapter.VariableSelectedIndex];
            if (!Adapter.RenameVariableForm.Visible)
                Adapter.RenameVariableForm.Show(this);
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var diagRes = MessageBox.Show(
                "Are your sure you want to remove this variable/column?",
                "Genbi",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (diagRes.HasFlag(DialogResult.OK))
                Adapter.InvokeVariableRemove(new VariableRemoveEventArgs(columnHeaderChoice.SelectedIndex));
        }

        internal void DeclareBindings()
        {
            columnHeaderChoice.DataSource = bindingColumnNames;
            csvContent.DataSource = bindingCsv;
        }


    }
}
