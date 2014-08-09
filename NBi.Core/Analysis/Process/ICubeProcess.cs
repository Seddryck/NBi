using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Process
{
    public interface ICubeProcess
    {
        string Cube { get; set; }
        string ConnectionString { get; set; }

    }
}
