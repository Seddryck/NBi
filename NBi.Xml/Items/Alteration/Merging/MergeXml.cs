using NBi.Xml.Systems;
using NBi.Xml.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Merging;

public class MergeXml : AlterationXml
{
    [XmlElement("result-set")]
    public ResultSetSystemXml ResultSet { get; set; }
}
