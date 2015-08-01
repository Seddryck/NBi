using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using NBi.Core.Etl;

namespace NBi.Core.SqlServer.IntegrationService
{
    class EtlDtsWindowsRunner : EtlDtsRunner
    {
        public EtlDtsWindowsRunner(IEtl etl)
            : base(etl)
        {
            var argumentNullExceptionSentence = "You must specify a value for parameter '{0}' when using an EtlDtsWindowsRunner";
            if (string.IsNullOrEmpty(Etl.Server))
                throw new ArgumentNullException("Server", string.Format(argumentNullExceptionSentence, "Server"));
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
    