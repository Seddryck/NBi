using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Member
{
    public class CountConstraintTest
    {
        [Test]
        public void Matches_ExactlyCorrectlySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.Exactly(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_ExactlyWronglySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.Exactly(1);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_MoreThanCorrectlySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.MoreThan(1);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_MoreThanWronglySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.MoreThan(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_LessThanCorrectlySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.LessThan(3);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_LessThanWronglySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.LessThan(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_LessThanAndMoreThanCorrectlySpecified_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.MoreThan(1);
            countConstraint.LessThan(3);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_LessThanAndMoreThanWronglySpecifiedForMoreThan_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.MoreThan(2);
            countConstraint.LessThan(3);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_LessThanAndMoreThanWronglySpecifiedForLessThan_Validated()
        {
            var members = new MemberResult();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new CountConstraint();
            countConstraint.MoreThan(1);
            countConstraint.LessThan(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void WriteTo_FailingAssertionForExactly_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var countConstraint = new CountConstraint() { CommandFactory = me };
            countConstraint.Exactly(10);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, countConstraint);
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
                                            .StringContaining("exactly").And
                                            .StringContaining("10"));

        }

        [Test]
        public void WriteTo_FailingAssertionForMoreThan_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var countConstraint = new CountConstraint() { CommandFactory = me };
            countConstraint.MoreThan(10);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, countConstraint);
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
                                            .StringContaining("more than").And
                                            .StringContaining("10"));

        }

        [Test]
        public void WriteTo_FailingAssertionForLessThan_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var countConstraint = new CountConstraint() { CommandFactory = me };
            countConstraint.LessThan(1);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, countConstraint);
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
                                            .StringContaining("less than").And
                                            .StringContaining("1"));

        }
        [Test]
        public void WriteTo_FailingAssertionForBetween_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                "member-caption",
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                null);

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<MembersAdomdEngine>();
            meStub.Setup(engine => engine.GetMembers(cmd))
                .Returns(members);
            var me = meStub.Object;

            var countConstraint = new CountConstraint() { CommandFactory = me };
            countConstraint = countConstraint.MoreThan(8);
            countConstraint = countConstraint.LessThan(12);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, countConstraint);
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
                                            .StringContaining("between").And
                                            .StringContaining("8").And
                                            .StringContaining("12"));

        }
    }
}
