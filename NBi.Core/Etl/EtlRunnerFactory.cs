using System;
using System.Linq;
using NBi.Core.Etl.IntegrationService;

namespace NBi.Core.Etl
{
    public class EtlRunnerFactory
    {
        public IEtlRunner Get(IEtl etl)
        {
            var factory = new SsisEtlRunnerFactory();
            var runner = factory.Get(etl);
            return runner;
        }

    }
}
