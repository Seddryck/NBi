using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBi.Core.Decoration.IO;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.IO.Conditions;

namespace NBi.Testing.Integration.Core.Decoration.IO.Conditions
{
    public class FolderExistsConditionTest
    {
        [SetUp]
        public void CreateDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");

            if(Directory.Exists(@"Temp\Target"))
                Directory.Delete(@"Temp\Target", true);
            Directory.CreateDirectory(@"Temp\Target");

            if (Directory.Exists(@"Temp\TargetNotExisting"))
                Directory.Delete(@"Temp\TargetNotExisting", true);
        }

        [Test]
        public void Execute_ExistingFolder_True()
        {
            var folderArgs = new FolderExistsConditionArgs
            (
                @"Temp\",
                new LiteralScalarResolver<string>(@"."),
                new LiteralScalarResolver<string>(@"Target"),
                new LiteralScalarResolver<bool>(false)
            );

            var condition = new FolderExistsCondition(folderArgs);
            Assert.That(condition.Validate(), Is.True);
        }

        [Test]
        public void Execute_ExistingFolderButEmpty_True()
        {
            var folderArgs = new FolderExistsConditionArgs
            (
                @"Temp\",
                new LiteralScalarResolver<string>(@"."),
                new LiteralScalarResolver<string>(@"Target"),
                new LiteralScalarResolver<bool>(true)
            );

            var condition = new FolderExistsCondition(folderArgs);
            Assert.That(condition.Validate(), Is.False);
        }

        [Test]
        public void Execute_ExistingFolderButNotEmpty_True()
        {
            File.WriteAllText(@"Temp\Target\file.txt", "a little text");
            var folderArgs = new FolderExistsConditionArgs
            (
                @"Temp\",
                new LiteralScalarResolver<string>(@"."),
                new LiteralScalarResolver<string>(@"Target"),
                new LiteralScalarResolver<bool>(true)
            );

            var condition = new FolderExistsCondition(folderArgs);
            Assert.That(condition.Validate(), Is.True);
        }

    }
}
