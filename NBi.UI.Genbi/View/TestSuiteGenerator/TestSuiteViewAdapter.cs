using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.Service.Dto;
using NBi.UI.Genbi.Interface.TestSuiteGenerator;
using NBi.UI.Genbi.Interface.TestSuiteGenerator.Events;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public class TestSuiteViewAdapter: ITestSuiteGeneratorView, IAdapter
    {
        public TestSuiteView GlobalForm { get; set; }
        public DisplayTestView DisplayTestForm { get; set; }
        public OpenTemplateView OpenTemplateForm { get; set; }
        public RenameVariableView RenameVariableForm { get; set; }


        public TestSuiteViewAdapter(GenbiDispatcher dispatcher)
        {
            GlobalForm = new TestSuiteView(dispatcher, this);
            DisplayTestForm = new DisplayTestView(this);
            OpenTemplateForm = new OpenTemplateView(this);
            RenameVariableForm = new RenameVariableView(this);
        }

        public Form MainForm
        {
            get
            {
                return GlobalForm;
            }
        }

        #region properties

        public DataTable CsvContent
        {
            get
            {
                return GlobalForm.CsvContent;
            }
            set
            {
                GlobalForm.CsvContent=value;
            }
        }

        public bool UseGrouping
        {
            get
            {
                return GlobalForm.UseGrouping;
            }
            set
            {
                GlobalForm.UseGrouping = value;
            }
        }

        public BindingList<string> Variables
        {
            get
            {
                return GlobalForm.Variables;
            }
            set
            {
                GlobalForm.Variables = value;
            }

        }

        public BindingList<Test> Tests
        {
            get
            {
                return GlobalForm.Tests;
            }
            set
            {
                GlobalForm.Tests = value;
            }
        }

        public BindingList<string> EmbeddedTemplates
        {
            get
            {
                return OpenTemplateForm.EmbeddedTemplates;
            }
            set
            {
                OpenTemplateForm.EmbeddedTemplates = value;
            }
        }

        public string Template
        {
            get
            {
                return GlobalForm.Template;
            }
            set
            {
                GlobalForm.Template = value;
            }
        }

        public BindingList<string> SettingsNames
        {
            get
            {
                return GlobalForm.SettingsNames;
            }
            set
            {
                GlobalForm.SettingsNames = value;
            }
        }

        public string SettingsValue
        {
            get
            {
                return GlobalForm.SettingsValue;
            }
            set
            {
                GlobalForm.SettingsValue = value;
            }
        }

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
                    DisplayTestForm.TestContent = string.Empty;
                else
                    DisplayTestForm.TestContent = value.Content;
            }
        }

        public int TestSelectedIndex
        {
            get
            {
                return GlobalForm.TestSelectedIndex;
            }
            set
            {
                GlobalForm.TestSelectedIndex = value;
            }
        }

        public bool CanGenerate
        {
            set
            {
                GlobalForm.CanGenerate = value;
            }
        }

        public bool CanUndo
        {
            set
            {
                GlobalForm.CanUndo = value;
            }
        }

        public bool CanClear
        {
            set
            {
                GlobalForm.CanClear = value;
            }
        }

        public bool CanSaveAs
        {
            set
            {
                GlobalForm.CanSaveAs = value;
            }
        }

        public bool CanSaveTemplate
        {
            set
            {
                GlobalForm.CanSaveTemplate = value;
            }
        }

        public bool CanRename
        {
            set
            {
                GlobalForm.CanRename = value;
            }
        }

        public bool CanRemove
        {
            set
            {
                GlobalForm.CanRemove = value;
            }
        }

        public int ProgressValue
        {
            set
            {
                GlobalForm.ProgressValue = value;
            }
        }

        #endregion

        #region Invoke & Events

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

        #endregion


        public void ShowException(string text)
        {
            MessageBox.Show(text, "Genbi", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public void ShowInform(string text)
        {
            MessageBox.Show(text, "Genbi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }


        public int VariableSelectedIndex { get; set; }
    }
}
