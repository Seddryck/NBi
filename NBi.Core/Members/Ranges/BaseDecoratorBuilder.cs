using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges;

internal abstract class BaseDecoratorBuilder : IDecoratorBuilder
{
    protected IRange? Range { get; set; }
    protected IEnumerable<string> Result { get; set; } = [];
    private bool isSetup = false;
    private bool isBuild = false;

    public void Setup(IRange range)
    {
        Result = [];
        Range = range;
        isBuild = false;
        isSetup = true;
    }

    public void Apply(IEnumerable<string> values)
    {
        if (!isSetup)
            throw new InvalidOperationException();
        Result = InternalApply(values);
        isBuild = true;
    }

    protected abstract IEnumerable<string> InternalApply(IEnumerable<string> values);

    public IEnumerable<string> GetResult()
    {
        if (!isBuild)
            throw new InvalidOperationException();
        return Result;
    }
}
