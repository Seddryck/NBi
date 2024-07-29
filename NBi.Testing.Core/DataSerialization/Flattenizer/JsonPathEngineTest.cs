using NBi.Core.DataSerialization.Flattening;
using NBi.Core.DataSerialization.Flattening.Json;
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

namespace NBi.Core.Testing.DataSerialization.Flattenizer
{
    public class JsonPathEngineTest
    {
        protected StreamReader GetResourceReader(string filename)
        {
            // A Stream is needed to read the XML document.
            var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream($"{GetType().Namespace}.Resources.{filename}.json") ?? throw new NullReferenceException();
            var reader = new StreamReader(stream);
            return reader;
        }


        [Test]
        [TestCase("$.PurchaseOrders[*].Items[*]", 5)]
        [TestCase("$.PurchaseOrders[*]", 4)]
        public void Execute_Example_RowCount(string from, int rowCount)
        {
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("$"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
            var result = engine.Execute(reader);
            Assert.That(result.Count, Is.EqualTo(rowCount));
        }

        [Test]
        public void Execute_Example_FirstColumnIsCorrect()
        {
            var from = "$.PurchaseOrders[*].Items[*]";
            var selects = new List<ElementSelect>()
            {
                new (new LiteralScalarResolver<string>("PartNumber"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
            var result = engine.Execute(reader);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Select(x => ((string)((IEnumerable<object>)x).ElementAt(0)).Length), Is.All.EqualTo(6)); //Format is 123-XY
        }

        [Test]
        public void Execute_Example_AllColumnsAreCorrect()
        {
            var from = "$.PurchaseOrders[*].Items[*]";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("PartNumber")),
                new ElementSelect(new LiteralScalarResolver<string>("Quantity"))
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
                var result = engine.Execute(reader);
                Assert.That(result.Count, Is.EqualTo(5));
                Assert.That(result.Count, Is.EqualTo(5));
                Assert.That(result.Select(x => ((string)((IEnumerable<object>)x).ElementAt(0)).Length), Is.All.EqualTo(6)); //Format is 123-XY
                Assert.That(result.Select(x => ((IEnumerable<object>)x).ElementAt(1)), Is.All.EqualTo(1).Or.EqualTo(2)); //All quantity are between 1 and 2
            }
        }


        [Test]
        public void Execute_FromElement_ValueCorrect()
        {
            var from = "$.PurchaseOrders[*].Items[*].ProductName";
            var selects = new List<ElementSelect>()
            {
                new (new LiteralScalarResolver<string>("$"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("Lawnmower"));
        }

        [Test]
        public void Execute_FromAttribute_ValueCorrect()
        {
            var from = "$.PurchaseOrders[*].Items[*]";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("$.PartNumber"))
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
                var result = engine.Execute(reader);
                Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("872-AA"));
            }
        }

        [Test]
        public void Execute_MissingElement_Null()
        {
            var from = "$.PurchaseOrders[*]";
            var selects = new List<ElementSelect>()
            {
                new(new LiteralScalarResolver<string>("$.PurchaseOrderNumber"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(3)).ElementAt(0), Is.EqualTo("(null)"));
        }

        [Test]
        public void Execute_ParentElement_ValueCorrect()
        {
            var from = "$.PurchaseOrders[*].Items[*]";
            var selects = new List<ElementSelect>()
            {
                new (new LiteralScalarResolver<string>("!!.PurchaseOrderNumber")),
                new (new LiteralScalarResolver<string>("$.PartNumber"))
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
                var result = engine.Execute(reader);
                Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Does.Contain("99503"));
                Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(1), Does.Contain("872-AA"));
                Assert.That(((IEnumerable<object>)result.ElementAt(1)).ElementAt(0), Does.Contain("99503"));
                Assert.That(((IEnumerable<object>)result.ElementAt(1)).ElementAt(1), Does.Contain("926-AA"));
                Assert.That(((IEnumerable<object>)result.ElementAt(2)).ElementAt(0), Does.Contain("99505"));
                Assert.That(((IEnumerable<object>)result.ElementAt(2)).ElementAt(1), Does.Contain("456-NM"));
            }
        }

        [Test]
        public void Execute_ParentElementGoingAboveRoot_ValueCorrect()
        {
            var from = "$.PurchaseOrders[*].Items[*]";
            var selects = new List<ElementSelect>()
            {
                new(new LiteralScalarResolver<string>("!!!!!!.PurchaseOrderNumber")),
                new(new LiteralScalarResolver<string>("$.PartNumber"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new JsonPathEngine(new LiteralScalarResolver<string>(from), selects);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Does.Contain("(null)"));
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(1), Does.Contain("872-AA"));
            Assert.That(((IEnumerable<object>)result.ElementAt(1)).ElementAt(0), Does.Contain("(null)"));
            Assert.That(((IEnumerable<object>)result.ElementAt(1)).ElementAt(1), Does.Contain("926-AA"));
            Assert.That(((IEnumerable<object>)result.ElementAt(2)).ElementAt(0), Does.Contain("(null)"));
            Assert.That(((IEnumerable<object>)result.ElementAt(2)).ElementAt(1), Does.Contain("456-NM"));
        }
    }
}
