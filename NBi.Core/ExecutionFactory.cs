using System;
using System.Linq;
using NBi.Core.Etl;
using NBi.Extensibility;
using NBi.Core.Decoration.DataEngineering;
using NBi.Extensibility.Decoration.DataEngineering;

namespace NBi.Core
{
    public class ExecutionFactory
    {
        public IExecutable Instantiate(IExecutableArgs args)
        {
            switch (args)
            {
                case IEtlArgs etl: return new EtlRunnerProvider().Instantiate(etl.Version).Instantiate(etl);
                default:throw new ArgumentException();
            }
        }
    }
}
