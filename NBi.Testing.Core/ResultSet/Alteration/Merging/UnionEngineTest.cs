using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Core.ResultSet.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Merging
{
    public class UnionEngineTest
    {
        [Test()]
        public void Execute_UnionByNameTwoDataSets_TotalIsExpected()
        {
            var args1 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var rs1 = new ObjectsResultSetResolver(args1).Execute();

            var args2 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Delta", 10, 5 }, new object[] { "Epsilon", 7, 3 } });
            var rs2 = new ObjectsResultSetResolver(args2);

            var merge = new UnionByNameEngine(rs2);
            var result = merge.Execute(rs1);

            Assert.That(result.Rows.Count, Is.EqualTo(5));
            Assert.That(result.ColumnCount, Is.EqualTo(3));
        }

        [Test()]
        public void Execute_UnionTwoDataSetsWithOneDifferentColumn_AllTheColumnsInOutput()
        {
            var args1 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var rs1 = new ObjectsResultSetResolver(args1).Execute();
            rs1.GetColumn(1)?.Rename("first");
            rs1.GetColumn(2)?.Move(0);

            var args2 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Delta", 10, 5 }, new object[] { "Epsilon", 7, 3 } });
            var rs2 = new ObjectsResultSetResolver(args2);

            var merge = new UnionByNameEngine(rs2);
            var result = merge.Execute(rs1);

            Assert.That(result.Rows.Count, Is.EqualTo(5));
            Assert.That(result.ColumnCount, Is.EqualTo(4));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("Column2"));
            Assert.That(result.GetColumn(1)?.Name, Is.EqualTo("Column0"));
            Assert.That(result.GetColumn(2)?.Name, Is.EqualTo("first"));
            Assert.That(result.GetColumn(3)?.Name, Is.EqualTo("Column1"));
            Assert.That(result[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[1][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[2][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[3][3], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[4][3], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[0][2], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[1][2], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[2][2], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[3][2], Is.EqualTo(DBNull.Value));
            Assert.That(result[4][2], Is.EqualTo(DBNull.Value));
        }

        [Test()]
        public void Execute_UnionByOrdinal_TotalIsExpected()
        {
            var args1 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var rs1 = new ObjectsResultSetResolver(args1).Execute();

            var args2 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Delta", 10, 5 }, new object[] { "Epsilon", 7, 3 } });
            var rs2 = new ObjectsResultSetResolver(args2);

            var merge = new UnionByOrdinalEngine(rs2);
            var result = merge.Execute(rs1);

            Assert.That(result.Rows.Count, Is.EqualTo(5));
            Assert.That(result.ColumnCount, Is.EqualTo(3));
        }

        [Test()]
        public void Execute_UnionByOrdinalUnexpectedColumns_CorrectUnion()
        {
            var args1 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var rs1 = new ObjectsResultSetResolver(args1).Execute();

            var args2 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Delta", 10, 5, true }, new object[] { "Epsilon", 7, 3, false } });
            var rs2 = new ObjectsResultSetResolver(args2);

            var merge = new UnionByOrdinalEngine(rs2);
            var result = merge.Execute(rs1);

            Assert.That(result.Rows.Count, Is.EqualTo(5));
            Assert.That(result.ColumnCount, Is.EqualTo(4));
            Assert.That(result[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[1][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[2][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[3][3], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[4][3], Is.Not.EqualTo(DBNull.Value));
        }

        [Test()]
        public void Execute_UnionByOrdinalMissingColumns_CorrectUnion()
        {
            var args1 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var rs1 = new ObjectsResultSetResolver(args1).Execute();

            var args2 = new ObjectsResultSetResolverArgs(new[] { new object[] { "Delta", 10 }, new object[] { "Epsilon", 7 } });
            var rs2 = new ObjectsResultSetResolver(args2);

            var merge = new UnionByOrdinalEngine(rs2);
            var result = merge.Execute(rs1);

            Assert.That(result.Rows.Count, Is.EqualTo(5));
            Assert.That(result.ColumnCount, Is.EqualTo(3));
            Assert.That(result[0][2], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[1][2], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[2][2], Is.Not.EqualTo(DBNull.Value));
            Assert.That(result[3][2], Is.EqualTo(DBNull.Value));
            Assert.That(result[4][2], Is.EqualTo(DBNull.Value));
        }
    }
}
