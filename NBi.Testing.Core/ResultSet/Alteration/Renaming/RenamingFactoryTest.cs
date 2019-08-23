﻿using NBi.Core.ResultSet;
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
    public class RenamingFactoryTest
    {
        [Test]
        public void Instantiate_NewNameRenamingEngineArgs_NewNameRenamingEngineArgs()
        {
            var factory = new RenamingFactory();
            var renamer = factory.Instantiate(new NewNameRenamingArgs(
                new ColumnOrdinalIdentifier(1),
                new LiteralScalarResolver<string>("myNewName")
                ));
            Assert.That(renamer, Is.Not.Null);
            Assert.That(renamer, Is.TypeOf<NewNameRenamingEngine>());
        }
    }
}
