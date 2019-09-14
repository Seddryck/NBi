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

            var copyArgs = Mock.Of<ICopyCommandArgs>
            (
                c => c.SourceName == new LiteralScalarResolver<string>(Path.GetFileName(existingFile))
                && c.SourcePath == new LiteralScalarResolver<string>(Path.GetDirectoryName(existingFile))
                && c.DestinationName == new LiteralScalarResolver<string>(Path.GetFileName(targetFile))
                && c.DestinationPath == new LiteralScalarResolver<string>(Path.GetDirectoryName(targetFile))
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

            var copyArgs = Mock.Of<ICopyCommandArgs>
            (
                c => c.SourceName == new LiteralScalarResolver<string>(Path.GetFileName(existingFile))
                && c.SourcePath == new LiteralScalarResolver<string>(Path.GetDirectoryName(existingFile))
                && c.DestinationName == new LiteralScalarResolver<string>(Path.GetFileName(targetFile))
                && c.DestinationPath == new LiteralScalarResolver<string>(Path.GetDirectoryName(targetFile))
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

            var copyArgs = Mock.Of<ICopyCommandArgs>
            (
                c => c.SourceName == new LiteralScalarResolver<string>(Path.GetFileName(nonExistingFile))
                && c.SourcePath == new LiteralScalarResolver<string>(Path.GetDirectoryName(nonExistingFile))
                && c.DestinationName == new LiteralScalarResolver<string>(Path.GetFileName(targetFile))
                && c.DestinationPath == new LiteralScalarResolver<string>(Path.GetDirectoryName(targetFile))
            );

            var command = new CopyCommand(copyArgs);
            Assert.Throws<ExternalDependencyNotFoundException>(() => command.Execute());
        }
    }
}
