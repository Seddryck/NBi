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
using NBi.Xml.Constraints.Comparer;
using NBi.NUnit.Execution;
using NUnitCtr = NUnit.Framework.Constraints;
using System;
using NBi.Xml.Items.Calculation;
using System.Collections.Generic;
using NBi.Core.Variable;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ResultSetAllRowsBuilderTest
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

            var ctrXml = new AllRowsXml();
            ctrXml.Predicate = new PredicateXml();
            ctrXml.Predicate.MoreThan = new MoreThanXml();
            ctrXml.Predicate.MoreThan.Value = "100";

            var builder = new ResultSetAllRowsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<AllRowsConstraint>());
            var allRows = ctr as AllRowsConstraint;
            Assert.That(allRows.Child, Is.Null);
        }

        [Test]
        public void GetConstraint_Build_HandleVariable()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new AllRowsXml();
            ctrXml.Predicate = new PredicateXml();
            ctrXml.Predicate.MoreThan = new MoreThanXml();
            ctrXml.Predicate.MoreThan.Value = "@year";

            var variables = new Dictionary<string, ITestVariable>()
            {
                {"year", new CSharpTestVariable("DateTime.Now.Year") }
            };

            var builder = new ResultSetAllRowsBuilder();
            builder.Setup(sutXml, ctrXml, null, variables);
            builder.Build();

            Assert.That(variables["year"].IsEvaluated, Is.True);
        }

        [Test]
        public void GetConstraint_BuildNotUsedVariable_DontEvaluateIt()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new AllRowsXml();
            ctrXml.Predicate = new PredicateXml();
            ctrXml.Predicate.MoreThan = new MoreThanXml();
            ctrXml.Predicate.MoreThan.Value = "@year";

            var variables = new Dictionary<string, ITestVariable>()
            {
                {"year", new CSharpTestVariable("DateTime.Now.Year") },
                {"NotUsed", new CSharpTestVariable("1978") }
            };

            var builder = new ResultSetAllRowsBuilder();
            builder.Setup(sutXml, ctrXml, null, variables);
            builder.Build();

            Assert.That(variables["NotUsed"].IsEvaluated, Is.False);
        }

    }
}
