using NBi.Core.Scalar.Resolver;
using NBi.NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.NUnit
{
    public class DifferedConstraintTest
    {
        [Test]
        public void Resolve_GreaterThanInt32Success_Pass()
        {
            var differed = new DifferedConstraint(typeof(GreaterThanConstraint), new LiteralScalarResolver<decimal>(new LiteralScalarResolverArgs(3)));
            var ctr = differed.Resolve();
            Assert.That(5, ctr);
        }

        [Test]
        public void Resolve_GreaterThanInt32Fail_ThrowException()
        {
            var differed = new DifferedConstraint(typeof(GreaterThanConstraint), new LiteralScalarResolver<decimal>(new LiteralScalarResolverArgs(3)));
            var ctr = differed.Resolve();
            Assert.Throws<AssertionException>(() => Assert.That(2, ctr));
        }
    }
}
