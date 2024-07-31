using System;
using System.Linq;
using System.Collections.Generic;
using NBi.GenbiL.Stateful;
using System.Data;

namespace NBi.GenbiL.Action.Case
{
    class ReplaceCaseAction : AbstractCompareCaseAction
    {
        public string Column { get; set; }
        public string NewValue { get; set; }
        public IEnumerable<string> Values { get; set; } = [];
        public bool Negation { get; set; }
        public OperatorType Operator { get; set; }

        public ReplaceCaseAction(string column, string newValue)
        {
            Column = column;
            NewValue = newValue;
        }

        public ReplaceCaseAction(string column, string newValue, OperatorType @operator, IEnumerable<string> values, bool negation)
            : this(column, newValue)
        {
            Values = values;
            Operator = @operator;
            Negation = negation;
        }

        public override void Execute(CaseSet testCases)
        {
            if (!Values.Any())
                Replace(testCases, Column, NewValue);
            else
                Replace(testCases, Column, NewValue, Operator, Negation, Values);
        }

        public virtual void Replace(CaseSet testCases, string columnName, string newValue)
        {
            if (!testCases.Variables.Contains(columnName))
                throw new ArgumentException($"No column named '{columnName}' has been found.");

            var index = testCases.Variables.ToList().IndexOf(columnName);

            foreach (DataRow row in testCases.Content.Rows)
                row[index] = newValue;

            testCases.Content.AcceptChanges();
        }

        public void Replace(CaseSet testCases, string columnName, string newValue, OperatorType @operator, bool negation, IEnumerable<string> values)
        {
            if (!testCases.Variables.Contains(columnName))
                throw new ArgumentException($"No column named '{columnName}' has been found.");

            var compare = AssignCompare(@operator);

            var index = testCases.Variables.ToList().IndexOf(columnName);

            foreach (DataRow row in testCases.Content.Rows)
            {
                if (compare(row[index].ToString() ?? "(null)", values) != negation)
                    row[index] = newValue;
            }

            testCases.Content.AcceptChanges();
        }

        public override string Display
        {
            get
            {
                var display = string.Format(
                        "Replacing content of column '{0}' with value '{1}'"
                        , Column
                        , NewValue);

                if (Values != null && Values.Any())
                    display += string.Format(
                        " when values {0}{1} '{2}'"
                        , Negation ? "not " : string.Empty
                        , GetOperatorText(Operator)
                        , string.Join("', '", Values));

                return display;
            }
        }
    }
}
