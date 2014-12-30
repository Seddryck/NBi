using System;
using System.Linq;
using NBi.Service;
using System.Collections.Generic;
using NBi.GenbiL.Stateful;
using System.Data;
using System.Text.RegularExpressions;

namespace NBi.GenbiL.Action.Case
{
    public class ReplaceConditionalCaseAction : ReplaceCaseAction
    {
        public IEnumerable<string> Values { get; set; }
        public bool Negation { get; set; }
        public Operator Operator { get; set; }
        private Func<string, IEnumerable<string>, bool> compareMultiple;

        public ReplaceConditionalCaseAction(string column, string newValue, Operator @operator, IEnumerable<string> values, bool negation)
            : base(column, newValue)
        {
            Values = values;
            Operator = @operator;
            Negation = negation;

            AssignCompareMultiple(@operator);
        }

        private void AssignCompareMultiple(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    compareMultiple = Equal;
                    break;
                case Operator.Like:
                    compareMultiple = Like;
                    break;
                default:
                    break;
            }
        }

        private bool Like(string value, string pattern)
        {
            //Turn a SQL-like-pattern into regex, by turning '%' into '.*'
            //Doesn't handle SQL's underscore into single character wild card '.{1,1}',
            //        or the way SQL uses square brackets for escaping.
            //(Note the same concept could work for DOS-style wildcards (* and ?)
            var regex = new Regex("^" + pattern
                           .Replace(".", "\\.")
                           .Replace("%", ".*")
                           .Replace("\\.*", "\\%")
                           + "$");

            return regex.IsMatch(value);
        }

        private bool Like(string value, IEnumerable<string> patterns)
        {
            var result = false;
            foreach (var pattern in patterns)
                result |= Like(value, pattern);
            return result;
        }

        private bool Equal(string value, IEnumerable<string> patterns)
        {
            var result = false;
            foreach (var pattern in patterns)
                result |= value == pattern;
            return result;
        }

        protected override bool Condition(DataRow row, int columnIndex)
        {
            return compareMultiple(row[columnIndex].ToString(), Values) != Negation;
        }

        public override string Display
        {
            get
            {
                return base.Display + string.Format(
                        " when values {0}{1} '{2}'"
                        , Negation ? "not " : string.Empty
                        , GetOperatorText(Operator)
                        , string.Join("', '", Values));
            }
        }
        private string GetOperatorText(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    return "equal to";
                case Operator.Like:
                    return "like";
                default:
                    break;
            }
            throw new ArgumentException();
        }
    }
}
