#region Using directives
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NUnit.Framework;
using Items = NBi.Xml.Items;
using Systems = NBi.Xml.Systems;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ExecutionEqualsToBuilderTest
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
        public void GetConstraint_BuildWithResultSet_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new EqualToXml(SettingsXml.Empty);
            ctrXml.ResultSet = new ResultSetXml();

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
        }

        [Test]
        public void GetConstraint_BuildWithQuery_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new EqualToXml(SettingsXml.Empty);
            ctrXml.Query = new Items.QueryXml() {InlineQuery = "query"};

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
        }

        [Test]
        public void GetConstraint_BuildWithTolerance_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new EqualToXml(SettingsXml.Empty);
            ctrXml.Query = new Items.QueryXml() { InlineQuery = "query" };
            ctrXml.Tolerance = "10";

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            //Get the tolerance for the column with 1 (and not 0) to avoid to get the tolerance on a key.
            var settings = ((EqualToConstraint)ctr).Engine.Settings as SettingsResultSetComparisonByIndex;
            Assert.That(settings.GetTolerance(1).ValueString, Is.EqualTo("10"));
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectDiscoveryCommand()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new EqualToXml(SettingsXml.Empty);
            ctrXml.Query = new Items.QueryXml() { InlineQuery = "query" };

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<IDbCommand>());
        }

        [Test]
        public void GetConstraint_BuildWithParallel_CorrectConstraint()
        {           
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new EqualToXml(true);
            ctrXml.ResultSet = new ResultSetXml();

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            Assert.That(((EqualToConstraint)ctr).IsParallelizeQueries(), Is.True);
        }

        [Test]
        public void GetConstraint_Transformer_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var transformation = Mock.Of<TransformationXml>
                (
                    t => t.Language == LanguageType.CSharp
                    && t.OriginalType == ColumnType.Text
                    && t.Code == "value.Substring(2)"
                );

            var columnDef = Mock.Of<ColumnDefinitionXml>
                (
                    c => c.Index == 1
                    && c.Role == ColumnRole.Value
                    && c.Type == ColumnType.Text
                    && c.TransformationInner == transformation
                );

            var ctrXml = new EqualToXml(true);
            ctrXml.Query = new QueryXml();
            ctrXml.Query.InlineQuery="select * from Table;";
            Assert.That(ctrXml.ColumnsDef.Count, Is.EqualTo(0));
            ctrXml.columnsDef.Add(columnDef);
            Assert.That(ctrXml.ColumnsDef.Count, Is.EqualTo(1));

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            Assert.That(((EqualToConstraint)ctr).TransformationProvider, Is.Not.Null);
        }

        [Test]
        public void GetConstraint_SingleRow_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var columnDef = Mock.Of<ColumnDefinitionXml>
                (
                    c => c.Index == 1
                    && c.Role == ColumnRole.Value
                    && c.Type == ColumnType.Text
                );

            var ctrXml = new EqualToXml(true);
            ctrXml.Behavior = EqualToXml.ComparisonBehavior.SingleRow;
            ctrXml.Query = new QueryXml();
            ctrXml.Query.InlineQuery = "select * from Table;";
            
            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            Assert.That(((EqualToConstraint)ctr).Engine, Is.InstanceOf<SingleRowComparer>());
            Assert.That(((EqualToConstraint)ctr).Engine.Settings, Is.InstanceOf<SettingsSingleRowComparison>());
        }
    }
}
