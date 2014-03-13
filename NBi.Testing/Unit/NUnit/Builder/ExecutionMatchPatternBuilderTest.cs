#region Using directives
using System.Collections.Generic;
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ExecutionMatchPatternBuilderTest
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
            var sutXmlStubFactory = new Mock<ExecutionXml>();
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item.GetParameters()).Returns(new List<QueryParameterXml>());
            sutXmlStubFactory.Setup(s => s.Item.GetVariables()).Returns(new List<QueryTemplateVariableXml>());
            var sutXml = sutXmlStubFactory.Object;

            var ctrXml = new MatchPatternXml();

            var builder = new ExecutionMatchPatternBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<MatchPatternConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectIDbCommand()
        {
            var sutXmlStubFactory = new Mock<ExecutionXml>();
            sutXmlStubFactory.Setup(s => s.Item.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item.GetParameters()).Returns(new List<QueryParameterXml>());
            sutXmlStubFactory.Setup(s => s.Item.GetVariables()).Returns(new List<QueryTemplateVariableXml>());
            
            var sutXml = sutXmlStubFactory.Object;

            var ctrXml = new MatchPatternXml();

            var builder = new ExecutionMatchPatternBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<IDbCommand>());
        }

    }
}
