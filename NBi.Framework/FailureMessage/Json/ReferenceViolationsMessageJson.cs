using NBi.Core.ResultSet.Lookup;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json
{
    class ReferenceViolationsMessageJson : IReferenceViolationsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<DataRow>> samplers;
        private string child;
        private string parent;
        private string analysis;

        public ReferenceViolationsMessageJson(IDictionary<string, ISampler<DataRow>> samplers)
        {
            this.samplers = samplers;
        }

        public void Generate(IEnumerable<DataRow> parentRows, IEnumerable<DataRow> childRows, ReferenceViolations violations)
        {
            parent = BuildTable(parentRows, samplers["expected"]);
            child = BuildTable(childRows, samplers["actual"]);

            var rows = new List<DataRow>();
            foreach (var violation in violations)
                rows = rows.Union(violation.Value).ToList();

            analysis = BuildTable(rows, samplers["analysis"]);
        }

        private string BuildTable(IEnumerable<DataRow> rows, ISampler<DataRow> sampler)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw);

            var tableHelper = new TableHelperJson();
            tableHelper.Execute(rows, sampler, writer);

            writer.Close();
            return sb.ToString();
        }

        public string RenderChild() => child;
        public string RenderParent() => parent;
        public string RenderAnalysis() => analysis;
        public virtual string RenderPredicate() => "Some references are missing and violate referential integrity";

        public string RenderMessage()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                if (!string.IsNullOrEmpty(child))
                {
                    writer.WritePropertyName("child");
                    writer.WriteRawValue(child);
                }
                if (!string.IsNullOrEmpty(parent))
                {
                    writer.WritePropertyName("parent");
                    writer.WriteRawValue(parent);
                }
                if (!string.IsNullOrEmpty(analysis))
                {
                    writer.WritePropertyName("analysis");
                    writer.WriteRawValue(analysis);
                }
                writer.WriteEndObject();
                return sb.ToString();
            }
        }
    }
}
