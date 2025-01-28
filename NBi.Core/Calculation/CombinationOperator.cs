using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.Calculation;

public enum CombinationOperator
{
    [XmlEnum("or")]
    Or,
    [XmlEnum("xor")]
    XOr,
    [XmlEnum("and")]
    And,
}
