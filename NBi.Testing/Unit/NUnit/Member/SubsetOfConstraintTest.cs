using System.Collections.Generic;
using System.Linq;
using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;
using NBi.Framework.FailureMessage.Markdown;

namespace NBi.Testing.Unit.NUnit.Member
{
    [TestFixture]
    public class SubsetOfConstraintTest
    {
        [Test]
        public void Matches_OneCaptionContainedInMembers_Validated()
        {
            //Buiding object used during test
            var members = new MemberResult();
            members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));

            var subsetOfConstraint = new NBi.NUnit.Member.ContainedInConstraint(new List<string>() {"First member", "Second Member"});

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

            var subsetOfConstraint = new NBi.NUnit.Member.ContainedInConstraint(new List<string>() { "First member" });

            //Call the method to test
            var res = subsetOfConstraint.Matches(members);

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
            var mockedFailure = Mock.Of<ItemsMessageMarkdown>(f => f.RenderActual() == "failure actual");

            //Buiding object used during test
            var subsetOfConstraint = new NBi.NUnit.Member.ContainedInConstraint(new List<string>() { "First member" });
            subsetOfConstraint.Failure = mockedFailure;

            //Call the method to test
            subsetOfConstraint.WriteActualValueTo(writer);

            //Test conclusion            
            Mock.Get(mockedFailure).Verify(f => f.RenderActual());
        }

        [Test]
        public void WriteDescriptionTo_FailureExist_FailureRenderExpectedCalled()
        {
            //Stub the writer
            var stubWriter = new Mock<global::NUnit.Framework.Constraints.MessageWriter>();
            var writer = stubWriter.Object;

            //Mock the failure
            var mockedFailure = Mock.Of<ItemsMessageMarkdown>(f => f.RenderExpected() == "failure actual");

            //Buiding object used during test
            var subsetOfConstraint = new NBi.NUnit.Member.ContainedInConstraint(new List<string>() { "First member" });
            subsetOfConstraint.Failure = mockedFailure;

            //Call the method to test
            subsetOfConstraint.WriteDescriptionTo(writer);

            //Test conclusion            
            Mock.Get(mockedFailure).Verify(f => f.RenderExpected());
        }

    }
}
