using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NBi.Core.Hierarchical;
using NBi.Core.Hierarchical.Xml;
using NBi.Core.ResultSet;
using System.Text.RegularExpressions;

namespace NBi.Core.Hierarchical.Json
{
    public abstract class JsonPathEngine : AbstractPathEngine
    {
        protected JsonPathEngine(string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects) { }

        public IEnumerable<object> Execute(JToken items)
        {
            var result = from item in items.SelectTokens(From)
                         select GetObj(item);

            return result;
        }

        private object GetObj(JToken item)
        {
            var obj = new List<object>();
            obj.AddRange(BuildPaths(item, Selects).ToArray());
            return obj;
        }

        protected internal IEnumerable<object> BuildPaths(JToken item, IEnumerable<AbstractSelect> selects)
        {
            foreach (var select in selects)
            {
                var path = select.Path.Trim();
                var root = item;
                if (path.StartsWith("!"))
                {
                    var match = Regex.Matches(path, @"^(!*)").Cast<Match>().First();
                    var i = 0;
                    while (i < match.Value.Length && !string.IsNullOrEmpty(root.Path))
                    {
                        var previousParentPath = root.Path;
                        root = root.Parent;
                        if (previousParentPath != root.Path)
                            i++;
                    }
                        
                    path = $"${ path.Substring(match.Value.Length)}";
                }

                yield return
                (
                    root.SelectToken(path)
                    ?? new JValue("(null)")
                ).ToObject<object>();
            }
        }
    }
}
