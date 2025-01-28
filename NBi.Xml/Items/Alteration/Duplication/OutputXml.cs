using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Extensibility;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Duplication;

public class OutputXml
{
    [XmlAttribute("identifier")]
    public string IdentifierSerializer { get; set; }
    [XmlIgnore]
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }

    [XmlAttribute("class")]
    public OutputClass Class { get; set; }

    [XmlElement("script")]
    public ScriptXml Script { get; set; }

    [XmlElement("value")]
    public string Value { get; set; }
}
