using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening.Json;

class JsonPathEngine : PathFlattenizer, IDataSerializationFlattenizer
{
    public JsonPathEngine(IScalarResolver<string> from, IEnumerable<IPathSelect> selects)
        : base(from, selects) { }

    public override IEnumerable<object> Execute(TextReader textReader)
    {
        var json = JToken.ReadFrom(new JsonTextReader(textReader));
        var result = from item in json.SelectTokens(From.Execute() ?? throw new NullReferenceException())
                     select GetObj(item);
        return result;
    }

    private object GetObj(JToken item)
    {
        var obj = new List<object>();
        obj.AddRange(BuildPaths(item, Selects).ToArray() ?? []);
        return obj;
    }

    protected internal IEnumerable<object> BuildPaths(JToken item, IEnumerable<IPathSelect> selects)
    {
        foreach (var select in selects)
        {
            var path = (select.Path.Execute() ?? string.Empty).Trim();
            var root = item;
            if (path.StartsWith("!"))
            {
                var match = Regex.Matches(path, @"^(!*)").Cast<Match>().First();
                var i = 0;
                while (i < match.Value.Length && !string.IsNullOrEmpty(root?.Path))
                {
                    var previousParentPath = root.Path;
                    root = root.Parent;
                    if (previousParentPath != (root?.Path ?? string.Empty))
                        i++;
                }

                path = $"${ path[match.Value.Length..]}";
            }

            yield return
            (
                root!.SelectToken(path)
                ?? new JValue("(null)")
            ).ToObject<object>() ?? throw new NullReferenceException();
        }
    }
}
