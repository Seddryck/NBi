using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Member
{
    [TestFixture]
    public class EquivalentToConstraintTest
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

            var equivalentConstraint = new EquivalentToConstraint(exp) { MembersEngine = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, equivalentConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("equivalent to").And
                                            .StringContaining("Expected member 1").And
                                            .StringContaining("Expected member 2"));
        }

        [Test]
        public void Matches_OneCaptionContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var equivalentConstraint = new NBi.NUnit.Member.EquivalentToConstraint(new List<string>() { "First member", "Second member" });

            //Call the method to test
            var res = equivalentConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_OneCaptionContainedInMembersOneMore_Failure()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Third member]", "Third member", 3, 0));

            var equivalentConstraint = new NBi.NUnit.Member.EquivalentToConstraint(new List<string>() { "First member", "Second member" });

            //Call the method to test
            var res = equivalentConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_OneCaptionContainedInMembersOneLess_Failure()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));

            var equivalentConstraint = new NBi.NUnit.Member.EquivalentToConstraint(new List<string>() { "First member", "Second member" });

            //Call the method to test
            var res = equivalentConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }
        

       
    }
}
