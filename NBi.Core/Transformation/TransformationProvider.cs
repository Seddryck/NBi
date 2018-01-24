using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation
{
    public class TransformationProvider
    {
        private IDictionary<int, ITransformer> cacheTransformers;
        private readonly TransformerFactory factory;

        public TransformationProvider()
        {
            cacheTransformers = new Dictionary<int, ITransformer>();
            factory = new TransformerFactory();
        }

        public void Add(int columnIndex, ITransformationInfo transfo)
        {
            var transformer = factory.Instantiate(transfo);
            transformer.Initialize(transfo.Code);
            if (cacheTransformers.ContainsKey(columnIndex))
                throw new NBiException($"You can't define two transformers for the same column. The column with index '{columnIndex}' has already another transformer specified.");
            cacheTransformers.Add(columnIndex, transformer);
        }

        public virtual ResultSet.ResultSet Transform(ResultSet.ResultSet resultSet)
        {
            foreach (var index in cacheTransformers.Keys)
            {
                var tsStart = DateTime.Now;
                var transformer = cacheTransformers[index];

                var newColumn = new DataColumn() { DataType = typeof(object) };
                resultSet.Table.Columns.Add(newColumn);

                foreach (DataRow row in resultSet.Table.Rows)
                    row[newColumn.Ordinal] = transformer.Execute(row[index]);

                resultSet.Table.Columns.RemoveAt(index);
                newColumn.SetOrdinal(index);

                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Time needed to transform column with index {0}: {1}", index, DateTime.Now.Subtract(tsStart).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            }

            return resultSet;
        }
    }
}
