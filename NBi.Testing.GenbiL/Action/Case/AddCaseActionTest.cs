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

namespace NBi.Testing.GenbiL.Action.Case
{
    public class AddCaseActionTest
    {
        [Test]
        public void Execute_SecondColumn_ColumnAdded()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(newRow);

            var action = new AddCaseAction("myColumn");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
        }

        [Test]
        public void Execute_SecondColumn_ColumnAddedWithDefaultValue()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(newRow);

            var action = new AddCaseAction("myColumn");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumnWithSpecifiedDefaultValue_ColumnAddedWithDefaultValue()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            newRow[0] = "firstCell";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(newRow);

            var action = new AddCaseAction("myColumn", "value");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo("value"));
        }
    }
}
