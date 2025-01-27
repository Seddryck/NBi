using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Decoration.DataEngineering;

public interface IEtlRunner : IExecutable
{
    new IEtlRunResult Execute();
}
