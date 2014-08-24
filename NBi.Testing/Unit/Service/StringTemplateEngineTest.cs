using System;
using System.Data;
using System.Linq;
using NBi.Service;
using NUnit.Framework;
using System.Collections.Generic;
using NBi.Xml.Systems;
using NBi.Xml.Constraints;
using System.Reflection;
using System.IO;

namespace NBi.Testing.Unit.Service
{
    [TestFixture]
    public class StringTemplateEngineTest
    {

        private string ReadTemplateFile(string filename)
        {
            var template = string.Empty;
            // A Stream is needed to read the nbitt document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(
                                           string.Format("NBi.Testing.Unit.Service.Resources.{0}.nbitt", filename)
                ))
            using (StreamReader reader = new StreamReader(stream))
            {
                template = reader.ReadToEnd();
            }
            return template;
        }

        private List<List<object>> BuildCase(IEnumerable<string> inlineCase)
        {
            var caseBuilt = new List<List<object>>();
            foreach (var item in inlineCase)
                caseBuilt.Add(new List<object>() { item });

            return caseBuilt;
        }

        [Test]
        public void Build_OrderedLightTemplate_CorrectTest()
        {
            var template = ReadTemplateFile("OrderedLight");
            var variables = new string[] { "perspective", "dimension", "hierarchy", "order" };
            var data = new List<List<List<object>>>();
            data.Add(BuildCase(new string[] { "myPerspective", "myDimension", "myHierarchy", "numerical" }));

            var engine = new StringTemplateEngine(template, variables);
            var testSuite = engine.Build(data);
            var test = testSuite.ElementAt(0);

            //Test the object
            var members = test.Systems[0] as MembersXml;
            Assert.That(members.Exclude.Items, Is.Null.Or.Empty);
            Assert.That(members.Exclude.Patterns, Is.Null.Or.Empty);

            var ordered = test.Constraints[0] as OrderedXml;
            Assert.That(ordered.Rule, Is.EqualTo(OrderedXml.Order.Numerical));
            Assert.That(ordered.Descending, Is.EqualTo(false));
            Assert.That(ordered.Definition, Is.Null.Or.Empty);

            //Test the content serialized
            var content = test.Content;
            Assert.That(content, Is.StringContaining("rule=\"numerical\""));
            Assert.That(content, Is.Not.StringContaining("descending=\"false\""));
            Assert.That(content, Is.Not.StringContaining("<rule-definition"));
            Assert.That(content, Is.Not.StringContaining("<exclude"));
            
        }

        [Test]
        public void Build_OrderedLightTemplate_ConditionSetupCleanupAreNotAvailable()
        {
            var template = ReadTemplateFile("OrderedLight");
            var variables = new string[] { "perspective", "dimension", "hierarchy", "order" };
            var data = new List<List<List<object>>>();
            data.Add(BuildCase(new string[] { "myPerspective", "myDimension", "myHierarchy", "numerical" }));

            var engine = new StringTemplateEngine(template, variables);
            var testSuite = engine.Build(data);
            var test = testSuite.ElementAt(0);

            //Test the content serialized
            var content = test.Content;
            Assert.That(content, Is.Not.StringContaining("<setup"));
            Assert.That(content, Is.Not.StringContaining("<condition"));
            Assert.That(content, Is.Not.StringContaining("<cleanup"));
        }

        [Test]
        public void Build_OrderedFullTemplate_CorrectTest()
        {
            var template = ReadTemplateFile("OrderedFull");
            var variables = new string[] { "perspective", "dimension", "hierarchy", "order", "descending", "exclude" };
            var data = new List<List<List<object>>>();
            data.Add(BuildCase(new string[] { "myPerspective", "myDimension", "myHierarchy", "specific", "true", "Unknown" }));

            var engine = new StringTemplateEngine(template, variables);
            var testSuite = engine.Build(data);
            var test = testSuite.ElementAt(0);

            //Test the object
            var members = test.Systems[0] as MembersXml;
            Assert.That(members.Exclude.Items, Is.Not.Null.And.Not.Empty);

            var ordered = test.Constraints[0] as OrderedXml;
            Assert.That(ordered.Rule, Is.EqualTo(OrderedXml.Order.Specific));
            Assert.That(ordered.Descending, Is.EqualTo(true));
            Assert.That(ordered.Definition, Is.Not.Null.And.Not.Empty);

            //Test the content serialized
            var content = test.Content;
            Assert.That(content, Is.StringContaining("rule=\"specific\""));
            Assert.That(content, Is.StringContaining("descending=\"true\""));
            Assert.That(content, Is.StringContaining("<rule-definition"));
            Assert.That(content, Is.StringContaining("<exclude"));
        }

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
