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
        public IPredicate Get(IPredicateInfo info)
        {
            switch (info.ColumnType)
            {
                case NBi.Core.ResultSet.ColumnType.Text:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new TextLessThan();
                        case ComparerType.LessThanOrEqual: return new TextLessThanOrEqual();
                        case ComparerType.Equal: return new TextEqual();
                        case ComparerType.MoreThanOrEqual: return new TextMoreThanOrEqual();
                        case ComparerType.MoreThan: return new TextMoreThan();
                        default:
                            break;
                    }
                    break;
                case NBi.Core.ResultSet.ColumnType.Numeric:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new NumericLessThan();
                        case ComparerType.LessThanOrEqual: return new NumericLessThanOrEqual();
                        case ComparerType.Equal: return new NumericEqual();
                        case ComparerType.MoreThanOrEqual: return new NumericMoreThanOrEqual();
                        case ComparerType.MoreThan: return new NumericMoreThan();
                        default:
                            break;
                    }
                    break;
                case NBi.Core.ResultSet.ColumnType.DateTime:
                    switch (info.ComparerType)
                    {
                        case ComparerType.LessThan: return new DateTimeLessThan();
                        case ComparerType.LessThanOrEqual: return new DateTimeLessThanOrEqual();
                        case ComparerType.Equal: return new DateTimeEqual();
                        case ComparerType.MoreThanOrEqual: return new DateTimeMoreThanOrEqual();
                        case ComparerType.MoreThan: return new DateTimeMoreThan();
                        default:
                            break;
                    }
                    break;
                case NBi.Core.ResultSet.ColumnType.Boolean:
                    if (info.ComparerType == ComparerType.Equal)
                        return new BooleanEqual();
                    else
                        throw new ArgumentException("Boolean columns only support Equal comparers and not More or Less Than comparers.");
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
