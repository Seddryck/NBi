using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation
{
    public class DefaultInstanceArgs : IInstanceArgs
    {
        public IEnumerable<string> Categories { get; } = [];
        public IDictionary<string, string> Traits { get; } = new Dictionary<string, string>();
    }
}
