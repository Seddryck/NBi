using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Systems
{
    public class StructureXml: AbstractSystemUnderTestXml
    {
        [XmlElement(Type = typeof(PerspectiveXml), ElementName = "perspective"),
        XmlElement(Type = typeof(MeasureGroupXml), ElementName = "measure-group"),
        XmlElement(Type = typeof(MeasureXml), ElementName = "measure"),
        XmlElement(Type = typeof(DimensionXml), ElementName = "dimension"),
        XmlElement(Type = typeof(HierarchyXml), ElementName = "hierarchy"),
        XmlElement(Type = typeof(LevelXml), ElementName = "level"),
        XmlElement(Type = typeof(PerspectivesXml), ElementName = "perspectives"),
        XmlElement(Type = typeof(MeasureGroupsXml), ElementName = "measure-groups"),
        XmlElement(Type = typeof(MeasuresXml), ElementName = "measures"),
        XmlElement(Type = typeof(DimensionsXml), ElementName = "dimensions"),
        XmlElement(Type = typeof(HierarchiesXml), ElementName = "hierarchies"),
        XmlElement(Type = typeof(LevelsXml), ElementName = "levels"),
        XmlElement(Type = typeof(TableXml), ElementName = "table"),
        XmlElement(Type = typeof(ColumnXml), ElementName = "column")
        ]
        public AbstractItem Item { get; set; }

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
            values.Add("Structure");
            return values;
        }
    }
}
