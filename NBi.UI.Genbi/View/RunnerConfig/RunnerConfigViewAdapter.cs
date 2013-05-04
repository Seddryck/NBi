using System;
using System.Linq;
using NBi.UI.Genbi.Interface.RunnerConfig;

namespace NBi.UI.Genbi.View.RunnerConfig
{
    public class RunnerConfigViewAdapter: IRunnerConfigView
    {
        public RunnerConfigView MainForm { get; set; }

        public RunnerConfigViewAdapter()
        {
            MainForm = new RunnerConfigView(this);
        }
        
        public event EventHandler<RunnerConfigBuildEventArgs> Build;
        public void InvokeBuild(RunnerConfigBuildEventArgs e)
        {
            var handler = Build;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler Close;
        public void InvokeClose(EventArgs e)
        {
            var handler = Close;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler Initialize;
        public void InvokeInitialize(EventArgs e)
        {
            var handler = Initialize;
            if (handler != null)
                handler(this, e);
        }

        public void ShowException(string text)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void ShowInform(string text)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
