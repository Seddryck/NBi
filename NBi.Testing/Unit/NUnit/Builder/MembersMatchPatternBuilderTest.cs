#region Using directives
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Builder;
using NBi.NUnit.Member;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class MembersMatchPatternBuilderTest
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
            var sutXml = new MembersXml();
            var item = new HierarchyXml();
            sutXml.Item = item;
            var ctrXml = new MatchPatternXml();

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

            var builder = new MembersMatchPatternBuilder(discoFactoStub);
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<MatchPatternConstraint>());
        }
    }
}
