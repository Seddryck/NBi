using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Service;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.TestCases;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Presenter
{
    class TestCasesPresenter : PresenterBase
    {
        private readonly TestCasesManager testCasesManager;

        public TestCasesPresenter(RenameVariableWindow window,TestCasesManager testCasesManager, DataTable testCases, BindingList<string> variables)
        {
            this.OpenTestCasesCommand = new OpenTestCasesCommand(this);
            this.RenameVariableCommand = new RenameVariableCommand(this, window);
            this.RemoveVariableCommand = new RemoveVariableCommand(this);

            this.testCasesManager = testCasesManager;
            TestCases = testCases;
            Variables = variables;
        }

        public ICommand OpenTestCasesCommand { get; private set; }
        public ICommand RenameVariableCommand { get; private set; }
        public ICommand RemoveVariableCommand { get; private set; }

        #region Bindable properties

        public DataTable TestCases
        {
            get { return GetValue<DataTable>("TestCases"); }
            set { SetValue("TestCases", value); }
        }

        public BindingList<string> Variables
        {
            get { return GetValue<BindingList<string>>("Variables"); }
            set { SetValue("Variables", value); }
        }

        public string NewVariableName
        {
            get { return this.GetValue<string>("NewVariableName"); }
            set { this.SetValue("NewVariableName", value); }
        }
        
        public int VariableSelectedIndex
        {
            get { return GetValue<int>("VariableSelectedIndex"); }
            set { SetValue<int>("VariableSelectedIndex", value); }
        }      

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "TestCases":
                    break;
                case "Variables":
                    this.RenameVariableCommand.Refresh();
                    this.RemoveVariableCommand.Refresh();
                    break;
                case "VariableSelectedIndex":
                    this.RenameVariableCommand.Refresh();
                    this.RemoveVariableCommand.Refresh();
                    break;
                default:
                    break;
            }
        }

        internal void LoadCsv(string fullPath)
        {
            testCasesManager.ReadFromCsv(fullPath);

            var dtReader = new DataTableReader(testCasesManager.Content);

            //Reset the state of the DataTable
            //Remove the Sort Order or you'll be in troubles when loading the datatable
            TestCases.DefaultView.Sort = String.Empty;
            TestCases.Rows.Clear();
            TestCases.Columns.Clear();
            TestCases.RejectChanges();

            //Load it
            TestCases.Load(dtReader, LoadOption.PreserveChanges); 
            OnPropertyChanged("TestCases");

            //Take care of variables
            Variables.Clear();
            foreach (var v in testCasesManager.Variables)
                Variables.Add(v);
            OnPropertyChanged("Variables");
        }

        internal void Rename(int index, string newName)
        {
            Variables[index] = newName;
            OnPropertyChanged("Variables");
            TestCases.Columns[index].ColumnName = newName;
        }

        internal void Remove(int index)
        {
            Variables.RemoveAt(index);
            OnPropertyChanged("Variables");
            TestCases.Columns.RemoveAt(index);
        }

        internal bool IsRenamable()
        {
            return Variables.Count > 0;
        }

        internal bool IsDeletable()
        {
            return Variables.Count > 1;
        }

        internal bool IsValidVariableName(string variableName)
        {
            return !string.IsNullOrEmpty(variableName) && Variables.Contains(variableName);
        }
        
        
    }
}
