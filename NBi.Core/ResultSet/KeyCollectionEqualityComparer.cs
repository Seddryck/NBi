using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

class KeyCollectionEqualityComparer : IEqualityComparer<object>
{
    public int GetHashCode(object x)
        => (x as KeyCollection)?.GetHashCode() ?? 0;
    public new bool Equals(object? x, object? y)
        => (x as KeyCollection)?.Equals(y as KeyCollection) ?? false;
}
