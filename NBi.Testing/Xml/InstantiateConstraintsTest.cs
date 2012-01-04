using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Xml
{
    [TestFixture]
    public class InstantiateConstraintsTest
    {
        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            _connectionString = "Data Source=.;Initial Catalog=NBi.Testing;Integrated Security=True";
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void Instantiate_FirstTest_IsOfTypeQueryParser()
        {
            var t = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>() { new QueryParserXml() }
            };

            var cs = t.Instantiate();

            Assert.That(cs[0], Is.InstanceOfType<NBi.NUnit.QueryParserConstraint>());
        }

        [Test]
        public void Instantiate_SecondTest_AsSeveralTypes()
        {
            var t = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>() 
                { 
                    new QueryParserXml(),
                    new QueryPerformanceXml()
                },
            };


            var cs = t.Instantiate();

            Assert.That(cs[0], Is.InstanceOfType<NBi.NUnit.QueryParserConstraint>());
            Assert.That(cs[1], Is.InstanceOfType<NBi.NUnit.QueryPerformanceConstraint>());
        }
    }
}
