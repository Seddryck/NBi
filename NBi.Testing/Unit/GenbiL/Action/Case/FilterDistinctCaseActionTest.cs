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
    public class FilterDistinctCaseActionTest
    {
        [Test]
        public void Filter_Distinct_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var firstRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            firstRow[0] = "alpha";
            var secondRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            secondRow[0] = "beta";
            var duplicatedRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            duplicatedRow[0] = "alpha";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(firstRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(secondRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(duplicatedRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("alpha"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1][0], Is.EqualTo("beta"));
        }

        [Test]
        public void Filter_Distinct_EmptyContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterDistinctCaseAction();
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(0));
        }
    }
}
