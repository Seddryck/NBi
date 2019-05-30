using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting
{
    public class CasterFactory<T>
    {
        public ICaster<T> Instantiate()
        {
            switch (typeof(T).Name)
            {
                case "Object": return (ICaster<T>)new ImplicitCaster();
                case "String": return (ICaster<T>)new TextCaster();
                case "Decimal": return (ICaster<T>)new NumericCaster();
                case "Boolean": return (ICaster<T>)new BooleanCaster();
                case "DateTime": return (ICaster<T>)new DateTimeCaster();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class CasterFactory
    {
        public ICaster Instantiate(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Text: return new TextCaster();
                case ColumnType.Numeric: return new NumericCaster();
                case ColumnType.Boolean: return new BooleanCaster();
                case ColumnType.DateTime: return new DateTimeCaster();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
