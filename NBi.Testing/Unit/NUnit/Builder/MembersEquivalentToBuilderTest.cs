#region Using directives
using System;
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.Core.Members.Predefined;
using NBi.NUnit.Builder;
using NBi.NUnit.Member;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Items.Ranges;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class MembersEquivalentToBuilderTest
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
        public void GetConstraint_BuildWithQuery_CorrectConstraint()
        {
            var sutXml = new MembersXml();
            var item = new HierarchyXml();
            sutXml.Item = item;
            var ctrXml = new EquivalentToXml();
            ctrXml.Query = new QueryXml();
            ctrXml.Query.ConnectionString = "Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly";
            ctrXml.Query.InlineQuery = "select * from one-column-table";

            var discoFactoStubFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoStub = discoFactoStubFactory.Object;

            var builder = new MembersEquivalentToBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EquivalentToConstraint>());
        }

        [Test]
        public void GetConstraint_BuildWithItems_CorrectConstraint()
        {
            var sutXml = new MembersXml();
            var item = new HierarchyXml();
            sutXml.Item = item;
            var ctrXml = new EquivalentToXml();
            ctrXml.Items = new List<string>() { "Hello", "World" };

            var discoFactoStubFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoStub = discoFactoStubFactory.Object;

            var builder = new MembersEquivalentToBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EquivalentToConstraint>());
        }

        [Test]
        public void GetConstraint_BuildWithPredefinedItems_CorrectConstraint()
        {
            var sutXml = new MembersXml();
            var item = new HierarchyXml();
            sutXml.Item = item;
            var ctrXml = new EquivalentToXml();
            ctrXml.PredefinedItems = new PredefinedItemsXml() { Type=PredefinedMembers.DaysOfWeek, Language = "en" };

            var discoFactoStubFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoStub = discoFactoStubFactory.Object;

            var builder = new MembersEquivalentToBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EquivalentToConstraint>());
        }

        [Test]
        public void GetConstraint_BuildWithRange_CorrectConstraint()
        {
            var sutXml = new MembersXml();
            var item = new HierarchyXml();
            sutXml.Item = item;
            var ctrXml = new EquivalentToXml();
            ctrXml.Range = new IntegerRangeXml() { Start = 1, End = 10, Step = 2 };

            var discoFactoStubFactory = new Mock<DiscoveryRequestFactory>();
            discoFactoStubFactory.Setup(dfs =>
                dfs.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<PatternValue>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .Returns(new MembersDiscoveryRequest());
            var discoFactoStub = discoFactoStubFactory.Object;

            var builder = new MembersEquivalentToBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EquivalentToConstraint>());
        }

    }
}
