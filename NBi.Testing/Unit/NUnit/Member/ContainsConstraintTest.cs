using Moq;
using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Member;
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
            var cmdMembers = DiscoveryFactory.BuildForMembers(
                        "connectionString",
                        "perspective",
                        "[dimension]");

            var cmd = (MembersDiscoveryCommand)cmdMembers;

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meMock = new Mock<IDiscoverMemberEngine>();
            meMock.Setup(engine => engine.Execute(cmd))
                .Returns(members);
            var me = meMock.Object;

            var containsConstraint = new ContainsConstraint(exp) { MemberEngine = me };

            //Method under test
            containsConstraint.Matches(cmd);
         
            //Test conclusion            
            meMock.Verify(engine => engine.Execute(cmd), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertion_TextContainsFewKeyInfo()
        {
            var exp = "Expected member";
            var cmdMembers = DiscoveryFactory.BuildForMembers(
                        "connectionString",
                        "perspective",
                        "[dimension]");

            var cmd = cmdMembers;

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<IDiscoverMemberEngine>();
            meStub.Setup(engine => engine.Execute(cmd))
                .Returns(members);
            var me = meStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { MemberEngine = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, containsConstraint);
            }
            catch (AssertionException ex)
            {
                 assertionText=ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining(cmd.PerspectiveName).And
                                            .StringContaining(cmd.Path).And
                                            .StringContaining("Expected member"));
        }


        

       
    }
}
