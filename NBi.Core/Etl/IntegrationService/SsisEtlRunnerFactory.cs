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
            else if (!string.IsNullOrEmpty(etl.Catalog) || !string.IsNullOrEmpty(etl.Folder) || !string.IsNullOrEmpty(etl.Project))
                return new EtlCatalogRunner(etl);
            else if (string.IsNullOrEmpty(etl.UserName))
                return new EtlDtsWindowsRunner(etl);
            else
                return new EtlDtsSqlServerRunner(etl);
        }
    }
}
