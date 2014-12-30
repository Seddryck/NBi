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
            var action = new ReplaceConditionalCaseAction("myColumn", "new value", NBi.Service.Operator.Like, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Replacing content of column 'myColumn' with value 'new value' when values like 'first value'"));
        }

        [Test]
        public void Execute_ReplaceSecondColumn_ColumnReplaced()
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
            

            var action = new ReplaceCaseAction("secondColumn", "new cell");
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            foreach (DataRow row in state.TestCaseSetCollection.Scope.Content.Rows)
                Assert.That(row[1], Is.EqualTo("new cell"));
        }

        
    }
}
