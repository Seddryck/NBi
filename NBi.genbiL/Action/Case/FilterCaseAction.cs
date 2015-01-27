using System;
using System.Linq;
using NBi.Service;
using System.Collections.Generic;
using NBi.GenbiL.Stateful;
using System.Data;
using System.Text.RegularExpressions;

namespace NBi.GenbiL.Action.Case
{
    class FilterCaseAction : ICaseAction
    {
        public IEnumerable<string> Values { get; set; }
        public string Column { get; set; }
        public bool Negation { get; set; }
        public Operator Operator { get; set; }

        private Func<string, IEnumerable<string>, bool> compareMultiple;

        public FilterCaseAction(string column, Operator @operator, IEnumerable<string> values, bool negation)
        {
            Values = values;
            Operator = @operator;
            Column = column;
            Negation = negation;
        }
        public void Execute(GenerationState state)
        {
            var scope = state.TestCaseSetCollection.Scope;
            if (!scope.Variables.Contains(Column))
                throw new ArgumentOutOfRangeException("variableName");
            var index = scope.Content.Columns.IndexOf(Column);

            AssignCompareMultiple(Operator);

            
            DataTableReader dataReader = null;
            var filteredRows = scope.Content.AsEnumerable().Where(row => compareMultiple(row[index].ToString(), Values) != Negation);
            if (filteredRows.Count() > 0)
            {
                var filteredTable = filteredRows.CopyToDataTable();
                dataReader = filteredTable.CreateDataReader();
            }

            scope.Content.Clear();
            if (dataReader != null)
                scope.Content.Load(dataReader, LoadOption.PreserveChanges);

            scope.Content.AcceptChanges();
        }

        public virtual string Display
        {
            get
            {
                return string.Format(
                    "Filtering on column '{0}' all instances {1}{2} '{3}'"
                    , Column
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
    }
}
