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
    public class ReplaceCaseActionTest
    {
        [Test]
        public void Display_LikeOneValue_CorrectString()
        {
            var action = new ReplaceCaseAction("myColumn", "new value", NBi.Service.Operator.Like, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Replacing content of column 'myColumn' with value 'new value' when values like 'first value'"));
        }

        [Test]
        public void Execute_ReplaceSecondColumn_ColumnReplaced()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            

            var action = new ReplaceCaseAction("secondColumn", "new cell");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            foreach (DataRow row in state.TestCaseCollection.Scope.Content.Rows)
                Assert.That(row[1], Is.EqualTo("new cell"));
        }

        [Test]
        public void Execute_ReplaceSecondColumnWithCondition_ColumnReplaced()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);


            var action = new ReplaceCaseAction("secondColumn", "new cell", NBi.Service.Operator.Like, new[] {"%1"}, false);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0][1], Is.EqualTo("new cell"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1][1], Is.EqualTo("secondCell2"));
        }

        [Test]
        public void Execute_ReplaceSecondColumnWithConditionAndMultiple_ColumnReplaced()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell3";
            thirdRow[1] = "(none)";
            thirdRow[2] = "thirdCell3";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);


            var action = new ReplaceCaseAction("secondColumn", "new cell", NBi.Service.Operator.Equal, new[] { "secondCell1", "(none)" }, false);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0][1], Is.EqualTo("new cell"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1][1], Is.EqualTo("secondCell2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2][1], Is.EqualTo("new cell"));
        }
    }
}
