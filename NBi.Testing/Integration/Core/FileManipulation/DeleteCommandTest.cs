using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBi.Core.FileManipulation;
using Moq;

namespace NBi.Testing.Integration.Core.FileManipulation
{
    public class DeleteCommandTest
    {
        [SetUp]
        public void CreateDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");
        }

        [Test]
        public void Execute_ExistingFile_FileIsDeleted()
        {
            var existingFile = @"Temp\Text.txt";
            File.WriteAllText(existingFile, "a little text");

            var deleteInfo = Mock.Of<IDeleteCommand>
            (
                c => c.FullPath == existingFile
            );


            var command = new DeleteCommand(deleteInfo);
            command.Execute();

            Assert.That(File.Exists(existingFile), Is.False);

        }

        [Test]
        public void Execute_NonExistingFile_NoException()
        {
            var nonExistingFile = @"Temp\nonExistingFile.txt";

            var deleteInfo = Mock.Of<IDeleteCommand>
            (
                c => c.FullPath == nonExistingFile
            );


            var command = new DeleteCommand(deleteInfo);
            command.Execute();
            Assert.Pass();
        }

        [Test]
        public void Execute_NonExistingDirectory_NoException()
        {
            var nonExistingDirectory = @"NonExistingDirectory\File.txt";

            var deleteInfo = Mock.Of<IDeleteCommand>
            (
                c => c.FullPath == nonExistingDirectory
            );


            var command = new DeleteCommand(deleteInfo);
            command.Execute();
            Assert.Pass();
        }
    }
}
