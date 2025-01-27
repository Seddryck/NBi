using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables;

public class ScriptXml
{
    [XmlAttribute("language")]
    [DefaultValue(LanguageType.CSharp)]
    public LanguageType Language { get; set; }

    [XmlText]
    public string Code { get; set; }
}
