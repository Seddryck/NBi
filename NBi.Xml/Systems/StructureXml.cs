using System;
using System.Xml.Serialization;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Metadata;

namespace NBi.Xml.Systems
{
    public class StructureXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("measure-group")]
        public string MeasureGroup { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override object Instantiate()
        {
            var cmd = new DiscoverCommand(ConnectionString ?? Default.ConnectionString);
            cmd.Perspective = Perspective;

            if (!string.IsNullOrEmpty(Path) && !Path.StartsWith("[Measures]"))
            {
                if (string.IsNullOrEmpty(MeasureGroup))
                    cmd.Path = Path;
                else
                    throw new Exception("If path is specified and not starting by '[Measures]' then MeasureGroup must not be specified");
            }
            else
            {
                if (!string.IsNullOrEmpty(MeasureGroup))
                    cmd.Path = string.Format("[Measures].[{0}]", MeasureGroup);         
                else
                    throw new Exception("If path is not specified or not starting by '[Measures]' then MeasureGroup must be specified");
            }
            

            return cmd;
        }
    }
}
