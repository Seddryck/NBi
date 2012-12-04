using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

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


        [XmlIgnore]
        public override string TypeName
        {
            get { return "level"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:hierarchy", Hierarchy);
            return dico;
        }
    }
}
