using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Etl.IntegrationService
{
    class SsisEtlRunnerFactory
    {
        public IEtlRunner Get(IEtl etl)
        {
            if (string.IsNullOrEmpty(etl.Server))
                return new EtlFileRunner(etl);
            else
                return new EtlSqlServerRunner(etl);
        }
    }
}
