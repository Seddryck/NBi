using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.Transformation;

public enum LanguageType
{
    [XmlEnum(Name = "c-sharp")]
    CSharp =0,
    [XmlEnum(Name = "ncalc")]
    NCalc = 1,
    [XmlEnum(Name = "format")]
    Format = 2,
    [XmlEnum(Name = "native")]
    Native = 3
}
