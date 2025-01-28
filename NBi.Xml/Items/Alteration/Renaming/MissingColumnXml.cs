using NBi.Core.ResultSet.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Renaming;

public class MissingColumnXml
{
    [XmlAttribute("behavior")]
    [DefaultValue(MissingColumnBehavior.Failure)]
    public MissingColumnBehavior Behavior { get; set; } = MissingColumnBehavior.Failure;
}

public enum MissingColumnBehavior
{
    [XmlEnum("failure")]
    Failure = 0,
    [XmlEnum("skip")]
    Skip = 1,
}
