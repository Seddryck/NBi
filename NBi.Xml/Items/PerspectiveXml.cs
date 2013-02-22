using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{

    public class PerspectiveXml : AbstractItem
    {

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }

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
