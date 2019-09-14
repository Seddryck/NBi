using NBi.Extensibility.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class BatchRunnerArgs : IBatchRunnerArgs
    {
        public string FullPath { get; set; }
        public string ConnectionString { get; set; }
    }
}
