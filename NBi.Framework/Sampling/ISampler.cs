using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Sampling;

public interface ISampler<T>
{
    void Build(IEnumerable<T> fullSet);
    IEnumerable<T> GetResult();
    bool GetIsSampled();
    int GetExcludedRowCount();
}
