using NBi.Core.ResultSet;
using NBi.Model.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Model.Xml.Items.Alteration.Projection;

public abstract class AbstractProjectionXml : AlterationXml
{
    [XmlElement("column")]
    public List<ColumnDefinitionLightXml> Columns { get; set; } = new List<ColumnDefinitionLightXml>();
}
