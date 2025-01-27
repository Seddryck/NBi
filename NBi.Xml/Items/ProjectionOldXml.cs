using NBi.Core.Scalar.Projection;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class ProjectionOldXml
{
    [XmlAttribute("type")]
    public ProjectionType Type { get; set; }

    [XmlElement("result-set")]
    public ResultSetSystemXml ResultSet { get; set; }

    [Obsolete("Replaced by result-set")]
    [XmlIgnore()]
    public ResultSetSystemXml ResultSetOld
    {
        get => ResultSet;
        set { ResultSet = value; }
    }
}
