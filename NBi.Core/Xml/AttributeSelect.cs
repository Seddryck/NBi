using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Xml
{
    public class AttributeSelect: ElementSelect
    {
        internal AttributeSelect(string xpath, string attribute)
            : base(xpath)
        {
            this.Attribute = attribute;
        }

        public string Attribute { get; private set; }
    }
}
