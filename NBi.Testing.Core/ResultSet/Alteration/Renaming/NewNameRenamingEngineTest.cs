using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Renaming;
using NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Renaming
{
    public class NewNameRenamingEngineTest
    {
        [Test]
        public void Execute_ExistingColumnByOrdinal_ColumnRenamed()
        {
            var args = new ObjectsResultSetResolverArgs([new[] { "100,12", "Alpha" }, new[] { "100", "Beta" }, new[] { "0,1", "Gamma" }]);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var renamer = new NewNameRenamingEngine(
                new ColumnOrdinalIdentifier(1),
                new LiteralScalarResolver<string>("myNewName")
                );
            var newRs = renamer.Execute(rs);

            Assert.That(newRs.GetColumn(1)?.Name, Is.EqualTo("myNewName"));
        }

        [Test]
        public void Execute_ExistingColumnByName_ColumnRenamed()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "100,12", "Alpha" }, new[] { "100", "Beta" }, new[] { "0,1", "Gamma" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs?.GetColumn(1)?.Rename("myOldName");

            var renamer = new NewNameRenamingEngine(
                new ColumnNameIdentifier("myOldName"),
                new LiteralScalarResolver<string>("myNewName")
                );
            var newRs = renamer.Execute(rs!);

            Assert.That(newRs.GetColumn(1)?.Name, Is.EqualTo("myNewName"));
            Assert.That(newRs.Columns.Any(c => c.Name == "myOldName"), Is.False);
        }

        [Test]
        public void Execute_NotExistingColumnFailureStrategy_IgnoreIssue()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "100,12", "Alpha" }, new[] { "100", "Beta" }, new[] { "0,1", "Gamma" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var renamer = new NewNameRenamingEngine(
                new ColumnNameIdentifier("unexistingColumn"),
                new LiteralScalarResolver<string>("myNewName")
                );
            Assert.Throws<NBiException>( () => renamer.Execute(rs));
        }


        [Test]
        public void Execute_NotExistingColumnSkipStrategy_IgnoreIssue()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "100,12", "Alpha" }, new[] { "100", "Beta" }, new[] { "0,1", "Gamma" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var renamer = new NewNameRenamingEngine(
                new ColumnNameIdentifier("unexistingColumn"),
                new LiteralScalarResolver<string>("myNewName"),
                new SkipAlterationStrategy()
                );
            var newRs = renamer.Execute(rs);

            Assert.That(newRs.Columns.Any(c => c.Name == "unexistingColumn"), Is.False);
        }
    }
}
