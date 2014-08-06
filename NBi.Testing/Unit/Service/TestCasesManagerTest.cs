using System;
using System.Data;
using System.Linq;
using NBi.Service;
using NUnit.Framework;

namespace NBi.Testing.Unit.Service
{
    [TestFixture]
    public class TestCasesManagerTest
    {
        [Test]
        public void Filter_Equal_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0]="matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0]="xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Equal, false, "matching");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_NotEqual_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Equal, true, "matching");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.Not.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeStart_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Like, false, "match%");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeEnd_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Like, false, "%ing");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeContain_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Like, false, "%atch%");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.EqualTo("matching"));
        }
        
        [Test]
        public void Filter_LikeContainBounded_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Like, false, "%matching%");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.EqualTo("matching"));
        }

        [Test]
        public void Filter_LikeContainComplex_CorrectNewContent()
        {
            var manager = new TestCasesManager();
            //Setup content;
            manager.Content.Columns.Add(new DataColumn("columnName"));
            manager.Variables.Add("columnName");
            var matchingRow = manager.Content.NewRow();
            matchingRow[0] = "matching";
            var nonMatchingRow = manager.Content.NewRow();
            nonMatchingRow[0] = "xyz";
            manager.Content.Rows.Add(matchingRow);
            manager.Content.Rows.Add(nonMatchingRow);
            manager.Content.AcceptChanges();

            //Setup filter
            manager.Filter("columnName", Operator.Like, false, "ma%h%ng%");

            Assert.That(manager.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(manager.Content.Rows[0][0], Is.EqualTo("matching"));
        }
    }
}
