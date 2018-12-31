using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet.Lookup
{
    public enum Behavior
    {
        [XmlEnum("maintain")]
        Maintain = 0,
        [XmlEnum("discard")]
        Discard = 1
    }
}
