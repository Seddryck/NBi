using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Sampling;

class FullSampler<T> : ISampler<T>
{
    private bool isBuild = false;

    private IEnumerable<T> result = [];

    public FullSampler()
    {
    }

    public void Build(IEnumerable<T> fullSet)
    {
        result = fullSet;
        isBuild = true;
    }

    public IEnumerable<T> GetResult()
    {
        if (!isBuild)
            throw new InvalidOperationException();
        return result;
    }

    public bool GetIsSampled()
    {
        if (!isBuild)
            throw new InvalidOperationException();
        return false;
    }

    public int GetExcludedRowCount()
    {
        if (!isBuild)
            throw new InvalidOperationException();
        return 0;
    }




}
