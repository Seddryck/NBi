using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation;

public interface IInstanceArgs
{
    IEnumerable<string> Categories { get; }
    IDictionary<string, string> Traits { get; }
}
