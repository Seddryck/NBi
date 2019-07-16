using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class TrimCaseAction : ICaseAction
    {
        public IEnumerable<string> ColumnNames { get; private set; }
        public DirectionType Direction { get; private set; }
        public TrimCaseAction(IEnumerable<string> columnNames, DirectionType direction)
        {
            ColumnNames = columnNames;
            Direction = direction;
        }

        public void Execute(GenerationState state)
        {
            var columnNames = ColumnNames;
            if (columnNames == null || columnNames.Count() == 0)
                columnNames = state.TestCaseCollection.Scope.Variables;

            foreach (var columnName in columnNames)
            {
                if (!state.TestCaseCollection.Scope.Variables.Contains(columnName))
                    throw new ArgumentOutOfRangeException($"No column named '{columnName}' has been found.");

                var index = state.TestCaseCollection.Scope.Variables.ToList().FindIndex(v => v == columnName);

                foreach (DataRow row in state.TestCaseCollection.Scope.Content.Rows)
                {
                    if ((string)row[columnName] != "(none)")
                        row[columnName] = Trim((string)row[columnName]);
                }
            }
        }

        private string Trim(string value)
        {
            switch (Direction)
            {
                case DirectionType.Both:
                    return value.Trim();
                case DirectionType.Left:
                    return value.TrimStart();
                case DirectionType.Right:
                    return value.TrimEnd();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual string Display
        {
            get => $"Trimming some columns";
        }
    }
}
