using NBi.Core.ResultSet.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Lookup;

public class MissingXml
{
    [XmlAttribute("behavior")]
    [DefaultValue(Behavior.Failure)]
    public Behavior Behavior { get; set; } = Behavior.Failure;

    [XmlText]
    public string DefaultValue { get; set; }
}

public enum Behavior
{
    [XmlEnum("failure")]
    Failure = 0,
    [XmlEnum("original-value")]
    OriginalValue = 1,
    [XmlEnum("default-value")]
    DefaultValue = 2,
    [XmlEnum("discard-row")]
    DiscardRow = 3
}
