using System;
using System.Linq;
using Moq;
using NBi.Core.Etl;
using NBi.Core.Etl.IntegrationService;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Etl
{
    [TestFixture]
    public class SsisEtlRunnerFactoryTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

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
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void Get_FilePackage_ReturnsEtlFileRunner()
        {
            var etl = Mock.Of<IEtl>(e => e.Path=="\\Etl\\" && e.Path=="mySample.dtsx");
            
            var factory = new SsisEtlRunnerFactory();

            Assert.IsInstanceOf<EtlFileRunner>(factory.Get(etl));
        }


        [Test]
        public void Get_SqlServerAuthenticationPackage_ReturnsEtlFileRunner()
        {
            var etl = Mock.Of<IEtl>(e => e.Path == "/Etl/" && e.Name == "mySample.dtsx" && e.Server=="." && e.UserName=="sa" && e.Password=="p@ssw0rd");

            var factory = new SsisEtlRunnerFactory();

            Assert.IsInstanceOf<EtlDtsSqlServerRunner>(factory.Get(etl));
        }


        [Test]
        public void Get_WindowsAuthenticationPackage_ReturnsEtlFileRunner()
        {
            var etl = Mock.Of<IEtl>(e => e.Path == "/Etl/" && e.Name == "mySample.dtsx" && e.Server == ".");

            var factory = new SsisEtlRunnerFactory();

            Assert.IsInstanceOf<EtlDtsWindowsRunner>(factory.Get(etl));
        }

        [Test]
        public void Get_CatalogPackage_ReturnsEtlCatalogRunner()
        {
            var etl = Mock.Of<IEtl>(e => e.Catalog == "Etl" && e.Folder == "folder" && e.Project == "project" && e.Name == "mySample" && e.Server == ".");

            var factory = new SsisEtlRunnerFactory();

            Assert.IsInstanceOf<EtlCatalogRunner>(factory.Get(etl));
        }
    }
}
