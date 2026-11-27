using NBi.Model.Xml.Systems;
using NBi.Model.Xml.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Model.Xml.Items.Alteration.Merging;

public class MergeXml : AlterationXml
{
    [XmlElement("result-set")]
    public ResultSetSystemXml ResultSet { get; set; }
}
