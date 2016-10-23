using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Converter
{
    public class ConverterFactory<T>
    {
        public IConverter<T> Build()
        {
            switch (typeof(T).Name)
            {
                case "String": return (IConverter<T>)new TextConverter();
                case "Decimal": return (IConverter<T>)new NumericConverter();
                case "Boolean": return (IConverter<T>)new BooleanConverter();
                case "DateTime": return (IConverter<T>)new DateTimeConverter();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
