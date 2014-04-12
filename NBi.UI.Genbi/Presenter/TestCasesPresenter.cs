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

        public TestCasesPresenter(RenameVariableWindow renameVariablewindow, OpenQueryWindow openQueryWindow, FilterWindow filterWindow, TestCasesManager testCasesManager, DataTable testCases, BindingList<string> variables)
        {
            this.OpenTestCasesCommand = new OpenTestCasesCommand(this);
            this.OpenTestCasesQueryCommand = new OpenTestCasesQueryCommand(this, openQueryWindow);
            this.RenameVariableCommand = new RenameVariableCommand(this, renameVariablewindow);
            this.RemoveVariableCommand = new RemoveVariableCommand(this);
            this.MoveLeftVariableCommand = new MoveLeftVariableCommand(this);
            this.MoveRightVariableCommand = new MoveRightVariableCommand(this);
            this.FilterCommand = new FilterCommand(this, filterWindow);

            this.testCasesManager = testCasesManager;
            TestCases = testCases;
            Variables = variables;
        }

        public ICommand OpenTestCasesCommand { get; private set; }
        public ICommand OpenTestCasesQueryCommand { get; private set; }
        public ICommand RenameVariableCommand { get; private set; }
        public ICommand RemoveVariableCommand { get; private set; }
        public ICommand MoveLeftVariableCommand { get; private set; }
        public ICommand MoveRightVariableCommand { get; private set; }
        public ICommand FilterCommand { get; private set; }

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
                    this.MoveLeftVariableCommand.Refresh();
                    this.MoveRightVariableCommand.Refresh();
                    this.FilterCommand.Refresh();
                    break;
                case "VariableSelectedIndex":
                    this.RenameVariableCommand.Refresh();
                    this.RemoveVariableCommand.Refresh();
                    this.MoveLeftVariableCommand.Refresh();
                    this.MoveRightVariableCommand.Refresh();
                    break;
                default:
                    break;
            }
        }

        internal void LoadCsv(string fullPath)
        {
            testCasesManager.ReadFromCsv(fullPath);
            Reload();
            OnPropertyChanged("Variables");
        }

        internal void LoadQuery(string fullPath, string connectionString)
        {
            testCasesManager.ReadFromQueryFile(fullPath, connectionString);
            Reload();
            OnPropertyChanged("Variables");
        }

        private void Reload()
        {
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

            if (VariableSelectedIndex < 0 && Variables.Count > 0)
                VariableSelectedIndex = 0;
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

        internal bool IsFirst()
        {
            return VariableSelectedIndex == 0;
        }

        internal bool IsLast()
        {
            return VariableSelectedIndex == Variables.Count - 1;
        }

        internal void Move(int selectedIndex, int newPosition)
        {
            testCasesManager.MoveVariable(Variables[VariableSelectedIndex], newPosition);
            Reload();
            VariableSelectedIndex = newPosition;
            OnPropertyChanged("Variables");
        }

        internal void Filter(int selectedIndex, Operator @operator, bool negation, string text)
        {
            testCasesManager.Filter(Variables[VariableSelectedIndex], @operator, negation, text);
            Reload();
            OnPropertyChanged("TestCases");
        }
    }
}
