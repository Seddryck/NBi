using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressif.Predicates.Text;
using Expressif.Predicates.Numeric;
using Expressif.Predicates.Temporal;
using Expressif.Predicates.Boolean;
using Expressif.Values;
using Exssif = Expressif.Predicates;
using Expressif.Values.Casters;
using System.Text.RegularExpressions;

namespace NBi.Core.Calculation.Asserting;

public class PredicateFactory
{
    public IPredicate Instantiate(PredicateArgs args)
        => new Predicate(CreateFrom(args), args.Not);

    public IPredicate Instantiate(PredicateArgs[] args, CombinationOperator @operator)
    {
        var combiner = new Exssif.PredicateCombiner();
        var combination = args.First().Not
                            ? combiner.WithNot(CreateFrom(args.First()))
                            : combiner.With(CreateFrom(args.First()));
        foreach (var arg in args.Skip(1))
        {
            var right = CreateFrom(arg);
            combination = @operator switch
            {
                CombinationOperator.Or when arg.Not => combination.OrNot(right),
                CombinationOperator.Or => combination.Or(right),
                CombinationOperator.XOr when arg.Not => combination.Xor(right),
                CombinationOperator.XOr => combination.Xor(right),
                CombinationOperator.And when arg.Not => combination.And(right),
                CombinationOperator.And => combination.And(right),
                _ => throw new NotImplementedException(),
            };
        }
        return new Predicate(combination.Build());
    }

    internal Exssif.IPredicate CreateFrom(PredicateArgs args)
        => CreateFrom(args.ComparerType, args.ColumnType, args.Not
            , (args as ReferencePredicateArgs)?.Reference
            , (args as CultureSensitivePredicateArgs)?.Culture
            , (args as CaseSensitivePredicateArgs)?.StringComparison ?? StringComparison.InvariantCulture
            , (args as SecondOperandPredicateArgs)?.SecondOperand
            );

    internal Exssif.IPredicate CreateFrom(ComparerType comparerType, ColumnType columnType, bool not, IResolver? reference, string? culture, StringComparison? stringComparison, object? secondOperand)
        => columnType switch
        {
            ColumnType.Text => CreateFromText(comparerType, reference, culture, stringComparison),
            ColumnType.Numeric => CreateFromNumeric(comparerType, reference, secondOperand),
            ColumnType.DateTime => CreateFromDateTime(comparerType, reference),
            ColumnType.Boolean => CreateFromBoolean(comparerType, reference),
            _ => throw new NotImplementedException(),
        };

    #region Internal Buil of Expressif Predicates

    protected Exssif.IPredicate CreateFromText(ComparerType comparerType, IResolver? reference, string? culture, StringComparison? stringComparison)
    {
        var comparer = StringComparer.FromComparison(stringComparison ?? StringComparison.InvariantCultureIgnoreCase);
        string cultureFunc() => culture ?? "";
        string referenceFunc() => (string)((IScalarResolver)reference!).Execute()!;

        return comparerType switch
        {
            ComparerType.LessThan => new SortedBefore(referenceFunc),
            ComparerType.LessThanOrEqual => new SortedBeforeOrEquivalentTo(referenceFunc),
            ComparerType.Equal => new EquivalentTo(referenceFunc),
            ComparerType.MoreThanOrEqual => new SortedAfterOrEquivalentTo(referenceFunc),
            ComparerType.MoreThan => new SortedAfter(referenceFunc),
            ComparerType.Null => new Exssif.Special.Null(),
            ComparerType.Empty => new Empty(),
            ComparerType.NullOrEmpty => new EmptyOrNull(),
            ComparerType.LowerCase => new LowerCase(),
            ComparerType.UpperCase => new UpperCase(),
            ComparerType.StartsWith => new StartsWith(referenceFunc, comparer),
            ComparerType.EndsWith => new EndsWith(referenceFunc, comparer),
            ComparerType.Contains => new Contains(referenceFunc, comparer),
            ComparerType.MatchesRegex => new MatchesRegex(referenceFunc, comparer),
            ComparerType.MatchesNumeric => new MatchesNumeric(cultureFunc),
            ComparerType.MatchesDate => new MatchesDate(cultureFunc),
            ComparerType.MatchesTime => new MatchesTime(cultureFunc),
            ComparerType.MatchesDateTime => new MatchesDateTime(cultureFunc),
            ComparerType.AnyOf => new AnyOf(() => (IEnumerable<string>)((IScalarResolver)reference!).Execute()!, comparer),
            _ => throw new ArgumentOutOfRangeException($"Text columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer."),
        };
    }

