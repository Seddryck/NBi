using System.Collections.Generic;
using Moq;
using NBi.Core.Structure;
using NBi.NUnit.Member;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Member
{
    //[TestFixture]
    //public class EquivalentToConstraintTest
    //{
    //    [Test]
    //    public void Matches_OneCaptionContainedInMembers_Validated()
    //    {
    //        //Buiding object used during test
    //        var members = new MemberResult();
    //        members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
    //        members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));

    //        var equivalentConstraint = new NBi.NUnit.Member.EquivalentToConstraint(new List<string>() { "First member", "Second member" });

    //        //Call the method to test
    //        var res = equivalentConstraint.Matches(members);

    //        //Test conclusion            
    //        Assert.That(res, Is.True);
    //    }

    //    [Test]
    //    public void Matches_OneCaptionContainedInMembersOneMore_Failure()
    //    {
    //        //Buiding object used during test
    //        var members = new MemberResult();
    //        members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));
    //        members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Second member]", "Second member", 2, 0));
    //        members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[Third member]", "Third member", 3, 0));

    //        var equivalentConstraint = new NBi.NUnit.Member.EquivalentToConstraint(new List<string>() { "First member", "Second member" });

    //        //Call the method to test
    //        var res = equivalentConstraint.Matches(members);

    //        //Test conclusion            
    //        Assert.That(res, Is.False);
    //    }

    //    [Test]
    //    public void Matches_OneCaptionContainedInMembersOneLess_Failure()
    //    {
    //        //Buiding object used during test
    //        var members = new MemberResult();
    //        members.Add(new NBi.Core.Analysis.Member.Member("[Hierarchy].[First member]", "First member", 1, 0));

    //        var equivalentConstraint = new NBi.NUnit.Member.EquivalentToConstraint(new List<string>() { "First member", "Second member" });

    //        //Call the method to test
    //        var res = equivalentConstraint.Matches(members);

    //        //Test conclusion            
    //        Assert.That(res, Is.False);
    //    }
        

       
    //}
}
