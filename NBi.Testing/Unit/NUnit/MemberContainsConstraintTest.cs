#region Using directives
using System.Collections.Generic;
using System.Linq;
using Moq;
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
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint("First member");
            
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
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint("Third member");

            //Call the method to test
            var res = containsConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void WriteActualValueTo_OneCaptionNotContainedInLessThan15Members_DisplayAllMembers()
        {
            //Mock the writer
            var mockWriter = new Mock<global::NUnit.Framework.Constraints.MessageWriter>();
            var writer = mockWriter.Object;

            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint("Third member");

            //Call the method to test
            containsConstraint.Matches(members);
            containsConstraint.WriteActualValueTo(writer);

            //Test conclusion            
            mockWriter.Verify(wr => wr.WriteActualValue(members));
        }

        [Test]
        public void WriteActualValueTo_OneCaptionNotContainedInZeroMembers_DisplayNothingFoundMessage()
        {
            //Mock the writer
            var mockWriter = new Mock<global::NUnit.Framework.Constraints.MessageWriter>();
            var writer = mockWriter.Object;

            //Buiding object used during test
            var members = new MemberResult();
            
            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint("Third member");

            //Call the method to test
            containsConstraint.Matches(members);
            containsConstraint.WriteActualValueTo(writer);

            //Test conclusion            
            mockWriter.Verify(wr => wr.WriteActualValue(It.IsAny<NBi.NUnit.Member.ContainsConstraint.NothingFoundMessage>()));
        }

        [Test]
        public void WriteActualValueTo_OneCaptionNotContainedInMoreThan15Members_DisplayOnlyFirstMembers()
        {
            //Mock the writer
            var mockWriter = new Mock<global::NUnit.Framework.Constraints.MessageWriter>();
            var writer = mockWriter.Object;

            //Buiding object used during test
            var members = new MemberResult();
            for (int i = 0; i < 25; i++)
                members.Add(new NBi.Core.Analysis.Member.Member(string.Format("[Hierarchy].[member {0}]", i), string.Format("member {0}", i), i, 0));
            
            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint("Searched member");

            //Call the method to test
            containsConstraint.Matches(members);
            containsConstraint.WriteActualValueTo(writer);

            //Test conclusion 
            var shortList = members.Take(10);
            mockWriter.Verify(wr => wr.WriteActualValue(shortList));
            mockWriter.Verify(wr => wr.WriteActualValue(It.Is<string>(str => str.Contains("15") && str.Contains("other"))));
        }

        [Test]
        public void Matches_TwoCaptionsBothContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint(new string[] {"First member", "Second member"});

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
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containsConstraint = new NBi.NUnit.Member.ContainsConstraint(new string[] {"Third member", "Second member"});

            //Call the method to test
            var res = containsConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

    }
}
