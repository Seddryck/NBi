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
    public class MoveCaseActionTest
    {
        [Test]
        public void Execute_SecondColumnMoveLeft_ColumnMoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("fourthColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("fourthColumn");


            var action = new MoveCaseAction("secondColumn", -1);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseCollection.Scope.Variables[0], Is.EqualTo("secondColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[1], Is.EqualTo("firstColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[2], Is.EqualTo("thirdColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[3], Is.EqualTo("fourthColumn"));
        }

        [Test]
        public void Execute_SecondColumnMoveRight_ColumnMoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("fourthColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("fourthColumn");


            var action = new MoveCaseAction("secondColumn", 1);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseCollection.Scope.Variables[0], Is.EqualTo("firstColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[1], Is.EqualTo("thirdColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[2], Is.EqualTo("secondColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[3], Is.EqualTo("fourthColumn"));
        }

        [Test]
        public void Execute_ThirdColumnMoveFirst_ColumnMoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("fourthColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("fourthColumn");


            var action = new MoveCaseAction("thirdColumn", int.MinValue);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseCollection.Scope.Variables[0], Is.EqualTo("thirdColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[1], Is.EqualTo("firstColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[2], Is.EqualTo("secondColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[3], Is.EqualTo("fourthColumn"));
        }

        [Test]
        public void Execute_SecondColumnMoveLast_ColumnMoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("fourthColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("fourthColumn");


            var action = new MoveCaseAction("secondColumn", int.MaxValue);
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseCollection.Scope.Variables[0], Is.EqualTo("firstColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[1], Is.EqualTo("thirdColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[2], Is.EqualTo("fourthColumn"));
            Assert.That(state.TestCaseCollection.Scope.Variables[3], Is.EqualTo("secondColumn"));
        }

    }
}
