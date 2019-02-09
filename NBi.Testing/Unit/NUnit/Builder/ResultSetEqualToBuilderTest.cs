#region Using directives
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.ResultSetComparison;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NUnit.Framework;
using Items = NBi.Xml.Items;
using Systems = NBi.Xml.Systems;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet.Equivalence;
using System.Data.SqlClient;
using NBi.Core.Injection;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ResultSetEqualToBuilderTest
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

            var ctrXml = new EqualToXml(SettingsXml.Empty) { ResultSet = new ResultSetXml() };

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
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

            var ctrXmlStubFactory = new Mock<EqualToXml>();
            ctrXmlStubFactory.Setup(i => i.GetCommand()).Returns(new SqlCommand());
            ctrXmlStubFactory.SetupGet(i => i.BaseItem).Returns(new QueryXml() { InlineQuery="query", ConnectionString = "connStr" });
            ctrXmlStubFactory.SetupGet(i => i.Settings).Returns(SettingsXml.Empty);
            var ctrXml = ctrXmlStubFactory.Object;

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
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

            var ctrXmlStubFactory = new Mock<EqualToXml>();
            ctrXmlStubFactory.Setup(i => i.GetCommand()).Returns(new SqlCommand());
            ctrXmlStubFactory.SetupGet(i => i.BaseItem).Returns(new QueryXml() { InlineQuery = "query", ConnectionString = "connStr" });
            ctrXmlStubFactory.SetupGet(i => i.Settings).Returns(SettingsXml.Empty);
            ctrXmlStubFactory.SetupGet(i => i.Tolerance).Returns("10");
            var ctrXml = ctrXmlStubFactory.Object;

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            //Get the tolerance for the column with 1 (and not 0) to avoid to get the tolerance on a key.
            var settings = ((EqualToConstraint)ctr).Engine.Settings as SettingsOrdinalResultSet;
            Assert.That(settings.GetTolerance(1).ValueString, Is.EqualTo("10"));
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

            var ctrXml = new EqualToXml(true) { ResultSet = new ResultSetXml() };

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            Assert.That(((EqualToConstraint)ctr).IsParallelizeQueries(), Is.True);
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

            var ctrXmlStubFactory = new Mock<EqualToXml>();
            ctrXmlStubFactory.Setup(i => i.GetCommand()).Returns(new SqlCommand());
            ctrXmlStubFactory.SetupGet(i => i.BaseItem).Returns(new QueryXml() { InlineQuery = "select top(1) * from Table;", ConnectionString = "connStr" });
            ctrXmlStubFactory.SetupGet(i => i.Settings).Returns(SettingsXml.Empty);
            ctrXmlStubFactory.SetupGet(i => i.Behavior).Returns(EqualToXml.ComparisonBehavior.SingleRow);
            var ctrXml = ctrXmlStubFactory.Object;

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            Assert.That(((EqualToConstraint)ctr).Engine, Is.InstanceOf<SingleRowEquivaler>());
            Assert.That(((EqualToConstraint)ctr).Engine.Settings, Is.InstanceOf<SettingsSingleRowResultSet>());
        }

        [Test]
        public void GetSystemUnderTest_ExecutionXml_IResultSetService()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXmlStubFactory = new Mock<EqualToXml>();
            ctrXmlStubFactory.Setup(i => i.GetCommand()).Returns(new SqlCommand());
            ctrXmlStubFactory.SetupGet(i => i.BaseItem).Returns(new QueryXml() { InlineQuery = "select * from Table;", ConnectionString = "connStr" });
            ctrXmlStubFactory.SetupGet(i => i.Settings).Returns(SettingsXml.Empty);
            var ctrXml = ctrXmlStubFactory.Object;

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }

        [Test]
        public void GetSystemUnderTest_ResultSetSystemXml_IResultSetService()
        {
            var sutXmlStub = new Mock<Systems.ResultSetSystemXml>();
            sutXmlStub.Setup(s => s.File.Path).Returns("myFile.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXmlStubFactory = new Mock<EqualToXml>();
            ctrXmlStubFactory.Setup(i => i.GetCommand()).Returns(new SqlCommand());
            ctrXmlStubFactory.SetupGet(i => i.BaseItem).Returns(new QueryXml() { InlineQuery = "select * from Table;", ConnectionString = "connStr"});
            ctrXmlStubFactory.SetupGet(i => i.Settings).Returns(SettingsXml.Empty);
            var ctrXml = ctrXmlStubFactory.Object;

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }
    }
}
