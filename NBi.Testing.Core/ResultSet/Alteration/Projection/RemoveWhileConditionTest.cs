using Moq;
using NBi.Core.Calculation;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Alteration.ColumnBased;


public class ProjectAwayWhileConditionTest
{
    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_AllStrategyFirstColumnNullOrEmpty_OneColumnRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "(null)", 1, 120 }, new object[] { "(empty)", 2, 155 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AllRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(2));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col1"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(2));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_AnyStrategyFirstColumnNullOrEmpty_OneColumnRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "12", 1, 120 }, new object[] { "(empty)", 2, 155 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AnyRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(2));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col1"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(2));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_TopStrategyFirstColumnNullOrEmpty_OneColumnRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "(empty)", 1, 120 }, new object[] { "12", 2, 155 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new TopRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(2));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col1"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(2));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_AllStrategyColumnNotFullyNullOrEmpty_NoColumnRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "(null)", 1, 120 }, new object[] { "(empty)", 2, 155 } , new object[] { "xyz", 3, 178 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AllRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(3));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col0"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col1"));
        //Assert.That(rs.Columns[2].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(3));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_AnyRowsFirstColumnNotFullyNullOrEmpty_NoColumnRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "sde", 1, 120 }, new object[] { "pkx", 2, 155 }, new object[] { "xyz", 3, 178 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AnyRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(3));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col0"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col1"));
        //Assert.That(rs.Columns[2].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(3));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_TopRowsFirstColumnNotFullyNullOrEmpty_NoColumnRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "sde", 1, 120 }, new object[] { "(null)", 2, 155 }, new object[] { "xyz", 3, 178 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new TopRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(3));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col0"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col1"));
        //Assert.That(rs.Columns[2].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(3));
    }


    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_AllStrategyTwoFirstColumnNotFullyNullOrEmpty_TwoColumnsRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "(null)", "(null)", 1, 120 }, new object[] { "(empty)", "(null)", 2, 155 }, new object[] { "(null)", "(empty)", 3, 178 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";
        //rs.Columns[3].ColumnName = "Col3";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AllRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(2));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col3"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(3));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_AnyRowsTwoFirstColumnNotFullyNullOrEmpty_TwoColumnsRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "(null)", "xyz", 1, 120 }, new object[] { "xyz", "(null)", 2, 155 }, new object[] { "xyz", "xyz", 3, 178 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";
        //rs.Columns[3].ColumnName = "Col3";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AnyRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(2));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col3"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(3));
    }

    [Test]
    [Ignore("Need to re-establish ProjectAwayWhile")]
    public void Execute_TopRowsTwoFirstColumnNotFullyNullOrEmpty_TwoColumnsRemoved()
    {
        //var rs = new NBi.Core.ResultSet.ResultSet();
        //rs.Load(new[] { new object[] { "(null)", "(empty)", 1, 120 }, new object[] { "xyz", "(null)", 2, 155 }, new object[] { "xyz", "xyz", 3, 178 } });
        //rs.Columns[0].ColumnName = "Col0";
        //rs.Columns[1].ColumnName = "Col1";
        //rs.Columns[2].ColumnName = "Col2";
        //rs.Columns[3].ColumnName = "Col3";

        //var predicateInfo = Mock.Of<IPredicateInfo>(
        //    p => p.ComparerType == ComparerType.NullOrEmpty
        //     && p.ColumnType == ColumnType.Text
        //     && p.Not == false
        //     && p.Operand == new ColumnPositionIdentifier(0)
        //    );

        //var remove = new RemoveWhileCondition(new AnyRowsStrategy(), predicateInfo);
        //remove.Execute(rs);

        //Assert.That(rs.Columns.Count, Is.EqualTo(2));
        //Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col2"));
        //Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col3"));
        //Assert.That(rs.Rows.Count, Is.EqualTo(3));
    }
}
