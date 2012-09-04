using System;
using System.Xml.Serialization;
using NBi.Core.Analysis;

namespace NBi.Xml.Systems
{
    public class StructureXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("target")]
        public DiscoverTarget Target { get; set;}
        
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
            Validate();
            
            var cmd = new DiscoverCommand(Target, ConnectionString ?? Default.ConnectionString);
            cmd.Perspective = Perspective;
            cmd.MeasureGroup = MeasureGroup;
            cmd.Path = Path;          

            return cmd;
        }

        public virtual void Validate()
        {
            switch (Target)
            {
                case DiscoverTarget.Perspectives:
                    if (!string.IsNullOrEmpty(Path))
                        throw new Exception("If target is 'perspectives' then path attribute must be missing");
                    if (!string.IsNullOrEmpty(Perspective))
                        throw new Exception("If target is 'perspectives' then perspective attribute must be missing");
                    break;
                case DiscoverTarget.MeasureGroups:
                    if (!string.IsNullOrEmpty(Path))
                        throw new Exception("If target is 'measure-groups' then path attribute must be missing");
                    if (!string.IsNullOrEmpty(MeasureGroup))
                        throw new Exception("If target is 'measure-groups' then measure-group attribute must be missing");
                    break;
                case DiscoverTarget.Measures:
                    if (!string.IsNullOrEmpty(Path))
                        throw new Exception("If target is 'measures' then path attribute must be missing or equal to '[Measures]'");
                    break;
                case DiscoverTarget.Dimensions:
                    if (!string.IsNullOrEmpty(Path))
                        throw new Exception("If target is 'dimensions' then path attribute must be missing");
                    break;
                case DiscoverTarget.Hierarchies:
                    if (string.IsNullOrEmpty(Path))
                        throw new Exception("If target is 'hierarchies' then path attribute must be filled");
                    break;
                case DiscoverTarget.Levels:
                    if (string.IsNullOrEmpty(Path))
                        throw new Exception("If target is 'levels' then path attribute must be filled");
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(MeasureGroup) && Target!=DiscoverTarget.Measures)
                throw new Exception("If target is not 'measures' then measure-group attribute must be missing");

            if (string.IsNullOrEmpty(Perspective) && Target != DiscoverTarget.Perspectives)
                throw new Exception("If target is not 'perspectives' then perspective attribute must be filled");

        }
    }
}
