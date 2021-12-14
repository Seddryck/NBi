using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Moq;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.IO.Commands;

namespace NBi.Testing.Integration.Core.Decoration.IO.Commands
{
    public class DeleteCommandTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }

        [SetUp]
        public void CreateDirectory()
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);
        }

        [Test]
        public void Execute_ExistingFile_FileIsDeleted()
        {
            var existingFile = @"Temp\Text.txt";
            File.WriteAllText(existingFile, "a little text");

            var deleteArgs = new IoDeleteCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(Path.GetFileName(existingFile))
                , string.Empty
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(existingFile))
            );

            var command = new DeleteCommand(deleteArgs);
            command.Execute();

            Assert.That(File.Exists(existingFile), Is.False);
        }

        [Test]
        public void Execute_NonExistingFile_NoException()
        {
            var nonExistingFile = @"Temp\nonExistingFile.txt";

            var deleteArgs = new IoDeleteCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(Path.GetFileName(nonExistingFile))
                , string.Empty
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(nonExistingFile))
            );

            var command = new DeleteCommand(deleteArgs);
            command.Execute();
            Assert.Pass();
        }

        [Test]
        public void Execute_NonExistingDirectory_NoException()
        {
            var nonExistingDirectory = @"NonExistingDirectory\File.txt";

            var deleteArgs = new IoDeleteCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(Path.GetFileName(nonExistingDirectory))
                , string.Empty
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(nonExistingDirectory))
            );

            var command = new DeleteCommand(deleteArgs);
            command.Execute();
            Assert.Pass();
        }
    }
}
