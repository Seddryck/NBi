using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Converter
{
    class TextConverter : IConverter<string>
    {
        public string Convert(object value)
        {
            if (value is string)
                return (string)value;

            return value.ToString();
        }

        public bool IsValid(object value)
        {
            return true;
        }
    }
}
