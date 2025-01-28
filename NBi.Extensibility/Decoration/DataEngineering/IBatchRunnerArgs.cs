using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Decoration.DataEngineering;

public interface IBatchRunnerArgs
{
    string FullPath { get; set; }
    string ConnectionString { get; set; }
}
