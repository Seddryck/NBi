using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Duplication
{
    public class OutputXml
    {
        [XmlAttribute("name")]
        public string NameSerializer { get; set; }
        [XmlIgnore]
        public IColumnIdentifier Identifier
        {
            get => new ColumnIdentifierFactory().Instantiate(NameSerializer);
            set => NameSerializer = value.Label;
        }

        [XmlAttribute("value")]
        public OutputValue Value { get; set; }
    }
}
