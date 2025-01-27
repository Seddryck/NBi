using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer;

public class ComparerResult
{
    public string Message { get; private set; } = string.Empty;
    public bool AreEqual { get; private set; }

    private ComparerResult(bool result, string message)
        => (AreEqual, Message) = (result, message);

    public ComparerResult(string message)
        : this(false, message) { }


    private static readonly EqualityComparerResult equality = new();
    public static ComparerResult Equality
        => equality;

    private class EqualityComparerResult : ComparerResult
    {
        public EqualityComparerResult()
            : base(true, "equal") { }
    }

    private class InequalityComparerResult : ComparerResult
    {
        public InequalityComparerResult(string message)
            : base(false, "equal") { }
    }
}
