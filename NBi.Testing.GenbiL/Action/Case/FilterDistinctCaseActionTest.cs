using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.GenbiL.Action.Case
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
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "secondCell1";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo("secondCell1"));
        }


        [Test]
        public void Execute_TwoRowsDuplicatedContainingAnArray_OnlyOneRemains()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn", typeof(object));
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "foo/bar";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "foo/bar";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var splitAction = new SplitCaseAction(new[] { "secondColumn" }, "/");
            splitAction.Execute(state);

            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("foo"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("bar"));
        }

        public void Execute_TwoRowsDuplicatedContainingAnArrayWithDifference_TwoRowsRemain()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn", typeof(object));
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn", typeof(object));
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "foo/bar";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "foo/bar/x";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var splitAction = new SplitCaseAction(new[] { "secondColumn" }, "/");
            splitAction.Execute(state);

            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("foo"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Has.Member("bar"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1].ItemArray[1], Has.Member("x"));
        }
    }
}
