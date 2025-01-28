using NBi.Core.ResultSet;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Renaming;

public class RenamingXml : AlterationXml
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
    public string NewName { get; set; }

    [XmlElement("missing")]
    public MissingColumnXml Missing { get; set; } = new MissingColumnXml { Behavior = MissingColumnBehavior.Failure };

    [XmlIgnore()]
    public bool MissingSpecified
    {
        get => Missing.Behavior != MissingColumnBehavior.Failure;
        set { }
    }
}
