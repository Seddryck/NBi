using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Filters;


public enum ParameterDirectionOption
{
    [XmlEnum(Name = "unspecified")]
    Unspecified = 0,
    [XmlEnum(Name = "in")]
    In = 1,
    [XmlEnum(Name = "out")]
    Out = 2,
    [XmlEnum(Name = "in-out")]
    InOut = 3
}
