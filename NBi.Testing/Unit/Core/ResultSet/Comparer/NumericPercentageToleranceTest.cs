﻿using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    public class NumericPercentageToleranceTest
    {
        [Test]
        public void ValueString_FiftyPercent_Correct()
        {
            var tolerance = new NumericPercentageTolerance(new decimal(0.5));
            Assert.That(tolerance.ValueString, Is.EqualTo("50.0%"));
        }

        [Test]
        public void ValueString_TwentyFivePercent_Correct()
        {
            var tolerance = new NumericPercentageTolerance(new decimal(0.25));
            Assert.That(tolerance.ValueString, Is.EqualTo("25.00%"));
        }
    }
}
