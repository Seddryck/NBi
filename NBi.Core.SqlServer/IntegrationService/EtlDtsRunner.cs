using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using NBi.Core.Etl;

namespace NBi.Core.SqlServer.IntegrationService
{
    abstract class EtlDtsRunner : EtlRunner
    {
        public EtlDtsRunner(IEtl etl) : base(etl) 
        {
            var argumentNullExceptionSentence = "You must specify a value for parameter '{0}' when using an EtlDtsRunner";

            if (string.IsNullOrEmpty(Etl.Path))
                throw new ArgumentNullException("Path", string.Format(argumentNullExceptionSentence, "Path"));

            if (string.IsNullOrEmpty(Etl.Name))
                throw new ArgumentNullException("Name", string.Format(argumentNullExceptionSentence, "Name"));
        }

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
                #if ! SqlServer2008R2
                if (package.Parameters.Contains(param.Name))
                    package.Parameters[param.Name].Value = param.StringValue;
                else
                {
                #endif
                    if (package.Variables.Contains(param.Name))
                        package.Variables[param.Name].Value = DefineValue(param.StringValue, package.Variables[param.Name].DataType);
                    else if ((package.Parent as Package) != null && (package.Parent as Package).Parameters.Contains(param.Name))
                        (package.Parent as Package).Parameters[param.Name].Value = param.StringValue;
                    else
                        throw new ArgumentOutOfRangeException(param.Name, $"No parameter or variable named '{param.Name}' found in the package {package.Name} or its parent, can't override its value for execution.");
                #if ! SqlServer2008R2 
                }
                #endif
            }
        }
    }
}
