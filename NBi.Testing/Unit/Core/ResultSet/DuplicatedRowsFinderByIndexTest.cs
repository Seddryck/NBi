using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NBi.Core.ResultSet;
using System.Data;

namespace NBi.Testing.Unit.Core.ResultSet
{
    class DuplicatedRowsFinderByIndexTest
    {
        protected DataTable BuildDataTable(IEnumerable<List<object>> rows)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            for (int i = 0; i < rows.Max(x => x.Count()); i++)
                dt.Columns.Add($"Column{i}");

            foreach (var row in rows)
            {
                var dr = dt.NewRow();
                int i = 0;
                foreach (var value in row)
                {
                    dr.SetField<object>(i, value);
                    i++;
                }
                dt.Rows.Add(dr);
            }
            
            return dt;
        }

        [Test]
        public void Execute_NoDuplicate_True()
        {
            var resultSet = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "d", 150 },
                });

            var finder = new DuplicatedRowsFinderByIndex();
            var result = finder.Execute(resultSet);
            Assert.That(result.HasNoDuplicate, Is.True);
        }

        [Test]
        public void Execute_Duplicate_False()
        {
            var resultSet = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "d", 150 },
                });

            var finder = new DuplicatedRowsFinderByIndex();
            var result = finder.Execute(resultSet);
            Assert.That(result.HasNoDuplicate, Is.False);
        }

        [Test]
        public void Execute_Duplicate_OneOccurence()
        {
            var resultSet = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "d", 150 },
                });

            var finder = new DuplicatedRowsFinderByIndex();
            var result = finder.Execute(resultSet);
            Assert.That(result.Values.Count(), Is.EqualTo(1));
            Assert.That(result.Values.ElementAt(0).OccurenceCount, Is.EqualTo(2));
        }

        [Test]
        public void Execute_Duplicate_TwoOccurences()
        {
            var resultSet = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "b", 120 },
                });

            var finder = new DuplicatedRowsFinderByIndex();
            var result = finder.Execute(resultSet);
            Assert.That(result.Values.Count(), Is.EqualTo(1));
            Assert.That(result.Values.ElementAt(0).OccurenceCount, Is.EqualTo(3));
            Assert.That(result.Values.ElementAt(0).Keys.Members, Has.Member("a"));
            Assert.That(result.Values.ElementAt(0).Keys.Members, Has.Member("b"));
            Assert.That(result.Values.ElementAt(0).Keys.Members, Has.Member("120"));
            Assert.That(result.Rows.Count(), Is.EqualTo(1));
            Assert.That(result.Rows.ElementAt(0).ItemArray, Has.Member(3));
            Assert.That(result.Rows.ElementAt(0).ItemArray, Has.Member("a"));
            Assert.That(result.Rows.ElementAt(0).ItemArray, Has.Member("b"));
            Assert.That(result.Rows.ElementAt(0).ItemArray, Has.Member("120"));
        }
    }
}
