using NBi.Core.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Xml
{
    public class XPathEngineTest
    {
        protected StreamReader GetResourceReader()
        {
            // A Stream is needed to read the XML document.
            var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Core.Resources.PurchaseOrders.xml");
            var reader = new StreamReader(stream);
            return reader;
        }

        [Test]
        public void Execute_Example_ColumnCount()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder/PurchaseOrderNumber" }
                , new Select() { Attribute = "PartNumber", Path="."}
                , new Select() { Path="//PurchaseOrder/Address[@Type=\"Shiping\"]/City" }
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Columns.Count, Is.EqualTo(3));
            }
        }

        [Test]
        [TestCase("//PurchaseOrder/Items/Item", 5)]
        [TestCase("//PurchaseOrder", 3)]
        public void Execute_Example_RowCount(string from, int rowCount)
        {
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder/PurchaseOrderNumber" }
                , new Select() { Attribute = "PartNumber", Path="."}
                , new Select() { Path="//PurchaseOrder/Address[@Type=\"Shiping\"]/City" }
            };

            var engine = new XPathEngine(from, selects);
            var result = engine.Execute(GetResourceReader());

            Assert.That(result.Rows.Count, Is.EqualTo(rowCount));
        }

        [Test]
        public void Execute_FromElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item/ProductName";
            var selects = new List<Select>()
            {
                new Select() { Path="."}
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("Lawnmower"));
            }
        }

        [Test]
        public void Execute_FromAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Attribute = "PartNumber", Path="."}
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("872-AA"));
            }
        }

        [Test]
        public void Execute_ChildElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder/Items/Item/ProductName"}
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("Lawnmower"));
            }
        }

        [Test]
        public void Execute_ChildAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder/Items/Item", Attribute = "PartNumber" }
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("872-AA"));
            }
        }

        [Test]
        public void Execute_ParentElement_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder"}
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.StringStarting("Ellen Adams"));
                Assert.That(result.Rows[0].ItemArray[0], Is.StringContaining("Maple Street"));
            }
        }

        [Test]
        public void Execute_ParentAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder", Attribute = "PurchaseOrderNumber" }
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("99503"));
            }
        }

        [Test]
        public void Execute_MissingElement_Null()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder/Missing"}
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo(DBNull.Value));
            }
        }

        [Test]
        public void Execute_MissingAttribute_ValueCorrect()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="//PurchaseOrder", Attribute = "Missing" }
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo(DBNull.Value));
            }
        }
    }
}
