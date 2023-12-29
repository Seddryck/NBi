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
using Exssif = Expressif.Predicates;
using Expressif.Values.Casters;

namespace NBi.Core.Calculation.Asserting
{
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
                //ComparerType.WithinRange => new WithinInterval(referenceFunc),
                ComparerType.Integer => new Integer(),
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
                //ComparerType.WithinRange=>new ContainedIn(...),
                ComparerType.OnTheDay => new OnTheDay(),
                ComparerType.OnTheHour => new OnTheHour(),
                ComparerType.OnTheMinute => new OnTheMinute(),
                _ =>
                       throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer."),
            };
        }

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
}
