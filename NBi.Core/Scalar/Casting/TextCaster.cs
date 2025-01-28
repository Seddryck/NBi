using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting;

class TextCaster : ICaster<string>
{
    public string Execute(object? value)
        => value switch
        {
            null => "(null)",
            string str => str,
            DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
            bool b => b ? "True" : "False",
            _ when new NumericCaster().IsStrictlyValid(value) => Convert.ToDecimal(value).ToString(new CultureFactory().Invariant.NumberFormat),
            _ => value.ToString() ?? string.Empty
        };

    object ICaster.Execute(object? value) 
        => Execute(value);

    public bool IsValid(object? value)
        => true;

    public bool IsStrictlyValid(object? value)
        => !(value == null || value == DBNull.Value || (value is string str && str == "(null)"));
}
