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
                        case ComparerType.LessThan: return new TextLessThan(info.Reference);
                        case ComparerType.LessThanOrEqual: return new TextLessThanOrEqual(info.Reference);
                        case ComparerType.Equal: return new TextEqual(info.Reference);
                        case ComparerType.MoreThanOrEqual: return new TextMoreThanOrEqual(info.Reference);
                        case ComparerType.MoreThan: return new TextMoreThan(info.Reference);
                        case ComparerType.Null: return new TextNull();
                        case ComparerType.Empty: return new TextEmpty();
                        case ComparerType.NullOrEmpty: return new TextNullOrEmpty();
                        case ComparerType.LowerCase: return new TextLowerCase();
                        case ComparerType.UpperCase: return new TextUpperCase();
                        case ComparerType.StartsWith: return new TextStartsWith(info.Reference, info.StringComparison);
                        case ComparerType.EndsWith: return new TextEndsWith(info.Reference, info.StringComparison);
                        case ComparerType.Contains: return new TextContains(info.Reference, info.StringComparison);
                        case ComparerType.MatchesRegex: return new TextMatchesRegex(info.Reference, info.StringComparison);
                        case ComparerType.MatchesNumeric: return new TextMatchesNumeric((info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesDate: return new TextMatchesDate((info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesTime: return new TextMatchesTime((info as ICultureSensitivePredicateInfo).Culture);
                        default:
                            throw new ArgumentOutOfRangeException($"Text columns don't support  the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.Numeric:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new NumericLessThan(info.Reference);
                        case ComparerType.LessThanOrEqual: return new NumericLessThanOrEqual(info.Reference);
                        case ComparerType.Equal: return new NumericEqual(info.Reference);
                        case ComparerType.MoreThanOrEqual: return new NumericMoreThanOrEqual(info.Reference);
                        case ComparerType.MoreThan: return new NumericMoreThan(info.Reference);
                        case ComparerType.Null: return new NumericNull();
                        case ComparerType.WithinRange: return new NumericWithinRange(info.Reference);
                        case ComparerType.Integer: return new NumericInteger();
                        case ComparerType.Modulo: return new NumericModulo(info.SecondOperand, info.Reference);
                        default:
                            throw new ArgumentOutOfRangeException($"Numeric columns don't support the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.DateTime:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new DateTimeLessThan(info.Reference);
                        case ComparerType.LessThanOrEqual: return new DateTimeLessThanOrEqual(info.Reference);
                        case ComparerType.Equal: return new DateTimeEqual(info.Reference);
                        case ComparerType.MoreThanOrEqual: return new DateTimeMoreThanOrEqual(info.Reference);
                        case ComparerType.MoreThan: return new DateTimeMoreThan(info.Reference);
                        case ComparerType.Null: return new DateTimeNull();
                        case ComparerType.WithinRange: return new DateTimeWithinRange(info.Reference);
                        case ComparerType.OnTheDay: return new DateTimeOnTheDay();
                        case ComparerType.OnTheHour: return new DateTimeOnTheHour();
                        case ComparerType.OnTheMinute: return new DateTimeOnTheMinute();
                        default:
                            throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.Boolean:
                    switch (info.ComparerType)
                    {
                        case ComparerType.Equal: return new BooleanEqual(info.Reference);
                        case ComparerType.Null: return new BooleanNull();
                        case ComparerType.True: return new BooleanTrue();
                        case ComparerType.False: return new BooleanFalse();
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
