using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Management.IntegrationServices;

namespace NBi.Core.Etl.IntegrationService
{
    class EtlCatalogRunner : EtlRunner
    {
        public EtlCatalogRunner(IEtl etl)
            : base(etl)
        {

        }

        public override IExecutionResult Run()
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=master;Integrated Security=SSPI;", Etl.Server));
            var integrationServices = new IntegrationServices(connection);

            var catalog = integrationServices.Catalogs[Etl.Catalog];
            var folder  = catalog.Folders[Etl.Folder];
            var project = folder.Projects[Etl.Project];
            var package = project.Packages[Etl.Name];

            var setValueParameters = new Collection<PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50,
                ParameterName = "SYNCHRONIZED",
                ParameterValue = 1
            });

            long executionIdentifier = package.Execute(Etl.Is32Bits, null, setValueParameters);

            var execution = catalog.Executions[executionIdentifier];

            var result = EtlRunResult.Build(
                execution.Status
                , execution.Messages.Where(m => m.MessageType == 120 || m.MessageType == 110).Select(m => m.Message)
                , execution.StartTime
                , execution.EndTime);

            return result;

        }

    }
}
    