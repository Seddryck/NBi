using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{

    public class PerspectivesXml : AbstractItem
    {
        [XmlIgnore]
        public override string TypeName
        {
            get { return "perspectives"; }
        }

        internal override ICollection<string> GetAutoCategories()
        {
            var values = new List<string>();
            values.Add("Perspectives");
            return values;
        }
    }
}
