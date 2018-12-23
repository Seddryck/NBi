using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime
{
    public class TestInstance
    {
        public IDictionary<string, TestVariable> Variables { get; } = new Dictionary<string, TestVariable>();
    }
}
