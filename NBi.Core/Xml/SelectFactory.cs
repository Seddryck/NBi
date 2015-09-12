using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Xml
{
    public class SelectFactory
    {
        public AbstractSelect Instantiate(string xpath, string attribute)
        {
            if (String.IsNullOrEmpty(attribute))
                return new ElementSelect(xpath);
            else
                return new AttributeSelect(xpath, attribute);

        }
    }
}
