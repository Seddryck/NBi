using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;
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
            var mc = new DiscoverCommand(string.Empty) { Path = "[dimension].[hierarchy]"
                                                , Perspective = "perspective" };

            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meMock = new Mock<IDiscoverMemberEngine>();
            meMock.Setup(engine => engine.Execute(mc))
                .Returns(members);
            var me = meMock.Object;

            var containsConstraint = new ContainsConstraint(exp) { MemberEngine = me };

            //Method under test
            containsConstraint.Matches(mc);
         
            //Test conclusion            
            meMock.Verify(engine => engine.Execute(mc), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertion_TextContainsFewKeyInfo()
        {
            var exp = "Expected member";
            var mc = new DiscoverCommand(string.Empty)
                                {
                                    Path = "[dimension].[hierarchy]",
                                    Perspective = "perspective"
                                };


            var memberStub = new Mock<NBi.Core.Analysis.Member.Member>();
            var member1 = memberStub.Object;
            var member2 = memberStub.Object;
            var members = new MemberResult();
            members.Add(member1);
            members.Add(member2);

            var meStub = new Mock<IDiscoverMemberEngine>();
            meStub.Setup(engine => engine.Execute(mc))
                .Returns(members);
            var me = meStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { MemberEngine = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(mc, containsConstraint);
            }
            catch (AssertionException ex)
            {
                 assertionText=ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining(mc.Perspective).And
                                            .StringContaining(mc.Path).And
                                            .StringContaining("hierarchy").And
                                            .StringContaining(exp));
        }


        

       
    }
}
