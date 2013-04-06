using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.Generator;
using NBi.UI.Genbi.Interface.Generator.Events;
using NBi.UI.Genbi.Presenter.Generator;
using NBi.Xml;

namespace NBi.UI.Genbi.View.Generator
{
    public partial class CsvGeneratorView : Form, ICsvGeneratorView
    {
        private CsvGeneratorPresenter Presenter { get; set; }

        protected RenameVariableView RenameVariableView { get; set; }
        protected OpenTemplateView OpenTemplateView { get; set; }
        protected DisplayTestView DisplayTestView { get; set; }

        public CsvGeneratorView()
        {
            Presenter = new CsvGeneratorPresenter(this);
            InitializeComponent();
            InitializeSubViews();
            DeclareBindings();
            InvokeInitialize(new EventArgs());
        }

        protected void InitializeSubViews()
        {
            RenameVariableView = new RenameVariableView(this);
            OpenTemplateView = new OpenTemplateView(this);
            DisplayTestView = new DisplayTestView(this);
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

        private TestXml testSelected;
        public TestXml TestSelected
        {
            get
            {
                return testSelected;
            }
            set
            {
                testSelected = value;
                if (value == null)
                    DisplayTestView.TestContent = string.Empty;
                else
                    DisplayTestView.TestContent = value.Content;
            }
        }

        #endregion

        protected void DeclareBindings()
        {
            csvContent.DataSource = bindingCsv;
            columnHeaderChoice.DataSource = bindingColumnNames;
            testsList.DataSource = bindingTests;
        }

        public event EventHandler<CsvSelectEventArgs> CsvSelect;
        public void InvokeCsvSelect(CsvSelectEventArgs e)
        {
            var handler = CsvSelect;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TemplateSelectEventArgs> TemplateSelect;
        public void InvokeTemplateSelect(TemplateSelectEventArgs e)
        {
            var handler = TemplateSelect;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler TestsGenerate;
        public void InvokeTestsGenerate(EventArgs e)
        {
            var handler = TestsGenerate;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler TestsUndoGenerate;
        public void InvokeTestsUndoGenerate(EventArgs e)
        {
            var handler = TestsUndoGenerate;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TestSuitePersistEventArgs> TestSuitePersist;
        public void InvokeTestSuitePersist(TestSuitePersistEventArgs e)
        {
            var handler = TestSuitePersist;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<VariableRenameEventArgs> VariableRename;
        public void InvokeVariableRename(VariableRenameEventArgs e)
        {
            var handler = VariableRename;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TestSelectEventArgs> TestSelect;
        public void InvokeTestSelect(TestSelectEventArgs e)
        {
            var handler = TestSelect;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler TestDelete;
        public void InvokeTestDelete(EventArgs e)
        {
            var handler = TestDelete;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler TestsClear;
        public void InvokeTestsClear(EventArgs e)
        {
            var handler = TestsClear;
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
            InvokeTestsGenerate(e);
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            InvokeTestsUndoGenerate(e);
        }

        private void OpenCsv_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                InvokeCsvSelect(new CsvSelectEventArgs(openFileDialog.FileName));
        }


        private void OpenTemplateClick(object sender, System.EventArgs e)
        {
            if (!OpenTemplateView.Visible)
                OpenTemplateView.Show(this);
        }


        private void Rename_Click(object sender, EventArgs e)
        {
            RenameVariableView.Index = columnHeaderChoice.SelectedIndex;
            RenameVariableView.Variable = columnHeaderChoice.SelectedItem.ToString();
            if (!RenameVariableView.Visible)
                RenameVariableView.Show(this);
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
                InvokeTestSuitePersist(new TestSuitePersistEventArgs(saveAsDialog.FileName));
        }

        private void TestsList_DoubleClick(object sender, EventArgs e)
        {
            if (!DisplayTestView.Visible)
                DisplayTestView.Show();
        }

        private void TestsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvokeTestSelect(new TestSelectEventArgs(testsList.SelectedIndex));
        }

        private void TestsList_MouseDown(object sender, MouseEventArgs e)
        {
            if ( e.Button == MouseButtons.Right )
            {
                //select the item under the mouse pointer
                testsList.SelectedIndex = testsList.IndexFromPoint(e.Location);
                if (testsList.SelectedIndex != -1)
                {
                    deleteTest.Show();   
                }                
            }
        }

        private void DeleteTest_Click(object sender, EventArgs e)
        {
            InvokeTestDelete(EventArgs.Empty);
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
                InvokeTestsClear(EventArgs.Empty);
        }
    }
}
