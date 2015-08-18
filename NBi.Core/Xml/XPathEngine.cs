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
    class XPathEngine
    {
        private readonly IEnumerable<Select> selects;
        private readonly string from;

        public XPathEngine(string from, IEnumerable<Select> selects)
        {
            this.from = from;
            this.selects = selects;
        }

        public NBi.Core.ResultSet.ResultSet Execute(string value)
        {
            var textReader = new StringReader(value);
            return Execute(textReader);
        }

        public NBi.Core.ResultSet.ResultSet Execute(TextReader reader)
        {
            var items = XDocument.Load(reader);
            var result = from item in items.XPathSelectElements(@from)
                         select GetObj(item);

            var builder = new ResultSetBuilder();
            var resultArray = result.ToArray();
            var resultSet = builder.Build(resultArray);
            return resultSet;
        }

        private object GetObj(XElement x)
        {
            var obj = new List<object>();
            obj.AddRange(BuildXPaths(x, selects).ToArray());
            return obj;
        }

        protected internal IEnumerable<object> BuildXPaths(XElement item, IEnumerable<Select> elements)
        {
            foreach (var element in elements)
                if (string.IsNullOrEmpty(element.Attribute))
                    yield return
                    (
                        item.XPathSelectElement(element.Path)
                        ?? new XElement("null", "(null)")
                    ).Value;
                else
                    yield return
                    (
                        (
                            item.XPathSelectElement(element.Path)
                            ?? new XElement("null", "(null)")
                        ).Attribute(element.Attribute)
                        ?? new XAttribute("null", "(null)")
                    ).Value;
        }
    }
}
