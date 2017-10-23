using Moq;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Markdown;

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

            var containsConstraint = new ContainConstraint(exp) { MembersEngine = me };

            //Method under test
            containsConstraint.Matches(cmd);

            //Test conclusion            
            meMock.Verify(engine => engine.GetMembers(cmd), Times.Once());
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
        public void WriteActualValueTo_FailureExist_FailureRenderActualCalled()
        {
            //Stub the writer
            var stubWriter = new Mock<global::NUnit.Framework.Constraints.MessageWriter>();
            var writer = stubWriter.Object;

            //Mock the failure
            var mockedFailure = Mock.Of<IItemsMessageFormatter>(f => f.RenderActual() == "failure actual");

            //Buiding object used during test
            var containConstraint = new ContainConstraint("Third member");
            containConstraint.Failure = mockedFailure;

            //Call the method to test
            containConstraint.WriteActualValueTo(writer);

            //Test conclusion            
            Mock.Get(mockedFailure).Verify(f => f.RenderActual());
        }

        [Test]
        public void WriteDescriptionTo_FailureExist_FailureRenderExpectedCalled()
        {
            //Mock the writer
            var stubWriter = new Mock<global::NUnit.Framework.Constraints.MessageWriter>();
            var writer = stubWriter.Object;

            var mockedFailure = Mock.Of<IItemsMessageFormatter>(f => f.RenderExpected() == "failure actual");

            //Buiding object used during test
            var containConstraint = new ContainConstraint("Third member");
            containConstraint.Failure = mockedFailure;
            //Call the method to test
            
            containConstraint.WriteDescriptionTo(writer);

            //Test conclusion            
            Mock.Get(mockedFailure).Verify(f => f.RenderExpected());
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
