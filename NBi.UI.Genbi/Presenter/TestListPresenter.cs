using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.Service;
using NBi.Service.Dto;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Test;
using NBi.UI.Genbi.Command.TestsXml;
using NBi.UI.Genbi.Stateful;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using NBi.GenbiL.Stateful;

namespace NBi.UI.Genbi.Presenter
{
    class TestListPresenter : PresenterBase
    {
        private readonly GenerationState state;
        public GenerationState State
        {
            get { return state; }
        }
        public bool IsUndo { get; private set; }

        public TestListPresenter(GenerationState state)
        {
            this.ClearTestsXmlCommand = new ClearTestListCommand(this);
            this.GenerateTestsXmlCommand = new GenerateTestListCommand(this);
            this.UndoGenerateTestsXmlCommand = new UndoGenerateTestListCommand(this);
            this.DeleteTestCommand = new DeleteTestCommand(this);
            this.DisplayTestCommand = new EditTestCommand(this, new DisplayTestView());
            this.AddCategoryCommand = new AddCategoryTestCommand(this, new NewCategoryWindow());

            this.state = state;
            //testListManager.Progressed += (sender, e) => 
            //{
            //    var newValue = Math.Min(100, 100 * e.Done / e.Total);
            //    if (newValue - Progress >= 5 || (newValue==0 && Progress!=0) || (newValue==100 && Progress!=100))
            //        Progress = newValue; 
            //};
        }

        public ICommand ClearTestsXmlCommand { get; private set; }
        public ICommand GenerateTestsXmlCommand { get; private set; }
        public ICommand UndoGenerateTestsXmlCommand { get; private set; }
        public ICommand DeleteTestCommand { get; private set; }
        public ICommand DisplayTestCommand { get; private set; }
        public ICommand AddCategoryCommand { get; private set; }


        #region Bindable properties

        public LargeBindingList<Test> Tests 
        {
            get { return GetValue<LargeBindingList<Test>>("Tests"); }
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

        public IEnumerable<Test> SelectedTests
        {
            get { return GetValue<IEnumerable<Test>>("SelectedTests"); }
            set { SetValue("SelectedTests", value); }
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
                    this.AddCategoryCommand.Refresh();
                    break;
                case "SelectedTests":
                    this.DeleteTestCommand.Refresh();
                    this.DisplayTestCommand.Refresh();
                    this.AddCategoryCommand.Refresh();
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

        //internal TestListGenerationResult Generate()
        //{
        //    TestListGenerationResult message = null;
        //    try
        //    {
        //        Progress = 0;
        //        OnGenerationStarted(EventArgs.Empty);
        //        //TODO testListManager.Build(Template, Variables.ToArray(), TestCases, UseGrouping);
        //        Progress = 100;
        //        IsUndo = true;
        //        ReloadTests();
        //        message = TestListGenerationResult.Success(Tests.Count);
        //    }
        //    catch (ExpectedVariableNotFoundException)
        //    {
        //        message = TestListGenerationResult.Failure("The template has at least one variable which wasn't supplied by the test cases provider (CSV file). Check the name of the variables.");
        //    }
        //    catch (TemplateExecutionException ex)
        //    {
        //        message = TestListGenerationResult.Failure(ex.Message);
        //    }
        //    finally
        //    {
        //        OnGenerationEnded(EventArgs.Empty);
        //    }

        //    return message;
        //}

        internal void Clear()
        {
            //testListManager.Clear();
            //IsUndo = false;
            ReloadTests();
        }

        internal void Undo()
        {
            //testListManager.Undo();
            IsUndo = false;
            ReloadTests();
        }

        internal void AddCategory(string categoryName)
        {
            //foreach (var test in SelectedTests)
            //    testListManager.AddCategory(test, categoryName);
            
            //ReloadTests();
        }

        public void ReloadTests()
        {
            //var tests = testListManager.GetTests();

            //Tests.Clear();
            //Tests.AddRange(tests);
            //foreach (var test in tests)
            //    Tests.Add(test);
            OnPropertyChanged("Tests");
        }

        public event EventHandler<EventArgs> GenerationStarted;

        protected void OnGenerationStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = GenerationStarted;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<EventArgs> GenerationEnded;

        protected void OnGenerationEnded(EventArgs e)
        {
            EventHandler<EventArgs> handler = GenerationEnded;
            if (handler != null)
                handler(this, e);
        }



        
    }
}
