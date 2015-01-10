using System;
using System.Linq;
using NBi.Core.Etl;

namespace NBi.Core
{
    public class ExecutionFactory
    {
        public IExecution Get(IExecutable executable)
        {
            if (executable is IEtl)
            {
                var factory = new EtlRunnerFactory();
                var instance = factory.Get(executable as IEtl);
                return instance;
            }
            else
                throw new ArgumentException();
        }
    }
}
