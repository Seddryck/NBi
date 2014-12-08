using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
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
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);

            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            var secondColumn = state.TestCaseCollection.Scope.Content.NewRow();
            secondColumn[0] = "secondCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondColumn);

            var action = new RemoveCaseAction("firstColumn");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(1));
        }

        [Test]
        public void Display_FirstColumn_CorrectMessage()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);

            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            var secondColumn = state.TestCaseCollection.Scope.Content.NewRow();
            secondColumn[0] = "secondCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondColumn);

            var action = new RemoveCaseAction("firstColumn");
            Assert.That(action.Display, Is.EqualTo("Removing column 'firstColumn'"));
        }

        [Test]
        public void Execute_FirstAndThirdColumns_ColumnsRemoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);

            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            var secondColumn = state.TestCaseCollection.Scope.Content.NewRow();
            secondColumn[0] = "secondCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondColumn);

            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            var thirdColumn = state.TestCaseCollection.Scope.Content.NewRow();
            thirdColumn[0] = "thirdCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdColumn);

            var action = new RemoveCaseAction(new List<string>() { "firstColumn", "thirdColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(1));
        }

        [Test]
        public void Display_FirstAndThirdColumns_ColumnsRemoved()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);

            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            var secondColumn = state.TestCaseCollection.Scope.Content.NewRow();
            secondColumn[0] = "secondCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondColumn);

            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            var thirdColumn = state.TestCaseCollection.Scope.Content.NewRow();
            thirdColumn[0] = "thirdCell";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdColumn);

            var action = new RemoveCaseAction(new List<string>() { "firstColumn", "thirdColumn" });
            Assert.That(action.Display, Is.EqualTo("Removing columns 'firstColumn', 'thirdColumn'"));
        }
        
    }
}
