using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Process;

namespace NBi.Core.Process
{
    public interface IRunCommand : IProcessCommand
    {

        string FullPath { get; }
        string Argument { get; }
        int TimeOut { get; set; }
    }
}
