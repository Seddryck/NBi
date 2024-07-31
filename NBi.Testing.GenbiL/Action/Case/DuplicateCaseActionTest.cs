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

namespace NBi.GenbiL.Testing.Action.Case
{
    public class DuplicateCaseActionTest
    {
        protected GenerationState BuildInitialState()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "Cell";
            firstRow[1] = "secondCell1";
            firstRow[2] = "Text";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "Cell";
            secondRow[1] = "secondCell2";
            secondRow[2] = "";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);
            var thirdRow = state.CaseCollection.CurrentScope.Content.NewRow();
            thirdRow[0] = "XXX";
            thirdRow[1] = "secondCell3";
            thirdRow[2] = "YYY";
            state.CaseCollection.CurrentScope.Content.Rows.Add(thirdRow);

            return state;
        }

        [Test]
        public void Execute_SecondColumnDuplicate_NewColumnCreated()
        {
            var state = BuildInitialState();
            
            var action = new DuplicateCaseAction("secondColumn", ["fourthColumn"]);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns[3].ColumnName, Is.EqualTo("fourthColumn"));

            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[3], Is.EqualTo("secondCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1].ItemArray[3], Is.EqualTo("secondCell2"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[2].ItemArray[3], Is.EqualTo("secondCell3"));
        }

        [Test]
        public void Execute_SecondColumnDuplicate_NewColumnsCreated()
        {
            var state = BuildInitialState();

            var action = new DuplicateCaseAction("secondColumn", ["fourthColumn", "fifthColumn"]);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(5));
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns[3].ColumnName, Is.EqualTo("fourthColumn"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns[4].ColumnName, Is.EqualTo("fifthColumn"));

            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[3], Is.EqualTo("secondCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1].ItemArray[3], Is.EqualTo("secondCell2"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[2].ItemArray[3], Is.EqualTo("secondCell3"));

            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[4], Is.EqualTo("secondCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1].ItemArray[4], Is.EqualTo("secondCell2"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[2].ItemArray[4], Is.EqualTo("secondCell3"));
        }

    }
}
