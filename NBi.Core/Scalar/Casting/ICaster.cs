using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Caster
{
    public interface ICaster<T>
    {
        T Execute(object obj);
        bool IsValid(object obj);
    }
}
