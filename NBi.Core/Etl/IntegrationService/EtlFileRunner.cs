using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl.IntegrationService
{
    class EtlFileRunner : EtlRunner
    {
        public EtlFileRunner(IEtl etl) : base(etl)
        {

        }

        protected override Package Load(IEtl etl, Application app)
        {
            var packageName = etl.Path + etl.Name;
            if (!packageName.ToLower().EndsWith(".dtsx"))
                packageName += ".dtsx";

            var events = new PackageEvents();
            var package = app.LoadPackage(packageName, events);
            return package;                
        }
    }
}
