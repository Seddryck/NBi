#region Using directives
using System;
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
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
            var ctrXml = new ContainXml();

            var discoFactoStubFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoStub = discoFactoStubFactory.Object;

            var builder = new MembersContainBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ContainConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_ConnectionStringInDefault_CorrectlyInitialized()
        {
            var sutXml = new MembersXml();
            sutXml.ChildrenOf = "memberCaption";
            var item = new HierarchyXml();
            sutXml.Item = item;
            item.Perspective = "perspective";
            item.Dimension = "dimension";
            item.Caption = "hierarchy";

            var defXml = new DefaultXml();
            defXml.ConnectionString = "connectionString-default";
            sutXml.Default = defXml;
            
            var ctrXml = new ContainXml();

            var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoMockFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoMock = discoFactoMockFactory.Object;

            var builder = new MembersContainBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MembersDiscoveryRequest>());
            discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString-default", It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<PatternValue>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null));
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
            var ctrXml = new ContainXml();
            ctrXml.Caption = "caption";

            var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoMockFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoMock = discoFactoMockFactory.Object;

            var builder = new MembersContainBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MembersDiscoveryRequest>());
            discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", "memberCaption", It.IsAny<List<string>>(), It.IsAny<List<PatternValue>>(), "perspective", "dimension", "hierarchy", null));
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
            var ctrXml = new ContainXml();
            ctrXml.Items.Add("caption 1");
            ctrXml.Items.Add("caption 2");

            var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoMockFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoMock = discoFactoMockFactory.Object;

            var builder = new MembersContainBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MembersDiscoveryRequest>());
            discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", "memberCaption", It.IsAny<List<string>>(), It.IsAny<List<PatternValue>>(), "perspective", "dimension", "hierarchy", "level"));
        }

        [Test]
        public void GetSystemUnderTest_BuildWithDimension_Failure()
        {
            var sutXml = new MembersXml();
            sutXml.ChildrenOf = "memberCaption";
            var item = new DimensionXml();
            sutXml.Item = item;
            item.ConnectionString = "connectionString";
            item.Perspective = "perspective";
            item.Caption = "dimension";
            var ctrXml = new ContainXml();

            var discoFactoStubFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoMock = discoFactoStubFactory.Object;

            var builder = new MembersContainBuilder(discoFactoMock);
            builder.Setup(sutXml, ctrXml);

            Assert.Throws<ArgumentOutOfRangeException>(delegate { builder.Build(); });
        }



    }
}
