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

namespace NBi.Core.Hierarchical.Json
{
    public class JsonPathUrlEngine : JsonPathEngine
    {
        public IScalarResolver<string> Url { get; }

        public JsonPathUrlEngine(IScalarResolver<string> url, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
            => (Url) = (url);

        public override IEnumerable<object> Execute()
        {
            var url = Url.Execute();
            var jsonText = GetRemoteContent(url);
            var json = JToken.Parse(jsonText);
            return Execute(json);
        }

        protected virtual string GetRemoteContent(string url)
        {
            using (var webClient = new WebClient())
                return webClient.DownloadString(url);
        }
    }
}
