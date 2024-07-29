using NBi.Core.Scalar.Casting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup
{
    abstract public class CellRetriever
    {
        protected IEnumerable<IColumnDefinition> Settings { get; }

        public CellRetriever(IEnumerable<IColumnDefinition> settings)
        {
            Settings = settings;
        }

        public abstract KeyCollection GetColumns(IResultRow row);

        protected internal object FormatValue(ColumnType columnType, object? value)
        {
            var factory = new CasterFactory();
            var caster = factory.Instantiate(columnType);
            return caster.Execute(value);
        }
    }
}
