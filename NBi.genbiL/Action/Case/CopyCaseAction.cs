using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CopyCaseAction : ICaseAction
    {
        public string From { get; set; }
        public string To { get; set; }

        public CopyCaseAction(string from, string to)
        {
            From = from;
            To = to;
        }

        public void Execute(GenerationState state)
        {
            if (!state.TestCaseCollection.ItemExists(From))
                throw new ArgumentException($"The set of test-cases named '{From}' doesn't exist.", nameof(From));

            if (state.TestCaseCollection.ItemExists(To))
                throw new ArgumentException($"The set of test-cases named '{To}' already exists. The copy command cannot be performed on an existing test cases set", nameof(To));

            var dataReader = state.TestCaseCollection.Item(From).Content.CreateDataReader();

            state.TestCaseCollection.Item(To).Content.Clear();
            state.TestCaseCollection.Item(To).Content.Load(dataReader, LoadOption.PreserveChanges);
            state.TestCaseCollection.Item(To).Content.AcceptChanges();

            state.TestCaseCollection.Item(To).Variables.Clear();
            foreach (DataColumn col in state.TestCaseCollection.Item(To).Content.Columns)
                state.TestCaseCollection.Item(To).Variables.Add(col.ColumnName);
        }

        public virtual string Display
            { get => "Copying set of test-cases '{From}' to '{To}'"; }
    }
}
