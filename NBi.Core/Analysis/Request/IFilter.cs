using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request;

public interface IFilter
{
    string Value { get; }
    DiscoveryTarget Target { get; }
}
