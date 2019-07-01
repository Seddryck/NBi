using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Renaming
{
    public class RenamingXml
    {
        [XmlAttribute("identifier")]
        public string IdentifierSerializer { get; set; }
        [XmlIgnore]
        public IColumnIdentifier Identifier
        {
            get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
            set => IdentifierSerializer = value.Label;
        }

        [XmlAttribute("new-name")]
        public string NewNameSerializer { get; set; }
        [XmlIgnore]
        public IColumnIdentifier NewName
        {
            get => new ColumnIdentifierFactory().Instantiate(NewNameSerializer);
            set => NewNameSerializer = value.Label;
        }
    }
}
