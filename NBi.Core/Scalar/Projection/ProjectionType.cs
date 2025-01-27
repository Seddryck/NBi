using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.Scalar.Projection;

public enum ProjectionType
{
    [XmlEnum("row-count")]
    RowCount = 1
}
