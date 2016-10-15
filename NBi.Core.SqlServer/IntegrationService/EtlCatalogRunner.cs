#if ! SqlServer2008R2
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Management.IntegrationServices;
using System.Collections.Generic;
using NBi.Core.Etl;


namespace NBi.Core.SqlServer.IntegrationService
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

            var package = GetPackage(integrationServices);

            var environmentReference = GetEnvironmentReference(package.Parent);

            var setValueParameters = new Collection<PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50,
                ParameterName = "SYNCHRONIZED",
                ParameterValue = 1
            });
            var parameters = Parameterize(Etl.Parameters, package, package.Name);
            parameters.ToList().ForEach(p => setValueParameters.Add(p));

            long executionIdentifier = -1;
            if (Etl.Timeout==0)
                executionIdentifier = package.Execute(Etl.Is32Bits, environmentReference, setValueParameters);
            else
                executionIdentifier = package.Execute(Etl.Is32Bits, environmentReference, setValueParameters, Etl.Timeout);

            var execution = package.Parent.Parent.Parent.Executions[executionIdentifier];

            var etlRunResultFactory = new EtlRunResultFactory();

            var result = etlRunResultFactory.Instantiate(
                execution.Status
                , execution.Messages.Where(m => m.MessageType == 120 || m.MessageType == 110).Select(m => m.Message)
                , execution.StartTime
                , execution.EndTime);

            return result;

        }

        private EnvironmentReference GetEnvironmentReference(ProjectInfo project)
        {
            var folder = project.Parent;

            if (string.IsNullOrEmpty(Etl.Environment))
                return null;

            if (!folder.Environments.Contains(Etl.Environment))
            {
                var names = String.Join(", ",folder.Environments.Select(e => e.Name));
                throw new ArgumentOutOfRangeException("Environment", String.Format("The environment named '{0}' hasn't been found on the folder '{1}'. List of existing catalogs: {2}.", Etl.Environment, folder.Name, names));
            }

            if (!project.References.Contains(folder.Environments[Etl.Environment].Name, folder.Name))
            {
                var names = String.Join(", ", project.References.Select(r => r.Name));
                throw new ArgumentOutOfRangeException("Environment", String.Format("The environment named '{0}' exists but is not referenced in the project '{1}'. List of existing references: {2}.", Etl.Environment, project.Name, names));
            }

            return project.References[folder.Environments[Etl.Environment].Name, folder.Name];
        }

        private PackageInfo GetPackage(IntegrationServices integrationServices)
        {
            if (!integrationServices.Catalogs.Contains(Etl.Catalog))
            {
                var names = String.Join(", ",integrationServices.Catalogs.Select(c => c.Name));
                throw new ArgumentOutOfRangeException("Catalog", String.Format("The catalog named '{0}' hasn't been found on the server '{1}'. List of existing catalogs: {2}.", Etl.Catalog, Etl.Server, names));
            }
            
            var catalog = integrationServices.Catalogs[Etl.Catalog];

            if (!catalog.Folders.Contains(Etl.Folder))
            {
                var names = String.Join(", ", catalog.Folders.Select(f => f.Name));
                throw new ArgumentOutOfRangeException("Folder", String.Format("The folder named '{0}' hasn't been found on the catalog '{1}'. List of existing folders: {2}.", Etl.Folder, Etl.Catalog, names));
            }
            var folder = catalog.Folders[Etl.Folder];

            if (!folder.Projects.Contains(Etl.Project))
            {
                var names = String.Join(", ", folder.Projects.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Project", String.Format("The project named '{0}' hasn't been found on the catalog '{1}'. List of existing projects: {2}.", Etl.Project, Etl.Folder, names));
            }
            var project = folder.Projects[Etl.Project];

            if (!project.Packages.Contains(Etl.Name))
            {
                var names = String.Join(", ", project.Packages.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Name", String.Format("The package named '{0}' hasn't been found on the project '{1}'. List of existing packages: {2}.", Etl.Name, Etl.Project, names));
            }
            var package = project.Packages[Etl.Name];

            return package;
        }

        protected virtual IEnumerable<PackageInfo.ExecutionValueParameterSet> Parameterize(IEnumerable<EtlParameter> overridenParameters, PackageInfo package, string packageName)
        {
            var existingParameters = package.Parameters;
            foreach (var projectParam in package.Parent.Parameters)
                existingParameters.Add(projectParam);

            foreach (var param in overridenParameters)
            {

                if (!existingParameters.Contains(param.Name))
                {
                    var existingParameterList = String.Join("', '", existingParameters.Select(n => string.Format("{0} ({1})", n.Name, n.ObjectType)));
                    throw new ArgumentOutOfRangeException("overridenParameters", string.Format("No parameter named '{0}' found in the package {1}, can't override its value for execution. List of existing parameters '{2}'", param.Name, packageName, existingParameterList));
                }

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
#endif
