using System.Xml.Serialization;
using NBi.Core.Analysis.Metadata;

namespace NBi.Xml.Systems
{
    public class StructureXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override object Instantiate()
        {
            var cmd = new MetadataQuery();
            cmd.ConnectionString = ConnectionString;
            cmd.Perspective = Perspective;
            cmd.Path = Path;

            return cmd;
        }
    }
}
