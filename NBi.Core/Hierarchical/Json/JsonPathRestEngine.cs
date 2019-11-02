using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using NBi.Core.Api.Rest;

namespace NBi.Core.Hierarchical.Json
{
    public class JsonPathRestEngine : JsonPathEngine
    {
        public RestEngine Rest { get; }

        public JsonPathRestEngine(RestEngine rest, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
            => (Rest) = (rest);

        public override IEnumerable<object> Execute()
        {
            var jsonText = Rest.Execute();
            var json = JToken.Parse(jsonText);
            return Execute(json);
        }
    }
}
