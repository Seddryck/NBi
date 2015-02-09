using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;

namespace NBi.Xml.Items.Validate
{
    public class ExpressionXml : IColumnExpression
    {
        [XmlAttribute("column-index")]
        public int Column { get; set; }

        [XmlText]
        public string Value { get; set; }

        [XmlAttribute("type")]
        public ColumnType Type  { get; set; }

        [XmlAttribute("tolerance")]
        public string Tolerance { get; set; }
    }
}
