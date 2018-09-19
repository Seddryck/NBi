using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown
{
    class ScoreMessageMarkdown : IScoreMessageFormatter
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

        public string RenderExpected() => $"threshold was set to {Threshold}";
        public string RenderActual() => $"score is {Score}";

        public string RenderMessage() => $"{(Result ? "A good" : "An insufficient")} score of {Score} was received when the threshold was set {Threshold}.";

        private string WriteJson(IDictionary<string, decimal> values)
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
}
