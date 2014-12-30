using System;
using System.Linq;
using NBi.Service;
using System.Collections.Generic;
using NBi.GenbiL.Stateful;
using System.Data;

namespace NBi.GenbiL.Action.Case
{
    public class ReplaceCaseAction : ICaseAction
    {
        public string Column { get; set; }
        public string NewValue { get; set; }

        public ReplaceCaseAction(string column, string newValue)
        {
            Column = column;
            NewValue = newValue;
        }

        public void Execute(GenerationState state)
        {
            var scope = state.TestCaseSetCollection.Scope;

            if (!scope.Variables.Contains(Column))
                throw new ArgumentException(string.Format("No column named '{0}' has been found.", Column));

            var index = scope.Content.Columns.IndexOf(Column);

            foreach (DataRow row in scope.Content.Rows)
            {
                if (Condition(row, index))
                    row[index] = NewValue;
            }

            scope.Content.AcceptChanges();
        }

        protected virtual bool Condition(DataRow row, int columnIndex)
        {
            return true;
        }

        public virtual string Display
        {
            get
            {
                return string.Format(
                        "Replacing content of column '{0}' with value '{1}'"
                        , Column
                        , NewValue);
            }
        }
    }
}
