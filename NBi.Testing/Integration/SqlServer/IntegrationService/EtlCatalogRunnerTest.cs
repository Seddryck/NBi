using Microsoft.SqlServer.Management.IntegrationServices;
using NBi.Core.Etl;
using NBi.Core.SqlServer.IntegrationService;
using NBi.Xml.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.SqlServer.IntegrationService
{
    [TestFixture]
    [global::NUnit.Framework.Category("Etl")]
    public class EtlCatalogRunnerTest
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [SetUp]
        public void SetUp()
        {
            CleanTables();
            CreateFlatFiles();
        }

        private void CleanTables()
        {
            var tableNames = new[] { "Geography", "Scientist" };
            foreach (var tableName in tableNames)
                using (var conn = new OleDbConnection(ConnectionStringReader.GetIntegrationServerTargetDatabase()))
                {
                    conn.Open();
                    var cmd = new OleDbCommand($"truncate table Dim{tableName};", conn);
                    cmd.ExecuteNonQuery();
                }
        }

        private void CreateFlatFiles()
        {
            var filenames = new[] { "Calendar", "Geography", "Scientist" };
            foreach (var filename in filenames)
                DiskOnFile.CreatePhysicalFile($@"data\Dim{filename}.csv", $"{GetType().Namespace}.Resources.Dim{filename}.csv");
        }

        [Test]
        public void Execute_WithoutTimeout_CorrectlyHandled()
        {
            var etl = new EtlXml()
            {
                Server = ConnectionStringReader.GetIntegrationServer(),
                Catalog = "SSISDB",
                Folder = "Demo.NBi.Ssis",
                Project = "Demo.NBi.Ssis.Integration",
                Name = "Dimensions.dtsx",
            };
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "FlatFileScientist", StringValue = $@"{AssemblyDirectory}\data\DimScientist.csv" });
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "FlatFileGeography", StringValue = $@"{AssemblyDirectory}\data\DimGeography.csv" });
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "ConnectionString", StringValue = ConnectionStringReader.GetIntegrationServerTargetDatabase() });

            var runner = new EtlCatalogRunner(etl);
            runner.Execute();
        }

        [Test]
        public void Execute_ShortWithTimeout_SqlException()
        {
            var etl = new EtlXml()
            {
                Server = ConnectionStringReader.GetIntegrationServer(),
                Catalog = "SSISDB",
                Folder = "Demo.NBi.Ssis",
                Project = "Demo.NBi.Ssis.Integration",
                Name = "Dimensions.dtsx",
                Timeout = 1
            };
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "FlatFileScientist", StringValue = $@"{AssemblyDirectory}\data\DimScientist.csv" });
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "FlatFileGeography", StringValue = $@"{AssemblyDirectory}\data\DimGeography.csv" });
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "ConnectionString", StringValue = ConnectionStringReader.GetIntegrationServerTargetDatabase() });
            
            var runner = new EtlCatalogRunner(etl);
            Assert.Throws<SqlException>(() => runner.Execute());
        }

        [Test]
        public void Execute_WithLongTimeout_Success()
        {
            var etl = new EtlXml()
            {
                Server = ConnectionStringReader.GetIntegrationServer(),
                Catalog = "SSISDB",
                Folder = "Demo.NBi.Ssis",
                Project = "Demo.NBi.Ssis.Integration",
                Name = "Dimensions.dtsx",
                Timeout = 60
            };
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "FlatFileScientist", StringValue = $@"{AssemblyDirectory}\data\DimScientist.csv" });
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "FlatFileGeography", StringValue = $@"{AssemblyDirectory}\data\DimGeography.csv" });
            etl.InternalParameters.Add(new EtlParameterXml() { Name = "ConnectionString", StringValue = ConnectionStringReader.GetIntegrationServerTargetDatabase() });

            var runner = new EtlCatalogRunner(etl);
            runner.Execute();
        }
    }
}
