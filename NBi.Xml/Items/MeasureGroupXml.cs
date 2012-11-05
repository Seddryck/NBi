using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Items
{
    public class MeasureGroupXml : PerspectiveXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }
    }
}
