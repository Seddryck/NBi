using NBi.Core.Scalar.Casting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public abstract class DataRowKeysComparer : IEqualityComparer<IResultRow>
    {

        public bool Equals(IResultRow? x, IResultRow? y)
        {
            if (x is null || y is null)
                return false;

            if (!CheckKeysExist(x))
                throw new ArgumentException("First datarow has not the required key fields");
            if (!CheckKeysExist(y))
                throw new ArgumentException("Second datarow has not the required key fields");

            return GetHashCode(x) == GetHashCode(y);
        }

        protected abstract bool CheckKeysExist(IResultRow dr);
        public abstract KeyCollection GetKeys(IResultRow row);

        public int GetHashCode(IResultRow? dr)
        {
            if (dr is null)
                return 0;

            int hash = 0;
            foreach (var value in GetKeys(dr).Members)
            {
                var v = value is IConvertible convertible
                    ? convertible.ToString(CultureInfo.InvariantCulture)
                    : value.ToString();

                hash = (hash * 397) ^ v!.GetHashCode();

            }
            return hash;
        }

        protected internal object FormatValue(ColumnType columnType, object? value)
        {
            if (value==null || value==DBNull.Value || value as string == "(null)")
                return "(null)";

            switch (columnType)
            {
                case ColumnType.Numeric:
                    return new NumericCaster().Execute(value);
                case ColumnType.DateTime:
                    return new DateTimeCaster().Execute(value);
                case ColumnType.Boolean:
                    return new ThreeStateBooleanCaster().Execute(value);
                default:
                    if (value == DBNull.Value)
                        return "(null)";
                    else if (value is IConvertible convertible)
                        return convertible.ToString(CultureInfo.InvariantCulture);
                    else
                        return value.ToString() ?? "";
            }
        }
    }
}
