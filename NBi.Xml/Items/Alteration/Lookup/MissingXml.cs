using NBi.Core.ResultSet.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Lookup
{
    public class MissingXml
    {
        [XmlAttribute("behavior")]
        [DefaultValue(Behavior.Maintain)]
        public Behavior Behavior { get; set; }

        [XmlText]
        [DefaultValue("(null)")]
        public string DefaultValue { get; set; }

        public MissingXml()
        {
            Behavior = Behavior.Maintain;
            DefaultValue = "(null)";
        }

        protected static readonly MissingXml @default = new MissingXml();
        [XmlIgnore]
        public static MissingXml Default { get => @default; }
    }
}
