using System;
using System.Collections.Generic;
using System.Globalization;

namespace NBi.Core.Members.Predefined;

public interface IPredefinedMembersBuilder
{
    void Setup(CultureInfo culture);
    void Build();
    IEnumerable<string> GetResult();
}
