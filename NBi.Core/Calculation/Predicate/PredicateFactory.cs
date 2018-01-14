using NBi.Core.Calculation.Predicate.Boolean;
using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.Calculation.Predicate.Numeric;
using NBi.Core.Calculation.Predicate.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    class PredicateFactory
    {
        public IPredicate Instantiate(IPredicateInfo info)
        {
            switch (info.ColumnType)
            {
                case ResultSet.ColumnType.Text:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new TextLessThan(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.LessThanOrEqual: return new TextLessThanOrEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Equal: return new TextEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThanOrEqual: return new TextMoreThanOrEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThan: return new TextMoreThan(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new TextNull(info.Not);
                        case ComparerType.Empty: return new TextEmpty(info.Not);
                        case ComparerType.NullOrEmpty: return new TextNullOrEmpty(info.Not);
                        case ComparerType.LowerCase: return new TextLowerCase(info.Not);
                        case ComparerType.UpperCase: return new TextUpperCase(info.Not);
                        case ComparerType.StartsWith: return new TextStartsWith(info.Not, (info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.EndsWith: return new TextEndsWith(info.Not, (info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.Contains: return new TextContains(info.Not, (info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.MatchesRegex: return new TextMatchesRegex(info.Not, (info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.MatchesNumeric: return new TextMatchesNumeric(info.Not, (info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesDate: return new TextMatchesDate(info.Not, (info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesTime: return new TextMatchesTime(info.Not, (info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesDateTime: return new TextMatchesDateTime(info.Not, (info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.WithinList: return new TextWithinList(info.Not, (info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        default:
                            throw new ArgumentOutOfRangeException($"Text columns don't support  the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.Numeric:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new NumericLessThan(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.LessThanOrEqual: return new NumericLessThanOrEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Equal: return new NumericEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThanOrEqual: return new NumericMoreThanOrEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThan: return new NumericMoreThan(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new NumericNull(info.Not);
                        case ComparerType.WithinRange: return new NumericWithinRange(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Integer: return new NumericInteger(info.Not);
                        case ComparerType.Modulo: return new NumericModulo(info.Not, (info as ISecondOperandPredicateInfo).SecondOperand, (info as IReferencePredicateInfo).Reference);
                        default:
                            throw new ArgumentOutOfRangeException($"Numeric columns don't support the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.DateTime:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new DateTimeLessThan(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.LessThanOrEqual: return new DateTimeLessThanOrEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Equal: return new DateTimeEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThanOrEqual: return new DateTimeMoreThanOrEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThan: return new DateTimeMoreThan(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new DateTimeNull(info.Not);
                        case ComparerType.WithinRange: return new DateTimeWithinRange(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.OnTheDay: return new DateTimeOnTheDay(info.Not);
                        case ComparerType.OnTheHour: return new DateTimeOnTheHour(info.Not);
                        case ComparerType.OnTheMinute: return new DateTimeOnTheMinute(info.Not);
                        default:
                            throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.Boolean:
                    switch (info.ComparerType)
                    {
                        case ComparerType.Equal: return new BooleanEqual(info.Not, (info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new BooleanNull(info.Not);
                        case ComparerType.True: return new BooleanTrue(info.Not);
                        case ComparerType.False: return new BooleanFalse(info.Not);
                        default:
                            throw new ArgumentOutOfRangeException($"Boolean columns only support Equal, Null, True and False comparers and not the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
