using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Constraints.Comparer
{
    public class EqualXml : PredicateXml
    {
        internal override ComparerType ComparerType { get => ComparerType.Equal; }
    }
}
