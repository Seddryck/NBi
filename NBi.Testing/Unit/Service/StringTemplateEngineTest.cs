using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Service;
using NBi.Xml.Constraints;
using NUnit.Framework;

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
        public void Build_OrderingTemplate_CorrectTest()
        {
            var template  = ReadTemplateFile("Ordered");
            var variables = new string[] { "perspective", "dimension", "hierarchy", "order" };
            var data      = new List<List<List<object>>>();
            data.Add(BuildCase(new string[] { "myPerspective", "myDimension", "myHierarchy", "numerical" }));

            var engine = new StringTemplateEngine(template, variables);
            var testSuite = engine.Build(data);
            var test = testSuite.ElementAt(0);

            //Test the object
            var ordered = test.Constraints[0] as OrderedXml;
            Assert.That(ordered.Rule, Is.EqualTo(OrderedXml.Order.Numerical));
            Assert.That(ordered.Descending, Is.EqualTo(false));
            Assert.That(ordered.Definition.Count, Is.EqualTo(0));

            //Test the content serialized
            var content = test.Content;
            Assert.That(content, Is.StringContaining("rule=\"numerical\""));
            Assert.That(content, Is.Not.StringContaining("descending=\"false\""));
            Assert.That(content, Is.Not.StringContaining("<rule-definition"));
        }
    }
}
