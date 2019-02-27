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
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using System.Collections.Generic;
using NBi.Core.Injection;
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

            var ctrXml = new RowCountXml(SettingsXml.Empty)
            {
                MoreThan = new MoreThanXml() { Value = "100"} 
            };

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<RowCountConstraint>());
            var rowCount = ctr as RowCountConstraint;
            Assert.That(rowCount.Differed.Resolve(), Is.InstanceOf<NUnitCtr.GreaterThanConstraint>());
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

            var ctrXml = new RowCountXml(SettingsXml.Empty)
            {
                Equal = new EqualXml
                {
                    Value = "50"
                },
                Filter = new FilterXml()
            };
            ctrXml.Filter.InternalAliases.Add(new AliasXml());
            ctrXml.Filter.Predication = new PredicationXml() { Predicate = new NullXml(), Operand = new ColumnNameIdentifier("myColumn") };

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<RowCountFilterConstraint>());
            var rowCount = ctr as RowCountFilterConstraint;
            Assert.That(rowCount.Differed.Resolve(), Is.InstanceOf<NUnitCtr.EqualConstraint>());
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

            var ctrXml = new RowCountXml(SettingsXml.Empty)
            {
                Equal = new EqualXml() { Value = "50.4%" },
                Filter = new FilterXml()
                {
                    InternalAliases = new List<AliasXml>() { new AliasXml()},
                    Predication = new PredicationXml() { Predicate = new NullXml(), Operand = new ColumnNameIdentifier("myColumn") }
                }
            };

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<RowCountFilterPercentageConstraint>());
            var rowCount = ctr as RowCountFilterPercentageConstraint;
            Assert.That(rowCount.Differed.Resolve(), Is.InstanceOf<NUnitCtr.EqualConstraint>());
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

            var ctrXml = new RowCountXml(SettingsXml.Empty)
            {
                MoreThan = new MoreThanXml() { Value = "10" }
            };

            var builder = new ResultSetRowCountBuilder();
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

            var ctrXml = new RowCountXml(SettingsXml.Empty)
            {
                MoreThan = new MoreThanXml() { Value = "10" }
            };

            var builder = new ResultSetRowCountBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }

    }
}
