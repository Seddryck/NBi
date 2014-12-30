using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class ReplaceConditionCaseActionTest
    {
        [Test]
        public void Execute_ReplaceSecondColumnWithCondition_ColumnReplaced()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseSetCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseSetCollection.Scope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(secondRow);


            var action = new ReplaceConditionalCaseAction("secondColumn", "new cell", NBi.Service.Operator.Like, new[] { "%1" }, false);
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][1], Is.EqualTo("new cell"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1][1], Is.EqualTo("secondCell2"));
        }

        [Test]
        public void Execute_ReplaceSecondColumnWithConditionAndMultiple_ColumnReplaced()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseSetCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseSetCollection.Scope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell3";
            thirdRow[1] = "(none)";
            thirdRow[2] = "thirdCell3";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(thirdRow);


            var action = new ReplaceConditionalCaseAction("secondColumn", "new cell", NBi.Service.Operator.Equal, new[] { "secondCell1", "(none)" }, false);
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][1], Is.EqualTo("new cell"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1][1], Is.EqualTo("secondCell2"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[2][1], Is.EqualTo("new cell"));
        }
    }
}
