using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration
{
    public class DecorationStandaloneXml : DecorationXml
    {
        public DecorationStandaloneXml()
        { }

        public DecorationStandaloneXml(DecorationXml full)
        {
            Commands = full.Commands;
        }
    }

    [XmlRoot(ElementName = "setup", Namespace = "")]
    public class SetupStandaloneXml : DecorationStandaloneXml { }
}
