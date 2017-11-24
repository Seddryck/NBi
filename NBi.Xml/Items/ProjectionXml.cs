using NBi.Core.Scalar.Projection;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class ProjectionXml
    {
        [XmlAttribute("type")]
        public ProjectionType Type { get; set; }

        [XmlElement("resultSet")]
        public ResultSetSystemXml ResultSet { get; set; }
    }
}
