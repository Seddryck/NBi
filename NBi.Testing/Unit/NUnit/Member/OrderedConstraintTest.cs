#region Using directives
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Member
{
    [TestFixture]
    public class OrderedConstraintTest
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
        public void Matches_AlphabeticallyCorrectlyOrdered_Validated()
        {
            var members = new MemberResult();
            members.Add("A member");
            members.Add("B member");
            members.Add("C member");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Alphabetical;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_AlphabeticallyNotCorrectlyOrdered_Failed()
        {
            var members = new MemberResult();
            members.Add("A member");
            members.Add("C member");
            members.Add("B member");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Alphabetical;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void WriteTo_FailingAssertionForAlphabetic_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var member1Stub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = member1Stub.Object;
            member1.Caption = "Z";
            var member2Stub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member2 = member2Stub.Object;
            member2.Caption = "A";
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var orderedConstraint = new OrderedConstraint() { MembersEngine = me };
            orderedConstraint = orderedConstraint.Alphabetical;

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, orderedConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }
            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("member-caption").And
                                            .StringContaining("children").And
                                            .StringContaining("alphabetic"));

        }

        [Test]
        public void Matches_ReverseCorrectlyOrdered_Validated()
        {
            var members = new MemberResult();
            members.Add("C member");
            members.Add("B member");
            members.Add("A member");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Descending;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_ReverseNotCorrectlyOrdered_Failed()
        {
            var members = new MemberResult();
            members.Add("A member");
            members.Add("C member");
            members.Add("B member");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Descending;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_ChronologicalCorrectlyOrdered_Validated()
        {
            var members = new MemberResult();
            members.Add("20/10/2010");
            members.Add("5/2/2011");
            members.Add("3/10/2011");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Chronological;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_ChronologicalNotCorrectlyOrdered_Failed()
        {
            var members = new MemberResult();
            members.Add("20/10/2010");
            members.Add("3/10/2011");
            members.Add("5/2/2011");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Chronological;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_NumericalCorrectlyOrdered_Validated()
        {
            var members = new MemberResult();
            members.Add("1");
            members.Add("5");
            members.Add("100");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Numerical;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_NumericalNotCorrectlyOrdered_Failed()
        {
            var members = new MemberResult();
            members.Add("1");
            members.Add("100");
            members.Add("5");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Numerical;

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_SpecificCorrectlyOrdered_Succeed()
        {
            var members = new MemberResult();
            members.Add("Leopold");
            members.Add("Albert");
            members.Add("Baudoin");

            var ordspec = new List<object>();
            ordspec.Add("Leopold");
            ordspec.Add("Albert");
            ordspec.Add("Baudoin");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Specific(ordspec);

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_SpecificNotCorrectlyOrdered_Failed()
        {
            var members = new MemberResult();
            members.Add("Leopold");
            members.Add("Baudoin");
            members.Add("Albert");

            var ordspec = new List<object>();
            ordspec.Add("Leopold");
            ordspec.Add("Albert");
            ordspec.Add("Baudoin");

            var orderedConstraint = new OrderedConstraint();
            orderedConstraint = orderedConstraint.Specific(ordspec);

            //Method under test
            var res = orderedConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void WriteTo_FailingAssertionForSpecific_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var member1Stub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = member1Stub.Object;
            member1.Caption="A";
            var member2Stub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member2 = member2Stub.Object;
            member2.Caption="B";
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var orderedConstraint = new OrderedConstraint() { MembersEngine = me };
            orderedConstraint.Specific(new List<object>() { "B", "A" });

            //var assertionText = orderedConstraint.CreatePredicate();
            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, orderedConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }
            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("member-caption").And
                                            .StringContaining("children").And
                                            .StringContaining("specifically"));

        }

        [Test]
        public void Matches_GivenDiscoverCommand_EngineCalledOnceWithParametersComingFromDiscoveryCommand()
        {
            var disco = new DiscoveryRequestFactory().Build(
                "ConnectionString",
                "member-caption",
                "perspective",
                "dimension",
                "hierarchy",
                null);

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meMock = new Mock<MembersAdomdEngine>();
            meMock.Setup(engine => engine.GetMembers(disco))
                .Returns(members);
            var me = meMock.Object;

            var orderedConstraint = new OrderedConstraint() { MembersEngine = me };

            //Method under test
            orderedConstraint.Matches(disco);

            //Test conclusion            
            meMock.Verify(engine => engine.GetMembers(disco), Times.Once());
        }
    }
}
