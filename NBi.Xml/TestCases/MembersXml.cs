using System;
using System.Xml.Serialization;
using NBi.Core.Analysis;

namespace NBi.Xml.TestCases
{
    public class MembersXml : AbstractTestCaseXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlAttribute("hierarchy")]
        public string Hierarchy { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override object Instantiate()
        {
            var extractor = new MemberAdomdExtractor(ConnectionString);
            MemberList list = null;
            if (!string.IsNullOrEmpty(Level))
                list = extractor.GetMembersByLevel(Perspective, Level);
            else if (!string.IsNullOrEmpty(Hierarchy))
                list = extractor.GetMembersByHierarchy(Perspective, Hierarchy);
            else
                throw new Exception();

            return list;
        }
    }
}
