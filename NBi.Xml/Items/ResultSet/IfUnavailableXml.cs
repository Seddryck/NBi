using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet;

public class IfUnavailableXml
{
    [XmlElement("result-set")]
    public virtual ResultSetSystemXml ResultSet { get; set; }
}
