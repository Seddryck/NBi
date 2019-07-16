using NBi.GenbiL;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser.Valuable;
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
    public class TrimCaseActionTest
    {
        protected GenerationState BuildInitialState()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "Cell ";
            firstRow[1] = " secondCell1 ";
            firstRow[2] = " Text";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "Cell";
            secondRow[1] = "(none)";
            secondRow[2] = "";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            return state;
        }

        [Test]
        public void Execute_FirstColumnLeftTrimWithValue_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] { "firstColumn" }, DirectionType.Left);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell "));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
        }

        [Test]
        public void Execute_FirstColumnRightTrimWithValue_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] { "firstColumn" }, DirectionType.Right);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
        }

        [Test]
        public void Execute_SecondColumnSubstitutueWithValue_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] { "secondColumn" }, DirectionType.Both);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1]["secondColumn"], Is.EqualTo("(none)"));
        }


        [Test]
        public void Execute_AllColumnsTrim_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] {}, DirectionType.Both);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"], Is.EqualTo("Text"));

            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1]["secondColumn"], Is.EqualTo("(none)"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows[1]["thirdColumn"], Is.EqualTo(""));
        }


    }
}
