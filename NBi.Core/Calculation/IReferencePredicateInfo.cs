using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public interface IReferencePredicateInfo
    {
        string Reference { get; }
    }
}
