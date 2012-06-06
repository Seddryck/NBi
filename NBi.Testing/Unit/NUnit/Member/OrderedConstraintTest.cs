#region Using directives
using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
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
            var members = new List<string>();
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
            var members = new List<string>();
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
        public void Matches_ReverseCorrectlyOrdered_Validated()
        {
            var members = new List<string>();
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
            var members = new List<string>();
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
            var members = new List<string>();
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
            var members = new List<string>();
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
            var members = new List<string>();
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
            var members = new List<string>();
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
        public void Matches_GivenDiscoverCommand_EngineCalledOnceWithParametersComingFromDiscoverCommand()
        {
            var disco = new DiscoverCommand(string.Empty)
            {
                Path = "[dimension].[hierarchy]"
                ,
                Perspective = "perspective"
            };

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meMock = new Mock<IDiscoverMemberEngine>();
            meMock.Setup(engine => engine.Execute(disco))
                .Returns(members);
            var me = meMock.Object;

            var orderedConstraint = new OrderedConstraint() { MemberEngine = me };

            //Method under test
            orderedConstraint.Matches(disco);

            //Test conclusion            
            meMock.Verify(engine => engine.Execute(disco), Times.Once());
        }
    }
}
