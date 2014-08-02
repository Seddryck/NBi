using System;
using System.Data;
using System.Linq;
using NBi.Service;
using NUnit.Framework;
using System.Collections.Generic;

namespace NBi.Testing.Unit.Service
{
    [TestFixture]
    public class StringTemplateEngineTest
    {
        [Test]
        public void BuildTestString_OneSimpleRow_CorrectRendering()
        {
            var template = "<dimension caption='$dimension$' perspective='$perspective$'/>";
            var engine = new StringTemplateEngine(template, new string[] {"dimension", "perspective"});

            var values = new List<List<object>>();
            var firstCell = new List<object>() {"myDim"};
            var secondCell = new List<object>() {"myPersp"};
            values.Add(firstCell);
            values.Add(secondCell);
            var loaded = new List<string>();

            engine.InitializeTemplate();
            var result = engine.BuildTestString(values);

            Assert.That(result, Is.EqualTo("<dimension caption='myDim' perspective='myPersp'/>"));

        }

        [Test]
        public void BuildTestString_OneRowWithMultipleItems_CorrectRendering()
        {
            var template = "$dimension$ ... <subsetOf>\r\n\t<item>$items; separator=\"</item>\r\n\t<item>\"$</item>\r\n</subsetOf>";
            var engine = new StringTemplateEngine(template, new string[] { "dimension", "items" });

            var values = new List<List<object>>();
            var firstCell = new List<object>() { "myDim" };
            var secondCell = new List<object>() { "item A", "item B" };
            values.Add(firstCell);
            values.Add(secondCell);
            var loaded = new List<string>();

            engine.InitializeTemplate();
            var result = engine.BuildTestString(values);

            Assert.That(result, Is.EqualTo("myDim ... <subsetOf>\r\n\t<item>item A</item>\r\n\t<item>item B</item>\r\n</subsetOf>"));
        }

        [Test]
        public void BuildTestString_OneRowWithNoneVariable_CorrectRenderingTextIsIgnored()
        {
            var template = "$dimension$ ... $if(ignore)$<ignore>$ignore$</ignore>$endif$";
            var engine = new StringTemplateEngine(template, new string[] { "dimension", "ignore" });

            var values = new List<List<object>>();
            var firstCell = new List<object>() { "myDim" };
            var secondCell = new List<object>() { "(none)" };
            values.Add(firstCell);
            values.Add(secondCell);
            var loaded = new List<string>();

            engine.InitializeTemplate();
            var result = engine.BuildTestString(values);

            Assert.That(result, Is.EqualTo("myDim ... "));
        }

        [Test]
        public void BuildTestString_OneRowWithEmptyVariable_CorrectRenderingTextIsIgnoredAndVariablePlaceHolderIsEmpty()
        {
            var template = "$dimension$ ->$empty$<- ... $if(empty)$<ignore>$empty$</ignore>$endif$";
            var engine = new StringTemplateEngine(template, new string[] { "dimension", "empty" });

            var values = new List<List<object>>();
            var firstCell = new List<object>() { "myDim" };
            var secondCell = new List<object>() { string.Empty };
            values.Add(firstCell);
            values.Add(secondCell);
            var loaded = new List<string>();

            engine.InitializeTemplate();
            var result = engine.BuildTestString(values);

            Assert.That(result, Is.EqualTo("myDim -><- ... "));
        }

        [Test]
        public void BuildTestString_OneRowWithNotIgnoredVariable_CorrectRenderingTextIsDisplayed()
        {
            var template = "$dimension$ ... $if(ignore)$<ignore>$ignore$</ignore>$endif$";
            var engine = new StringTemplateEngine(template, new string[] { "dimension", "ignore" });

            var values = new List<List<object>>();
            var firstCell = new List<object>() { "myDim" };
            var secondCell = new List<object>() { "reason to ignore" };
            values.Add(firstCell);
            values.Add(secondCell);
            var loaded = new List<string>();

            engine.InitializeTemplate();
            var result = engine.BuildTestString(values);

            Assert.That(result, Is.EqualTo("myDim ... <ignore>reason to ignore</ignore>"));
        }
    }
}
