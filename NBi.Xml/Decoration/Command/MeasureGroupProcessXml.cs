using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public class MeasureGroupProcessXml : ProcessableAbstractXml
    {
        [XmlAttribute("cube")]
        public string Cube { get; set; }

        [XmlAttribute("partition")]
        public string Partition { get; set; }
    }
}
