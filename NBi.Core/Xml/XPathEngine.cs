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
using NBi.Core.ResultSet;

namespace NBi.Core.Xml
{
    public abstract class XPathEngine
    {
        private readonly IEnumerable<AbstractSelect> selects;
        private readonly string from;
        public string DefaultNamespacePrefix { get; }

        public XPathEngine(string from, IEnumerable<AbstractSelect> selects, string defaultNamespacePrefix)
        {
            this.from = from;
            this.selects = selects;
            DefaultNamespacePrefix = defaultNamespacePrefix;
        }

        public abstract IEnumerable<object> Execute();

        public IEnumerable<object> Execute(XDocument items)
        {
            var nsMgr = new XmlNamespaceManager(new NameTable());
            if (!string.IsNullOrEmpty(DefaultNamespacePrefix))
                nsMgr.AddNamespace(DefaultNamespacePrefix, items.Root.GetDefaultNamespace().NamespaceName);

            var xmlNamespaces = ((IEnumerable<object>)items.XPathEvaluate(@"//namespace::*[not(. = ../../namespace::*)]")).Cast<XAttribute>();
            foreach (var namespaceNode in xmlNamespaces)
                if (namespaceNode.Name.LocalName != "xmlns")
                    nsMgr.AddNamespace(namespaceNode.Name.LocalName, namespaceNode.Value);

            var result = from item in items.XPathSelectElements(@from, nsMgr)
                         select GetObj(item, nsMgr);

            return result;
        }

        private object GetObj(XElement x, IXmlNamespaceResolver ns)
        {
            var obj = new List<object>();
            obj.AddRange(BuildXPaths(x, ns, selects).ToArray());
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
    }
}
