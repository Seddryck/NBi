using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Request
{
    [TestFixture]
    [Category("Olap")]
    public class DiscoveryRequestFactoryTest
    {
        [Test]
        public void Build_MemberCaptionFilled_MemberCaptionIsSet()
        {
            var facto = new DiscoveryRequestFactory();

            var disco = facto.Build("connectionString", "parent-member", "perspective", "dimension", "hierarchy", null);

            //Assertion
            Assert.That(disco.MemberCaption, Is.EqualTo("parent-member"));
        }

        [Test]
        public void Build_MemberCaptionFilled_FunctionIsSetToChildren()
        {
            var facto = new DiscoveryRequestFactory();

            var disco = facto.Build("connectionString", "parent-member", "perspective", "dimension", "hierarchy", null);

            //Assertion
            Assert.That(disco.Function.ToLower(), Is.EqualTo("Children".ToLower()));
        }

        [Test]
        public void Build_MemberCaptionNotFilled_FunctionIsSetToMembers()
        {
            var facto = new DiscoveryRequestFactory();

            var disco = facto.Build("connectionString", string.Empty, "perspective", "dimension", "hierarchy", null);

            //Assertion
            Assert.That(disco.Function.ToLower(), Is.EqualTo("Members".ToLower()));
        }
    }
}
