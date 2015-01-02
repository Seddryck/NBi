using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{

    public class PerspectiveXml : AbstractItem
    {
        [XmlIgnore]
        public override string TypeName
        {
            get { return "perspective"; }
        }

        internal override ICollection<string> GetAutoCategories()
        {
            var values = new List<string>();
            values.Add(string.Format("Perspective '{0}'", Caption));
            values.Add("Perspectives");
            return values;
        }
    }
}
