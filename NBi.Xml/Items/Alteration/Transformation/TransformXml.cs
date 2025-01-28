using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using System.Xml.Serialization;
using System.ComponentModel;
using NBi.Extensibility;

namespace NBi.Xml.Items.Alteration.Transform;

public class TransformXml : AlterationXml, ITransformationInfo
{
    [XmlText()]
    public string Code { get; set; }

    [XmlAttribute("language")]
    [DefaultValue(LanguageType.CSharp)]
    public LanguageType Language { get; set; }

    [XmlAttribute("original-type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType OriginalType { get; set; }

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
