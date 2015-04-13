using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    public class NumericAbsoluteToleranceTest
    {
        [Test]
        public void ValueString_Fifty_Correct()
        {
            var tolerance = new NumericAbsoluteTolerance(new decimal(50));
            Assert.That(tolerance.ValueString, Is.EqualTo("50"));
        }

        [Test]
        public void ValueString_TwentyFivePercentDotSeven_Correct()
        {
            var tolerance = new NumericAbsoluteTolerance(new decimal(25.7));
            Assert.That(tolerance.ValueString, Is.EqualTo("25.7"));
        }
    }
}
