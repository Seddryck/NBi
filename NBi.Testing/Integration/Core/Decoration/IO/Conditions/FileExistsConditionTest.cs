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
    public class FileExistsConditionTest
    {
        [SetUp]
        public void CreateDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");

            if(!Directory.Exists(@"Temp\Target"))
                Directory.CreateDirectory(@"Temp\Target");

            File.WriteAllText(@"Temp\Target\myFile.txt", "a little text");
            File.WriteAllText(@"Temp\Target\myEmptyFile.txt", string.Empty);
        }

        [Test]
        public void Execute_ExistingFile_True()
        {
            var fileArgs = new FileExistsConditionArgs
            (
                @"Temp\",
                new LiteralScalarResolver<string>(@"Target\"),
                new LiteralScalarResolver<string>(@"myFile.txt"),
                new LiteralScalarResolver<bool>(false)
            );

            var condition = new FileExistsCondition(fileArgs);
            Assert.That(condition.Validate(), Is.True);
        }

        [Test]
        public void Execute_ExistingFileButEmpty_True()
        {
            var fileArgs = new FileExistsConditionArgs
            (
                @"Temp\",
                new LiteralScalarResolver<string>(@"Target"),
                new LiteralScalarResolver<string>(@"myEmptyFile.txt"),
                new LiteralScalarResolver<bool>(true)
            );

            var condition = new FileExistsCondition(fileArgs);
            Assert.That(condition.Validate(), Is.False);
        }

        [Test]
        public void Execute_ExistingFileButNotEmpty_True()
        {
            
            var fileArgs = new FileExistsConditionArgs
            (
                @"Temp\",
                new LiteralScalarResolver<string>(@"Target"),
                new LiteralScalarResolver<string>(@"myFile.txt"),
                new LiteralScalarResolver<bool>(true)
            );

            var condition = new FileExistsCondition(fileArgs);
            Assert.That(condition.Validate(), Is.True);
        }

    }
}
