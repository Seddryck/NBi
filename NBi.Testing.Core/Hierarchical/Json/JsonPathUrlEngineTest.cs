using NBi.Core.Hierarchical;
using NBi.Core.Hierarchical.Json;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBi.Testing.Core.Hierarchical.Json
{
    public class JsonPathUrlEngineTest
    {
        private class StubJsonPathUrlEngine : JsonPathUrlEngine
        {
            public StubJsonPathUrlEngine(IScalarResolver<string> url, string from, IEnumerable<AbstractSelect> selects)
            : base(url, from, selects) { }

            protected override string GetRemoteContent(string url)
            {
                using (var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"{GetType().Namespace}.Resources.{url}"))
                using (var streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        [Test]
        [TestCase("$.PurchaseOrders[*].Items[*]", 5)]
        [TestCase("$.PurchaseOrders[*]", 4)]
        public void Execute_Example_RowCount(string from, int rowCount)
        {
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("$")
            };

            var engine = new StubJsonPathUrlEngine(new LiteralScalarResolver<string>("PurchaseOrders.json"), from, selects);
            var result = engine.Execute();
            Assert.That(result.Count, Is.EqualTo(rowCount));
        }
    }
}
