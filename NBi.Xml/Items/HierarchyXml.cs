using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class HierarchyXml : DimensionXml
    {
        [XmlAttribute("dimension")]
        public string Dimension { get; set; }

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }

        [XmlIgnore]
        protected virtual string ParentPath { get { return string.Format("[{0}]", Dimension); } }
        [XmlIgnore]
        protected  override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

        [XmlIgnore]
        public override string TypeName
        {
            get { return "hierarchy"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:dimension", Dimension);
            return dico;
        }

        internal override ICollection<string> GetAutoCategories()
        {
            var values = new List<string>();
            if (!string.IsNullOrEmpty(Perspective))
                values.Add(string.Format("Perspective '{0}'", Perspective));
            if (!string.IsNullOrEmpty(Dimension))
                values.Add(string.Format("Dimension '{0}'", Dimension));
            values.Add(string.Format("Hierarchy '{0}'", Caption));
            values.Add("Hierarchies");
            return values;
        }
    }
}
