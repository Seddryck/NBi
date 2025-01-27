using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.Sequence.Resolver.Loop;

public enum IntervalMode
{
    [XmlEnum("close")]
    Close = 0,
    [XmlEnum("half-open")]
    HalfOpen = 1
}
