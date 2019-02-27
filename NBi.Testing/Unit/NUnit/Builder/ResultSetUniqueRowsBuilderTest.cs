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
    public class ResultSetUniqueRowsBuilderTest
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
        public void GetConstraint_UniqueRowsXml_UniqueRowsConstraint()
        {
            var sutXmlStubFactory = new Mock<Systems.ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new UniqueRowsXml();

            var builder = new ResultSetUniqueRowsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<UniqueRowsConstraint>());
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

            var ctrXml = new UniqueRowsXml();

            var builder = new ResultSetUniqueRowsBuilder();
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
            sutXmlStub.Setup(s => s.FilePath).Returns("myFile.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new UniqueRowsXml();
            
            var builder = new ResultSetUniqueRowsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }
    }
}
