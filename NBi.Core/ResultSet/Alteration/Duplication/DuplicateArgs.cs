using NBi.Core.Calculation.Asserting;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication
{
    public class DuplicateArgs : IDuplicationArgs
    {
        public IPredication Predication { get; set; }
        public IScalarResolver<int> Times {get; set;}
        public IList<OutputArgs> Outputs { get; set; } = [];

        public DuplicateArgs(IPredication predication, IScalarResolver<int> times, IList<OutputArgs> outputs)
            => (Predication, Times, Outputs) = (predication, times, outputs);
    }
}
