using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup
{
    abstract class KeysRetriever
    {
        protected IEnumerable<IColumnDefinition> Settings { get; }

        public KeysRetriever(IEnumerable<IColumnDefinition> settings)
        {
            Settings = settings;
        }

        public abstract KeyCollection GetKeys(DataRow row);

        protected internal object FormatValue(ColumnType columnType, object value)
        {
            var factory = new CasterFactory();
            var caster = factory.Instantiate(columnType);
            return caster.Execute(value);
        }
    }
}
