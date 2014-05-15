using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.RunnerConfig
{
    public class RunnerConfigBuildEventArgs : EventArgs
    {
        public string RootPath { get; set; }
        public string FrameworkPath { get; set; }
        public string TestSuitePath { get; set; }

        public bool IsNUnit { get; set; }
        public bool IsGallio { get; set; }
    }
}
