using System;
using System.Linq;
using NBi.Core.Etl;
using NBi.Extensibility;
using NBi.Extensibility.DataEngineering;

namespace NBi.Core
{
    public class ExecutionFactory
    {
        public IExecutable Instantiate(IExecutableArgs args)
        {
            switch (args)
            {
                case IEtlArgs etl: return new EtlRunnerProvider().Instantiate(etl);
                default:throw new ArgumentException();
            }
        }
    }
}
