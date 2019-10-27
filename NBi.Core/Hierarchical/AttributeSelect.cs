using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Hierarchical
{
    public class AttributeSelect: ElementSelect
    {
        public string Attribute { get; }

        internal AttributeSelect(string path, string attribute)
            : base(path)
            => Attribute = attribute;
    }
}
