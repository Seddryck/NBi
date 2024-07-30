using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Extension
{
    public class NCalcExtendEngineTest
    {
        [Test]
        public void Execute_StandardRsColumnOrdinal_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["Alpha", 1, 2], ["Beta", 3, 2], new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var extender = new NCalcExtendEngine(
                new ServiceLocator(),
                new Context(),
                new ColumnOrdinalIdentifier(3),
                "[#1] * [#2]"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.ColumnCount, Is.EqualTo(4));
            Assert.That(newRs[0][3], Is.EqualTo(2));
            Assert.That(newRs[1][3], Is.EqualTo(6));
            Assert.That(newRs[2][3], Is.EqualTo(35));
        }

        [Test]
        public void Execute_StandardRsColumnName_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, ["Beta", 3, 2], ["Gamma", 5, 7] });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)!.Rename("a");
            rs.GetColumn(1)!.Rename("b");
            rs.GetColumn(2)!.Rename("c");

            var extender = new NCalcExtendEngine(
                new ServiceLocator(),
                new Context(),
                new ColumnNameIdentifier("d"),
                "[b] * [c]"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.ColumnCount, Is.EqualTo(4));
            Assert.That(newRs.GetColumn(3)!.Name, Is.EqualTo("d"));
            Assert.That(newRs[0][3], Is.EqualTo(2));
            Assert.That(newRs[1][3], Is.EqualTo(6));
            Assert.That(newRs[2][3], Is.EqualTo(35));
        }

        [Test]
        public void Execute_StandardRsColumnNameAndVariable_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["Alpha", 1, 2], ["Beta", 3, 2], new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)!.Rename("a");
            rs.GetColumn(1)!.Rename("b");
            rs.GetColumn(2)!.Rename("c");

            var context = new Context();
            context.Variables.Add<decimal>("myVar", () => new GlobalVariable(new LiteralScalarResolver<decimal>(2)).GetValue());

            var extender = new NCalcExtendEngine(
                new ServiceLocator(),
                context,
                new ColumnNameIdentifier("d"),
                "[@myVar] * [b] * [c]"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.ColumnCount, Is.EqualTo(4));
            Assert.That(newRs.GetColumn(3)!.Name, Is.EqualTo("d"));
            Assert.That(newRs[0][3], Is.EqualTo(4));
            Assert.That(newRs[1][3], Is.EqualTo(12));
            Assert.That(newRs[2][3], Is.EqualTo(70));
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [Retry(3)]
        public void Execute_ManyTimes_Performances(int count)
        {
            var rows = new List<object[]>();
            for (int i = 0; i < count; i++)
                rows.Add([i, i + 1]);

            var args = new ObjectsResultSetResolverArgs(rows.ToArray());
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)!.Rename("a");
            rs.GetColumn(1)!.Rename("b");

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var extender = new NCalcExtendEngine(
                new ServiceLocator(),
                new Context(),
                new ColumnNameIdentifier("c"),
                "[b] - [a] + Max(a,b) - Sin(a)"
                );
            _ = extender.Execute(rs);
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(5000));
        }
    }
}
