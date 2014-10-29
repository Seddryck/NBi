using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl.IntegrationService
{
    abstract class EtlDtsRunner : EtlRunner
    {
        public EtlDtsRunner(IEtl etl) : base(etl) {}

        public override IExecutionResult Run()
        {
            var app = new Application();
            if (!string.IsNullOrEmpty(Etl.Password))
                app.PackagePassword = Etl.Password;
            var package = Load(Etl, app);

            Parameterize(Etl.Parameters, ref package);

            var events = new PackageEvents();
            var pkgResults = package.Execute(null, null, events, null, null);
            var result = (ExecResult)pkgResults;
            return EtlRunResult.Build(result, events);
        }

        protected abstract Package Load(IEtl etl, Application app);

        protected virtual void Parameterize(IEnumerable<EtlParameter> parameters, ref Package package)
        {
            foreach (var param in parameters)
            {
                package.Parameters[param.Name].Value = param.StringValue;
            }
        }
    }
}
