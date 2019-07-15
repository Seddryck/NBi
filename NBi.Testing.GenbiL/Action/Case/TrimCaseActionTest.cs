using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser.Valuable;
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
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "Cell ";
            firstRow[1] = " secondCell1 ";
            firstRow[2] = " Text";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "Cell";
            secondRow[1] = "(none)";
            secondRow[2] = "";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);

            return state;
        }

        [Test]
        public void Execute_FirstColumnLeftTrimWithValue_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] { "firstColumn" }, Directions.Left);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell "));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
        }

        [Test]
        public void Execute_FirstColumnRightTrimWithValue_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] { "firstColumn" }, Directions.Right);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
        }

        [Test]
        public void Execute_SecondColumnSubstitutueWithValue_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] { "secondColumn" }, Directions.Both);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("(none)"));
        }


        [Test]
        public void Execute_AllColumnsTrim_ValueTrimmed()
        {
            var state = BuildInitialState();

            var action = new TrimCaseAction(new string[] {}, Directions.Both);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["thirdColumn"], Is.EqualTo("Text"));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("(none)"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["thirdColumn"], Is.EqualTo(""));
        }


    }
}
