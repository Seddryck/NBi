using NBi.Core.Hierarchical;
using NBi.Core.Hierarchical.Xml;
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

namespace NBi.Testing.Core.Hierarchical.Xml
{
    public class XPathStreamEngineTest
    {
        private class XPathStreamEngine : XPathFileEngine
        {
            private readonly StreamReader streamReader;

            public XPathStreamEngine(StreamReader streamReader, string from, IEnumerable<ElementSelect> selects)
                : this(streamReader, from, selects, string.Empty, false)
            { }

            public XPathStreamEngine(StreamReader streamReader, string from, IEnumerable<ElementSelect> selects, string prefix)
                : this(streamReader, from, selects, prefix, false)
            { }

            protected XPathStreamEngine(StreamReader streamReader, string from, IEnumerable<ElementSelect> selects, string prefix, bool ignoreDefaultNamespace)
                : base(new LiteralScalarResolver<string>(string.Empty), string.Empty, from, selects, prefix, ignoreDefaultNamespace)
                => this.streamReader = streamReader;

            protected override TextReader GetTextReader(string filePath)
                => streamReader;

            protected override string EnsureFileExist()
                => string.Empty;
        }

        private class XPathStreamIgnoreNamespaceEngine : XPathStreamEngine
        {
            public XPathStreamIgnoreNamespaceEngine(StreamReader streamReader, string from, IEnumerable<ElementSelect> selects, string prefix)
                : base(streamReader, from, selects, prefix, true)
                { }
        }


        protected StreamReader GetResourceReader(string filename)
        {
            // A Stream is needed to read the XML document.
            var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream($"{GetType().Namespace}.Resources.{filename}.xml");
            var reader = new StreamReader(stream);
            return reader;
        }

        [Test]
        public void Execute_Example_ColumnCount()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("//PurchaseOrder/PurchaseOrderNumber")
                , new AttributeSelect(".", "PartNumber")
                , new ElementSelect("//PurchaseOrder/Address[@Type=\"Shiping\"]/City")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).Count, Is.EqualTo(3));
            }
        }

        [Test]
        [TestCase("//PurchaseOrder/Items/Item", 5)]
        [TestCase("//PurchaseOrder", 3)]
        public void Execute_Example_RowCount(string from, int rowCount)
        {
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("//PurchaseOrder/PurchaseOrderNumber")
                , new AttributeSelect(".","PartNumber")
                , new ElementSelect("//PurchaseOrder/Address[@Type=\"Shiping\"]/City")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That(result.Count, Is.EqualTo(rowCount));
            }
        }

        [Test]
        public void Execute_FromElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item/ProductName";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(".")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("Lawnmower"));
            }
        }

        [Test]
        public void Execute_FromAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect(".","PartNumber")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("872-AA"));
            }
        }

        [Test]
        public void Execute_ChildElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("//PurchaseOrder/Items/Item/ProductName")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("Lawnmower"));
            }
        }

        [Test]
        public void Execute_ChildAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect("//PurchaseOrder/Items/Item","PartNumber")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("872-AA"));
            }
        }

        [Test]
        public void Execute_ParentElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("//PurchaseOrder")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Does.Contain("Ellen Adams"));
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Does.Contain("Maple Street"));
            }
        }

        [Test]
        public void Execute_ParentAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect("//PurchaseOrder","PurchaseOrderNumber")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("99503"));
            }
        }

        [Test]
        public void Execute_MissingElement_Null()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("//PurchaseOrder/Missing")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("(null)"));
            }
        }

        [Test]
        public void Execute_MissingAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<ElementSelect>()
            {
                new AttributeSelect("//PurchaseOrder", "Missing")
            };

            using (var reader = GetResourceReader("PurchaseOrders"))
            {
                var engine = new XPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("(null)"));
            }
        }

        [Test]
        public void Execute_FromElementWithDefaultNamespace_ValueCorrect()
        {
            var from = "//prefix:PurchaseOrder/prefix:Items/prefix:Item/prefix:ProductName";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(".")
            };

            using (var reader = GetResourceReader("PurchaseOrdersDefaultNamespace"))
            {
                var engine = new XPathStreamEngine(reader, from, selects, "prefix");
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("Lawnmower"));
            }
        }

        [Test]
        public void Execute_FromElementWithDefaultNamespaceAndIgnoreDefaultNamespace_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item/ProductName";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(".")
            };

            using (var reader = GetResourceReader("PurchaseOrdersDefaultNamespace"))
            {
                var engine = new XPathStreamIgnoreNamespaceEngine(reader, from, selects, "prefix");
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("Lawnmower"));
            }
        }

        [Test]
        public void Execute_FromElementWithManyNamespaces_ValueCorrect()
        {
            var from = "//prefix:PurchaseOrder/adr:Address/prefix:Street";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(".")
            };

            using (var reader = GetResourceReader("PurchaseOrdersManyNamespaces"))
            {
                var engine = new XPathStreamEngine(reader, from, selects, "prefix");
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("123 Maple Street"));
            }
        }

        [Test]
        public void Execute_FromElementWithManyNamespacesIgnoringDefaultNamespace_ValueCorrect()
        {
            var from = "//PurchaseOrder/Address/Street";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect(".")
            };

            using (var reader = GetResourceReader("PurchaseOrdersManyNamespaces"))
            {
                var engine = new XPathStreamIgnoreNamespaceEngine(reader, from, selects, "prefix");
                var result = engine.Execute();
                Assert.That((result.ElementAt(0) as IEnumerable<object>).ElementAt(0), Is.EqualTo("123 Maple Street"));
            }
        }
    }
}
