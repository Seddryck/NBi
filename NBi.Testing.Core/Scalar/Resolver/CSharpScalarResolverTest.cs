using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NBi.Testing.Core.Scalar.Resolver
{
    public class CSharpScalarResolverTest
    {
        [Test]
        public void Instantiate_GetValueObject_CorrectComputation()
        {
            var args = new CSharpScalarResolverArgs("DateTime.Now.Year");
            var resolver = new CSharpScalarResolver<object>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo(DateTime.Now.Year));
        }

        [Test]
        public void Instantiate_GetValueInt_CorrectComputation()
        {
            var args = new CSharpScalarResolverArgs("DateTime.Now.Year");
            var resolver = new CSharpScalarResolver<int>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo(DateTime.Now.Year));
        }

        [Test]
        public void Instantiate_GetValueXmlLinq_CorrectComputation()
        {
            var xmlPath = new Uri(FileOnDisk.CreatePhysicalFile("PurchaseOrders.xml", "NBi.Testing.Core.Resources.PurchaseOrders.xml")).AbsolutePath;
            string xmlDoc = string.Format(@"XDocument.Load(""{0}"").Root.Name.ToString()", xmlPath);

            var args = new CSharpScalarResolverArgs(xmlDoc);
            var resolver = new CSharpScalarResolver<string>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo(XDocument.Load(xmlPath).Root.Name.ToString()));
        }

        [Test]
        public void Instantiate_GetValueXmlXpath_CorrectComputation()
        {
            var xPath = "./PurchaseOrders/PurchaseOrder/Address/Name";
            var xmlPath = new Uri(FileOnDisk.CreatePhysicalFile("PurchaseOrders.xml", "NBi.Testing.Core.Resources.PurchaseOrders.xml")).AbsolutePath;
            string xmlDoc = string.Format(@"XDocument.Load(""{0}"").XPathSelectElement(""{1}"").Value.ToString()", xmlPath, xPath);

            var args = new CSharpScalarResolverArgs(xmlDoc);
            var resolver = new CSharpScalarResolver<string>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo(XDocument.Load(xmlPath).XPathSelectElement(xPath).Value.ToString()));
        }
    }
}
