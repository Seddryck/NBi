using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using System.Xml.Serialization;
using System.ComponentModel;

namespace NBi.Xml.Items.Alteration.Transform;

public class LightTransformXml : ITransformationInfo
{
    [XmlText()]
    public string Code { get; set; }

    [XmlAttribute("language")]
    [DefaultValue(LanguageType.CSharp)]
    public LanguageType Language { get; set; }

    [XmlAttribute("original-type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType OriginalType { get; set; }
}
