using NBi.Core.ResultSet;
using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Mutation
{
    public abstract class FilteringColumnXml : AlterationItemXml
    {
        [XmlElement("column")]
        public List<ColumnDefinitionLightXml> Columns { get; set; }

        public FilteringColumnXml()
        {
            Columns = new List<ColumnDefinitionLightXml>();
        }
    }
}
