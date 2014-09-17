using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBi.Core.FileManipulation;
using Moq;
using NBi.Core;

namespace NBi.Testing.Integration.Core.FileManipulation
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
        }

        [Test]
        public void Execute_ExistingFile_FileIsCopied()
        {
            var existingFile = @"Temp\Text.txt";
            var targetFile = @"Target\TextCopy.txt";
            File.WriteAllText(existingFile, "a little text");

            var copyInfo = Mock.Of<ICopyCommand>
            (
                c =>c.SourceFullPath == existingFile
                  && c.FullPath == targetFile
            );


            var command = new CopyCommand(copyInfo);
            command.Execute();

            Assert.That(File.Exists(existingFile), Is.True);
            Assert.That(File.Exists(targetFile), Is.True);
        }

        [Test]
        public void Execute_NonExistingFile_ExternalDependencyNotFound()
        {
            var nonExistingFile = @"Temp\nonExistingFile.txt";
            var targetFile = @"Target\TextCopy.txt";

            var copyInfo = Mock.Of<ICopyCommand>
            (
                c => c.SourceFullPath == nonExistingFile
                  && c.FullPath == targetFile
            );


            var command = new CopyCommand(copyInfo);
            Assert.Throws<ExternalDependencyNotFoundException>(() => command.Execute());
        }
    }
}
