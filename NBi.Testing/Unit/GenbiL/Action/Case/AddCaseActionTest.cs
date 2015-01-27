using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class AddCaseActionTest
    {
        [Test]
        public void Execute_SecondColumn_ColumnAdded()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Scope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(newRow);

            var action = new AddCaseAction("myColumn");
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(2));
        }

        [Test]
        public void Execute_SecondColumn_ColumnAddedWithDefaultValue()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Scope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(newRow);

            var action = new AddCaseAction("myColumn");
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0].ItemArray[1], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumnWithSpecifiedDefaultValue_ColumnAddedWithDefaultValue()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Scope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(newRow);

            var action = new AddCaseAction("myColumn", "value");
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0].ItemArray[1], Is.EqualTo("value"));
        }
    }
}
