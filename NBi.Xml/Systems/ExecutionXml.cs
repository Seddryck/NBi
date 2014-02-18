using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public class ExecutionXml : AbstractSystemUnderTestXml
    {       
        public virtual bool IsQuery()
        {
            return true;
        }
        
        [XmlElement(Type = typeof(QueryXml), ElementName = "query"),
        XmlElement(Type = typeof(AssemblyXml), ElementName = "assembly"),
        XmlElement(Type = typeof(ReportXml), ElementName = "report"),
        ]
        public virtual QueryableXml Item { get; set; }

        public override BaseItem BaseItem
        {
            get
            {
                return (BaseItem) Item;
            }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            return dico;
        }

        public override ICollection<string> GetAutoCategories()
        {
            return new string[] { "Execution" };
        }
       
    }
}
