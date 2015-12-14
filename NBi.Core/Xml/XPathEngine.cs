using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using NBi.Core.ResultSet;

namespace NBi.Core.Xml
{
    public abstract class XPathEngine
    {
        private readonly IEnumerable<AbstractSelect> selects;
        private readonly string from;

        public XPathEngine(string from, IEnumerable<AbstractSelect> selects)
        {
            this.from = from;
            this.selects = selects;
        }

        public abstract NBi.Core.ResultSet.ResultSet Execute();

        public NBi.Core.ResultSet.ResultSet Execute(XDocument items)
        {
            var result = from item in items.XPathSelectElements(@from)
                         select GetObj(item);

            var builder = new ResultSetBuilder();
            var rows = result.ToArray();
            var resultSet = builder.Build(rows);
            return resultSet;
        }

        private object GetObj(XElement x)
        {
            var obj = new List<object>();
            obj.AddRange(BuildXPaths(x, selects).ToArray());
            return obj;
        }

        protected internal IEnumerable<object> BuildXPaths(XElement item, IEnumerable<AbstractSelect> selects)
        {
            foreach (var select in selects)
                if (select is AttributeSelect)
                {
                    var attributeSelect = select as AttributeSelect;
                    yield return
                    (
                        (
                            item.XPathSelectElement(attributeSelect.Path)
                            ?? new XElement("null", "(null)")
                        ).Attribute(attributeSelect.Attribute)
                        ?? new XAttribute("null", "(null)")
                    ).Value;
                }
                else if (select is EvaluateSelect)
                {
                    yield return
                    (
                        item.XPathEvaluate(select.Path)
                        ?? new XElement("null", "(null)")
                    );
                }
                else
                    yield return
                    (
                        item.XPathSelectElement(select.Path)
                        ?? new XElement("null", "(null)")
                    ).Value;
        }
    }
}