    protected Exssif.IPredicate CreateFromNumeric(ComparerType comparerType, IResolver? reference, object? secondOperand)
    {
        decimal referenceFunc() => Convert.ToDecimal(((IScalarResolver)reference!).Execute()!);

        return comparerType switch
        {
            ComparerType.LessThan => new LessThan(referenceFunc),
            ComparerType.LessThanOrEqual => new LessThanOrEqual(referenceFunc),
            ComparerType.Equal => new EqualTo(referenceFunc),
            ComparerType.MoreThanOrEqual => new GreaterThanOrEqual(referenceFunc),
            ComparerType.MoreThan => new GreaterThan(referenceFunc),
            ComparerType.Null => new Exssif.Special.Null(),
            ComparerType.WithinRange => new WithinInterval(() => BuildInterval<decimal>(reference!)),
            ComparerType.Integer => new WholeNumber(),
            ComparerType.Modulo => new Modulo(() => new NumericCaster().Cast(secondOperand ?? 0), referenceFunc),
            _ =>
               throw new ArgumentOutOfRangeException($"Numeric columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer."),
        };
    }

    protected Exssif.IPredicate CreateFromDateTime(ComparerType comparerType, IResolver? reference)
    {
        DateTime referenceFunc() => (DateTime)((IScalarResolver)reference!).Execute()!;

        return comparerType switch
        {
            ComparerType.LessThan => new Before(referenceFunc),
            ComparerType.LessThanOrEqual => new BeforeOrSameInstant(referenceFunc),
            ComparerType.Equal => new SameInstant(referenceFunc),
            ComparerType.MoreThanOrEqual => new AfterOrSameInstant(referenceFunc),
            ComparerType.MoreThan => new After(referenceFunc),
            ComparerType.Null => new Exssif.Special.Null(),
            ComparerType.WithinRange => new ContainedIn(() => BuildInterval<DateTime>(reference!)),
            ComparerType.OnTheDay => new OnTheDay(),
            ComparerType.OnTheHour => new OnTheHour(),
            ComparerType.OnTheMinute => new OnTheMinute(),
            _ =>
                   throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer."),
        };
    }

    private static Interval<T> BuildInterval<T>(IResolver reference)
        where T : struct, IComparable
    {
        var value = ((string)reference.Execute()!).Trim();

        // NBi range definitions predate Expressif's I[...] syntax.
        var range = Regex.Match(value, @"^([\[\]])\s*([^;]+?)\s*;\s*([^;]+?)\s*([\[\]])$");
        if (range.Success)
        {
            var lower = FormatIntervalBound<T>(range.Groups[2].Value);
            var upper = FormatIntervalBound<T>(range.Groups[3].Value);
            value = $"I{range.Groups[1].Value}{lower}, {upper}{range.Groups[4].Value}";
        }
        else if (value.StartsWith('(') && value.EndsWith(')'))
        {
            var shorthand = value[1..^1].Trim();
            var comparison = Regex.Match(shorthand, @"^([<>]=?)\s*(.+)$");
            var bound = FormatIntervalBound<T>(comparison.Groups[2].Value.Trim());
            value = comparison.Groups[1].Value switch
            {
                ">" => $"I]{bound}, +INF]",
                ">=" => $"I[{bound}, +INF]",
                "<" => $"I[-INF, {bound}[",
                "<=" => $"I[-INF, {bound}]",
                _ => shorthand.ToLowerInvariant() switch
                {
                    "positive" or "0+" => "I[0, +INF]",
                    "negative" or "0-" => "I[-INF, 0]",
                    "absolutely-positive" or "+" => "I]0, +INF]",
                    "absolutely-negative" or "-" => "I[-INF, 0[",
                    _ => value,
                },
            };
        }

        return (Interval<T>)new IntervalBuilder().Create(value);
    }

    private static string FormatIntervalBound<T>(string bound)
        => typeof(T) == typeof(DateTime) && bound.Length > 0 && char.IsAsciiDigit(bound[0])
            ? $"#\"{bound.Replace(' ', 'T')}\""
            : bound;

    protected Exssif.IPredicate CreateFromBoolean(ComparerType comparerType, IResolver? reference)
    {
        return comparerType switch
        {
            ComparerType.Equal => new IdenticalTo(() => (bool)((IScalarResolver)reference!).Execute()!),
            ComparerType.Null => new Exssif.Special.Null(),
            ComparerType.True => new True(),
            ComparerType.False => new False(),
            _ =>
                   throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer."),
        };
    }

    #endregion
}
