using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class ReferenceExistsXml : AbstractConstraintXml
    {
        [XmlElement("column-mapping")]
        public List<ColumnMappingXml> Mappings { get; set; }

        [XmlElement("resultSet")]
        public ResultSetSystemXml ResultSet { get; set; }

        public ReferenceExistsXml()
        {
            Mappings = new List<ColumnMappingXml>();
        }

        [XmlIgnore()]
        public override DefaultXml Default
        {
            get { return base.Default; }
            set
            {
                base.Default = value;
                if (ResultSet != null)
                    ResultSet.Default = value;
            }
        }

    }
}
