using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Xml
{
    public class AbstractSelect
    {
        protected AbstractSelect(string xpath)
        {
            this.Path = xpath;
        }

        public string Path { get; private set; }
    }
}
