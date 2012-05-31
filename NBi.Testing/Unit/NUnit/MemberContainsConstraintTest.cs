#region Using directives
using NBi.Core.Analysis.Member;
using NUnit.Framework;

#endregion

namespace NBi.Testing.Unit.NUnit
{
    [TestFixture]
    public class MemberContainsConstraintTest
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
        public void Matches_OneCaptionContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new Member("[Hierarchy].[First member]","First member",1,0));
            members.Add(new Member("[Hierarchy].[Second member]","Second member",2,0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint();
            containsConstraint.Caption("First member");
            
            //Call the method to test
            var res = containsConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_OneCaptionNotContainedInMembers_Failure()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint();
            containsConstraint.Caption("Third member");

            //Call the method to test
            var res = containsConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_TwoCaptionsBothContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint();
            containsConstraint.Caption("First member").Caption("Second member");

            //Call the method to test
            var res = containsConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_TwoCaptionsOneOfThemIsNotContainedInMembers_Failure()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint();
            containsConstraint.Caption("Third member").Caption("Second member");

            //Call the method to test
            var res = containsConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

    }
}
