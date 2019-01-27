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

            return value.ToString();
        }

        object ICaster.Execute(object value) => Execute(value);

        public bool IsValid(object value)
        {
            return true;
        }
    }
}
