using NBi.Core.ResultSet.Combination;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rs = NBi.Core.ResultSet;

namespace NBi.Testing.Unit.Core.ResultSet.Combination
{
    public class CartesianProductSequenceCombinationTest
    {
        private (Rs.ResultSet rs, ISequenceResolver resolver) Initialize()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            dataTable.AcceptChanges();
            var rs = new Rs.ResultSet();
            rs.Load(dataTable);

            var scalarResolvers = new List<IScalarResolver>()
            {
                new LiteralScalarResolver<string>("2015-01-01"),
                new LiteralScalarResolver<string>("2016-01-01"),
                new LiteralScalarResolver<string>("2017-01-01"),
            };
            var args = new ListSequenceResolverArgs(scalarResolvers);
            var resolver = new ListSequenceResolver<DateTime>(args);
            return (rs, resolver);
        }

        [Test()]
        public void Execute_TwentyRowsAndSequenceOfTwo_SixtyRows()
        {
            var (rs, resolver) = Initialize();
            var combination = new CartesianProductSequenceCombination(resolver);
            combination.Execute(rs);

            Assert.That(rs.Rows.Count, Is.EqualTo(60));
        }

        [Test()]
        public void Execute_TwentyRowsAndSequenceOfTwo_OneAdditionalColumn()
        {
            var (rs, resolver) = Initialize();
            var initColumnCount = rs.Columns.Count;
            var combination = new CartesianProductSequenceCombination(resolver);
            combination.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(initColumnCount + 1));
        }

        [Test()]
        public void Execute_TwentyRowsAndSequenceOfZero_EmptyResultSet()
        {
            var rs = Initialize().rs;
            var initColumnCount = rs.Columns.Count;

            var resolver = new ListSequenceResolver<DateTime>(new ListSequenceResolverArgs(new List<IScalarResolver>()));
            var combination = new CartesianProductSequenceCombination(resolver);
            combination.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(initColumnCount + 1));
            Assert.That(rs.Rows.Count, Is.EqualTo(0));
        }

        [Test()]
        public void Execute_EmptyResultSetAndSequenceOfTwo_EmptyResultSet()
        {
            var (rs, resolver) = Initialize();
            rs.Table.Clear();
            rs.Table.AcceptChanges();
            var initColumnCount = rs.Columns.Count;

            var combination = new CartesianProductSequenceCombination(resolver);
            combination.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(initColumnCount + 1));
            Assert.That(rs.Rows.Count, Is.EqualTo(0));
        }
    }
}
