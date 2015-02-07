using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.Service;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.TestCases;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using NBi.GenbiL.Stateful;
using NBi.GenbiL.Action.Case;

namespace NBi.UI.Genbi.Presenter
{
    class TestCasesPresenter : PresenterBase
    {
        private readonly GenerationState state;
        public GenerationState State
        {
            get { return state; }
        }
        private readonly ConnectionStringManager ConnectionStringManager;

        public TestCasesPresenter(RenameVariableWindow renameVariablewindow, FilterWindow filterWindow, ConnectionStringWindow connectionStringWindow, GenerationState state)
        {
            this.OpenTestCasesCommand = new OpenTestCasesCommand(this);
            this.OpenTestCasesQueryCommand = new OpenTestCasesQueryCommand(this);
            this.RenameVariableCommand = new RenameVariableCommand(this, renameVariablewindow);
            this.RemoveVariableCommand = new RemoveVariableCommand(this);
            this.MoveLeftVariableCommand = new MoveLeftVariableCommand(this);
            this.MoveRightVariableCommand = new MoveRightVariableCommand(this);
            this.FilterCommand = new FilterCommand(this, filterWindow);
            this.FilterDistinctCommand = new FilterDistinctCommand(this);
            this.AddConnectionStringCommand = new AddConnectionStringCommand(this, connectionStringWindow);
            this.RemoveConnectionStringCommand = new RemoveConnectionStringCommand(this);
            this.EditConnectionStringCommand = new EditConnectionStringCommand(this, connectionStringWindow);
            this.RunQueryCommand = new RunQueryCommand(this);

            this.state = state;
            TestCases = state.TestCaseSetCollection.Scope.Content;
            Variables = new BindingList<string>(state.TestCaseSetCollection.Scope.Variables);
            //ConnectionStringNames = State.ConnectionStrings.Names;
            ConnectionStringSelectedIndex = -1;
            VariableSelectedIndex = -1;
        }

        public ICommand OpenTestCasesCommand { get; private set; }
        public ICommand OpenTestCasesQueryCommand { get; private set; }
        public ICommand RenameVariableCommand { get; private set; }
        public ICommand RemoveVariableCommand { get; private set; }
        public ICommand MoveLeftVariableCommand { get; private set; }
        public ICommand MoveRightVariableCommand { get; private set; }
        public ICommand FilterCommand { get; private set; }
        public ICommand FilterDistinctCommand { get; private set; }
        public ICommand AddConnectionStringCommand { get; private set; }
        public ICommand RemoveConnectionStringCommand { get; private set; }
        public ICommand EditConnectionStringCommand { get; private set; }
        public ICommand RunQueryCommand { get; private set; }

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

        public BindingList<string> ConnectionStringNames
        {
            get { return GetValue<BindingList<string>>("ConnectionStringNames"); }
            set { SetValue("ConnectionStringNames", value); }
        } 

        public int ConnectionStringSelectedIndex
        {
            get { return GetValue<int>("ConnectionStringSelectedIndex"); }
            set { SetValue<int>("ConnectionStringSelectedIndex", value); }
        }

        public string ConnectionStringSelectedName
        {
            get { return ConnectionStringNames[ConnectionStringSelectedIndex]; }
        }

        public string ConnectionStringSelectedValue
        {
            get { return State.ConnectionStrings.Get(ConnectionStringSelectedName); }
        }

        public string Query
        {
            get { return this.GetValue<string>("Query"); }
            set { this.SetValue("Query", value); }
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
                    this.FilterDistinctCommand.Refresh();
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
                case "ConnectionStringNames":
                    ReloadConnectionStrings();
                    this.RemoveConnectionStringCommand.Refresh();
                    this.EditConnectionStringCommand.Refresh();
                    break;
                case "ConnectionStringSelectedIndex":
                    this.RemoveConnectionStringCommand.Refresh();
                    this.EditConnectionStringCommand.Refresh();
                    this.RunQueryCommand.Refresh();
                    break;
                case "Query":
                    this.RunQueryCommand.Refresh();
                    break;
                default:
                    break;
            }
        }

        public void Reload()
        {
            //
        }

        private void ReloadConnectionStrings()
        {
     //       //Take care of variables
     //       ConnectionStringNames.Clear();
     //       foreach (var connStr in ConnectionStringManager.Names)
     //e           ConnectionStringNames.Add(connStr);

     //       if (ConnectionStringSelectedIndex < 0 && ConnectionStringNames.Count > 0)
     //           ConnectionStringSelectedIndex = 0;
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
    }
}
