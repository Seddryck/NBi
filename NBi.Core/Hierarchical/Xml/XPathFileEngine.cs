using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NBi.Core.Hierarchical.Xml
{
    public class XPathFileEngine : XPathEngine
    {
        public string BasePath { get; }
        public IScalarResolver<string> ResolverPath { get; }
        
        public XPathFileEngine(IScalarResolver<string> resolverPath, string basePath, string from, IEnumerable<AbstractSelect> selects, string defaultNamespacePrefix, bool isRemoveDefaultNamespace)
            : base(from, selects, defaultNamespacePrefix, isRemoveDefaultNamespace)
            => (BasePath, ResolverPath) = (basePath, resolverPath);

        public override IEnumerable<object> Execute()
        {
            var filePath = EnsureFileExist();

            using (var xmlreader = CreateReader(filePath, IsIgnoreNamespace))
            {
                var doc = XDocument.Load(xmlreader);
                return Execute(doc);
            }
        }

        protected virtual string EnsureFileExist()
        {
            var filePath = PathExtensions.CombineOrRoot(BasePath, string.Empty, ResolverPath.Execute());
            if (!File.Exists(filePath))
                throw new ExternalDependencyNotFoundException(filePath);
            return filePath;
        }

        private XmlReader CreateReader(string filePath, bool isRemoveDefaultNamespace)
        {
            var settings = new XmlReaderSettings();
            var streamReader = GetTextReader(filePath);
            if (isRemoveDefaultNamespace)
                return new XmlIgnoreNamespaceReader(streamReader, settings);
            else
                return XmlReader.Create(streamReader, settings);
        }

        protected virtual TextReader GetTextReader(string filePath)
            => new StreamReader(filePath);
    }
}
