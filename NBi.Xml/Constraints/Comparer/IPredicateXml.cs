using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Constraints.Comparer;

public interface IPredicateXml
{
    bool Not { get; set; }
    ComparerType ComparerType { get; }
}
