using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace NBi.GenbiL.Action.Case
{
    public class FilterCaseAction : AbstractCompareCaseAction
    {
        public IEnumerable<string> Values { get; set; }
        public string Column { get; set; }
        public bool Negation { get; set; }
        public OperatorType Operator { get; set; }

        public FilterCaseAction(string column, OperatorType @operator, IEnumerable<string> values, bool negation)
        {
            Values = values;
            Operator = @operator;
            Column = column;
            Negation = negation;
        }

        public override void Execute(CaseSet testCases)
        {
            if (!testCases.Variables.Contains(Column))
                throw new ArgumentOutOfRangeException(nameof(Column));

            var compare = AssignCompare(Operator);

            var index = testCases.Variables.ToList().IndexOf(Column);

            DataTableReader? dataReader = null;
            var filteredRows = testCases.Content.AsEnumerable().Where(row => compare(row[index].ToString() ?? string.Empty, Values) != Negation);
            if (filteredRows.Any())
            {
                var filteredTable = filteredRows.CopyToDataTable();
                dataReader = filteredTable.CreateDataReader();
            }

            testCases.Content.Clear();
            if (dataReader != null)
                testCases.Content.Load(dataReader, LoadOption.PreserveChanges);

            testCases.Content.AcceptChanges();
        }

        public override string Display
            => $"Filtering on column '{Column}' all instances {(Negation ? "not " : string.Empty)}{GetOperatorText(Operator)} '{string.Join("', '", Values)}'";
    }
}
