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
    public class ConcatenateCaseActionTest
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
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell3";
            thirdRow[1] = "(none)";
            thirdRow[2] = "thirdCell3";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);

            return state;
        }

        [Test]
        public void Execute_SecondColumn_ValueConcatenated()
        {
            var state = BuildInitialState();

            var builder = new ValuableBuilder();
            var values = builder.Build(ValuableType.Value, new[] {"alpha"});

            var action = new ConcatenateCaseAction("secondColumn", values);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1alpha"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("alpha"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2]["secondColumn"], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumn_ColumnsConcatenated()
        {
            var state = BuildInitialState();

            var builder = new ValuableBuilder();
            var values = builder.Build(ValuableType.Column, new[] { "thirdColumn", "firstColumn" });

            var action = new ConcatenateCaseAction("secondColumn", values);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1thirdCell1firstCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("thirdCell2firstCell2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2]["secondColumn"], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumn_ColumnsConcatenatedWithNone()
        {
            var state = BuildInitialState();
            state.TestCaseCollection.Scope.Content.Rows[0]["firstColumn"] = "(none)";

            var builder = new ValuableBuilder();
            var values = builder.Build(ValuableType.Column, new[] { "firstColumn" });

            var action = new ConcatenateCaseAction("secondColumn", values);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("(none)"));
        }
    }
}
