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
using NBi.Core.ResultSet;
using NBi.Core.Injection;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ResultSetSingleRowBuilderTest
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

            var ctrXml = new SingleRowXml
            {
                Predication = new PredicationXml()
                {
                    Predicate = new MoreThanXml() { Value = "100" },
                    Operand = "#0"
                }
            };

            var builder = new ResultSetSingleRowBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<SingleRowConstraint>());
            var singleRow = ctr as SingleRowConstraint;
            Assert.That(singleRow.Differed, Is.Null);
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

            var ctrXml = new SingleRowXml
            {
                Predication = new PredicationXml()
                {
                    Predicate = new MoreThanXml() { Value = "@year" },
                    Operand = "#0"
                }
            };

            var yearResolverMock = new Mock<ITestVariable>();
            yearResolverMock.Setup(x => x.GetValue()).Returns(2017);

            var variables = new Dictionary<string, ITestVariable>()
            {
                {"year", yearResolverMock.Object }
            };

            var builder = new ResultSetSingleRowBuilder();
            builder.Setup(sutXml, ctrXml, null, variables, new ServiceLocator());
            builder.Build();

            yearResolverMock.Verify(x => x.GetValue(), Times.Once);
        }

        [Test]
        public void GetConstraint_BuildWithVariables_DontEvaluateThem()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new SingleRowXml
            {
                Predication = new PredicationXml()
                {
                    Predicate = new MoreThanXml() { Value = "@year" },
                    Operand = "#0"
                }
            };

            var yearResolverMock = new Mock<ITestVariable>();
            yearResolverMock.Setup(x => x.GetValue()).Returns(2017);
            var notUsedResolverMock = new Mock<ITestVariable>();
            notUsedResolverMock.Setup(x => x.GetValue());

            var variables = new Dictionary<string, ITestVariable>()
            {
                {"year", yearResolverMock.Object },
                {"NotUsed", notUsedResolverMock.Object }
            };

            var builder = new ResultSetSingleRowBuilder();
            builder.Setup(sutXml, ctrXml, null, variables, new ServiceLocator());
            builder.Build();

            notUsedResolverMock.Verify(x => x.GetValue(), Times.Never);
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

            var ctrXml = new SingleRowXml
            {
                Predication = new PredicationXml()
                {
                    Predicate = new MoreThanXml() { Value = "100" },
                    Operand = "#0"
                }
            };

            var builder = new ResultSetSingleRowBuilder();
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
            sutXmlStub.Setup(s => s.File).Returns("myFile.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new SingleRowXml
            {
                Predication = new PredicationXml()
                {
                    Predicate = new MoreThanXml() { Value = "100" },
                    Operand = "#0"
                }
            };

            var builder = new ResultSetSingleRowBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }
    }
}
