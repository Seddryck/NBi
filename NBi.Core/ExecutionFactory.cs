using System;
using System.Linq;
using NBi.Core.Etl;
using NBi.Extensibility.DataEngineering;

namespace NBi.Core
{
    public class ExecutionFactory
    {
        public IExecution Instantiate(IExecutable executable)
        {
            switch (executable)
            {
                case IEtlExecutable etl: return new EtlRunnerFactory().Instantiate(etl);
                default:throw new ArgumentException();
            }
        }
    }
}
