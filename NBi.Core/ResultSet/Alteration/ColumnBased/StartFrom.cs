using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet.Alteration.ColumnBased.Strategy
{
    public enum StartFrom
    {
        [XmlEnum("first")]
        First = 0,
        [XmlEnum("last")]
        Last = 1,
    }
}
