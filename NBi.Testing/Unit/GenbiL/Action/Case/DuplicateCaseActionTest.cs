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

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class DuplicateCaseActionTest
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
        public void Execute_SecondColumnDuplicate_NewColumnCreated()
        {
            var state = BuildInitialState();
            
            var action = new DuplicateCaseAction("secondColumn", new[] { "fourthColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseCollection.Scope.Content.Columns[3].ColumnName, Is.EqualTo("fourthColumn"));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0].ItemArray[3], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1].ItemArray[3], Is.EqualTo("secondCell2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2].ItemArray[3], Is.EqualTo("secondCell3"));
        }

        [Test]
        public void Execute_SecondColumnDuplicate_NewColumnsCreated()
        {
            var state = BuildInitialState();

            var action = new DuplicateCaseAction("secondColumn", new[] { "fourthColumn", "fifthColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(5));
            Assert.That(state.TestCaseCollection.Scope.Content.Columns[3].ColumnName, Is.EqualTo("fourthColumn"));
            Assert.That(state.TestCaseCollection.Scope.Content.Columns[4].ColumnName, Is.EqualTo("fifthColumn"));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0].ItemArray[3], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1].ItemArray[3], Is.EqualTo("secondCell2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2].ItemArray[3], Is.EqualTo("secondCell3"));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0].ItemArray[4], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[1].ItemArray[4], Is.EqualTo("secondCell2"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[2].ItemArray[4], Is.EqualTo("secondCell3"));
        }

    }
}
