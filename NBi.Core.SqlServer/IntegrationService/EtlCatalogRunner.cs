using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Management.IntegrationServices;
using System.Collections.Generic;
using NBi.Core.Etl;

namespace NBi.Core.SqlServer2014.IntegrationService
{
    class EtlCatalogRunner : EtlRunner
    {
        public EtlCatalogRunner(IEtl etl)
            : base(etl)
        {
            var argumentNullExceptionSentence = "You must specify a value for parameter '{0}' when using an EtlCatalogRunner";
            if (string.IsNullOrEmpty(Etl.Server))
                throw new ArgumentNullException("Server", string.Format(argumentNullExceptionSentence, "Server"));

            if (string.IsNullOrEmpty(Etl.Catalog))
                throw new ArgumentNullException("Catalog", string.Format(argumentNullExceptionSentence, "Catalog"));

            if (string.IsNullOrEmpty(Etl.Folder))
                throw new ArgumentNullException("Folder", string.Format(argumentNullExceptionSentence, "Folder"));

            if (string.IsNullOrEmpty(Etl.Project))
                throw new ArgumentNullException("Project", string.Format(argumentNullExceptionSentence, "Project"));

            if (string.IsNullOrEmpty(Etl.Name))
                throw new ArgumentNullException("Name", string.Format(argumentNullExceptionSentence, "Name"));
        }

        public override IExecutionResult Run()
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=master;Integrated Security=SSPI;", Etl.Server));
            var integrationServices = new IntegrationServices(connection);

            Catalog catalog;
            PackageInfo package;
            GetPackage(integrationServices, out catalog, out package);
            

            var setValueParameters = new Collection<PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50,
                ParameterName = "SYNCHRONIZED",
                ParameterValue = 1
            });
            var parameters = Parameterize(Etl.Parameters, package.Parameters, package.Name);
            parameters.ToList().ForEach(p => setValueParameters.Add(p));

            long executionIdentifier = -1;
            if (Etl.Timeout==0)
                executionIdentifier = package.Execute(Etl.Is32Bits, null, setValueParameters);
            else
                executionIdentifier = package.Execute(Etl.Is32Bits, null, setValueParameters, Etl.Timeout);

            var execution = catalog.Executions[executionIdentifier];

            var etlRunResultFactory = new EtlRunResultFactory();

            var result = etlRunResultFactory.Instantiate(
                execution.Status
                , execution.Messages.Where(m => m.MessageType == 120 || m.MessageType == 110).Select(m => m.Message)
                , execution.StartTime
                , execution.EndTime);

            return result;

        }

        private void GetPackage(IntegrationServices integrationServices, out Catalog catalog, out PackageInfo package)
        {

            if (integrationServices.Catalogs.Contains(Etl.Catalog))
                catalog = integrationServices.Catalogs[Etl.Catalog];
            else
            {
                var names = String.Join(", ",integrationServices.Catalogs.Select(c => c.Name));
                throw new ArgumentOutOfRangeException("Catalog", String.Format("The catalog named '{0}' hasn't been found on the server '{1}'. List of existing catalogs: {2}.", Etl.Catalog, Etl.Server, names));
            }
                

            CatalogFolder folder;
            if (catalog.Folders.Contains(Etl.Folder))
                folder = catalog.Folders[Etl.Folder];
            else
            {
                var names = String.Join(", ", catalog.Folders.Select(f => f.Name));
                throw new ArgumentOutOfRangeException("Folder", String.Format("The folder named '{0}' hasn't been found on the catalog '{1}'. List of existing folders: {2}.", Etl.Folder, Etl.Catalog, names));
            }

            ProjectInfo project;
            if (folder.Projects.Contains(Etl.Project))
                project = folder.Projects[Etl.Project];
            else
            {
                var names = String.Join(", ", folder.Projects.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Project", String.Format("The project named '{0}' hasn't been found on the catalog '{1}'. List of existing projects: {2}.", Etl.Project, Etl.Folder, names));
            }

            if (project.Packages.Contains(Etl.Name))
                package = project.Packages[Etl.Name];
            else
            {
                var names = String.Join(", ", project.Packages.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Name", String.Format("The package named '{0}' hasn't been found on the project '{1}'. List of existing packages: {2}.", Etl.Name, Etl.Project, names));
            }
        }

        protected virtual IEnumerable<PackageInfo.ExecutionValueParameterSet> Parameterize(IEnumerable<EtlParameter> overridenParameters, ParameterCollection existingParameters, string packageName)
        {
            foreach (var param in overridenParameters)
            {
                if (!existingParameters.Contains(param.Name))
                    throw new ArgumentOutOfRangeException("overridenParameters", string.Format("No parameter named '{0}' found in the package {1}, can't override its value for execution.", param.Name, packageName));

                var existingParam = existingParameters[param.Name];
                var execParam = new PackageInfo.ExecutionValueParameterSet()
                {
                    ObjectType = existingParam.ObjectType,
                    ParameterName = param.Name,
                    ParameterValue = DefineValue(param.StringValue, existingParam.DataType)
                };
                yield return execParam;
            }
        }

    }
}
    