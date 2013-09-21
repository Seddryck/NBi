#region Using directives
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Builder;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class StructureExistsBuilderTest
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
            var sutXml = new StructureXml();
            var item = new HierarchyXml();
            item.ConnectionString = "connectionString";
            item.Perspective = "perspective-name";
            item.Dimension = "dimension-caption";
            item.Caption = "hierarchy-caption";
            sutXml.Item = item;
            var ctrXml = new ExistsXml();

            var builder = new StructureExistsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ExistsConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectSystemUnderTest()
        {
            var sutXml = new StructureXml();
            var item = new PerspectiveXml();
            sutXml.Item = item;
            item.ConnectionString = "connectionString";
            item.Caption = "perspective";
            var ctrXml = new ExistsXml();

            var builder = new StructureExistsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        //[Test]
        //public void GetSystemUnderTest_BuildWithMeasureGroup_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new MeasureGroupXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Caption = "measure-group";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.MeasureGroups, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithMeasure_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new MeasureXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Caption = "measure";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Measures, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithMeasureAndDisplayFolder_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new MeasureXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.DisplayFolder = "display-folder";
        //    item.Caption = "measure";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Measures, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithMeasureAndMeasureGroup_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new MeasureXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.MeasureGroup = "measure-group";
        //    item.Caption = "measure";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Measures, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithDimension_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new DimensionXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Caption = "dimension";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Dimensions, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithHierarchy_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new HierarchyXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Dimension = "dimension";
        //    item.Caption = "hierarchy";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Hierarchies, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithHierarchyAndDisplayFolder_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new HierarchyXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Dimension = "dimension";
        //    item.DisplayFolder = "display-folder";
        //    item.Caption = "hierarchy";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Hierarchies, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithLevel_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new LevelXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Dimension = "dimension";
        //    item.Hierarchy = "hierarchy";
        //    item.Caption = "level";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Levels, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithTable_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new TableXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Caption = "table";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Tables, It.IsAny<List<IFilter>>()));
        //}

        //[Test]
        //public void GetSystemUnderTest_BuildWithColumn_CorrectCallToDiscoverFactory()
        //{
        //    var sutXml = new StructureXml();
        //    var item = new ColumnXml();
        //    sutXml.Item = item;
        //    item.ConnectionString = "connectionString";
        //    item.Perspective = "perspective";
        //    item.Table = "table";
        //    item.Caption = "column";
        //    var ctrXml = new ExistsXml();

        //    var discoFactoMockFactory = new Mock<DiscoveryRequestFactory>();
        //    discoFactoMockFactory.Setup(dfs =>
        //        dfs.Build(
        //            It.IsAny<string>(),
        //            It.IsAny<DiscoveryTarget>(),
        //            It.IsAny<List<IFilter>>()
        //            ))
        //            .Returns(new MetadataDiscoveryRequest());
        //    var discoFactoMock = discoFactoMockFactory.Object;

        //    var builder = new StructureExistsBuilder(discoFactoMock);
        //    builder.Setup(sutXml, ctrXml);
        //    builder.Build();
        //    var sut = builder.GetSystemUnderTest();

        //    Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        //    discoFactoMockFactory.Verify(dfm => dfm.Build("connectionString", DiscoveryTarget.Columns, It.IsAny<List<IFilter>>()));
        //}
    }
}
