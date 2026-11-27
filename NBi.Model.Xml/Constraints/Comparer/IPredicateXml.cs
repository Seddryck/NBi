using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Xml.Constraints.Comparer;

public interface IPredicateXml
{
    bool Not { get; set; }
    ComparerType ComparerType { get; }
}
