using System;
using System.Collections.Generic;
using System.Globalization;

namespace NBi.Core.Members.Ranges;

public interface IRangeMembersBuilder
{
    void Setup(IRange range);
    void Build();
    IEnumerable<string> GetResult();
}
