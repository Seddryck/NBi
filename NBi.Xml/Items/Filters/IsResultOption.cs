using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Filters;

public enum IsResultOption
{
    [XmlEnum(Name = "unspecified")]
    Unspecified = 0,
    [XmlEnum(Name = "yes")]
    Yes = 1,
    [XmlEnum(Name = "no")]
    No = 2,
}
