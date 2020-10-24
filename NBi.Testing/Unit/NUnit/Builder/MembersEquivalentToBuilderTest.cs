#region Using directives
using System;
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.Core.Injection;
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
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
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
            var sutXml = new MembersXml() { Item = new HierarchyXml() { ConnectionString = "connStr" } };
            var ctrXml = new EquivalentToXml()
            {
                Query = new QueryXml()
                {
                    ConnectionString = "Data Source=(local)\\SQL2017;Initial Catalog=AdventureWorksDW2017;User Id=sa;password=Password12!",
                    InlineQuery = "select * from one-column-table"
                }
            };
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
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EquivalentToConstraint>());
        }

        [Test]
        public void GetConstraint_BuildWithItems_CorrectConstraint()
        {
            var sutXml = new MembersXml() { Item = new HierarchyXml() { ConnectionString = "connStr" } };
            var ctrXml = new EquivalentToXml() { Items = new List<string>() { "Hello", "World" } };

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
            var sutXml = new MembersXml() { Item = new HierarchyXml() { ConnectionString = "connStr" } };
            var ctrXml = new EquivalentToXml() { PredefinedItems = new PredefinedItemsXml() { Type = PredefinedMembers.DaysOfWeek, Language = "en" } };

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
            var sutXml = new MembersXml() { Item = new HierarchyXml() { ConnectionString = "connStr" } };
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
