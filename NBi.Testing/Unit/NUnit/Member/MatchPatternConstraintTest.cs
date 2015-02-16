using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Member
{
    public class MatchPatternConstraintTest
    {
        [Test]
        public void Matches_RegexCorrectlySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("800-555-5555");
            members.Add("212-666-1234");

            var matchPatternConstraint = new MatchPatternConstraint();
            matchPatternConstraint = matchPatternConstraint.Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$");

            //Method under test
            var res = matchPatternConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_RegexWronglySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("000-000-0000");
            members.Add("2126661234");

            var matchPatternConstraint = new MatchPatternConstraint();
            matchPatternConstraint = matchPatternConstraint.Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$");

            //Method under test
            var res = matchPatternConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void WriteTo_FailingAssertionForRegex_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var member1 = new NBi.Core.Analysis.Member.Member() { Caption = "217-487-1125" };
            var member2 = new NBi.Core.Analysis.Member.Member() { Caption = "000-000-0000" };
            var member3 = new NBi.Core.Analysis.Member.Member() { Caption = "444-222-3333" };
            var member4 = new NBi.Core.Analysis.Member.Member() { Caption = "4442223333" };
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);
            members.Add(member3);
            members.Add(member4);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var matchPatternConstraint = new MatchPatternConstraint() { MembersEngine = me };
            matchPatternConstraint = matchPatternConstraint.Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, matchPatternConstraint);
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
                                            .StringContaining("regex pattern").And
                                            .StringContaining(@"^[2-9]\d{2}-\d{3}-\d{4}$").And
                                            .StringContaining(@"But was:    2 elements").And
                                            .StringContaining(@"000-000-0000").And
                                            .Not.StringContaining(@"444-222-3333"));


        }

    }
}
