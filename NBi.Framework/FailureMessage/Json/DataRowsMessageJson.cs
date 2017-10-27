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

namespace NBi.Framework.FailureMessage.Json
{
    class DataRowsMessageJson : IDataRowsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<DataRow>> samplers;
        private readonly ComparisonStyle style;

        private string expected;
        private string actual;
        private string analysis;


        public DataRowsMessageJson(ComparisonStyle style, IDictionary<string, ISampler<DataRow>> samplers)
        {
            this.style = style;
            this.samplers = samplers;
        }

        public void BuildComparaison(IEnumerable<DataRow> expectedRows, IEnumerable<DataRow> actualRows, ResultSetCompareResult compareResult)
        {
            compareResult = compareResult ?? ResultSetCompareResult.Build(new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>());

            expected = BuildTable(expectedRows, samplers["expected"]);
            actual = BuildTable(actualRows, samplers["actual"]);

            analysis = BuildMultipleTables(
                new[]
                {
                    new Tuple<string, IEnumerable<DataRow>, TableHelperJson>("unexpected", compareResult.Unexpected, new TableHelperJson()),
                    new Tuple<string, IEnumerable<DataRow>, TableHelperJson>("missing", compareResult.Missing, new TableHelperJson()),
                    new Tuple<string, IEnumerable<DataRow>, TableHelperJson>("duplicated", compareResult.Duplicated, new TableHelperJson()),
                    new Tuple<string, IEnumerable<DataRow>, TableHelperJson>("non-matching", compareResult.NonMatchingValue.Rows, new CompareTableHelperJson()),
                }, samplers["analysis"]
             );
        }

        public void BuildDuplication(IEnumerable<DataRow> actualRows, UniqueRowsResult result)
        {
            actual = BuildTable(actualRows, samplers["actual"]);
            analysis = BuildTable(result.Rows, samplers["analysis"]);
        }

        public void BuildFilter(IEnumerable<DataRow> actualRows, IEnumerable<DataRow> filteredRows)
        {
            actual = BuildTable(actualRows, samplers["actual"]);
            analysis = BuildTable(filteredRows, samplers["analysis"]);
        }
        public void BuildCount(IEnumerable<DataRow> actualRows)
        {
            actual = BuildTable(actualRows, samplers["actual"]);
        }

        private string BuildTable(IEnumerable<DataRow> rows, ISampler<DataRow> sampler)
        {

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw);

            BuildTable(rows, sampler, writer);

            writer.Close();
            return sb.ToString();
        }

        private string BuildMultipleTables(IEnumerable<Tuple<string, IEnumerable<DataRow>, TableHelperJson>> tableInfos, ISampler<DataRow> sampler)
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

        private void BuildTable(IEnumerable<DataRow> rows, ISampler<DataRow> sampler, JsonWriter writer)
        {
            BuildTable(rows, sampler, new TableHelperJson(), writer);
        }

        private void BuildTable(IEnumerable<DataRow> rows, ISampler<DataRow> sampler, TableHelperJson tableHelper, JsonWriter writer)
        {
            tableHelper.Execute(rows, sampler, writer);
        }


        public string RenderExpected()
        {
            return expected;
        }

        public string RenderActual()
        {
            return actual;
        }

        public string RenderAnalysis()
        {
            return analysis;
        }

        public string RenderMessage()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
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
}
