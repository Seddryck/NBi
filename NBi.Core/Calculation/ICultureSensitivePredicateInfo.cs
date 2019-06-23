using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public interface ICultureSensitivePredicateInfo : IReferencePredicateInfo
    {
        string Culture { get; }
    }
}
