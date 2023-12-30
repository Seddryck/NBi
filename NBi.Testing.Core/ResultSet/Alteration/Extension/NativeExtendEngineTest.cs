using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
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
    public class NativeExtendEngineTest
    {
        [Test]
        public void Execute_StandardRsColumnOrdinal_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["Alpha", 1, 2], ["Beta", 3, 2], new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var extender = new NativeExtendEngine(
                new ServiceLocator(),
                new Context(), 
                new ColumnOrdinalIdentifier(3),
                "#1 | numeric-to-multiply(#2)"
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
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)!.Rename("a");
            rs.GetColumn(1)!.Rename("b");
            rs.GetColumn(2)!.Rename("c");

            var extender = new NativeExtendEngine(
                new ServiceLocator(),
                new Context(),
                new ColumnNameIdentifier("d"),
                "[a] | text-to-first-chars([c]) | text-to-upper"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.ColumnCount, Is.EqualTo(4));
            Assert.That(newRs.GetColumn(3)!.Name, Is.EqualTo("d"));
            Assert.That(newRs[0][3], Is.EqualTo("AL"));
            Assert.That(newRs[1][3], Is.EqualTo("BE"));
            Assert.That(newRs[2][3], Is.EqualTo("GAMMA"));
        }

        [Test]
        public void Execute_StandardRsColumnNameAndVariable_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)!.Rename("a");
            rs.GetColumn(1)!.Rename("b");
            rs.GetColumn(2)!.Rename("c");

            var extender = new NativeExtendEngine(
                new ServiceLocator(),
                new Context(new Dictionary<string, IVariable> { { "myVar", new GlobalVariable(new LiteralScalarResolver<decimal>(2)) } }),
                new ColumnNameIdentifier("d"),
                "[a] | text-to-first-chars(@myVar) | text-to-upper"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.ColumnCount, Is.EqualTo(4));
            Assert.That(newRs.GetColumn(3)!.Name, Is.EqualTo("d"));
            Assert.That(newRs[0][3], Is.EqualTo("AL"));
            Assert.That(newRs[1][3], Is.EqualTo("BE"));
            Assert.That(newRs[2][3], Is.EqualTo("GA"));
        }

        [Test]
        public void Execute_StandardRsColumnNameAndVariableFirstArg_CorrectExtension()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0).Rename("a");
            rs.GetColumn(1).Rename("b");
            rs.GetColumn(2).Rename("c");

            var extender = new NativeExtendEngine(
                new ServiceLocator(),
                new Context(new Dictionary<string, IVariable> { { "myVar", new GlobalVariable(new LiteralScalarResolver<string>("foo")) } }),
                new ColumnNameIdentifier("d"),
                "@myVar | text-to-first-chars(#1) | text-to-upper"
                );
            var newRs = extender.Execute(rs);

            Assert.That(newRs.ColumnCount, Is.EqualTo(4));
            Assert.That(newRs.GetColumn(3).Name, Is.EqualTo("d"));
            Assert.That(newRs[0][3], Is.EqualTo("F"));
            Assert.That(newRs[1][3], Is.EqualTo("FOO"));
            Assert.That(newRs[2][3], Is.EqualTo("FOO"));
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
            var extender = new NativeExtendEngine(
                new ServiceLocator(),
                new Context(),
                new ColumnNameIdentifier("c"),
                "[b] | numeric-to-multiply([a]) | numeric-to-add([a], [b])"
                );
            extender.Execute(rs);
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(5000));
        }
    }
}
