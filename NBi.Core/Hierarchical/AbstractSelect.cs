using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Hierarchical
{
    public class AbstractSelect
    {
        public string Path { get; }

        protected AbstractSelect(string path)
            => Path = path;
    }
}
