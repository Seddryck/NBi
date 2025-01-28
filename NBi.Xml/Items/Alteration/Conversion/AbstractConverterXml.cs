using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Conversion;

public abstract class AbstractConverterXml
{
    [XmlAttribute("culture")]
    [DefaultValue("")]
    public string Culture { get; set; }

    [XmlAttribute("default-value")]
    [DefaultValue("(null)")]
    public string DefaultValue { get; set; }

    [XmlIgnore]
    public abstract string From { get; }
    [XmlIgnore]
    public abstract string To { get; }
}
