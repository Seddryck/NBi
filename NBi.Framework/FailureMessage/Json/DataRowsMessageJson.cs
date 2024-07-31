using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NBi.Core.ResultSet;
using System.IO;
using Newtonsoft.Json;
using NBi.Framework.Sampling;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Extensibility;

namespace NBi.Framework.FailureMessage.Json
{
    class DataRowsMessageJson : IDataRowsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<IResultRow>> samplers;

        private string expected = string.Empty;
        private string actual = string.Empty;
        private string analysis = string.Empty;


        public DataRowsMessageJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
        {
            this.samplers = samplers;
        }

        public void BuildComparaison(IEnumerable<IResultRow> expectedRows, IEnumerable<IResultRow> actualRows, ResultResultSet? compareResult)
        {
            compareResult ??= ResultResultSet.Build([], [], [], [], []);

            expected = BuildTable(expectedRows, samplers["expected"]);
            actual = BuildTable(actualRows, samplers["actual"]);

            analysis = BuildMultipleTables(
                [
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("unexpected", compareResult.Unexpected ?? [], new TableHelperJson()),
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("missing", compareResult.Missing ?? [], new TableHelperJson()),
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("duplicated", compareResult.Duplicated ?? [], new TableHelperJson()),
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("non-matching", compareResult.NonMatchingValue?.Rows ?? [], new CompareTableHelperJson()),
                ], samplers["analysis"]
             );
        }

        public void BuildDuplication(IEnumerable<IResultRow> actualRows, ResultUniqueRows result)
        {
            actual = BuildTable(actualRows, samplers["actual"]);
            analysis = BuildMultipleTables(
                [
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("not-unique", result.Rows, new TableHelperJson())
                ], samplers["analysis"]);
        }

        public void BuildFilter(IEnumerable<IResultRow> actualRows, IEnumerable<IResultRow> filteredRows)
        {
            actual = BuildTable(actualRows, samplers["actual"]);
            analysis = BuildMultipleTables(
                [
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("filtered", filteredRows, new TableHelperJson())
                ], samplers["analysis"]);
        }
        public void BuildCount(IEnumerable<IResultRow> actualRows)
        {
            actual = BuildTable(actualRows, samplers["actual"]);
        }

        private string BuildTable(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw);

            BuildTable(rows, sampler, writer);

            writer.Close();
            return sb.ToString();
        }

        private string BuildMultipleTables(IEnumerable<Tuple<string, IEnumerable<IResultRow>, TableHelperJson>> tableInfos, ISampler<IResultRow> sampler)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            foreach (var item in tableInfos)
            {
                writer.WritePropertyName(item.Item1);
                BuildTable(item.Item2, sampler, item.Item3, writer);
            }
            writer.WriteEndObject();

            writer.Close();
            return sb.ToString();
        }

        private void BuildTable(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, JsonWriter writer)
        {
            BuildTable(rows, sampler, new TableHelperJson(), writer);
        }

        protected virtual void BuildTable(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, TableHelperJson tableHelper, JsonWriter writer)
        {
            tableHelper.Execute(rows, sampler, writer);
        }


        public string RenderExpected() => expected;

        public string RenderActual() => actual;

        public string RenderAnalysis() => analysis;

        public string RenderMessage()
        {
            var sb = new StringBuilder();
            using var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("timestamp");
            writer.WriteValue(DateTime.Now);
            if (!string.IsNullOrEmpty(expected))
            {
                writer.WritePropertyName("expected");
                writer.WriteRawValue(expected);
            }
            if (!string.IsNullOrEmpty(actual))
            {
                writer.WritePropertyName("actual");
                writer.WriteRawValue(actual);
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
