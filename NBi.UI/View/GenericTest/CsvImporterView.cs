using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Interface;
using NBi.UI.Presenter.GenericTest;
using NBi.Xml;

namespace NBi.UI.View.GenericTest
{
    public partial class CsvImporterView : Form, ICsvImporterView
    {
        private CsvImportPresenter Presenter { get; set; }

        protected OpenTemplateView OpenTemplateView { get; set; }

        public CsvImporterView()
        {
            Presenter = new CsvImportPresenter(this);
            InitializeComponent();
            InitializeSubViews();
            DeclareBindings();
            InvokeInitialize(new EventArgs());
        }

        protected void InitializeSubViews()
        {
            OpenTemplateView = new OpenTemplateView(this);
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

        public BindingList<TestXml> Tests
        {
            get
            {
                return (BindingList<TestXml>)(bindingTests.DataSource);
            }
            set
            {
                bindingTests.DataSource = value;
            }
        }

        public BindingList<string> EmbeddedTemplates
        {
            get
            {
                return (BindingList<string>)(OpenTemplateView.BindingEmbeddedTemplates.DataSource);
            }
            set
            {
                OpenTemplateView.BindingEmbeddedTemplates.DataSource = value;
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

        #endregion

        protected void DeclareBindings()
        {
            csvContent.DataSource = bindingCsv;
            columnHeaderChoice.DataSource = bindingColumnNames;
            testsList.DataSource = bindingTests;
        }

        public event EventHandler<NewCsvSelectedEventArgs> NewCsvSelected;
        public void InvokeNewCsvSelected(NewCsvSelectedEventArgs e)
        {
            var handler = NewCsvSelected;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<NewTemplateSelectedEventArgs> NewTemplateSelected;
        public void InvokeNewTemplateSelected(NewTemplateSelectedEventArgs e)
        {
            var handler = NewTemplateSelected;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler GenerateTests;
        public void InvokeGenerateTests(EventArgs e)
        {
            var handler = GenerateTests;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<PersistTestSuiteEventArgs> PersistTestSuite;
        public void InvokePersistTestSuite(PersistTestSuiteEventArgs e)
        {
            var handler = PersistTestSuite;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<VariableRenamedEventArgs> VariableRenamed;
        public void InvokeVariableRenamed(VariableRenamedEventArgs e)
        {
            var handler = VariableRenamed;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler Initialize;
        public void InvokeInitialize(EventArgs e)
        {
            EventHandler handler = Initialize;
            if (handler != null)
                handler(this, e);
        }


        private void CsvImporterView_Load(object sender, EventArgs e)
        {

        }

        private void Generate_Click(object sender, EventArgs e)
        {
            InvokeGenerateTests(e);
        }

        private void OpenCsv_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                InvokeNewCsvSelected(new NewCsvSelectedEventArgs(openFileDialog.FileName));
        }


        private void OpenTemplateClick(object sender, System.EventArgs e)
        {
            OpenTemplateView.Show(this);
        }


        private void Rename_Click(object sender, EventArgs e)
        {
            var view = new RenameVariableView(this, columnHeaderChoice.SelectedIndex);
            view.Variable = columnHeaderChoice.SelectedItem.ToString();
            view.ShowDialog();
        }

        public void ShowException(string text)
        {
            MessageBox.Show(text, "Generic test builder", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public void ShowInform(string text)
        {
            MessageBox.Show(text, "Generic test builder", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void SaveAsClick(object sender, EventArgs e)
        {
            var saveAsDialog = new SaveFileDialog();
            DialogResult result = saveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
                InvokePersistTestSuite(new PersistTestSuiteEventArgs(saveAsDialog.FileName));
        }
    }
}
