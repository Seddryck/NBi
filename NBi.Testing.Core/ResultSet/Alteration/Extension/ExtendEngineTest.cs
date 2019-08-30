﻿using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet.Alteration.Extension
{
    public class ExtendEngineTest
    {
        [Test]
        public void Execute_StandardRsColumnOrdinal_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var extender = new ExtendEngine(
                new ColumnOrdinalIdentifier(3),
                "[#1] * [#2]"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(2));
            Assert.That(newRs.Rows[1][3], Is.EqualTo(6));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(35));
        }

        [Test]
        public void Execute_StandardRsColumnName_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "a";
            rs.Columns[1].ColumnName = "b";
            rs.Columns[2].ColumnName = "c";

            var extender = new ExtendEngine(
                new ColumnNameIdentifier("d"),
                "[b] * [c]"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("d"));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(2));
            Assert.That(newRs.Rows[1][3], Is.EqualTo(6));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(35));
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        public void Execute_ManyTimes_Performances(int count)
        {
            var rows = new List<object[]>();
            for (int i = 0; i < count; i++)
                rows.Add(new object[] { i, i + 1 });

            var args = new ObjectsResultSetResolverArgs(rows.ToArray());
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "a";
            rs.Columns[1].ColumnName = "b";

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var extender = new ExtendEngine(
                new ColumnNameIdentifier("c"),
                "[b] - [a] + Max(a,b) - Sin(a)"
                );
            var newRs = extender.Execute(rs);
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(5000));
        }
    }
}