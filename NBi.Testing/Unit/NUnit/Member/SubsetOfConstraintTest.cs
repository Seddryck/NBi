using System.Collections.Generic;
using System.Linq;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Member
{
    [TestFixture]
    public class SubsetOfConstraintTest
    {
        [Test]
        public void WriteTo_FailingAssertionForListOfLevels_TextContainsFewKeyInfo()
        {
            var exp = new string[] { "Expected member 1", "Expected member 2" };
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        "member-caption",
                        "perspective-name",
                        "dimension-caption", "hierarchy-caption", null);


            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meMock = new Mock<MembersAdomdEngine>();
            meMock.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meMock.Object;

            var containsConstraint = new SubsetOfConstraint(exp) { MembersEngine = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("set").And
                                            .StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("Expected member 1").And
                                            .StringContaining("Expected member 2"));
        }


        [Test]
        public void Matches_OneCaptionContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));

            var subsetOfConstraint = new NBi.NUnit.Member.SubsetOfConstraint(new List<string>() {"First member", "Second Member"});

            //Call the method to test
            var res = subsetOfConstraint.Matches(members);

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

            var subsetOfConstraint = new NBi.NUnit.Member.SubsetOfConstraint(new List<string>() { "First member" });

            //Call the method to test
            var res = subsetOfConstraint.Matches(members);

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

            var subsetOfConstraint = new NBi.NUnit.Member.SubsetOfConstraint(new List<string>() { "First member" });

            //Call the method to test
            subsetOfConstraint.Matches(members);
            subsetOfConstraint.WriteActualValueTo(writer);

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

            var subsetOfConstraint = new NBi.NUnit.Member.SubsetOfConstraint(new List<string>() { "First member" });

            //Call the method to test
            subsetOfConstraint.Matches(members);
            subsetOfConstraint.WriteActualValueTo(writer);

            //Test conclusion            
            mockWriter.Verify(wr => wr.WriteActualValue(It.IsAny<NBi.NUnit.Member.SubsetOfConstraint.NothingFoundMessage>()));
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

            var containConstraint = new NBi.NUnit.Member.SubsetOfConstraint(new List<string>() { "First member" });

            //Call the method to test
            containConstraint.Matches(members);
            containConstraint.WriteActualValueTo(writer);

            //Test conclusion 
            var shortList = members.Take(10);
            mockWriter.Verify(wr => wr.WriteActualValue(shortList));
            mockWriter.Verify(wr => wr.WriteActualValue(It.Is<string>(str => str.Contains("15") && str.Contains("other"))));
        }

       
       
    }
}
