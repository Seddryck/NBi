using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;
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
        private Context Context { get; }

        public TransformationProvider(ServiceLocator serviceLocator, Context context)
        {
            cacheTransformers = new Dictionary<IColumnIdentifier, ITransformer>();
            factory = new TransformerFactory(serviceLocator, context);
            Context = context;
        }

        public void Add(IColumnIdentifier indentifier, ITransformationInfo transfo)
        {
            var transformer = factory.Instantiate(transfo);
            transformer.Initialize(transfo.Code);
            if (cacheTransformers.ContainsKey(indentifier))
                throw new NBiException($"You can't define two transformers for the same column. The column {indentifier.Label} has already another transformer specified.");
            cacheTransformers.Add(indentifier, transformer);
        }

        public virtual IResultSet Transform(IResultSet resultSet)
        {
            foreach (var identifier in cacheTransformers.Keys)
            {
                var tsStart = DateTime.Now;
                var transformer = cacheTransformers[identifier];

                var newColumn = new DataColumn() { DataType = typeof(object) };
                resultSet.Columns.Add(newColumn);

                var ordinal = (identifier as ColumnOrdinalIdentifier)?.Ordinal ?? resultSet.Columns[(identifier as ColumnNameIdentifier).Name].Ordinal;
                var originalName = resultSet.Columns[ordinal].ColumnName;

                foreach (DataRow row in resultSet.Rows)
                {
                    Context.Switch(row);
                    row[newColumn.Ordinal] = transformer.Execute(row[ordinal]);
                }

                resultSet.Columns.RemoveAt(ordinal);
                newColumn.SetOrdinal(ordinal);
                newColumn.ColumnName = originalName;

                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Time needed to transform column {0}: {1}", identifier.Label, DateTime.Now.Subtract(tsStart).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            }

            return resultSet;
        }
    }
}
