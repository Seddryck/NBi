using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NBi.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class FilterCaseActionTest
    {
        [Test]
        public void Display_LikeOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", Operator.Like, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances like 'first value'"));
        }

        [Test]
        public void Display_NotLikeOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", Operator.Like, new[] { "first value" }, true);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances not like 'first value'"));
        }

        [Test]
        public void Display_EqualOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", Operator.Equal, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances equal to 'first value'"));
        }

        [Test]
        public void Display_LikeMultipleValues_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", Operator.Like, new[] { "first value", "second value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances like 'first value', 'second value'"));
        }

        public void Filter_Equal_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Equal, new[] {"matching"}, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_NotEqual_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Equal, new[] { "matching" }, true);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("xyz"));
        }

        [Test]
        public void Filter_LikeStart_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Like, new[] {"match%"}, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeEnd_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Like, new[] { "%ing" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeContain_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Like, new[] { "%atch%" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeContainBounded_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Like, new[] { "%matching%" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeContainComplex_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Like, new[] { "ma%h%%ng%" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_EqualWithoutMatch_EmptyContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow[0] = "abc";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Equal, new[] { "matching" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(0));
        }


        [Test]
        public void Filter_EqualMultipleValues_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow1 = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow1[0] = "matching 1";
            var matchingRow2 = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow2[0] = "matching 2";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow1);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow2);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Equal, new[] { "matching 1", "matching 2" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching 1"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1][0], Is.EqualTo("matching 2"));
        }

        [Test]
        public void Filter_NotEqualMultipleValues_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow1 = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow1[0] = "matching 1";
            var matchingRow2 = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow2[0] = "matching 2";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow1);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow2);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Equal, new[] { "matching 1", "matching 2" }, true);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("xyz"));
        }

        [Test]
        public void Filter_LikeMultipleValues_CorrectNewContent()
        {
            var state = new GenerationState();
            //Setup content;
            state.TestCaseSetCollection.Scope.Content.Columns.Add(new DataColumn("columnName"));
            var matchingRow1 = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow1[0] = "matching 1";
            var matchingRow2 = state.TestCaseSetCollection.Scope.Content.NewRow();
            matchingRow2[0] = "matching 2";
            var nonMatchingRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow1);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(matchingRow2);
            state.TestCaseSetCollection.Scope.Content.Rows.Add(nonMatchingRow);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();

            //Setup filter
            var action = new FilterCaseAction("columnName", Operator.Like, new[] { "%1", "%2" }, false);
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0][0], Is.EqualTo("matching 1"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1][0], Is.EqualTo("matching 2"));
        }
    }
}
