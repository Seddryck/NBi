using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet.Combination;

public enum SequenceCombinationOperation
{
    [XmlEnum("cartesian-product")]
    CartesianProduct = 1
}
