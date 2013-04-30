using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.Service.Dto;
using NBi.UI.Genbi.Interface.Generator;
using NBi.UI.Genbi.Interface.Generator.Events;
using NBi.UI.Genbi.Presenter.Generator;

namespace NBi.UI.Genbi.View.Generator
{
    public partial class GenbiView : Form
    {
        protected TestSuiteViewAdapter Adapter { get; set; }


        public GenbiView(TestSuiteViewAdapter adapter)
        {
            Adapter = adapter;
            InitializeComponent();
            DeclareBindings();
        }

        protected void DeclareBindings()
        {
            csvContent.DataSource = bindingCsv;
            columnHeaderChoice.DataSource = bindingColumnNames;
            testsList.DataSource = bindingTests;
            settingsControl.Adapter = Adapter;
            settingsControl.DeclareBindings();
        }

        #region properties

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

        public bool UseGrouping
        {
            get
            {
                return useGrouping.Checked;
            }
            set
            {
                useGrouping.Checked = value;
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

        public BindingList<Test> Tests
        {
            get
            {
                return (BindingList<Test>)(bindingTests.DataSource);
            }
            set
            {
                bindingTests.DataSource = value;
                testsList.DisplayMember = "Title";
            }
        }

        

        public string Template
        {
            get
            {
                return template.Text;
            }
            set
            {
                template.Text = value;
            }
        }


        public string SettingsValue
        {
            get
            {
                return settingsControl.Value;
            }
            set
            {
                settingsControl.Value = value;
            }
        }


        public BindingList<string> SettingsNames
        {
            get
            {
                return settingsControl.Names;
            }
            set
            {
                settingsControl.Names = value;
            }
        }

        public int TestSelectedIndex
        {
            get
            {
                return testsList.SelectedIndex;
            }
            set
            {
                testsList.SelectedIndex = value;
            }
        }

        public bool CanGenerate
        {
            set
            {
                generate.Enabled = value;
            }
        }

        public bool CanUndo
        {
            set
            {
                undo.Enabled = value;
            }
        }

        public bool CanClear
        {
            set
            {
                clear.Enabled = value;
            }
        }

        public bool CanSaveAs
        {
            set
            {
                saveAs.Enabled = value;
            }
        }

        public bool CanSaveTemplate
        {
            set
            {
                saveTemplate.Enabled = value;
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

        public int ProgressValue
        {
            set
            {
                if (progressBarTest.Value != value)
                {
                    progressBarTest.Value = value;
                    progressBarTest.Refresh();
                }
            }
        }

        #endregion


        private void CsvImporterView_Load(object sender, EventArgs e)
        {

        }

        private void Generate_Click(object sender, EventArgs e)
        {
            Adapter.InvokeTestsGenerate(e);
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            Adapter.InvokeTestsUndoGenerate(e);
        }

        private void OpenCsv_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|CSV (Comma delimited) (*.csv)|*.csv|Text Files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                Adapter.InvokeCsvSelect(new CsvSelectEventArgs(openFileDialog.FileName));
        }


        private void OpenTemplateClick(object sender, System.EventArgs e)
        {
            if (!Adapter.OpenTemplateForm.Visible)
                Adapter.OpenTemplateForm.Show(this);
        }

        private void SaveTemplateClick(object sender, System.EventArgs e)
        {
            var saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Template Files (*.nbitt)|*.nbitt|Text Files (*.txt)|*.txt";
            saveAsDialog.FilterIndex = 2;
            DialogResult result = saveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
                Adapter.InvokeTemplatePersist(new TemplatePersistEventArgs(saveAsDialog.FileName));
        }


        private void Rename_Click(object sender, EventArgs e)
        {
            Adapter.VariableSelectedIndex       = columnHeaderChoice.SelectedIndex;
            Adapter.RenameVariableForm.Variable = Adapter.Variables[Adapter.VariableSelectedIndex];
            if (!Adapter.RenameVariableForm.Visible)
                Adapter.RenameVariableForm.Show(this);
        }

        private void SaveAsClick(object sender, EventArgs e)
        {
            var saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Suite Files (*.nbits)|*.nbits|Xml Files (*.xml)|*.xml|Text Files (*.txt)|*.txt";
            saveAsDialog.FilterIndex = 2;
            DialogResult result = saveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
                Adapter.InvokeTestSuitePersist(new TestSuitePersistEventArgs(saveAsDialog.FileName));
        }

        private void TestsList_DoubleClick(object sender, EventArgs e)
        {
            if (!Adapter.DisplayTestForm.Visible)
                Adapter.DisplayTestForm.Show();
        }

        private void TestsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Adapter.InvokeTestSelect(new TestSelectEventArgs(testsList.SelectedIndex));
        }

        private void TestsList_MouseDown(object sender, MouseEventArgs e)
        {
            if ( e.Button == MouseButtons.Right )
            {
                //select the item under the mouse pointer
                testsList.SelectedIndex = testsList.IndexFromPoint(e.Location);
                if (testsList.SelectedIndex != -1)
                {
                    testsListMenu.Show();   
                }                
            }
        }

        private void DeleteTest_Click(object sender, EventArgs e)
        {
            Adapter.InvokeTestDelete(EventArgs.Empty);
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            var diagRes = MessageBox.Show(
                "Are your sure you want to clear the test-suite?",
                "Genbi",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (diagRes.HasFlag(DialogResult.OK))
                Adapter.InvokeTestsClear(EventArgs.Empty);
        }

        private void Template_TextChanged(object sender, EventArgs e)
        {
            Adapter.InvokeTemplateUpdate(EventArgs.Empty);
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
    }
}
