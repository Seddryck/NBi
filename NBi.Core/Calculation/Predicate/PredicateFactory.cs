using NBi.Core.Calculation.Predicate.Boolean;
using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.Calculation.Predicate.Numeric;
using NBi.Core.Calculation.Predicate.Text;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    public class PredicateFactory
    {
        private IPredicate Instantiate(ComparerType comparerType, ColumnType columnType, bool not, IResolver reference, string culture, StringComparison stringComparison, object secondOperand)
        {
            switch (columnType)
            {
                case ColumnType.Text:
                    switch (comparerType)
                    {
                        case ComparerType.LessThan: return new TextLessThan(not, (IScalarResolver)reference);
                        case ComparerType.LessThanOrEqual: return new TextLessThanOrEqual(not, (IScalarResolver)reference);
                        case ComparerType.Equal: return new TextEqual(not, (IScalarResolver)reference);
                        case ComparerType.MoreThanOrEqual: return new TextMoreThanOrEqual(not, (IScalarResolver)reference);
                        case ComparerType.MoreThan: return new TextMoreThan(not, (IScalarResolver)reference);
                        case ComparerType.Null: return new TextNull(not);
                        case ComparerType.Empty: return new TextEmpty(not);
                        case ComparerType.NullOrEmpty: return new TextNullOrEmpty(not);
                        case ComparerType.LowerCase: return new TextLowerCase(not);
                        case ComparerType.UpperCase: return new TextUpperCase(not);
                        case ComparerType.StartsWith: return new TextStartsWith(not, (IScalarResolver)reference, stringComparison);
                        case ComparerType.EndsWith: return new TextEndsWith(not, (IScalarResolver)reference, stringComparison);
                        case ComparerType.Contains: return new TextContains(not, (IScalarResolver)reference, stringComparison);
                        case ComparerType.MatchesRegex: return new TextMatchesRegex(not, (IScalarResolver)reference, stringComparison);
                        case ComparerType.MatchesNumeric: return new TextMatchesNumeric(not, culture);
                        case ComparerType.MatchesDate: return new TextMatchesDate(not, culture);
                        case ComparerType.MatchesTime: return new TextMatchesTime(not, culture);
                        case ComparerType.MatchesDateTime: return new TextMatchesDateTime(not, culture);
                        case ComparerType.AnyOf: return new TextAnyOf(not, (ISequenceResolver)reference, stringComparison);
                        default:
                            throw new ArgumentOutOfRangeException($"Text columns don't support  the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ColumnType.Numeric:
                    switch (comparerType)
                    {
                        case ComparerType.LessThan: return new NumericLessThan(not, (IScalarResolver)reference);
                        case ComparerType.LessThanOrEqual: return new NumericLessThanOrEqual(not, (IScalarResolver)reference);
                        case ComparerType.Equal: return new NumericEqual(not, (IScalarResolver)reference);
                        case ComparerType.MoreThanOrEqual: return new NumericMoreThanOrEqual(not, (IScalarResolver)reference);
                        case ComparerType.MoreThan: return new NumericMoreThan(not, (IScalarResolver)reference);
                        case ComparerType.Null: return new NumericNull(not);
                        case ComparerType.WithinRange: return new NumericWithinRange(not, (IScalarResolver)reference);
                        case ComparerType.Integer: return new NumericInteger(not);
                        case ComparerType.Modulo: return new NumericModulo(not, secondOperand, (IScalarResolver)reference);
                        default:
                            throw new ArgumentOutOfRangeException($"Numeric columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ColumnType.DateTime:
                    switch (comparerType)
                    {
                        case ComparerType.LessThan: return new DateTimeLessThan(not, (IScalarResolver)reference);
                        case ComparerType.LessThanOrEqual: return new DateTimeLessThanOrEqual(not, (IScalarResolver)reference);
                        case ComparerType.Equal: return new DateTimeEqual(not, (IScalarResolver)reference);
                        case ComparerType.MoreThanOrEqual: return new DateTimeMoreThanOrEqual(not, (IScalarResolver)reference);
                        case ComparerType.MoreThan: return new DateTimeMoreThan(not, (IScalarResolver)reference);
                        case ComparerType.Null: return new DateTimeNull(not);
                        case ComparerType.WithinRange: return new DateTimeWithinRange(not, (IScalarResolver)reference);
                        case ComparerType.OnTheDay: return new DateTimeOnTheDay(not);
                        case ComparerType.OnTheHour: return new DateTimeOnTheHour(not);
                        case ComparerType.OnTheMinute: return new DateTimeOnTheMinute(not);
                        default:
                            throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ColumnType.Boolean:
                    switch (comparerType)
                    {
                        case ComparerType.Equal: return new BooleanEqual(not, (IScalarResolver)reference);
                        case ComparerType.Null: return new BooleanNull(not);
                        case ComparerType.True: return new BooleanTrue(not);
                        case ComparerType.False: return new BooleanFalse(not);
                        default:
                            throw new ArgumentOutOfRangeException($"Boolean columns only support Equal, Null, True and False comparers and not the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException();
        }

        public IPredicate Instantiate(PredicateArgs args)
            => Instantiate(args.ComparerType, args.ColumnType, args.Not
                , (args as ReferencePredicateArgs)?.Reference
                , (args as CultureSensitivePredicateArgs)?.Culture
                , (args as CaseSensitivePredicateArgs)?.StringComparison ?? StringComparison.InvariantCulture
                , (args as SecondOperandPredicateArgs)?.SecondOperand
                );
    }
}
