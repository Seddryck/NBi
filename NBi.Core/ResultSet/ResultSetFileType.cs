using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet
{
    public enum ResultSetFileType
    {
        [XmlEnum(Name = "csv")]
        Csv = 0,
        [XmlEnum(Name = "xml")]
        Xml = 1
    }
}
