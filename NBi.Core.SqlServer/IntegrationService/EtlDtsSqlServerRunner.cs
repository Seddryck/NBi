using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using NBi.Core.Etl;

namespace NBi.Core.SqlServer.IntegrationService
{
    class EtlDtsSqlServerRunner : EtlDtsRunner
    {
        public EtlDtsSqlServerRunner(IEtl etl)
            : base(etl)
        {
            var argumentNullExceptionSentence = "You must specify a value for parameter '{0}' when using an EtlDtsSqlServerRunner";
            if (string.IsNullOrEmpty(Etl.Server))
                throw new ArgumentNullException("Server", string.Format(argumentNullExceptionSentence, "Server"));

            if (string.IsNullOrEmpty(Etl.UserName))
                throw new ArgumentNullException("UserName", string.Format(argumentNullExceptionSentence, "UserName"));

            if (string.IsNullOrEmpty(Etl.Password))
                throw new ArgumentNullException("Password", string.Format(argumentNullExceptionSentence, "Password"));
        }

        protected override Package Load(IEtl etl, Application app)
        {
            var server = etl.Server.Replace(" ", "");
            if(server.ToLower()=="(local)")
                server=".";

            var packageName = etl.Path + etl.Name;
            if (!packageName.ToLower().EndsWith(".dtsx"))
                packageName += ".dtsx";

            var events = new PackageEvents();
            var package = app.LoadFromSqlServer(packageName, server, etl.UserName, etl.Password, events);
            return package;                
        }
    }
}
    