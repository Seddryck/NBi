using System.Xml.Serialization;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class TestCaseXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("file")]
        public string Filename { get; set; }

        [XmlText]
        public string Sql { get; set; }

        protected internal string ReadSql()
        {
            //TODO read file
            return Sql;
        }

        public void Play(Constraint constraint)
        {
            Assert.That(ReadSql(), constraint);
        }

    }
}
