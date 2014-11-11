using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Substitutions
{
    public class FakeXml : AbstractSubstitutionXml
    {
        private string code;

        [XmlElement("code")]
        public string Code
        {
            get
            { return code; }
            set
            { code = value.Trim(); }
        }
    }
}
