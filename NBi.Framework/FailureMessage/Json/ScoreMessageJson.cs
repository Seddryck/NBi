using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json;

class ScoreMessageJson : IScoreMessageFormatter
{
    protected decimal Score { get; private set; }
    protected decimal Threshold { get; private set; }
    protected bool Result { get; private set; }

    public void Initialize(decimal score, decimal threshold, bool result)
    {
        Score = score;
        Threshold = threshold;
        Result = result;
    }

    public string RenderExpected() => WriteJson(new Dictionary<string, object>() { { "threshold", Threshold } });
    public string RenderActual() => WriteJson(new Dictionary<string, object>() { { "score", Score } });

    public string RenderMessage() => WriteJson(
        new Dictionary<string, object>() {
            { "timestamp", DateTime.Now },
            { "success", Result },
            { "score", Score },
            { "threshold", Threshold },
        });

    protected virtual string WriteJson(IDictionary<string, object> values)
    {
        var sb = new StringBuilder();
        var sw = new StringWriter(sb);
        var writer = new JsonTextWriter(sw);

        writer.WriteStartObject();
        foreach (var value in values)
        {
            writer.WritePropertyName(value.Key);
            writer.WriteValue(value.Value);
        }
        writer.WriteEndObject();

        return sb.ToString();
    }

}
