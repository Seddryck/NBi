using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Grouping
{
    public class GroupByXml
    {
        [XmlElement("column")]
        public List<ColumnDefinitionLightXml> Columns { get; set; }
    }
}
