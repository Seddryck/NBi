using NUnit.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed.Filter
{
    [Serializable]
    public class NBiNameFilter : SimpleNameFilter
    {
        public NBiNameFilter(string name)
            : base($@"NBi.NUnit.Runtime.TestSuite.{name}")
        { }
    }
}
