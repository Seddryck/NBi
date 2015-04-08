using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Converter
{
    class NumericConverter : BaseNumericConverter, IConverter<decimal>
    {
        public decimal Convert(object value)
        {
            if (value is decimal)
                return (decimal)value;

            return System.Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
        }
    }
}
