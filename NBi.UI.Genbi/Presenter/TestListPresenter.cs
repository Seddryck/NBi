using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.Service;
using NBi.Service.Dto;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Test;
using NBi.UI.Genbi.Command.TestsXml;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.Stateful;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Presenter
{
    class TestListPresenter : BasePresenter<ITestsGenerationView>
    {
        private readonly TestListManager testListManager;
        public bool IsUndo { get; private set; }

        public TestListPresenter(ITestsGenerationView testsGenerationView, TestListManager testListManager, BindingList<Test> tests, DataTable testCases, BindingList<string> variables, string template)
            : base(testsGenerationView)
        {
            this.ClearTestsXmlCommand = new ClearTestListCommand(this);
            this.GenerateTestsXmlCommand = new GenerateTestListCommand(this);
            this.UndoGenerateTestsXmlCommand = new UndoGenerateTestListCommand(this);
            this.DeleteTestCommand = new DeleteTestCommand(this);
            this.DisplayTestCommand = new EditTestCommand(this, new DisplayTestView());


            this.testListManager = testListManager;

            Tests = tests;
            TestCases = testCases;
            Variables = variables;
            Template = template;

            testListManager.Progressed += (sender, e) => { Progress = Math.Min(100, 100 * e.Done / e.Total); };
        }

        public TestListManager Manager 
        { 
            get
            {
                return testListManager;
            }
        }

        public ICommand ClearTestsXmlCommand { get; private set; }
        public ICommand GenerateTestsXmlCommand { get; private set; }
        public ICommand UndoGenerateTestsXmlCommand { get; private set; }
        public ICommand DeleteTestCommand { get; private set; }
        public ICommand DisplayTestCommand { get; private set; }



        #region Bindable properties

        public BindingList<Test> Tests 
        {
            get { return GetValue<BindingList<Test>>("Tests"); }
            set { SetValue("Tests", value); }
        }

        public Test SelectedTest
        {
            get { return GetValue<Test>("SelectedTest"); }
            set { SetValue("SelectedTest", value); }
        }

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

        public string Template
        {
            get { return GetValue<string>("Template"); }
            set { SetValue("Template", value); }
        }

        public bool UseGrouping
        {
            get { return GetValue<bool>("UseGrouping"); }
            set { SetValue("UseGrouping", value); }
        }

        public int Progress
        {
            get { return GetValue<int>("Progress"); }
            set { SetValue("Progress", value); }
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "Tests":
                    this.ClearTestsXmlCommand.Refresh();
                    this.UndoGenerateTestsXmlCommand.Refresh();
                    break;
                case "SelectedTest":
                    this.DeleteTestCommand.Refresh();
                    this.DisplayTestCommand.Refresh();
                    break;
                case "TestCases":
                    this.GenerateTestsXmlCommand.Refresh();
                    break;
                case "Variables":
                    this.GenerateTestsXmlCommand.Refresh();
                    break;
                case "Template":
                    this.GenerateTestsXmlCommand.Refresh();
                    break;
                default:
                    break;
            }
        }

        internal TestListGenerationResult Generate()
        {
            try
            {
                testListManager.Build(Template, Variables.ToArray(), TestCases, UseGrouping);
            }
            catch (ExpectedVariableNotFoundException)
            {
                return TestListGenerationResult.Failure("The template has at least one variable which wasn't supplied by the test cases provider (CSV file). Check the name of the variables.");
            }
            catch (TemplateExecutionException ex)
            {
                return TestListGenerationResult.Failure(ex.Message);
            }

            IsUndo = true;
            ReloadTests();
            
            return TestListGenerationResult.Success(Tests.Count);
        }

        internal void Clear()
        {
            testListManager.Clear();
            IsUndo = false;
            ReloadTests();
        }

        internal void Undo()
        {
            testListManager.Undo();
            IsUndo = false;
            ReloadTests();
        }

        public void ReloadTests()
        {
            var tests = testListManager.GetTests();

            Tests.Clear();
            foreach (var test in tests)
                Tests.Add(test);
            OnPropertyChanged("Tests");
        }
    }
}
