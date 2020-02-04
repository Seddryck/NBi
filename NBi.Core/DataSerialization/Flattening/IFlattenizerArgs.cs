using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening
{
    public interface IFlattenizerArgs
    {
        IScalarResolver<string> From { get; }
        IEnumerable<IPathSelect> Selects { get; }
    }
}
