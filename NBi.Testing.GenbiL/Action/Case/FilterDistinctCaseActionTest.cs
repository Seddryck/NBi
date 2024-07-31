using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Case
{
    public class FilterDistinctCaseActionTest
    {
        [Test]
        public void Display_LikeOneValue_CorrectString()
        {
            var action = new FilterDistinctCaseAction();
            Assert.That(action.Display, Is.EqualTo("Filtering distinct cases."));
        }

        [Test]
        public void Execute_OneRowDuplicated_OnlyOneRemains()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "secondCell1";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo("secondCell1"));
        }


        [Test]
        public void Execute_TwoRowsDuplicatedContainingAnArray_OnlyOneRemains()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn", typeof(object));
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "foo/bar";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "foo/bar";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var splitAction = new SplitCaseAction(["secondColumn"], "/");
            splitAction.Execute(state);

            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("foo"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("bar"));
        }

        public void Execute_TwoRowsDuplicatedContainingAnArrayWithDifference_TwoRowsRemain()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn", typeof(object));
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn", typeof(object));
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "foo/bar";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "foo/bar/x";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var splitAction = new SplitCaseAction(["secondColumn"], "/");
            splitAction.Execute(state);

            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("foo"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("bar"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1].ItemArray[1], Has.Member("x"));
        }
    }
}
