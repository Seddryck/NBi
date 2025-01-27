using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Extension;

public class ExtendXml : AlterationXml
{
    [XmlAttribute("identifier")]
    public string IdentifierSerializer { get; set; }
    [XmlIgnore]
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }

    [XmlElement("script")]
    public ScriptXml Script { get; set; }
}
