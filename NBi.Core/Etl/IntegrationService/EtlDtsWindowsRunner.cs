using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl.IntegrationService
{
    class EtlDtsWindowsRunner : EtlDtsRunner
    {
        public EtlDtsWindowsRunner(IEtl etl)
            : base(etl)
        {

        }

        protected override Package Load(IEtl etl, Application app)
        {
            var server = etl.Server.Replace(" ", "");
            if(server.ToLower()=="(local)")
                server=".";

            var packageName = etl.Path + etl.Name;

            var events = new PackageEvents();
            var package = app.LoadFromDtsServer(packageName, server, events);
            return package;                
        }
    }
}
    