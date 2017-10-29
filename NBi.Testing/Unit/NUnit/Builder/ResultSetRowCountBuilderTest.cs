#region Using directives
using System;
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NUnit.Framework;
using Items = NBi.Xml.Items;
using Systems = NBi.Xml.Systems;
using NBi.Xml.Constraints.Comparer;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Xml.Items.Calculation;
using NBi.Core.ResultSet.Loading;
using NBi.Core.ResultSet;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ResultSetRowCountBuilderTest
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

            var ctrXml = new RowCountXml(SettingsXml.Empty);
            ctrXml.MoreThan = new MoreThanXml();
            ctrXml.MoreThan.Value = "100";

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<RowCountConstraint>());
            var rowCount = ctr as RowCountConstraint;
            Assert.That(rowCount.Child, Is.InstanceOf<NUnitCtr.GreaterThanConstraint>());
        }

        [Test]
        public void GetConstraint_RowCountFiltered_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new RowCountXml(SettingsXml.Empty);
            ctrXml.Equal = new EqualXml();
            ctrXml.Equal.Value = "50";
            ctrXml.Filter = new FilterXml();
            ctrXml.Filter.InternalAliases.Add(new AliasXml());
            ctrXml.Filter.Predicate = new PredicateXml() { Null = new NullXml(), Name = "myColumn" };

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<RowCountFilterConstraint>());
            var rowCount = ctr as RowCountFilterConstraint;
            Assert.That(rowCount.Child, Is.InstanceOf<NUnitCtr.EqualConstraint>());
        }

        [Test]
        public void GetConstraint_PercentageForRowCount_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new RowCountXml(SettingsXml.Empty);
            ctrXml.Equal = new EqualXml();
            ctrXml.Equal.Value = "50.4%";
            ctrXml.Filter = new FilterXml();
            ctrXml.Filter.InternalAliases.Add(new AliasXml());
            ctrXml.Filter.Predicate = new PredicateXml() { Null = new NullXml(), Name = "myColumn" };

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<RowCountFilterPercentageConstraint>());
            var rowCount = ctr as RowCountFilterPercentageConstraint;
            Assert.That(rowCount.Child, Is.InstanceOf<NUnitCtr.EqualConstraint>());
        }

        [Test]
        public void GetConstraint_NonIntegerValueForRowCount_ThrowException()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new RowCountXml(SettingsXml.Empty);
            ctrXml.MoreThan = new MoreThanXml();
            ctrXml.MoreThan.Value = "Something";

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml);
            Assert.Throws<ArgumentException>(delegate{ builder.Build();});
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

            var builder = new ResultSetEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }

    }
}
