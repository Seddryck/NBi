using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening.Json
{
    public class JsonPathArgs : IFlattenizerArgs
    {
        public string From { get; set; }
        public IEnumerable<IPathSelect> Selects { get; set; } = new List<IPathSelect>();
        public JsonPathArgs() { }
    }
}
