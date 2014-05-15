using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl.IntegrationService
{
    abstract class EtlRunner: IEtlRunner
    {
        public IEtl Etl { get; private set; }

        public EtlRunner(IEtl etl)
        {
            Etl = etl;
        }

        public abstract IExecutionResult Run();

        public void Execute()
        {
            Run();
        }
    }
}
