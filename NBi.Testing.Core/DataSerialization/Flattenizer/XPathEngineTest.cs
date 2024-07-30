using NBi.Core.DataSerialization.Flattening;
using NBi.Core.DataSerialization.Flattening.Xml;
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
    public class XPathStreamEngineTest
    {
        protected StreamReader GetResourceReader(string filename)
        {
            // A Stream is needed to read the XML document.
            var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream($"{GetType().Namespace}.Resources.{filename}.xml")
                                           ?? throw new FileNotFoundException();
            var reader = new StreamReader(stream);
            return reader;
        }

        [Test]
        public void Execute_Example_ColumnCount()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder/PurchaseOrderNumber"))
                , new AttributeSelect(new LiteralScalarResolver<string>("."), "PartNumber")
                , new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder/Address[@Type=\"Shiping\"]/City"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).Count, Is.EqualTo(3));
        }

        [Test]
        [TestCase("//PurchaseOrder/Items/Item", 5)]
        [TestCase("//PurchaseOrder", 3)]
        public void Execute_Example_RowCount(string from, int rowCount)
        {
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder/PurchaseOrderNumber"))
                , new AttributeSelect(new LiteralScalarResolver<string>("."),"PartNumber")
                , new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder/Address[@Type=\"Shiping\"]/City"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(result.Count, Is.EqualTo(rowCount));
        }

        [Test]
        public void Execute_FromElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item/ProductName";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("."))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("Lawnmower"));
        }

        [Test]
        public void Execute_FromAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect(new LiteralScalarResolver<string>("."),"PartNumber")
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("872-AA"));
        }

        [Test]
        public void Execute_ChildElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder/Items/Item/ProductName"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("Lawnmower"));
        }

        [Test]
        public void Execute_ChildAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect(new LiteralScalarResolver<string>("//PurchaseOrder/Items/Item"),"PartNumber")
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("872-AA"));
        }

        [Test]
        public void Execute_ParentElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Does.Contain("Ellen Adams"));
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Does.Contain("Maple Street"));
        }

        [Test]
        public void Execute_ParentAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect(new LiteralScalarResolver<string>("//PurchaseOrder"),"PurchaseOrderNumber")
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("99503"));
        }

        [Test]
        public void Execute_MissingElement_Null()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("//PurchaseOrder/Missing"))
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("(null)"));
        }

        [Test]
        public void Execute_MissingAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect(new LiteralScalarResolver<string>("//PurchaseOrder"), "Missing")
            };

            using var reader = GetResourceReader("PurchaseOrders");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, string.Empty, false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("(null)"));
        }

        [Test]
        public void Execute_FromElementWithDefaultNamespace_ValueCorrect()
        {
            var from = "//prefix:PurchaseOrder/prefix:Items/prefix:Item/prefix:ProductName";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("."))
            };

            using var reader = GetResourceReader("PurchaseOrdersDefaultNamespace");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, "prefix", false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("Lawnmower"));
        }

        [Test]
        public void Execute_FromElementWithDefaultNamespaceAndIgnoreDefaultNamespace_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item/ProductName";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("."))
            };

            using var reader = GetResourceReader("PurchaseOrdersDefaultNamespace");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, "prefix", true);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("Lawnmower"));
        }

        [Test]
        public void Execute_FromElementWithManyNamespaces_ValueCorrect()
        {
            var from = "//prefix:PurchaseOrder/adr:Address/prefix:Street";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("."))
            };

            using var reader = GetResourceReader("PurchaseOrdersManyNamespaces");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, "prefix", false);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("123 Maple Street"));
        }

        [Test]
        public void Execute_FromElementWithManyNamespacesIgnoringDefaultNamespace_ValueCorrect()
        {
            var from = "//PurchaseOrder/Address/Street";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(new LiteralScalarResolver<string>("."))
            };

            using var reader = GetResourceReader("PurchaseOrdersManyNamespaces");
            var engine = new XPathEngine(new LiteralScalarResolver<string>(from), selects, "prefix", true);
            var result = engine.Execute(reader);
            Assert.That(((IEnumerable<object>)result.ElementAt(0)).ElementAt(0), Is.EqualTo("123 Maple Street"));
        }
    }
}
