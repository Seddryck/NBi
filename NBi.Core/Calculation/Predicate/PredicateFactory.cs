using NBi.Core.Calculation.Predicate.Boolean;
using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.Calculation.Predicate.Numeric;
using NBi.Core.Calculation.Predicate.Text;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    class PredicateFactory
    {
        public IPredicate Instantiate(ComparerType comparerType, ColumnType columnType, bool not, object reference, string culture, StringComparison stringComparison, object secondOperand)
        {
            switch (columnType)
            {
                case ColumnType.Text:
                    switch (comparerType)
                    {
                        case ComparerType.LessThan: return new TextLessThan(not, reference);
                        case ComparerType.LessThanOrEqual: return new TextLessThanOrEqual(not, reference);
                        case ComparerType.Equal: return new TextEqual(not, reference);
                        case ComparerType.MoreThanOrEqual: return new TextMoreThanOrEqual(not, reference);
                        case ComparerType.MoreThan: return new TextMoreThan(not, reference);
                        case ComparerType.Null: return new TextNull(not);
                        case ComparerType.Empty: return new TextEmpty(not);
                        case ComparerType.NullOrEmpty: return new TextNullOrEmpty(not);
                        case ComparerType.LowerCase: return new TextLowerCase(not);
                        case ComparerType.UpperCase: return new TextUpperCase(not);
                        case ComparerType.StartsWith: return new TextStartsWith(not, reference, stringComparison);
                        case ComparerType.EndsWith: return new TextEndsWith(not, reference, stringComparison);
                        case ComparerType.Contains: return new TextContains(not, reference, stringComparison);
                        case ComparerType.MatchesRegex: return new TextMatchesRegex(not, reference, stringComparison);
                        case ComparerType.MatchesNumeric: return new TextMatchesNumeric(not, culture);
                        case ComparerType.MatchesDate: return new TextMatchesDate(not, culture);
                        case ComparerType.MatchesTime: return new TextMatchesTime(not, culture);
                        case ComparerType.MatchesDateTime: return new TextMatchesDateTime(not, culture);
                        case ComparerType.AnyOf: return new TextAnyOf(not, reference, stringComparison);
                        default:
                            throw new ArgumentOutOfRangeException($"Text columns don't support  the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ColumnType.Numeric:
                    switch (comparerType)
                    {
                        case ComparerType.LessThan: return new NumericLessThan(not, reference);
                        case ComparerType.LessThanOrEqual: return new NumericLessThanOrEqual(not, reference);
                        case ComparerType.Equal: return new NumericEqual(not, reference);
                        case ComparerType.MoreThanOrEqual: return new NumericMoreThanOrEqual(not, reference);
                        case ComparerType.MoreThan: return new NumericMoreThan(not, reference);
                        case ComparerType.Null: return new NumericNull(not);
                        case ComparerType.WithinRange: return new NumericWithinRange(not, reference);
                        case ComparerType.Integer: return new NumericInteger(not);
                        case ComparerType.Modulo: return new NumericModulo(not, secondOperand, reference);
                        default:
                            throw new ArgumentOutOfRangeException($"Numeric columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ColumnType.DateTime:
                    switch (comparerType)
                    {
                        case ComparerType.LessThan: return new DateTimeLessThan(not, reference);
                        case ComparerType.LessThanOrEqual: return new DateTimeLessThanOrEqual(not, reference);
                        case ComparerType.Equal: return new DateTimeEqual(not, reference);
                        case ComparerType.MoreThanOrEqual: return new DateTimeMoreThanOrEqual(not, reference);
                        case ComparerType.MoreThan: return new DateTimeMoreThan(not, reference);
                        case ComparerType.Null: return new DateTimeNull(not);
                        case ComparerType.WithinRange: return new DateTimeWithinRange(not, reference);
                        case ComparerType.OnTheDay: return new DateTimeOnTheDay(not);
                        case ComparerType.OnTheHour: return new DateTimeOnTheHour(not);
                        case ComparerType.OnTheMinute: return new DateTimeOnTheMinute(not);
                        default:
                            throw new ArgumentOutOfRangeException($"DateTime columns don't support the '{comparerType.ToString().ToDashedCase()}' comparer.");
                    }
                case ColumnType.Boolean:
                    switch (comparerType)
                    {
                        case ComparerType.Equal: return new BooleanEqual(not, reference);
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

        public IPredicate Instantiate(IPredicateInfo info)
            => Instantiate(info.ComparerType, info.ColumnType, info.Not
                , info is IReferencePredicateInfo ? (info as IReferencePredicateInfo).Reference : null
                , info is ICultureSensitivePredicateInfo ? (info as ICultureSensitivePredicateInfo).Culture : null
                , info is ICaseSensitivePredicateInfo ? (info as ICaseSensitivePredicateInfo).StringComparison : StringComparison.InvariantCulture
                , info is ISecondOperandPredicateInfo ? (info as ISecondOperandPredicateInfo).SecondOperand : null
                );

        public IPredicate Instantiate(IPredicateInfo info, IDictionary<string, ITestVariable> variables)
        {
            object reference = null;
            if (info is IReferencePredicateInfo)
            {
                reference = (info as IReferencePredicateInfo).Reference;
                if ((info as IReferencePredicateInfo).Reference is string)
                    if (((info as IReferencePredicateInfo).Reference as string).StartsWith("@"))
                    {
                        if (variables == null)
                            throw new ArgumentException("The dictionary of variables can't be null", nameof(variables));

                        var key = ((info as IReferencePredicateInfo).Reference as string).Substring(1);
                        if (!variables.ContainsKey(key))
                            throw new NBiException($"The predicate uses the variable '{key}' as a reference but this variable is not defined.");
                        reference = variables[key];
                    }
            }

            return Instantiate(info.ComparerType, info.ColumnType, info.Not
                , reference
                , (info as ICultureSensitivePredicateInfo)?.Culture
                , (info as ICaseSensitivePredicateInfo)?.StringComparison ?? StringComparison.InvariantCulture
                , (info as ISecondOperandPredicateInfo)?.SecondOperand
                );
        }
    }
}
