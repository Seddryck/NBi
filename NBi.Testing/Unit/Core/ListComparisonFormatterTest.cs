using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core
{
    [TestFixture]
    public class ListComparisonFormatterTest
    {
        [Test]
        public void Compare_MultipleMissing_Plural()
        {
            var res = new ListComparer.Result(
                new List<string> { "a", "b", "c" },
                null
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Is.StringContaining("Missing items"));
            Assert.That(display, Is.StringContaining("<a>"));
            Assert.That(display, Is.StringContaining("<b>"));
            Assert.That(display, Is.StringContaining("<c>"));

        }

        [Test]
        public void Compare_NoUnexpected_UnexpectedNotVisible()
        {
            var res = new ListComparer.Result(
                new List<string> { "a", "b", "c" },
                null
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Is.Not.StringContaining("nexpected"));

        }


        [Test]
        public void Compare_Mix_CorrectDisplay()
        {
            var res = new ListComparer.Result(
                new List<string> { "x" },
                new List<string> { "a", "b", "c" }
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Is.Not.StringContaining("Missing items"));
            Assert.That(display, Is.StringContaining("Missing item"));
            Assert.That(display, Is.StringContaining("Unexpected items"));
            Assert.That(display, Is.StringContaining("<x>"));
            Assert.That(display, Is.StringContaining("<a>"));
            Assert.That(display, Is.StringContaining("<b>"));
            Assert.That(display, Is.StringContaining("<c>"));

        }

        [Test]
        public void Compare_BothEmpty_CorrectDisplay()
        {
            var res = new ListComparer.Result(
                new List<string> { },
                new List<string> { }
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Is.StringContaining("No missing item"));
            Assert.That(display, Is.StringContaining("No unexpected item"));

        }

    }
}
