using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class OrderedXml : AbstractConstraintXml
    {
        [XmlAttribute("descending")]
        [DefaultValue(false)]
        public bool Descending { get; set; }

        [XmlAttribute("rule")]
        public Order Rule { get; set; }

        [XmlArray("rule-definition"),
        XmlArrayItem(Type = typeof(string), ElementName = "item")
        ]
        public List<object> Definition { get; set; }

        /// <summary>
        /// This property is there to control the serialization of the Definition object. If there is no definition (or an empty one), the field will not be part the output.
        /// </summary>
        [XmlIgnore]
        public bool DefinitionSpecified
        {
            get
            {
                return !(Definition == null || Definition.Count == 0);
            }
            set
            {
                return;
            }
        }

        public OrderedXml()
        {
            Definition = new List<object>();
        }

        public enum Order
        {
            [XmlEnum(Name = "alphabetical")]
            Alphabetical = 0,
            [XmlEnum(Name = "chronological")]
            Chronological = 1,
            [XmlEnum(Name = "numerical")]
            Numerical = 2,
            [XmlEnum(Name = "specific")]
            Specific = 3
        }

    }
}
