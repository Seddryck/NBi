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
                        case ComparerType.LessThan: return new TextLessThan((info as IReferencePredicateInfo).Reference);
                        case ComparerType.LessThanOrEqual: return new TextLessThanOrEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Equal: return new TextEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThanOrEqual: return new TextMoreThanOrEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThan: return new TextMoreThan((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new TextNull();
                        case ComparerType.Empty: return new TextEmpty();
                        case ComparerType.NullOrEmpty: return new TextNullOrEmpty();
                        case ComparerType.LowerCase: return new TextLowerCase();
                        case ComparerType.UpperCase: return new TextUpperCase();
                        case ComparerType.StartsWith: return new TextStartsWith((info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.EndsWith: return new TextEndsWith((info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.Contains: return new TextContains((info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.MatchesRegex: return new TextMatchesRegex((info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        case ComparerType.MatchesNumeric: return new TextMatchesNumeric((info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesDate: return new TextMatchesDate((info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.MatchesTime: return new TextMatchesTime((info as ICultureSensitivePredicateInfo).Culture);
                        case ComparerType.WithinList: return new TextWithinList((info as IReferencePredicateInfo).Reference, (info as ICaseSensitivePredicateInfo).StringComparison);
                        default:
                            throw new ArgumentOutOfRangeException($"Text columns don't support  the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.Numeric:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new NumericLessThan((info as IReferencePredicateInfo).Reference);
                        case ComparerType.LessThanOrEqual: return new NumericLessThanOrEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Equal: return new NumericEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThanOrEqual: return new NumericMoreThanOrEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThan: return new NumericMoreThan((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new NumericNull();
                        case ComparerType.WithinRange: return new NumericWithinRange((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Integer: return new NumericInteger();
                        case ComparerType.Modulo: return new NumericModulo((info as ISecondOperandPredicateInfo).SecondOperand, (info as IReferencePredicateInfo).Reference);
                        default:
                            throw new ArgumentOutOfRangeException($"Numeric columns don't support the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.DateTime:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new DateTimeLessThan((info as IReferencePredicateInfo).Reference);
                        case ComparerType.LessThanOrEqual: return new DateTimeLessThanOrEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Equal: return new DateTimeEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThanOrEqual: return new DateTimeMoreThanOrEqual((info as IReferencePredicateInfo).Reference);
                        case ComparerType.MoreThan: return new DateTimeMoreThan((info as IReferencePredicateInfo).Reference);
                        case ComparerType.Null: return new DateTimeNull();
                        case ComparerType.WithinRange: return new DateTimeWithinRange((info as IReferencePredicateInfo).Reference);
                        case ComparerType.OnTheDay: return new DateTimeOnTheDay();
                        case ComparerType.OnTheHour: return new DateTimeOnTheHour();
                        case ComparerType.OnTheMinute: return new DateTimeOnTheMinute();
                        default:
                            throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{info.ComparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ResultSet.ColumnType.Boolean:
                    switch (info.ComparerType)
                    {
                        case ComparerType.Equal: return new BooleanEqual((info as IReferencePredicateInfo).Reference);
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
