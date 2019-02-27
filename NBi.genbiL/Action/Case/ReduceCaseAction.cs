using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class ReduceCaseAction : ICaseAction
    {
        public List<string> columnNames { get; set; }

        public ReduceCaseAction(IEnumerable<string> variableNames)
        {
            this.columnNames = new List<string>(variableNames);
        }

        public void Execute(GenerationState state)
        {

            foreach (DataRow row in state.TestCaseCollection.Scope.Content.Rows)
            {
                foreach (var columnName in columnNames)
                {
                    if (row[columnName] is IEnumerable<string> list)
                        row[columnName] = list.Distinct().ToArray();
                }
            }
        }

        public string Display
        {
            get
            {
                if (columnNames.Count == 1)
                    return string.Format("Reducing the length of groups for column '{0}'", columnNames[0]);
                else
                    return string.Format("Reducing the length of groups for columns '{0}'", String.Join("', '", columnNames));
            }
        }
    }
}
