using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Members.Ranges;

namespace NBi.Xml.Items.Ranges;

public abstract class RangeXml : IRange
{
    public IEnumerable<string> GetItems()
    {
        var factory = new RangeMembersFactory();
        return factory.Instantiate(this);
    }
}
