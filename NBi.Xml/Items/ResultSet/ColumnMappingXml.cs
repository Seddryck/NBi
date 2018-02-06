using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet
{
    public class ColumnMappingXml
    {
        [XmlAttribute("child")]
        public string Child { get; set; }
        [XmlAttribute("parent")]
        public string Parent { get; set; }
        [XmlAttribute("type")]
        [DefaultValue(ColumnType.Text)]
        public ColumnType Type { get; set; }
    }
}
