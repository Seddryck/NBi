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
using NBi.Core.Hierarchical;
using NBi.Core.ResultSet;

namespace NBi.Core.Hierarchical.Xml
{
    public abstract class XPathEngine : AbstractPathEngine
    {
        public string DefaultNamespacePrefix { get; }
        public bool IsIgnoreNamespace { get; }

        protected XPathEngine(string from, IEnumerable<AbstractSelect> selects, string defaultNamespacePrefix, bool isIgnoreNamespace)
            : base(from, selects)
            => (DefaultNamespacePrefix, IsIgnoreNamespace) = (defaultNamespacePrefix, isIgnoreNamespace);

        public IEnumerable<object> Execute(XDocument items)
        {
            var nsMgr = new XmlNamespaceManager(new NameTable());
            if (!string.IsNullOrEmpty(DefaultNamespacePrefix))
                nsMgr.AddNamespace(DefaultNamespacePrefix, items.Root.GetDefaultNamespace().NamespaceName);

            var xmlNamespaces = ((IEnumerable<object>)items.XPathEvaluate(@"//namespace::*[not(. = ../../namespace::*)]")).Cast<XAttribute>();
            foreach (var namespaceNode in xmlNamespaces)
                if (namespaceNode.Name.LocalName != "xmlns")
                    nsMgr.AddNamespace(namespaceNode.Name.LocalName, namespaceNode.Value);

            var result = from item in items.XPathSelectElements(From, nsMgr)
                         select GetObj(item, nsMgr);

            return result;
        }

        private object GetObj(XElement x, IXmlNamespaceResolver ns)
        {
            var obj = new List<object>();
            obj.AddRange(BuildXPaths(x, ns, Selects).ToArray());
            return obj;
        }

        protected internal IEnumerable<object> BuildXPaths(XElement item, IXmlNamespaceResolver ns, IEnumerable<AbstractSelect> selects)
        {
            foreach (var select in selects)
                if (select is AttributeSelect)
                {
                    var attributeSelect = select as AttributeSelect;
                    yield return
                    (
                        (
                            item.XPathSelectElement(attributeSelect.Path, ns)
                            ?? new XElement("null", "(null)")
                        ).Attribute(attributeSelect.Attribute)
                        ?? new XAttribute("null", "(null)")
                    ).Value;
                }
                else if (select is EvaluateSelect)
                {
                    yield return
                    (
                        item.XPathEvaluate(select.Path, ns)
                        ?? new XElement("null", "(null)")
                    );
                }
                else
                    yield return
                    (
                        item.XPathSelectElement(select.Path, ns)
                        ?? new XElement("null", "(null)")
                    ).Value;
        }

        protected XmlReader CreateReader(string filePath, bool isRemoveDefaultNamespace)
        {
            var settings = new XmlReaderSettings();
            var streamReader = GetTextReader(filePath);
            if (isRemoveDefaultNamespace)
                return new XmlIgnoreNamespaceReader(streamReader, settings);
            else
                return XmlReader.Create(streamReader, settings);
        }

        protected abstract TextReader GetTextReader(string filePath);
    }
}
