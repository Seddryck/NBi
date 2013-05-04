using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.RunnerConfig
{
    public interface IRunnerConfigView : IView
    {
        event EventHandler<RunnerConfigBuildEventArgs> Build;
        event EventHandler Close;

        void Show();
    }
}
