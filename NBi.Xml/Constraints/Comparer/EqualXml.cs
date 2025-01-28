using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Constraints.Comparer;

public class EqualXml : ScalarReferencePredicateXml
{
    public override ComparerType ComparerType { get => ComparerType.Equal; }
}
