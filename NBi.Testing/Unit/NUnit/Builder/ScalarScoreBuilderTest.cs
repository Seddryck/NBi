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
using NBi.NUnit.Scoring;
using NBi.Core.Scalar.Resolver;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ScalarScoreBuilderTest
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
        public void GetConstraint_BuildWithScalar_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ScalarXml>();
            var queryXmlStubFactory = new Mock<QueryScalarXml>();
            queryXmlStubFactory.SetupGet(q => q.InlineQuery).Returns("select 0.78");
            sutXmlStubFactory.Setup(s => s.BaseItem).Returns(queryXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Query = queryXmlStubFactory.Object;

            var ctrXml = new ScoreXml() { Threshold = 0.75m };

            var builder = new ScalarScoreBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ScoreConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_ScalarXmlIsQuery_IScalarResolver()
        {
            var sutXmlStubFactory = new Mock<Systems.ScalarXml>();
            var queryXmlStubFactory = new Mock<QueryScalarXml>();
            queryXmlStubFactory.SetupGet(q => q.InlineQuery).Returns("select 0.78");
            sutXmlStubFactory.Setup(s => s.BaseItem).Returns(queryXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Query = queryXmlStubFactory.Object;

            var ctrXmlStubFactory = new Mock<ScoreXml>();
            ctrXmlStubFactory.SetupGet(i => i.Threshold).Returns(0.75m);
            var ctrXml = ctrXmlStubFactory.Object;

            var builder = new ScalarScoreBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IScalarResolver<decimal>>());
            Assert.That(sut, Is.InstanceOf<QueryScalarResolver<decimal>>());
        }
    }
}
