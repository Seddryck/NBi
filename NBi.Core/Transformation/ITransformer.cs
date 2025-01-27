using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation;

public interface ITransformer
{
    void Initialize(string code);
    object? Execute(object value);
}
