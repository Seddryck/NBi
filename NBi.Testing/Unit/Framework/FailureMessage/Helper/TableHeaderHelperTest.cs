﻿using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Framework.FailureMessage
{
    class TableHeaderFormatterTest
    {
        [Test]
        public void GetText_NumericAbsoluteTolerance_CorrectHeader()
        {
            var formatter = new TableHeaderHelper();
            var text = formatter.GetText(ColumnRole.Value, ColumnType.Numeric, new NumericAbsoluteTolerance(new decimal(0.5)), null);

            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(+/- 0.5)"));
        }

        [Test]
        public void GetText_NumericPercentageTolerance_CorrectHeader()
        {
            var formatter = new TableHeaderHelper();
            var text = formatter.GetText(ColumnRole.Value, ColumnType.Numeric, new NumericPercentageTolerance(new decimal(12.5)), null);

            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(+/- 12.5%)"));
        }

        [Test]
        public void GetText_DateTimeTolerance_CorrectHeader()
        {
            var formatter = new TableHeaderHelper();
            var text = formatter.GetText(ColumnRole.Value, ColumnType.DateTime, new DateTimeTolerance(new TimeSpan(0, 15, 0)), null);

            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("DateTime"));
            Assert.That(text, Is.StringContaining("(+/- 00:15:00)"));
        }

        [Test]
        public void GetText_DateTimeRounding_CorrectHeader()
        {
            var formatter = new TableHeaderHelper();
            var text = formatter.GetText(ColumnRole.Value, ColumnType.DateTime, null, new DateTimeRounding(new TimeSpan(0, 15, 0), Rounding.RoundingStyle.Floor));

            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("DateTime"));
            Assert.That(text, Is.StringContaining("(floor 00:15:00)"));
        }

        [Test]
        public void GetText_NumericRounding_CorrectHeader()
        {
            var formatter = new TableHeaderHelper();
            var text = formatter.GetText(ColumnRole.Value, ColumnType.Numeric, null, new NumericRounding(10.5, Rounding.RoundingStyle.Round));

            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(round 10.5)"));
        }

        [Test]
        public void GetText_NoToleranceOrRounding_DoesntEndWithSpace()
        {
            var formatter = new TableHeaderHelper();
            var text = formatter.GetText(ColumnRole.Value, ColumnType.Numeric, null, null);

            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.Not.StringEnding(" "));
        }
    }
}
