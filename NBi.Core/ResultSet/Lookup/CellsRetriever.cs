using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup
{
    abstract public class CellsRetriever
    {
        protected IEnumerable<IColumnDefinition> Settings { get; }

        public CellsRetriever(IEnumerable<IColumnDefinition> settings)
        {
            Settings = settings;
        }

        public abstract KeyCollection GetColumns(DataRow row);

        protected internal object FormatValue(ColumnType columnType, object value)
        {
            var factory = new CasterFactory();
            var caster = factory.Instantiate(columnType);
            return caster.Execute(value);
        }
    }
}
