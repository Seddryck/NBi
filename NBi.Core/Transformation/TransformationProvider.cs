using NBi.Core.ResultSet;
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
        private IDictionary<IColumnIdentifier, ITransformer> cacheTransformers;
        private readonly TransformerFactory factory;

        public TransformationProvider()
        {
            cacheTransformers = new Dictionary<IColumnIdentifier, ITransformer>();
            factory = new TransformerFactory();
        }

        public void Add(IColumnIdentifier indentifier, ITransformationInfo transfo)
        {
            var transformer = factory.Instantiate(transfo);
            transformer.Initialize(transfo.Code);
            if (cacheTransformers.ContainsKey(indentifier))
                throw new NBiException($"You can't define two transformers for the same column. The column {indentifier.Label} has already another transformer specified.");
            cacheTransformers.Add(indentifier, transformer);
        }

        public virtual ResultSet.ResultSet Transform(ResultSet.ResultSet resultSet)
        {
            foreach (var identifier in cacheTransformers.Keys)
            {
                var tsStart = DateTime.Now;
                var transformer = cacheTransformers[identifier];

                var newColumn = new DataColumn() { DataType = typeof(object) };
                resultSet.Table.Columns.Add(newColumn);

                var ordinal = (identifier as ColumnPositionIdentifier)?.Position ?? resultSet.Table.Columns[(identifier as ColumnNameIdentifier).Name].Ordinal;

                foreach (DataRow row in resultSet.Table.Rows)
                    row[newColumn.Ordinal] = transformer.Execute(row[ordinal]);

                resultSet.Table.Columns.RemoveAt(ordinal);
                newColumn.SetOrdinal(ordinal);

                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, string.Format("Time needed to transform column {0}: {1}", identifier.Label, DateTime.Now.Subtract(tsStart).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            }

            return resultSet;
        }
    }
}
