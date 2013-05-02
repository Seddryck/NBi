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
            testListControl.Adapter = Adapter;
            testListControl.DeclareBindings();

            variablesControl.Adapter = Adapter;
            variablesControl.DeclareBindings();

            settingsControl.Adapter = Adapter;
            settingsControl.DeclareBindings();

            templateControl.Adapter = Adapter;
        }

        #region properties

        public DataTable CsvContent
        {
            get
            {
                return (DataTable)(variablesControl.bindingCsv.DataSource);
            }
            set
            {
                variablesControl.bindingCsv.DataSource = value;
            }
        }

        public bool UseGrouping
        {
            get
            {
                return templateControl.UseGrouping;
            }
            set
            {
                templateControl.UseGrouping = value;
            }
        }

        public BindingList<string> Variables
        {
            get
            {
                return variablesControl.Variables;
            }
            set
            {
                variablesControl.Variables = value;
            }
        }

        public BindingList<Test> Tests
        {
            get
            {
                return testListControl.Tests;
            }
            set
            {
                testListControl.Tests = value;
            }
        }

        public string Template
        {
            get
            {
                return templateControl.Template;
            }
            set
            {
                templateControl.Template = value;
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
                return testListControl.TestSelectedIndex;
            }
            set
            {
                testListControl.TestSelectedIndex = value;
            }
        }

        public bool CanGenerate
        {
            set
            {
                generateToolStripMenuItem.Enabled = value;
                toolStripTestSuiteGenerate.Enabled = value;
            }
        }

        public bool CanUndo
        {
            set
            {
                undoToolStripMenuItem.Enabled = value;
            }
        }

        public bool CanClear
        {
            set
            {
                clearToolStripMenuItem.Enabled = value;
                toolStripTestSuiteSaveAs.Enabled = value;
            }
        }

        public bool CanSaveAs
        {
            set
            {
                saveAsToolStripMenuItem.Enabled = value;
            }
        }

        public bool CanSaveTemplate
        {
            set
            {
                saveTemplateAsToolStripMenuItem.Enabled = value;
            }
        }

        public bool CanRename
        {
            set
            {
                variablesControl.CanRename = value;
            }
        }

        public bool CanRemove
        {
            set
            {
                variablesControl.CanRemove = value;
            }
        }


        public int ProgressValue
        {
            set
            {
                testListControl.ProgressValue = value;
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


        private void OpenTemplate_Click(object sender, EventArgs e)
        {
            if (!Adapter.OpenTemplateForm.Visible)
                Adapter.OpenTemplateForm.Show(this);
        }

        private void SaveTemplate_Click(object sender, EventArgs e)
        {
            var saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Template Files (*.nbitt)|*.nbitt|Text Files (*.txt)|*.txt";
            saveAsDialog.FilterIndex = 2;
            DialogResult result = saveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
                Adapter.InvokeTemplatePersist(new TemplatePersistEventArgs(saveAsDialog.FileName));
        }

        private void SaveTestSuiteAs_Click(object sender, EventArgs e)
        {
            var saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Suite Files (*.nbits)|*.nbits|Xml Files (*.xml)|*.xml|Text Files (*.txt)|*.txt";
            saveAsDialog.FilterIndex = 2;
            DialogResult result = saveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
                Adapter.InvokeTestSuitePersist(new TestSuitePersistEventArgs(saveAsDialog.FileName));
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
  
    }
}
