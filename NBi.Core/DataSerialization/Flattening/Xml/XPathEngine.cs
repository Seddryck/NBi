using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NBi.Core.DataSerialization;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;

namespace NBi.Core.DataSerialization.Flattening.Xml
{
    public class XPathEngine : PathFlattenizer, IDataSerializationFlattenizer
    {
        public string DefaultNamespacePrefix { get; }
        public bool IsIgnoreNamespace { get; }

        public XPathEngine(IScalarResolver<string> from, IEnumerable<IPathSelect> selects, string defaultNamespacePrefix, bool isIgnoreNamespace)
            : base(from, selects)
            => (DefaultNamespacePrefix, IsIgnoreNamespace) = (defaultNamespacePrefix, isIgnoreNamespace);

        public override IEnumerable<object> Execute(TextReader textReader)
        {
            var xmlReader = CreateReader(textReader, IsIgnoreNamespace);
            var items = XDocument.Load(xmlReader);
            var nsMgr = new XmlNamespaceManager(new NameTable());
            if (!string.IsNullOrEmpty(DefaultNamespacePrefix))
                nsMgr.AddNamespace(DefaultNamespacePrefix, items.Root?.GetDefaultNamespace().NamespaceName ?? string.Empty);

            var xmlNamespaces = ((IEnumerable<object>)items.XPathEvaluate(@"//namespace::*[not(. = ../../namespace::*)]")).Cast<XAttribute>();
            foreach (var namespaceNode in xmlNamespaces)
                if (namespaceNode.Name.LocalName != "xmlns")
                    nsMgr.AddNamespace(namespaceNode.Name.LocalName, namespaceNode.Value);

            var result = from item in items.XPathSelectElements(From.Execute() ?? throw new NullReferenceException(), nsMgr)
                         select GetObj(item, nsMgr);

            return result;
        }

        private object GetObj(XElement x, IXmlNamespaceResolver ns)
        {
            var obj = new List<object>();
            obj.AddRange(BuildXPaths(x, ns, Selects).ToArray());
            return obj;
        }

        protected internal IEnumerable<object> BuildXPaths(XElement item, IXmlNamespaceResolver ns, IEnumerable<IPathSelect> selects)
        {
            foreach (var select in selects)
                if (select is AttributeSelect attributeSelect)
                {
                    yield return
                    (
                        (
                            item.XPathSelectElement(attributeSelect.Path.Execute()!, ns)
                            ?? new XElement("null", "(null)")
                        ).Attribute(attributeSelect.Attribute)
                        ?? new XAttribute("null", "(null)")
                    ).Value;
                }
                else if (select is EvaluateSelect evaluateSelect)
                {
                    yield return
                    (
                        item.XPathEvaluate(evaluateSelect.Path.Execute()!, ns)
                        ?? new XElement("null", "(null)")
                    );
                }
                else
                    yield return
                    (
                        item.XPathSelectElement(select!.Path.Execute()!, ns)
                        ?? new XElement("null", "(null)")
                    ).Value;
        }

        protected XmlReader CreateReader(TextReader textReader, bool isRemoveDefaultNamespace)
        {
            var settings = new XmlReaderSettings();
            if (isRemoveDefaultNamespace)
                return new XmlIgnoreNamespaceReader(textReader, settings);
            else
                return XmlReader.Create(textReader, settings);
        }
    }
}
