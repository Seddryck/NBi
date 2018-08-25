using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using System.Xml.Serialization;
using System.ComponentModel;

namespace NBi.Xml.Items.Alteration.Transform
{
    public class TransformXml : LightTransformXml
    {
        [XmlIgnore]
        [Obsolete("Use Identifier in place of ColumnOrdinal")]
        public int ColumnOrdinal
        {
            get => throw new InvalidOperationException();
            set => Identifier = new ColumnIdentifierFactory().Instantiate($"#{value}");
        }

        [XmlAttribute("column")]
        public string IdentifierSerializer { get; set; }
        [XmlIgnore]
        public IColumnIdentifier Identifier
        {
            get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
            set => IdentifierSerializer = value.Label;
        }
    }
}
