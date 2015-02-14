using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Converter
{
    interface IConverter<T>
    {
        T Convert(object obj);
        bool IsValid(object obj);
    }
}
