using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting
{
    class TextCaster : ICaster<string>
    {
        public string Execute(object value)
        {
            if (value is string)
                return (string)value;
            
            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");

            if (value is bool)
                return (bool)value ? "True" : "False";
            
            var numericCaster = new NumericCaster();
            if (numericCaster.IsStrictlyValid(value))
                return Convert.ToDouble(value).ToString(new CultureFactory().Invariant.NumberFormat);

            return value.ToString();
        }

        object ICaster.Execute(object value) => Execute(value);

        public bool IsValid(object value)
        {
            return true;
        }
    }
}
