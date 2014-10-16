using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class ReplaceCaseActionTest
    {
        [Test]
        public void Execute_FirstColumn_ValueReplaced()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(newRow);

            var action = new ReplaceCaseAction("firstColumn", "new value");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0].ItemArray[0], Is.EqualTo("new value"));
        }
    }
}
