using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Moq;
using NBi.Core.Etl;
using NBi.Core.Etl.IntegrationService;
using NBi.Xml.Items;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Etl.IntegrationService
{
    [TestFixture]
    [Category ("Etl")]
    public class EtlFileRunnerTest
    {
        
        private static bool isIntegrationServiceStarted = false;

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {
            isIntegrationServiceStarted = CheckIfIntegrationServiceStarted();

            //Build the fullpath for the file to read
            Directory.CreateDirectory("ETL");
            DiskOnFile.CreatePhysicalFile(@"Etl\Sample.dtsx", "NBi.Testing.Integration.Core.Etl.IntegrationService.Resources.Sample.dtsx");
        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
            if (!isIntegrationServiceStarted)
                Assert.Ignore("Integration Service not started.");
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }

        private bool CheckIfIntegrationServiceStarted()
        {
            var pname = Process.GetProcesses().Where(p => p.ProcessName.Contains("MsDtsSrvr"));
            return pname.Count() > 0;
        }
        #endregion

        [Test]
        public void Execute_ExistingSamplePackage_Success()
        {
            var etl = Mock.Of<IEtl>( e =>
                e.Server == string.Empty
                && e.Path == @"Etl\"
                && e.Name == "Sample.dtsx"
                && e.Password=="p@ssw0rd"
                && e.Parameters == new List<EtlParameter>()
                );
            
            var runner = new EtlFileRunner(etl);
            var result = runner.Run();

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Execute_ExistingSamplePackageWithParameter_SuccessAndParameterUsed()
        {
            var destPath = DiskOnFile.GetDirectoryPath() + "SampleFile.txt";
            if(File.Exists(destPath))
                File.Delete(destPath);

            var etl = new EtlXml();
            etl.Path = @"Etl\";
            etl.Name = "Sample.dtsx";
            etl.Password = "p@ssw0rd";
            var param = new EtlParameterXml();
            param.Name="DestinationPath";
            param.StringValue = destPath;
            etl.InternalParameters.Add(param);
            

            var runner = new EtlFileRunner(etl);
            var result = runner.Run();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(File.Exists(destPath), Is.True);
         }

        [Test]
        public void Execute_ExistingSamplePackageWithParameterWithInvalidValue_FailureWithMessage()
        {
            var destPath = DiskOnFile.GetDirectoryPath() + @"\/.txt";
            if (File.Exists(destPath))
                File.Delete(destPath);

            var etl = new EtlXml();
            etl.Path = @"Etl\";
            etl.Name = "Sample.dtsx";
            etl.Password = "p@ssw0rd";
            var param = new EtlParameterXml();
            param.Name = "DestinationPath";
            param.StringValue = destPath;
            etl.InternalParameters.Add(param);

            var runner = new EtlFileRunner(etl);
            var result = runner.Run();

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.StringContaining("invalid characters"));
        }
    }
}
