using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer
{
    class TextComparer : BaseComparer
    {

        protected override ComparerResult CompareObjects(object? x, object? y)
        {
            var rxText = x?.ToString() ?? string.Empty;
            var ryText = y?.ToString() ?? string.Empty;

            if (IsEqual(rxText, ryText, StringComparer.InvariantCulture))
                return ComparerResult.Equality;

            return new ComparerResult(string.IsNullOrEmpty(rxText) ? "(empty)" : rxText);
        }

        protected override ComparerResult CompareObjects(object? x, object? y, Tolerance tolerance)
            => tolerance switch
            {
                TextCaseTolerance text => CompareObjects(x, y, text.Comparison),
                TextSingleMethodTolerance single => CompareObjects(x, y, single),
                TextMultipleMethodsTolerance multiple => CompareObjects(x, y, multiple),
                _ => throw new ArgumentException("Tolerance must be of type 'TextTolerance'")
            };

        protected ComparerResult CompareObjects(object? x, object? y, StringComparer comparer)
            => CompareStrings(x as string, y as string, comparer);

        protected ComparerResult CompareObjects(object? x, object? y, TextSingleMethodTolerance tolerance)
            => CompareStrings(x as string, y as string, tolerance);

        protected ComparerResult CompareObjects(object? x, object? y, TextMultipleMethodsTolerance tolerance)
            => CompareStrings(x as string, y as string, tolerance);

        protected ComparerResult CompareStrings(string? x, string? y, StringComparer comparer)
            => IsEqual(x, y, comparer) ? ComparerResult.Equality : new ComparerResult(string.IsNullOrEmpty(x) ? "(empty)" : x);

        protected ComparerResult CompareStrings(string? x, string? y, TextSingleMethodTolerance tolerance)
        {
            if (x is null || y is null)
                return new ComparerResult("Can't apply distance if one of them is null");

            var distance = tolerance.Implementation.Invoke(x, y);

            if (tolerance.Predicate.Invoke(distance, tolerance.Value))
                return ComparerResult.Equality;
            else
                return new ComparerResult(distance.ToString());
        }

        protected ComparerResult CompareStrings(string? x, string? y, TextMultipleMethodsTolerance tolerance)
        {
            if (x is null || y is null)
                return new ComparerResult("different");

            if (tolerance.Implementation.Invoke(x, y))
                return ComparerResult.Equality;
            else
                return new ComparerResult("different");
        }

        protected override ComparerResult CompareObjects(object? x, object? y, Rounding rounding)
            => throw new NotImplementedException("You cannot compare with a text comparer and a rounding.");

        protected bool IsEqual(string? x, string? y, StringComparer comparer)
        {
            //quick check
            if (comparer.Compare(x, y) == 0)
                return true;

            if (x == "(empty)" && string.IsNullOrEmpty(y))
                return true;

            if (y == "(empty)" && string.IsNullOrEmpty(x))
                return true;

            if (x == "(blank)" && string.IsNullOrWhiteSpace(y))
                return true;

            if (y == "(blank)" && string.IsNullOrWhiteSpace(x))
                return true;

            return false;
        }

        protected override bool IsValidObject(object x)
            => true;
    }
}
