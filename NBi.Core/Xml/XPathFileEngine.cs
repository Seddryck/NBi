using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NBi.Extensibility;

namespace NBi.Core.Xml
{
    public class XPathFileEngine : XPathEngine
    {
        public string BasePath { get; }
        public IScalarResolver<string> ResolverPath { get; }

        public XPathFileEngine(IScalarResolver<string> resolverPath, string basePath, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
        {
            BasePath = basePath;
            ResolverPath = resolverPath;
        }

        public override IEnumerable<object> Execute()
        {
            var filePath = PathExtensions.CombineOrRoot(BasePath, string.Empty, ResolverPath.Execute());
            if (!File.Exists(filePath))
                throw new ExternalDependencyNotFoundException(filePath);

            var doc = XDocument.Load(filePath);
            return Execute(doc);
        }
    }
}
