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
            return typeof(T).Name switch
            {
                "Object" => (ICaster<T>)new ImplicitCaster(),
                "String" => (ICaster<T>)new TextCaster(),
                "Decimal" => (ICaster<T>)new NumericCaster(),
                "Boolean" => (ICaster<T>)new BooleanCaster(),
                "DateTime" => (ICaster<T>)new DateTimeCaster(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }

    public class CasterFactory
    {
        public ICaster Instantiate(ColumnType type)
        {
            return type switch
            {
                ColumnType.Untyped => new UntypedCaster(),
                ColumnType.Text => new TextCaster(),
                ColumnType.Numeric => new NumericCaster(),
                ColumnType.Boolean => new BooleanCaster(),
                ColumnType.DateTime => new DateTimeCaster(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
