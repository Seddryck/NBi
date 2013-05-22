using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.RunnerConfig;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.View
{
    public class GenbiDispatcher
    {

        protected TestSuiteGeneratorPresenter TestSuiteGeneratorPresenter {get; set;}
        protected RunnerConfigPresenter RunnerConfigPresenter {get; set;}

        public GenbiDispatcher()
        {

        }

        public void Initialize()
        {
            IAdapter adapter = null;
            
            adapter = new TestSuiteViewAdapter(this);
            TestSuiteGeneratorPresenter = new TestSuiteGeneratorPresenter((TestSuiteViewAdapter)adapter);
            adapter.InvokeInitialize(EventArgs.Empty);

            adapter = new RunnerConfigViewAdapter();
            RunnerConfigPresenter = new RunnerConfigPresenter((RunnerConfigViewAdapter)adapter);
            adapter.InvokeInitialize(EventArgs.Empty);           
        }

        internal Form GetMainForm()
        {
            return TestSuiteGeneratorPresenter.View.MainForm;
        }

        internal void StartRunnerConfig()
        {
            RunnerConfigPresenter.View.Show();
        }
    }
}
