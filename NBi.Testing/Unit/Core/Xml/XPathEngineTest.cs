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
                new Select() { Path="../../PurchaseOrderNumber" }
                , new Select() { Attribute = "PartNumber", Path="."}
                , new Select() { Path="../../Address[@Type=\"Shiping\"]/City" }
            };

            var engine = new XPathEngine(from, selects);
            using (var reader = GetResourceReader())
            {
                var result = engine.Execute(reader);
                Assert.That(result.Columns, Is.EqualTo(3));
            }
        }

        [Test]
        public void Execute_Example_RowCount()
        {
            var from = "//PurchaseOrder/Items/Item";
            var selects = new List<Select>()
            {
                new Select() { Path="../../PurchaseOrderNumber" }
                , new Select() { Attribute = "PartNumber"}
                , new Select() { Path="../../Address[@Type=\"Shiping\"]/City" }
            };

            var engine = new XPathEngine(from, selects);
            var result = engine.Execute(GetResourceReader());

            Assert.That(result.Rows, Is.EqualTo(5));
        }
    }
}
