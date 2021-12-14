using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Moq;
using NBi.Core;
using NBi.Core.Decoration.IO;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.IO.Commands;
using NBi.Extensibility;

namespace NBi.Testing.Integration.Core.Decoration.IO.Commands
{
    public class CopyCommandTest
    {
        [SetUp]
        public void CreateDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");

            if (!Directory.Exists(@"Temp\Target"))
                Directory.CreateDirectory(@"Temp\Target");

            if (Directory.Exists(@"Temp\TargetNotExisting"))
                Directory.Delete(@"Temp\TargetNotExisting", true);
        }

        [Test]
        public void Execute_ExistingFile_FileIsCopied()
        {
            var existingFile = @"Temp\Text.txt";
            var targetFile = @"Temp\Target\TextCopy.txt";
            File.WriteAllText(existingFile, "a little text");

            var copyArgs = new IoCopyCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(Path.GetFileName(existingFile))
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(existingFile))
                , new LiteralScalarResolver<string>(Path.GetFileName(targetFile))
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(targetFile))
                , string.Empty
            );

            var command = new CopyCommand(copyArgs);
            command.Execute();

            Assert.That(File.Exists(existingFile), Is.True);
            Assert.That(File.Exists(targetFile), Is.True);
        }

        [Test]
        public void Execute_ExistingFileInNotExistingDirectory_FileIsCopied()
        {
            var existingFile = @"Temp\Text.txt";
            var targetFile = @"Temp\TargetNotExisting\TextCopy.txt";
            File.WriteAllText(existingFile, "a little text");

            var copyArgs = new IoCopyCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(Path.GetFileName(existingFile))
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(existingFile))
                , new LiteralScalarResolver<string>(Path.GetFileName(targetFile))
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(targetFile))
                , string.Empty
            );

            var command = new CopyCommand(copyArgs);
            command.Execute();

            Assert.That(File.Exists(existingFile), Is.True);
            Assert.That(File.Exists(targetFile), Is.True);
        }

        [Test]
        public void Execute_NonExistingFile_ExternalDependencyNotFound()
        {
            var nonExistingFile = @"Temp\nonExistingFile.txt";
            var targetFile = @"Temp\Target\TextCopy.txt";

            var copyArgs = new IoCopyCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(Path.GetFileName(nonExistingFile))
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(nonExistingFile))
                , new LiteralScalarResolver<string>(Path.GetFileName(targetFile))
                , new LiteralScalarResolver<string>(Path.GetDirectoryName(targetFile))
                , string.Empty
            );

            var command = new CopyCommand(copyArgs);
            Assert.Throws<ExternalDependencyNotFoundException>(() => command.Execute());
        }
    }
}
