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

            var setValueParameters = new Collection<PackageInfo.ExecutionValueParameterSet>()
            {
                new PackageInfo.ExecutionValueParameterSet
                {
                    ObjectType = 50,
                    ParameterName = "SYNCHRONIZED",
                    ParameterValue = 1
                }
            };
            var parameters = Parameterize(Etl.Parameters, package);
            parameters.ToList().ForEach(p => setValueParameters.Add(p));

            long executionIdentifier = -1;
            if (Etl.Timeout==0)
                executionIdentifier = package.Execute(Etl.Is32Bits, environmentReference, setValueParameters);
            else
                executionIdentifier = package.Execute(Etl.Is32Bits, environmentReference, setValueParameters, Etl.Timeout, connection.ConnectionString);

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

            var environmentName = Etl.Environment;
            var environmentFolder = folder.Name;
            if (Etl.Environment.Contains(@"\"))
            {
                var pathTokens = Etl.Environment.Split(new[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                environmentName = pathTokens.Last();
                environmentFolder = String.Join(@"\", pathTokens.Take(pathTokens.Length - 1));
            }
                

            if (!folder.Environments.Contains(environmentName))
            {
                var names = String.Join(", ",folder.Environments.Select(e => e.Name));
                throw new ArgumentOutOfRangeException("Environment", $"The environment named '{environmentName}' hasn't been found on the folder '{folder.Name}'. List of existing catalogs: {names}.");
            }

            if (!project.References.Contains(folder.Environments[environmentName].Name, environmentFolder))
            {
                var names = String.Join(", ", project.References.Select(r => r.Name));
                if (environmentFolder==folder.Name)
                    throw new ArgumentOutOfRangeException("Environment", $"The environment named '{environmentName}' from the project folder exists but is not referenced in the project '{project.Name}'. List of existing references: {names}.");
                else
                    throw new ArgumentOutOfRangeException("Environment", $"The environment named '{environmentName}' from the folder '{environmentFolder}' exists but is not referenced in the project '{project.Name}'. List of existing references: {names}.");
            }

            return project.References[folder.Environments[environmentName].Name, environmentFolder];
        }

        private PackageInfo GetPackage(IntegrationServices integrationServices)
        {
            if (!integrationServices.Catalogs.Contains(Etl.Catalog))
            {
                var names = String.Join(", ",integrationServices.Catalogs.Select(c => c.Name));
                throw new ArgumentOutOfRangeException("Catalog", $"The catalog named '{Etl.Catalog}' hasn't been found on the server '{Etl.Server}'. List of existing catalogs: {names}.");
            }
            
            var catalog = integrationServices.Catalogs[Etl.Catalog];

            if (!catalog.Folders.Contains(Etl.Folder))
            {
                var names = String.Join(", ", catalog.Folders.Select(f => f.Name));
                throw new ArgumentOutOfRangeException("Folder", $"The folder named '{Etl.Folder}' hasn't been found on the catalog '{Etl.Catalog}'. List of existing folders: {names}.");
            }
            var folder = catalog.Folders[Etl.Folder];

            if (!folder.Projects.Contains(Etl.Project))
            {
                var names = String.Join(", ", folder.Projects.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Project", $"The project named '{Etl.Project}' hasn't been found on the folder '{Etl.Folder}'. List of existing projects: {names}.");
            }
            var project = folder.Projects[Etl.Project];

            if (!project.Packages.Contains(Etl.Name))
            {
                var names = String.Join(", ", project.Packages.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Name", $"The package named '{Etl.Name}' hasn't been found on the project '{Etl.Project}'. List of existing packages: {names}.");
            }
            var package = project.Packages[Etl.Name];

            return package;
        }

        protected virtual IEnumerable<PackageInfo.ExecutionValueParameterSet> Parameterize(IEnumerable<EtlParameter> overridenParameters, PackageInfo package)
        {
            var existingParameters = new List<ParameterInfo>();
            existingParameters.AddRange(package.Parameters);

            if (package?.Parent?.Parameters!=null)
                existingParameters.AddRange(package.Parent.Parameters);

            foreach (var param in overridenParameters)
            {
                var existingParam = existingParameters.SingleOrDefault(x => x.Name == param.Name);
                if (existingParam == null)
                {
                    var existingParameterList = String.Join("', '", existingParameters.Select(n => $"\r\n - {n.Name} ({n.ObjectType})"));
                    throw new ArgumentOutOfRangeException(nameof(overridenParameters), $"No parameter named '{param.Name}' found in the package {package.Name}, can't override its value for execution. List of existing parameters:{existingParameterList}");
                }

                var execParam = new PackageInfo.ExecutionValueParameterSet()
                {
                    ObjectType = (existingParam as ParameterInfo).ObjectType,
                    ParameterName = param.Name,
                    ParameterValue = DefineValue(param.StringValue, (existingParam as ParameterInfo).DataType)
                };
                yield return execParam;
            }
        }

    }
}
#endif
