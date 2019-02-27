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
using NBi.NUnit.ResultSetComparison;
using NBi.Xml.Items.ResultSet.Lookup;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ResultSetLookupExistsBuilderTest
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
        public void GetConstraint_LookupExistsXml_LookupExistsConstraint()
        {
            var sutXmlStub = new Mock<Systems.ResultSetSystemXml>();
            sutXmlStub.Setup(s => s.File.Path).Returns("myCandidate.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new LookupExistsXml();
            var rsXmlStub = new Mock<Systems.ResultSetSystemXml>();
            rsXmlStub.Setup(s => s.File.Path).Returns("myReference.csv");
            ctrXml.ResultSet = rsXmlStub.Object;
            ctrXml.Join = new JoinXml();

            var builder = new ResultSetLookupExistsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<LookupExistsConstraint>());
        }

        [Test]
        public void GetConstraint_LookupExistsXml_LookupReverseExistsConstraint()
        {
            var sutXmlStub = new Mock<Systems.ResultSetSystemXml>();
            sutXmlStub.Setup(s => s.File.Path).Returns("myCandidate.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new LookupExistsXml() { IsReversed = true };
            var rsXmlStub = new Mock<Systems.ResultSetSystemXml>();
            rsXmlStub.Setup(s => s.File.Path).Returns("myReference.csv");
            ctrXml.ResultSet = rsXmlStub.Object;
            ctrXml.Join = new JoinXml();

            var builder = new ResultSetLookupExistsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<LookupReverseExistsConstraint>());
        }


        [Test]
        public void GetSystemUnderTest_ResultSetSystemXml_IResultSetService()
        {
            var sutXmlStub = new Mock<Systems.ResultSetSystemXml>();
            sutXmlStub.Setup(s => s.File.Path).Returns("myFile.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new LookupExistsXml();
            var parentXmlStub = new Mock<Systems.ResultSetSystemXml>();
            parentXmlStub.Setup(s => s.File.Path).Returns("myParent.csv");
            ctrXml.ResultSet = parentXmlStub.Object;
            ctrXml.Join = new JoinXml();

            var builder = new ResultSetLookupExistsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }
    }
}
