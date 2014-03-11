using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl.IntegrationService
{
    class EtlSqlServerRunner : EtlRunner
    {
        public EtlSqlServerRunner(IEtl etl)
            : base(etl)
        {

        }

        protected override Package Load(IEtl etl, Application app)
        {
            var server = etl.Server.Replace(" ", "");
            if(server.ToLower()=="(local)")
                server=".";

            var packageName = etl.Path + etl.Name;
            if (!packageName.ToLower().EndsWith(".dtsx"))
                packageName += ".dtsx";
            
            var package = app.LoadFromDtsServer(packageName, server, null);
            return package;                
        }
    }
}
