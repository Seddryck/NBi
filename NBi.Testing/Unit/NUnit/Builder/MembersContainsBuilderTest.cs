#region Using directives
using Moq;
using NBi.Core.Analysis.Discovery;
using NBi.NUnit.Builder;
using NBi.NUnit.Member;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class MembersContainsBuilderTest
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
        public void GetConstraint_Build_CorrectConstraint()
        {
            var sutXml = new MembersXml();
            var item = new HierarchyXml();
            sutXml.Item = item;
            var ctrXml = new ContainsXml();

            var discoFactoStubFactory = new Mock<DiscoveryFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryCommand());
            var discoFactoStub = discoFactoStubFactory.Object;

            var builder = new MembersContainsBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ContainsConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_ConnectionStringInDefault_CorrectlyInitialized()
        {
            var defXml = new DefaultXml();
            defXml.ConnectionString = "connectionString-default";
            
            var sutXml = new MembersXml();
            sutXml.Default = defXml;
            sutXml.ChildrenOf = "memberCaption";
            var item = new HierarchyXml();
            sutXml.Item = item;
            item.Perspective = "perspective";
            item.Dimension = "dimension";
            item.Caption = "hierarchy";
            var ctrXml = new ContainsXml();

            var discoFactoMockFactory = new Mock<DiscoveryFactory>();
            discoFactoMockFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryCommand());
            var discoFactoMock = discoFactoMockFactory.Object;

            var builder = new MembersContainsBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MembersDiscoveryCommand>());
            discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString-default", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null));
        }


        [Test]
        public void GetSystemUnderTest_BuildWithHierarchy_CorrectCallToDiscoverFactory()
        {
            var sutXml = new MembersXml();
            sutXml.ChildrenOf = "memberCaption";
            var item = new HierarchyXml();
            sutXml.Item = item;
            item.ConnectionString = "connectionString";
            item.Perspective = "perspective";
            item.Dimension = "dimension";
            item.Caption = "hierarchy";
            var ctrXml = new ContainsXml();

            var discoFactoMockFactory = new Mock<DiscoveryFactory>();
            discoFactoMockFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryCommand());
            var discoFactoMock = discoFactoMockFactory.Object;

            var builder = new MembersContainsBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MembersDiscoveryCommand>());
            discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", "memberCaption", "perspective", "dimension", "hierarchy", null));
        }

        [Test]
        public void GetSystemUnderTest_BuildWithLevel_CorrectCallToDiscoverFactory()
        {
            var sutXml = new MembersXml();
            sutXml.ChildrenOf = "memberCaption";
            var item = new LevelXml();
            sutXml.Item = item;
            item.ConnectionString = "connectionString";
            item.Perspective = "perspective";
            item.Dimension = "dimension";
            item.Hierarchy = "hierarchy";
            item.Caption = "level";
            var ctrXml = new ContainsXml();

            var discoFactoMockFactory = new Mock<DiscoveryFactory>();
            discoFactoMockFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryCommand());
            var discoFactoMock = discoFactoMockFactory.Object;

            var builder = new MembersContainsBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MembersDiscoveryCommand>());
            discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", "memberCaption", "perspective", "dimension", "hierarchy", "level"));
        }

        



    }
}
