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

        public BindingList<string> SettingsNames
        {
            get
            {
                return (BindingList<string>)(bindingSettings.DataSource);
            }
            set
            {
                bindingSettings.DataSource = value;
            }
        }

        public string SettingsValue
        {
            get
            {
                return settingsValue.Text;
            }
            set
            {
                settingsValue.Text = value;
            }
        }

        private string settingsNameSelected;
        //public string SettingsNameSelected
        //{
        //    get
        //    {
        //        return settingsNameSelected;
        //    }
        //    set
        //    {
        //        settingsNameSelected = value;
        //        if (value == null)
        //            settingsName.SelectedIndex = -1;
        //        else
        //            settingsName.SelectedValue = value;
        //    }
        //}

        private Test testSelected;
        public Test TestSelected
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

        public event EventHandler<TemplatePersistEventArgs> TemplatePersist;
        public void InvokeTemplatePersist(TemplatePersistEventArgs e)
        {
            var handler = TemplatePersist;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<SettingsSelectEventArgs> SettingsSelect;
        public void InvokeSettingsSelect(SettingsSelectEventArgs e)
        {
            var handler = SettingsSelect;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<SettingsUpdateEventArgs> SettingsUpdate;
        public void InvokeSettingsUpdate(SettingsUpdateEventArgs e)
        {
            var handler = SettingsUpdate;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler TemplateUpdate;
        public void InvokeTemplateUpdate(EventArgs e)
        {
            var handler = TemplateUpdate;
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

        public event EventHandler<VariableRemoveEventArgs> VariableRemove;
        public void InvokeVariableRemove(VariableRemoveEventArgs e)
        {
            var handler = VariableRemove;
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
            openFileDialog.Filter = "All Files (*.*)|*.*|CSV (Comma delimited) (*.csv)|*.csv|Text Files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                InvokeCsvSelect(new CsvSelectEventArgs(openFileDialog.FileName));
        }


        private void OpenTemplateClick(object sender, System.EventArgs e)
        {
            if (!OpenTemplateView.Visible)
                OpenTemplateView.Show(this);
        }

        private void SaveTemplateClick(object sender, System.EventArgs e)
        {
            var saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Template Files (*.nbitt)|*.nbitt|Text Files (*.txt)|*.txt";
            saveAsDialog.FilterIndex = 2;
            DialogResult result = saveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
                InvokeTemplatePersist(new TemplatePersistEventArgs(saveAsDialog.FileName));
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
            saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Suite Files (*.nbits)|*.nbits|Xml Files (*.xml)|*.xml|Text Files (*.txt)|*.txt";
            saveAsDialog.FilterIndex = 2;
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
                    testsListMenu.Show();   
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

        private void Template_TextChanged(object sender, EventArgs e)
        {
            InvokeTemplateUpdate(EventArgs.Empty);
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
                InvokeVariableRemove(new VariableRemoveEventArgs(columnHeaderChoice.SelectedIndex));
        }

        private void SettingsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingsValue.TextChanged -= SettingsValue_TextChanged;
            InvokeSettingsSelect(new SettingsSelectEventArgs((string)settingsName.SelectedValue));
            settingsValue.TextChanged += SettingsValue_TextChanged;
        }

        private void SettingsValue_TextChanged(object sender, EventArgs e)
        {
            InvokeSettingsUpdate(new SettingsUpdateEventArgs((string)settingsName.SelectedValue, settingsValue.Text));
        }


    }
}
