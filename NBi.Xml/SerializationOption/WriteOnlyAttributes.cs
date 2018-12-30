using NBi.Xml.Items;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.SerializationOption
{
    public class WriteOnlyAttributes : ReadWriteAttributes
    {

        public WriteOnlyAttributes()
            : base()
        {
        }

        protected override void AdditionalBuild()
        {
            var attrs = new XmlAttributes() { XmlIgnore = true };
            Add(typeof(QueryXml), "InlineQuery", attrs);

            attrs = new XmlAttributes() { XmlIgnore = false };
            attrs.XmlAnyElements.Add(new XmlAnyElementAttribute());
            Add(typeof(QueryXml), "InlineQueryWrite", attrs);
        }
    }
}
