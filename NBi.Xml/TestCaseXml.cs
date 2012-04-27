using System.IO;
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
        public string InlineQuery { get; set; }

        public string Query
        {
            get
            {
                //if Sql is specified then return it
                if (!string.IsNullOrEmpty(InlineQuery))
                    return InlineQuery;

                //Else read the file's content and 
                var query = File.ReadAllText(Filename);
                return query;
            }
        }

        public void Play(Constraint constraint)
        {
            Assert.That(Query, constraint);
        }

    }
}
