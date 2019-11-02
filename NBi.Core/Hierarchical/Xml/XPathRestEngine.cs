using NBi.Core.Api.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBi.Core.Hierarchical.Xml
{
    public class XPathRestEngine : XPathEngine
    {
        public RestEngine Rest { get; }

        public XPathRestEngine(RestEngine rest, string from, IEnumerable<AbstractSelect> selects, string defaultNamespacePrefix)
            : base(from, selects, defaultNamespacePrefix, false)
            => Rest = rest;

        public override IEnumerable<object> Execute()
        {
            var xmlText = Rest.Execute();
            using (var xmlreader = CreateReader(xmlText, IsIgnoreNamespace))
            {
                var doc = XDocument.Load(xmlreader);
                return Execute(doc);
            }
        }

        protected override TextReader GetTextReader(string text)
            => new StringReader(text);
    }
}
