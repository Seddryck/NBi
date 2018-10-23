using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class LookupExistsXml : AbstractConstraintXml
    {
        [XmlAttribute("reverse")]
        [DefaultValue(false)]
        public bool IsReversed { get; set; }

        [XmlElement("join")]
        public JoinXml Join { get; set; }

        [XmlElement("resultSet")]
        public ResultSetSystemXml ResultSet { get; set; }

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
