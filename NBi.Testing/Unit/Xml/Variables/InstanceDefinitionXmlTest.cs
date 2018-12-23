using NBi.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Xml.Variables;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using NBi.Xml.Items;
using System.Reflection;

namespace NBi.Testing.Unit.Xml.Variables
{
    public class InstanceDefinitionXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            var manager = new XmlManager();

            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.Variables.InstanceDefinitionXmlTestSuite.xml"))
                using (StreamReader reader = new StreamReader(stream))
                    manager.Read(reader);

            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_TestSuiteLoaded()
        {
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Instance, Is.Not.Null);
        }


    }
}
