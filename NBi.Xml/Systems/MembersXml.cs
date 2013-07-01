using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Systems
{
    public class MembersXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("children-of")]
        public string ChildrenOf { get; set; }

        [XmlElement(Type = typeof(DimensionXml), ElementName = "dimension"),
        XmlElement(Type = typeof(HierarchyXml), ElementName = "hierarchy"),
        XmlElement(Type = typeof(LevelXml), ElementName = "level")
        ]
        public AbstractMembersItem Item { get; set; }

        [XmlElement("exclude")]
        public ExcludeXml Exclude { get; set; }

        public override BaseItem BaseItem
        {
            get
            {
                return Item;
            }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            return Item.GetRegexMatch();
        }

        public override ICollection<string> GetAutoCategories()
        {
            var values = Item.GetAutoCategories();
            values.Add("Members");
            return values;
        }
    }
}
