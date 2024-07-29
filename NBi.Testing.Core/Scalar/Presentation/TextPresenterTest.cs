using NBi.Core.ResultSet;
using NBi.Core.Scalar.Presentation;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Presentation
{
    public class TextPresenterTest
    {
        [Test]
        [TestCase("", "(empty)")]
        [TestCase(" ", "(1 space)")]
        [TestCase("  ", "(2 spaces)")]
        [TestCase("MyText", "MyText")]
        [TestCase(10, "10")]
        [TestCase(true, "True")]
        public void Execute_TextColumnObjectValue_CorrectDisplay(object value, string expected)
        {
            var factory = new PresenterFactory();
            var presenter = factory.Instantiate(ColumnType.Text);
            var text = presenter.Execute(value);
            Assert.That(text, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10.50, "10,5", "fr-fr")]
        [TestCase(10.50, "10.5", "en-us")]
        public void Execute_TextColumnObjectValueCultureSpecific_CorrectDisplay(object value, string expected, string culture)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture, false);
            var factory = new PresenterFactory();
            var presenter = factory.Instantiate(ColumnType.Text);
            var text = presenter.Execute(value);
            Assert.That(text, Is.EqualTo(expected));
        }
    }
}
