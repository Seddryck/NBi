using NBi.GenbiL;
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

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class SubstituteCaseActionTest
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
            firstRow[0] = "Cell";
            firstRow[1] = "secondCell1";
            firstRow[2] = "Text";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "Cell";
            secondRow[1] = "secondCell2";
            secondRow[2] = "";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "XXX";
            thirdRow[1] = "secondCell3";
            thirdRow[2] = "YYY";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);

            return state;
        }

        [Test]
        public void Execute_SecondColumnSubstitutueWithValue_ValueSubstitued()
        {
            var state = BuildInitialState();
            state.TestCaseCollection.Scope.Content.Rows[2]["secondColumn"] = "(none)";

            var builder = new ValuableBuilder();
            var oldValue = builder.Build(ValuableType.Value, "Cell");
            var newValue = builder.Build(ValuableType.Value, "Text");

            var action = new SubstituteCaseAction("secondColumn", oldValue, newValue);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondText1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("secondText2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2]["secondColumn"], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumnSubstitutueWithColumn_ValueSubstitued()
        {
            var state = BuildInitialState();

            var builder = new ValuableBuilder();
            var oldValue = builder.Build(ValuableType.Column, "firstColumn");
            var newValue = builder.Build(ValuableType.Column, "thirdColumn");

            var action = new SubstituteCaseAction("secondColumn", oldValue, newValue);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondText1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("second2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2]["secondColumn"], Is.EqualTo("secondCell3"));
        }

    }
}
