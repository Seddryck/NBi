#region Using directives
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NUnit.Framework;
using Items = NBi.Xml.Items;
using Systems = NBi.Xml.Systems;
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
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            var sutXml = sutXmlStubFactory.Object;

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
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            var sutXml = sutXmlStubFactory.Object;

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
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            var sutXml = sutXmlStubFactory.Object;

            var ctrXml = new EqualToXml(SettingsXml.Empty);
            ctrXml.Query = new Items.QueryXml() { InlineQuery = "query" };
            ctrXml.Tolerance = "10";

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            //Get the tolerance for the column with 1 (and not 0) to avoid to get the tolerance on a key.
            Assert.That(((EqualToConstraint)ctr).Engine.Settings.GetTolerance(1).ValueString, Is.EqualTo("10"));
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectDiscoveryCommand()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            var sutXml = sutXmlStubFactory.Object;

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
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            var sutXml = sutXmlStubFactory.Object;

            var ctrXml = new EqualToXml(true);
            ctrXml.ResultSet = new ResultSetXml();

            var builder = new ExecutionEqualToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EqualToConstraint>());
            Assert.That(((EqualToConstraint)ctr).IsParallelizeQueries(), Is.True);
        }

    }
}
