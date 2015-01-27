using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful
{
    public class TestCaseSetState
    {
        private readonly DataTable content;
        public DataTable Content
        {
            get
            {
                return content;
            }
        }

        private readonly List<string> variables;
        public IReadOnlyList<string> Variables
        {
            get
            {
                SyncVariables();
                return variables;
            }
        }

        private void SyncVariables()
        {
            variables.Clear();
            foreach (DataColumn column in Content.Columns)
                variables.Add(column.ColumnName);
        }

        public TestCaseSetState()
        {
            content = new DataTable();
            variables = new List<string>();
        }
    }
}
