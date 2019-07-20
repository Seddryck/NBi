using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Renaming;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet.Alteration.Renaming
{
    public class RenamingEngineFactoryTest
    {
        [Test]
        public void Execute_FirstColumnIsText_FirstColumnIsNumeric()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "100,12", "Alpha" }, new[] { "100", "Beta" }, new[] { "0,1", "Gamma" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var renamer = new NewNameRenamingEngine(
                new ColumnOrdinalIdentifier(1),
                new LiteralScalarResolver<string>("myNewName")
                );
            var newRs = renamer.Execute(rs);

            Assert.That(newRs.Columns[1].ColumnName, Is.EqualTo("myNewName"));
        }
    }
}
