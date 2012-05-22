using System.Collections.Generic;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit
{
    public class CountConstraintTest
    {
        [Test]
        public void Matches_ExactlyCorrectlySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.Exactly(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_ExactlyWronglySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.Exactly(1);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_MoreThanCorrectlySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.MoreThan(1);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_MoreThanWronglySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.MoreThan(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_LessThanCorrectlySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.LessThan(3);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_LessThanWronglySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.LessThan(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_LessThanAndMoreThanCorrectlySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.MoreThan(1);
            countConstraint.LessThan(3);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_LessThanAndMoreThanWronglySpecified_Validated()
        {
            var members = new List<string>();
            members.Add("First member");
            members.Add("Second member");

            var countConstraint = new NBi.NUnit.CountConstraint();
            countConstraint.MoreThan(1);
            countConstraint.LessThan(2);

            //Method under test
            var res = countConstraint.Matches(members);

            //Test conclusion            
            Assert.That(res, Is.False);
        }
    }
}
