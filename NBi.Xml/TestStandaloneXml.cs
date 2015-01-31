using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml
{
    [XmlRoot(ElementName = "test", Namespace = "")]
    public class TestStandaloneXml : TestXml
    {

        public TestXml ToTest()
        {
            var test = new TestXml();
            test.Categories = this.Categories;
            test.Cleanup = this.Cleanup;
            test.Condition = this.Condition;
            test.Constraints = this.Constraints;
            test.Content = this.Content;
            test.DescriptionElement = this.DescriptionElement;
            test.Edition = this.Edition;
            test.IgnoreElement = this.IgnoreElement;
            test.Name = this.Name;
            test.Setup = this.Setup;
            test.Systems = this.Systems;
            test.Timeout = this.Timeout;
            test.Traits = this.Traits;
            test.UniqueIdentifier = this.UniqueIdentifier;

            return test;
        }
    }
}
