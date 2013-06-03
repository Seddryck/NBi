using Moq;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Member
{
    [TestFixture]
    public class ContainsConstraintTest
    {
        [Test]
        public void Matches_GivenMemberCommand_EngineCalledOnceWithParametersComingFromMemberCommand()
        {
            var exp = "Expected member";
            var cmd = new DiscoveryRequestFactory().Build(
                "ConnectionString",
                "member-caption",
                "perspective",
                "dimension",
                null,
                null);

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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = me };

            //Method under test
            containsConstraint.Matches(cmd);

            //Test conclusion            
            meMock.Verify(engine => engine.GetMembers(cmd), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForChild_TextContainsFewKeyInfo()
        {
            var exp = "Expected member";
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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = me };

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
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("child").And
                                            .StringContaining("Expected member"));

        }

        [Test]
        public void WriteTo_FailingAssertionForMember_TextContainsFewKeyInfo()
        {
            var exp = "Expected member";
            var cmd = new DiscoveryRequestFactory().Build(
                "connectionString",
                string.Empty,
                "perspective-name",
                "dimension-caption",
                "hierarchy-caption",
                "level-caption");

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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = me };

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
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("level-caption").And
                                            .StringContaining("member").And
                                            .StringContaining("Expected member"));

        }



        [Test]
        public void Matches_OneCaptionContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

            var containConstraint = new NBi.NUnit.Member.ContainConstraint("First member");

            //Call the method to test
            var res = containConstraint.Matches(members);

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

            var containConstraint = new NBi.NUnit.Member.ContainConstraint("Third member");

            //Call the method to test
            var res = containConstraint.Matches(members);

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

            var containConstraint = new NBi.NUnit.Member.ContainConstraint("Third member");

            //Call the method to test
            containConstraint.Matches(members);
            containConstraint.WriteActualValueTo(writer);

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

            var containConstraint = new NBi.NUnit.Member.ContainConstraint("Third member");

            //Call the method to test
            containConstraint.Matches(members);
            containConstraint.WriteActualValueTo(writer);

            //Test conclusion            
            mockWriter.Verify(wr => wr.WriteActualValue(It.IsAny<NBi.NUnit.Member.ContainConstraint.NothingFoundMessage>()));
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

            var containConstraint = new NBi.NUnit.Member.ContainConstraint("Searched member");

            //Call the method to test
            containConstraint.Matches(members);
            containConstraint.WriteActualValueTo(writer);

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

            var containConstraint = new NBi.NUnit.Member.ContainConstraint(new string[] { "First member", "Second member" });

            //Call the method to test
            var res = containConstraint.Matches(members);

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

            var containConstraint = new NBi.NUnit.Member.ContainConstraint(new string[] { "Third member", "Second member" });

            //Call the method to test
            var res = containConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

       
    }
}
