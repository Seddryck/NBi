using Moq;
using NBi.Core.Calculation;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.ColumnBased;
using NBi.Core.ResultSet.Alteration.ColumnBased.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Alteration.ColumnBased
{

    public class HoldWhileConditionTest
    {
        [Test]
        public void Execute_AllStrategyAllColumnNotNullOrEmpty_ThreeColumnsHold()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load(new[] { new object[] { "xyz", 1, 120 }, new object[] { "abc", 2, 155 } });
            rs.Columns[0].ColumnName = "Col0";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var predicateInfo = Mock.Of<IPredicateInfo>(
                p => p.ComparerType == ComparerType.NullOrEmpty
                 && p.ColumnType == ColumnType.Text
                 && p.Not == true
                 && p.Operand == new ColumnDynamicIdentifier("i", (int i) => i + 1)
                );

            var hold = new HoldWhileCondition(new AllRowsStrategy(), predicateInfo);
            hold.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Execute_AnyStrategyFirstColumnNullOrEmpty_OneColumnRemoved()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load(new[] { new object[] { 1, "(empty)", 120 }, new object[] { 2, "12", 155 } });
            rs.Columns[0].ColumnName = "Col0";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var predicateInfo = Mock.Of<IPredicateInfo>(
                p => p.ComparerType == ComparerType.NullOrEmpty
                 && p.ColumnType == ColumnType.Text
                 && p.Not == true
                 && p.Operand == new ColumnDynamicIdentifier("i", (int i) => i + 1 )
                );

            var hold = new HoldWhileCondition(new AnyRowsStrategy(), predicateInfo);
            hold.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Execute_TopStrategyFirstColumnNullOrEmpty_OneColumnRemoved()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load(new[] { new object[] { "xyz", 1, 120 }, new object[] { "(empty)", 2, 155 } });
            rs.Columns[0].ColumnName = "Col0";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var predicateInfo = Mock.Of<IPredicateInfo>(
                p => p.ComparerType == ComparerType.NullOrEmpty
                 && p.ColumnType == ColumnType.Text
                 && p.Not == true
                 && p.Operand == new ColumnDynamicIdentifier("i", (int i) => i + 1)
                );

            var hold = new HoldWhileCondition(new TopRowsStrategy(), predicateInfo);
            hold.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Execute_AllStrategyColumnNotFullyNullOrEmpty_NoColumnRemoved()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load(new[] { new object[] {  1, 120,"(null)" }, new object[] {2, 155, "(empty)" } , new object[] { 3, 178,  "xyz" } });
            rs.Columns[0].ColumnName = "Col0";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var predicateInfo = Mock.Of<IPredicateInfo>(
                p => p.ComparerType == ComparerType.NullOrEmpty
                 && p.ColumnType == ColumnType.Text
                 && p.Not == true
                 && p.Operand == new ColumnDynamicIdentifier("i", (int i) => i + 1)
                );

            var hold = new HoldWhileCondition(new AllRowsStrategy(), predicateInfo);
            hold.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(2));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col0"));
            Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col1"));
            Assert.That(rs.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void Execute_AnyRowsFirstColumnNotFullyNullOrEmpty_NoColumnRemoved()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load(new[] { new object[] { "sde", "(null)", 120 }, new object[] { "pkx", "(null)", 155 }, new object[] { "xyz", "(null)", 178 } });
            rs.Columns[0].ColumnName = "Col0";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var predicateInfo = Mock.Of<IPredicateInfo>(
                p => p.ComparerType == ComparerType.NullOrEmpty
                 && p.ColumnType == ColumnType.Text
                 && p.Not == true
                 && p.Operand == new ColumnDynamicIdentifier("i", (int i) => i + 1)
                );

            var hold = new HoldWhileCondition(new AnyRowsStrategy(), predicateInfo);
            hold.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(1));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col0"));
            Assert.That(rs.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void Execute_TopRowsFirstColumnNotFullyNullOrEmpty_NoColumnRemoved()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load(new[] { new object[] { "sde", 1, "(null)" }, new object[] { "(null)", 2, 155 }, new object[] { "xyz", 3, 178 } });
            rs.Columns[0].ColumnName = "Col0";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var predicateInfo = Mock.Of<IPredicateInfo>(
                p => p.ComparerType == ComparerType.NullOrEmpty
                 && p.ColumnType == ColumnType.Text
                 && p.Not == true
                 && p.Operand == new ColumnDynamicIdentifier("i", (int i) => i + 1)
                );

            var hold = new HoldWhileCondition(new TopRowsStrategy(), predicateInfo);
            hold.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(2));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col0"));
            Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col1"));
            Assert.That(rs.Rows.Count, Is.EqualTo(3));
        }
    }
}
