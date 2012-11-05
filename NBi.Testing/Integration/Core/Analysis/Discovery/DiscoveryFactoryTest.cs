using System;
using System.Linq;
using NBi.Core.Analysis.Discovery;
using NBi.NUnit.Builder;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Discovery
{
    [TestFixture]
    public class DiscoveryFactoryTest
    {
        [Test]
        public void Build_MemberCaptionFilled_MemberCaptionIsSet()
        {
            var facto = new DiscoveryFactory();

            var disco = facto.Build("connectionString", "parent-member", "perspective", "dimension", "hierarchy", null);

            //Assertion
            Assert.That(disco.MemberCaption, Is.EqualTo("parent-member"));
        }

        [Test]
        public void Build_MemberCaptionFilled_FunctionIsSetToChildren()
        {
            var facto = new DiscoveryFactory();

            var disco = facto.Build("connectionString", "parent-member", "perspective", "dimension", "hierarchy", null);

            //Assertion
            Assert.That(disco.Function.ToLower(), Is.EqualTo("Children".ToLower()));
        }

        [Test]
        public void Build_MemberCaptionNotFilled_FunctionIsSetToMembers()
        {
            var facto = new DiscoveryFactory();

            var disco = facto.Build("connectionString", string.Empty, "perspective", "dimension", "hierarchy", null);

            //Assertion
            Assert.That(disco.Function.ToLower(), Is.EqualTo("Members".ToLower()));
        }
    }
}
