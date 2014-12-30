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
    public class MergeCaseActionTest
    {
        [Test]
        public void Execute_TwoScopesWithIdenticalColumns_CurrentScopeHasMoreRows()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Item("firstScope").Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell-firstScope";
            state.TestCaseCollection.Scope.Content.Rows.Add(newRow);

            state.TestCaseCollection.Item("secondScope").Content.Columns.Add("firstColumn");
            var newRowBis = state.TestCaseCollection.Item("secondScope").Content.NewRow();
            newRowBis[0] = "firstCell-secondScope";
            state.TestCaseCollection.Item("secondScope").Content.Rows.Add(newRowBis);

            var action = new MergeCaseAction("secondScope");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Execute_TwoScopesWithDifferentColumns_CurrentScopeHasMoreRowsAndNewColumn()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Item("firstScope").Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell-firstScope";
            state.TestCaseCollection.Scope.Content.Rows.Add(newRow);

            state.TestCaseCollection.Item("secondScope").Content.Columns.Add("secondColumn");
            var newRowBis = state.TestCaseCollection.Item("secondScope").Content.NewRow();
            newRowBis[0] = "firstCell-secondScope";
            state.TestCaseCollection.Item("secondScope").Content.Rows.Add(newRowBis);

            var action = new MergeCaseAction("secondScope");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(2));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell-firstScope"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0].IsNull(1), Is.True);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1].ItemArray[1], Is.EqualTo("firstCell-secondScope"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1].IsNull(0), Is.True);
        }

    }
}
