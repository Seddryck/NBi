using System;
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
    }
}
