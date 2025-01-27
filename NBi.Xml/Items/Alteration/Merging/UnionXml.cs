using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Merging;

public class UnionXml : MergeXml
{
    [XmlAttribute("column-identity")]
    [DefaultValue(ColumnIdentity.Ordinal)]
    public ColumnIdentity ColumnIdentity { get; set; } = ColumnIdentity.Ordinal;
}
