using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Evaluate;

namespace NBi.Xml.Items.Calculation
{
    public class VariableXml : IColumnVariable
    {
        [XmlAttribute("column-index")]
        public int Column { get; set; }

        [XmlText]
        public string Name { get; set; }

    }
}
