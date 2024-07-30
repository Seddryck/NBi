using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NUnit.Framework;

namespace NBi.Core.Testing
{
    [TestFixture]
    public class ListComparisonFormatterTest
    {
        [Test]
        public void Compare_MultipleMissing_Plural()
        {
            var res = new ListComparer.Result(
                ["a", "b", "c"],
                null
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Does.Contain("Missing items"));
            Assert.That(display, Does.Contain("<a>"));
            Assert.That(display, Does.Contain("<b>"));
            Assert.That(display, Does.Contain("<c>"));

        }

        [Test]
        public void Compare_NoUnexpected_UnexpectedNotVisible()
        {
            var res = new ListComparer.Result(
                ["a", "b", "c"],
                null
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Does.Contain("No unexpected"));

        }


        [Test]
        public void Compare_Mix_CorrectDisplay()
        {
            var res = new ListComparer.Result(
                ["x"],
                ["a", "b", "c"]
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Does.Not.Contain("Missing items"));
            Assert.That(display, Does.Contain("Missing item"));
            Assert.That(display, Does.Contain("Unexpected items"));
            Assert.That(display, Does.Contain("<x>"));
            Assert.That(display, Does.Contain("<a>"));
            Assert.That(display, Does.Contain("<b>"));
            Assert.That(display, Does.Contain("<c>"));

        }

        [Test]
        public void Compare_BothEmpty_CorrectDisplay()
        {
            var res = new ListComparer.Result(
                [],
                []
                );

            var formatter = new ListComparisonFormatter();
            var display = formatter.Format(res).ToString();

            Assert.That(display, Does.Contain("No missing item"));
            Assert.That(display, Does.Contain("No unexpected item"));

        }

    }
}
