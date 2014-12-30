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
    public class RemoveCaseActionTest
    {
        [Test]
        public void Execute_FirstColumn_ColumnRemoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");

            var action = new RemoveCaseAction("firstColumn");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(2));
        }

        [Test]
        public void Display_FirstColumn_CorrectMessage()
        {
            var action = new RemoveCaseAction("firstColumn");
            Assert.That(action.Display, Is.EqualTo("Removing column 'firstColumn'"));
        }

        [Test]
        public void Execute_FirstAndThirdColumns_ColumnsRemoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");

            var action = new RemoveCaseAction(new List<string>() { "firstColumn", "thirdColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(1));
        }

        [Test]
        public void Display_FirstAndThirdColumns_ColumnsRemoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");

            var action = new RemoveCaseAction(new List<string>() { "firstColumn", "thirdColumn" });
            Assert.That(action.Display, Is.EqualTo("Removing columns 'firstColumn', 'thirdColumn'"));
        }
        
    }
}
