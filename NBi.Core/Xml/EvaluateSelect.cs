using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Xml
{
    public class EvaluateSelect: ElementSelect
    {
        internal EvaluateSelect(string xpath)
            : base(xpath)
        {
        }
    }
}
