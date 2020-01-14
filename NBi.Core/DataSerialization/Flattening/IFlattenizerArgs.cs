using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening
{
    public interface IFlattenizerArgs
    {
        IEnumerable<IPathSelect> Selects { get; }
    }
}
