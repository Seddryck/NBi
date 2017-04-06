using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch
{
    public interface IBatchRunCommand
    {
        string FullPath { get; }

        string ConnectionString { get; }

        string Version { get; }
    }
}
