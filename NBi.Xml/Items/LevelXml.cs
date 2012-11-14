using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Items
{
    public class LevelXml : HierarchyXml
    {
        [XmlAttribute("hierarchy")]
        public string Hierarchy { get; set; }

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }

        [XmlIgnore]
        protected override string  ParentPath { get { return string.Format("{0}.[{1}]",base.ParentPath, Hierarchy);}}

        [XmlIgnore]
        protected override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }
    }
}
