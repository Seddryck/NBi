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
            secondRow[1] = "";
            secondRow[2] = "thirdCell2";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell3";
            thirdRow[1] = "(none)";
            thirdRow[2] = "thirdCell3";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(thirdRow);

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
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1alpha"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("alpha"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[2]["secondColumn"], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumn_ColumnsConcatenated()
        {
            var state = BuildInitialState();

            var builder = new ValuableBuilder();
            var values = builder.Build(ValuableType.Column, new[] { "thirdColumn", "firstColumn" });

            var action = new ConcatenateCaseAction("secondColumn", values);
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1thirdCell1firstCell1"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1]["secondColumn"], Is.EqualTo("thirdCell2firstCell2"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[2]["secondColumn"], Is.EqualTo("(none)"));
        }

        [Test]
        public void Execute_SecondColumn_ColumnsConcatenatedWithNone()
        {
            var state = BuildInitialState();
            state.TestCaseSetCollection.Scope.Content.Rows[0]["firstColumn"] = "(none)";

            var builder = new ValuableBuilder();
            var values = builder.Build(ValuableType.Column, new[] { "firstColumn" });

            var action = new ConcatenateCaseAction("secondColumn", values);
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("(none)"));
        }
    }
}
